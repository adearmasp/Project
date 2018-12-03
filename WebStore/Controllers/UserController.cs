using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store;

namespace WebStore.Controllers
{
    public class UserController : Controller

    {
        private readonly IUsersFacade _userfacade;


        public UserController(IUsersFacade userfacade)
        {
            _userfacade = userfacade;

        }


        // GET: User
        public async Task<ActionResult> List()
        {
            var user = await _userfacade.GetUsersAsync(item => true);

            return View(user);
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var user = (await _userfacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                //cust.Id = new Guid();
                await _userfacade.SaveUserAsync(user);
                return RedirectToAction(nameof(List));

            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var user = (await _userfacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, User user)
        {
            var getuser = (await _userfacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);
            if (getuser == null)
            {
                return NotFound();
            }
            await _userfacade.DeleteUserAsync(getuser);

            await _userfacade.SaveUserAsync(user);

            return RedirectToAction(nameof(List));

        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = (await _userfacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);

            await _userfacade.DeleteUserAsync(user);


            return RedirectToAction(nameof(List));



        }

        
    }
}