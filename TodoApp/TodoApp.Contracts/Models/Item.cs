using System;

namespace TodoApp.Contracts.Models
{
    public class Item
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}