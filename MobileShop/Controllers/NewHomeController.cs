using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileShop.Models;
using PagedList;
using PagedList.Mvc;
using MobileShop.Models.ViewModel;

namespace MobileShop.Controllers
{
    public class NewHomeController : Controller
    {
        DtMobileShopDataContext data = new DtMobileShopDataContext();

        private List<ProductViewModel> TakeNewProducts()
        {
            var lstProd = from p in data.Products
                          join pc in data.ProductColors on p.ProductId equals pc.ProductId
                          //orderby p.UpdateDate descending
                          select new
                          {
                              pId = p.ProductId,
                              imgColor = pc.ImageColor,
                              pName = p.ProductName,
                              nameColor = pc.ColorName,
                              pPrice = pc.Price,
                              cpu = p.CPU,
                              screen = p.Screen
                          };
            var list = new List<ProductViewModel>();
            foreach(var item in lstProd)
            {
                list.Add(new ProductViewModel
                {
                    Id = item.pId,
                    Imagecolor = item.imgColor,
                    Name = item.pName,
                    ColorName = item.nameColor,
                    Price = item.pPrice,
                    Cpu = item.cpu,
                    Screen = item.screen
                });
            }
            //shuffle list products
            list = list.OrderBy(a => Guid.NewGuid()).ToList();
            return list;
        }

        // GET: NewHome
        public ActionResult Index( int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var newProduct = TakeNewProducts();
            return View(newProduct.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Slider()
        {
            return PartialView();
        }
        public ActionResult Types()
        {
            var typeP = from t in data.ProductTypes select t;
            return PartialView(typeP);
        }
        public class BrandQuantity{
            private int id, quantity;
            private string name;

            public int Id
            {
                get
                {
                    return id;
                }

                set
                {
                    id = value;
                }
            }

            public string Name
            {
                get
                {
                    return name;
                }

                set
                {
                    name = value;
                }
            }

            public int Quantity
            {
                get
                {
                    return quantity;
                }

                set
                {
                    quantity = value;
                }
            }
        }
        public ActionResult Brands()
        {
            var brand = from t in data.Brands select t;
            var list = new List<BrandQuantity>();
            foreach (var item in brand)
            {
                var quantity = data.Products.Count(a => a.BrandId == item.BrandId);
                list.Add(new BrandQuantity {
                    Id = item.BrandId,
                    Name = item.BrandName,
                    Quantity = quantity
                });
            }
            return PartialView(list);
        }
    }
}