using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models.Entities
{
    [Bind(Include = "Id,Name,Description,CreatorId,CreationDate,SectionId,CreatorUsername")]
    public class TopicViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Topic must have name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Topic must have description")]
        [UIHint("MultilineText")]
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationDate { get; set; }
        public byte SectionId { get; set; }
        public string CreatorUsername { get; set; }
    }
}