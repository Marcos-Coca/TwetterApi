using System;
using System.Collections.Generic;

namespace TwetterApi.Domain.Entities
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Visibitity { get; set; }
        public int Type { get; set; }
        public int Media { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
    
    }
}
