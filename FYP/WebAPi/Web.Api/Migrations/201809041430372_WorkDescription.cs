namespace Web.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contracts", "HoursWorked", c => c.Int(nullable: false));
            AddColumn("dbo.Contracts", "WorkStarted", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contracts", "IsWorkStarted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Contracts", "IsWorkFinished", c => c.Boolean(nullable: false));
            AddColumn("dbo.Contracts", "WorkFinished", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contracts", "WorkFinished");
            DropColumn("dbo.Contracts", "IsWorkFinished");
            DropColumn("dbo.Contracts", "IsWorkStarted");
            DropColumn("dbo.Contracts", "WorkStarted");
            DropColumn("dbo.Contracts", "HoursWorked");
        }
    }
}
