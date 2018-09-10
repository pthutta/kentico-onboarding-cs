using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.DAL.Entities
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }

        [MinLength(1, ErrorMessage = "Text length must be greater than 1.")]
        public string Text { get; set; }
    }
}