using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoMongo
{
    public class DatoJson
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("movie_genres")]
        public string MovieGenres { get; set; }

        [JsonPropertyName("movie_title")]
        public string MovieTitle { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("seat")]
        public int Seat { get; set; }

        [JsonPropertyName("cinema_room")]
        public int CinemaRoom { get; set; }
    }
}
