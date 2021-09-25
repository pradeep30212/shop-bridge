using Microsoft.EntityFrameworkCore;
using ShopBridge.Entities;
using ShopBridge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.Repository
{
    public class InventoryRepository : IRepository<Inventory>
    {
        private readonly ShopBridgeDbContext _shopBridgeDbContext;

        public InventoryRepository(ShopBridgeDbContext shopBridgeDbContext)
        {
            _shopBridgeDbContext = shopBridgeDbContext;
        }

        public async Task<Inventory> Add(Inventory entity)
        {
            var result = await _shopBridgeDbContext.AddAsync(entity);
            await _shopBridgeDbContext.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int Id)
        {
            var result = await _shopBridgeDbContext.Inventories.FirstOrDefaultAsync(x => x.InventoryId == Id);
            
            if (result != null)
            {
                _shopBridgeDbContext.Inventories.Remove(result);
                await _shopBridgeDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Inventory>> Get()
        {
            return await _shopBridgeDbContext.Inventories.ToListAsync();
        }

        public async Task<Inventory> Get(int id)
        {
            var result = await _shopBridgeDbContext.Inventories.FirstOrDefaultAsync(x => x.InventoryId == id);
            
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<Inventory> Update(Inventory entity)
        {
            var result = await _shopBridgeDbContext.Inventories.FirstOrDefaultAsync(x => x.InventoryId == entity.InventoryId);
           
            if (result != null)
            {
                result.Name = entity.Name;
                result.Description = entity.Description;
                result.Price = entity.Price;
                
                await _shopBridgeDbContext.SaveChangesAsync();
                
                return result;
            }

            return null;
        }
    }
}
