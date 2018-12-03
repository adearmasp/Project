using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Store.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersFacade _userFacade;

        public UsersController(IUsersFacade customerFacade)
        {
            _userFacade = customerFacade;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var users = (await _userFacade.GetUsersAsync(c => true)).ToList();
            return Ok(users);
        }

        // GET api/user/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = (await _userFacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }


        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _userFacade.SaveUserAsync(user);


            return CreatedAtRoute("GetUser", new { id = user.Id }, user);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<User> user)
        {
            if (user == null)
            {
                return BadRequest("Problem with charge the Json ");
            }

            var userFromStore = (await _userFacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);
            if (userFromStore == null)
            {
                return NotFound();
            }
            var userToPatch = new User()
            {
                Name = userFromStore.Name,
                Email = userFromStore.Email,
                Password = userFromStore.Password,
            };
            user.ApplyTo(userToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userFromStore.Name = userToPatch.Name;
            userFromStore.Email = userToPatch.Email;
            userFromStore.Password = userToPatch.Password;
            return NoContent();
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {

            if (user == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var userFromStore = (await _userFacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);
            if (userFromStore == null)
            {
                return NotFound();
            }
            userFromStore.Name = user.Name;
            userFromStore.Email = user.Email;
            userFromStore.Password = user.Password;

            return NoContent();



        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = (await _userFacade.GetUsersAsync(item => true)).FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            await _userFacade.DeleteUserAsync(user);
            return NoContent();
        }
    }
}
