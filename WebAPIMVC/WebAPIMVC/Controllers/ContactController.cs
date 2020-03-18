using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIMVC.Models;

namespace WebAPIMVC.Controllers
{
    public class ContactController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAllContact()
        {
            IList<ContactViewModel> contacts = null;

            using (var cont = new ContactDatabaseEntities())
            {
                contacts = cont.Tables.Select(a => new ContactViewModel()
                {
                    ContactID = a.ContactID,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Status = a.Status
                }).ToList<ContactViewModel>();

                if (contacts.Count == 0)
                    return NotFound();
                return Ok(contacts);
            }
        }

        [HttpGet]
        public IHttpActionResult GetContactByID(int id)
        {

            ContactViewModel contact = null;
            using (var cont = new ContactDatabaseEntities())
            {
                contact = cont.Tables.Select(a => new ContactViewModel()
                {
                    ContactID=a.ContactID,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Status = a.Status
                }).Where(a => a.ContactID == id).FirstOrDefault<ContactViewModel>();

                if (contact == null)
                    return NotFound();
                return Ok(contact);
            }
        }

        [HttpPost]
        public IHttpActionResult PostContact(ContactViewModel contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data not formated");
            }
            using (var cnt = new ContactDatabaseEntities())
            {
                cnt.Tables.Add(new Table()
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Status = contact.Status,
                    PhoneNumber = contact.PhoneNumber,
                    Email = contact.Email,

                });
                cnt.SaveChanges();
            }
            return Ok();
        }


        [HttpPut]
        public IHttpActionResult PutContact(ContactViewModel contact)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new ContactDatabaseEntities())
            {
                var existingContact = ctx.Tables.Where(s => s.ContactID == contact.ContactID)
                                                        .FirstOrDefault<Table>();

                if (existingContact != null)
                {
                    existingContact.FirstName = contact.FirstName;
                    existingContact.LastName = contact.LastName;
                    existingContact.Status = contact.Status;
                    existingContact.Email = contact.Email;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }


        [HttpDelete]
        public IHttpActionResult DeleteContact(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new ContactDatabaseEntities())
            {
                var contact = ctx.Tables
                    .Where(s => s.ContactID == id)
                    .FirstOrDefault();

                ctx.Entry(contact).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
