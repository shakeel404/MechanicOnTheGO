namespace Web.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoursWorkedRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contracts", "HoursWorked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contracts", "HoursWorked", c => c.Int(nullable: false));
        }
    }
}
