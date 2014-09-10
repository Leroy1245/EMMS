using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMMS.Areas.Manage.Models.ViewModel
{
    public class editTerminalModel
    {
        [Key]
        [StringLength(20)]
        [DisplayName("采集终端ID")]
        public string TerminalID { get; set; }

        [DisplayName("采集终端名称")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "采集终端名称长度应在2-30个字符之间")]
        [Required(ErrorMessage = "请输入采集终端名称")]
        public string TerminalName { get; set; }

        [DisplayName("采集终端位置")]
        [StringLength(200, ErrorMessage = "采集终端位置长度应在200个字符以下")]
        public string TerminalAddr { get; set; }

        [DisplayName("温度警戒值")]
        [Required(ErrorMessage = "请输入温度警戒值")]
        public double TemMax { get; set; }

        [DisplayName("湿度警戒值")]
        [Required(ErrorMessage = "请输入湿度警戒值")]
        public double HumiMax { get; set; }

        [DisplayName("PM2.5警戒值")]
        [Required(ErrorMessage = "请输入PM2.5警戒值")]
        public int Pm25Max { get; set; }

        [DisplayName("亮度警戒值")]
        [Required(ErrorMessage = "请输入亮度警戒值")]
        public int LuminMax { get; set; }

        [DisplayName("紫外强度警戒值")]
        [Required(ErrorMessage = "请输入紫外强度警戒值")]
        public int UVMax { get; set; }

        [DisplayName("可燃气体浓度警戒值")]
        [Required(ErrorMessage = "请输入可燃气体浓度警戒值")]
        public int GasMax { get; set; }
    }
}