using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySim.Entities
{
    public class Person
    {
        public int BirthYear { get; set; }
        public Gender Gender { get; set; }
        public int NumChildren { get; set; }
        public bool Living { get; set; }

        public Person()
        {
            Living = true;
        }
    }
}
