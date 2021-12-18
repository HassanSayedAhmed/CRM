namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "note");
        }
    }
}
