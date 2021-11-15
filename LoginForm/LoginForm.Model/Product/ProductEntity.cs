using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginForm.Model.Product
{
    public class ProductEntity : BaseEntity
    {
        [Required]
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(16,2)")]
        [Required]
        public decimal Price { get; set; }

    }
}
