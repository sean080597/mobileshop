using MobileShop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileShop.Models
{
    public class ItemInCart
    {
        public int id { set; get; }
        public int idcolor { set; get; }
        public string name { set; get; }
        public string color_name { set; get; }
        public string image { set; get; }
        public decimal? price { set; get; }
        public int quantity { set; get; }

        DtMobileShopDataContext data = new DtMobileShopDataContext();

        public decimal? Total()
        {
            return quantity * price;
        }
        public ItemInCart(int idP, int idColor)
        {
            id = idP;
            idcolor = idColor;

            Product p = data.Products.Single(t => t.ProductId == idP);
            name = p.ProductName;

            ProductColor pc = data.ProductColors.Single(t => t.ColorId == idColor);
            color_name = pc.ColorName;
            image = pc.ImageColor;
            price = pc.Price;
            quantity = 1;
        }
    }
}