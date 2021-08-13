using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project_4.Models;

namespace project_4.Controllers
{
    public class HomeController : Controller
    {
        GradContext db = new GradContext();
      
        public ActionResult Index()
        {
            DateTime d = DateTime.Now.AddMonths(-2);
            List<Drug> u = db.Drugs.Where(n=>n.Production_Date>d). ToList();
         
            return View(u);
        }

        public ActionResult About()
        {
               return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult register(User u, HttpPostedFileBase img)
        {
            if (img != null)
            {
                img.SaveAs(Server.MapPath($"~/attach/{img.FileName}"));
                u.UImage = img.FileName;
            }
            
            if (ModelState.IsValid)
            {
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("login");
            }
            else
            {
                ViewBag.status = "Invalid Data";
                return View();
            }
        }

        public ActionResult login()
        {
            if (Request.Cookies["full"] != null)
            {
                Session["user_ssn"] = Request.Cookies["full"].Values["ssn"];
                return RedirectToAction("profile");
            }
            return View();
        }


        //public ActionResult login()
        //{
        //    if (Request.Cookies["full"] != null)
        //    {
        //        Session["user_ssn"] = Request.Cookies["full"].Values["ssn"];
        //        return RedirectToAction("profile");
        //    }
        //    return View();
        //}

        [HttpPost]
        public ActionResult login([Bind(Include = "email,password")] User u, bool rememberme)
        {
            User us = db.Users.Where(n => n.Email == u.Email && n.Password == u.Password).SingleOrDefault();
            if (us != null)
            {
                if (rememberme)
                {
                    HttpCookie c = new HttpCookie("full");
                    c.Values.Add("ssn", us.SSN.ToString());
                    c.Values.Add("Name", us.UName);
                    c.Values.Add("Role", us.userRole);
                    c.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Add(c);
                    ViewBag.user = Request.Cookies["full"].Values["Name"];
                }
                
                Session.Add("user_ssn", us.SSN);
                Session.Add("user_uname", us.UName);
                Session.Add("user_role", us.userRole);
                ViewBag.user = Session["user_ssn"];
                return RedirectToAction("profile");
            }
            else
            {
                ViewBag.status = "Incorrect Email Or Password";
                return View();
            }       
        }


       

        public ActionResult profile()
        {
            if (Session["user_ssn"] != null)
            {
                Int64 ssn = Int64.Parse(Session["user_ssn"].ToString());
                User u = db.Users.Where(n => n.SSN == ssn).SingleOrDefault();
                List<Has_Drug> h = db.Has_Drug.Where(n => n.SSN == ssn).ToList();
                //int no = 12/ h.Count();
                //ViewBag.no = no;
                ViewBag.drugs = h;
                return View(u);
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public ActionResult editProfile(Int64 ssn)
        {
            User usr = db.Users.Where(n => n.SSN == ssn).SingleOrDefault();
            string pass = usr.Password;
            string path = usr.UImage;
            ViewBag.Path = path;
            ViewBag.pass = pass;
            return View(usr);
        }

        [HttpPost]
        public ActionResult editProfile(User u, HttpPostedFileBase img)
        {
            if (img != null)
            {
                img.SaveAs(Server.MapPath($"~/attach/{img.FileName}"));
                u.UImage = img.FileName;
            }
            if (ModelState.IsValid)
            {
                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            else
            {
                return View();
            }
        }

        public ActionResult logout()
        {
            Session["user_ssn"] = null;
            HttpCookie c = new HttpCookie("full");
            c.Expires = DateTime.Now.AddDays(-15);
            Response.Cookies.Add(c);
            return RedirectToAction("login");
        }


        //[Authorize(Session["user_role"] = "Admin")]

        //[Authorize(Roles = "Admins")]
        public ActionResult allUsers()
        {
            List<User> u = db.Users.ToList();
            return View(u);
        }


        public ActionResult deleteUser(Int64 ssn)
        {
            User u = db.Users.Where(n => n.SSN == ssn).SingleOrDefault();
            db.Users.Remove(u);
            db.SaveChanges();
            return RedirectToAction("allUsers");
        }

        public ActionResult detailsUser(Int64 ssn)
        {
            User us = db.Users.Where(n => n.SSN == ssn).SingleOrDefault();
            return View(us);
        }

        public ActionResult editRole(Int64 ssn)
        {
            User usr = db.Users.Where(n => n.SSN == ssn).SingleOrDefault();
            string pass = usr.Password;
            string path = usr.UImage;
            ViewBag.Path = path;
            ViewBag.pass = pass;
            return View(usr);
        }


        [HttpPost]
        public ActionResult editRole([Bind(Exclude = "UImage,Password,confirm_Password")] User u)
        {
            if (ModelState.IsValid)
            {
                db.Entry(u).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("allUsers");
            }
            else
            {
                return View();
            }
        }

        public ActionResult allDrugs()
        {
            List<Drug> d = db.Drugs.ToList();
            return View(d);
        }
        public ActionResult Drugs(string Department)
        {
            List<Drug> d = db.Drugs.Where(n => n.Department == Department).ToList();
            ViewBag.dept = d[0].Department;
            return View(d);
        }

        //public ActionResult Drugs(int DID)
        //{
        //    Drug d = db.Drugs.Where(n => n.DID==DID).SingleOrDefault();
        //    return View(d);
        //}

        public ActionResult detailsDrug(int id)
        {
            return View(db.Drugs.Where(x => x.DID == id).FirstOrDefault());
        }

        public ActionResult Exchange2(int id)
        {
            if (Session["user_ssn"] == null)
            {
                return RedirectToAction("login");
            } else
            {

            Drug d = db.Drugs.Where(n => n.DID == id).SingleOrDefault();
            d.Dstate = "Reserved";
            db.Entry(d).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("allDrugs");
            }
        }

        public ActionResult approveRequest (int id)
        {
            Drug d = db.Drugs.Where(n => n.DID == id).SingleOrDefault();
            d.Dstate = "Approved";
            db.Entry(d).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("profile");
        }
        public ActionResult denyRequest(int id)
        {
            Drug d = db.Drugs.Where(n => n.DID == id).SingleOrDefault();
            d.Dstate = "Available";
            db.Entry(d).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("profile");
        }

        //public ActionResult Exchange(int id)
        //{
        //    Drug d = db.Drugs.Where(n => n.DID == id).SingleOrDefault();
        //    //Int64 ussn = Int64.Parse(Session["user_ssn"].ToString());
        //    //Has_Drug dm = db.Has_Drug.Where(n => n.DID==id && n.SSN == ussn).SingleOrDefault();
        //    //ViewBag.amount = dm.Amount;
        //    return View(d);
        //}
        //[HttpPost]
        //public ActionResult Exchange(Drug dg, HttpPostedFileBase img, int id)
        //{
        //    if (img != null)
        //    {
        //        img.SaveAs(Server.MapPath($"~/attach/{img.FileName}"));
        //        dg.DImage = img.FileName;
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        //Int64 ussn = Int64.Parse(Session["user_ssn"].ToString());
        //        //Has_Drug hd = db.Has_Drug.Where(n => n.DID == id && n.SSN == ussn).SingleOrDefault();
        //        //hd.Amount = amount;
        //        //db.Entry(hd).State = System.Data.Entity.EntityState.Modified;
        //        db.Entry(dg).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("allDrugs");
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}


        public ActionResult search(string searching)
        {
            List<Drug> d = db.Drugs.Where(n => n.DName.Contains(searching) || n.Department.Contains(searching) || searching == null).ToList();
            ViewBag.dept = d[0].Department;
            return View(d);
        }

        public ActionResult deleteDrug(int id)
        {

            Drug d = db.Drugs.Where(n => n.DID == id).SingleOrDefault();
            db.Drugs.Remove(d);
            db.SaveChanges();
            return RedirectToAction("Profile");
        }

        public ActionResult editDrug(int id)
        {
            Drug d = db.Drugs.Where(n => n.DID == id).SingleOrDefault();
            //Has_Drug dm = db.Has_Drug.Where(n=>n.DID==id && n.SSN == (Int64)Session["user_ssn"]).SingleOrDefault();
            Int64 ussn = Int64.Parse(Session["user_ssn"].ToString());
            Has_Drug dm = db.Has_Drug.Where(n=>n.DID==id  &&  n.SSN == ussn).SingleOrDefault();
            ViewBag.amount = dm.Amount;
            return View(d);
        }
        [HttpPost]
        public ActionResult editDrug(Drug dg, HttpPostedFileBase img, int amount, int id)
        {
            if (img != null)
            {
                img.SaveAs(Server.MapPath($"~/attach/{img.FileName}"));
                dg.DImage = img.FileName;
            }
            if (ModelState.IsValid) 
            {
                Int64 ussn = Int64.Parse(Session["user_ssn"].ToString());
                Has_Drug hd = db.Has_Drug.Where(n => n.DID == id && n.SSN == ussn).SingleOrDefault();
                hd.Amount = amount;
                db.Entry(hd).State = System.Data.Entity.EntityState.Modified;
                db.Entry(dg).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            else
            {
                  return View();
            }
        }

        public ActionResult addDrug()
        {
            if (Session["user_ssn"] == null)
            {
                return RedirectToAction("login");
            }
            
                return View();
        }
        [HttpPost]
        public ActionResult addDrug(Drug d, HttpPostedFileBase img, int amount) {
            if (img != null)
            {
                img.SaveAs(Server.MapPath($"~/attach/{img.FileName}"));
                d.DImage = img.FileName;
            }
            Has_Drug ux = new Has_Drug() { DID = d.DID, SSN = (Int64)Session["user_ssn"], Amount = amount };            
                if (ModelState.IsValid)
                {
                    db.Drugs.Add(d);
                db.Has_Drug.Add(ux);
                db.SaveChanges();
                
                    return RedirectToAction("profile");
                }
                else
                {
                    return View();
                }
            
        }
    }
}