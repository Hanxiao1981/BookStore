using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Taihang.BookStore.Net.Models
{
    // 可以通过将更多属性添加到 ApplicationUser 类来为用户添加配置文件数据，请访问 https://go.microsoft.com/fwlink/?LinkID=317594 了解详细信息。
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }

        [DisplayName("状态")]
        public bool Disable { get; set; } // 禁用

        //==================外键和导航部分=====================

        [StringLength(100)]
        public string ProfileID { get; set; } // 外键，命名约定：导航属性+主键 > 主类+主键 > 主键名
        public virtual UserProfile Profile { get; set; } // 导航属性

        //================外键和导航部分结束====================
    }

    // 用户角色
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        [DisplayName("说明")]
        [StringLength(100)]
        public string Describe { get; set; } // 角色说明

        [DisplayName("状态")]
        public bool Disable { get; set; } // 禁用
    }

    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // 在此更改默认约定
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 本句必须加上
            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); // 删除复数表名约定
        }
        
        public DbSet<UserProfile> UserProfile { get; set; } // 用户个人信息
    }
}