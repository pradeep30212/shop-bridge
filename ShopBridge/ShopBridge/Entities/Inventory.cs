using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Entities
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
