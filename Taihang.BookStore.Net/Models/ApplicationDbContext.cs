using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Taihang.BookStore.Net.Models
{
    // 商品、购物车、订单相关内容
    public partial class ApplicationDbContext
    {
        public DbSet<BookCategory> BookCategory { get; set; } // 图书分类表
        public DbSet<Book> Book { get; set; } // 图书表

        public DbSet<CartItem> Cart { get; set; } // 购物车

        public DbSet<Order> Order { get; set; } // 订单
        public DbSet<OrderItem> OrderItem { get; set; } // 订单明细
    }
}