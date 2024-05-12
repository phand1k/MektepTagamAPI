using MektepTagamAPI.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace MektepTagamAPI.Models
{
    public class CardCode
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? DateOfCreated { get; set; } = DateTime.Now;
        [ForeignKey("OrganizationId")]
        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        [ForeignKey("AspNetUserId")]
        public string? AspNetUserId { get; set; }
        public AspNetUser? AspNetUser { get; set; }
        public CardCode()
        {

        }
    }
}
