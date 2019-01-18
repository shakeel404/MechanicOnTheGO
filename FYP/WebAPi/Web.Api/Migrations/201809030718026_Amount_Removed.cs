namespace Web.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Amount_Removed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contracts", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contracts", "Amount", c => c.Int(nullable: false));
        }
    }
}
