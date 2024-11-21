using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
namespace Net.Data.Web
{
    public interface ISolicitudTrasladoRepository
    {
        Task<ResultadoTransaccionEntity<SolicitudTrasladoEntity>> GetNumber();
        Task<ResultadoTransaccionEntity<SolicitudTrasladoEntity>> GetListByFiltro(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<SolicitudTrasladoEntity>> GetById(int id);
        Task<ResultadoTransaccionEntity<SolicitudTrasladoEntity>> SetCreate(SolicitudTrasladoEntity value);
        Task<ResultadoTransaccionEntity<SolicitudTrasladoEntity>> SetUpdate(SolicitudTrasladoEntity value);
        Task<ResultadoTransaccionEntity<SolicitudTrasladoEntity>> SetClose(SolicitudTrasladoEntity value);
    }
}
