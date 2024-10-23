using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProyectoMongo
{
    public class Dato
    {
        private readonly IMongoCollection<Dato> _datos;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("first_name")]
        public string FirstName { get; set; }

        [BsonElement("last_name")]
        public string LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("movie_genres")]
        public string MovieGenres { get; set; }

        [BsonElement("movie_title")]
        public string MovieTitle { get; set; }

        [BsonElement("date")]
        public DateOnly Date { get; set; }

        [BsonElement("time")]
        public TimeOnly Time { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("seat")]
        public int Seat { get; set; }

        [BsonElement("cinema_room")]
        public int CinemaRoom { get; set; }

        public Dato()
        {
            string connectionString = "mongodb://localhost:27017";

            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("MongoToSql");
            _datos = database.GetCollection<Dato>("datos");
        }

        public void Agregar(List<Dato> datos)
        {
            try
            {
                _datos.InsertMany(datos);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}