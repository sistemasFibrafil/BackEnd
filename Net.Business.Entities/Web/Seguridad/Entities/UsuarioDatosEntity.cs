using System.Collections.Generic;
namespace Net.Business.Entities.Web
{
    public class UsuarioDatosEntity
    {
        public int? IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int? IdPersona { get; set; }
        public int? IdPerfil { get; set; }
        public int? CodSede { get; set; }
        public int? CodVendedorSAP { get; set; }
        public string WarehouseDefault { get; set; }
        public List<MenuEntity> ListaAccesoMenu { get; set; }
    }
}
