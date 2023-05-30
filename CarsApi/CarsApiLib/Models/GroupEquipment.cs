using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApiLib.Models
{
    public class GroupEquipment
    {
        public int GroupId { get; set; }
        public string Name { get; set; }

        public EquipmentModelCar EquipmentModel { get; set; }
    }
}
