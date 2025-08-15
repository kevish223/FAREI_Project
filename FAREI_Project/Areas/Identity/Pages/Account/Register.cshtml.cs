// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using AspNetCoreGeneratedDocument;
using FAREI_Project.Data;
using FAREI_Project.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace FAREI_Project.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public List<SelectListItem> Types { get; set; }
        public List<SelectListItem> Sites { get; set; }
        public List<SelectListItem> Supervisor { get; set; }
        public List<SelectListItem> Dept { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string SelectedSupervisor { get; set; }
            [Required]
            [Display(Name = "Role")]
            public string Type { get; set; }
            [Required]
            [Display(Name = "Site")]
            public string Site { get; set; }
            [Display(Name = "Supervisor")]
            public List<SelectListItem> SupervisorList { get; set; }
            [Display(Name = "Dept")]
            public string Dept { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Types = new List<SelectListItem>
            {
                new SelectListItem { Value = "User", Text = "User" },
                new SelectListItem { Value = "Supervisor", Text = "Supervisor" },
                new SelectListItem { Value = "Registry", Text = "Registry" },
                new SelectListItem { Value = "Technician", Text = "Technician" },
                new SelectListItem { Value = "ITO", Text = "ITO" },
                new SelectListItem { Value = "Admin", Text = "Admin" }
            };
            Sites = new List<SelectListItem>
            {
                new SelectListItem { Value = "St Pierre", Text = "St Pierre" },
                new SelectListItem { Value = "Reduit", Text = "Reduit" },
                new SelectListItem { Value = "Curepipe", Text = "Curepipe" },
                new SelectListItem { Value = "Mapou", Text = "Mapou" },
                new SelectListItem { Value = "Flacq", Text = "Flacq" },
                new SelectListItem { Value = "riviere des anguilles", Text = "riviere des anguilles" },
                new SelectListItem { Value = "Plaisance", Text = "Plaisance" },
                new SelectListItem { Value = "Vacoas", Text = "Vacoas" },
            };
            try
            {
                if (Input == null)
                {
                    Input = new InputModel();
                }

                Input.SupervisorList = await _context.Alluser.Where(m => m.Type == "Supervisor")
                    .Select(u => new SelectListItem
                    {
                        Value = u.UserName,
                        Text = u.UserName
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading supervisor list: {ex.Message}");
                throw;
            }

            Dept = new List<SelectListItem>
            {
                new SelectListItem { Value = "Livestock Research", Text = "Livestock Research" },
                new SelectListItem { Value = "Crop Research", Text = "Crop Research" },
                new SelectListItem { Value = "Extension and training", Text = "Extension and training" },
                new SelectListItem { Value = "Technical Support", Text = "Technical Support" },
                new SelectListItem { Value = "Administrative section", Text = "Administrative section" },
                new SelectListItem { Value = "Internal Audit", Text = "Internal Audit" },
            };

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            returnUrl ??= Url.Content("~/");
            Types = new List<SelectListItem>
            {
                new SelectListItem { Value = "User", Text = "User" },
                new SelectListItem { Value = "Supervisor", Text = "Supervisor" },
                new SelectListItem { Value = "Registry", Text = "Registry" },
                new SelectListItem { Value = "Technician", Text = "Technician" },
                new SelectListItem { Value = "Admin", Text = "Admin" }
            };
            Sites = new List<SelectListItem>
            {
                new SelectListItem { Value = "St Pierre", Text = "St Pierre" },
                new SelectListItem { Value = "Reduit", Text = "Reduit" },
                new SelectListItem { Value = "Curepipe", Text = "Curepipe" },                                
                new SelectListItem { Value = "Mapou", Text = "Mapou" },
                new SelectListItem { Value = "Flacq", Text = "Flacq" },
                new SelectListItem { Value = "riviere des anguilles", Text = "riviere des anguilles" },
                new SelectListItem { Value = "Plaisance", Text = "Plaisance" },
                new SelectListItem { Value = "Vacoas", Text = "Vacoas" },
            };
            try
            {
                if (Input == null)
                {
                    Input = new InputModel();
                }

                Input.SupervisorList = await _context.Alluser.Where(m => m.Type == "Supervisor")
                    .Select(u => new SelectListItem
                    {
                        Value = u.UserName,
                        Text = u.UserName
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading supervisor list: {ex.Message}");
                throw;
            }
            Dept = new List<SelectListItem>
            {
                new SelectListItem { Value = "Livestock Research", Text = "Livestock Research" },
                new SelectListItem { Value = "Crop Research", Text = "Crop Research" },
                new SelectListItem { Value = "Extension and training", Text = "Extension and training" },
                new SelectListItem { Value = "Technical Support", Text = "Technical Support" },
                new SelectListItem { Value = "Administrative section", Text = "Administrative section" },
                new SelectListItem { Value = "Internal Audit", Text = "Internal Audit" },
            };
            
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.Type = Input.Type;
                user.Site = Input.Site;
                user.Supervisor = Input.SelectedSupervisor;
             
                user.Dept = Input.Dept;
                

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                   
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
