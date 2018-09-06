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

        public override bool Equals(object obj)
        {
            if (!(obj is Item other))
            {
                return false;
            }

            return Id == other.Id && Text == other.Text;
        }

        public override int GetHashCode()
        {
            var hashCode = -1144598946;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }
    }
}