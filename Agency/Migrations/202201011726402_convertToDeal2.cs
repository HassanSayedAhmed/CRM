namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class convertToDeal2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Leads", "deal_property_price", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Leads", "deal_property_price", c => c.Single(nullable: false));
        }
    }
}
