using Net.Business.Entities.Web;
namespace Net.Business.DTO.Web
{
    public class LecturaDeleteRequestDto
    {
        public string BaseType { get; set; }
        public int BaseEntry { get; set; }

        public LecturaEntity ReturnValue()
        {
            return new LecturaEntity()
            {
                BaseType = this.BaseType,
                BaseEntry = this.BaseEntry
            };
        }
    }
}
