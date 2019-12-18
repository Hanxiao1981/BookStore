using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taihang.BookStore.Net.Models;

namespace Taihang.BookStore.Net.Areas.Admin.Models
{
    public class UserRolesViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }

        // 用户是否隶属于某个角色
        public bool Selected { get; set; }
    }
}