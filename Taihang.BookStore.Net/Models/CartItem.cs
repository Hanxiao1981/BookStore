using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taihang.BookStore.Net.Models
{
    public class CartItem
    {
        public int ID { get; set; } // 自增主键

        [StringLength(50)]
        public string CartId { get; set; } // 购物车ID
        public DateTime CreateDate { get; set; } // 创建日期，超过一定期限后自动清空
        public int Count { get; set; } // 数量

        public int BookID { get; set; } // 图书ID       
        public virtual Book Book { get; set; } // 导航属性
    }
}