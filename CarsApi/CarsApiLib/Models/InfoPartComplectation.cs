using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApiLib.Models
{
    public class InfoPartComplectation
    {
        public string TreeCode { get; set; }
        public string Tree { get; set; }
        public List<PartComplectation> PartsComplectation { get; set; }
        public ComplectationGroup ComplectationGroup { get; set; }
    }
}
