using Microsoft.AspNetCore.Mvc;
using ProjectBack.Context;
using ProjectBack.Entities;
using ProjectBack.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : Controller
    {
        public AppDbContext Context { get; }

        public VehiculoController(AppDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// GET api/GetConteoVehiculos
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetConteo")]
        public IActionResult GetConteoVehiculos()
        {
            ModelResponse<ConteoVehiculo> Response = new ModelResponse<ConteoVehiculo>();
            Response.CodigoRespuesta = HttpStatusCode.OK;
            Response.Mensaje = "Operacion exitosa.";
            try
            {
                var vehiculo = Context.ConteoVehiculo.ToList();
                //return vehiculo;
                foreach (var item in vehiculo)
                {
                    ConteoVehiculo vehiculoResponse = new ConteoVehiculo();

                    vehiculoResponse.estacion = item.estacion;
                    vehiculoResponse.sentido = item.sentido;
                    vehiculoResponse.hora = item.hora;
                    vehiculoResponse.categoria = item.categoria;
                    vehiculoResponse.cantidad = item.cantidad;
                    vehiculoResponse.Fecha = item.Fecha;

                    Response.Data.Add(vehiculoResponse);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.CodigoRespuesta = HttpStatusCode.BadRequest;
                Response.Mensaje = ex.Message;
                return BadRequest(Response);

            }
        }

        /// <summary>
        /// GET api/GetConteoVehiculos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpGet("GetConteoFecha")]
        public IActionResult GetConteoFecha(string fecha)
        {
            ModelResponse<ConteoVehiculo> Response = new ModelResponse<ConteoVehiculo>();
            Response.CodigoRespuesta = HttpStatusCode.OK;
            Response.Mensaje = "Operacion exitosa.";
            try
            {
                var vehiculo = Context.ConteoVehiculo.ToList().Where(p => p.Fecha == fecha);
                //return vehiculo;
                foreach (var item in vehiculo)
                {
                    ConteoVehiculo vehiculoResponse = new ConteoVehiculo();

                    vehiculoResponse.estacion = item.estacion;
                    vehiculoResponse.sentido = item.sentido;
                    vehiculoResponse.hora = item.hora;
                    vehiculoResponse.categoria = item.categoria;
                    vehiculoResponse.cantidad = item.cantidad;
                    vehiculoResponse.Fecha = fecha;

                    Response.Data.Add(vehiculoResponse);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.CodigoRespuesta = HttpStatusCode.BadRequest;
                Response.Mensaje = ex.Message;
                return BadRequest(Response);

            }
        }

        /// <summary>
        /// POST api/PostConteoVehiculos
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost("PostConteoVehiculos")]
        public IActionResult PostConteoVehiculos(string fecha)
        {
            ModelResponse<ConteoVehiculo> Response = new ModelResponse<ConteoVehiculo>();
            Response.CodigoRespuesta = HttpStatusCode.OK;
            try
            {

                List<VehiculoResponse> data = new List<VehiculoResponse>();

                if (fecha == "000")
                {
                    int conteo=0;
                    int suma = 0;
                    string dia = "";
                    int mes = 7;
                    fecha = "2021-07-01";

                    while (fecha != "2021-10-10")
                    {

                        //Validamos si ya existen registros con esta fecha
                        var regVehiculoExiste = Context.ConteoVehiculo.ToList().Where(p => p.Fecha == fecha);
                        if (regVehiculoExiste.Count() < 1)
                        {
                            //Consultamos api externa para obtener datos
                            AppProxy proxyContext = new AppProxy();
                            data = proxyContext.ConsultaVehiculos("ConteoVehiculos/", fecha);

                            //Si existen datos con la fecha indicada se proceden a insertar
                            if (data != null)
                            {
                                if (data.Count > 0)
                                {
                                    foreach (var item in data)
                                    {
                                        ConteoVehiculo vehiculo = new ConteoVehiculo();

                                        vehiculo.estacion = item.estacion;
                                        vehiculo.sentido = item.sentido;
                                        vehiculo.hora = item.hora;
                                        vehiculo.categoria = item.categoria;
                                        vehiculo.cantidad = item.cantidad;
                                        vehiculo.Fecha = fecha;

                                        Context.ConteoVehiculo.Add(vehiculo);

                                    }
                                    Context.SaveChanges();
                                    conteo += data.Count();
                                    Response.Mensaje = "Operacion exitosa se insertaron los registros, cantidad insertada: " + conteo.ToString();
                                }
                            }
                            if (conteo == 0)
                            {
                                Response.Mensaje = "No hay registros nuevos para insertar.";
                            }
                        }

                        suma += 1;
                        if (suma > 31)
                        {
                            suma = 1;
                            mes += 1;
                        }
                        dia = suma.ToString();
                        if (suma.ToString().Length < 2)
                        {
                            dia = "0" + suma.ToString();
                        }
                        fecha = "2021-"+mes.ToString() + "-" + dia;
                        if (mes.ToString().Length < 2)
                        {
                            fecha = "2021-0" + mes.ToString() + "-" + dia;
                        }

                    }
                }
                else
                {
                    //Validamos si ya existen registros con esta fecha
                    var regVehiculoExiste = Context.ConteoVehiculo.ToList().Where(p => p.Fecha == fecha);
                    if (regVehiculoExiste.Count() > 0)
                    {
                        Response.Mensaje = "Ya existen registros con este filtro de fecha: " + fecha;
                    }
                    else
                    {
                        //Consultamos api externa para obtener datos
                        AppProxy proxyContext = new AppProxy();
                        data = proxyContext.ConsultaVehiculos("ConteoVehiculos/", fecha);

                        //Si existen datos con la fecha indicada se proceden a insertar
                        if (data != null)
                        {
                            if (data.Count > 0)
                            {
                                foreach (var item in data)
                                {
                                    ConteoVehiculo vehiculo = new ConteoVehiculo();

                                    vehiculo.estacion = item.estacion;
                                    vehiculo.sentido = item.sentido;
                                    vehiculo.hora = item.hora;
                                    vehiculo.categoria = item.categoria;
                                    vehiculo.cantidad = item.cantidad;
                                    vehiculo.Fecha = fecha;

                                    Context.ConteoVehiculo.Add(vehiculo);

                                }
                                Context.SaveChanges();
                                Response.Mensaje = "Operacion exitosa se insertaron los registros, cantidad insertada: " + data.Count();
                            }
                            else
                            {
                                Response.Mensaje = "No se encontraron registros con el filtro de fecha: " + fecha;
                            }
                        }
                        else
                        {
                            Response.Mensaje = "No se encontraron registros con el filtro de fecha: " + fecha;
                        }
                    }
                }


                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.CodigoRespuesta = HttpStatusCode.BadRequest;
                Response.Mensaje = ex.Message;
                return BadRequest(Response);
            }
        }

        /// <summary>
        /// GET api/GetRecaudoVehiculo
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRecaudo")]
        public IActionResult GetRecaudo()
        {
            ModelResponse<RecaudoVehiculo> Response = new ModelResponse<RecaudoVehiculo>();
            Response.CodigoRespuesta = HttpStatusCode.OK;
            Response.Mensaje = "Operacion exitosa.";
            try
            {
                var vehiculo = Context.RecaudoVehiculo.ToList();
                //return vehiculo;
                foreach (var item in vehiculo)
                {
                    RecaudoVehiculo vehiculoResponse = new RecaudoVehiculo();

                    vehiculoResponse.estacion = item.estacion;
                    vehiculoResponse.sentido = item.sentido;
                    vehiculoResponse.hora = item.hora;
                    vehiculoResponse.categoria = item.categoria;
                    vehiculoResponse.valorTabulado = item.valorTabulado;
                    vehiculoResponse.Fecha = item.Fecha;

                    Response.Data.Add(vehiculoResponse);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.CodigoRespuesta = HttpStatusCode.BadRequest;
                Response.Mensaje = ex.Message;
                return BadRequest(Response);

            }
        }

        /// <summary>
        /// GET api/GetRecaudoFecha
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpGet("GetRecaudoFecha")]
        public IActionResult GetRecaudoFecha(string fecha)
        {
            ModelResponse<RecaudoVehiculo> Response = new ModelResponse<RecaudoVehiculo>();
            Response.CodigoRespuesta = HttpStatusCode.OK;
            Response.Mensaje = "Operacion exitosa.";
            try
            {
                var vehiculo = Context.RecaudoVehiculo.ToList().Where(p => p.Fecha == fecha);
                //return vehiculo;
                foreach (var item in vehiculo)
                {
                    RecaudoVehiculo vehiculoResponse = new RecaudoVehiculo();

                    vehiculoResponse.estacion = item.estacion;
                    vehiculoResponse.sentido = item.sentido;
                    vehiculoResponse.hora = item.hora;
                    vehiculoResponse.categoria = item.categoria;
                    vehiculoResponse.valorTabulado = item.valorTabulado;
                    vehiculoResponse.Fecha = fecha;

                    Response.Data.Add(vehiculoResponse);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.CodigoRespuesta = HttpStatusCode.BadRequest;
                Response.Mensaje = ex.Message;
                return BadRequest(Response);

            }
        }

        [HttpPost("PostRecaudoVehiculo")]
        public IActionResult PostRecaudoVehiculo(string fecha)
        {
            ModelResponse<RecaudoVehiculo> Response = new ModelResponse<RecaudoVehiculo>();
            Response.CodigoRespuesta = HttpStatusCode.OK;
            try
            {

                List<VehiculoResponse> data = new List<VehiculoResponse>();


                if (fecha == "000")
                {
                    int conteo = 0;
                    int suma = 0;
                    string dia = "";
                    int mes = 7;
                    fecha = "2021-07-01";

                    while (fecha != "2021-10-10")
                    {

                        //Validamos si ya existen registros con esta fecha
                        var regVehiculoExiste = Context.RecaudoVehiculo.ToList().Where(p => p.Fecha == fecha);
                        if (regVehiculoExiste.Count() < 1)
                        {
                            //Consultamos api externa para obtener datos
                            AppProxy proxyContext = new AppProxy();
                            data = proxyContext.ConsultaVehiculos("RecaudoVehiculos/", fecha);

                            //Si existen datos con la fecha indicada se proceden a insertar
                            if (data != null)
                            {
                                if (data.Count > 0)
                                {
                                    foreach (var item in data)
                                    {
                                        RecaudoVehiculo vehiculo = new RecaudoVehiculo();

                                        vehiculo.estacion = item.estacion;
                                        vehiculo.sentido = item.sentido;
                                        vehiculo.hora = item.hora;
                                        vehiculo.categoria = item.categoria;
                                        vehiculo.valorTabulado = item.valorTabulado;
                                        vehiculo.Fecha = fecha;

                                        Context.RecaudoVehiculo.Add(vehiculo);

                                    }
                                    Context.SaveChanges();
                                    conteo += data.Count();
                                    Response.Mensaje = "Operacion exitosa se insertaron los registros, cantidad insertada: " + conteo.ToString();
                                }
                            }
                            if (conteo == 0)
                            {
                                Response.Mensaje = "No hay registros nuevos para insertar.";
                            }
                        }

                        suma += 1;
                        if (suma > 31)
                        {
                            suma = 1;
                            mes += 1;
                        }
                        dia = suma.ToString();
                        if (suma.ToString().Length < 2)
                        {
                            dia = "0" + suma.ToString();
                        }
                        fecha = "2021-" + mes.ToString() + "-" + dia;
                        if (mes.ToString().Length < 2)
                        {
                            fecha = "2021-0" + mes.ToString() + "-" + dia;
                        }

                    }
                }
                else
                {

                    //Validamos si ya existen registros con esta fecha
                    var regVehiculoExiste = Context.RecaudoVehiculo.ToList().Where(p => p.Fecha == fecha);
                    if (regVehiculoExiste.Count() > 0)
                    {
                        Response.Mensaje = "Ya existen registros con este filtro de fecha: " + fecha;
                    }
                    else
                    {
                        //Consultamos api externa para obtener datos
                        AppProxy proxyContext = new AppProxy();
                        data = proxyContext.ConsultaVehiculos("RecaudoVehiculos/", fecha);

                        //Si existen datos con la fecha indicada se proceden a insertar
                        if (data != null)
                        {
                            if (data.Count > 0)
                            {
                                foreach (var item in data)
                                {
                                    RecaudoVehiculo vehiculo = new RecaudoVehiculo();

                                    vehiculo.estacion = item.estacion;
                                    vehiculo.sentido = item.sentido;
                                    vehiculo.hora = item.hora;
                                    vehiculo.categoria = item.categoria;
                                    vehiculo.valorTabulado = item.valorTabulado;
                                    vehiculo.Fecha = fecha;

                                    Context.RecaudoVehiculo.Add(vehiculo);

                                }
                                Context.SaveChanges();
                                Response.Mensaje = "Operacion exitosa se insertaron los registros, cantidad insertada: " + data.Count();
                            }
                            else
                            {
                                Response.Mensaje = "No se encontraron registros con el filtro de fecha: " + fecha;
                            }
                        }
                        else
                        {
                            Response.Mensaje = "No se encontraron registros con el filtro de fecha: " + fecha;
                        }

                    }

                }

                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.CodigoRespuesta = HttpStatusCode.BadRequest;
                Response.Mensaje = ex.Message;
                return BadRequest(Response);
            }
        }
    }
}
