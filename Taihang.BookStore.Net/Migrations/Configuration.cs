namespace Taihang.BookStore.Net.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Taihang.BookStore.Net.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            // 禁用自动迁移，以手动运行命令的方式迁移
            AutomaticMigrationsEnabled = false;
        }

        // 每当创建数据库或模型更改后调用
        protected override void Seed(ApplicationDbContext context)
        {
            #region 初始化用户和角色
            bool succeeded = true;

            string adminEmail = "xiao4407@163.com", adminPswd = "Admin#123", adminRole = "Admins", userRole = "Users";

            // 添加2个角色: Admins和Users
            List<ApplicationRole> appRoles = new List<ApplicationRole>();

            appRoles.Add(new ApplicationRole { Name = adminRole, Describe = "管理员" });
            appRoles.Add(new ApplicationRole { Name = userRole, Describe = "普通用户" });

            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
            foreach (ApplicationRole role in appRoles)
            {
                if (succeeded && roleManager.FindByName(role.Name) == null)
                {
                    var result = roleManager.Create(role);
                    succeeded = result.Succeeded;
                }
            }

            if (!succeeded) throw new Exception("初始化角色数据出错。");
            context.SaveChanges(); // 保存更改

            // 创建管理员xiao4407@163.com
            var userStore = new UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(context);          
            var userManager = new ApplicationUserManager(userStore);

            if (userManager.FindByEmail(adminEmail) == null)
            {
                var result = userManager.Create(new ApplicationUser { UserName = adminEmail, Email = adminEmail }, adminPswd); // 密码Admin#123
                succeeded = result.Succeeded;
            }

            if (!succeeded) throw new Exception("初始化用户数据出错。");
            context.SaveChanges(); // 保存更改

            // 将用户xiao4407@163.com添加到管理组
            var admin = userManager.FindByEmail(adminEmail);
            if (admin != null && !userManager.IsInRole(admin.Id, adminRole))
            {
                var result = userManager.AddToRole(admin.Id, adminRole);
                succeeded = result.Succeeded;                
            }

            if (!succeeded) throw new Exception("初始化角色数据出错。");
            context.SaveChanges(); // 保存更改
            #endregion

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            #region 添加图书
            List<BookCategory> categoryList = new List<BookCategory>
            {
                new BookCategory { ID = 1, Name = "宗教哲学" },
                new BookCategory { ID = 2, Name = "国学典籍" },
                new BookCategory { ID = 3, Name = "计算机与互联网" }
            };

            categoryList.ForEach(c => context.BookCategory.AddOrUpdate(categoryList.ToArray()));
            context.SaveChanges();

            List<Book> bookList = new List<Book>
            {
                new Book { ID = 1, CategoryID = 1, Name = "宗教哲学1", Author = "作者A", Describe = "第1本宗教哲学图书", Price = 20, Img = "a.jpg" },
                new Book { ID = 2, CategoryID = 1, Name = "宗教哲学2", Author = "作者B", Describe = "第2本宗教哲学图书", Price = 30, Img = "a.jpg" },
                new Book { ID = 3, CategoryID = 1, Name = "宗教哲学3", Author = "作者C", Describe = "第3本宗教哲学图书", Price = 40, Img = "a.jpg" },
                new Book { ID = 4, CategoryID = 2, Name = "国学典籍1", Author = "作者A", Describe = "第1本国学典籍图书", Price = 50, Img = "b.jpg" },
                new Book { ID = 5, CategoryID = 2, Name = "国学典籍2", Author = "作者B", Describe = "第2本国学典籍图书", Price = 60, Img = "b.jpg" },
                new Book { ID = 6, CategoryID = 2, Name = "国学典籍3", Author = "作者C", Describe = "第3本国学典籍图书", Price = 70, Img = "b.jpg" },
                new Book { ID = 7, CategoryID = 3, Name = "计算机1", Author = "作者A", Describe = "第1本计算机图书", Price = 120, Img = "c.jpg" },
                new Book { ID = 8, CategoryID = 3, Name = "计算机2", Author = "作者B", Describe = "第2本计算机图书", Price = 130, Img = "c.jpg" },
                new Book { ID = 9, CategoryID = 1, Name = "宗教哲学1", Author = "作者A", Describe = "第1本宗教哲学图书", Price = 20, Img = "a.jpg" },
                new Book { ID = 10, CategoryID = 1, Name = "宗教哲学2", Author = "作者B", Describe = "第2本宗教哲学图书", Price = 30, Img = "a.jpg" },
                new Book { ID = 11, CategoryID = 1, Name = "宗教哲学3", Author = "作者C", Describe = "第3本宗教哲学图书", Price = 40, Img = "a.jpg" },
                new Book { ID = 12, CategoryID = 2, Name = "国学典籍1", Author = "作者A", Describe = "第1本国学典籍图书", Price = 50, Img = "b.jpg" },
                new Book { ID = 13, CategoryID = 2, Name = "国学典籍2", Author = "作者B", Describe = "第2本国学典籍图书", Price = 60, Img = "b.jpg" },
                new Book { ID = 14, CategoryID = 2, Name = "国学典籍3", Author = "作者C", Describe = "第3本国学典籍图书", Price = 70, Img = "b.jpg" },
                new Book { ID = 15, CategoryID = 3, Name = "计算机1", Author = "作者A", Describe = "第1本计算机图书", Price = 120, Img = "c.jpg" },
                new Book { ID = 16, CategoryID = 3, Name = "计算机2", Author = "作者B", Describe = "第2本计算机图书", Price = 130, Img = "c.jpg" },
                new Book { ID = 17, CategoryID = 1, Name = "宗教哲学1", Author = "作者A", Describe = "第1本宗教哲学图书", Price = 20, Img = "a.jpg" },
                new Book { ID = 18, CategoryID = 1, Name = "宗教哲学2", Author = "作者B", Describe = "第2本宗教哲学图书", Price = 30, Img = "a.jpg" },
                new Book { ID = 19, CategoryID = 1, Name = "宗教哲学3", Author = "作者C", Describe = "第3本宗教哲学图书", Price = 40, Img = "a.jpg" },
                new Book { ID = 20, CategoryID = 2, Name = "国学典籍1", Author = "作者A", Describe = "第1本国学典籍图书", Price = 50, Img = "b.jpg" },
                new Book { ID = 21, CategoryID = 2, Name = "国学典籍2", Author = "作者B", Describe = "第2本国学典籍图书", Price = 60, Img = "b.jpg" },
                new Book { ID = 22, CategoryID = 2, Name = "国学典籍3", Author = "作者C", Describe = "第3本国学典籍图书", Price = 70, Img = "b.jpg" },
                new Book { ID = 23, CategoryID = 3, Name = "计算机1", Author = "作者A", Describe = "第1本计算机图书", Price = 120, Img = "c.jpg" },
                new Book { ID = 24, CategoryID = 3, Name = "计算机2", Author = "作者B", Describe = "第2本计算机图书", Price = 130, Img = "c.jpg" },
                new Book { ID = 25, CategoryID = 1, Name = "宗教哲学1", Author = "作者A", Describe = "第1本宗教哲学图书", Price = 20, Img = "a.jpg" },
                new Book { ID = 26, CategoryID = 1, Name = "宗教哲学2", Author = "作者B", Describe = "第2本宗教哲学图书", Price = 30, Img = "a.jpg" },
                new Book { ID = 27, CategoryID = 1, Name = "宗教哲学3", Author = "作者C", Describe = "第3本宗教哲学图书", Price = 40, Img = "a.jpg" },
                new Book { ID = 28, CategoryID = 2, Name = "国学典籍1", Author = "作者A", Describe = "第1本国学典籍图书", Price = 50, Img = "b.jpg" },
                new Book { ID = 29, CategoryID = 2, Name = "国学典籍2", Author = "作者B", Describe = "第2本国学典籍图书", Price = 60, Img = "b.jpg" },
                new Book { ID = 30, CategoryID = 2, Name = "国学典籍3", Author = "作者C", Describe = "第3本国学典籍图书", Price = 70, Img = "b.jpg" },
                new Book { ID = 31, CategoryID = 3, Name = "计算机1", Author = "作者A", Describe = "第1本计算机图书", Price = 120, Img = "c.jpg" },
                new Book { ID = 32, CategoryID = 3, Name = "计算机2", Author = "作者B", Describe = "第2本计算机图书", Price = 130, Img = "c.jpg" },
                new Book { ID = 33, CategoryID = 3, Name = "计算机3", Author = "作者C", Describe = "第3本计算机图书", Price = 140, Img = "c.jpg" }
            };

            bookList.ForEach(b => context.Book.AddOrUpdate(bookList.ToArray()));
            context.SaveChanges();
            #endregion
        }
    }
}
