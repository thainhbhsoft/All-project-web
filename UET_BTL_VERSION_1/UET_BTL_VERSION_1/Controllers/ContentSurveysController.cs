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
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string text = form["Text"].ToString();
            if (db.ContentSurveys.Any(x => x.Text.Equals(text)))
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ContentSurvey content = new ContentSurvey();
                content.Text = text;
                db.ContentSurveys.Add(content);
                db.SaveChanges();
                int lastId = db.ContentSurveys.Max(s => s.ContentSurveyID);
                return Json(new { status = 1, content = content, id = lastId }, JsonRequestBehavior.AllowGet);
            }
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
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            db.Configuration.ProxyCreationEnabled = false;
            string text = form["Text"].ToString();
            int idContentSurvey = int.Parse(form["idContentSurvey"].ToString());
            if (db.ContentSurveys.Where(x => x.Text.Equals(text) && x.ContentSurveyID != idContentSurvey).ToList().Count() > 0)
            {
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ContentSurvey content = db.ContentSurveys.FirstOrDefault(x => x.ContentSurveyID == idContentSurvey);
                content.Text = text;
                db.SaveChanges();
                return Json(new { status = 2 }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: ContentSurveys/Delete/5
        public ActionResult Delete(int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentSurvey contentSurvey = db.ContentSurveys.Find(id);
            if (contentSurvey == null)
            {
                return HttpNotFound();
            }
            return Json(contentSurvey, JsonRequestBehavior.AllowGet);
        }
        // POST: ContentSurveys/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
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
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
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
