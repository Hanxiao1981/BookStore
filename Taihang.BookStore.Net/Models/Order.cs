using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taihang.BookStore.Net.Models
{
    // 订单状态
    public enum OrderStateEnum { 已下单 = 1, 已付款, 已发货, 已完成, 已废弃 = 99 }

    public class Order
    {
        [DisplayName("订单ID")]
        public int ID { get; set; } // 订单ID       

        [StringLength(50)]
        [DisplayName("用户ID")]
        public string UserID { get; set; } // 用户ID

        [StringLength(50)]
        [DisplayName("收货人")]
        public string ReciverName { get; set; } // 收货人

        [StringLength(50)]
        [DisplayName("联系电话")]
        public string ReciverPhone { get; set; } // 联系电话

        [StringLength(100)]
        [DisplayName("邮寄地址")]
        public string PostAddr { get; set; } // 邮寄地址

        [DisplayName("订单金额")]
        public decimal TotalSum { get; set; } // 订单总金额

        [DisplayName("订单日期")]
        public DateTime OrderDate { get; set; } // 订单日期

        [StringLength(50)]
        [DisplayName("订单状态")]
        public string StateDescribe { get; set; } // 状态描述

        public OrderStateEnum OrderState { get; set; } // 订单状态

        public virtual ICollection<OrderItem> OrderItems { get; set; } // 订单条目
    }
}