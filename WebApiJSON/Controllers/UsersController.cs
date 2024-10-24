using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApiJSON.Model;

namespace WebApiJSON.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _filePath = "data.json";

        private List<User> ReadUsersFromFile()
        {
            var json = System.IO.File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        private void WriteUsersToFile(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            System.IO.File.WriteAllText(_filePath, json);
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {

            var json=System.IO.File.ReadAllText(_filePath);
            var users= JsonConvert.DeserializeObject<IEnumerable<User>>(json);
            //or
            //var users=ReadUsersFromFile();
            return Ok(users);
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {

            var json = System.IO.File.ReadAllText(_filePath);
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(json);
 
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return Ok(user);
        }

        // POST api/Users
        [HttpPost]
        public ActionResult Post([FromBody] User newUser)
        {
       

            var json = System.IO.File.ReadAllText(_filePath);
            var users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            if (users.Any(u => u.Id == newUser.Id))
            {
                return BadRequest($"User with ID {newUser.Id} already exists.");
            }

            users.Add(newUser);
            var updatedJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            System.IO.File.WriteAllText(_filePath, updatedJson);
           

            return Ok($"User {newUser.Name} added successfully.");
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User updatedUser)
        {
            var users = ReadUsersFromFile();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.age = updatedUser.age;

            WriteUsersToFile(users);

            return Ok($"User with ID {id} updated successfully.");
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var users = ReadUsersFromFile();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            users.Remove(user);
            WriteUsersToFile(users);

            return Ok($"User with ID {id} deleted successfully.");
        }
    }
}
