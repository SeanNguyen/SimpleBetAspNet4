using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Route("api/user/{id:long}")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, long id)
        {
            //this is facebook id
            if (id > int.MaxValue)
            {

                User user = this.dataService.GetUserByFacebookId(id);
                if (user == null)
                {
                    return request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    return request.CreateResponse<User>(HttpStatusCode.OK, user);
                }
            }
            else
            {
                int userId = (int) id;
                User user = this.dataService.GetUserById(userId);
                if (user == null)
                {
                    return request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    return request.CreateResponse<User>(HttpStatusCode.OK, user);
                }
            }
        }

        // POST api/values
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody]User user)
        {
            user = this.dataService.AddUser(user);
            return request.CreateResponse<User>(HttpStatusCode.OK, user);
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