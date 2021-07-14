using DonatekartTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

namespace DonatekartTask.Controllers
{
    public class CampaignController : ApiController
    {
        static string path = "https://testapi.donatekart.com/api/campaign";
        static HttpClient client = new HttpClient();

        [Route("api/Campaign/ListAllCampaigns")]
        [HttpGet]
        [AllowAnonymous]

        public async Task<IHttpActionResult> ListAllCampaigns()
        {
            List<Campaign> campaigns = new List<Campaign>();
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                campaigns = JsonConvert.DeserializeObject<List<Campaign>>(jsonString).OrderByDescending(x => x.totalAmount).ToList();

                return Json(campaigns.Select(x => new { 
                    Title = x.title,
                    TotalAmount = x.totalAmount,
                    BackersCount = x.backersCount,
                    EndDate = x.endDate
                }));
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("api/Campaign/ActiveCampaigns")]
        [HttpGet]
        [AllowAnonymous]

        public async Task<IHttpActionResult> ActiveCampaigns()
        {
            List<Campaign> campaigns = new List<Campaign>();
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                campaigns = JsonConvert.DeserializeObject<List<Campaign>>(jsonString).Where(x => x.endDate.Date >= DateTime.Now.Date && x.created.Date >= DateTime.Now.AddDays(-30)).ToList();

                return Json(campaigns);
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("api/Campaign/ClosedCampaigns")]
        [HttpGet]
        [AllowAnonymous]

        public async Task<IHttpActionResult> ClosedCampaigns()
        {
            List<Campaign> campaigns = new List<Campaign>();
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                campaigns = JsonConvert.DeserializeObject<List<Campaign>>(jsonString).Where(x => x.endDate.Date < DateTime.Now.Date || x.procuredAmount >= x.totalAmount).ToList();

                return Json(campaigns);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}