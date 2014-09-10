using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EMMS.Areas.Manage.Models.ViewModel
{
    public class Account
    {
        [DisplayName("用户名")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "用户名长度应该在5-20之间")]
        [Required]
        public string Username { get; set; }

        [DisplayName("密码")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "密码长度应该在2-50之间")]
        [Required(ErrorMessage = "请输入您的密码")]
        public string Userpw { get; set; }
    }
}