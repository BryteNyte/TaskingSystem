using System.Linq.Dynamic;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskingSystem.Models;
using TaskingSystem.ViewModels;

namespace TaskingSystem.Controllers
{
    public class SuppliersController : Controller
    {
        SuppliersViewModel supVM = new SuppliersViewModel();
        public ActionResult Suppliers()
        {
            return View();
        }

        public async Task<ActionResult> GetSuppliers()
        {
            var useID = Session["useDetails"] as Users;
            var id = useID.useCompanyID;
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortCulmn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;
            var Name = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            List<SupplierModel> sup = new List<SupplierModel>();
            sup = await supVM.Get(id);
            if (!string.IsNullOrEmpty(Name))
            {
                sup = sup.Where(a => a.supName.ToLower().Contains(Name.ToLower())).ToList();
            }
            if (!(string.IsNullOrEmpty(sortCulmn + "" + sortColumnDir)))
            {
                sup = sup.OrderBy(sortCulmn + " " + sortColumnDir).ToList();
            }
            totalRecords = sup.Count();
            var data = sup.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddSupplier(SupplierModel sup)
        {
            if (ModelState.IsValid == false)
            {
                return View(sup);
            }
            else
            {
                var useID = Session["useDetails"] as Users;
                sup.supUser_CompanyID = useID.useCompanyID;
                var response = await supVM.AddSupplier(sup);
                if (response == true)
                {
                    return RedirectToAction("Supplier_Details");
                }
                else
                {
                    return View(sup);
                }

            }
        }

        [HttpGet]
        public async Task<ActionResult> Supplier_Details(int id)
        {
            SupplierModel sup = new SupplierModel();
            sup = await supVM.SupplierDetails(id);
            return View(sup);
        }

        // GET: Suppliers
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            DataTable dt = new DataTable();
            //check we have a file
            if (postedFile.ContentLength > 0)
            {
                //Workout our file path
                string fileName = Path.GetFileName(postedFile.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                //Try and upload
                try
                {
                    postedFile.SaveAs(path);

                    //Process the CSV file and capture the results to our DataTable place holder
                    dt = Test(path);
                    ViewData["dt"] = dt;
                }
                catch (Exception ex)
                {
                    //Catch errors
                    ViewData["Feedback"] = ex.Message;
                }
            }
            else
            {
                //Catch errors
                ViewData["Feedback"] = "Please select a file";
            }

            return View("Index", ViewData["Feedback"]);

        }

        public DataTable Test(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                // Read column headers from file
                CsvReader csv = new CsvReader(System.IO.File.OpenText(filePath), System.Globalization.CultureInfo.CurrentCulture);
                csv.Read();
                csv.ReadHeader();

                List<string> headers = csv.Context.HeaderRecord.ToList();

                // Read csv into datatable
                System.Data.DataTable dataTable = new System.Data.DataTable();
                List<Users> u  = new List<Users>();

                string[] split = new string[0];
                
                foreach (string header in headers)
                {
                     split = header.Split(';');
                }
                int i = 1;
                foreach (string item in split)
                {
                    i++;
                    u.Add(new Users
                    {
                        useID = i,
                        useName = item
                    });
                }

               
                
                return dataTable;

               
            }
            return null;

        }

        public DataTable csvToDataTable(string file, bool isRowOneHeader)
        {

            DataTable csvDataTable = new DataTable();

            //no try/catch - add these in yourselfs or let exception happen
            var csvData = System.IO.File.ReadAllLines(file);

            //if no data in file ‘manually’ throw an exception
            if (csvData.Length == 0)
            {
                throw new Exception("CSV File Appears to be Empty");
            }

            String[] headings = csvData[0].Split(',');
            int index = 0; //will be zero or one depending on isRowOneHeader


            DataTable heading = new DataTable();
            


            if (isRowOneHeader) //if first record lists headers
            {
                index = 1; //so we won’t take headings as data

                //for each heading
                for (int i = 0; i < headings.Length; i++)
                {
                    //replace spaces with underscores for column names
                    headings[i] = headings[i].Replace(" ", "_");

                    //add a column for each heading
                    csvDataTable.Columns.Add(headings[i], typeof(string));
                }
            }
            else //if no headers just go for col1, col2 etc.
            {
                for (int i = 0; i < headings.Length; i++)
                {
                    //create arbitary column names
                    csvDataTable.Columns.Add("col" + (i + 1).ToString(), typeof(string));
                }
            }

            //populate the DataTable
            for (int i = index; i < csvData.Length; i++)
            {
                //create new rows
                DataRow row = csvDataTable.NewRow();

                for (int j = 0; j < 1; j++)
                {
                    //fill them
                    row[j] = csvData[i].Split(',')[j];
                }

                //add rows to over DataTable
                csvDataTable.Rows.Add(row);
            }

            //return the CSV DataTable
            return csvDataTable;

        }
    }
}