using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMMS.Models
{
    public class Alarm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlarmID { get; set; }

        [DisplayName("发出警报采集终端ID")]
        public string AlarmTermID { get; set; }

        [DisplayName("报警发出时间")]
        public DateTime AlarmDate { get; set; }

        [DisplayName("警报信息")]
        [StringLength(200)]
        public string AlarmContent { get; set; }

        [DisplayName("阅读状态")]
        public int IsRead { get; set; }


        public Terminal Terminal { get; set; }

    }
}