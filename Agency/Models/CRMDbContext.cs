using CRM.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace CRM.Models
{
    public class CRMDbContext : DbContext
    {
        // Your context has been configured to use a 'DbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CRM.Models.DbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DbContext' 
        // connection string in the application configuration file.
        public CRMDbContext()
            : base("name=DbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Chat>  Chats { get; set; }
        public virtual DbSet<Company> Companies  { get; set; }
        public virtual DbSet<Developer> Developers  { get; set; }
        public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<LeadActivity> LeadActivities { get; set; }
        public virtual DbSet<LeadCategory> LeadCategories { get; set; }
        public virtual DbSet<LeadProject> LeadProjects { get; set; }
        public virtual DbSet<LeadStage> LeadStages { get; set; }
        public virtual DbSet<LeadSubType> LeadSubTypes { get; set; }
        public virtual DbSet<LeadUnitType> LeadUnitTypes { get; set; }
        public virtual DbSet<Project>  Projects { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<Requirement> Requirements { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<SubType> SubTypes { get; set; }
        public virtual DbSet<Timeline> Timelines { get; set; }
        public virtual DbSet<TypeOfVisitor> TypeOfVisitors { get; set; }
        public virtual DbSet<UnitType> UnitTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }


    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}