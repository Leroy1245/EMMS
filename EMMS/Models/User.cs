using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMMS.Models
{
    public class User
    {
        public int ID { get; set; }

        [DisplayName("用户名")]
        [StringLength(20,MinimumLength=5,ErrorMessage="用户名长度应该在5-20之间")]
        [Required(ErrorMessage = "请输入您的用户名")]
        public string Username { get; set; }

        [DisplayName("密码")]
        [StringLength(50, MinimumLength = 2,ErrorMessage="密码长度应该在2-50之间")]
        [Required(ErrorMessage = "请输入您的密码")]
        public string Userpw { get; set; }

        [DisplayName("真实姓名")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "真实姓名长度应该在2-20之间")]
        [Required(ErrorMessage = "请输入您的真实姓名")]
        public string Realname { get; set; }

        [DisplayName("上次登录IP")]
        [StringLength(50)]
        public string Userip { get; set; }

        [DisplayName("用户类型")]
        [Required(ErrorMessage = "请选择用户类型")]
        public int Usertype { get; set; }

        [ForeignKey("TerminalUserID")]
        public List<Terminal> MyTerminals { get; set; }
    }
}