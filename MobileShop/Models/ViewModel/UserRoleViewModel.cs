using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MobileShop.Models.ViewModel
{
    public class UserRoleViewModel
    {
        [DisplayName("Mã User")]
        public int user_id { set; get; }
        [DisplayName("Họ Tên")]
        public string user_name { set; get; }
        [DisplayName("Tài Khoản")]
        public string user_acc { set; get; }
        [DisplayName("Mật Khẩu")]
        public string user_pass { set; get; }
        [DisplayName("Địa Chỉ")]
        public string user_addr { set; get; }
        [DisplayName("Điện Thoại")]
        public string user_phone { set; get; }
        [DisplayName("Email")]
        public string user_mail { set; get; }
        [DisplayName("Số CMND")]
        public string user_idCart { set; get; }
        public int? user_roleid { set; get; }
        [DisplayName("Vai Trò")]
        public string user_rolename { set; get; }
    }
}