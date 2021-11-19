using Catalog.API.Entites;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
        public IConfiguration Configuration { get; }

        public CatalogContext(IConfiguration configuration)
        {
            Configuration = configuration;

            var client = new MongoClient(Configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(Configuration["DatabaseSettings:DatabaseName"]);
            Products = database.GetCollection<Product>(Configuration["DatabaseSettings:CollectionName"]);
            CatalogContextSeed.SeedData(Products);
        }

    }
}
