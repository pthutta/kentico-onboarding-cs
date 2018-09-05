using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoApp.Api.Models
{
    public class Item
    {
        [Key]
        public string Id { get; set; }

        [MinLength(1, ErrorMessage = "Text length must be greater than 1.")]
        public string Text { get; set; }
    }
}