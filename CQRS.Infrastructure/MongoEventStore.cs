//using CQRS.Domain;

//namespace CQRS.Infrastructure
//{
//    public class MongoEventStore : IEventStore
//    {
//        private readonly IMongoCollection<BsonDocument> _collection;

//        public MongoEventStore(IMongoDatabase database)
//        {
//            _collection = database.GetCollection<BsonDocument>("event_store");
//        }

//        public void SaveEvents(Guid aggregateId, List<IEvent> events)
//        {
//            var documents = events.Select(e => new BsonDocument
//        {
//            { "AggregateId", e.AggregateId.ToString() },
//            { "Type", e.GetType().Name },
//            { "Data", JsonConvert.SerializeObject(e) },
//            { "Timestamp", e.Timestamp }
//        });

//            _collection.InsertMany(documents);
//        }

//        public List<IEvent> GetEvents(Guid aggregateId)
//        {
//            var filter = Builders<BsonDocument>.Filter.Eq("AggregateId", aggregateId.ToString());
//            var documents = _collection.Find(filter).ToList();
//            return documents.Select(d => JsonConvert.DeserializeObject<IEvent>(d["Data"].AsString)).ToList();
//        }
//    }
//}
