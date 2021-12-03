using System.ComponentModel.DataAnnotations;

namespace ProjectBack.Entities
{
    public class VehiculoModel
    {
        [Key]
        public int id { get; set; }
        public string estacion { get; set; }
        public string sentido { get; set; }
        public int hora { get; set; }
        public string categoria { get; set; }
        public string Fecha { get; set; }
    }
}
