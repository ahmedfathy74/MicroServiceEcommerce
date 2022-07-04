using CatalogApi.Data.interfaces;
using CatalogApi.Entities;
using CatalogApi.Setting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products  { get; set; }

        public CatalogContext(ICatalogDatabaseSettings settings)
        {
           var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Products = database.GetCollection<Product>(settings.CollectionName);

            CatalogContextSeed.SeedData(Products);

        }
    }
}
