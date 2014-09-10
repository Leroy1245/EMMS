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
    public class TerminalController : Controller
    {
        DB db = new DB();
        [AuthorizeFilter]
        public ActionResult Index(int? userId,int id = 1, string key = "")
        {
            if (userId == null)
            {
                var result = db.Terminals.Where(p => p.TerminalName.Contains(key) || p.TerminalAddr.Contains(key)).Include(p => p.User).Include(p=>p.Emdatas).OrderBy(p => p.TerminalID);
                foreach (var item in result)
                {
                    if (item.Emdatas.Count() == 0)
                    {
                        item.TerminalConnect = 0;
                        continue;
                    }
                    DateTime lastUpTime = new DateTime(1900, 1, 1);
                    if (item.Emdatas.Count() != 0) lastUpTime = item.Emdatas.OrderByDescending(p => p.Update).FirstOrDefault().Update;
                    TimeSpan ts=DateTime.Now-lastUpTime;
                    if (ts.Days>0 || (ts.Days==0 && ts.Hours>0) || (ts.Days==0 && ts.Hours==0 && ts.Minutes>10))
                    {
                        item.TerminalConnect = 0;
                    }
                }
                db.SaveChanges();
                return View(result);
            }
            else
            {
                var result = db.Terminals.Where(p => (p.TerminalName.Contains(key) || p.TerminalAddr.Contains(key)) && p.TerminalUserID == userId).Include(p => p.User).Include(p => p.Emdatas).OrderByDescending(p => p.TerminalID);
                foreach (var item in result)
                {
                    DateTime lastUpTime = new DateTime(1900, 1, 1);
                    if (item.Emdatas.Count()!=0) lastUpTime = item.Emdatas.OrderByDescending(p => p.Update).FirstOrDefault().Update;
                    TimeSpan ts = DateTime.Now - lastUpTime;
                    if (ts.Days > 0 || (ts.Days == 0 && ts.Hours > 0) || (ts.Days == 0 && ts.Hours == 0 && ts.Minutes > 10))
                    {
                        item.TerminalConnect = 0;
                    }
                }
                db.SaveChanges();
                return View(result);
            }
        }

        [AuthorizeFilter]
        public ActionResult Create()
        {
            Terminal model = new Terminal();
            model.TemMax = model.HumiMax = 9999;
            model.Pm25Max = model.LuminMax = model.UVMax = model.GasMax = 9999;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Terminal model)
        {
            if (this.ModelState.IsValid)
            {
                string terID="";
                Random rdn = new Random();
                int count = 1;
                while (count != 0)
                {
                    int rint = rdn.Next(0, 9999);
                    terID = "EMMS" + rint;
                    count = db.Terminals.Where(p => p.TerminalID == terID).Count();
                }
                Terminal newTer = new Terminal();
                UpdateModel(newTer);
                newTer.TerminalID = terID;
                newTer.TerminalUserID =int.Parse(Session["UserID"].ToString());

                db.Terminals.Add(newTer);
                db.SaveChanges();
            }
            return RedirectToAction("Index","Terminal");
        }

        [AuthorizeFilter]
        public ActionResult Edit(string terID)
        {
            if (string.IsNullOrEmpty(terID))
            {
                return RedirectToAction("Index");
            }
            else {
                var editTer = db.Terminals.FirstOrDefault(p => p.TerminalID == terID);
                if (editTer == null) return RedirectToAction("Index");
                editTerminalModel nowTer = new editTerminalModel();
                nowTer.TerminalID = editTer.TerminalID;
                nowTer.TerminalName = editTer.TerminalName;
                nowTer.TerminalAddr = editTer.TerminalAddr;
                nowTer.TemMax = editTer.TemMax;
                nowTer.HumiMax = editTer.HumiMax;
                nowTer.Pm25Max = editTer.Pm25Max;
                nowTer.LuminMax = editTer.LuminMax;
                nowTer.UVMax = editTer.UVMax;
                nowTer.GasMax = editTer.GasMax;
                return View(nowTer);
            }
        }

        [HttpPost]
        public ActionResult Edit(editTerminalModel model)
        {
            if (this.ModelState.IsValid)
            {
                var editTer = db.Terminals.FirstOrDefault(p => p.TerminalID == model.TerminalID);
                if (editTer == null)
                {
                    return RedirectToAction("Index");
                }
                UpdateModel(editTer);
                db.SaveChanges();
                return RedirectToAction("Index", "Terminal");
            }
            else { 
                this.ModelState.AddModelError("error","未知错误");
                return View(model);
            }
        }

        [AuthorizeFilter]
        public ActionResult delete(string terID,string key="")
        {
            if (string.IsNullOrEmpty(terID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                var delTer = db.Terminals.FirstOrDefault(p => p.TerminalID == terID);
                if (delTer == null)
                {
                    return RedirectToAction("Index");
                }
                db.Terminals.Remove(delTer);
                db.SaveChanges();
                return RedirectToAction("Index", "Terminal", new { key = key });
            }
        }

    }
}
