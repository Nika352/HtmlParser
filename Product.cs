using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser
{
    public class Product
    {
        public string productName { get; set; }
        public string price { get; set; }
        public string rating { get; set; }

        public Product(string productName, string price, string rating) { 
            this.productName = productName;
            this.price = price;
            this.rating = rating;
        }
    }
}
