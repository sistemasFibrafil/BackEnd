using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
namespace Net.Data.Web
{
    public interface ILecturaRepository
    {
        Task<ResultadoTransaccionEntity<LecturaEntity>> GetListByFiltro(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<LecturaByBaseTypeAndBaseEntryEntity>> GetListByBaseTypeAndBaseEntry(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<LecturaByBaseTypeAndBaseEntryAndFiltroEntity>> GetListByBaseTypeAndBaseEntryAndFiltro(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<LecturaEntity>> SetCreate(LecturaEntity value);
        Task<ResultadoTransaccionEntity<LecturaEntity>> SetDeleteMassive(LecturaEntity value);
        Task<ResultadoTransaccionEntity<LecturaEntity>> SetDelete(int id);
        Task<ResultadoTransaccionEntity<LecturaCopyToTransferenciaEntity>> GetLecturaCopyToTransferencia(LecturaCopyToTransferenciaFindEntity value);
    }
}
