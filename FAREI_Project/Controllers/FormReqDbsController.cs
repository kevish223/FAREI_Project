using Azure.Core;
using FAREI_Project.Data;
using FAREI_Project.Models;

using FAREI_Project.ViewModel;
using FormRequest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Mono.TextTemplating;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
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



        public FormReqDbsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
           
        }

        // GET: FormReqDbs
        public async Task<IActionResult> Index()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Include(m=>m.ITTReports).ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            var username = User.Identity.Name;
            if (username==null)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(username);
            var type=user.Type;
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
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> SupervisorForm()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j=>j.Supervisor.Contains(User.Identity.Name)&&j.status=="pending").ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> ITOform()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Include(m=>m.Equipments).Include(m=>m.ITTReports).ToListAsync(),
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
                FormReqDb = await _context.FormReqDb.Include(m=>m.Equipments).Where(j => (j.status.Contains("Transitting") || j.status.Contains("Send back") || j.status.Contains("Return")) && j.Site.Contains(Site) ).ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> TechnicianForm()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.ToListAsync(),
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
                    AllUsers = _userManager.Users.ToList()
                };
                return View(User);
            }else if (user.Type == "Supervisor") 
            {
                var Supervisor = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Where(j => j.Supervisor == name).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(Supervisor);
            }else if (user.Type== "Technician" || user.Type== "ITO") 
            {
                var model = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
                    AllUsers = _userManager.Users.ToList()
                };
                return View(model);
            } else if (user.Type== "Registry") 
            {
                var Registry = new RequestsViewModel
                {
                    FormReqDb = await _context.FormReqDb.Include(m => m.Equipments).Where(m=>m.Site==user.Site).ToListAsync(),
                    Registries = await _context.Registries.ToListAsync(),
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
             
                FormReqDb=await _context.FormReqDb.ToListAsync(),
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

           var newFormDBReq = await _context.FormReqDb.Where(j => j.status == "Complete").ToListAsync();
            var model = new RequestsViewModel
            {
                FormReqDb = newFormDBReq,
                Registries = await _context.Registries.ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }

        // GET: FormReqDbs/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var formReqDb = await _context.FormReqDb
                .FirstOrDefaultAsync(m => m.Id == id);
            var AllUsers = _userManager.Users.ToList();
            
            var viewModel = new RequestsViewModel
            {
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
            var formReqDb = await _context.FormReqDb
               .FirstOrDefaultAsync(m => m.Id == Registry.FormReqDbId);
   

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
                FormReqDbs = new FormReqDb(), 
                AllUsers = users,
                User=FindUser
               
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                        Console.WriteLine($"ModelState Error for {state.Key}: {error.ErrorMessage}");
                    }
                }
            }
                if (ModelState.IsValid)
            {
                // Example: create new FormReqDb object to save
                var newForm = model.Inventory;

                // Optionally set extra fields:
               

                _context.Equipment.Add(newForm);
                _context.SaveChanges();

                return RedirectToAction("CreateEquipment");
            }

            // If invalid → reload page and pass Users list again
            model.AllUsers = _context.Users.ToList();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitThirdPartyForm(RequestsViewModel model,String? Status,String? remarks)
        {

            if (ModelState.IsValid)
            {
                // Example: create new FormReqDb object to save
                var newForm = model.Third_Party;
                var request= await _context.FormReqDb.FindAsync(newForm.FormReqDbID);
                if (request.status== "Sent to third party")
                {
                    request.remarks=remarks;
                    request.status ="approve";
                    _context.SaveChanges();
                    return RedirectToAction("ITOForm");
                }

                // Optionally set extra fields:
                if (Status== "approve")
                {
                    _context.Third_Parties.Add(newForm);
                    request.status ="Sent to third party";
                    _context.SaveChanges();
                }
                else
                {
                    request.status="Rejected";
                    _context.SaveChanges();
                }



                    return RedirectToAction("ITOForm");
            }

            // If invalid → reload page and pass Users list again
            model.AllUsers = _context.Users.ToList();

            return RedirectToAction("ITOForm");
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
        public async Task<IActionResult> EditUser(String? UserName)
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
        public async Task<IActionResult> editUser(String userName,String Supervisor,String Site,String dept)
        {
            Console.WriteLine($"Searching for username: {userName}");
            var Alluser = await _context.Alluser.FirstOrDefaultAsync(m => m.UserName == userName);
            Alluser.Supervisor = Supervisor;
            Alluser.Site=Site;
            Alluser.Dept = dept;
            await _context.SaveChangesAsync();
            return RedirectToAction("UserForm");
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
        public async Task<IActionResult> ChangeStatus(int id, int Status)
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
                formReqDb.Pointer = 0;
            }
            else if(Status == 3)
            {
                formReqDb.status = "Onsite request";
                formReqDb.Pointer += 2;//3
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TechnicianForm));
            }
            else if (Status == 4) 
            {
                formReqDb.status = "Transit request";
                formReqDb.Pointer += 1;//2
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
        public async Task<IActionResult> SubmitFeedback(int id, String feedback)
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
        public async Task<IActionResult> ITOstatus(int id,String Status,String Remarks)
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
                    formReqDb.Pointer = 0;
                }
                else {
                    formReqDb.status = Status;
                    formReqDb.remarks = Remarks;
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
                    request.Pointer = 0;
                    _context.SaveChanges();
                    return Json(new { success = true, newStatus = request.status });
                }
                else 
                {
                    request.status = "rejected";
                    request.Pointer = 0;
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
        public async Task<IActionResult> Report(int id,String SerialNumber, String Status,String Remarks,RequestsViewModel model)
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
                Equipment.Remarks += " " + Remarks;
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

            if (formReqDb.Pointer == 0)
                {
                formReqDb.status = "rejected";
                Registry.Remarks = Remarks;
                }
                else if (formReqDb.Pointer == 4)
                {
                    formReqDb.status = "Repairing";                
                    Registry.IsValid = true;
                    Registry.Remarks = Remarks;
                    formReqDb.Pointer += 1;//5
                } 
                else if (formReqDb.Pointer == 9)            
                {                
                formReqDb.status = "Complete";
                formReqDb.RequestDate =DateTime.Now;
                Registry.IsValid = !Registry.IsValid;
                Registry.Remarks = Remarks;
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
