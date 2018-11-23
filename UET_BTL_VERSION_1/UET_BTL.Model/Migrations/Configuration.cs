namespace UET_BTL.Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<UET_BTL.Model.UetSurveyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UET_BTL.Model.UetSurveyDbContext context)
        {
            if (!context.Users.Any(s => s.Position == "Admin"))
            {
                context.Users.Add(new Entities.User { UserName = "admin", PassWord = "admin", Position = "Admin" });
            }
        }
    }
}
