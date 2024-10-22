using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProyectoMongo
{
    public class Dato
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        public string LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("moviegenres")]
        public string MovieGenres { get; set; }

        [BsonElement("movietitle")]
        public string MovieTitle { get; set; }

        [BsonElement("date")]
        public DateOnly Date { get; set; }

        [BsonElement("time")]
        public TimeOnly Time { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("seat")]
        public int Seat { get; set; }

        [BsonElement("cinemaroom")]
        public int CinemaRoom { get; set; }
    }
}