using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.ViewModel
{
    public class UserViewModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
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
        public string company_name { get; set; }
        public double? salary { get; set; }
        public string currency { get; set; }
        public DateTime? birthDate { get; set; }
        public HttpPostedFileBase image { get; set; }
        public int? type { get; set; }
        public int? status { get; set; }
        public int? lead_owner { get; set; }
        public int? active { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public int? company_id { get; set; }
        public string imagePath { get; set; }
        public List<ChatViewModel> chats { get; set; }

    }
}