using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MobileShop.Models.ViewModel
{
    public class OrderProductViewModel
    {
        [DisplayName("Mã Hóa Đơn")]
        public int order_id { set; get; }
        [DisplayName("Đã Thanh Toán")]
        public Boolean order_checkout { set; get; }
        [DisplayName("Ngày Đặt")]
        public DateTime order_day { set; get; }
        [DisplayName("Ngày Giao")]
        public DateTime order_deliveryday { set; get; }
        [DisplayName("Địa Chỉ")]
        public string order_place { set; get; }
        public int? order_userid { set; get; }
        [DisplayName("Tên Khách Hàng")]
        public string order_username { set; get; }
    }
}