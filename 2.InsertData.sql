INSERT INTO Events (OrderID, EventType, Data, Timestamp) VALUES
(101, 'OrderCreated', '{"UserID":5001, "TotalAmount":100.00}', '2025-02-08 08:00:00'),
(101, 'PaymentInitiated', '{"Amount":100.00, "Method":"CreditCard"}', '2025-02-08 09:00:00'),
(101, 'PaymentSuccess', '{"TransactionID":"TXN98765"}', '2025-02-08 09:30:00'),
(101, 'OrderConfirmed', '{}', '2025-02-08 09:35:00'),
(101, 'ShipmentCreated', '{"Carrier":"FedEx", "TrackingID":"TRK1234567"}', '2025-02-08 10:00:00'),
(101, 'ShipmentDelivered', '{}', '2025-02-08 12:00:00');

-- select * from events

-- 1️⃣ Order Created, Waiting for Payment
INSERT INTO events (OrderID, EventType, Data, Timestamp) VALUES
(201, 'OrderCreated', '{"UserID":1001, "TotalAmount":250.00}', '2025-02-10 10:00:00');

-- 2️⃣ Order Created, Payment Failed
INSERT INTO events (OrderID, EventType, Data, Timestamp) VALUES
(202, 'OrderCreated', '{"UserID":1002, "TotalAmount":300.00}', '2025-02-10 10:05:00'),
(202, 'PaymentInitiated', '{"Amount":300.00, "Method":"CreditCard"}', '2025-02-10 10:10:00'),
(202, 'PaymentFailed', '{"Reason":"Insufficient Funds"}', '2025-02-10 10:12:00');

-- 3️⃣ Order Created, Paid, and Waiting for Shipment
INSERT INTO events (OrderID, EventType, Data, Timestamp) VALUES
(203, 'OrderCreated', '{"UserID":1003, "TotalAmount":150.00}', '2025-02-10 10:20:00'),
(203, 'PaymentInitiated', '{"Amount":150.00, "Method":"PayPal"}', '2025-02-10 10:25:00'),
(203, 'PaymentSuccess', '{"TransactionID":"TXN12345"}', '2025-02-10 10:27:00');

-- 4️⃣ Order Created, Paid, Shipped
INSERT INTO events (OrderID, EventType, Data, Timestamp) VALUES
(204, 'OrderCreated', '{"UserID":1004, "TotalAmount":400.00}', '2025-02-10 10:30:00'),
(204, 'PaymentInitiated', '{"Amount":400.00, "Method":"CreditCard"}', '2025-02-10 10:35:00'),
(204, 'PaymentSuccess', '{"TransactionID":"TXN67890"}', '2025-02-10 10:40:00'),
(204, 'OrderConfirmed', '{}', '2025-02-10 10:45:00'),
(204, 'ShipmentCreated', '{"Carrier":"DHL", "TrackingID":"DHL987654"}', '2025-02-10 11:00:00');

-- 5️⃣ Order Created, Paid, Shipped, Delivered
INSERT INTO events (OrderID, EventType, Data, Timestamp) VALUES
(205, 'OrderCreated', '{"UserID":1005, "TotalAmount":500.00}', '2025-02-10 10:50:00'),
(205, 'PaymentInitiated', '{"Amount":500.00, "Method":"UPI"}', '2025-02-10 10:55:00'),
(205, 'PaymentSuccess', '{"TransactionID":"TXN98765"}', '2025-02-10 11:00:00'),
(205, 'OrderConfirmed', '{}', '2025-02-10 11:05:00'),
(205, 'ShipmentCreated', '{"Carrier":"FedEx", "TrackingID":"FEDX12345"}', '2025-02-10 12:00:00'),
(205, 'ShipmentDelivered', '{}', '2025-02-10 14:00:00');