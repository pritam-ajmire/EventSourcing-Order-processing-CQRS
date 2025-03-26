CREATE VIEW OrderSummary AS
SELECT 
    e.OrderID,
    (SELECT Data->>'UserID' FROM Events WHERE EventType = 'OrderCreated' AND OrderID = e.OrderID) AS UserID,
    (SELECT Data->>'TotalAmount' FROM Events WHERE EventType = 'OrderCreated' AND OrderID = e.OrderID) AS TotalAmount,
    CASE 
        WHEN EXISTS (SELECT 1 FROM Events WHERE EventType = 'ShipmentDelivered' AND OrderID = e.OrderID) 
        THEN 'Delivered'
        WHEN EXISTS (SELECT 1 FROM Events WHERE EventType = 'ShipmentCreated' AND OrderID = e.OrderID) 
        THEN 'Shipped'
        WHEN EXISTS (SELECT 1 FROM Events WHERE EventType = 'OrderConfirmed' AND OrderID = e.OrderID) 
        THEN 'Confirmed'
        WHEN EXISTS (SELECT 1 FROM Events WHERE EventType = 'PaymentSuccess' AND OrderID = e.OrderID) 
        THEN 'Paid'
        ELSE 'Created'
    END AS OrderStatus,
    (SELECT MAX(Timestamp) FROM Events WHERE OrderID = e.OrderID) AS LastUpdated
FROM Events e
GROUP BY e.OrderID;

-- select * from  OrderSummary;


CREATE VIEW PaymentSummary AS
SELECT 
    e.OrderID,
    (SELECT Data->>'Amount' FROM Events WHERE EventType = 'PaymentInitiated' AND OrderID = e.OrderID) AS Amount,
    (SELECT Data->>'Method' FROM Events WHERE EventType = 'PaymentInitiated' AND OrderID = e.OrderID) AS PaymentMethod,
    (SELECT Data->>'TransactionID' FROM Events WHERE EventType = 'PaymentSuccess' AND OrderID = e.OrderID) AS TransactionID,
    CASE 
        WHEN EXISTS (SELECT 1 FROM Events WHERE EventType = 'PaymentSuccess' AND OrderID = e.OrderID) 
        THEN 'Success'
        ELSE 'Pending'
    END AS PaymentStatus,
    (SELECT MAX(Timestamp) FROM Events WHERE OrderID = e.OrderID AND EventType LIKE 'Payment%') AS LastUpdated
FROM Events e
GROUP BY e.OrderID;


-- select * from  PaymentSummary;


CREATE VIEW ShipmentSummary AS
SELECT 
    e.OrderID,
    (SELECT Data->>'Carrier' FROM Events WHERE EventType = 'ShipmentCreated' AND OrderID = e.OrderID) AS Carrier,
    (SELECT Data->>'TrackingID' FROM Events WHERE EventType = 'ShipmentCreated' AND OrderID = e.OrderID) AS TrackingID,
    CASE 
        WHEN EXISTS (SELECT 1 FROM Events WHERE EventType = 'ShipmentDelivered' AND OrderID = e.OrderID) 
        THEN 'Delivered'
        WHEN EXISTS (SELECT 1 FROM Events WHERE EventType = 'ShipmentCreated' AND OrderID = e.OrderID) 
        THEN 'In Transit'
        ELSE 'Not Shipped'
    END AS ShipmentStatus,
    (SELECT MAX(Timestamp) FROM Events WHERE OrderID = e.OrderID AND EventType LIKE 'Shipment%') AS LastUpdated
FROM Events e
GROUP BY e.OrderID;


select * from  ShipmentSummary;




-- REHYDRATION - This Query reconstructs the latest state of each order:
WITH LatestEvents AS (
    SELECT 
        e.OrderID,
        e.EventType,
        e.Data,
        e.Timestamp,
        ROW_NUMBER() OVER (PARTITION BY e.OrderID, e.EventType ORDER BY e.Timestamp DESC) AS rn
    FROM events e
)
SELECT 
    o.OrderID,
    MAX(o.Data->>'UserID') AS UserID,
    MAX(o.Data->>'TotalAmount') AS TotalAmount,
    MAX(p.Data->>'Method') AS PaymentMethod,
    MAX(ps.Data->>'TransactionID') AS TransactionID,
    MAX(pf.Data->>'Reason') AS PaymentFailureReason,
    CASE 
        WHEN MAX(sd.OrderID) IS NOT NULL THEN 'Delivered'
        WHEN MAX(sc.OrderID) IS NOT NULL THEN 'Shipped'
        WHEN MAX(oc.OrderID) IS NOT NULL THEN 'Confirmed'
        WHEN MAX(ps.OrderID) IS NOT NULL THEN 'Payment Successful'
        WHEN MAX(pf.OrderID) IS NOT NULL THEN 'Payment Failed'
        ELSE 'Order Created'
    END AS OrderStatus,
    MAX(sc.Data->>'Carrier') AS ShippingCarrier,
    MAX(sc.Data->>'TrackingID') AS TrackingID,
    MAX(sd.Timestamp) AS DeliveredAt
FROM LatestEvents o
LEFT JOIN LatestEvents p  ON o.OrderID = p.OrderID AND p.EventType = 'PaymentInitiated'  AND p.rn = 1
LEFT JOIN LatestEvents ps ON o.OrderID = ps.OrderID AND ps.EventType = 'PaymentSuccess'  AND ps.rn = 1
LEFT JOIN LatestEvents pf ON o.OrderID = pf.OrderID AND pf.EventType = 'PaymentFailed'  AND pf.rn = 1
LEFT JOIN LatestEvents oc ON o.OrderID = oc.OrderID AND oc.EventType = 'OrderConfirmed' AND oc.rn = 1
LEFT JOIN LatestEvents sc ON o.OrderID = sc.OrderID AND sc.EventType = 'ShipmentCreated' AND sc.rn = 1
LEFT JOIN LatestEvents sd ON o.OrderID = sd.OrderID AND sd.EventType = 'ShipmentDelivered' AND sd.rn = 1
WHERE o.EventType = 'OrderCreated' AND o.rn = 1
GROUP BY o.OrderID;



-- materialized view (stores the data in separate table), which needs periodic refresh to update the data.
CREATE MATERIALIZED VIEW OrderSummary AS
SELECT OrderID, EventType, Data, Timestamp
FROM events;
