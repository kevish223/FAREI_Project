using FormRequest.Models;
using System.ComponentModel.DataAnnotations;

namespace FAREI_Project.Models
{
    public class Registry
    {

        public int RegistryId { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public String? Driver { get; set; }
        [DataType(DataType.Date)]
        public DateTime? MovementDate { get; set; }
        public string? Remarks { get; set; }
        public bool IsValid { get; set; }
        public int? FormReqDbId { get; set; }
        public EquipmentInventory? Equipment{ get; set; }

    }
}
