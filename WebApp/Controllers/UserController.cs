using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Store;
using WebApp.Data;
using WebApp.Models;
using static Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal.LoginModel;

namespace WebApp.Controllers
{
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

        [Required]
        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Id")]
        public string Id { get; set; }
    }

    [Authorize(Roles = "AdminRole")]
    public class UserController : Controller
    {
        private readonly IHostingEnvironment _hosting;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public UserController(ApplicationDbContext dbContext, IHttpClientFactory httpclientFactory,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            this.dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }


        // GET: User
        //[Authorize(Roles = "AdminRole")]
        public async Task<ActionResult> List()
        {
            List<InputModel> inputModels = new List<InputModel>();
            foreach (var item in dbContext.Users.ToList())
            {
                inputModels.Add(new InputModel
                {
                    Id = item.Id,
                    Email = item.Email,
                    IsAdmin = item.IsAdmin,
                    Password = item.PasswordHash
                });
            }
            //var items = await dbContext.Users.ToListAsync();
            //return View(items);
            return View(inputModels);
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id.Equals(" "))
            {
                return NotFound();
            }
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            var inputModel = new InputModel
            {
                Id = user.Id,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Password = user.PasswordHash
            };

            if (user.Equals(" "))
            {
                return NotFound();
            }
            return View(inputModel);

        }
        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InputModel user)
        {

            var returnUrl = Url.Content("~/");
            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser { UserName = user.Email, Email = user.Email, IsAdmin = user.IsAdmin, Id = user.Id};
                var result = await _userManager.CreateAsync(applicationUser, user.Password);
                if (result.Succeeded)
                {
                    var roleString = user.IsAdmin ? "AdminRole" : "UserRole";
                    await _roleManager.CreateAsync(new IdentityRole(roleString));

                    var currentUser = await _userManager.FindByNameAsync(applicationUser.UserName);
                    var roleResult = await _userManager.AddToRoleAsync(currentUser, roleString);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = applicationUser.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                    //return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            returnUrl = Url.Content("~/User/List");
            return LocalRedirect(returnUrl);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id.Equals(" "))
            {
                return NotFound();
            }
            var item = await dbContext.Users.FirstOrDefaultAsync(u=>u.Id==id);
            var inputModel = new InputModel
            {
                Id = item.Id,
                Email = item.Email,
                IsAdmin = item.IsAdmin,
            };

            if (item.Equals(" "))
            {
                return NotFound();
            }

            return View(inputModel);
          
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, InputModel user)
        {
            var userDb = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            userDb.UserName = user.Email;
            userDb.Id = id;
            userDb.Email = user.Email;
            userDb.IsAdmin = user.IsAdmin;
           // userDb.PasswordHash = user.Password;

            var applicationUser = new ApplicationUser { UserName = user.Email, Email = user.Email, IsAdmin = user.IsAdmin, Id = user.Id };

            var roleString = user.IsAdmin ? "AdminRole" : "UserRole";
            await _roleManager.CreateAsync(new IdentityRole(roleString));

            //var currentUser = await _userManager.FindByNameAsync(applicationUser.UserName);
            var roleResult = await _userManager.AddToRoleAsync(applicationUser, roleString);

            dbContext.Update(userDb);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id.Equals(" "))
            {
                return NotFound();
            }
            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user.Equals(" "))
            {
                return NotFound();
            }
            dbContext.Remove(user);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(List));
           
        }
    }
}