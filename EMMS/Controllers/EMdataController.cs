using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMMS.Models;
using System.Data.Entity;

namespace EMMS.Controllers
{
    public class EMdataController : Controller
    {
        DB db = new DB();
        public ActionResult Index(int id=1,string terminal="EMMS001")
        {
            Bindddl();
            ViewBag.TerminalID = terminal;
            var result = db.EMdatas.Where(p=>p.TerminalID==terminal).Include(p=>p.Terminal).OrderBy(p=>p.Update);
            var bindColor = (IEnumerable<EMdata>)result;
            EMdata now = new EMdata();
            if (bindColor.LastOrDefault() != null)
            {
                now.Temperature = bindColor.LastOrDefault().Temperature;
                now.Pm25 = bindColor.LastOrDefault().Pm25;
                now.Luminance = bindColor.LastOrDefault().Luminance;
                now.Humidity = bindColor.LastOrDefault().Humidity;
                now.UV = bindColor.LastOrDefault().UV;
                now.GasIntensity = bindColor.LastOrDefault().GasIntensity;
                BindColor(now);
            }
            if (result.Count() == 0)
            {
                EMdata em = new EMdata();
                Terminal term = new Terminal();
                term.TerminalName = "无";
                term.TerminalAddr = "无";
                em.Terminal = term;
                em.UV = em.Pm25 = em.Luminance = em.GasIntensity = -1;
                em.Humidity = em.Temperature = -1;
                List<EMdata> emlst=new List<EMdata>();
                emlst.Add(em);
                return View(emlst);
            }
            return View(result);
        }

        public ActionResult Month(string terminal = "EMMS001")
        {
            Bindddl();

            DateTime Ago24 = DateTime.Now.AddDays(-30);

            var result = db.EMdatas.Where(p => p.TerminalID == terminal && p.Update >= Ago24).Include(p => p.Terminal).OrderBy(p => p.Update);
            var bindColor = (IEnumerable<EMdata>)result;
            double TemAvg = 0, HumiAvg = 0, Pm25Avg = 0, LuminAvg = 0, UVAvg = 0;
            int count = 0;
            foreach (var item in bindColor)
            {
                TemAvg += (double)item.Temperature;
                HumiAvg += (double)item.Humidity;
                Pm25Avg += (int)item.Pm25;
                LuminAvg += (int)item.Luminance;
                UVAvg += (int)item.UV;
                count++;
            }
            TemAvg /= count;
            HumiAvg /= count;
            Pm25Avg /= count;
            LuminAvg /= count;
            UVAvg /= count;
            ViewBag.TemAvg = TemAvg;
            ViewBag.HumiAvg = HumiAvg;
            ViewBag.Pm25Avg = Pm25Avg;
            ViewBag.LuminAvg = LuminAvg;
            ViewBag.UVAvg = UVAvg;
            ViewBag.TerminalID = terminal;
            EMdata now = new EMdata();
            if (bindColor.LastOrDefault() != null)
            {
                now.Temperature = bindColor.LastOrDefault().Temperature;
                now.Pm25 = bindColor.LastOrDefault().Pm25;
                now.Luminance = bindColor.LastOrDefault().Luminance;
                now.Humidity = bindColor.LastOrDefault().Humidity;
                now.UV = bindColor.LastOrDefault().UV;
                now.GasIntensity = bindColor.LastOrDefault().GasIntensity;
                BindColor(now);
            }
            return View(result);
        }

        public ActionResult Weeks(string terminal = "EMMS001")
        {
            Bindddl();

            DateTime Ago24 = DateTime.Now.AddDays(-7);

            var result = db.EMdatas.Where(p => p.TerminalID == terminal && p.Update>=Ago24).Include(p => p.Terminal).OrderBy(p => p.Update);
            var bindColor = (IEnumerable<EMdata>)result;
            double TemAvg = 0, HumiAvg = 0, Pm25Avg = 0, LuminAvg = 0, UVAvg = 0;
            int count=0;
            foreach (var item in bindColor)
            {
                TemAvg += (double)item.Temperature;
                HumiAvg += (double)item.Humidity;
                Pm25Avg += (int)item.Pm25;
                LuminAvg += (int)item.Luminance;
                UVAvg += (int)item.UV;
                count++;
            }
            TemAvg /= count;
            HumiAvg /= count;
            Pm25Avg /= count;
            LuminAvg /= count;
            UVAvg /= count;
            ViewBag.TemAvg = TemAvg;
            ViewBag.HumiAvg = HumiAvg;
            ViewBag.Pm25Avg = Pm25Avg;
            ViewBag.LuminAvg = LuminAvg;
            ViewBag.UVAvg = UVAvg;
            ViewBag.TerminalID = terminal;
            EMdata now = new EMdata();
            if (bindColor.LastOrDefault() != null)
            {
                now.Temperature = bindColor.LastOrDefault().Temperature;
                now.Pm25 = bindColor.LastOrDefault().Pm25;
                now.Luminance = bindColor.LastOrDefault().Luminance;
                now.Humidity = bindColor.LastOrDefault().Humidity;
                now.UV = bindColor.LastOrDefault().UV;
                now.GasIntensity = bindColor.LastOrDefault().GasIntensity;
                BindColor(now);
            }
            return View(result);
        }

