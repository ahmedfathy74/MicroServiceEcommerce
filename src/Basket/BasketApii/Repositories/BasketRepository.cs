﻿using BasketApii.Entities;
using BasketApii.Repositories.interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BasketApii.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);

        }

        //public async Task<bool> DeleteBaskett(string userName)
        //{
        //    // await _redisCache.RemoveAsync(userName);
        //    return await _redisCache
        //                  .RemoveAsync(userName);
        //    //    .KeyDeleteAsync(userName);
        //}

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket=await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket)) return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);


        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }
    }
}
