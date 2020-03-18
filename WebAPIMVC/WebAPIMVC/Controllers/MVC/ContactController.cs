using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebAPIMVC.Models;

namespace WebAPIMVC.Controllers.MVC
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return RedirectToAction("Contact");
        }

        public ActionResult Contact()
        {
            IEnumerable<ContactViewModel> contacts = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53741/api/");
                var responseTask = client.GetAsync("contact");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ContactViewModel>>();
                    readTask.Wait();
                    contacts = readTask.Result;
                }
                else
                {
                    contacts = Enumerable.Empty<ContactViewModel>();
                    ModelState.AddModelError("Exception", "APi issue");
                }
            }
            return View(contacts);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(ContactViewModel contact)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53741/api/contact");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ContactViewModel>("contact", contact);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(contact);
        }

        public ActionResult Edit(int id)
        {
            ContactViewModel contact = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53741/api/");
                //HTTP GET
                var responseTask = client.GetAsync("contact?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ContactViewModel>();
                    readTask.Wait();
                    contact = readTask.Result;
                }
            }

            return View(contact);
        }

        [HttpPost]
        public ActionResult Edit(ContactViewModel contact)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53741/api/contact");

                var putTask = client.PutAsJsonAsync<ContactViewModel>("contact", contact);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(contact);
        }


        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53741/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("contact/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}