        public ActionResult Hours24(string terminal = "EMMS001")
        {
            Bindddl();

            DateTime Ago24 = DateTime.Now.AddDays(-1);

            var result = db.EMdatas.Where(p => p.TerminalID == terminal && p.Update >= Ago24).Include(p => p.Terminal).OrderBy(p => p.Update);
            var bindColor = (IEnumerable<EMdata>)result;
            double TemAvg = 0, HumiAvg = 0, Pm25Avg = 0, LuminAvg = 0, UVAvg = 0;
            int count = 0;
            foreach (var item in bindColor)
            {
                TemAvg += (double)item.Temperature;
                HumiAvg += (double)item.Humidity;
                Pm25Avg += (int)item.Pm25;
                LuminAvg += (int)item.Luminance;
                UVAvg += (int)item.UV;
                count++;
            }
            TemAvg /= count;
            HumiAvg /= count;
            Pm25Avg /= count;
            LuminAvg /= count;
            UVAvg /= count;
            ViewBag.TemAvg = TemAvg;
            ViewBag.HumiAvg = HumiAvg;
            ViewBag.Pm25Avg = Pm25Avg;
            ViewBag.LuminAvg = LuminAvg;
            ViewBag.UVAvg = UVAvg;
            ViewBag.TerminalID = terminal;
            EMdata now = new EMdata();
            if (bindColor.LastOrDefault() != null)
            {
                now.Temperature = bindColor.LastOrDefault().Temperature;
                now.Pm25 = bindColor.LastOrDefault().Pm25;
                now.Luminance = bindColor.LastOrDefault().Luminance;
                now.Humidity = bindColor.LastOrDefault().Humidity;
                now.UV = bindColor.LastOrDefault().UV;
                now.GasIntensity = bindColor.LastOrDefault().GasIntensity;
                BindColor(now);
            }
            return View(result);
        }

        private void BindColor(EMdata now)
        {
            if (null != now)
            {
                if (now.Temperature < 0)
                {
                    ViewBag.TemColor1 = "#188eff";
                    ViewBag.TemColor2 = "blue";
                }
                else if (now.Temperature >= 0 && now.Temperature < 10)
                {
                    ViewBag.TemColor1 = "#39fb18";
                    ViewBag.TemColor2 = "green";
                }
                else if (now.Temperature >= 10 && now.Temperature < 20)
                {
                    ViewBag.TemColor1 = "#ffe718";
                    ViewBag.TemColor2 = "yellow";
                }
                else if (now.Temperature >= 20 && now.Temperature < 30)
                {
                    ViewBag.TemColor1 = "#398a18";
                    ViewBag.TemColor2 = "orange";
                }
                else if (now.Temperature >= 30)
                {
                    ViewBag.TemColor1 = "#ff2c18";
                    ViewBag.TemColor2 = "red";
                }

                if (now.Pm25 >= 0 && now.Pm25 < 75)
                {
                    ViewBag.Pm25Color1 = "P6";
                    ViewBag.Pm25Color2 = "#3BCA18";
                    ViewBag.Pm25Level = "优";
                }
                else if (now.Pm25 >= 75 && now.Pm25 < 150)
                {
                    ViewBag.Pm25Color1 = "P5";
                    ViewBag.Pm25Color2 = "#EEDC32";
                    ViewBag.Pm25Level = "良";
                }
                else if (now.Pm25 >= 150 && now.Pm25 < 300)
                {
                    ViewBag.Pm25Color1 = "P4";
                    ViewBag.Pm25Color2 = "#DCAD43";
                    ViewBag.Pm25Level = "轻度污染";
                }
                else if (now.Pm25 >= 300 && now.Pm25 < 1050)
                {
                    ViewBag.Pm25Color1 = "P3";
                    ViewBag.Pm25Color2 = "#F2401A";
                    ViewBag.Pm25Level = "中度污染";
                }
                else if (now.Pm25 >= 1050 && now.Pm25 < 3000)
                {
                    ViewBag.Pm25Color1 = "P2";
                    ViewBag.Pm25Color2 = "#BF0841";
                    ViewBag.Pm25Level = "重度污染";
                }
                else if (now.Pm25 >= 3000)
                { 
                    ViewBag.Pm25Color1 = "P1";
                    ViewBag.Pm25Color2 = "#9B0A4D";
                    ViewBag.Pm25Level = "严重污染";
                }

                if (now.Luminance >= 0 && now.Luminance < 5) ViewBag.LuminColor = "L5";
                else if (now.Luminance >= 5 && now.Luminance < 75) ViewBag.LuminColor = "L4";
                else if (now.Luminance >= 75 && now.Luminance < 750) ViewBag.LuminColor = "L3";
                else if (now.Luminance >= 750 && now.Luminance < 7500) ViewBag.LuminColor = "L2";
                else if (now.Luminance >= 7500) ViewBag.LuminColor = "L1";

                if (now.Humidity >= 0 && now.Humidity < 20) ViewBag.HumiColor = "S5";
                else if (now.Humidity >= 20 && now.Humidity < 40) ViewBag.HumiColor = "S4";
                else if (now.Humidity >= 40 && now.Humidity < 60) ViewBag.HumiColor = "S3";
                else if (now.Humidity >= 60 && now.Humidity < 80) ViewBag.HumiColor = "S2";
                else if (now.Humidity >= 80) ViewBag.HumiColor = "S1";

                if (now.UV >= 0 && now.UV < 2) ViewBag.UVColor = "Z1";
                else if (now.UV >= 2 && now.UV < 4) ViewBag.UVColor = "Z2";
                else if (now.UV >= 4 && now.UV < 6) ViewBag.UVColor = "Z3";
                else if (now.UV >= 6 && now.UV < 9) ViewBag.UVColor = "Z4";
                else if (now.UV >= 9) ViewBag.UVColor = "Z5";

                if (now.GasIntensity >= 300) ViewBag.GasColor = "red";
                else ViewBag.GasColor = "green";

            }
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
