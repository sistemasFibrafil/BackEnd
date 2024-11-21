using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IPagoRecibidoSapRepository
    {
        Task<ResultadoTransaccionEntity<CobranzaCarteraVencidaByFechaCorteSapEntity>> GetListCobranzaCarteraVencidaByFechaCorte(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetListCobranzaCarteraVencidaExcelByFechaCorte(FilterRequestEntity value);
    }
}
