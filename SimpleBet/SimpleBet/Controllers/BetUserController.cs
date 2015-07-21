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
    public class BetUserController : ApiController
    {
        //Attributes
        private IDataService dataService = new DataService();

        //Constructors
        public BetUserController() { }

        // GET: api/values
        //[HttpGet]
        //public string Get()
        //{
        //    return JsonConvert.SerializeObject(this.dataService);
        //}

        //currently cannot get by Id will add later if required
        // GET api/values/5
        //[HttpGet("{id:int}")]
        //public IActionResult Get(int id)
        //{
        //    BetUser user = this.dbContext.BetUsers.Find(id);
        //    if (user == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }
        //    else
        //    {
        //        string userJson = JsonConvert.SerializeObject(user);
        //        return new ObjectResult(userJson);
        //    }
        //}

        // POST api/values
        //[HttpPost]
        //public IActionResult Post([FromBody]BetUser betUser)
        //{
        //    this.dbContext.BetUsers.Add(betUser);
        //    this.dbContext.SaveChanges();
        //    string json = JsonConvert.SerializeObject(betUser);
        //    return new ObjectResult(json);
        //}

        // PUT api/values/5
        [Route("api/[controller]/{betId}/{userId}")]
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, int betId, int userId, [FromBody]BetUser betUser)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (betId != betUser.BetId || userId != betUser.UserId)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }
            
            try
            {
                this.dataService.UpdateBetUser(betUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (this.dataService.GetBetUser(betId, userId) == null)
                {
                    return request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    throw;
                }
            }

            //seriallize the object
            return request.CreateResponse<BetUser>(HttpStatusCode.OK, betUser);
        }

        // DELETE api/values/5
        [Route("api/[controller]/{betId}/{userId}")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int userId, int betId)
        {
            BetUser betUser = this.dataService.RemoveBetUser(betId, userId);
            if (betUser == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            string json = JsonConvert.SerializeObject(betUser);
            return request.CreateResponse<BetUser>(HttpStatusCode.OK, betUser);
        }
    }
}
