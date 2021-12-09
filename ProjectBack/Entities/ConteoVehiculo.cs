using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBack.Entities
{
    [Table("ConteoVehiculo", Schema = "Cristian")]
    public class ConteoVehiculo : VehiculoModel
    {
        public int cantidad { get; set; }
    }
}
