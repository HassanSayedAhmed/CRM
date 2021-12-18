using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class LeadProperty
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Property")]
        public int? property_id { get; set; }
        public Property Property { get; set; }
        public string note { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        [ForeignKey("Lead")]
        public int? lead_id { get; set; }
        public Lead Lead { get; set; }
    }
}