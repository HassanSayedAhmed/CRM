namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relations_update : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LeadSubTypes", name: "SubType_id", newName: "sub_type_id");
            RenameIndex(table: "dbo.LeadSubTypes", name: "IX_SubType_id", newName: "IX_sub_type_id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.LeadSubTypes", name: "IX_sub_type_id", newName: "IX_SubType_id");
            RenameColumn(table: "dbo.LeadSubTypes", name: "sub_type_id", newName: "SubType_id");
        }
    }
}
