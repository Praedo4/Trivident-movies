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

            var con = new MongoUrl(ConfigurationManager.AppSettings["MongoConnectionURL"]);
            _client = new MongoClient(MongoClientSettings.FromUrl(con));
            _server = _client.GetServer();
            _database = _server.GetDatabase(MongoDatabaseName);
        }
    }
}