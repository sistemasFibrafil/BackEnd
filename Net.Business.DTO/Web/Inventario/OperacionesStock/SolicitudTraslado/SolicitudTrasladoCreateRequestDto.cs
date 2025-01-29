﻿using System;
using Net.Business.Entities.Web;
using System.Collections.Generic;
namespace Net.Business.DTO.Web
{
    public class SolicitudTrasladoCreateRequestDto
    {
        public int Id { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string Read { get; set; } = null;
        public string CardCode { get; set; } = null;
        public string CardName { get; set; } = null;
        public int CntctCode { get; set; } = 0;
        public string Address { get; set; } = null;
        public string Filler { get; set; }
        public string ToWhsCode { get; set; }
        public string CodTipTraslado { get; set; }
        public string CodMotTraslado { get; set; }
        public string CodTipSalida { get; set; }
        public int SlpCode { get; set; }
        public string JrnlMemo { get; set; } = null;
        public string Comments { get; set; } = null;
        public int? IdUsuarioCreate { get; set; } = null;
        public List<SolicitudTrasladoDetalleCreateRequestDto> Linea { get; set; } = new List<SolicitudTrasladoDetalleCreateRequestDto>();

        public SolicitudTrasladoEntity ReturnValue()
        {
            var value = new SolicitudTrasladoEntity()
            {
                Id = Id,
                DocDate = DocDate,
                DocDueDate = DocDueDate,
                TaxDate = TaxDate,
                Read = Read,
                CardCode = CardCode,
                CardName = CardName,
                CntctCode = CntctCode,
                Address = Address,
                Filler = Filler,
                ToWhsCode = ToWhsCode,
                CodTipTraslado = CodTipTraslado,
                CodMotTraslado = CodMotTraslado,
                CodTipSalida = CodTipSalida,
                SlpCode = SlpCode,
                JrnlMemo = JrnlMemo,
                Comments = Comments,
                IdUsuarioCreate = IdUsuarioCreate,
            };
            foreach (var linea in Linea)
            {
                value.Linea.Add(new SolicitudTrasladoDetalleEntity()
                {
                    Id = linea.Id,
                    Line = linea.Line,
                    ItemCode = linea.ItemCode,
                    Dscription = linea.Dscription,
                    FromWhsCod = linea.FromWhsCod,
                    WhsCode = linea.WhsCode,
                    UnitMsr = linea.UnitMsr,
                    Quantity = linea.Quantity,
                    OpenQty = linea.OpenQty,
                    OpenQtyRding = linea.OpenQtyRding,
                    IdUsuarioCreate = linea.IdUsuarioCreate,
                });
            }

            return value;
        }
    }

    public class SolicitudTrasladoDetalleCreateRequestDto
    {
        public int Id { get; set; }
        public int Line { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string FromWhsCod { get; set; }
        public string WhsCode { get; set; }
        public string UnitMsr { get; set; }
        public decimal Quantity { get; set; }
        public decimal OpenQty { get; set; }
        public decimal OpenQtyRding { get; set; }
        public int? IdUsuarioCreate { get; set; } = null;
    }
}
