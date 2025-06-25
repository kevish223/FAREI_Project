using FAREI_Project.Models;
using FormRequest.Models;

namespace FAREI_Project.ViewModel
{
    public class RequestsViewModel
    {
        public List<FAREI_Project.Models.Request>? Requests { get; set; }
        public List<FormReqDb>? FormReqDb { get; set; } = new();
        public List<ApplicationUser>? AllUsers { get; set;}
        public FormReqDb? FormReqDbs { get; set; }
        public Registry? Registry{ get; set; }
    }
}
