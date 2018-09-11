using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Api.Models;

namespace TodoApp.Api.Tests.Utils
{
    public static class ItemExtensions
    {
        public static bool IsEqual(this Item item1, Item item2)
        {
            return item1.Id == item2.Id && item1.Text == item2.Text;
        }
    }
}
