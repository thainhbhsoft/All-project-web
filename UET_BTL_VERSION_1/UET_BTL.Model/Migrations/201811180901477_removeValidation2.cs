namespace UET_BTL.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeValidation2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "DateOfBirth", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "DateOfBirth", c => c.DateTime());
        }
    }
}
