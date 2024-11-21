using Net.Business.Entities.Sap;
using Net.Business.Entities;
using System.Threading.Tasks;
namespace Net.Data.Sap
{
    public interface IGrupoArticuloSapRepository
    {
        Task<ResultadoTransaccionEntity<GrupoArticuloSapEntity>> GetList();
    }
}
