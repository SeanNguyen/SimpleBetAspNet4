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
    public class BetController : ApiController
    {
        //Attributes
        private IDataService dataService = new DataService();

        //Constructors
        public BetController() { }

        // GET: api/values
        //[HttpGet]
        //public string Get()
        //{
        //    return JsonConvert.SerializeObject(this.dataService.GetBets());
        //}

        // GET api/values/5
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            Bet bet = this.dataService.GetBet(id);
            if (bet == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return request.CreateResponse<Bet>(HttpStatusCode.OK, bet);
            }
        }

        // POST api/values
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody]Bet bet)
        {
            //manually create datetime here, TODO: maybe move this creation to client
            bet.CreationTime = DateTime.UtcNow;

            this.dataService.AddBet(bet);
            return request.CreateResponse<Bet>(HttpStatusCode.OK, bet);
        }

        // PUT api/values/5
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, int id, [FromBody]Bet bet)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != bet.Id)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                this.dataService.UpdateBet(bet);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (dataService.GetBet(id) != null)
                {
                    return request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    throw;
                }
            }
            return request.CreateResponse<Bet>(HttpStatusCode.OK, bet);
        }

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //}
    }
}