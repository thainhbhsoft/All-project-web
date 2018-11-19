namespace UET_BTL.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContentSurveys", "Text", c => c.String());
            AlterColumn("dbo.Students", "Name", c => c.String());
            AlterColumn("dbo.Students", "StudentCode", c => c.String());
            AlterColumn("dbo.Students", "Course", c => c.String());
            AlterColumn("dbo.Students", "DateOfBirth", c => c.DateTime());
            AlterColumn("dbo.Students", "Email", c => c.String());
            AlterColumn("dbo.Students", "UserName", c => c.String());
            AlterColumn("dbo.Students", "PassWord", c => c.String());
            AlterColumn("dbo.Teachers", "Name", c => c.String());
            AlterColumn("dbo.Teachers", "Email", c => c.String());
            AlterColumn("dbo.Teachers", "UserName", c => c.String());
            AlterColumn("dbo.Teachers", "PassWord", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teachers", "PassWord", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "PassWord", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "DateOfBirth", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Students", "Course", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "StudentCode", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ContentSurveys", "Text", c => c.String(nullable: false));
        }
    }
}
