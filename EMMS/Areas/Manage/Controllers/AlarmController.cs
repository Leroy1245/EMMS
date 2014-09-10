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
    public class AlarmController : Controller
    {
        //
        // GET: /Manage/Alarm/
        DB db = new DB();
        [AuthorizeFilter]
        public ActionResult Index(string terId = "")
        {
            string nowTerId;
            EmdataSearchModel esm = new EmdataSearchModel();

            if (string.IsNullOrEmpty(terId))
            {
                nowTerId = db.Terminals.FirstOrDefault().TerminalID;
                var result = db.Alarms.Where(p => p.AlarmTermID == nowTerId).OrderByDescending(p=>p.AlarmDate).Include(p => p.Terminal);

                List<Alarm> ls = new List<Alarm>();
                foreach (var item in result)
                {
                    Alarm newAlarmItem = new Alarm();
                    newAlarmItem.AlarmContent = item.AlarmContent;
                    newAlarmItem.AlarmDate = item.AlarmDate;
                    newAlarmItem.AlarmID = item.AlarmID;
                    newAlarmItem.IsRead = item.IsRead;
                    newAlarmItem.Terminal = item.Terminal;
                    ls.Add(newAlarmItem);
                    item.IsRead = 1;
                }
                db.SaveChanges();
                ViewBag.alarmData = ls;
            }
            else
            {
                nowTerId = terId;
                var result = db.Alarms.Include(p => p.Terminal).Where(p => p.AlarmTermID == terId).OrderByDescending(p => p.AlarmDate);
                List<Alarm> ls = new List<Alarm>();
                foreach (var item in result)
                {
                    Alarm newAlarmItem = new Alarm();
                    newAlarmItem.AlarmContent = item.AlarmContent;
                    newAlarmItem.AlarmDate = item.AlarmDate;
                    newAlarmItem.AlarmID = item.AlarmID;
                    newAlarmItem.IsRead = item.IsRead;
                    newAlarmItem.Terminal = item.Terminal;
                    ls.Add(newAlarmItem);
                    item.IsRead = 1;
                }
                db.SaveChanges();
                ViewBag.alarmData = ls;
            }

            esm.TerminalID = nowTerId;
            esm.StartTime = new DateTime(2010, 1, 1);
            esm.EndTime = DateTime.Now;
            Bindddl();

            return View(esm);
        }

        [HttpPost]
        public ActionResult Index(EmdataSearchModel model)
        {
            if (this.ModelState.IsValid)
            {
                DateTime sTime = new DateTime(model.StartTime.Year, model.StartTime.Month, model.StartTime.Day, 0, 0, 0);
                DateTime eTime = new DateTime(model.EndTime.Year, model.EndTime.Month, model.EndTime.Day, 23, 59, 59);
                var result = db.Alarms.Where(p => p.AlarmDate >= sTime && p.AlarmDate <= eTime && p.AlarmTermID == model.TerminalID).OrderByDescending(p => p.AlarmDate).Include(p => p.Terminal);
                List<Alarm> ls = new List<Alarm>();
                foreach (var item in result)
                {
                    Alarm newAlarmItem = new Alarm();
                    newAlarmItem.AlarmContent = item.AlarmContent;
                    newAlarmItem.AlarmDate = item.AlarmDate;
                    newAlarmItem.AlarmID = item.AlarmID;
                    newAlarmItem.IsRead = item.IsRead;
                    newAlarmItem.Terminal = item.Terminal;
                    ls.Add(newAlarmItem);
                    item.IsRead = 1;
                }
                db.SaveChanges();
                ViewBag.alarmData = ls;
            }
            else
            {
                var result = db.Alarms.Where(p => p.AlarmTermID == model.TerminalID).OrderByDescending(p => p.AlarmDate).Include(p => p.Terminal);
                List<Alarm> ls = new List<Alarm>();
                foreach (var item in result)
                {
                    Alarm newAlarmItem = new Alarm();
                    newAlarmItem.AlarmContent = item.AlarmContent;
                    newAlarmItem.AlarmDate = item.AlarmDate;
                    newAlarmItem.AlarmID = item.AlarmID;
                    newAlarmItem.IsRead = item.IsRead;
                    newAlarmItem.Terminal = item.Terminal;
                    ls.Add(newAlarmItem);
                    item.IsRead = 1;
                }
                db.SaveChanges();
                ViewBag.alarmData = ls;
            }
            Bindddl();
            return View(model);
        }

        private void Bindddl()
        {
            var ddlTerminal = (from p in db.Terminals select p).ToList();
            var list = from p in ddlTerminal select new SelectListItem { Text = p.TerminalName, Value = p.TerminalID.ToString() };
            var TerminalList = list.ToList();
            if (TerminalList.Count() == 0)
            {
                TerminalList.Insert(0, new SelectListItem { Text = "无", Value = "0" });
            }
            ViewBag.TerList = TerminalList;
        }

    }
}
