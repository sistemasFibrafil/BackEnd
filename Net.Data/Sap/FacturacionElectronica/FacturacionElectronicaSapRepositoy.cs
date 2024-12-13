using System;
using System.Data;
using Net.Connection;
using Newtonsoft.Json;
using Net.CrossCotting;
using Net.Business.Entities;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
namespace Net.Data.Sap
{
    public class FacturacionElectronicaSapRepositoy : RepositoryBase<FacturacionElectronicaSapEntity>, IFacturacionElectronicaSapRepositoy
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST_GUIA_ELECTRONICA_BY_FILTRO = DB_ESQUEMA + "VEN_GetListGuiaElectronicaByFiltro";
        const string SP_GET_GUIA_ELECTRONICA_BY_ID = DB_ESQUEMA + "VEN_GetGuiaElectronicaById";
        const string SP_GET_LIST_GUIA_DETALLE_ELECTRONICA_BY_ID = DB_ESQUEMA + "VEN_GetListGuiaDetalleElectronicaById";
        const string SP_SET_UPDATE_GUIA = DB_ESQUEMA + "VEN_SetUpdateGuiaElectronica";
        const string SP_SET_UPDATE_GUIA_ERROR = DB_ESQUEMA + "VEN_SetUpdateGuiaElectronicaError";

        const string SP_GET_LIST_GUIA_INTERNA_ELECTRONICA_BY_FILTRO = DB_ESQUEMA + "INV_GuiaInternaElectronicaByFiltro";
        const string SP_GET_GUIA_INTERNA_ELECTRONICA_BY_ID = DB_ESQUEMA + "INV_GetGuiaInternaElectronicaById";
        const string SP_GET_LIST_GUIA_DETALLE_INTERNA_ELECTRONICA_BY_ID = DB_ESQUEMA + "INV_GetListGuiaDetalleInternaElectronicaById";


        public FacturacionElectronicaSapRepositoy(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _cnxSap = Utilidades.GetCon(configuration, "EntornoConnectionSap:Entorno");
        }


