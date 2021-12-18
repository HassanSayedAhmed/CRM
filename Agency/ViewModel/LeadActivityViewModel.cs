using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.ViewModel
{
    public class LeadActivityViewModel
    {
        public int id { get; set; }
        public int? activity_id { get; set; }
        public DateTime? activity_date_time { get; set; }
        public float? activity_duration { get; set; }
        public string note { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int? lead_id { get; set; }
    }
}