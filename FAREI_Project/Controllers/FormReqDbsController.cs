using Azure.Core;
using FAREI_Project.Data;
using FAREI_Project.Models;
using FAREI_Project.ViewModel;
using FormRequest.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using MigraDoc.Rendering;
using Mono.TextTemplating;
using Newtonsoft.Json.Linq;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Registry = FAREI_Project.Models.Registry;

namespace FAREI_Project.Controllers
{
    public class FormReqDbsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private int atNewRequest = 0;
        private int atAccepted = 1;
        private int atTransitRequest = 2;
        private int atOnsiteRequest = 3;
        private int atTransitting = 4;
        private int atReport = 5;
        private int atPendingRequest = 6;
        private int atStartRepairing = 7;
        private int atFinalRequest = 8;
        private int atReturn = 9;
        DateTime fiveDaysAgo = DateTime.Now.AddDays(-5);


        public FormReqDbsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
           
        }

        
        public async Task<IActionResult> Index()
        {
            
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Include(m=>m.ITTReports).ToListAsync(),
                AllUsers = _userManager.Users.ToList(),
                Notification=await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 20 || m.Pointer == 9) && m.RequestDate >= fiveDaysAgo &&
            m.ResponsibleOfficer == User.Identity.Name).ToListAsync()
            };
            var username = User.Identity.Name;
            if (username==null)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(username);
            var UserModel =await _context.Alluser.FirstOrDefaultAsync(m=>m.UserName==username);
            string? type = user.Type;
            if (type.Equals("Supervisor"))
            {
                return RedirectToAction("SupervisorForm");
            }else if (type.Equals("Registry"))
            {
                return RedirectToAction("RegistryForm");
            }
            else if (type.Equals("Technician"))
            {
                return RedirectToAction("TechnicianForm");
            }
            else if (type.Equals("Admin"))
            {
                return View(model);
            }
            else if (type.Equals("ITO"))
            {
                return RedirectToAction("ITOform");
            }
            return View(model);
        }
        public async Task<IActionResult> MyRequestForm()
        {
            
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j=>j.ResponsibleOfficer.Equals(User.Identity.Name)).ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 20 || m.Pointer == 9) && m.RequestDate >= fiveDaysAgo &&
                m.ResponsibleOfficer == User.Identity.Name).ToListAsync(),
                
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> SupervisorForm()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j => j.Supervisor.Contains(User.Identity.Name) && j.status == "pending").ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 0 || m.Pointer == 4) && m.RequestDate >= fiveDaysAgo &&
                m.Supervisor == User.Identity.Name).ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> ITOform()
        {
            
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m=>m.Equipments).Include(m=>m.ITTReports).ToListAsync(),
                Notification= await _context.FormReqDb.Where(m => (m.Pointer == 2 || m.Pointer == 3 || 
                m.Pointer == 6 || m.Pointer == 8 || m.Pointer == 10) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> Inventory()
        {

            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Include(m => m.ITTReports).ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 2 || m.Pointer == 3 ||
                m.Pointer == 6 || m.Pointer == 8 || m.Pointer == 10) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                Inventories = await _context.Equipment.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> ARegistry()
        {

            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Include(m => m.ITTReports).ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 2 || m.Pointer == 3 ||
                m.Pointer == 6 || m.Pointer == 8 || m.Pointer == 10) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                Registries = await _context.Registries.ToListAsync(),
                Inventories = await _context.Equipment.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> NewComponent()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j => j.status =="New Component").ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> ThirdParty()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j => j.status =="Third party" || j.status== "Sent to third party").ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> RegistryForm()
        {
            var username = User.Identity.Name;
           
            if (username == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByEmailAsync(username);
            var Site = user.Site;
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m=>m.Equipments).Where(j => (j.status.Contains("Transitting") 
                || j.status.Contains("Send back") || j.status.Contains("Return")) && j.Site.Contains(Site) ).ToListAsync(),

                Notification= await _context.FormReqDb.Where(m => (m.Pointer == 4 || m.Pointer == 20 || m.Pointer == 5)            
                && m.RequestDate >= fiveDaysAgo && m.Site == user.Site).ToListAsync(),

                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> TechnicianForm()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 5 || m.Pointer == 10 || m.Pointer == 4 
                || m.Pointer == 7 || m.Pointer == 9 || m.Pointer == 20) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                Registries = await _context.Registries.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> UserForm()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.ToListAsync(),
                Registries = await _context.Registries.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> Movement()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j=>j.status== "Accepted").ToListAsync(),
                Registries = await _context.Registries.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> SAllRequest()
        {
            var name=User.Identity.Name;
            if (name==null)
            {
                return RedirectToAction("TechnicianForm");
            }
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j => j.Supervisor == name).ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 0 || m.Pointer == 4) && m.RequestDate >= fiveDaysAgo &&
                m.Supervisor == User.Identity.Name).ToListAsync(),
                Registries = await _context.Registries.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> MovementConfirmation()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j => j.status == "Accepted").ToListAsync(),
                Registries = await _context.Registries.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> SReportForm()
        {
          
            var name = User.Identity.Name;
            if (name==null) 
            {
                return RedirectToAction("Index");
            }
            var user=await _context.Alluser.FirstOrDefaultAsync(m=>m.UserName==name);
            if (user.Type=="User")
            {
                var User = new RequestsViewModel 
                {
                    FormReqDb = await _context.FormReqDb.Include(m=>m.Equipments).Where(j => j.ResponsibleOfficer==name).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 20 || m.Pointer == 9) && m.RequestDate >= fiveDaysAgo &&
                    m.ResponsibleOfficer == user.UserName).ToListAsync(),
                    User = user,
                    AllUsers = _userManager.Users.ToList()
                };
                return View(User);
            }else if (user.Type == "Supervisor") 
            {
                var Supervisor = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Where(j => j.Supervisor == name || j.ResponsibleOfficer==name).ToListAsync(),
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 0 || m.Pointer == 4) && m.RequestDate >= fiveDaysAgo &&
                    m.Supervisor == User.Identity.Name).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    User = user,
                    AllUsers = _userManager.Users.ToList()
                };
                return View(Supervisor);
            }else if (user.Type== "Technician"  || user.Type == "Admin") 
            {
                var model = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).ToListAsync(),
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 5 || m.Pointer == 10 || m.Pointer == 4
                    || m.Pointer == 7 || m.Pointer == 9 || m.Pointer == 20) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    User = user,
                    AllUsers = _userManager.Users.ToList()
                };
                return View(model);
            }else if (user.Type == "ITO")
            {
                var model = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).ToListAsync(),
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 2 || m.Pointer == 3 ||
                    m.Pointer == 6 || m.Pointer == 8 || m.Pointer == 10) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                    User = user,
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(model);
            }
            else if (user.Type == "Registry")
            {
                var Registry = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Where(m => m.Site == user.Site).ToListAsync(),
                    Notification= await _context.FormReqDb.Where(m => (m.Pointer == 4 || m.Pointer == 20 || m.Pointer == 5)          
                    && m.RequestDate >= fiveDaysAgo && m.Site == user.Site ).ToListAsync(),
                    User=user,
                    Registries = await _context.Registries.Include(m=>m.Equipment).Where(m=>m.From==user.Site || m.To==user.Site).ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(Registry);
            }
            return View();
        }
        public async Task<IActionResult> Transite()
        {
            var username = User.Identity.Name;
            
            if (username == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByEmailAsync(username);
            var Site = user.Site;
            var model = new RequestsViewModel
            {
             
                FormReqDb= await _context.FormReqDb.Include(m => m.Equipments).Where(j => (j.status.Contains("Transitting") || j.status.Contains("Send back") || j.status.Contains("Return")) 
                && j.Site.Contains(Site)).ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 4 || m.Pointer == 20 || m.Pointer == 5)
                && m.RequestDate >= fiveDaysAgo && m.Site == user.Site).ToListAsync(),
                Registries = await _context.Registries.Include(m=>m.Equipment).Where(j=>  j.From==Site || j.To==Site ).ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> Feedback()
        {
         
            var FormDBReq =  await _context.FormReqDb.Where(j => j.status == "Complete").ToListAsync();
            TimeSpan duration;
            for (int requests = 0; requests < FormDBReq.Count; requests++)
            {
                FormReqDb? request = FormDBReq[requests];
                duration =request.RequestDate-DateTime.Now;
                if (duration.Days>10)
                {
                    request.status = "Close";
                    await _context.SaveChangesAsync();
                }
            }


           var newFormDBReq = await _context.FormReqDb.Where(j => j.ResponsibleOfficer == User.Identity.Name).ToListAsync();
            var model = new RequestsViewModel
            {
                FormReqDb = newFormDBReq,
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 20 || m.Pointer == 9) && m.RequestDate >= fiveDaysAgo &&
                m.ResponsibleOfficer == User.Identity.Name).ToListAsync(),
                Registries = await _context.Registries.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> GeneratePdfAsync(DateTime date)
        {
            var Request = await _context.FormReqDb.Where(m => m.RequestDate < date).Include(j => j.Equipments).ToListAsync();
            using (var memoryStream = new MemoryStream())
            {
                var writer = new iText.Kernel.Pdf.PdfWriter(memoryStream);
                var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                document.Add(new iText.Layout.Element.Paragraph("Product List")                    
                    .SetFontSize(16)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                // Create table with 3 columns
                var table = new iText.Layout.Element.Table(6, true);

                // Header row
                table.AddHeaderCell("Request date");
                table.AddHeaderCell("Responsible Officer");
                table.AddHeaderCell("Supervisor");
                table.AddHeaderCell("Equipment Name");
                table.AddHeaderCell("Equipment type");
                table.AddHeaderCell("Serial number");

                foreach (var item in Request) {
                    
                    table.AddCell(item.RequestDate.ToString());
                    table.AddCell(item.ResponsibleOfficer);
                    table.AddCell(item?.Supervisor ?? "");
                    table.AddCell(item.Equipments.EquipmentName);
                    table.AddCell(item.Equipments.EquipmentType);
                    table.AddCell(item.Equipments.SerialNumber);
                }
                // Add table to document
                document.Add(table);
                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", "static-table.pdf");
            }
        } 
        
        
        [HttpPost]
        public async Task<ActionResult> GeneratePdfDetailAsync(int ID)
        {
            var Request = await _context.FormReqDb.Include(m=>m.Equipments).FirstOrDefaultAsync(m=>m.Id==ID);
            using (var memoryStream = new MemoryStream())
            {
                var writer = new iText.Kernel.Pdf.PdfWriter(memoryStream);
                var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                var rootTable = new iText.Layout.Element.Table(2);
                rootTable.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100));

                var leftCell = new iText.Layout.Element.Cell();
                leftCell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);


                leftCell.Add(new Paragraph("FormReqDb").SetFontSize(14).SetMarginBottom(10));
                leftCell.Add(KeyValue("RequestDate", "7/29/2025"));
                leftCell.Add(KeyValue("Site", "Reduit"));
                leftCell.Add(KeyValue("Department", "Technical Support"));
                leftCell.Add(KeyValue("ResponsibleOfficer", "KEVISH1@gmail.com"));
                leftCell.Add(KeyValue("ContactPhone", "52704432"));
                leftCell.Add(KeyValue("ProblemDescription", "burning"));
                leftCell.Add(KeyValue("SerialNumber", "CND23502QR"));

                var rightCell = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                rightCell.Add(new Paragraph("Equipment Details").SetFontSize(14).SetMarginBottom(10));
                rightCell.Add(KeyValue("Serial Number", "CND23502QR"));
                rightCell.Add(KeyValue("Equipment Type", "Laptop"));
                rightCell.Add(KeyValue("Equipment name", "FAREI-CROPEXT"));
                rightCell.Add(KeyValue("Problem Description", "burning"));

                rootTable.AddCell(leftCell);
                rootTable.AddCell(rightCell);

                document.Add(rootTable);
                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", $"{Request.Id}-document.pdf");
            }
        }
        private Paragraph KeyValue(string label, string value)
        {
            var p = new Paragraph();
            p.Add(new Text(label + ": "));
            p.Add(new Text(value ?? ""));
            p.SetMarginBottom(5);
            return p;
        }
        
        // GET: FormReqDbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var formReqDb = await _context.FormReqDb?.Include(m=>m.Equipments).FirstOrDefaultAsync(m => m.Id == id);
            var allUsers = _context.Users.ToList();
            var viewModel = new RequestsViewModel
            {
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 20 || m.Pointer == 9) && m.RequestDate >= fiveDaysAgo &&
                m.ResponsibleOfficer == User.Identity.Name).ToListAsync(),
               
                FormReqDbs = formReqDb,
                AllUsers = allUsers
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> EquipmentDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Equipment = await _context.Equipment.FirstOrDefaultAsync(m => m.ID == id);
            var allUsers = _context.Users.ToList();
        
            if (Equipment == null)
            {
                return NotFound();
            }
            var viewModel = new RequestsViewModel
            {
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 20 || m.Pointer == 9) && m.RequestDate >= fiveDaysAgo &&
                m.ResponsibleOfficer == User.Identity.Name).ToListAsync(),
                Equipment = Equipment,
                AllUsers = allUsers
            };
            return View(viewModel);
        }

        public async Task<IActionResult> ReportDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var formReqDb = await _context.FormReqDb?.Include(m => m.Equipments).FirstOrDefaultAsync(m => m.Id == id);

            var allUsers = _context.Users.ToList();
            if (formReqDb == null)
            {
                return NotFound();
            }
            var name = User.Identity.Name;
            if (name == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _context.Alluser.FirstOrDefaultAsync(m => m.UserName == name);
            if (user.Type == "User")
            {
                var User = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Where(j => j.ResponsibleOfficer == name).ToListAsync(),
                    FormReqDbs = formReqDb,
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 20 || m.Pointer == 9) && m.RequestDate >= fiveDaysAgo &&
                    m.ResponsibleOfficer == user.UserName).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(User);
            }
            else if (user.Type == "Supervisor")
            {
                var Supervisor = new RequestsViewModel
                {
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 0 || m.Pointer == 4) && m.RequestDate >= fiveDaysAgo &&
                    m.Supervisor == User.Identity.Name).ToListAsync(),
                    FormReqDbs = formReqDb,
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(Supervisor);
            }
            else if (user.Type == "Technician" || user.Type == "Admin")
            {
                var model = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).ToListAsync(),
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 5 || m.Pointer == 10 || m.Pointer == 4                
                    || m.Pointer == 7 || m.Pointer == 9 || m.Pointer == 20) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                    FormReqDbs = formReqDb,
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(model);
            }
            else if (user.Type == "ITO")
            {
                var model = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).ToListAsync(),
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 2 || m.Pointer == 3 ||
                    m.Pointer == 6 || m.Pointer == 8 || m.Pointer == 10) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(model);
            }
            else if (user.Type == "Registry")
            {
                var Registry = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Where(m => m.Site == user.Site).ToListAsync(),
                    FormReqDbs = formReqDb,
                    Notification = await _context.FormReqDb.Where(m => (m.Pointer == 4 || m.Pointer == 20 || m.Pointer == 5)
                    && m.RequestDate >= fiveDaysAgo && m.Site == user.Site).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(Registry);
            }
            return View();
        }
        public async Task<IActionResult> TechnicianDetailsForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb?.FirstOrDefaultAsync(m => m.Id == id);
            var equipment= await _context.Equipment.FirstOrDefaultAsync(m=>m.SerialNumber==formReqDb.SerialNumber);
            var allUsers = _context.Users.ToList();
            var viewModel = new RequestsViewModel
            {
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 1 || m.Pointer == 5 || m.Pointer == 10 || m.Pointer == 4
                || m.Pointer == 7 || m.Pointer == 9 || m.Pointer == 20) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                FormReqDb = await _context.FormReqDb.ToListAsync(),
                FormReqDbs = formReqDb,
                AllUsers = allUsers,
                Inventory = equipment   
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> DetailsSupervisorForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb
                .FirstOrDefaultAsync(m => m.Id == id);
            var AllUsers = _userManager.Users.ToList();
            var viewModel = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j => j.Supervisor.Contains(User.Identity.Name) && j.status == "pending").ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 0 || m.Pointer == 4) && m.RequestDate >= fiveDaysAgo &&
                m.Supervisor == User.Identity.Name).ToListAsync(),
                FormReqDbs = formReqDb,
                AllUsers = AllUsers
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> DetailsRegistryForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
          
            var username = User.Identity.Name;
            if (username == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByEmailAsync(username);
            var Site = user.Site;
            var formReqDb = await _context.FormReqDb
                .FirstOrDefaultAsync(m => m.Id == id);
            var AllUsers = _userManager.Users.ToList();
            
            var viewModel = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Where(j => (j.status.Contains("Transitting")
               || j.status.Contains("Send back") || j.status.Contains("Return")) && j.Site.Contains(Site)).ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 4 || m.Pointer == 20 || m.Pointer == 5)
                && m.RequestDate >= fiveDaysAgo && m.Site == user.Site).ToListAsync(),
                FormReqDbs = formReqDb,
                Registry = new Registry(),
                AllUsers = AllUsers
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> DetailsTransiteForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var Registry = await _context.Registries.FindAsync(id);
            var formReqDb = await _context.FormReqDb.Include(m=>m.Equipments)
               .FirstOrDefaultAsync(m => m.Id == Registry.FormReqDbId);
            var username = User.Identity.Name;
            if (username == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByEmailAsync(username);
            var Site = user.Site;

            var AllUsers = _userManager.Users.ToList();

            var viewModel = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 4 || m.Pointer == 20 || m.Pointer == 5)
                && m.RequestDate >= fiveDaysAgo && m.Site == user.Site).ToListAsync(),
                FormReqDbs = formReqDb,
                Registry = Registry,
                AllUsers = AllUsers
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> NewComponentDetailsForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb
               .FirstOrDefaultAsync(m => m.Id == id);
            var Registry = await _context.Registries
               .FirstOrDefaultAsync(m => m.FormReqDbId == id);

            var AllUsers = _userManager.Users.ToList();

            var viewModel = new RequestsViewModel
            {
                FormReqDbs = formReqDb,
                Registry = Registry,
                AllUsers = AllUsers
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> repairedDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
       
            var formReqDb = await _context.FormReqDb.FirstOrDefaultAsync(m => m.Id == id);
            var Registry = await _context.Registries.FirstOrDefaultAsync(m => m.FormReqDbId == id);
            var equipment = await _context.Equipment.FirstOrDefaultAsync(m => m.SerialNumber==formReqDb.SerialNumber);
            var ITTReport =await _context.ITTreport.FirstOrDefaultAsync(m=>m.FormReqDb==id);
            var AllUsers = _userManager.Users.ToList();

            var viewModel = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.ToListAsync(),
                Notification = await _context.FormReqDb.Where(m => (m.Pointer == 2 || m.Pointer == 3 ||
                m.Pointer == 6 || m.Pointer == 8 || m.Pointer == 10) && m.RequestDate >= fiveDaysAgo).ToListAsync(),
                FormReqDbs = formReqDb,
                Registry = Registry,
                AllUsers = AllUsers,
                Inventory= equipment,
                ITTreport=ITTReport
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> ThirdPartyForm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb
               .FirstOrDefaultAsync(m => m.Id == id);
            var Registry = await _context.Registries
               .FirstOrDefaultAsync(m => m.FormReqDbId == id);

            var AllUsers = _userManager.Users.ToList();

            var viewModel = new RequestsViewModel
            {
                FormReqDbs = formReqDb,
                Registry = Registry,
                AllUsers = AllUsers
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> MovementDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb?
                .FirstOrDefaultAsync(m => m.Id == id);
            var allUsers = _context.Users.ToList();
            var viewModel = new RequestsViewModel
            {
                FormReqDbs = formReqDb,
                AllUsers = allUsers
            };
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: FormReqDbs/Create
        public async Task<IActionResult> Create()
        {
            var name = User.Identity.Name;
            var users = _context.Users.ToList();
            if (name == null)
            {
                return RedirectToAction("Index");
            }
            var FindUser = await _context.Alluser.FirstOrDefaultAsync(m=>m.UserName==name);

            var viewModel = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.ToListAsync(),
                FormReqDbs = new FormReqDb(),
                AllUsers = users,
                User = FindUser

            };
            return View("Create",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RequestsViewModel model)
        {

            if (ModelState.IsValid)
            {
                var name = User.Identity.Name;
                if (name == null)
                {
                    return RedirectToAction("Index");
                }
                var user = await _context.Alluser.FirstOrDefaultAsync(m => m.UserName == name);
                if (user.Type=="User") 
                {
                    var newForm = model.FormReqDbs;
                    var equipment = await _context.Equipment?.FirstOrDefaultAsync(m => m.SerialNumber == newForm.SerialNumber);

                    newForm.RequestDate = DateTime.Now;
                    newForm.Pointer = 0;
                    newForm.Equipments = equipment;

                    _context.FormReqDb.Add(newForm);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }else if (user.Type == "Supervisor") 
                {
                    var newForm = model.FormReqDbs;
                    var equipment = await _context.Equipment?.FirstOrDefaultAsync(m => m.SerialNumber == newForm.SerialNumber);

                    newForm.RequestDate = DateTime.Now;
                    newForm.Pointer = 1;
                    newForm.Equipments = equipment;
                    newForm.status = "Accepted";

                    _context.FormReqDb.Add(newForm);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
              

            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"ModelState Error for {state.Key}: {error.ErrorMessage}");
                    }
                }
            }
            // If invalid → reload page and pass Users list again
            model.AllUsers = _context.Users.ToList();

            return View(model);
        }
        public IActionResult CreateEquipment()
        {
            var users = _context.Users.ToList();

            var viewModel = new RequestsViewModel
            {
                FormReqDbs = new FormReqDb(), // empty form for Create page
                AllUsers = users
            };
            return View(viewModel);
        }
        // POST: FormReqDbs/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEquipment(RequestsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"  {state.Key}: {error.ErrorMessage}");
                    }
                }
            }
                if (ModelState.IsValid)
            {
              
                var newForm = model.Inventory;

                _context.Equipment.Add(newForm);
                _context.SaveChanges();

                return RedirectToAction("Inventory");
            }

      
            model.AllUsers = _context.Users.ToList();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormRequests(string site, string department, string type)
        {
            var records = await _context.Equipment.Where(f =>
            f.Site == site &&
            f.Department == department &&
            f.EquipmentType == type).ToListAsync();

            return Json(records);
        }


        // GET: FormReqDbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb.FindAsync(id);
            if (formReqDb == null)
            {
                return NotFound();
            }
            return View(formReqDb);
        }


        // POST: FormReqDbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestDate,Site,Department,ResponsibleOfficer,ContactPhone,EquipmentType,ProblemDescription,SerialNumber,From,To,MovementDate,Remarks,IsApproved,IsInvalid,Verification")] FormReqDb formReqDb)
        {
            if (id != formReqDb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(formReqDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormReqDbExists(formReqDb.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(formReqDb);
        }
        [HttpGet("FormReqDbs/EditUser/{UserName}")]
        public async Task<IActionResult> EditUser(string? UserName)
        {
            if (UserName == null)
            {
                return NotFound();
            }
            var allUsers =  _context.Users.ToList();
            var Alluser = await _context.Alluser.FirstOrDefaultAsync(m=>m.UserName==UserName);
            if (Alluser == null)
            {
                return NotFound();
            }
            var viewModel = new RequestsViewModel

            {
                AllUsers=allUsers,
                User = Alluser
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editUser(string userName,string Supervisor,string Site,string dept)
        {
            Console.WriteLine($"Searching for username: {userName}");
            var Alluser = await _context.Alluser.FirstOrDefaultAsync(m => m.UserName == userName);
            Alluser.Supervisor = Supervisor;
            Alluser.Site=Site;
            Alluser.Dept = dept;
            await _context.SaveChangesAsync();
            return RedirectToAction("UserForm");
        }
        public async Task<IActionResult> EditEquipment(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var allUsers = _context.Users.ToList();
            var equipment = await _context.Equipment.FirstOrDefaultAsync(m => m.ID == id);
            if (equipment == null)
            {
                return NotFound();
            }
            var viewModel = new RequestsViewModel

            {
                AllUsers = allUsers,
                Inventory= equipment
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEquipment(RequestsViewModel model)
        {
            if (model.Inventory==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model.Inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                
                }
                return RedirectToAction("Inventory");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FilterByDate(DateTime Date)
        {
            var Request =await _context.FormReqDb.Where(m=>m.RequestDate>Date).Include(j=>j.Equipments).ToListAsync();
            var view = new RequestsViewModel
            {
                FormReqDb = Request,
                AllUsers=await _context.Alluser.ToListAsync(),
            };
            return View("SReportForm",view);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int Status,string? Remarks)
        {
            var formReqDb = await _context.FormReqDb.FindAsync(id);
            if (formReqDb == null)
            {
                return NotFound();
            }

            if (Status==1)
            {
                formReqDb.status = "Accepted";
                formReqDb.Pointer += 1;//1
            }
            else if (Status == 2)
            {
                formReqDb.status = "Rejected";
                formReqDb.Pointer = 20;
            }
            else if(Status == 3)
            {
                formReqDb.status = "Onsite request";
                formReqDb.Pointer += 2;//3
                formReqDb.remarks= Remarks ?? "";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TechnicianForm));
            }
            else if (Status == 4) 
            {
                formReqDb.status = "Transit request";
                formReqDb.Pointer += 1;//2
                formReqDb.remarks = Remarks ?? "";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TechnicianForm));
            }


                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormReqDbExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            return RedirectToAction(nameof(SupervisorForm));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(int id, string feedback)
        {
            var formReqDb = await _context.FormReqDb.FindAsync(id);
            if (formReqDb == null)
            {
                return NotFound();
            }
            else
            {
                formReqDb.Feedback = feedback;
                formReqDb.status = "Close";
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormReqDbExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Feedback));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ITOstatus(int id,string Status,string Remarks)
        {
            var formReqDb = await _context.FormReqDb.FindAsync(id);
            var registry =  _context.Registries.Any(k => k.FormReqDbId == id);
            if (formReqDb == null)
            {
                return NotFound();
            }
            if (Status== "rejects")
            {
                if (registry) {
                    formReqDb.status = "Send back";
                    formReqDb.remarks = Remarks;
                    formReqDb.Pointer = 20;
                }
                else {
                    formReqDb.status = Status;
                    formReqDb.remarks = Remarks;
                    formReqDb.Pointer = 20;
                }
            } 
            else if (formReqDb.Pointer == atTransitRequest)
            {
                formReqDb.status = "Transitting";
                formReqDb.Pointer += 2;
            }
            else if (formReqDb.Pointer == atOnsiteRequest)
            {
                formReqDb.status = "Repairing";
                formReqDb.Pointer += 2;
            }
            else if (formReqDb.Pointer == atPendingRequest)
            {
                formReqDb.status = "Start repairing";
                formReqDb.Pointer += 1;//7
            }
            else if (formReqDb.Pointer == atFinalRequest)
            {
                if (registry)
                {
                    formReqDb.status = "Return";
                    formReqDb.Pointer += 1;//9
                }
                else
                {
                    formReqDb.status = "Complete";
                    formReqDb.Pointer += 1;//9
                    formReqDb.RequestDate = DateTime.Now;
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormReqDbExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("ITOForm");
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string actionType)
        {
            var request = _context.FormReqDb.FirstOrDefault(r => r.Id == id);
            var checkRegistry =_context.Registries.Any(j=>j.FormReqDbId==id);
            if (request == null)
                return Json(new { success = false, message = "Request not found." });

            if (actionType == "reject")
            {
                if (checkRegistry) 
                {
                    request.status = "send back";
                    request.Pointer = 20;
                    _context.SaveChanges();
                    return Json(new { success = true, newStatus = request.status });
                }
                else 
                {
                    request.status = "rejected";
                    request.Pointer = 20;
                    _context.SaveChanges();
                    return Json(new { success = true, newStatus = request.status });
                }
            } 
            else if (actionType == "accept") 
            {
                if (request.Pointer == atNewRequest)
                {
                        request.status = "Accepted";
                        request.Pointer += 1;
                        _context.SaveChanges();
                        return Json(new { success = true, newStatus = request.status });
                }
                else if (request.Pointer == atOnsiteRequest)
                {
                    request.status = "repairing";
                    request.Pointer += 2;
                    _context.SaveChanges();
                    return Json(new { success = true, newStatus = request.status });
                }
                else if (request.Pointer == atTransitRequest)
                {
                    request.status = "Transitting";
                    request.Pointer += 2;
                    _context.SaveChanges();
                    return Json(new { success = true, newStatus = request.status });
                }
                else if (request.Pointer == atPendingRequest)
                {
                    request.status = "Start repairing";
                    request.Pointer += 1;
                    _context.SaveChanges();
                    return Json(new { success = true, newStatus = request.status });
                }
                else if (request.Pointer == atFinalRequest)
                {
                    if (checkRegistry)
                    {
                        request.status = "Return";
                        request.Pointer += 1;
                        _context.SaveChanges();
                        return Json(new { success = true, newStatus = request.status });
                    }
                    else
                    {
                        request.status = "Complete";
                        request.Pointer += 1;
                        _context.SaveChanges();
                        return Json(new { success = true, newStatus = request.status });
                    }

                }
            }


                /*
                string currentStatus = request.status?.ToLower();
                if (currentStatus == "transit" || currentStatus == "accept transit" || currentStatus == "reject transit")
                    request.status = actionType == "accept" ? "accept transit" : "reject transit";
                else if (currentStatus == "onsite" || currentStatus == "accept onsite" || currentStatus == "reject onsite")
                    request.status = actionType == "accept" ? "accept onsite" : "reject onsite";
                else
                    return Json(new { success = false, message = "Invalid status for action." });
                */
                _context.SaveChanges();
            return Json(new { success = true, newStatus = request.status });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int id,string SerialNumber, string Status,string Remarks,RequestsViewModel model)
        {
            var formReqDb = await _context.FormReqDb.FindAsync(id);
            var Equipment = await _context.Equipment.FirstOrDefaultAsync(f => f.SerialNumber==formReqDb.SerialNumber);


            if (formReqDb == null)
            {
                return NotFound();
            }
            if (formReqDb.Pointer == atStartRepairing)
            {
                var ITTreport =await _context.ITTreport.FirstOrDefaultAsync(f=>f.FormReqDb==id);
                ITTreport.Report +=" 2. "+ Remarks;
                formReqDb.status = "Final request";
                formReqDb.Pointer += 1;//8

            }
            else if (formReqDb.Pointer==atReport)

            {

                    var newForm = new ITTreport
                    {
                        FormReqDb=id,
                        SerialNumber=SerialNumber,
                        Report="1. " + Remarks
                    };
                    formReqDb.ITTReports =newForm;
                    _context.ITTreport.Add(newForm);
                    _context.SaveChanges();
                    formReqDb.status = "Pending request";
                    formReqDb.Pointer += 1;//6
            }
            else 
            {
                formReqDb.status = "Pending request";
                formReqDb.remarks = Remarks;
               
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormReqDbExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("TechnicianForm");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Arrived(int id,string Remarks)
        {
            var Registry = await _context.Registries.FindAsync(id);
            if (Registry == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb.FindAsync(Registry.FormReqDbId);
            if (formReqDb == null)
            {
                return NotFound();
            }

            if (formReqDb.Pointer == 20)
                {
                formReqDb.status = "rejected";
                Registry.Remarks += " 2. "+ Remarks;
                }
                else if (formReqDb.Pointer == atTransitting)
                {
                    formReqDb.status = "Repairing";                
                    Registry.IsValid = true;
                    Registry.Remarks += " 2. " + Remarks;
                    formReqDb.Pointer += 1;//5
                } 
                else if (formReqDb.Pointer == atReturn)            
                {                
                formReqDb.status = "Complete";
                formReqDb.RequestDate =DateTime.Now;
                Registry.IsValid = !Registry.IsValid;
                Registry.Remarks += " 2. " + Remarks;
                }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormReqDbExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("RegistryForm");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRegistry( RequestsViewModel model)
        {

            try
            {
                var formReqDb = await _context.FormReqDb.FindAsync(model.Registry?.FormReqDbId);
                var equipment =await _context.Equipment.FirstOrDefaultAsync(m=>m.SerialNumber==formReqDb.SerialNumber);
                var existRegistry= _context.Registries.Any(k=>k.FormReqDbId==formReqDb.Id);
                var newform = model.Registry;
                if (formReqDb == null)
                {
                 return NotFound();
                }

                await _context.SaveChangesAsync();
                if (existRegistry)
                {
                    newform.To= formReqDb.Site;
                    newform.From = "Reduit";
                    newform.Equipment = equipment;
                    _context.Registries.Add(newform);
                    _context.SaveChanges();
                    return RedirectToAction("RegistryForm");
                }
                else
                {
                    newform.From = formReqDb.Site;
                    newform.To = "Reduit";
                    newform.Equipment = equipment;
                    _context.Registries.Add(newform);
                    _context.SaveChanges();
                    return RedirectToAction("RegistryForm");
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB error: " + ex.Message);
            }

            return RedirectToAction("RegistryForm");
        }



        // GET: FormReqDbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formReqDb = await _context.FormReqDb
                .FirstOrDefaultAsync(m => m.Id == id);
            if (formReqDb == null)
            {
                return NotFound();
            }

            return View(formReqDb);
        }

        // POST: FormReqDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formReqDb = await _context.FormReqDb.FindAsync(id);
            if (formReqDb != null)
            {
                _context.FormReqDb.Remove(formReqDb);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormReqDbExists(int id)
        {
            return _context.FormReqDb.Any(e => e.Id == id);
        }
    }
}
