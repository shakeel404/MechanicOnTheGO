namespace Web.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeTypeChangedToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contracts", "WorkStarted", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Contracts", "WorkFinished", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contracts", "WorkFinished", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Contracts", "WorkStarted", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
