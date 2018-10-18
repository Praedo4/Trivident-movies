using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace trivident_movies.Models
{
    public class ErrorMessages
    {
        public const string Title = "Please enter movie title here";
        public const string Director = "Please enter movie director's name here";
        public const string Actors = "Please enter names of actors here, separated with comas";
        public const string ImageUrl = "This field must contain a valid link to cover image of the movie";
        public const string Year = "Year of release must be a number between 1900 and 2020";
    }
    public class MovieModel
    {
        [Key][BsonId]
        public ObjectId Id { get; set; } // Unique ID

        [BsonElement("title")]
        [Required(ErrorMessage = ErrorMessages.Title )]
        public string Title { get; set; } // Movie title

        [BsonElement("director")]
        [Required(ErrorMessage = ErrorMessages.Director)]
        public string Director { get; set; } // Movie director

        [BsonElement("actors")]
        [Required(ErrorMessage = ErrorMessages.Actors)]
        public string Actors { get; set; } // List of movie actors

        [BsonElement("image")]
        [DataType(DataType.ImageUrl)][Url(ErrorMessage = ErrorMessages.ImageUrl)]
        [Required(ErrorMessage = ErrorMessages.ImageUrl)]
        public string ImageLink { get; set; } // Link to the movie image

        [BsonElement("year")][Range(1900,2020, ErrorMessage = ErrorMessages.Year)]
        [Required(ErrorMessage = ErrorMessages.Year)]
        public int Year { get; set; } // Year
    }
}