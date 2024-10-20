using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking.Models;
using Newtonsoft.Json;
using System.Text;

namespace Parking.Controllers
{
    public class ParkController : Controller
    {
        // GET: ParkController
        public async Task<ActionResult> Index()
        {
            string apiUrl = "https://localhost:7291/api/park/displayparkedcars";

            List<Parking.Models.User> users = new List<Parking.Models.User>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                var result = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<Parking.Models.User>>(result);
            }

            return View(users);
        }

        // GET: ParkController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ParkController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParkController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            string apiUrl = "https://localhost:7291/api/park/carsthatwillpark";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }

        // GET: ParkController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ParkController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ParkController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParkController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> DictionaryLookup(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                ViewBag.ErrorMessage = "Please enter a word.";
                return View();
            }

            string apiUrl = $"https://api.dictionaryapi.dev/api/v2/entries/en/{word}";

            List<DictionaryResponseModel> dictionaryData = new List<DictionaryResponseModel>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    dictionaryData = JsonConvert.DeserializeObject<List<DictionaryResponseModel>>(result);
                }
                else
                {
                    ViewBag.ErrorMessage = "Word not found or an error occurred.";
                }
            }

            return View(dictionaryData.FirstOrDefault());
        }
    }
}
