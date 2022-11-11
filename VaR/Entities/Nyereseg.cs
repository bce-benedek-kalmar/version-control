using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaR.Entities
{
    public class Nyereseg
    {
        public int Sorszam { get; set; }
        public DateTime KezdoNap { get; set; }
        public decimal NyeresegErtek { get; set; }
    }
}
