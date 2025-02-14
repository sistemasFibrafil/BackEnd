﻿using System;
using Net.Business.Entities.Web;
using System.Collections.Generic;
namespace Net.Business.DTO.Web
{
    public class TransferenciaStockCreateRequestDto
    {
        public int Id { get; set; }
        public string TipDocumento { get; set; }
        public string SerDocumento { get; set; }
        public string NumDocumento { get; set; }
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

        public string CodTipTransporte { get; set; }
        public string CodTipDocTransportista { get; set; }
        public string NumTipoDocTransportista { get; set; }
        public string NomTransportista { get; set; }
        public string NumPlaVehTransportista { get; set; }

        public string CodTipDocConductor { get; set; }
        public string NumTipoDocConductor { get; set; }
        public string NomConductor { get; set; }
        public string ApeConductor { get; set; }
        public string NomComConductor { get; set; }
        public string NumLicConductor { get; set; }

        public string CodTipTraslado { get; set; }
        public string CodMotTraslado { get; set; }
        public string CodTipSalida { get; set; }

        public int SlpCode { get; set; }
        public decimal NumBulto { get; set; }
        public decimal TotKilo { get; set; }
        public string JrnlMemo { get; set; } = null;
        public string Comments { get; set; } = null;

        public int? IdUsuarioCreate { get; set; } = null;
        public List<TransferenciaStockDetalleCreateRequestDto> Linea { get; set; } = new List<TransferenciaStockDetalleCreateRequestDto>();

        public TransferenciaStockEntity ReturnValue()
        {
            var value = new TransferenciaStockEntity()
            {
                Id = Id,
                TipDocumento = TipDocumento,
                SerDocumento = SerDocumento,
                NumDocumento = NumDocumento,
                DocDate = DocDate,
                DocDueDate = DocDueDate,
                TaxDate = TaxDate,
                CardCode = CardCode,
                CardName = CardName,
                CntctCode = CntctCode,
                Address = Address,

                Filler = Filler,
                ToWhsCode = ToWhsCode,

                CodTipTransporte = CodTipTransporte,
                CodTipDocTransportista = CodTipDocTransportista,
                NumTipoDocTransportista = NumTipoDocTransportista,
                NomTransportista = NomTransportista,
                NumPlaVehTransportista = NumPlaVehTransportista,

                CodTipDocConductor = CodTipDocConductor,
                NumTipoDocConductor = NumTipoDocConductor,
                NomConductor = NomConductor,
                ApeConductor = ApeConductor,
                NomComConductor = NomComConductor,
                NumLicConductor = NumLicConductor,

                CodTipTraslado = CodTipTraslado,
                CodMotTraslado = CodMotTraslado,
                CodTipSalida = CodTipSalida,

                SlpCode = SlpCode,
                NumBulto = NumBulto,
                TotKilo = TotKilo,
                JrnlMemo = JrnlMemo,
                Comments = Comments,
                IdUsuarioCreate = IdUsuarioCreate,
            };
            foreach (var linea in Linea)
            {
                value.Linea.Add(new TransferenciaStockDetalleEntity()
                {
                    Id = linea.Id,
                    Line = linea.Line,
                    IdLectura = linea.IdLectura,
                    IdBase = linea.IdBase,
                    LineBase = linea.LineBase,
                    BaseType = linea.BaseType,
                    BaseEntry = linea.BaseEntry,
                    BaseLine = linea.BaseLine,
                    Read = linea.Read,
                    ItemCode = linea.ItemCode,
                    Dscription = linea.Dscription,
                    FromWhsCod = linea.FromWhsCod,
                    WhsCode = linea.WhsCode,
                    CodTipOperacion = linea.CodTipOperacion,
                    UnitMsr = linea.UnitMsr,
                    Quantity = linea.Quantity,
                    OpenQty = linea.OpenQty,
                    IdUsuarioCreate = linea.IdUsuarioCreate,
                });
            }

            return value;
        }
    }

    public class TransferenciaStockDetalleCreateRequestDto
    {
        public int Id { get; set; }
        public int Line { get; set; }
        public int IdLectura { get; set; }
        public int IdBase { get; set; }
        public int LineBase { get; set; }
        public string BaseType { get; set; } = "-1";
        public int BaseEntry { get; set; }
        public int BaseLine { get; set; }
        public string Read { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string FromWhsCod { get; set; }
        public string WhsCode { get; set; }
        public string CodTipOperacion { get; set; }
        public string UnitMsr { get; set; }
        public decimal Quantity { get; set; }
        public decimal OpenQty { get; set; }
        public int? IdUsuarioCreate { get; set; } = null;
    }
}
