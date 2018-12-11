using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store;

namespace WebApp.Controllers
{
    [Authorize(Roles = "AdminRole, UserRole")]
    public class CustomerController : Controller
    {
        
        private readonly IHostingEnvironment _hosting;
        private readonly IHttpClientFactory _httpclientFactory;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomerController(IHostingEnvironment hosting,
              IHttpClientFactory httpclient,
              RoleManager<IdentityRole> roleManager)
        {
            _hosting = hosting;
            _httpclientFactory = httpclient;
            _roleManager = roleManager;

        }

        // GET: Customer
        public async Task<ActionResult> List()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claims = User.Claims.ToList();
            }
           

            var new_claims = User.Claims.ToList();

            var client = _httpclientFactory.CreateClient("StoreApi");
            var request = new HttpRequestMessage(HttpMethod.Get, "api/customer");

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseString);
            return View(items);
        }

        // GET: Customer/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var client = _httpclientFactory.CreateClient("StoreApi");
            var request = new HttpRequestMessage(HttpMethod.Get, "api/customer/" + id.ToString());
            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<Customer>(responseString);

            if (items == null)
            {
                return NotFound();
            }
            return View(items);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Customer cust, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await Photo.CopyToAsync(stream);
                        cust.Photo = stream.ToArray();
                    }
                }
                if (User.Identity.IsAuthenticated)
                {
                    cust.CreatedBy = User.Identity.Name;
                }

                var client = _httpclientFactory.CreateClient("StoreApi");
                var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress.AbsoluteUri + "api/customer/");
                var serialized = JsonConvert.SerializeObject(cust);
                var response = await client.PostAsync(request.RequestUri, new StringContent(serialized, Encoding.UTF8, "application/json"));

                return RedirectToAction(nameof(List));
            }
            return View(cust);
        }
 
        // GET: Customer/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            else
            {
                var client = _httpclientFactory.CreateClient("StoreApi");
                var request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress.AbsoluteUri + "api/customer/" + id.ToString());
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<Customer>(responseString);

                return View(items);
            }
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Customer cust, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await Photo.CopyToAsync(stream);
                        cust.Photo = stream.ToArray();
                    }
                }
                if (User.Identity.IsAuthenticated)
                {
                    cust.ModifiedBy = User.Identity.Name;
                }

                var client = _httpclientFactory.CreateClient("StoreApi");
                var request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress.AbsoluteUri + "api/customer/" + id.ToString());
                var serialized = JsonConvert.SerializeObject(cust);
                var response = await client.PutAsync(request.RequestUri, new StringContent(serialized, Encoding.UTF8, "application/json"));

                return RedirectToAction(nameof(List));
            }
            return View(cust);
        }

        //GET: Customer/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var client = _httpclientFactory.CreateClient("StoreApi");
            var request = new HttpRequestMessage(HttpMethod.Delete, client.BaseAddress.AbsoluteUri + "api/customer/" + id.ToString());
            var response = await client.DeleteAsync(request.RequestUri);

            return RedirectToAction(nameof(List));
        }
    }
}