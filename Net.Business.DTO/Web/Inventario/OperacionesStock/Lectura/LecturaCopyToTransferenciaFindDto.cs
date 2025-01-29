using Net.Business.Entities.Web;
using System.Collections.Generic;
namespace Net.Business.DTO.Web
{
    public class LecturaCopyToTransferenciaFindDto
    {
        public string BaseType { get; set; }
        public int IdBase { get; set; }
        public List<LecturaCopyToTransferenciaDetalleFindDto> Linea { get; set; } = new List<LecturaCopyToTransferenciaDetalleFindDto>();

        public LecturaCopyToTransferenciaFindEntity ReturnValue()
        {
            var value = new LecturaCopyToTransferenciaFindEntity()
            {
                IdBase = IdBase,
                BaseType = BaseType,
            };
            foreach (var linea in Linea)
            {
                value.Linea.Add( new LecturaCopyToTransferenciaDetalleFindEntity()
                {
                    BaseType = linea.BaseType,
                    IdBase = linea.IdBase,
                    LineBase = linea.LineBase,
                    Return = linea.Return,
                });
            }

            return value;
        }
    }

    public class LecturaCopyToTransferenciaDetalleFindDto
    {
        public string BaseType { get; set; }
        public int IdBase { get; set; }
        public int LineBase { get; set; }
        public string Return { get; set; }
    }
}
