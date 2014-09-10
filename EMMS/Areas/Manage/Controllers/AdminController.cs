using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMMS.Models;
using EMMS.Areas.Manage.Models.ViewModel;
using System.Data.Entity;
using EMMS.Areas.Manage.Filters;

namespace EMMS.Areas.Manage.Controllers
{
    public class AdminController : Controller
    {
        DB db = new DB();
        [AuthorizeFilter]
        public ActionResult Index(int id=1,string key="")
        {
            var result = db.Users.Where(p => p.Realname.Contains(key) || p.Username.Contains(key)).Include(p => p.MyTerminals).OrderByDescending(p => p.ID);
            return View(result);
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(Account account,string returnUrl="")
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Frame");
            }
            if (this.ModelState.IsValid)
            {
                var admin = db.Users.FirstOrDefault(p => p.Username == account.Username);
                if (admin == null) //判断用户是否存在
                {
                    this.ModelState.AddModelError("error","用户名不存在。");
                    return View(account);
                }

                if (admin.Userpw != System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(account.Userpw, "SHA1"))
                {
                    this.ModelState.AddModelError("error", "密码错误。");
                    return View(account);
                }
                Session["UserID"] = admin.ID;
                Session["UserName"] = admin.Username;
                Session["UserType"]=admin.Usertype;
                string userIP = Request.UserHostAddress;
                admin.Userip = userIP;
                db.SaveChanges();
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Frame");
                }
                else
                {
                    return Redirect("http://" + HttpContext.Request.Url.Host + ":" + HttpContext.Request.Url.Port.ToString() + "/" + returnUrl);
                }
            }
            return View();
        }

        [AuthorizeFilter]
        [UserTypeFilter]
        public ActionResult Create()
        {
            bindUserType();
            return View();
        }

        [UserTypeFilter]
        [HttpPost]
        public ActionResult Create(CreatUserModel model)
        {
            if (this.ModelState.IsValid)
            {
                User newUser = new User();
                UpdateModel(newUser);
                newUser.Userpw = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newUser.Userpw, "SHA1");
                db.Users.Add(newUser);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                this.ModelState.AddModelError("error", "未知错误");
            }
            bindUserType();
            return View();
        }


        private void bindUserType()
        {
            List<SelectListItem> sl = new List<SelectListItem>();
            sl.Insert(0,new SelectListItem { Text = "管理员", Value = "0" });
            sl.Insert(1,new SelectListItem { Text = "用户", Value = "1" });
            ViewBag.UserType=sl;
        }

        [AuthorizeFilter]
        public ActionResult chPwd()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult chPwd(chPwdModel model)
        {
            if (Session["UserID"] != null && this.ModelState.IsValid)
            {
                int nowUserID=int.Parse(Session["UserID"].ToString());
                var nowUser = db.Users.Where(p => p.ID == nowUserID ).FirstOrDefault();
                if (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(model.OldUserpw, "SHA1") == nowUser.Userpw)
                {
                    nowUser.Userpw = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(model.Userpw, "SHA1");
                    db.SaveChanges();
                    Response.Write("修改成功！");
                }
                else {
                    this.ModelState.AddModelError("error", "原密码错误");
                }
            }
            else {
                this.ModelState.AddModelError("error", "未知错误");
            }
            return View();
        }

        [AuthorizeFilter]
        [UserTypeFilter]
        public ActionResult delete(int id,string key)
        {
            var delUser=db.Users.FirstOrDefault(p => p.ID == id);
            db.Users.Remove(delUser);
            db.SaveChanges();
           
            return RedirectToAction("Index","Admin",new{ key=key });
        }

        [AuthorizeFilter]
        [UserTypeFilter]
        public ActionResult edit(int id)
        {
            
            var editUser = db.Users.FirstOrDefault(p => p.ID == id);
            if (editUser == null)
            {
                return RedirectToAction("Index");
            }
            editModel user=new editModel();
            user.Username=editUser.Username;
            user.Realname=editUser.Realname;
            user.Usertype=editUser.Usertype;
            bindUserType();
            return View(user);
        }

        [HttpPost]
        public ActionResult edit(int id,editModel model)
        {
            if (this.ModelState.IsValid)
            {
                var editUser = db.Users.FirstOrDefault(p => p.ID == id);
                editUser.Realname = model.Realname;
                editUser.Usertype = model.Usertype;
                db.SaveChanges();
            }
            else {
                return RedirectToAction("edit", new { id=id });
            }
            return RedirectToAction("index");
        }

        public ActionResult logOut()
        {
            Session["UserID"] = null;
            Session["UserType"] = null;
            Session["UserName"] = null;
            return RedirectToAction("SignIn","Admin");
        }

        public ActionResult IsExist(string Username)
        {
            var exist = db.Users.Count(p => p.Username == Username) > 0;
            return Json(!exist);
        }

        public ActionResult UserTypeNotSupport()
        {
            return View();
        }
    }
}
