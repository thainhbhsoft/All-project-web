using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model;
using UET_BTL.Model.Entities;

namespace UET_BTL_VERSION_1.Controllers
{
    public class ContentSurveysController : Controller
    {
        private UetSurveyDbContext db = new UetSurveyDbContext();
        // GET: ContentSurveys
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View(db.ContentSurveys.ToList());
            }
            return RedirectToAction("Login", "Users");
           
        }
        // GET: ContentSurveys/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: ContentSurveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContentSurveyID,Text")] ContentSurvey contentSurvey)
        {
            if (ModelState.IsValid)
            {
                db.Surveys.RemoveRange(db.Surveys);
                db.ContentSurveys.Add(contentSurvey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contentSurvey);
        }
        // GET: ContentSurveys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentSurvey contentSurvey = db.ContentSurveys.Find(id);
            if (contentSurvey == null)
            {
                return HttpNotFound();
            }
            return View(contentSurvey);
        }
        // POST: ContentSurveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContentSurveyID,Text")] ContentSurvey contentSurvey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contentSurvey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contentSurvey);
        }
        // GET: ContentSurveys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentSurvey contentSurvey = db.ContentSurveys.Find(id);
            if (contentSurvey == null)
            {
                return HttpNotFound();
            }
            return View(contentSurvey);
        }
        // POST: ContentSurveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContentSurvey contentSurvey = db.ContentSurveys.Find(id);
            db.ContentSurveys.Remove(contentSurvey);
            try
            {
                IEnumerable<Survey> listsurvey = db.Surveys.Where(x => x.ContentSurveyID == id);
                db.Surveys.RemoveRange(listsurvey);
            }
            catch (Exception)
            {
            }
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
