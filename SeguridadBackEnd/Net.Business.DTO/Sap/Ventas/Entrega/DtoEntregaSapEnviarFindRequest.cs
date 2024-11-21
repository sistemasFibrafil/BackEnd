using Net.Business.Entities.Sap;
namespace Net.Business.DTO.Sap
{
    public class EntregaSapEnviarFindRequestDto
    {
        public int DocEntry { get; set; }

        public EntregaSapEntity RetornaEntregaEnviar()
        {
            return new EntregaSapEntity
            {
                DocEntry = DocEntry
            };
        }
    }
}
