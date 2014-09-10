using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMMS.Areas.Manage.Models.ViewModel
{
    public class EmdataSearchModel
    {
        [Required(ErrorMessage = "请选择采集终端")]
        public string TerminalID { get; set; }

        [Required(ErrorMessage="请选择起始时间")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "请选择终止时间")]
        public DateTime EndTime { get; set; }

    }
}