using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taihang.BookStore.Net.Models
{
    // 用户个人信息
    public class UserProfile
    {
        // 主键，命名约定: ID || 类名+ID
        public string ID { get; set; }

        [StringLength(50)]
        [DisplayName("姓名")]
        public string RealName { get; set; } // 真实姓名
        [StringLength(50)]
        [DisplayName("电话")]
        public string Phone { get; set; } // 电话
        [StringLength(100)]
        [DisplayName("地址")]
        public string Address { get; set; } // 地址

        public DateTime CreateTime { get; set; } // 创建时间
        public DateTime? UpdateTime { get; set; } // 更新时间
    }
}