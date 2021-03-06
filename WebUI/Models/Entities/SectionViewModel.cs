﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models.Entities
{
    [Bind(Include = "Id,Name,Description")]
    public class SectionViewModel
    {
        public byte Id { get; set; }

        [Required(ErrorMessage = "Section must have name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Section must have description")]
        [UIHint("MultilineText")]
        [StringLength(250)]
        public string Description { get; set; }
        
    }
}