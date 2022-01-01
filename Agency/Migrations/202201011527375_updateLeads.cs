namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLeads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leads", "search_location", c => c.String());
            AddColumn("dbo.Leads", "assigned_user_id", c => c.Int());
            CreateIndex("dbo.Leads", "assigned_user_id");
            AddForeignKey("dbo.Leads", "assigned_user_id", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Leads", "assigned_user_id", "dbo.Users");
            DropIndex("dbo.Leads", new[] { "assigned_user_id" });
            DropColumn("dbo.Leads", "assigned_user_id");
            DropColumn("dbo.Leads", "search_location");
        }
    }
}
