using MobileShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileShop.Models.ViewModel;
using System.Globalization;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace MobileShop.Controllers
{
    public class AdminController : Controller
    {
        DtMobileShopDataContext data = new DtMobileShopDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection col)
        {
            var adRole = 1;
            var user = col["username"];
            var pwd = col["password"];
            User ad = data.Users.SingleOrDefault(a => a.UserAccount == user && a.UserPassword == pwd);
            if (ad != null)
            {
                int? role = ad.IdRole;
                if ( role == adRole)
                {
                    
                    Session["AdminAccount"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Error = "Bạn không đủ quyền truy cập vào trang này!";
                }
            }
            else
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng";
            }
            return View();
        }

        //*** Brand Control ***
        public ActionResult BrandControl()
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(data.Brands.ToList());
        }

        public string EditBrand(int brandId, string brandName)
        {
            Brand b = data.Brands.SingleOrDefault(t => t.BrandId == brandId);
            if (b == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            b.BrandName = brandName;
            UpdateModel(b);
            data.SubmitChanges();
            return "Sửa nhãn hiệu thành công";
        }

        public string DeleteBrand(int brandId)
        {
            var lstProd = from t in data.Products select t.BrandId;
            if (lstProd.Contains(brandId))
                return "Tồn tại sản phẩm đang sử dụng nhãn hiệu này!";
            else
            {
                Brand b = data.Brands.SingleOrDefault(t => t.BrandId == brandId);
                if (b == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.Brands.DeleteOnSubmit(b);
                data.SubmitChanges();
                return "Xóa thành công";
            }
        }

        public string CreateNewBrand(string brandName)
        {
            Brand b = new Brand();
            b.BrandName = brandName;
            data.Brands.InsertOnSubmit(b);
            data.SubmitChanges();
            return "Thêm mới nhãn hiệu thành công";
        }

        //*** Type Product Control ***
        public ActionResult TypeControl()
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(data.ProductTypes.ToList());
        }

        public string EditType(int typeId, string typeName)
        {
            ProductType pt = data.ProductTypes.SingleOrDefault(t => t.TypeId == typeId);
            if (pt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            pt.TypeName = typeName;
            UpdateModel(pt);
            data.SubmitChanges();
            return "Sửa loại sản phẩm thành công";
        }

        public string DeleteType(int typeId)
        {
            var lstProd = from t in data.Products select t.TypeId;
            if (lstProd.Contains(typeId))
                return "Tồn tại sản phẩm đang sử dụng loại sản phẩm này!";
            else
            {
                ProductType pt = data.ProductTypes.SingleOrDefault(t => t.TypeId == typeId);
                if (pt == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.ProductTypes.DeleteOnSubmit(pt);
                data.SubmitChanges();
                return "Xóa thành công";
            }
        }

        public string CreateNewType(string typeName)
        {
            ProductType pt = new ProductType();
            pt.TypeName = typeName;
            data.ProductTypes.InsertOnSubmit(pt);
            data.SubmitChanges();
            return "Thêm mới loại sản phẩm thành công";
        }

        //*** User Control ***
        public ActionResult UserControl()
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            //send list role user to show in dropdown list
            var lstRole = data.Roles.ToList();
            ViewBag.RoleList = lstRole;

            var lstUser = from u in data.Users
                          join r in data.Roles on u.IdRole equals r.RoleId
                          select new UserRoleViewModel
                          {
                              user_id = u.UserId,
                              user_name = u.UserName,
                              user_acc = u.UserAccount,
                              user_addr = u.UserAddress,
                              user_pass = u.UserPassword,
                              user_mail = u.Email,
                              user_phone = u.NPhone,
                              user_idCart = u.CMND,
                              user_roleid = u.IdRole,
                              user_rolename = r.RoleName
                          };
            return View(lstUser);
        }

        //set selected option for role user
        public string GetUserRole(int roleId)
        {
            var lstRole = data.Roles.ToList();
            string result = "";
            foreach (var item in lstRole)
            {
                if (item.RoleId == roleId)
                    result += "<option value="+item.RoleId+" selected>"+item.RoleName+"</option>";
                else
                    result += "<option value=" + item.RoleId + ">" + item.RoleName + "</option>";
            }
            return result;
        }

        public string EditUser(int userId, string userName, string userAcc, string userPass, string userAddr, string userPhone, string userEmail, string idCart, int roleId)
        {
            //return userId + "-" + userName + "-" + userAcc + "-" + userPass + "-" + userPhone + "-" + userAddr + "-" + userEmail + "-" + idCart + "-" + roleId;
            User u = data.Users.SingleOrDefault(t => t.UserId == userId);
            if (u == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            u.UserName = userName;
            u.UserAccount = userAcc;
            u.UserPassword = userPass;
            u.UserAddress = userAddr;
            u.NPhone = userPhone;
            u.Email = userEmail;
            u.CMND = idCart;
            u.IdRole = roleId;
            UpdateModel(u);
            data.SubmitChanges();
            return "Sửa người dùng thành công";
        }

        public string DeleteUser(int userId)
        {
            var lstUserId = from t in data.OrderProducts select t.UserId;
            if (lstUserId.Contains(userId))
                return "Tồn tại hóa đơn của người dùng này!";
            else
            {
                User u = data.Users.SingleOrDefault(t => t.UserId == userId);
                if (u == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.Users.DeleteOnSubmit(u);
                data.SubmitChanges();
                return "Xóa thành công";
            }
        }

        public string CreateNewUser(string userName, string userAcc, string userPass, string userAddr, string userPhone, string userEmail, string idCart, int roleId)
        {
            User u = new User();
            u.UserName = userName;
            u.UserAccount = userAcc;
            u.UserPassword = userPass;
            u.UserAddress = userAddr;
            u.NPhone = userPhone;
            u.Email = userEmail;
            u.CMND = idCart;
            u.IdRole = roleId;
            data.Users.InsertOnSubmit(u);
            data.SubmitChanges();
            return "Thêm người dùng thành công";
        }

        //*** Order Product Control ***
        public ActionResult OrderProductControl()
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var lstOrder = from o in data.OrderProducts
                          join u in data.Users on o.UserId equals u.UserId
                          select new OrderProductViewModel
                          {
                              order_id = o.OrderingId,
                              order_checkout = Convert.ToBoolean(o.CheckOut),
                              order_day = Convert.ToDateTime(o.OrderDay),
                              order_deliveryday = Convert.ToDateTime(o.DeliveryDay),
                              order_place = o.PlaceName,
                              order_userid = o.UserId,
                              order_username = u.UserName
                          };
            return View(lstOrder);
        }

        //set selected option for order user
        public string GetUserOrder(int userId)
        {
            var lstUser = data.Users.ToList();
            string result = "";
            foreach (var item in lstUser)
            {
                if (item.UserId == userId)
                    result += "<option value=" + item.UserId + " selected>" + item.UserName + "</option>";
                else
                    result += "<option value=" + item.UserId + ">" + item.UserName + "</option>";
            }
            return result;
        }
        public string GetUserOrderWithoutUserId()
        {
            var lstUser = data.Users.ToList();
            string result = "";
            foreach (var item in lstUser)
            {
                result += "<option value=" + item.UserId + ">" + item.UserName + "</option>";
            }
            return result;
        }

        public string DeleteOrder(int orderId)
        {
            //delete all products of order first
            var lstLineItem = from t in data.LineItems where t.OrderingId == orderId select t;
            foreach (var item in lstLineItem)
            {
                data.LineItems.DeleteOnSubmit(item);
            }
            data.SubmitChanges();

            //delete order after
            OrderProduct o = data.OrderProducts.SingleOrDefault(t => t.OrderingId == orderId);
            data.OrderProducts.DeleteOnSubmit(o);
            data.SubmitChanges();
            return "Xóa thành công";
        }

        public string EditOrderProduct(int orderId, Boolean orderChecked, string orderDay, string orderDeli, string orderPlace, int orderUserid)
        {
            //return orderId + "-" + orderChecked + "-" + orderDay + "-" + orderDeli + "-" + orderPlace + "-" + orderUserid;
            OrderProduct o = data.OrderProducts.SingleOrDefault(t => t.OrderingId == orderId);
            if (o == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            o.CheckOut = orderChecked;
            o.OrderDay = DateTime.Parse(orderDay);
            o.DeliveryDay = DateTime.Parse(orderDeli);
            //o.PlaceName = orderPlace;
            o.UserId = orderUserid;
            UpdateModel(o);
            data.SubmitChanges();
            return "Sửa hóa đơn thành công";
        }

        public string CreateNewOrderProduct(Boolean orderChecked, string orderDay, string orderDeli, string orderPlace, int orderUserid)
        {
            OrderProduct o = new OrderProduct();
            o.CheckOut = orderChecked;
            o.OrderDay = DateTime.Parse(orderDay);
            o.DeliveryDay = DateTime.Parse(orderDeli);
            //o.PlaceName = orderPlace;
            o.UserId = orderUserid;
            data.OrderProducts.InsertOnSubmit(o);
            data.SubmitChanges();
            return "Thêm hóa đơn thành công";
        }
        //get list of LineItem to show in detail of order
        public string GetLstLineItem(int orderId)
        {
            var lstLineItem = from li in data.LineItems
                              join p in data.Products on li.ProductId equals p.ProductId
                              join pc in data.ProductColors on li.ColorId equals pc.ColorId
                              where li.OrderingId == orderId
                              select new DetailOrderProductViewModel
                              {
                                  order_id = li.OrderingId,
                                  order_colorid = li.ColorId,
                                  prod_id = li.ProductId,
                                  order_qtt = li.quantity,
                                  order_price = li.Price,
                                  order_total = li.Total,
                                  order_p_name = p.ProductName,
                                  order_pc_name = pc.ColorName
                              };
            string result = "";
            string open_tag = "<td>";
            string close_tag = "</td>";
            foreach (var item in lstLineItem)
            {
                result += "<tr>" + open_tag + item.order_p_name + " " + item.order_pc_name + close_tag;
                result += open_tag + item.order_qtt + close_tag;
                result += open_tag + String.Format("{0:#,##0.##}", item.order_price) + " VNĐ" + close_tag;
                result += open_tag + String.Format("{0:#,##0.##}", item.order_total) + " VNĐ" + close_tag;
                result += open_tag + "<button type=\"button\" class=\"btn btn-info\" data-toggle=\"modal\" data-target=\"#modal_edit_order_detail\" id=\"btn_edit_detail_order\" order_id=\"" + item.order_id + "\" color_id=\"" + item.order_colorid + "\" p_id=\"" + item.prod_id + "\">Sửa</button>"
                     + close_tag;
                result += open_tag + "<button type=\"button\" class=\"btn btn-danger\" data-dismiss=\"modal\" id=\"btn_delete_detail_order\" order_id=\"" + item.order_id + "\" color_id=\"" + item.order_colorid + "\">Xóa</button>" + close_tag + "</tr>";
            }
            return result;
        }

        //get total of order
        public string GetTotalOrder(int orderId)
        {
            decimal? total = 0;
            var lstLineitem = data.LineItems.Where(li => li.OrderingId == orderId).ToList();
            foreach(var item in lstLineitem)
            {
                total += item.Total;
            }
            return String.Format("{0:#,###,###}", total) + " VNĐ";
        }

        //delete a product in order detail
        public string DeleteDetailOrder(int orderId, int colorId)
        {
            LineItem li = data.LineItems.SingleOrDefault(t => t.OrderingId == orderId && t.ColorId == colorId);
            if (li == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.LineItems.DeleteOnSubmit(li);
            data.SubmitChanges();
            return "Xóa thành công";
        }

        //handling before updating data
        //get select list product name for detail order
        public string GetSelProductName(int orderId, int colorId)
        {
            var prodId = data.LineItems.SingleOrDefault(t => t.OrderingId == orderId && t.ColorId == colorId).ProductId;
            var lstProd = data.Products.ToList();
            var result = "";
            foreach (var item in lstProd)
            {
                if (item.ProductId == prodId)
                {
                    result += "<option value=\""+ item.ProductId +"\" selected>" + item.ProductName +"</option>";
                }
                else
                {
                    result += "<option value=\"" + item.ProductId + "\">" + item.ProductName + "</option>";
                }
            }
            return result;
        }
        public string GetSelProductNameNoParams()
        {
            var lstProd = data.Products.ToList();
            var result = "";
            foreach (var item in lstProd)
            {
                result += "<option value=\"" + item.ProductId + "\">" + item.ProductName + "</option>";
            }
            return result;
        }
        //get select list color name for detail order
        public string GetSelColorName(int colorId, int prodId)
        {
            var lstColor = data.ProductColors.Where(t => t.ProductId == prodId).ToList();
            var result = "";
            foreach (var item in lstColor)
            {
                if (item.ColorId == colorId)
                {
                    result += "<option value=\"" + item.ColorId + "\" selected>" + item.ColorName + "</option>";
                }
                else
                {
                    result += "<option value=\"" + item.ColorId + "\">" + item.ColorName + "</option>";
                }
            }
            return result;
        }
        public string GetSelColorNameNoParams(int prodId)
        {
            //var prodId = data.LineItems.SingleOrDefault(t => t.OrderingId == orderId && t.ColorId == colorId).ProductId;
            var lstColor = data.ProductColors.Where(t => t.ProductId == prodId).ToList();
            var result = "";
            foreach (var item in lstColor)
            {
                result += "<option value=\"" + item.ColorId + "\">" + item.ColorName + "</option>";
            }
            return result;
        }
        //get price product for detail order
        public string GetPriceDetail(int colorId)
        {
            var x = data.ProductColors.SingleOrDefault(t => t.ColorId == colorId).Price;
            return String.Format("{0:#,###,###}", x) + " VNĐ";
        }
        //get total price product for detail order
        public string GetTotalPriceDetail(int colorId, int quantity)
        {
            var x = data.ProductColors.SingleOrDefault(t => t.ColorId == colorId).Price;
            var y = x * quantity;
            return String.Format("{0:#,###,###}", y) + " VNĐ";
        }
        //update detail order product
        public string UpdateDetailOrderProduct(int orderId, int oldProdId, int oldColorId, int newProdId, int newColorId, int detailQtt, decimal detailPrice, decimal detailTotalPrice)
        {
            //return orderId + "-" + oldProdId + "-" + oldColorId + "-" + newProdId + "-" + newColorId + "-" + detailQtt+"-"+detailPrice+"-"+detailTotalPrice;
            //check if not changed colorId and ProdId
            if (oldColorId == newColorId)
            {
                var li = data.LineItems.SingleOrDefault(t => t.OrderingId == orderId && t.ColorId == oldColorId);
                li.quantity = detailQtt;
                li.Price = detailPrice;
                li.Total = detailTotalPrice;
                UpdateModel(li);
                data.SubmitChanges();
                return "Sửa chi tiết hóa đơn thành công";
            }
            else
            {
                //if changed colorId and contained in the orderId
                var lstLineItem = data.LineItems.Where(t => t.OrderingId == orderId).Select(t => t.ColorId);
                if (lstLineItem.Contains(newColorId))
                    return "Không thể cập nhật vì tồn tại 2 sản phẩm như nhau trong hóa đơn!";
                else
                {
                    //changed that do not duplicate the old one
                    var li = data.LineItems.SingleOrDefault(t => t.OrderingId == orderId && t.ColorId == oldColorId);
                    data.LineItems.DeleteOnSubmit(li);
                    data.SubmitChanges();
                    LineItem newli = new LineItem();
                    newli.OrderingId = orderId;
                    newli.ProductId = newProdId;
                    newli.ColorId = newColorId;
                    newli.quantity = detailQtt;
                    newli.Price = detailPrice;
                    newli.Total = detailTotalPrice;
                    data.LineItems.InsertOnSubmit(newli);
                    data.SubmitChanges();
                    return "Sửa chi tiết hóa đơn thành công";
                }
            }
        }

        //add detail order product
        public string AddDetailOrderProduct(int orderId, int newProdId, int newColorId, int detailQtt, decimal detailPrice, decimal detailTotalPrice)
        {
            var lstLineItem = data.LineItems.Where(t => t.OrderingId == orderId).Select(t => t.ColorId);
            if (lstLineItem.Contains(newColorId))
                return "Không thể thêm vì đã tồn tại sản phẩm này trong hóa đơn!";
            else
            {
                LineItem newli = new LineItem();
                newli.OrderingId = orderId;
                newli.ProductId = newProdId;
                newli.ColorId = newColorId;
                newli.quantity = detailQtt;
                newli.Price = detailPrice;
                newli.Total = detailTotalPrice;
                data.LineItems.InsertOnSubmit(newli);
                data.SubmitChanges();
                return "Thêm sản phẩm vào chi tiết hóa đơn thành công";
            }
        }

        //*** Product Control ***
        public ActionResult ProductControl(int? page)
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(data.Products.ToList().ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.TypeID = new SelectList(data.ProductTypes.ToList().OrderBy(i => i.TypeName), "TypeId", "TypeName");
            ViewBag.BrandID = new SelectList(data.Brands.ToList().OrderBy(i => i.BrandName), "BrandId", "BrandName");
            return View();
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult CreateProduct( Product p, HttpPostedFileBase ImageProduct)
        {
            ViewBag.TypeID = new SelectList(data.ProductTypes.ToList().OrderBy(i => i.TypeName), "TypeId", "TypeName");
            ViewBag.BrandID = new SelectList(data.Brands.ToList().OrderBy(i => i.BrandName), "BrandId", "BrandName");
            if (ModelState.IsValid)
            {
                if (ImageProduct != null)
                {
                    if (Path.GetExtension(ImageProduct.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(ImageProduct.FileName).ToLower() == ".png"
                        || Path.GetExtension(ImageProduct.FileName).ToLower() == ".gif"
                        || Path.GetExtension(ImageProduct.FileName).ToLower() == ".jpeg"
                        )
                    {
                        var filename = Path.GetFileName(ImageProduct.FileName);
                        var path = Path.Combine(Server.MapPath("~/images/products"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.error = "Tên ảnh đã tồn tại";
                        }
                        else
                        {
                            ImageProduct.SaveAs(path);
                            p.ImageProduct = ImageProduct.FileName;
                            p.UpdateDate = DateTime.Now;
                            data.Products.InsertOnSubmit(p);
                            data.SubmitChanges();
                            return RedirectToAction("ProductControl");
                        }
                    }
                }
                else
                {
                    ViewBag.error = "Có lỗi trong quá trình upload";
                }
            }
            return this.CreateProduct();
        }
        public ActionResult RemoveProduct(int id)
        {
            Product p = data.Products.SingleOrDefault(i => i.ProductId == id);
            if (p == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //var filePath = Server.MapPath("~/images/products" + p.ImageProduct);
            var filePath = Path.Combine(Server.MapPath("~/images/products"), p.ImageProduct);
            try
            {
                data.Products.DeleteOnSubmit(p);
                data.SubmitChanges();
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErRemove = "Bạn phải xóa các màu của sản phẩm";
            }
            return RedirectToAction("ProductControl", "Admin");
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.TypeID = new SelectList(data.ProductTypes.ToList().OrderBy(i => i.TypeName), "TypeId", "TypeName");
            ViewBag.BrandID = new SelectList(data.Brands.ToList().OrderBy(i => i.BrandName), "BrandId", "BrandName");
            Product p = data.Products.SingleOrDefault(i => i.ProductId == id);
            ViewData["Image"] = p.ImageProduct;
            if (p==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(p);
        }
        [HttpPost,ValidateInput(false)]
        public ActionResult EditProduct(Product p, HttpPostedFileBase NewImageP, FormCollection col)
        {
            ViewBag.TypeID = new SelectList(data.ProductTypes.ToList().OrderBy(i => i.TypeName), "TypeId", "TypeName");
            ViewBag.BrandID = new SelectList(data.Brands.ToList().OrderBy(i => i.BrandName), "BrandId", "BrandName");
            var image = col["ImageP"];
            Product item = data.Products.First(n=>n.ProductId == p.ProductId);
            
            if (ModelState.IsValid)
            {
                if (NewImageP != null)
                {
                    if (Path.GetExtension(NewImageP.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(NewImageP.FileName).ToLower() == ".png"
                        || Path.GetExtension(NewImageP.FileName).ToLower() == ".gif"
                        || Path.GetExtension(NewImageP.FileName).ToLower() == ".jpeg"
                        )
                    {
                        var filename = Path.GetFileName(NewImageP.FileName);
                        var path = Path.Combine(Server.MapPath("~/images/products"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.error = "Tên ảnh đã tồn tại";
                        }
                        else
                        {
                            var newImageP = NewImageP.FileName;
                            NewImageP.SaveAs(path);
                            item.ImageProduct = newImageP;
                            item.UpdateDate = DateTime.Now;
                            UpdateModel(item);
                            data.SubmitChanges();
                            return RedirectToAction("ProductControl");
                        }
                    }
                    else
                    {
                        ViewBag.error = "Hãy nhập một ảnh";
                    }
                }
                else
                {
                    item.ImageProduct = image;
                    item.UpdateDate = DateTime.Now;
                    UpdateModel(item);
                    data.SubmitChanges();
                    return RedirectToAction("ProductControl");
                }
            }
            return this.EditProduct(p.ProductId);
        }
        public ActionResult DetailProduct(int id)
        {
            Product p = data.Products.SingleOrDefault(n=>n.ProductId == id);
            return View(p);
        }
        public ActionResult DetailColorP(int id)
        {
            var listColor = from c in data.ProductColors where c.ProductId == id select c;
            ViewData["ProductID"] = id;
            return View(listColor);
        }
        [HttpGet]
        public ActionResult CreateProductColor(int id)
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewData["ProductID"] = id;
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateProductColor(ProductColor p, HttpPostedFileBase ImageColor, FormCollection col)
        {
            var pid = col["ProductID"];
            if (ModelState.IsValid)
            {
                if (ImageColor != null)
                {
                    if (Path.GetExtension(ImageColor.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(ImageColor.FileName).ToLower() == ".png"
                        || Path.GetExtension(ImageColor.FileName).ToLower() == ".gif"
                        || Path.GetExtension(ImageColor.FileName).ToLower() == ".jpeg"
                        )
                    {
                        var filename = Path.GetFileName(ImageColor.FileName);
                        var path = Path.Combine(Server.MapPath("~/images/products"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.error = "Tên ảnh đã tồn tại";
                        }
                        else
                        {
                            ImageColor.SaveAs(path);
                            p.ImageColor = ImageColor.FileName;
                            p.ProductId = int.Parse(pid);
                            data.ProductColors.InsertOnSubmit(p);
                            data.SubmitChanges();
                            return RedirectToAction("DetailColorP", new { id = pid});
                        }
                    }
                }
                else
                {
                    ViewBag.error = "Có lỗi trong quá trình upload";
                }
            }
            return this.CreateProductColor(int.Parse(pid));
        }
        public ActionResult RemoveProductColor(int cid, int pid)
        {
            ProductColor p = data.ProductColors.SingleOrDefault(i => i.ColorId == cid);
            if (p == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //var filePath = Server.MapPath("~/images/products" + p.ImageProduct);
            var filePath = Path.Combine(Server.MapPath("~/images/products"), p.ImageColor);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            data.ProductColors.DeleteOnSubmit(p);
            data.SubmitChanges();
            return RedirectToAction("DetailColorP", new { id = pid});
        }
        [HttpGet]
        public ActionResult EditProductColor( int cid, int pid)
        {
            if (Session["AdminAccount"] == null || Session["AdminAccount"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ProductColor p = data.ProductColors.SingleOrDefault(n => n.ColorId == cid); 
            ViewData["ProductID"] = pid;
            if (p == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewData["Image"] = p.ImageColor;
            return View(p);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditProductColor(ProductColor p, HttpPostedFileBase NewImageP, FormCollection col)
        {
            int pid = int.Parse(col["ProductId"]);
            int cid = p.ColorId;
            //ViewData["ProductID"] = pid;
            var image = col["ImageP"];
            ProductColor item = data.ProductColors.SingleOrDefault(n => n.ColorId == cid);

            if (ModelState.IsValid)
            {
                if (NewImageP != null)
                {
                    if (Path.GetExtension(NewImageP.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(NewImageP.FileName).ToLower() == ".png"
                        || Path.GetExtension(NewImageP.FileName).ToLower() == ".gif"
                        || Path.GetExtension(NewImageP.FileName).ToLower() == ".jpeg"
                        )
                    {
                        var filename = Path.GetFileName(NewImageP.FileName);
                        var path = Path.Combine(Server.MapPath("~/images/products"), filename);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.error = "Tên ảnh đã tồn tại";
                        }
                        else
                        {
                            var newImageP = NewImageP.FileName;
                            NewImageP.SaveAs(path);
                            item.ImageColor = newImageP;
                            UpdateModel(item);
                            data.SubmitChanges();
                            return RedirectToAction("DetailColorP", new { id = pid});
                        }
                    }
                    else
                    {
                        ViewBag.error = "Hãy nhập một ảnh";
                    }
                }
                else
                {
                    item.ImageColor = image;
                    UpdateModel(item);
                    data.SubmitChanges();
                    return RedirectToAction("DetailColorP", new { id = pid});
                }
            }
            return this.EditProductColor(cid, pid);
        }
    }
}