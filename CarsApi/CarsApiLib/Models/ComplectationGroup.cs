using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApiLib.Models
{
    public class ComplectationGroup
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string SubGroup { get; set; }

        public GroupEquipment GroupEquipment { get; set; }
    }
}
