using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SimpleBet.Controllers
{
    public class WinningItemController : ApiController
    {
        //Attributes
        private IDataService dataService = new DataService();

        //Constructors
        public WinningItemController() { }

        // GET: api/values
        [HttpGet]
        public HttpResponseMessage Query(HttpRequestMessage request)
        {
            int typeInt = 0;
            string typeString = HttpContext.Current.Request.QueryString["type"];
            if (string.IsNullOrEmpty(typeString) || !int.TryParse(typeString, out typeInt))
            {
                typeInt = 0;
            }
            WINNING_ITEM_TYPE type = (WINNING_ITEM_TYPE) typeInt;

            int creatorId = 0;
            string creatorIdString = HttpContext.Current.Request.QueryString["creatorId"];
            if (string.IsNullOrEmpty(creatorIdString) || !int.TryParse(creatorIdString, out creatorId))
            {
                creatorId = 0;
            }

            int categoryInt = 0;
            string categoryString = HttpContext.Current.Request.QueryString["category"];
            if (string.IsNullOrEmpty(categoryString) || !int.TryParse(categoryString, out categoryInt))
            {
                categoryInt = 0;
            }
            WINNING_ITEM_CATEGORY category = (WINNING_ITEM_CATEGORY) categoryInt;

            IList<WinningItem> winningItems;
            
            if(type == WINNING_ITEM_TYPE.MONETARY)
            {
                if (category != WINNING_ITEM_CATEGORY.NONE)
                {
                    winningItems = this.dataService.GetMonetaryItemsByCategory(category);
                }
                else
                {
                    winningItems = this.dataService.GetWinningItemsByType(WINNING_ITEM_TYPE.MONETARY);
                }
            }
            else if(type == WINNING_ITEM_TYPE.NONMONETARY && creatorId > 0)
            {
                winningItems = dataService.GetNonmonetaryItemsByCreator(creatorId);
            }
            else
            {
                winningItems = dataService.GetWinningItems();
            }
            return request.CreateResponse<WinningItem[]>(HttpStatusCode.OK, winningItems.ToArray());
        }

        // GET api/values/5
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            WinningItem winningItem = dataService.GetWinningItem(id);

            if (winningItem == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            return request.CreateResponse<WinningItem>(HttpStatusCode.OK, winningItem);
        }

        // POST api/values
        [HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody]WinningItem winningItem)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            dataService.AddWinningItem(winningItem);
            string json = JsonConvert.SerializeObject(winningItem);
            return request.CreateResponse<WinningItem>(HttpStatusCode.OK, winningItem);
        }

        // PUT api/values/5
        [HttpPut]
        public HttpResponseMessage Put(HttpRequestMessage request, int id, [FromBody]WinningItem winningItem)
        {
            if (!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (id != winningItem.Id)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                this.dataService.UpdateWinningItem(winningItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (dataService.GetWinningItem(id) != null)
                {
                    return request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    throw;
                }
            }
            string json = JsonConvert.SerializeObject(winningItem);
            return request.CreateResponse<WinningItem>(HttpStatusCode.OK, winningItem);
        }

        // DELETE api/values/5
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            try
            {
                WinningItem winningItem = this.dataService.RemoveWinningItem(id);
                string json = JsonConvert.SerializeObject(winningItem);
                return request.CreateResponse<WinningItem>(HttpStatusCode.OK, winningItem);
            }
            catch
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
