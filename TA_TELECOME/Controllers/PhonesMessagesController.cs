using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TA_TELECOME.Models;

namespace TA_TELECOME.Controllers
{
    [Authorize]
    public class PhonesMessagesController : Controller
    {
        private DB db = new DB();

        // GET: PhonesMessages
        public ActionResult Index()
        {
            var phonesMessages = db.PhonesMessages.Include(p => p.Phone);
            return View(phonesMessages.ToList());
        }

        // GET: PhonesMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhonesMessage phonesMessage = db.PhonesMessages.Find(id);
            if (phonesMessage == null)
            {
                return HttpNotFound();
            }
            return View(phonesMessage);
        }

        // GET: PhonesMessages/Create
        public ActionResult Create() 
        { 
            ViewBag.PhoneList = db.Phones.Where(x=>x.SentAt == null).Take(10).ToList();
            return View();
        }

        // POST: PhonesMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhonesMessage phonesMessage,int[] PhoneId0)
        {
            if (ModelState.IsValid)
            {  
                foreach (var item in PhoneId0)
                {
                    db.PhonesMessages.Add(new PhonesMessage { PhoneId = item });
                    phonesMessage.SendAt = DateTime.Now.ToString("dd/MM/yyyy");
                    phonesMessage.MessageText = phonesMessage.MessageText;
                }
                db.PhonesMessages.Add(phonesMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhoneId = new SelectList(db.Phones, "Id", "PhoneNumberOwnerName", phonesMessage.PhoneId);
            return View(phonesMessage);
        }

        // GET: PhonesMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhonesMessage phonesMessage = db.PhonesMessages.Find(id);
            if (phonesMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhoneId = new SelectList(db.Phones, "Id", "PhoneNumberOwnerName", phonesMessage.PhoneId);
            return View(phonesMessage);
        }

        // POST: PhonesMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PhoneId,MessageText,SendAt")] PhonesMessage phonesMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phonesMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhoneId = new SelectList(db.Phones, "Id", "PhoneNumberOwnerName", phonesMessage.PhoneId);
            return View(phonesMessage);
        }

        // GET: PhonesMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhonesMessage phonesMessage = db.PhonesMessages.Find(id);
            if (phonesMessage == null)
            {
                return HttpNotFound();
            }
            return View(phonesMessage);
        }

        // POST: PhonesMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhonesMessage phonesMessage = db.PhonesMessages.Find(id);
            db.PhonesMessages.Remove(phonesMessage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
