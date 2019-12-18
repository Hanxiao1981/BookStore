using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taihang.BookStore.Net.Models
{
    public class OrderItem
    {
        public int ID { get; set; }

        //================图书快照信息，防止图书信息发生变动======
        [StringLength(50)]
        [DisplayName("封面")]
        public string Img { get; set; } //封面

        [StringLength(50)]
        [DisplayName("书名")]
        public string Name { get; set; } // 书名

        [StringLength(50)]
        [DisplayName("作者")]
        public string Author { get; set; } // 作者

        [DisplayName("价格")]
        public decimal Price { get; set; } // 价格
        //================快照信息结束==============================

        [DisplayName("数量")]
        public int Count { get; set; } // 数量      

        public int BookID { get; set; } // 图书ID
        public int OrderID { get; set; } // 订单号
         
        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }
    }
}