namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLeadStage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Leads", "lead_stage_id", "dbo.LeadStages");
            DropIndex("dbo.Leads", new[] { "lead_stage_id" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Leads", "lead_stage_id");
            AddForeignKey("dbo.Leads", "lead_stage_id", "dbo.LeadStages", "id");
        }
    }
}
