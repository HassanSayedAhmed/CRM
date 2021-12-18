namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "company");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "company", c => c.String());
        }
    }
}
