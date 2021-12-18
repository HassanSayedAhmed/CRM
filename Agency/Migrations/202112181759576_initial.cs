namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LeadTypes", newName: "Companies");
            RenameTable(name: "dbo.Status", newName: "EmploymentTypes");
            DropForeignKey("dbo.Leads", "activity_id", "dbo.Activities");
            DropForeignKey("dbo.Leads", "assigned_to", "dbo.Users");
            DropIndex("dbo.Leads", new[] { "assigned_to" });
            DropIndex("dbo.Leads", new[] { "activity_id" });
            RenameColumn(table: "dbo.Leads", name: "type_id", newName: "company_id");
            RenameColumn(table: "dbo.Leads", name: "status_id", newName: "employment_type_id");
            RenameIndex(table: "dbo.Leads", name: "IX_status_id", newName: "IX_employment_type_id");
            RenameIndex(table: "dbo.Leads", name: "IX_type_id", newName: "IX_company_id");
            CreateTable(
                "dbo.LeadActivities",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        activity_id = c.Int(),
                        activity_date_time = c.DateTime(),
                        activity_duration = c.Single(),
                        note = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        lead_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Activities", t => t.activity_id)
                .ForeignKey("dbo.Leads", t => t.lead_id)
                .Index(t => t.activity_id)
                .Index(t => t.lead_id);
            
            CreateTable(
                "dbo.LeadCategories",
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
                "dbo.LeadDevelopers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        developer_id = c.Int(),
                        note = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        lead_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Developers", t => t.developer_id)
                .ForeignKey("dbo.Leads", t => t.lead_id)
                .Index(t => t.developer_id)
                .Index(t => t.lead_id);
            
            CreateTable(
                "dbo.Developers",
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
                "dbo.Projects",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        image = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        developer_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Developers", t => t.developer_id)
                .Index(t => t.developer_id);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        price = c.Double(),
                        baths = c.Int(),
                        rooms = c.Int(),
                        beds = c.Int(),
                        area = c.String(),
                        type = c.String(),
                        status = c.String(),
                        image = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        project_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Projects", t => t.project_id)
                .Index(t => t.project_id);
            
            CreateTable(
                "dbo.LeadProjects",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        lead_id = c.Int(),
                        project_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Leads", t => t.lead_id)
                .ForeignKey("dbo.Projects", t => t.project_id)
                .Index(t => t.lead_id)
                .Index(t => t.project_id);
            
            CreateTable(
                "dbo.LeadProperties",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        property_id = c.Int(),
                        note = c.String(),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        lead_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Leads", t => t.lead_id)
                .ForeignKey("dbo.Properties", t => t.property_id)
                .Index(t => t.property_id)
                .Index(t => t.lead_id);
            
            CreateTable(
                "dbo.LeadStages",
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
                "dbo.LeadSubTypes",
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
                        lead_id = c.Int(),
                        SubType_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Leads", t => t.lead_id)
                .ForeignKey("dbo.SubTypes", t => t.SubType_id)
                .Index(t => t.lead_id)
                .Index(t => t.SubType_id);
            
            CreateTable(
                "dbo.LeadUnitTypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        active = c.Int(),
                        created_by = c.Int(),
                        updated_by = c.Int(),
                        deleted_by = c.Int(),
                        created_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                        lead_id = c.Int(),
                        unit_type_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Leads", t => t.lead_id)
                .ForeignKey("dbo.UnitTypes", t => t.unit_type_id)
                .Index(t => t.lead_id)
                .Index(t => t.unit_type_id);
            
            CreateTable(
                "dbo.UnitTypes",
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
                "dbo.PropertyTypes",
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
                "dbo.Requirements",
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
                "dbo.Timelines",
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
                "dbo.TypeOfVisitors",
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
                "dbo.SubTypes",
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
            
            AddColumn("dbo.Activities", "name", c => c.String());
            AddColumn("dbo.Activities", "description", c => c.String());
            AddColumn("dbo.Leads", "alternative_numbers", c => c.String());
            AddColumn("dbo.Leads", "type_of_visitor_id", c => c.Int());
            AddColumn("dbo.Leads", "lead_stage_id", c => c.Int());
            AddColumn("dbo.Leads", "lead_category_id", c => c.Int());
            AddColumn("dbo.Leads", "date_of_birth", c => c.DateTime());
            AddColumn("dbo.Leads", "date_of_anniversary", c => c.DateTime());
            AddColumn("dbo.Leads", "sales_agent", c => c.String());
            AddColumn("dbo.Leads", "address", c => c.String());
            AddColumn("dbo.Leads", "country", c => c.String());
            AddColumn("dbo.Leads", "property_type_id", c => c.Int());
            AddColumn("dbo.Leads", "requirement_id", c => c.Int());
            AddColumn("dbo.Leads", "budget_min", c => c.String());
            AddColumn("dbo.Leads", "budget_max", c => c.String());
            AddColumn("dbo.Leads", "minimum_area", c => c.String());
            AddColumn("dbo.Leads", "maximum_area", c => c.String());
            AddColumn("dbo.Leads", "area_metric", c => c.String());
            AddColumn("dbo.Leads", "remark", c => c.String());
            AddColumn("dbo.Leads", "street_address", c => c.String());
            AddColumn("dbo.Leads", "location", c => c.String());
            AddColumn("dbo.Leads", "sub_location", c => c.String());
            AddColumn("dbo.Leads", "state", c => c.String());
            AddColumn("dbo.Leads", "pincode", c => c.String());
            AddColumn("dbo.Leads", "location_country", c => c.String());
            AddColumn("dbo.Leads", "latitude", c => c.String());
            AddColumn("dbo.Leads", "longitude", c => c.String());
            AddColumn("dbo.Leads", "timeline_id", c => c.Int());
            AddColumn("dbo.Leads", "income", c => c.String());
            AddColumn("dbo.Leads", "designation", c => c.String());
            AddColumn("dbo.Leads", "company_name", c => c.String());
            AddColumn("dbo.Leads", "Project_id", c => c.Int());
            AddColumn("dbo.Users", "company_id", c => c.Int());
            CreateIndex("dbo.Leads", "type_of_visitor_id");
            CreateIndex("dbo.Leads", "lead_stage_id");
            CreateIndex("dbo.Leads", "lead_category_id");
            CreateIndex("dbo.Leads", "property_type_id");
            CreateIndex("dbo.Leads", "requirement_id");
            CreateIndex("dbo.Leads", "timeline_id");
            CreateIndex("dbo.Leads", "Project_id");
            CreateIndex("dbo.Users", "company_id");
            AddForeignKey("dbo.Users", "company_id", "dbo.Companies", "id");
            AddForeignKey("dbo.Leads", "lead_category_id", "dbo.LeadCategories", "id");
            AddForeignKey("dbo.Leads", "Project_id", "dbo.Projects", "id");
            AddForeignKey("dbo.Leads", "lead_stage_id", "dbo.LeadStages", "id");
            AddForeignKey("dbo.Leads", "property_type_id", "dbo.PropertyTypes", "id");
            AddForeignKey("dbo.Leads", "requirement_id", "dbo.Requirements", "id");
            AddForeignKey("dbo.Leads", "timeline_id", "dbo.Timelines", "id");
            AddForeignKey("dbo.Leads", "type_of_visitor_id", "dbo.TypeOfVisitors", "id");
            DropColumn("dbo.Activities", "activity_type");
            DropColumn("dbo.Activities", "activity_date_time");
            DropColumn("dbo.Activities", "activity_duration");
            DropColumn("dbo.Activities", "note");
            DropColumn("dbo.Leads", "code");
            DropColumn("dbo.Leads", "user_name");
            DropColumn("dbo.Leads", "middle_name");
            DropColumn("dbo.Leads", "password");
            DropColumn("dbo.Leads", "address1");
            DropColumn("dbo.Leads", "address2");
            DropColumn("dbo.Leads", "gender");
            DropColumn("dbo.Leads", "nationality");
            DropColumn("dbo.Leads", "job");
            DropColumn("dbo.Leads", "company");
            DropColumn("dbo.Leads", "salary");
            DropColumn("dbo.Leads", "currency");
            DropColumn("dbo.Leads", "birthDate");
            DropColumn("dbo.Leads", "image");
            DropColumn("dbo.Leads", "follow_up");
            DropColumn("dbo.Leads", "notes");
            DropColumn("dbo.Leads", "lead_owner");
            DropColumn("dbo.Leads", "assigned_to");
            DropColumn("dbo.Leads", "activity_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leads", "activity_id", c => c.Int());
            AddColumn("dbo.Leads", "assigned_to", c => c.Int());
            AddColumn("dbo.Leads", "lead_owner", c => c.Int());
            AddColumn("dbo.Leads", "notes", c => c.String());
            AddColumn("dbo.Leads", "follow_up", c => c.DateTime());
            AddColumn("dbo.Leads", "image", c => c.String());
            AddColumn("dbo.Leads", "birthDate", c => c.DateTime());
            AddColumn("dbo.Leads", "currency", c => c.String());
            AddColumn("dbo.Leads", "salary", c => c.Double());
            AddColumn("dbo.Leads", "company", c => c.String());
            AddColumn("dbo.Leads", "job", c => c.String());
            AddColumn("dbo.Leads", "nationality", c => c.String());
            AddColumn("dbo.Leads", "gender", c => c.Int());
            AddColumn("dbo.Leads", "address2", c => c.String());
            AddColumn("dbo.Leads", "address1", c => c.String());
            AddColumn("dbo.Leads", "password", c => c.String());
            AddColumn("dbo.Leads", "middle_name", c => c.String());
            AddColumn("dbo.Leads", "user_name", c => c.String());
            AddColumn("dbo.Leads", "code", c => c.String());
            AddColumn("dbo.Activities", "note", c => c.String());
            AddColumn("dbo.Activities", "activity_duration", c => c.Single());
            AddColumn("dbo.Activities", "activity_date_time", c => c.DateTime());
            AddColumn("dbo.Activities", "activity_type", c => c.Int());
            DropForeignKey("dbo.LeadSubTypes", "SubType_id", "dbo.SubTypes");
            DropForeignKey("dbo.LeadActivities", "lead_id", "dbo.Leads");
            DropForeignKey("dbo.Leads", "type_of_visitor_id", "dbo.TypeOfVisitors");
            DropForeignKey("dbo.Leads", "timeline_id", "dbo.Timelines");
            DropForeignKey("dbo.Leads", "requirement_id", "dbo.Requirements");
            DropForeignKey("dbo.Leads", "property_type_id", "dbo.PropertyTypes");
            DropForeignKey("dbo.LeadUnitTypes", "unit_type_id", "dbo.UnitTypes");
            DropForeignKey("dbo.LeadUnitTypes", "lead_id", "dbo.Leads");
            DropForeignKey("dbo.LeadSubTypes", "lead_id", "dbo.Leads");
            DropForeignKey("dbo.Leads", "lead_stage_id", "dbo.LeadStages");
            DropForeignKey("dbo.LeadProperties", "property_id", "dbo.Properties");
            DropForeignKey("dbo.LeadProperties", "lead_id", "dbo.Leads");
            DropForeignKey("dbo.LeadProjects", "project_id", "dbo.Projects");
            DropForeignKey("dbo.LeadProjects", "lead_id", "dbo.Leads");
            DropForeignKey("dbo.LeadDevelopers", "lead_id", "dbo.Leads");
            DropForeignKey("dbo.LeadDevelopers", "developer_id", "dbo.Developers");
            DropForeignKey("dbo.Properties", "project_id", "dbo.Projects");
            DropForeignKey("dbo.Leads", "Project_id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "developer_id", "dbo.Developers");
            DropForeignKey("dbo.Leads", "lead_category_id", "dbo.LeadCategories");
            DropForeignKey("dbo.Users", "company_id", "dbo.Companies");
            DropForeignKey("dbo.LeadActivities", "activity_id", "dbo.Activities");
            DropIndex("dbo.LeadUnitTypes", new[] { "unit_type_id" });
            DropIndex("dbo.LeadUnitTypes", new[] { "lead_id" });
            DropIndex("dbo.LeadSubTypes", new[] { "SubType_id" });
            DropIndex("dbo.LeadSubTypes", new[] { "lead_id" });
            DropIndex("dbo.LeadProperties", new[] { "lead_id" });
            DropIndex("dbo.LeadProperties", new[] { "property_id" });
            DropIndex("dbo.LeadProjects", new[] { "project_id" });
            DropIndex("dbo.LeadProjects", new[] { "lead_id" });
            DropIndex("dbo.Properties", new[] { "project_id" });
            DropIndex("dbo.Projects", new[] { "developer_id" });
            DropIndex("dbo.LeadDevelopers", new[] { "lead_id" });
            DropIndex("dbo.LeadDevelopers", new[] { "developer_id" });
            DropIndex("dbo.Users", new[] { "company_id" });
            DropIndex("dbo.Leads", new[] { "Project_id" });
            DropIndex("dbo.Leads", new[] { "timeline_id" });
            DropIndex("dbo.Leads", new[] { "requirement_id" });
            DropIndex("dbo.Leads", new[] { "property_type_id" });
            DropIndex("dbo.Leads", new[] { "lead_category_id" });
            DropIndex("dbo.Leads", new[] { "lead_stage_id" });
            DropIndex("dbo.Leads", new[] { "type_of_visitor_id" });
            DropIndex("dbo.LeadActivities", new[] { "lead_id" });
            DropIndex("dbo.LeadActivities", new[] { "activity_id" });
            DropColumn("dbo.Users", "company_id");
            DropColumn("dbo.Leads", "Project_id");
            DropColumn("dbo.Leads", "company_name");
            DropColumn("dbo.Leads", "designation");
            DropColumn("dbo.Leads", "income");
            DropColumn("dbo.Leads", "timeline_id");
            DropColumn("dbo.Leads", "longitude");
            DropColumn("dbo.Leads", "latitude");
            DropColumn("dbo.Leads", "location_country");
            DropColumn("dbo.Leads", "pincode");
            DropColumn("dbo.Leads", "state");
            DropColumn("dbo.Leads", "sub_location");
            DropColumn("dbo.Leads", "location");
            DropColumn("dbo.Leads", "street_address");
            DropColumn("dbo.Leads", "remark");
            DropColumn("dbo.Leads", "area_metric");
            DropColumn("dbo.Leads", "maximum_area");
            DropColumn("dbo.Leads", "minimum_area");
            DropColumn("dbo.Leads", "budget_max");
            DropColumn("dbo.Leads", "budget_min");
            DropColumn("dbo.Leads", "requirement_id");
            DropColumn("dbo.Leads", "property_type_id");
            DropColumn("dbo.Leads", "country");
            DropColumn("dbo.Leads", "address");
            DropColumn("dbo.Leads", "sales_agent");
            DropColumn("dbo.Leads", "date_of_anniversary");
            DropColumn("dbo.Leads", "date_of_birth");
            DropColumn("dbo.Leads", "lead_category_id");
            DropColumn("dbo.Leads", "lead_stage_id");
            DropColumn("dbo.Leads", "type_of_visitor_id");
            DropColumn("dbo.Leads", "alternative_numbers");
            DropColumn("dbo.Activities", "description");
            DropColumn("dbo.Activities", "name");
            DropTable("dbo.SubTypes");
            DropTable("dbo.TypeOfVisitors");
            DropTable("dbo.Timelines");
            DropTable("dbo.Requirements");
            DropTable("dbo.PropertyTypes");
            DropTable("dbo.UnitTypes");
            DropTable("dbo.LeadUnitTypes");
            DropTable("dbo.LeadSubTypes");
            DropTable("dbo.LeadStages");
            DropTable("dbo.LeadProperties");
            DropTable("dbo.LeadProjects");
            DropTable("dbo.Properties");
            DropTable("dbo.Projects");
            DropTable("dbo.Developers");
            DropTable("dbo.LeadDevelopers");
            DropTable("dbo.LeadCategories");
            DropTable("dbo.LeadActivities");
            RenameIndex(table: "dbo.Leads", name: "IX_company_id", newName: "IX_type_id");
            RenameIndex(table: "dbo.Leads", name: "IX_employment_type_id", newName: "IX_status_id");
            RenameColumn(table: "dbo.Leads", name: "employment_type_id", newName: "status_id");
            RenameColumn(table: "dbo.Leads", name: "company_id", newName: "type_id");
            CreateIndex("dbo.Leads", "activity_id");
            CreateIndex("dbo.Leads", "assigned_to");
            AddForeignKey("dbo.Leads", "assigned_to", "dbo.Users", "id");
            AddForeignKey("dbo.Leads", "activity_id", "dbo.Activities", "id");
            RenameTable(name: "dbo.EmploymentTypes", newName: "Status");
            RenameTable(name: "dbo.Companies", newName: "LeadTypes");
        }
    }
}
