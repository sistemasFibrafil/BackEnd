﻿using System;
using System.Data;
using Net.Connection;
using System.Transactions;
using Net.Business.Entities;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Office2010.Excel;
namespace Net.Data.Web
{
    public class LecturaRepository : RepositoryBase<LecturaEntity>, ILecturaRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST_BY_FILTRO = DB_ESQUEMA + "INV_GetListLecturaByFiltro";
        const string SP_GET_LIST_BY_BASETYPE_AND_BASEENTRY = DB_ESQUEMA + "INV_GetListLecturaByBaseTypeBaseEntry";
        const string SP_GET_LIST_BY_BASETYPE_AND_BASEENTRY_AND_FILTRO = DB_ESQUEMA + "INV_GetListLecturaByBaseTypeAndBaseEntryAndFiltro";

        const string SP_SET_CREATE = DB_ESQUEMA + "INV_SetLecturaCreate";
        const string SP_SET_DELETE1 = DB_ESQUEMA + "INV_SetLecturaDeleteMassive";   
        const string SP_SET_DELETE2 = DB_ESQUEMA + "INV_SetLecturaDelete";

        const string SP_GET_LIST_COPY_TO_TRANSFERENCIA = DB_ESQUEMA + "INV_GetLecturaCopyToTransferencia";
        const string SP_GET_LIST_COPY_TO_TRANSFERENCIA_DETALLE = DB_ESQUEMA + "INV_GetListLecturaCopyToTransferenciaDetalle";


