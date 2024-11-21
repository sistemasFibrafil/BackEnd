using System;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using Net.Business.Entities;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
namespace Net.Data.Sap
{
    public class TipoCambioSapRepository : RepositoryBase<TipoCambioSapEntity>, ITipoCambioSapRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_FECHA_CODE = DB_ESQUEMA + "SEG_GetTipoCambioByFechaAndCode";


        public TipoCambioSapRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _cnxSap = Utilidades.GetCon(configuration, "EntornoConnectionSap:Entorno");
        }


        public async Task<ResultadoTransaccionEntity<TipoCambioSapEntity>> GetByFechaCode(FilterRequestEntity value)
        {
            var response = new TipoCambioSapEntity();
            var resultTransaccion = new ResultadoTransaccionEntity<TipoCambioSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultTransaccion.NombreMetodo = _metodoName;
            resultTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_FECHA_CODE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Fecha", value.Dat1));
                        cmd.Parameters.Add(new SqlParameter("@Code", value.Cod1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<TipoCambioSapEntity>(reader);
                        }
                    }

                    resultTransaccion.IdRegistro = 0;
                    resultTransaccion.ResultadoCodigo = 0;
                    resultTransaccion.ResultadoDescripcion = "Dato obtenido con éxito.";
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
