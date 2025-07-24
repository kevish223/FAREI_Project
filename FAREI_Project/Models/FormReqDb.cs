using FAREI_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormRequest.Models
{
    public class FormReqDb
    {
        [Key]
        public int Id { get; set; }

        // General Form Details

        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }

        [Required]
        public string Site { get; set; }

        [Required]
        public string Department { get; set; }

        [Required(ErrorMessage = "contact phone is required")]
        public string ResponsibleOfficer { get; set; }

        [Required(ErrorMessage = "contact phone is required")]
        public int ContactPhone { get; set; }

        // Equipment Section (1 entry for now — add related tables later if needed)
        public int Pointer { get; set; }

        public string ProblemDescription { get; set; }

        public string SerialNumber { get; set; }

        public string Supervisor { get; set; }

        public String? status { get; set; }

        public String? remarks { get; set; }

        public String? Feedback { get; set; }

        public List<Registry>? Registries { get; set; } = new List<Registry>();
        [ForeignKey("EquipmentID")]
        public EquipmentInventory? Equipments { get; set; } 
        public ITTreport? ITTReports { get; set; } 
        public static implicit operator List<object>(FormReqDb? v)
        {
            throw new NotImplementedException();
        }
    }
}
