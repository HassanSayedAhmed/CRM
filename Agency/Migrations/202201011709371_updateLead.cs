namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLead : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leads", "decision_id", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leads", "decision_id");
        }
    }
}
