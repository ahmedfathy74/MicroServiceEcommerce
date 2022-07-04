using CatalogApi.Data.interfaces;
using CatalogApi.Entities;
using CatalogApi.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly ICatalogContext context;

        public ProductRepository(ICatalogContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await context.Products.Find(p => true).ToListAsync();
        }


        public async Task<Product> GetProduct(string id)
        {
            return await context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(prop => prop.Name, name);
            return await context.Products.Find(filter).ToListAsync();
        }



        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(prop => prop.Category, category);
            return await context.Products.Find(filter).ToListAsync();
        }

        public async Task Create(Product product)
        {
             await context.Products.InsertOneAsync(product);  
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(prop => prop.Id, id);

            var deleteResult = await context.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;

        }







        public async Task<bool> Update(Product product)
        {
            var updateResult = await context.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.MatchedCount > 0;
        }

     
    }
}
