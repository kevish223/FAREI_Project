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
                FormReqDb = await _context.FormReqDb.ToListAsync(),
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
                FormReqDb = await _context.FormReqDb.Where(j=>j.Supervisor.Contains(User.Identity.Name)&&j.status==null).ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> ITOform()
        {
            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.ToListAsync(),
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
                FormReqDb = await _context.FormReqDb.Where(j => (j.status.Contains("Transitting") || j.status.Contains("Send back") || j.status.Contains("Complete")) && j.Site.Contains(Site) ).ToListAsync(),
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
                Registries = await _context.Registries.Where(j=>j.From==Site || j.To==Site).ToListAsync(),
                AllUsers = _userManager.Users.ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> Feedback()
        {

            var model = new RequestsViewModel
            {
                FormReqDb = await _context.FormReqDb.Where(j=>j.status== "approve").ToListAsync(),
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

            var formReqDb = await _context.FormReqDbs?
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

            var formReqDb = await _context.FormReqDbs?
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

            var formReqDb = await _context.FormReqDbs?
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
        public IActionResult Create()
        {
            var users = _context.Users.ToList();

            var viewModel = new RequestsViewModel
            {
                FormReqDbs = new FormReqDb(), // empty form for Create page
                AllUsers = users
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RequestsViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Example: create new FormReqDb object to save
                var newForm = model.FormReqDbs;

                // Optionally set extra fields:
                newForm.RequestDate = DateTime.Now;

                _context.FormReqDbs.Add(newForm);
                _context.SaveChanges();

                return RedirectToAction("Index");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, String Accepted)
        {
            var formReqDb = await _context.FormReqDb.FindAsync(id);
            if (formReqDb == null)
            {
                return NotFound();
            }
            if (Accepted== "Transit")
            {
                formReqDb.status = "Transit request";
            }
            else if (Accepted == "Onsite")
            {
                formReqDb.status = "Onsite request";
            }else
            {
                formReqDb.status = Accepted;
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
                }
                else {
                    formReqDb.status = Status;
                    formReqDb.remarks = Remarks;
                }
            } else if (formReqDb.status == "Transit request")
            {
                formReqDb.status = "Transitting";
                formReqDb.remarks = Remarks ;
            }else if(formReqDb.status == "Pending request")
            {
                formReqDb.status = "Start repairing";
                formReqDb.remarks = Remarks ;
            }
            else if (formReqDb.status == "Final request")
            {
                formReqDb.status = "Complete";
                formReqDb.remarks = Remarks ;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int id, String Status,String Remarks)
        {
            var formReqDb = await _context.FormReqDb.FindAsync(id);
            var Equipment = await _context.Equipment.FirstOrDefaultAsync(f => f.SerialNumber==formReqDb.SerialNumber);

            if (formReqDb == null)
            {
                return NotFound();
            }
            if (formReqDb.status == "Start repairing")
            {
                formReqDb.status = "Final request";
                formReqDb.remarks = Remarks;
                Equipment.Remarks += " " + Remarks;
            }
            else {
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
        public async Task<IActionResult> Arrived(int id)
        {
            var Registry = await _context.Registries.FindAsync(id);
            var formReqDb =await _context.FormReqDb.FindAsync(Registry.FormReqDbId);
            var checkRegistry = _context.Registries.Any(j=>j.FormReqDbId==formReqDb.Id);
            if (Registry == null)
            {
                return NotFound();
            }
            if (checkRegistry) {
                if (formReqDb.status=="Send back")
                {
                    formReqDb.status ="rejected";
                }
                else if(formReqDb.status== "Transitting")
                {
                    formReqDb.status = "Repairing";
                    Registry.IsValid = true;
                }
            }
            else
            {
                Registry.IsValid = true;
                formReqDb.status = "Repairing";
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
                    _context.Registries.Add(newform);
                    _context.SaveChanges();
                    return RedirectToAction("RegistryForm");
                }
                else
                {
                    newform.From = formReqDb.Site;
                    newform.To = "Reduit";
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
