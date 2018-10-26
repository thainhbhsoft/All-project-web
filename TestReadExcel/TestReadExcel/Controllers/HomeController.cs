using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;
using TestReadExcel.Models;

namespace TestReadExcel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index( HttpPostedFileBase fileUpload)
        {
            string message = string.Empty;
            int count= 0;
            var package = new ExcelPackage(fileUpload.InputStream);
            ImportData(out count, package);
            return View(message);
        }

        public bool ImportData(out int count, ExcelPackage package)
        {
            count = 0;
            var result = false;
            try
            {
                int startColumn = 1;
                int startRow = 4;
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                object data = null;
                TestExcelImportEntities db = new TestExcelImportEntities();

                do
                {
                    data = workSheet.Cells[startRow, startColumn].Value;
                    object studentName = workSheet.Cells[startRow, startColumn + 1].Value;
                    object studentCode = workSheet.Cells[startRow, startColumn + 2].Value;

                    if (data != null)
                    {
                        var isSuccess = SaveStudent(studentName.ToString(), studentCode.ToString(), db);
                        if (isSuccess)
                        {
                            count++;
                        }
                    }
                    startRow++;
                } while (data != null);
            }
            catch (Exception x)
            {

                throw;
            }
            return result;
        }
        public bool SaveStudent(string Name, string stuCode, TestExcelImportEntities db)
        {
            var result = false;
            try
            {
                if (db.Student.Where(x => x.Code.Equals(stuCode)).Count() == 0)
                {
                    var stu = new Student();
                    stu.Name = Name;
                    stu.Code = stuCode;
                    db.Student.Add(stu);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception x)
            {

            }
            return result;

        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }
    }
}