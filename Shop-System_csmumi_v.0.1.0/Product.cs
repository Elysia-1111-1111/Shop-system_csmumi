using System;
using System.Collections.Generic;
using System.Text;

namespace Shop_System_csmumi_v._0._1._0
{
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

    }
    public class CartItem
    {
        public Product productInfo { get; set; }
        public int Quantity { get; set; }
        public int SubTotal => productInfo.Price * Quantity;
    }
            


}
