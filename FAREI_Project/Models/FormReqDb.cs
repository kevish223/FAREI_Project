using FAREI_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormRequest.Models
{
    public class FormReqDb
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }
        [Required]
        public string Site { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string ResponsibleOfficer { get; set; }
        [Required]
        public int ContactPhone { get; set; }
        public int Pointer { get; set; }
        [Required]
        public string ProblemDescription { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        public string? Supervisor { get; set; }
        public String? status { get; set; }
        public String? remarks { get; set; }
        public String? Feedback { get; set; }
        [ForeignKey("EquipmentID")]
        public EquipmentInventory? Equipments { get; set; } 
        public ITTreport? ITTReports { get; set; } 
        public FormReqDb() { }
    }
}
