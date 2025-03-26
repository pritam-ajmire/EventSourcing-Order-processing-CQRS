CREATE TABLE Events (
    EventID SERIAL PRIMARY KEY,
    OrderID INT NOT NULL,
    EventType TEXT NOT NULL,
    Data JSONB NOT NULL,
    Timestamp TIMESTAMP NOT NULL
);

CREATE INDEX idx_orderid ON Events (OrderID);
CREATE INDEX idx_eventtype ON Events (EventType);