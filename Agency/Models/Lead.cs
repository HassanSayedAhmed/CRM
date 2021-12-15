using CRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Agency.Models
{
    public class Lead
    {
        [Key]
        public int id { get; set; }
        public string code { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public int? gender { get; set; }
        public string nationality { get; set; }
        public string job { get; set; }
        public string company { get; set; }
        public double? salary { get; set; }
        public string currency { get; set; }
        public DateTime? birthDate { get; set; }
        public string image { get; set; }
        public DateTime? follow_up { get; set; }
        public string notes { get; set; }
        public int? lead_owner { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        [ForeignKey("User")]
        public int? assigned_to { get; set; }
        public User User { get; set; }
        [ForeignKey("Activity")]
        public int? activity_id { get; set; }
        public Activity Activity { get; set; }
        [ForeignKey("Source")]
        public int? source_id { get; set; }
        public Source Source { get; set; }
        [ForeignKey("Status")]
        public int? status_id { get; set; }
        public Status Status { get; set; }
        [ForeignKey("LeadType")]
        public int? type_id { get; set; }
        public LeadType LeadType { get; set; }
    }
}