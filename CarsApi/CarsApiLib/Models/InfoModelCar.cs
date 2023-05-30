using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApiLib.Models
{
    public class InfoModelCar
    {
        public string Name { get; set; }
        public List<ModelCar> ModelCars { get; set; }
    }
}
