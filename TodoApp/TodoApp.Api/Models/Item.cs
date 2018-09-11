using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoApp.Api.Models
{
    public class Item
    {
        public string Id { get; set; }

        public string Text { get; set; }
    }
}