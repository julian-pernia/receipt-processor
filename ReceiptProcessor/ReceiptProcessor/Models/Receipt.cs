using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceiptProcessor.Models
{
    public class Receipt
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Retailer { get; set; } = string.Empty;

        [Required]
        public DateOnly PurchaseDate { get; set; }

        [Required]
        public TimeOnly PurchaseTime { get; set; }

        [Required]
        public List<Item> Items { get; set; } = [];

        [Required]
        public string Total { get; set; } = "0.00";
    }
}
