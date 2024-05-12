using MektepTagamAPI.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace MektepTagamAPI.Models
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }
        [ForeignKey("OrganizationId")]
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<Transaction> Transactions { get; set; }
        public Dish()
        {
            Transactions = new List<Transaction>();
        }
    }
}
