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
    public class SubjectsController : Controller
    {
        private UetSurveyEntities db = new UetSurveyEntities();

        // GET: Subjects
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View(db.Subject.ToList());
            }
            return RedirectToAction("Login", "Users");
          
        }

        // GET: Subjects/Details/5
        public ActionResult ShowClass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IEnumerable<StudentDetail> listStudent = db.StudentDetail.Where(x => x.SubjectID == id);
            return View(listStudent);
           
        }
        public ActionResult ShowResultSurvey(int? id)
        {
            List<int> lis = db.StudentDetail.Where(x => x.SubjectID == id).Select(x => x.StudentDetailID).ToList();
            
            ViewBag.ListPointAver = db.Survey
                .Where(x => lis.Any(k => k == x.StudentDetailID))
                .GroupBy(x => x.ContentSurveyID)
                .Select(x => x.Average(y => y.Point)).ToList();
            ViewBag.hasSurvey = db.Survey.Where(x => lis.Any(k => k == x.StudentDetailID)).ToList().Count();
            ViewBag.NameSurvey = db.ContentSurvey.Select(x => x.Text).ToList();
            ViewBag.CountSurvey = db.ContentSurvey.ToList().Count();
            ViewBag.SumStudent = db.StudentDetail.Where(x => x.SubjectID == id).ToList().Count();
            StudentDetail student_Detail = db.StudentDetail.First(x => x.SubjectID == id);
            return View(student_Detail);
        }


        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subject.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subject.Find(id);
            db.Subject.Remove(subject);
            IEnumerable<StudentDetail> list = db.StudentDetail.Where(x => x.SubjectID == id);
            foreach (var item in list)
            {
                db.StudentDetail.Remove(item);
                IEnumerable<Survey> list2 = db.Survey.Where(x => x.StudentDetailID == item.StudentDetailID);
                try
                {
                    foreach (var item2 in list2)
                    {
                        db.Survey.Remove(item2);
                    }
                }
                catch (Exception)
                {

                }
               
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
