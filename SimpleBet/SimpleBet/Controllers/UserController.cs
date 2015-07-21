using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;


namespace SimpleBet.Controllers
{
    public class UserController : ApiController
    {
        //Attributes
        private IDataService dataService = new DataService();

        //Constructors
        public UserController() { }

        // GET: api/values
        //[HttpGet]
        //public string Get()
        //{
        //    return JsonConvert.SerializeObject(this.dataService.GetUsers());
        //}

        // GET api/values/5
        [Route("api/user/{id:int}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            User user = this.dataService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                string userJson = JsonConvert.SerializeObject(user);
                return Ok(userJson);
            }
        }

        //this will be hitted when querry by facebookID
        [Route("api/user/{id:long}")]
        [HttpGet]
        public IHttpActionResult Get(long id)
        {
            User user = this.dataService.GetUserByFacebookId(id);
            if (user == null)
            {
                return NotFound();
            } else
            {
                string userJson = JsonConvert.SerializeObject(user);
                return Ok(userJson);
            }
        }

        // POST api/values
        [HttpPost]
        public IHttpActionResult Post([FromBody]User user)
        {
            user = this.dataService.AddUser(user);
            string json = JsonConvert.SerializeObject(user);
            return Ok(json);
        }

        // PUT api/values/5
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}