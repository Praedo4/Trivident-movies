using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace trivident_movies.App_Start
{
    public class MongoContext
    {
        MongoClient _client;
        MongoServer _server;
        public MongoDatabase _database;
        public MongoContext()
        {
            // Get credentials from Web.config file
            string MongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];
            //string MongoUsername = ConfigurationManager.AppSettings["MongoUsername"];
            //string MongoPassword = ConfigurationManager.AppSettings["MongoPassword"];
            //Int32 MongoPort = Convert.ToInt32(ConfigurationManager.AppSettings["MongoPort"]);
            //string MongoHost = ConfigurationManager.AppSettings["MongoHost"];
            //
            //// Creating credentials
            //var credential = MongoCredential.CreateMongoCRCredential(MongoDatabaseName, MongoUsername, MongoPassword);
            //
            //// Creating MongoClientSettings
            //var settings = new MongoClientSettings
            //{
            //    Credential = credential,
            //    Server = new MongoServerAddress(MongoHost,MongoPort)
            //}; 
            //_client = new MongoClient(settings);
            //_server = _client.GetServer();
            var con = new MongoUrl("mongodb+srv://service:rSSCDZTIZzwPRsnEHGvY@cluster0-iv5oy.mongodb.net/movie?retryWrites=true");
            _client = new MongoClient(MongoClientSettings.FromUrl(con));
            _server = _client.GetServer();
            _database = _server.GetDatabase(MongoDatabaseName);
        }
    }
}