using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Taihang.BookStore.Net.Models
{
    public class BookCategory
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("类别")]
        public string Name { get; set; } // 类别名称

        // 导航属性定义为virtual
        public virtual ICollection<Book> Books { get; set; }
    }
}