namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crm_initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        activity_type = c.Int(),
                        activity_date_time = c.DateTime(),
                        activity_duration = c.Single(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        from_user = c.Int(),
                        to_user = c.Int(),
                        message = c.String(),
                        created_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Leads",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        user_name = c.String(),
                        first_name = c.String(),
                        middle_name = c.String(),
                        last_name = c.String(),
                        email = c.String(),
                        password = c.String(),
                        phone1 = c.String(),
                        phone2 = c.String(),
                        address1 = c.String(),
                        address2 = c.String(),
                        gender = c.Int(),
                        nationality = c.String(),
                        job = c.String(),
                        company = c.String(),
                        salary = c.Double(),
                        currency = c.String(),
                        birthDate = c.DateTime(),
                        image = c.String(),
                        follow_up = c.DateTime(),
                        notes = c.String(),
                        lead_owner = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        assigned_to = c.Int(),
                        activity_id = c.Int(),
                        source_id = c.Int(),
                        status_id = c.Int(),
                        type_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Activities", t => t.activity_id)
                .ForeignKey("dbo.LeadTypes", t => t.type_id)
                .ForeignKey("dbo.Sources", t => t.source_id)
                .ForeignKey("dbo.Status", t => t.status_id)
                .ForeignKey("dbo.Users", t => t.assigned_to)
                .Index(t => t.assigned_to)
                .Index(t => t.activity_id)
                .Index(t => t.source_id)
                .Index(t => t.status_id)
                .Index(t => t.type_id);
            
            CreateTable(
                "dbo.LeadTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        link = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        user_name = c.String(),
                        full_name = c.String(),
                        first_name = c.String(),
                        middle_name = c.String(),
                        last_name = c.String(),
                        email = c.String(),
                        password = c.String(),
                        phone1 = c.String(),
                        phone2 = c.String(),
                        address1 = c.String(),
                        address2 = c.String(),
                        gender = c.Int(),
                        nationality = c.String(),
                        job = c.String(),
                        company = c.String(),
                        salary = c.Double(),
                        currency = c.String(),
                        birthDate = c.DateTime(),
                        image = c.String(),
                        type = c.Int(),
                        status = c.Int(),
                        lead_owner = c.Int(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Leads", "assigned_to", "dbo.Users");
            DropForeignKey("dbo.Leads", "status_id", "dbo.Status");
            DropForeignKey("dbo.Leads", "source_id", "dbo.Sources");
            DropForeignKey("dbo.Leads", "type_id", "dbo.LeadTypes");
            DropForeignKey("dbo.Leads", "activity_id", "dbo.Activities");
            DropIndex("dbo.Leads", new[] { "type_id" });
            DropIndex("dbo.Leads", new[] { "status_id" });
            DropIndex("dbo.Leads", new[] { "source_id" });
            DropIndex("dbo.Leads", new[] { "activity_id" });
            DropIndex("dbo.Leads", new[] { "assigned_to" });
            DropTable("dbo.Users");
            DropTable("dbo.Status");
            DropTable("dbo.Sources");
            DropTable("dbo.LeadTypes");
            DropTable("dbo.Leads");
            DropTable("dbo.Chats");
            DropTable("dbo.Activities");
        }
    }
}
