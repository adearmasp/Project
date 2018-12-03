using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store;
using Store.Data.EntityFramework;

namespace WebStore.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerFacade _customerfacade;
        private readonly IHostingEnvironment _hosting;

        public CustomerController(ICustomerFacade customerfacade, IHostingEnvironment hosting)
        {
            _customerfacade = customerfacade;
            _hosting = hosting;

        }


        // GET: Customer
        public async Task<ActionResult> CustomerList()
        {

            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://localhost:64944/api/customer");
            var responseString = await response.Content.ReadAsStringAsync();

            var items = JsonConvert.DeserializeObject<IEnumerable<Customer>>(responseString);

            return View(items);
        }

        // GET: Customer/Details/5
        public async Task<ActionResult> Details(int id)
        {
            
            var customer = (await _customerfacade.GetCustomersAsync(item=>true)).FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> Create(Customer cust, IFormFile Photo)
        {
           
            if (ModelState.IsValid)
            {

                if (Photo!=null)
                {
                    string path_Root = _hosting.WebRootPath;
                    string path_photo = path_Root + "\\images\\Upload_image\\" + Photo.Name;


                    using ( var stream=new FileStream(path_photo, FileMode.Create))
                    {
                        await Photo.CopyToAsync(stream);
                    }

                    ViewData["filelocation"] = path_photo;
                }



                //cust.Id = new Guid();
                await _customerfacade.SaveCustomerAsync(cust);
                return RedirectToAction(nameof(CustomerList));

            }
            return View(cust);
        }

        // GET: Customer/Edit/5
        public async  Task<ActionResult> Edit(int id)
        {
            var customer = (await _customerfacade.GetCustomersAsync(item=>true)).FirstOrDefault(c => c.Id == id);

            if (customer==null)
            {
                return NotFound();
           }

            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, int create, [Bind("Name,Surname,Photo")] Customer cust)
        {
            var customer = (await _customerfacade.GetCustomersAsync(item=>true)).FirstOrDefault(c => c.Id == id);
            if (customer==null)
            {
                return NotFound();
            }
            customer.Name = cust.Name;
            customer.Photo = cust.Photo;
            customer.Surname = cust.Surname;
            customer.CreatedBy = cust.CreatedBy;
            customer.ModifiedBy = cust.ModifiedBy;


            await _customerfacade.SaveCustomerAsync(customer);

             //await _customerfacade.SaveCustomerAsync(cust);

                return RedirectToAction(nameof(CustomerList));
           
       
        }

        //GET: Customer/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var customer = (await _customerfacade.GetCustomersAsync(item => true)).FirstOrDefault(c => c.Id == id);

            await _customerfacade.DeleteCustomerAsync(customer);


            return RedirectToAction(nameof(CustomerList));


           
        }

     
    }
}