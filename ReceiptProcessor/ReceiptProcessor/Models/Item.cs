using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReceiptProcessor.Models
{
    public class Item
    {
        [Key]
        [JsonIgnore]
        public long? Id { get; set; }

        [JsonIgnore]
        public Receipt? Receipt { get; set; }
        [JsonIgnore]
        public Guid? ReceiptId { get; set; }

        [Required]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string Price { get; set; } = string.Empty;
    }
}
