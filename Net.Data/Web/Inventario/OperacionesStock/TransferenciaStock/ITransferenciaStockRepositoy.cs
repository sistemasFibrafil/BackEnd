using Net.Business.Entities.Web;
using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Net.Data.Web
{
    public interface ITransferenciaStockRepositoy
    {
        Task<ResultadoTransaccionEntity<TransferenciaStockRepositoy>> GetNumber();
    }
}
