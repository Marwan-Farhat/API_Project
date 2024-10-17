using Demo.Core.Domain.Contracts.Infrastructure;
using Demo.Core.Domain.Entities.Basket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Basket_Repository
{
    internal class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();  // GetDatabase: Give Me A Connection to redis Database
        }

        public async Task<CustomerBasket?> GetAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> UpdateAsync(CustomerBasket basket, TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(basket);

            var update = await _database.StringSetAsync(basket.Id,value,timeToLive);

            if (update)
                return basket;
            return null;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var deleted = await _database.KeyDeleteAsync(id);
            return deleted;
        }
    }
}
