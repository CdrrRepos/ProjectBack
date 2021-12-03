using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProjectBack.Entities
{
    public class ModelResponse<T>
    {
        /// <summary>
        /// Código de respuesta
        /// </summary>
        public HttpStatusCode CodigoRespuesta { get; set; }

        /// <summary>
        /// Mensaje.
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Data.
        /// </summary>
        public Collection<T> Data { get; set; } = new Collection<T>();
    }
}
