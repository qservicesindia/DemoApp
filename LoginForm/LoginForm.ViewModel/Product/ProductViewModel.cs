using System.ComponentModel.DataAnnotations;

namespace LoginForm.ViewModel.Product
{
    public class ProductViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
