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
    public class EmdataManageController : Controller
    {
        DB db = new DB();
        [AuthorizeFilter]
        public ActionResult Index(string terId = "")
        {
            string nowTerId;
            EmdataSearchModel esm = new EmdataSearchModel();

            if (string.IsNullOrEmpty(terId))
            {
                nowTerId = db.Terminals.FirstOrDefault().TerminalID;
                var result = db.EMdatas.Where(p => p.TerminalID == nowTerId).OrderByDescending(p=>p.Update).Include(p => p.Terminal);
                ViewBag.emData = result;
            }
            else
            {
                nowTerId = terId;
                var result = db.EMdatas.Include(p => p.Terminal).Where(p => p.TerminalID == terId).OrderByDescending(p => p.Update);
                ViewBag.emData = result;
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
                var result = db.EMdatas.Where(p => p.Update >= sTime && p.Update <= eTime && p.TerminalID == model.TerminalID).OrderByDescending(p => p.Update).Include(p => p.Terminal);
                ViewBag.emData = result;
            }
            else
            {
                var result = db.EMdatas.Where(p => p.TerminalID == model.TerminalID).Include(p => p.Terminal).OrderByDescending(p => p.Update);
                ViewBag.emData = result;
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

        [AuthorizeFilter]
        public ActionResult chart(string terId = "")
        {
            string nowTerId;
            EmdataSearchModel esm = new EmdataSearchModel();

            if (string.IsNullOrEmpty(terId))
            {
                nowTerId = db.Terminals.FirstOrDefault().TerminalID;
                var result = db.EMdatas.Where(p => p.TerminalID == nowTerId).OrderBy(p => p.Update).Include(p => p.Terminal);
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                ViewBag.recentData = recent;
                ViewBag.emData = result;
            }
            else
            {
                nowTerId = terId;
                var result = db.EMdatas.Include(p => p.Terminal).Where(p => p.TerminalID == terId);
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                ViewBag.recentData = recent;
                ViewBag.emData = result;
            }

            esm.TerminalID = nowTerId;
            esm.StartTime = new DateTime(2010, 1, 1);
            esm.EndTime = DateTime.Now;
            Bindddl();

            return View(esm);
        }

        [HttpPost]
        public ActionResult chart(EmdataSearchModel model)
        {
            if (this.ModelState.IsValid)
            {
                DateTime sTime = new DateTime(model.StartTime.Year, model.StartTime.Month, model.StartTime.Day, 0, 0, 0);
                DateTime eTime = new DateTime(model.EndTime.Year, model.EndTime.Month, model.EndTime.Day, 23, 59, 59);
                var result = db.EMdatas.Where(p => p.Update >= sTime && p.Update <= eTime && p.TerminalID == model.TerminalID).Include(p => p.Terminal);
                if (result.Count() == 0)
                {
                    return View("HaveNoData");
                }
                ViewBag.emData = result;
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                ViewBag.recentData = recent;
            }
            else
            {
                var result = db.EMdatas.Where(p => p.TerminalID == model.TerminalID).Include(p => p.Terminal);
                ViewBag.emData = result;
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                ViewBag.recentData = recent;
            }
            Bindddl();
            return View(model);
        }

        [AuthorizeFilter]
        public ActionResult dayChart(string terId = "")
        {
            string nowTerId;
            EmdataSearchModel esm = new EmdataSearchModel();

            if (string.IsNullOrEmpty(terId))
            {
                nowTerId = db.Terminals.FirstOrDefault().TerminalID;
                var result = db.EMdatas.Where(p => p.TerminalID == nowTerId).OrderBy(p => p.Update).Include(p => p.Terminal);
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                if (result.Count() == 0)
                {
                    return View("HaveNoData");
                }
                DateTime sDate = result.FirstOrDefault().Update;
                DateTime rDate = recent.Update;
                DateTime i = sDate;
                DateTime a = sDate;
                List<EMdata> ls = new List<EMdata>();
                int count = 0;
                for (; i <= rDate; i = i.AddDays(1))
                {
                    EMdata em = new EMdata();
                    em.Update = new DateTime(i.Year, i.Month, i.Day, 12, 0, 0);
                    em.GasIntensity = em.UV = em.Luminance = em.Pm25 = 0;
                    em.Temperature = em.Humidity = 0;
                    foreach (var item in result)
                    {
                        em.TerminalID = item.TerminalID;
                        em.Terminal = item.Terminal;
                        TimeSpan ts = item.Update - i;
                        if (ts.Days >= 0 && ts.Days < 1)
                        {
                            em.Temperature += item.Temperature;
                            em.Humidity += item.Humidity;
                            em.Luminance += item.Luminance;
                            em.Pm25 += item.Pm25;
                            em.UV += item.UV;
                            em.GasIntensity += item.GasIntensity;
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        continue;
                    }
                    em.Temperature = em.Temperature / count;
                    em.Humidity = em.Humidity / count;
                    em.Luminance = em.Luminance / count;
                    em.Pm25 = em.Pm25 / count;
                    em.UV = em.UV / count;
                    em.GasIntensity = em.GasIntensity / count;
                    ls.Add(em);
                    count = 0;
                }

                ViewBag.recentData = ls.LastOrDefault();
                ViewBag.emData = ls;
            }
            else
            {
                nowTerId = terId;
                var result = db.EMdatas.Include(p => p.Terminal).Where(p => p.TerminalID == terId);
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                ViewBag.recentData = recent;
                ViewBag.emData = result;
            }

            esm.TerminalID = nowTerId;
            esm.StartTime = new DateTime(2010, 1, 1);
            esm.EndTime = DateTime.Now;
            Bindddl();

            return View(esm);
        }

        [HttpPost]
        public ActionResult dayChart(EmdataSearchModel model)
        {
            if (this.ModelState.IsValid)
            {
                DateTime sTime = new DateTime(model.StartTime.Year, model.StartTime.Month, model.StartTime.Day, 0, 0, 0);
                DateTime eTime = new DateTime(model.EndTime.Year, model.EndTime.Month, model.EndTime.Day, 23, 59, 59);
                var result = db.EMdatas.Where(p => p.Update >= sTime && p.Update <= eTime && p.TerminalID == model.TerminalID).Include(p => p.Terminal);
                if (result.Count() == 0)
                {
                    return View("HaveNoData");
                }
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                DateTime sDate = result.FirstOrDefault().Update;
                DateTime rDate = recent.Update;
                DateTime i = sDate;
                DateTime a = sDate;
                List<EMdata> ls = new List<EMdata>();
                int count = 0;
                for (; i <= rDate; i = i.AddDays(1))
                {
                    EMdata em = new EMdata();
                    em.Update = new DateTime(i.Year, i.Month, i.Day, 12, 0, 0);
                    em.GasIntensity = em.UV = em.Luminance = em.Pm25 = 0;
                    em.Temperature = em.Humidity = 0;
                    foreach (var item in result)
                    {
                        em.TerminalID = item.TerminalID;
                        em.Terminal = item.Terminal;
                        TimeSpan ts = item.Update - i;
                        if (ts.Days >= 0 && ts.Days < 1)
                        {
                            em.Temperature += item.Temperature;
                            em.Humidity += item.Humidity;
                            em.Luminance += item.Luminance;
                            em.Pm25 += item.Pm25;
                            em.UV += item.UV;
                            em.GasIntensity += item.GasIntensity;
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        continue;
                    }
                    em.Temperature = em.Temperature / count;
                    em.Humidity = em.Humidity / count;
                    em.Luminance = em.Luminance / count;
                    em.Pm25 = em.Pm25 / count;
                    em.UV = em.UV / count;
                    em.GasIntensity = em.GasIntensity / count;
                    ls.Add(em);
                    count = 0;
                }

                ViewBag.recentData = ls.LastOrDefault();
                ViewBag.emData = ls;
            }
            else
            {
                var result = db.EMdatas.Where(p => p.TerminalID == model.TerminalID).Include(p => p.Terminal);
                var recent = result.OrderByDescending(p => p.Update).FirstOrDefault();
                DateTime sDate = result.FirstOrDefault().Update;
                DateTime rDate = recent.Update;
                DateTime i = sDate;
                DateTime a = sDate;
                List<EMdata> ls = new List<EMdata>();
                int count = 0;
                for (; i <= rDate; i = i.AddDays(1))
                {
                    EMdata em = new EMdata();
                    em.Update = new DateTime(i.Year, i.Month, i.Day, 12, 0, 0);
                    em.GasIntensity = em.UV = em.Luminance = em.Pm25 = 0;
                    em.Temperature = em.Humidity = 0;
                    foreach (var item in result)
                    {
                        em.TerminalID = item.TerminalID;
                        em.Terminal = item.Terminal;
                        TimeSpan ts = item.Update - i;
                        if (ts.Days >= 0 && ts.Days < 1)
                        {
                            em.Temperature += item.Temperature;
                            em.Humidity += item.Humidity;
                            em.Luminance += item.Luminance;
                            em.Pm25 += item.Pm25;
                            em.UV += item.UV;
                            em.GasIntensity += item.GasIntensity;
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        continue;
                    }
                    em.Temperature = em.Temperature / count;
                    em.Humidity = em.Humidity / count;
                    em.Luminance = em.Luminance / count;
                    em.Pm25 = em.Pm25 / count;
                    em.UV = em.UV / count;
                    em.GasIntensity = em.GasIntensity / count;
                    ls.Add(em);
                    count = 0;
                }

                ViewBag.recentData = ls.LastOrDefault();
                ViewBag.emData = ls;
            }
            Bindddl();
            return View(model);
        }


        

        [HttpPost]
        public ActionResult Receive(string terID, double tem, double humi, int pm25, int lum, int uv, int gas, double votal)
        {
            var terminal = db.Terminals.FirstOrDefault(p => p.TerminalID == terID);
            if (terminal == null)
            {
                Response.Write("Is not exist this terminal:TerminalID=" + terID);
                return View();
            }
            EMdata newData = new EMdata();
            newData.TerminalID = terID;
            newData.Temperature = tem;
            newData.Humidity = humi;
            newData.Pm25 = pm25;
            newData.Luminance = lum;
            newData.UV = uv;
            newData.GasIntensity = gas;
            newData.Update = DateTime.Now;
            db.EMdatas.Add(newData);
            terminal.TerminalVotal = votal;
            terminal.TerminalConnect = 1;
            db.SaveChanges();

            string alarmContent = "";
            int isAlarm = 0;
            if (tem >= terminal.TemMax)
            {
                isAlarm = 1;
                alarmContent += "温度超过警戒值；  ";
            }
            if (humi >= terminal.HumiMax)
            {
                isAlarm = 1;
                alarmContent += "湿度超过警戒值；  ";
            }
            if (pm25 >= terminal.Pm25Max)
            {
                isAlarm = 1;
                alarmContent += "PM2.5超过警戒值；  ";
            }
            if (lum >= terminal.LuminMax)
            {
                isAlarm = 1;
                alarmContent += "亮度超过警戒值；  ";
            }
            if (uv >= terminal.UVMax)
            {
                isAlarm = 1;
                alarmContent += "紫外强度超过警戒值；  ";
            }
            if (gas >= terminal.GasMax)
            {
                isAlarm = 1;
                alarmContent += "可燃气体浓度超过警戒值；  ";
            }
            if (isAlarm == 1)
            {
                Alarm newAlarm = new Alarm();
                newAlarm.AlarmContent = alarmContent;
                newAlarm.AlarmTermID = terminal.TerminalID;
                newAlarm.AlarmDate = DateTime.Now;
                newAlarm.IsRead = 0;
                db.Alarms.Add(newAlarm);
                db.SaveChanges();
                Response.Write("Upload succesful.Having alarm.");
            }
            else
            {
                Response.Write("Upload succesful");
            }
            return View();
        }

        public JsonResult Get(string terId)
        {
            var result = db.EMdatas.Where(p => p.TerminalID == terId).OrderByDescending(p => p.Update);
            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}
