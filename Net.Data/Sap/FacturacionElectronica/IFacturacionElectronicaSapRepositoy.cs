﻿using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Sap;
namespace Net.Data.Sap
{
    public interface IFacturacionElectronicaSapRepositoy
    {
        Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> GetListGuiaElectronicaByFiltro(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> SendGuiaElectronica(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> GetListGuiaInternaElectronicaByFiltro(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<FacturacionElectronicaSapEntity>> SendGuiaInternaElectronica(FilterRequestEntity value);
    }
}