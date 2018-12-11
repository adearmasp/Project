using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Store.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerFacade _customerFacade;

        public CustomersController(ICustomerFacade customerFacade)
        {
            _customerFacade = customerFacade;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            var customers = (await _customerFacade.GetCustomersAsync(c => true)).ToList();
            return Ok(customers);
        }

        // GET api/customers/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomersAsync(int id)
        {
            var customer = (await _customerFacade.GetCustomersAsync(item => true)).FirstOrDefault(c => c.Id == id);
            if (customer!=null)
            {
                return Ok(customer);
            }      
            return NotFound();
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer cust)
        {
            if (cust == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _customerFacade.SaveCustomerAsync(cust);           
            return CreatedAtRoute("GetCustomer", new { id = cust.Id }, cust);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id,[FromBody] JsonPatchDocument<Customer> cust)
        {
            if (cust == null)
            {
                return BadRequest("Problem with charge the Json ");
            }          
            var cutFromStore = (await _customerFacade.GetCustomersAsync(item => true)).FirstOrDefault(c => c.Id == id);
            if (cutFromStore == null)
            {
                return NotFound();
            }
            var custToPatch = new Customer()
            {
                Name = cutFromStore.Name,
                Surname = cutFromStore.Surname,
                Photo= cutFromStore.Photo,
                ModifiedBy=cutFromStore.ModifiedBy,
            };
            cust.ApplyTo(custToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            cutFromStore.Name = custToPatch.Name;
            cutFromStore.Surname = custToPatch.Surname;
            cutFromStore.Photo = custToPatch.Photo;
            cutFromStore.ModifiedBy = custToPatch.ModifiedBy;
            await _customerFacade.SaveCustomerAsync(cutFromStore);
            return NoContent();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var custFromStore = (await _customerFacade.GetCustomersAsync(item=>true)).FirstOrDefault(c=>c.Id==id);
            if (custFromStore == null)
            {
                return NotFound();
            }
            custFromStore.Name = customer.Name;
            custFromStore.Surname = customer.Surname;
            custFromStore.Photo = customer.Photo;
            custFromStore.ModifiedBy = customer.ModifiedBy;
            await _customerFacade.SaveCustomerAsync(custFromStore);
            return NoContent();

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cust = (await _customerFacade.GetCustomersAsync(item => true)).FirstOrDefault(c => c.Id == id);
            if (cust == null)
            {
                return NotFound();
            }
            await _customerFacade.DeleteCustomerAsync(cust);
            return NoContent();
        }
    }
}
