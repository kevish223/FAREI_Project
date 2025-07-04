using FormRequest.Models;
using System.ComponentModel.DataAnnotations;

namespace FAREI_Project.Models
{
    public class Third_Party
    {
        public int ID { get; set; }
        public String? companyName { get; set; }
        public String? CompanyNumber { get; set; }
        public String? serialNumber { get; set; }
        public String? Remarks { get; set; }
        public int? FormReqDbID { get; set; }
        public FormReqDb? FormReqDb { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RequestDate { get; set; }
    }
}
