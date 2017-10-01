using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class UserEditByModerator
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<SectionModerator> SectionModerating { get; set; }
        public short Reputation { get; set; }
    }
}