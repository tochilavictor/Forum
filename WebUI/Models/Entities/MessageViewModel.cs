using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models.Entities
{
    public class MessageViewModel
    {
        public long Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public string CreatorUsername { get; set; }
        public long? ParentMessageId { get; set; }
        [Required]
        public string Value { get; set; }
        public DateTime Creation_date { get; set; }
        public DateTime? Last_update { get; set; }
        public bool? Contains_pictures { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public IEnumerable<FullFileName> Filenames { get; set; }
    }
    public struct FullFileName
    {
        private string name;
        private string extension;
        public FullFileName(string name,string extension)
        {
            this.name = name;
            this.extension = extension;
        }
        public string Name => name;
        public string Extension => extension;
    }
}