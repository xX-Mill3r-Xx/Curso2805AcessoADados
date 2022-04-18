using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula005.Models
{
    public class CareerItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public Courses Courses { get; set; }
    }
}
