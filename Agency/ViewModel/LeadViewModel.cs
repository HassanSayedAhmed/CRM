using CRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.ViewModel
{
    public class LeadViewModel
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string alternative_numbers { get; set; }
        public int? type_of_visitor_id { get; set; }
        public int? lead_stage_id { get; set; }
        public LeadStage LeadStage { get; set; }
        public int? lead_category_id { get; set; }
        public LeadCategory LeadCategory { get; set; }
        public int? source_id { get; set; }
        public Source Source { get; set; }
        public DateTime? date_of_birth { get; set; }
        public DateTime? date_of_anniversary { get; set; }
        public string sales_agent { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public int? property_type_id { get; set; }
        public int? requirement_id { get; set; }
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

        public int? timeline_id { get; set; }
        public int? employment_type_id { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public string income { get; set; }
        public string designation { get; set; }
        public int? company_id { get; set; }
        public Company Company { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }


    }
}