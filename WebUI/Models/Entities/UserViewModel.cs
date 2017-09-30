using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Entities
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public short? Reputation { get; set; }
        public byte[] Image { get; set; }
        public string RoleName { get; set; }
        public string[] SectionModerating { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Describing { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}