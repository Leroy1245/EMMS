using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Management;
using System.Net;
using EMMS.Areas.Manage.Filters;
using EMMS.Models;

namespace EMMS.Areas.Manage.Controllers
{
    public class FrameController : Controller
    {
        //
        // GET: /Manage/Frame/
        DB db = new DB();

        [AuthorizeFilter]
        public ActionResult Index()
        {
            ViewBag.nowYY = DateTime.Now.Year;
            ViewBag.nowMM = DateTime.Now.Month;
            ViewBag.nowDD = DateTime.Now.Day;
            return View();
        }

        [AuthorizeFilter]
        public ActionResult defaultPage()
        {
            double cpuTem = 0;
            double diskTem = 0;
            double memUsed = 0;
            string DiskTem = "MSStorageDriver_ATAPISmartData";
            string CPUTem = "MSAcpi_ThermalZoneTemperature";

            Double Temper = 0;

            ManagementObjectSearcher mos = new ManagementObjectSearcher(@"root\WMI", "Select * From " + CPUTem);
            foreach (System.Management.ManagementObject mo in mos.Get())
            {
                cpuTem = Convert.ToDouble(Convert.ToDouble(mo.GetPropertyValue("CurrentTemperature").ToString()) - 2732) / 10;
            }

            mos.Query = new ObjectQuery("Select * From " + DiskTem);
            foreach (System.Management.ManagementObject mo in mos.Get())
            {
                byte[] data = (byte[])mo.GetPropertyValue("VendorSpecific");
                diskTem = data[3];
            }


            double capacity = 0;
            double available = 0;

            ManagementClass cimobject1 = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo1 in moc1)
            {
                capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024/ 1024.0, 1)));
            }
            moc1.Dispose();
            cimobject1.Dispose();


            //获取内存可用大小
            ManagementClass cimobject2 = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory");
            ManagementObjectCollection moc2 = cimobject2.GetInstances();
            foreach (ManagementObject mo2 in moc2)
            {
                available += ((Math.Round(Int64.Parse(mo2.Properties["AvailableMBytes"].Value.ToString()) / 1.0, 1)));

            }
            moc2.Dispose();
            cimobject2.Dispose();

            memUsed = capacity - available;

            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);  
            IPAddress localaddr = localhost.AddressList[3];


            int ms = Environment.TickCount;
            int s = ms / 1000;
            int min = s / 60;
            int sec = s % 60;
            int hour = min / 60;
            min = min % 60;

            ViewBag.cpuTem = cpuTem;
            ViewBag.diskTem = diskTem;
            ViewBag.memUsed = memUsed;
            ViewBag.runHour = hour;
            ViewBag.runMin = min;
            ViewBag.runSec = sec;
            ViewBag.memUsed = memUsed;
            ViewBag.nowYY = DateTime.Now.Year;
            ViewBag.nowMonth = DateTime.Now.Month;
            ViewBag.nowDD = DateTime.Now.Day;
            ViewBag.nowhh = DateTime.Now.Hour;
            ViewBag.nowmm = DateTime.Now.Minute;
            ViewBag.nowss = DateTime.Now.Second;
            ViewBag.IPAddress = localaddr.ToString();

            int alarmCount = db.Alarms.Count(p => p.IsRead == 0);
            ViewBag.alarmCount = alarmCount;
            return View();
        }

    }
}
