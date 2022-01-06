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
        public int? source_id { get; set; }
        public DateTime? date_of_birth { get; set; }
        public DateTime? date_of_anniversary { get; set; }
        public string sales_agent { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public int? property_type_id { get; set; }
        public string property_type_name { get; set; }
        public int? requirement_id { get; set; }
        public string requirement_name { get; set; }
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
        public int? timeline_id { get; set; }
        public int? employment_type_id { get; set; }
        public string income { get; set; }
        public string lead_ids { get; set; }
        public string designation { get; set; }
        public string search_location { get; set; }
        public int? assigned_user_id { get; set; }
        public string assigned_user_name { get; set; }
        public int? company_id { get; set; }
        public Company Company { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public string created_at_string { get; set; }
        public string lead_stage_name { get; set; }
        public string lead_category_name { get; set; }
        public string source_name { get; set; }
        public string time_line_name     { get; set; }
        public string employment_type_name { get; set; }
        public string company_name { get; set; }
        public string created_by_name { get; set; }
        public string updated_by_name { get; set; }
        public string deleted_by_name { get; set; }
        public List<int> lead_sub_types { get; set; }
        public List<int> lead_unit_types { get; set; }
        public List<int> project_ids { get; set; }
        public List<int> property_ids { get; set; }
        public List<int> developer_ids { get; set; }
        public List<LeadActivityViewModel> leadActivities { get; set; }
        public List<LeadDeveloperViewModel> leadDevelopers { get; set; }
        public List<LeadProjectViewModel> leadProjects { get; set; }
        public List<LeadPropertyViewModel> leadProperties { get; set; }
        public List<LeadSubTypeViewModel> leadSubTypes { get; set; }
        public List<LeadUnitTypeViewModel> leadUnitTypes { get; set; }
        public string leadSubTypesJson { get; set; }
        public string leadActivitiesJson { get; set; }
        public string leadDevelopersJson { get; set; }
        public string leadProjectsJson { get; set; }
        public string leadPropertiesJson { get; set; }
        public string leadUnitTypesJson { get; set; }
        public int created_by_company_id { get; set; }

    }
}