        public LecturaRepository(IConnectionSQL context)
            : base(context)
        {
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccionEntity<LecturaEntity>> GetListByFiltro(FilterRequestEntity value)
        {
            var response = new List<LecturaEntity>();
            var resultTransaccion = new ResultadoTransaccionEntity<LecturaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(context.GetConnectionSQL()))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FI", value.Dat1));
                        cmd.Parameters.Add(new SqlParameter("@FF", value.Dat2));
                        cmd.Parameters.Add(new SqlParameter("@BaseType", value.Cod1));
                        cmd.Parameters.Add(new SqlParameter("@Estado", value.Cod2));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<LecturaEntity>)context.ConvertTo<LecturaEntity>(reader);
                        }
                    }

                    resultTransaccion.IdRegistro = 0;
                    resultTransaccion.ResultadoCodigo = 0;
                    resultTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultTransaccion.dataList = response;
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

        public async Task<ResultadoTransaccionEntity<LecturaByBaseTypeAndBaseEntryEntity>> GetListByBaseTypeAndBaseEntry(FilterRequestEntity value)
        {
            var response = new List<LecturaByBaseTypeAndBaseEntryEntity>();
            var resultTransaccion = new ResultadoTransaccionEntity<LecturaByBaseTypeAndBaseEntryEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(context.GetConnectionSQL()))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_BASETYPE_AND_BASEENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@BaseType", value.Cod1));
                        cmd.Parameters.Add(new SqlParameter("@BaseEntry", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<LecturaByBaseTypeAndBaseEntryEntity>)context.ConvertTo<LecturaByBaseTypeAndBaseEntryEntity>(reader);
                        }
                    }

                    resultTransaccion.IdRegistro = 0;
                    resultTransaccion.ResultadoCodigo = 0;
                    resultTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultTransaccion.dataList = response;
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

        public async Task<ResultadoTransaccionEntity<LecturaByBaseTypeAndBaseEntryAndFiltroEntity>> GetListByBaseTypeAndBaseEntryAndFiltro(FilterRequestEntity value)
        {
            var response = new List<LecturaByBaseTypeAndBaseEntryAndFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccionEntity<LecturaByBaseTypeAndBaseEntryAndFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(context.GetConnectionSQL()))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_BASETYPE_AND_BASEENTRY_AND_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@BaseEntry", value.Id1));
                        cmd.Parameters.Add(new SqlParameter("@BaseType", value.Cod1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.Text1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<LecturaByBaseTypeAndBaseEntryAndFiltroEntity>)context.ConvertTo<LecturaByBaseTypeAndBaseEntryAndFiltroEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultadoTransaccion.dataList = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccionEntity<LecturaEntity>> SetCreate(LecturaEntity value)
        {
            var resultTransaccion = new ResultadoTransaccionEntity<LecturaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(context.GetConnectionSQL()))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;
                                cmdItem.Parameters.Add(new SqlParameter("@BaseType", value.BaseType));
                                cmdItem.Parameters.Add(new SqlParameter("@BaseEntry", value.BaseEntry));
                                cmdItem.Parameters.Add(new SqlParameter("@FromWhsCod", value.FromWhsCod));
                                cmdItem.Parameters.Add(new SqlParameter("@Barcode", value.Barcode));
                                cmdItem.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuarioCreate));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultTransaccion.IdRegistro = 0;
                            resultTransaccion.ResultadoCodigo = 0;
                            resultTransaccion.ResultadoDescripcion = "Registro creado con éxito ..!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultTransaccion.IdRegistro = -1;
                            resultTransaccion.ResultadoCodigo = -1;
                            resultTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
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

        public async Task<ResultadoTransaccionEntity<LecturaEntity>> SetDeleteMassive(LecturaEntity value)
        {
            var resultTransaccion = new ResultadoTransaccionEntity<LecturaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(context.GetConnectionSQL()))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_DELETE1, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;
                                cmdItem.Parameters.Add(new SqlParameter("@BaseType", value.BaseType));
                                cmdItem.Parameters.Add(new SqlParameter("@BaseEntry", value.BaseEntry));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultTransaccion.IdRegistro = 0;
                            resultTransaccion.ResultadoCodigo = 0;
                            resultTransaccion.ResultadoDescripcion = "Registro elminado con éxito ..!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultTransaccion.IdRegistro = -1;
                            resultTransaccion.ResultadoCodigo = -1;
                            resultTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
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

        public async Task<ResultadoTransaccionEntity<LecturaEntity>> SetDelete(int id)
        {
            var resultTransaccion = new ResultadoTransaccionEntity<LecturaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(context.GetConnectionSQL()))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_DELETE2, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;
                                cmdItem.Parameters.Add(new SqlParameter("@Id", id));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultTransaccion.IdRegistro = 0;
                            resultTransaccion.ResultadoCodigo = 0;
                            resultTransaccion.ResultadoDescripcion = "Registro elminado con éxito ..!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultTransaccion.IdRegistro = -1;
                            resultTransaccion.ResultadoCodigo = -1;
                            resultTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
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

        public async Task<ResultadoTransaccionEntity<LecturaCopyToTransferenciaEntity>> GetLecturaCopyToTransferencia(LecturaCopyToTransferenciaFindEntity value)
        {
            var response = new LecturaCopyToTransferenciaEntity();
            var resultTransaccion = new ResultadoTransaccionEntity<LecturaCopyToTransferenciaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(context.GetConnectionSQL()))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_COPY_TO_TRANSFERENCIA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdBase", value.IdBase));
                        cmd.Parameters.Add(new SqlParameter("@BaseType", value.BaseType));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<LecturaCopyToTransferenciaEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_COPY_TO_TRANSFERENCIA_DETALLE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        foreach (var linea in value.Linea)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@IdBase", linea.IdBase));
                            cmd.Parameters.Add(new SqlParameter("@LineBase", linea.LineBase));
                            cmd.Parameters.Add(new SqlParameter("@BaseType", linea.BaseType));
                            cmd.Parameters.Add(new SqlParameter("@Return", linea.Return));

                            var lista = new List<LecturaCopyToTransferenciaDetalleEntity>();

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                lista = (List<LecturaCopyToTransferenciaDetalleEntity>)context.ConvertTo<LecturaCopyToTransferenciaDetalleEntity>(reader);
                            }

                            foreach (var line in lista)
                            {
                                response.Linea.Add(line);
                            }
                        }
                    }

                    resultTransaccion.IdRegistro = 0;
                    resultTransaccion.ResultadoCodigo = 0;
                    resultTransaccion.ResultadoDescripcion = "Datos obtenidos con éxito ..!";
                    resultTransaccion.data = response;
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