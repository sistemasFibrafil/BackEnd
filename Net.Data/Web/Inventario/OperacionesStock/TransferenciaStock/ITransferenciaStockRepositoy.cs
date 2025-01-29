using Net.Business.Entities;
using System.Threading.Tasks;
using Net.Business.Entities.Web;
namespace Net.Data.Web
{
    public interface ITransferenciaStockRepositoy
    {
        Task<ResultadoTransaccionEntity<TransferenciaStockEntity>> GetNumber();
        Task<ResultadoTransaccionEntity<TransferenciaStockEntity>> GetListByFiltro(FilterRequestEntity value);
        Task<ResultadoTransaccionEntity<TransferenciaStockEntity>> GetById(int id);
        Task<ResultadoTransaccionEntity<TransferenciaStockEntity>> SetCreate1(TransferenciaStockEntity value);
        Task<ResultadoTransaccionEntity<TransferenciaStockEntity>> SetCreate2(TransferenciaStockEntity value);
        Task<ResultadoTransaccionEntity<TransferenciaStockEntity>> SetUpdate(TransferenciaStockEntity value);
    }
}
