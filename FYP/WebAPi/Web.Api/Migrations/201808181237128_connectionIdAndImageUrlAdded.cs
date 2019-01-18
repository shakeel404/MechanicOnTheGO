namespace Web.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class connectionIdAndImageUrlAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CurrentConnection", c => c.String(maxLength: 200));
            AddColumn("dbo.Customers", "ImageUrl", c => c.String(maxLength: 200));
            AddColumn("dbo.Mechanics", "CurrentConnection", c => c.String(maxLength: 200));
            AddColumn("dbo.Mechanics", "ImageUrl", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mechanics", "ImageUrl");
            DropColumn("dbo.Mechanics", "CurrentConnection");
            DropColumn("dbo.Customers", "ImageUrl");
            DropColumn("dbo.Customers", "CurrentConnection");
        }
    }
}
