using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace trivident_movies.Models
{
    public class MovieModel
    {
        [Key][BsonId]
        public ObjectId Id { get; set; } // Unique ID

        [BsonElement("title")]
        public string Title { get; set; } // Movie title

        [BsonElement("director")]
        public string Director { get; set; } // Movie director

        [BsonElement("actors")]
        public string Actors { get; set; } // List of movie actors

        [BsonElement("image")][DataType(DataType.ImageUrl)]
        public string ImageLink { get; set; } // Link to the movie image

        [BsonElement("year")][Range(1900,2020)]
        public int Year { get; set; } // Year
    }
}