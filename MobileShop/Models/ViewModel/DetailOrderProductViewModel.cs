using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileShop.Models.ViewModel
{
    public class DetailOrderProductViewModel
    {
        public int order_id { set; get; }
        public int order_colorid { set; get; }
        public int prod_id { set; get; }
        public int? order_qtt { set; get; }//quantity
        public decimal? order_price { set; get; }
        public decimal? order_total { set; get; }
        public string order_p_name { set; get; }//product name
        public string order_pc_name { set; get; }//product color name
    }
}