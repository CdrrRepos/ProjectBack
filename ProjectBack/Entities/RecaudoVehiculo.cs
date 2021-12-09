using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBack.Entities
{
    [Table("RecaudoVehiculo", Schema = "Cristian")]
    public class RecaudoVehiculo : VehiculoModel
    {
        public int valorTabulado { get; set; }
    }
}
