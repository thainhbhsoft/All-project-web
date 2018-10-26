using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UET_BTL_VERSION_1.Models;

namespace UET_BTL_VERSION_1.Controllers
{
    public class SurveysController : Controller
    {
        private UetSurveyEntities db = new UetSurveyEntities();

        // GET: Surveys
        public ActionResult Index()
        {
            var survey = db.Survey.Include(s => s.ContentSurvey).Include(s => s.StudentDetail);
            return View(survey.ToList());
        }

        // GET: Surveys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Survey.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // GET: Surveys/Create
        public ActionResult Create()
        {
            ViewBag.ContentSurveyID = new SelectList(db.ContentSurvey, "ContentSurveyID", "Text");
            ViewBag.StudentDetailID = new SelectList(db.StudentDetail, "StudentDetailID", "NoteSurvey");
            return View();
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SurveyID,StudentDetailID,ContentSurveyID,Point")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Survey.Add(survey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContentSurveyID = new SelectList(db.ContentSurvey, "ContentSurveyID", "Text", survey.ContentSurveyID);
            ViewBag.StudentDetailID = new SelectList(db.StudentDetail, "StudentDetailID", "NoteSurvey", survey.StudentDetailID);
            return View(survey);
        }

        // GET: Surveys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Survey.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentSurveyID = new SelectList(db.ContentSurvey, "ContentSurveyID", "Text", survey.ContentSurveyID);
            ViewBag.StudentDetailID = new SelectList(db.StudentDetail, "StudentDetailID", "NoteSurvey", survey.StudentDetailID);
            return View(survey);
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SurveyID,StudentDetailID,ContentSurveyID,Point")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(survey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContentSurveyID = new SelectList(db.ContentSurvey, "ContentSurveyID", "Text", survey.ContentSurveyID);
            ViewBag.StudentDetailID = new SelectList(db.StudentDetail, "StudentDetailID", "NoteSurvey", survey.StudentDetailID);
            return View(survey);
        }

        // GET: Surveys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Survey.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Survey survey = db.Survey.Find(id);
            db.Survey.Remove(survey);
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
