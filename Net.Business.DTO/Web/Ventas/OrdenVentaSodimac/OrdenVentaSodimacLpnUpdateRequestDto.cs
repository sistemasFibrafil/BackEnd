﻿using Net.Business.Entities.Web;
using System.Collections.Generic;
namespace Net.Business.DTO.Web
{
    public class OrdenVentaSodimacLpnUpdateRequestDto
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public List<OrdenVentaDetalleSodimacLpnUpdateRequestDto> Item { get; set; } = new List<OrdenVentaDetalleSodimacLpnUpdateRequestDto>();

        public OrdenVentaSodimacEntity ReturnValue()
        {
            var value = new OrdenVentaSodimacEntity()
            {
                Id = this.Id,
            };

            foreach (var item in Item)
            {
                value.Item.Add(new OrdenVentaDetalleSodimacEntity()
                {
                    Id = item.Id,
                    Line = item.Line,
                    NumLocal = item.NumLocal,
                });
            }

            return value;
        }
    }

    public class OrdenVentaDetalleSodimacLpnUpdateRequestDto
    {
        public int Id { get; set; }
        public int Line { get; set; }
        public int NumLocal { get; set; }
    }
}
