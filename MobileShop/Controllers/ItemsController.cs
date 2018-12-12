using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileShop.Models;
using MobileShop.Models.ViewModel;
using PagedList;
using PagedList.Mvc;
namespace MobileShop.Controllers
{
    public class ItemsController : Controller
    {
        DtMobileShopDataContext data = new DtMobileShopDataContext();
        // GET: Items
        public ActionResult Detail(int id)
        {
            //listItem to show a list of color of product
            var listItem = from t in data.Products
                       join c in data.ProductColors on t.ProductId equals c.ProductId
                       where t.ProductId == id
                       select new ProductViewModel()
                       {
                           //Id = t.ProductId,
                           //Name = t.ProductName,
                           //Detail = t.Detail,
                           //Sale = t.Sale,
                           //Image = t.ImageProduct,
                           //Screen = t.Screen,
                           //Cpu = t.CPU,
                           //UpdateDate = t.UpdateDate,
                           //Quantity = t.Quantity,
                           //Type = t.ProductType.TypeName,
                           //Brand = t.Brand.BrandName,
                           Color = c.ColorName,
                           Price = c.Price,
                           Imagecolor = c.ImageColor,
                           IdColor = c.ColorId
                       };
            //get basic info
            Product item = data.Products.Where(t => t.ProductId == id).First();
            ViewData["idP"] = item.ProductId;
            ViewBag.Name = item.ProductName;
            ViewData["quantity"] = item.Quantity;
            ViewBag.Brand = item.Brand;
            //return list
            return View(listItem.ToList());
        }

        public ActionResult Detail_in_cart(int id, int idColor)
        {
            var listItem = from t in data.Products
                           join c in data.ProductColors on t.ProductId equals c.ProductId
                           where t.ProductId == id
                           select new ProductViewModel()
                           {
                               Color = c.ColorName,
                               Price = c.Price,
                               Imagecolor = c.ImageColor,
                               IdColor = c.ColorId
                           };
            Product item = data.Products.Where(t => t.ProductId == id).First();
            ViewData["idP"] = item.ProductId;
            ViewBag.Name = item.ProductName;
            ViewData["quantity"] = item.Quantity;
            ViewBag.Brand = item.Brand;
            //get ColorId to set which color product is checked
            ViewBag.ColorId = idColor;

            return View(listItem.ToList());
        }
        private List<ProductViewModel> TakeProductBrands(int id)
        {
            var lstProd = from p in data.Products
                          join pc in data.ProductColors on p.ProductId equals pc.ProductId
                          where p.BrandId == id
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
            foreach (var item in lstProd)
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
            list.OrderByDescending(a => a.UpdateDate);
            return list;
        }
        private List<ProductViewModel> TakeProductTypes(int id)
        {
            var lstProd = from p in data.Products
                          join pc in data.ProductColors on p.ProductId equals pc.ProductId
                          where p.TypeId == id
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
            foreach (var item in lstProd)
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
            list.OrderByDescending(a => a.UpdateDate);
            return list;
        }
        public ActionResult ItemsForType(int id, int? page)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            var items = TakeProductTypes(id);
            ViewData["TypeId"] = id;
            var titleName = data.ProductTypes.SingleOrDefault(t => t.TypeId == id);
            ViewData["TitleName"] = titleName.TypeName;
            return View(items.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ItemsForBrand(int id, int? page)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            var items = TakeProductBrands(id);
            ViewData["BrandId"] = id;
            var titleName = data.Brands.SingleOrDefault(t => t.BrandId == id);
            ViewData["TitleName"] = titleName.BrandName;
            return View(items.ToPagedList(pageNum,pageSize));
        }
    }
}