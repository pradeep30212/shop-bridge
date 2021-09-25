using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Entities;
using ShopBridge.Repository;
using System;
using System.Threading.Tasks;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IRepository<Inventory> _inventoryRepository;

        public InventoriesController(IRepository<Inventory> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetInventories()
        {
            try
            {
                return Ok(await _inventoryRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error, please contact to system administrator");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            try
            {
                var result = await _inventoryRepository.Get(id);
                
                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error, please contact to system administrator");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Inventory>> CreateInventory(Inventory inventory)
        {
            try
            {
                if (inventory == null)
                {
                    return BadRequest();
                }

                var result = await _inventoryRepository.Add(inventory);

                return CreatedAtAction(nameof(GetInventory), new { id = result.InventoryId }, result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new inventory");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Inventory>> UpdateInventory(int id, Inventory inventory)
        {
            try
            {
                if (id != inventory.InventoryId)
                {
                    return BadRequest($"Inventory id = {id} mismatched.");
                }

                var result = await _inventoryRepository.Get(id);
                
                if (result == null)
                {
                    return NotFound($"Inventory with id = {id} not found.");
                }

                return await _inventoryRepository.Update(inventory);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating inventory");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteInventory(int id)
        {
            try
            {
                var result = await _inventoryRepository.Get(id);

                if (result == null)
                {
                    return NotFound($"Inventory with id = {id} not found.");
                }

                await _inventoryRepository.Delete(id);

                return Ok($"Inventory with id = {id} deleted.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting inventory");
            }
        }
    }
}
