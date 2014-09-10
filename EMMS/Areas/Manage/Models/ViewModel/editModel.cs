using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMMS.Areas.Manage.Models.ViewModel
{
    public class editModel
    {
        [DisplayName("用户名")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "用户名长度应该在5-20之间")]
        [Required(ErrorMessage = "请输入您的用户名")]
        public string Username { get; set; }

        [DisplayName("真实姓名")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "真实姓名长度应该在2-20之间")]
        [Required(ErrorMessage = "请输入您的真实姓名")]
        public string Realname { get; set; }

        [DisplayName("用户类型")]
        [Required(ErrorMessage = "请选择用户类型")]
        public int Usertype { get; set; }
    }
}