using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EMMS.Areas.Manage.Models.ViewModel
{
    public class chPwdModel
    {
        [DisplayName("原密码")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "密码长度应该在2-50之间")]
        [Required(ErrorMessage = "请输入原密码")]
        public string OldUserpw { get; set; }

        [DisplayName("新密码")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "密码长度应该在2-50之间")]
        [Required(ErrorMessage = "请输入您的密码")]
        public string Userpw { get; set; }

        [DisplayName("确认密码")]
        [Compare("Userpw", ErrorMessage = "密码不一致")]
        public string UserpwConfirm { get; set; }
    }
}