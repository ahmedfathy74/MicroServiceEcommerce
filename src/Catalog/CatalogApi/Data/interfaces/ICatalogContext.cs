using CatalogApi.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Data.interfaces
{
   public interface ICatalogContext
{
        IMongoCollection<Product> Products { get; }

    }
}
