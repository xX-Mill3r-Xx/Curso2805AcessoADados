using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace Blog.Models
{
    [Table("[Category]")]
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
