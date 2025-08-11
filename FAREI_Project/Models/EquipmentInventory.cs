using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace FAREI_Project.Models
{
    public class EquipmentInventory
    {
        public int ID { get; set; }
        public String EquipmentName { get; set; }
        public String SerialNumber { get; set; }
        public String EquipmentType { get; set; }
        public String Site { get; set; }
        public String Department { get; set; }
        public String Drives{ get; set; }
        public String capacity { get; set; }
        public String memory { get; set; }
        public String operatingSystem { get; set; }
        public String? OSkey { get; set; }
        public String Office { get; set; }
        public String supplier { get; set; }
        public String User { get; set; }
        public String? barcode { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfPurchase { get; set; }
        public int? amount { get; set; }
        public int? warranty {  get; set; }
        [DataType(DataType.Date)]
        public DateTime WarrantyExpire { get; set; }


    }
}
