using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taihang.BookStore.Net.Models
{
    public class Book
    {
        public int ID { get; set; }

        [StringLength(50)]
        [DisplayName("封面")]
        public string Img { get; set; } //封面

        [StringLength(50)]
        [DisplayName("书名")]
        public string Name { get; set; } // 书名

        [StringLength(50)]
        [DisplayName("作者")]
        public string Author { get; set; } // 作者

        [StringLength(100)]
        [DisplayName("描述")]
        public string Describe { get; set; } // 描述

        [DisplayName("价格")]
        public decimal Price { get; set; } // 价格 

        [DisplayName("状态")]
        public bool Disable { get; set; } // 禁用(下架)

        [DisplayName("类别")]
        public int CategoryID { get; set; } // 外键，导航属性 + 主键
        public virtual BookCategory Category { get; set; } // 导航属性
    }
}