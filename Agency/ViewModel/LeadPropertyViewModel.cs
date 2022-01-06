using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.ViewModel
{
    public class LeadPropertyViewModel
    {
        public int id { get; set; }
        public int? lead_id { get; set; }
        public int? property_id { get; set; }
        public string note { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public string property_name { get; set; }
        public double? property_price { get; set; }
        public int? property_baths { get; set; }
        public int? property_rooms { get; set; }
        public int? property_beds { get; set; }
        public string property_area { get; set; }
        public string property_type { get; set; }
        public string property_status { get; set; }
        public string property_image { get; set; }
        public int? property_project_id { get; set; }
    }
}