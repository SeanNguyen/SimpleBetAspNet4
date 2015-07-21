using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class WinningItemController : ApiController
    {
        //Attributes
        private IDataService dataService = new DataService();

        //Constructors
        public WinningItemController() { }

        // GET: api/values
        [HttpGet]
        public IHttpActionResult Query([FromUri] int creatorId, [FromUri] WINNING_ITEM_TYPE type, [FromUri] WINNING_ITEM_CATEGORY category)
        {
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
            string json = JsonConvert.SerializeObject(winningItems);
            return Ok(json);
        }

        // GET api/values/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            WinningItem winningItem = dataService.GetWinningItem(id);

            if (winningItem == null)
            {
                return NotFound();
            }
            string json = JsonConvert.SerializeObject(winningItem);
            return Ok(json);
        }

        // POST api/values
        [HttpPost]
        public IHttpActionResult Post([FromBody]WinningItem winningItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dataService.AddWinningItem(winningItem);
            string json = JsonConvert.SerializeObject(winningItem);
            return Ok(json);
        }

        // PUT api/values/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]WinningItem winningItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != winningItem.Id)
            {
                return BadRequest();
            }

            try
            {
                this.dataService.UpdateWinningItem(winningItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (dataService.GetWinningItem(id) != null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            string json = JsonConvert.SerializeObject(winningItem);
            return Ok(json);
        }

        // DELETE api/values/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                WinningItem winningItem = this.dataService.RemoveWinningItem(id);
                string json = JsonConvert.SerializeObject(winningItem);
                return Ok(json);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
