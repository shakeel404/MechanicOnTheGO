namespace Web.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeChangedToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contracts", "Time", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contracts", "Time", c => c.DateTime(nullable: false));
        }
    }
}
