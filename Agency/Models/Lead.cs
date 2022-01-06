using CRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Lead
    {
        [Key]
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string alternative_numbers { get; set; }
        [ForeignKey("TypeOfVisitor")]
        public int? type_of_visitor_id { get; set; }
        public TypeOfVisitor TypeOfVisitor { get; set; }
        //[ForeignKey("LeadStage")]
        public int? lead_stage_id { get; set; }
        //public LeadStage LeadStage { get; set; }
        [ForeignKey("LeadCategory")]
        public int? lead_category_id { get; set; }
        public LeadCategory LeadCategory { get; set; }
        [ForeignKey("Source")]
        public int? source_id { get; set; }
        public Source Source { get; set; }
        public DateTime? date_of_birth { get; set; }
        public DateTime? date_of_anniversary { get; set; }
        public string sales_agent { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        [ForeignKey("PropertyType")]
        public int? property_type_id { get; set; }
        public PropertyType PropertyType { get; set; }
        [ForeignKey("Requirement")]
        public int? requirement_id { get; set; }
        public Requirement Requirement { get; set; }
        public string budget_min { get; set; }
        public string budget_max { get; set; }
        public string minimum_area { get; set; }
        public string maximum_area { get; set; }
        public string area_metric { get; set; }
        public string remark { get; set; }

        public string street_address { get; set; }
        public string location { get; set; }
        public string sub_location { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string location_country { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int? decision_id { get; set; }
        public int? deal_property_id { get; set; }
        public double? deal_property_price { get; set; }
        public int? deal_make_user_id { get; set; }

        [ForeignKey("Timeline")]
        public int? timeline_id { get; set; }
        public Timeline Timeline { get; set; }
        [ForeignKey("EmploymentType")]
        public int? employment_type_id { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public string income { get; set; }
        public string designation { get; set; }
        public string search_location { get; set; }
        public string company_name { get; set; }
        [ForeignKey("Company")]
        public int? company_id { get; set; }
        public Company Company { get; set; }
        [ForeignKey("User")]
        public int? assigned_user_id { get; set; }
        public User User { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public virtual ICollection<LeadActivity> leadActivities { get; set; }
        public virtual ICollection<LeadSubType> LeadSubTypes { get; set; }
        public virtual ICollection<LeadProject> LeadProjects { get; set; }
        public virtual ICollection<LeadUnitType> LeadUnitTypes { get; set; }
        public virtual ICollection<LeadProperty> LeadProperties { get; set; }
        public virtual ICollection<LeadDeveloper> LeadDevelopers { get; set; }


    }
}