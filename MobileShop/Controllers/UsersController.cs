using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileShop.Models;
namespace MobileShop.Controllers
{
     
    public class UsersController : Controller
    {
        DtMobileShopDataContext data = new DtMobileShopDataContext();
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection col, User u)
        {
            var name = col["userName"];
            var acc = col["userAcc"];
            var pwd1 = col["pwd"];
            var pwd2 = col["pwd2"];
            var email = col["email"];
            var add = col["address"];
            var phoneNum = col["phoneNum"];
            var cmnd = col["cmnd"];

            if (String.IsNullOrEmpty(acc))
            {
                ViewBag.accEr = "Hãy điền tài khoản";
            }
            else if (String.IsNullOrEmpty(pwd1))
            {
                ViewBag.pwd1Er = "Vui lòng nhập mật khẩu";
            }
            else if (String.Compare(pwd1, pwd2, false) != 0)
            {
                ViewBag.pwd2Er = "Không trùng khớp";
            }
            else if (String.IsNullOrEmpty(name))
            {
                ViewBag.nameEr = "Tên bạn là gì?";
            }
            else if (String.IsNullOrEmpty(phoneNum))
            {
                ViewBag.phoneNumEr = "Số điện thoại để mình liên lạc";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewBag.emailEr = "Mình cần email của bạn";
            }
            else
            {
                u.UserAccount = acc;
                u.UserPassword = pwd1;
                u.UserName = name;
                u.Email = email;
                u.NPhone = phoneNum;
                u.CMND = cmnd;
                u.UserAddress = add;
                u.IdRole = 1;
                data.Users.InsertOnSubmit(u);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }

            return this.Register();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection col)
        {
            var acc = col["account"];
            var pwd = col["pwd"];
            if (String.IsNullOrEmpty(acc))
            {
                ViewBag.Error1 = "Chưa nhập Tài khoản";
            }
            else if (String.IsNullOrEmpty(pwd))
            {
                ViewBag.Error2 = "Chưa nhập Mật khẩu";
            }
            else
            {
                User u = data.Users.SingleOrDefault(t => t.UserAccount == acc && t.UserPassword == pwd);
                if (u != null)
                {
                    if (u.IdRole == 2)
                    {
                        ViewBag.Alert = "Access";
                        ViewBag.AlertInf = " : Ok";
                        Session["Customer"] = u;
                        return RedirectToAction("Index", "NewHome");
                    }
                    else
                    {
                        ViewBag.WelcomeAdmin = "Admin " + u.UserName;
                        Session["AdminAccount"] = u;
                        Session["Customer"] = u;
                        return RedirectToAction("Index", "NewHome");
                    }
                }
                else
                {
                    ViewBag.Alert = "Warnning";
                    ViewBag.AlertInf = " : Sai Tài Khoản hoặc mật khẩu";
                }
            }
            return View();
        }
    }
}