﻿using FAREI_Project.Models;
using FormRequest.Models;

namespace FAREI_Project.ViewModel
{
    public class RequestsViewModel
    {
        public List<FAREI_Project.Models.Request>? Requests { get; set; }
        public List<FormReqDb>? FormReqDb { get; set; }
        public List<ApplicationUser>? AllUsers { get; set;}
        public List<Registry>? Registries { get; set; }
        public List<EquipmentInventory>? Inventories { get; set; }
        public List<ITTreport>? ITTreports { get; set; }
        public FormReqDb? FormReqDbs { get; set; }
        public Registry? Registry{ get; set; }
        public EquipmentInventory? Inventory { get; set; }
        public Third_Party? Third_Party { get; set; }
        public ITTreport? ITTreport { get; set; }

       
    }
}