        public async Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> GetListGuiaElectronicaByFiltro(FilterRequestEntity value)
        {
            var response = new List<FacturacionElectronicaSapEntity>();
            var resultTransaccion = new ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_ELECTRONICA_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FI", value.Dat1));
                        cmd.Parameters.Add(new SqlParameter("@FF", value.Dat2));
                        cmd.Parameters.Add(new SqlParameter("@ObjType", value.Cod1));
                        cmd.Parameters.Add(new SqlParameter("@CodStatus", value.Cod2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro1", value.Text1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro2", value.Text2));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<FacturacionElectronicaSapEntity>)context.ConvertTo<FacturacionElectronicaSapEntity>(reader);
                        }

                        resultTransaccion.IdRegistro = 0;
                        resultTransaccion.ResultadoCodigo = 0;
                        resultTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        resultTransaccion.dataList = response;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                resultTransaccion.IdRegistro = -1;
                resultTransaccion.ResultadoCodigo = -1;
                resultTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultTransaccion;
        }
        public async Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> SetEnviar(FilterRequestEntity value)
        {
            var guia = new Invoice();
            var resultTransaccion = new ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_GUIA_ELECTRONICA_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@ObjType", value.Cod1));
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia = ((List<Invoice>)context.ConvertTo<Invoice>(reader))[0];
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_DETALLE_ELECTRONICA_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@ObjType", value.Cod1));
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia.items = (List<Items>)context.ConvertTo<Items>(reader);
                        }
                    }

                    string tsq = JsonConvert.SerializeObject(guia, Formatting.Indented);
                    var responseSend = FacturacionElectronica.GetResponse(tsq);

                    if (responseSend.errors == null)
                    {
                        using (SqlCommand cmd = new SqlCommand(SP_SET_UPDATE_GUIA, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@ObjType", value.Cod1));
                            cmd.Parameters.Add(new SqlParameter("@DocEntry", value.Id1));
                            cmd.Parameters.Add(new SqlParameter("@AceptadaPorSunat", responseSend.aceptada_por_sunat));
                            cmd.Parameters.Add(new SqlParameter("@SunatDescription", responseSend.sunat_description));
                            cmd.Parameters.Add(new SqlParameter("@SunatNote", responseSend.sunat_note));
                            cmd.Parameters.Add(new SqlParameter("@SunatResponsecode", responseSend.sunat_responsecode));
                            cmd.Parameters.Add(new SqlParameter("@SunatSoapError", responseSend.sunat_soap_error));
                            cmd.Parameters.Add(new SqlParameter("@CadenaParaCodigoQr", responseSend.cadena_para_codigo_qr));
                            cmd.Parameters.Add(new SqlParameter("@CodigoHash", responseSend.codigo_hash));

                            await cmd.ExecuteNonQueryAsync();
                        }

                        resultTransaccion.IdRegistro = 0;
                        resultTransaccion.ResultadoCodigo = 0;
                        resultTransaccion.ResultadoDescripcion = "El comprobante se envío con éxito...!!!";
                    }
                    else
                    {
                        using (SqlCommand cmd = new SqlCommand(SP_SET_UPDATE_GUIA_ERROR, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@ObjType", value.Cod1));
                            cmd.Parameters.Add(new SqlParameter("@DocEntry", value.Id1));
                            cmd.Parameters.Add(new SqlParameter("@Error", responseSend.errors));

                            await cmd.ExecuteNonQueryAsync();
                        }

                        resultTransaccion.IdRegistro = -1;
                        resultTransaccion.ResultadoCodigo = -1;
                        resultTransaccion.ResultadoDescripcion = responseSend.errors;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                resultTransaccion.IdRegistro = -1;
                resultTransaccion.ResultadoCodigo = -1;
                resultTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }
            return resultTransaccion;
        }

        public async Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> GetListGuiaInternaElectronicaByFiltro(FilterRequestEntity value)
        {
            var response = new List<FacturacionElectronicaSapEntity>();
            var resultTransaccion = new ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_INTERNA_ELECTRONICA_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FI", value.Dat1));
                        cmd.Parameters.Add(new SqlParameter("@FF", value.Dat2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro1", value.Text1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro2", value.Text2));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<FacturacionElectronicaSapEntity>)context.ConvertTo<FacturacionElectronicaSapEntity>(reader);
                        }

                        resultTransaccion.IdRegistro = 0;
                        resultTransaccion.ResultadoCodigo = 0;
                        resultTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        resultTransaccion.dataList = response;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                resultTransaccion.IdRegistro = -1;
                resultTransaccion.ResultadoCodigo = -1;
                resultTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultTransaccion;
        }
        public async Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> SendGuiaInternaElectronica(FilterRequestEntity value)
        {
            var guia = new Invoice();
            var resultTransaccion = new ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_GUIA_INTERNA_ELECTRONICA_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia = ((List<Invoice>)context.ConvertTo<Invoice>(reader))[0];
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_DETALLE_INTERNA_ELECTRONICA_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia.items = (List<Items>)context.ConvertTo<Items>(reader);
                        }
                    }

                    string tsq = JsonConvert.SerializeObject(guia, Formatting.Indented);
                    var responseSend = FacturacionElectronica.GetResponse(tsq);

                    if (responseSend.errors == null)
                    {
                        //using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_UPDATE, conn))
                        //{
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Clear();
                        //    cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Description", responseSend.sunat_description));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Note", responseSend.sunat_note));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Responsecode", responseSend.sunat_responsecode));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Soap_Error", responseSend.sunat_soap_error));
                        //    cmd.Parameters.Add(new SqlParameter("@Cadena_Para_Codigo_Qr", responseSend.cadena_para_codigo_qr));
                        //    cmd.Parameters.Add(new SqlParameter("@Codigo_Hash", responseSend.codigo_hash));

                        //    await cmd.ExecuteNonQueryAsync();
                        //}

                        resultTransaccion.IdRegistro = 0;
                        resultTransaccion.ResultadoCodigo = 0;
                        resultTransaccion.ResultadoDescripcion = "El comprobante se envío con éxito...!!!";
                    }
                    else
                    {
                        //using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_ERROR_UPDATE, conn))
                        //{
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Clear();
                        //    cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));
                        //    cmd.Parameters.Add(new SqlParameter("@Error", leer_respuesta.errors));

                        //    await cmd.ExecuteNonQueryAsync();
                        //}

                        resultTransaccion.IdRegistro = -1;
                        resultTransaccion.ResultadoCodigo = -1;
                        resultTransaccion.ResultadoDescripcion = responseSend.errors;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                resultTransaccion.IdRegistro = -1;
                resultTransaccion.ResultadoCodigo = -1;
                resultTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultTransaccion;
        }
    }
}
