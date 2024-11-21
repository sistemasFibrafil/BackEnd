﻿using System.IO;
using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IFacturaVentaSapRepository
    {
        Task<ResultadoTransaccionEntity<VentaProyeccionSapByFechaEntity>> GetListVentaProyeccionByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<FacturaVentaSapEntity>> GetListVentaResumenByFechaGrupo(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetVentaResumenExcelByFechaGrupo(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<VentaSapByFechaSlpCodeEntity>> GetListVentaByFechaAndSlpCode(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetVentaExcelByFechaAndSlpCode(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<FacturaVentaSapByFechaEntity>> GetListFacturaVentaByFecha(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<MemoryStream>> GetFacturaVentaExcelByFecha(FilterRequestEntity value);
    }
}
