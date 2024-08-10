using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using prjHw2new.Models;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;

namespace prjHw2new.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var products = db.TableBooks1091735.OrderBy(m => m.fBookISBN).ToList();
            var result = products.ToPagedList(currentPage, pageSize);
            return View(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TableBooks1091735 books)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                var temp = db.TableBooks1091735
                    .Where(m => m.fBookISBN == books.fBookISBN)
                    .FirstOrDefault();
                if (temp != null)
                {
                    ViewBag.Error = true;
                    return View(books);
                }
                db.TableBooks1091735.Add(books);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(books);
        }

        public ActionResult Edit(string fBookISBN)
        {
            var books = db.TableBooks1091735
                    .Where(m => m.fBookISBN == fBookISBN).FirstOrDefault();
            return View(books);
        }
        [HttpPost]
        public ActionResult Edit(TableBooks1091735 books)
        {
            if (ModelState.IsValid)
            {
                var temp = db.TableBooks1091735
                    .Where(m => m.fBookISBN == books.fBookISBN)
                    .FirstOrDefault();
                temp.fBookName = books.fBookName;
                temp.fBookAuthor = books.fBookAuthor;
                temp.fBookPublisher = books.fBookPublisher;
                temp.fPrice = books.fPrice;               
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(books);
        }

        public ActionResult Delete(string fBookISBN)
        {
            var books = db.TableBooks1091735
                .Where(m => m.fBookISBN == fBookISBN)
                .FirstOrDefault();
            db.TableBooks1091735.Remove(books);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Home
        [Authorize(Users = "jasper")]
        /*
        public string Index()
        {
            return "<p>" + User.Identity.Name +
                "您好，按 <a href='Home/Logout'>登出</a> 可進行登出系統<p>" +
                "<img src='../Images/banner01.jpg' width='100%'>";
        }
        */
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string txtUid, string txtPwd)
        {
            string[] uidAry = new string[] { "jasper" };
            string[] pwdAry = new string[] { "123" };

            int index = -1;
            for (int i = 0; i < uidAry.Length; i++)
            {
                if (uidAry[i] == txtUid && pwdAry[i] == txtPwd)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                ViewBag.Err = "帳密錯誤!";
            }
            else
            {
                FormsAuthentication.RedirectFromLoginPage(txtUid, true);

                Response.Cookies["Login.OK"].Value = "Yes";
                Session["Login.OK"] = "Yes";

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Recommend()
        {
            return View();
        }

        // GET: Home
        NorthwindEntities db = new NorthwindEntities();

        int pageSize = 10;
        // GET: Home
        
    }
}