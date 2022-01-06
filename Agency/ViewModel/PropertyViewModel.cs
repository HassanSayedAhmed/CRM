using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.ViewModel
{
    public class PropertyViewModel
    {
        [Key]
        public int id { get; set; }
        public int? company_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double? price { get; set; }
        public int? baths { get; set; }
        public int? rooms { get; set; }
        public int? beds { get; set; }
        public string area { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string image { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int? project_id { get; set; }
        public string created_at_string { get; set; }
        public string project_name { get; set; }
    }
}