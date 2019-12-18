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
            // �����Զ�Ǩ�ƣ����ֶ���������ķ�ʽǨ��
            AutomaticMigrationsEnabled = false;
        }

        // ÿ���������ݿ��ģ�͸��ĺ����
        protected override void Seed(ApplicationDbContext context)
        {
            #region ��ʼ���û��ͽ�ɫ
            bool succeeded = true;

            string adminEmail = "xiao4407@163.com", adminPswd = "Admin#123", adminRole = "Admins", userRole = "Users";

            // ���2����ɫ: Admins��Users
            List<ApplicationRole> appRoles = new List<ApplicationRole>();

            appRoles.Add(new ApplicationRole { Name = adminRole, Describe = "����Ա" });
            appRoles.Add(new ApplicationRole { Name = userRole, Describe = "��ͨ�û�" });

            var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));
            foreach (ApplicationRole role in appRoles)
            {
                if (succeeded && roleManager.FindByName(role.Name) == null)
                {
                    var result = roleManager.Create(role);
                    succeeded = result.Succeeded;
                }
            }

            if (!succeeded) throw new Exception("��ʼ����ɫ���ݳ���");
            context.SaveChanges(); // �������

            // ��������Աxiao4407@163.com
            var userStore = new UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(context);          
            var userManager = new ApplicationUserManager(userStore);

            if (userManager.FindByEmail(adminEmail) == null)
            {
                var result = userManager.Create(new ApplicationUser { UserName = adminEmail, Email = adminEmail }, adminPswd); // ����Admin#123
                succeeded = result.Succeeded;
            }

            if (!succeeded) throw new Exception("��ʼ���û����ݳ���");
            context.SaveChanges(); // �������

            // ���û�xiao4407@163.com��ӵ�������
            var admin = userManager.FindByEmail(adminEmail);
            if (admin != null && !userManager.IsInRole(admin.Id, adminRole))
            {
                var result = userManager.AddToRole(admin.Id, adminRole);
                succeeded = result.Succeeded;                
            }

            if (!succeeded) throw new Exception("��ʼ����ɫ���ݳ���");
            context.SaveChanges(); // �������
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

            #region ���ͼ��
            List<BookCategory> categoryList = new List<BookCategory>
            {
                new BookCategory { ID = 1, Name = "�ڽ���ѧ" },
                new BookCategory { ID = 2, Name = "��ѧ�伮" },
                new BookCategory { ID = 3, Name = "������뻥����" }
            };

            categoryList.ForEach(c => context.BookCategory.AddOrUpdate(categoryList.ToArray()));
            context.SaveChanges();

            List<Book> bookList = new List<Book>
            {
                new Book { ID = 1, CategoryID = 1, Name = "�ڽ���ѧ1", Author = "����A", Describe = "��1���ڽ���ѧͼ��", Price = 20, Img = "a.jpg" },
                new Book { ID = 2, CategoryID = 1, Name = "�ڽ���ѧ2", Author = "����B", Describe = "��2���ڽ���ѧͼ��", Price = 30, Img = "a.jpg" },
                new Book { ID = 3, CategoryID = 1, Name = "�ڽ���ѧ3", Author = "����C", Describe = "��3���ڽ���ѧͼ��", Price = 40, Img = "a.jpg" },
                new Book { ID = 4, CategoryID = 2, Name = "��ѧ�伮1", Author = "����A", Describe = "��1����ѧ�伮ͼ��", Price = 50, Img = "b.jpg" },
                new Book { ID = 5, CategoryID = 2, Name = "��ѧ�伮2", Author = "����B", Describe = "��2����ѧ�伮ͼ��", Price = 60, Img = "b.jpg" },
                new Book { ID = 6, CategoryID = 2, Name = "��ѧ�伮3", Author = "����C", Describe = "��3����ѧ�伮ͼ��", Price = 70, Img = "b.jpg" },
                new Book { ID = 7, CategoryID = 3, Name = "�����1", Author = "����A", Describe = "��1�������ͼ��", Price = 120, Img = "c.jpg" },
                new Book { ID = 8, CategoryID = 3, Name = "�����2", Author = "����B", Describe = "��2�������ͼ��", Price = 130, Img = "c.jpg" },
                new Book { ID = 9, CategoryID = 1, Name = "�ڽ���ѧ1", Author = "����A", Describe = "��1���ڽ���ѧͼ��", Price = 20, Img = "a.jpg" },
                new Book { ID = 10, CategoryID = 1, Name = "�ڽ���ѧ2", Author = "����B", Describe = "��2���ڽ���ѧͼ��", Price = 30, Img = "a.jpg" },
                new Book { ID = 11, CategoryID = 1, Name = "�ڽ���ѧ3", Author = "����C", Describe = "��3���ڽ���ѧͼ��", Price = 40, Img = "a.jpg" },
                new Book { ID = 12, CategoryID = 2, Name = "��ѧ�伮1", Author = "����A", Describe = "��1����ѧ�伮ͼ��", Price = 50, Img = "b.jpg" },
                new Book { ID = 13, CategoryID = 2, Name = "��ѧ�伮2", Author = "����B", Describe = "��2����ѧ�伮ͼ��", Price = 60, Img = "b.jpg" },
                new Book { ID = 14, CategoryID = 2, Name = "��ѧ�伮3", Author = "����C", Describe = "��3����ѧ�伮ͼ��", Price = 70, Img = "b.jpg" },
                new Book { ID = 15, CategoryID = 3, Name = "�����1", Author = "����A", Describe = "��1�������ͼ��", Price = 120, Img = "c.jpg" },
                new Book { ID = 16, CategoryID = 3, Name = "�����2", Author = "����B", Describe = "��2�������ͼ��", Price = 130, Img = "c.jpg" },
                new Book { ID = 17, CategoryID = 1, Name = "�ڽ���ѧ1", Author = "����A", Describe = "��1���ڽ���ѧͼ��", Price = 20, Img = "a.jpg" },
                new Book { ID = 18, CategoryID = 1, Name = "�ڽ���ѧ2", Author = "����B", Describe = "��2���ڽ���ѧͼ��", Price = 30, Img = "a.jpg" },
                new Book { ID = 19, CategoryID = 1, Name = "�ڽ���ѧ3", Author = "����C", Describe = "��3���ڽ���ѧͼ��", Price = 40, Img = "a.jpg" },
                new Book { ID = 20, CategoryID = 2, Name = "��ѧ�伮1", Author = "����A", Describe = "��1����ѧ�伮ͼ��", Price = 50, Img = "b.jpg" },
                new Book { ID = 21, CategoryID = 2, Name = "��ѧ�伮2", Author = "����B", Describe = "��2����ѧ�伮ͼ��", Price = 60, Img = "b.jpg" },
                new Book { ID = 22, CategoryID = 2, Name = "��ѧ�伮3", Author = "����C", Describe = "��3����ѧ�伮ͼ��", Price = 70, Img = "b.jpg" },
                new Book { ID = 23, CategoryID = 3, Name = "�����1", Author = "����A", Describe = "��1�������ͼ��", Price = 120, Img = "c.jpg" },
                new Book { ID = 24, CategoryID = 3, Name = "�����2", Author = "����B", Describe = "��2�������ͼ��", Price = 130, Img = "c.jpg" },
                new Book { ID = 25, CategoryID = 1, Name = "�ڽ���ѧ1", Author = "����A", Describe = "��1���ڽ���ѧͼ��", Price = 20, Img = "a.jpg" },
                new Book { ID = 26, CategoryID = 1, Name = "�ڽ���ѧ2", Author = "����B", Describe = "��2���ڽ���ѧͼ��", Price = 30, Img = "a.jpg" },
                new Book { ID = 27, CategoryID = 1, Name = "�ڽ���ѧ3", Author = "����C", Describe = "��3���ڽ���ѧͼ��", Price = 40, Img = "a.jpg" },
                new Book { ID = 28, CategoryID = 2, Name = "��ѧ�伮1", Author = "����A", Describe = "��1����ѧ�伮ͼ��", Price = 50, Img = "b.jpg" },
                new Book { ID = 29, CategoryID = 2, Name = "��ѧ�伮2", Author = "����B", Describe = "��2����ѧ�伮ͼ��", Price = 60, Img = "b.jpg" },
                new Book { ID = 30, CategoryID = 2, Name = "��ѧ�伮3", Author = "����C", Describe = "��3����ѧ�伮ͼ��", Price = 70, Img = "b.jpg" },
                new Book { ID = 31, CategoryID = 3, Name = "�����1", Author = "����A", Describe = "��1�������ͼ��", Price = 120, Img = "c.jpg" },
                new Book { ID = 32, CategoryID = 3, Name = "�����2", Author = "����B", Describe = "��2�������ͼ��", Price = 130, Img = "c.jpg" },
                new Book { ID = 33, CategoryID = 3, Name = "�����3", Author = "����C", Describe = "��3�������ͼ��", Price = 140, Img = "c.jpg" }
            };

            bookList.ForEach(b => context.Book.AddOrUpdate(bookList.ToArray()));
            context.SaveChanges();
            #endregion
        }
    }
}
