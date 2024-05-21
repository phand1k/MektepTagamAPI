using MektepTagamAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAvtoMigNew.Models
{
    public class ModelCar
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Model car name is required field!")]
        public string? Name { get; set; }
        [ForeignKey("CarId")]
        public int? CarId { get; set; }
        public Car? Car { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
