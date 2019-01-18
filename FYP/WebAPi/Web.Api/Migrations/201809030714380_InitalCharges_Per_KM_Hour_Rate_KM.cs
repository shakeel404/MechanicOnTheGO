namespace Web.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalCharges_Per_KM_Hour_Rate_KM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contracts", "InitialCharges", c => c.Int(nullable: false));
            AddColumn("dbo.Contracts", "RatePerKM", c => c.Int(nullable: false));
            AddColumn("dbo.Contracts", "RatePerHour", c => c.Int(nullable: false));
            AddColumn("dbo.Contracts", "KM", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contracts", "KM");
            DropColumn("dbo.Contracts", "RatePerHour");
            DropColumn("dbo.Contracts", "RatePerKM");
            DropColumn("dbo.Contracts", "InitialCharges");
        }
    }
}
