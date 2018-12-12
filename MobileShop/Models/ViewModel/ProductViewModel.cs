using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileShop.Models.ViewModel
{
    public class ProductViewModel
    {
        private int? id, idcolor;
        private int? quantity;
        private string name, detail, sale, image, screen, cpu, type, brand, color, imagecolor, colorname, typename, brandname;
        private DateTime? updateDate;
        private decimal? price;
        

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

        public string Detail
        {
            get
            {
                return detail;
            }

            set
            {
                detail = value;
            }
        }

        public string Sale
        {
            get
            {
                return sale;
            }

            set
            {
                sale = value;
            }
        }

        public string Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        public string Screen
        {
            get
            {
                return screen;
            }

            set
            {
                screen = value;
            }
        }

        public string Cpu
        {
            get
            {
                return cpu;
            }

            set
            {
                cpu = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Brand
        {
            get
            {
                return brand;
            }

            set
            {
                brand = value;
            }
        }

        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public string Imagecolor
        {
            get
            {
                return imagecolor;
            }

            set
            {
                imagecolor = value;
            }
        }

        public string ColorName
        {
            get
            {
                return colorname;
            }

            set
            {
                colorname = value;
            }
        }

        public decimal? Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public int? Id
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

        public int? IdColor
        {
            get
            {
                return idcolor;
            }

            set
            {
                idcolor = value;
            }
        }

        public int? Quantity
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

        public DateTime? UpdateDate
        {
            get
            {
                return updateDate;
            }

            set
            {
                updateDate = value;
            }
        }

        public string TypeName
        {
            get
            {
                return typename;
            }

            set
            {
                typename = value;
            }
        }

        public string BrandName
        {
            get
            {
                return brandname;
            }

            set
            {
                brandname = value;
            }
        }

        public ProductViewModel()
        {

        }
    }
}