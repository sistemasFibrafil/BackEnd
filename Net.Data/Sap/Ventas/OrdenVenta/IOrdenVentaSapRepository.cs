using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IOrdenVentaSapRepository
    {
        Task<ResultadoTransaccionEntity<OrdenVentaSapByFechaEntity>> GetListOrdenVentaSeguimientoByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetOrdenVentaSeguimientoExcelByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<OrdenVentaSapByFechaEntity>> GetListOrdenVentaSeguimientoDetalladoByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetOrdenVentaSeguimientoDetalladoExcelByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<OrdenVentaSapByFechaEntity>> GetListOrdenVentaPendienteStockAlmacenProduccionByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetOrdenVentaPendienteStockAlmacenProduccionExcelByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<OrdenVentaSapByFechaEntity>> GetListOrdenVentaProgramacionByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetOrdenVentaProgramacionExcelByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<OrdenVentaSodimacSapEntity>> GetListOrdenVentaSodimacPendienteByFiltro(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<OrdenVentaSodimacSapEntity>> GetOrdenVentaSodimacPendienteById(FilterRequestEntity value);
    }
}
