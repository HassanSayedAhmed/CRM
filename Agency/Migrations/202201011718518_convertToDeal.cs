namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class convertToDeal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leads", "deal_property_id", c => c.Int());
            AddColumn("dbo.Leads", "deal_property_price", c => c.Single(nullable: false));
            AddColumn("dbo.Leads", "deal_make_user_id", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leads", "deal_make_user_id");
            DropColumn("dbo.Leads", "deal_property_price");
            DropColumn("dbo.Leads", "deal_property_id");
        }
    }
}
