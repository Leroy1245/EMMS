using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMMS.Models
{
    public class EMdata
    {
        public int ID { get; set; }

        public string TerminalID { get; set; }

        [DisplayName("温度")]
        public double? Temperature { get; set; }

        [DisplayName("湿度")]
        public double? Humidity { get; set; }

        [DisplayName("PM2.5")]
        public int? Pm25 { get; set; }

        [DisplayName("亮度")]
        public int? Luminance { get; set; }

        [DisplayName("紫外线强度")]
        public int? UV { get; set; }

        [DisplayName("可燃气体浓度")]
        public int? GasIntensity { get; set; }

        [DisplayName("数据上传时间")]
        public DateTime Update { get; set; }

        public Terminal Terminal { get; set; }
    }
}