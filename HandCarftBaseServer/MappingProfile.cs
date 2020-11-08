﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using HandCarftBaseServer.Tools;

namespace HandCarftBaseServer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var now = DateTime.Now.Ticks;

            #region Color

            CreateMap<ColorDto, Color>();
            CreateMap<Color, ColorDto>();


            #endregion

            #region CatProduct

            CreateMap<CatProduct, CatProductDto>();
            CreateMap<CatProductDto, CatProduct>();


            #endregion

            #region Slider

            CreateMap<Slider, SliderDto>();
            CreateMap<SliderDto, Slider>();

            #endregion

            #region SliderPlace

            CreateMap<SliderPlace, SliderPlaceDto>();
            CreateMap<SliderPlaceDto, SliderPlace>();

            #endregion

            #region PackingType

            CreateMap<PackingTypeDto, PackingType>();
            CreateMap<PackingType, PackingTypeDto>()
                .ForMember(u => u.PackingTypeImage, opt => opt.MapFrom(x => x.PackingTypeImage));


            #endregion

            #region PackingTypeImage

            CreateMap<PackingTypeImageDto, PackingTypeImage>();
            CreateMap<PackingTypeImage, PackingTypeImageDto>();

            #endregion

            #region PostType

            CreateMap<PostTypeDto, PostType>();
            CreateMap<PostType, PostTypeDto>();



            #endregion

            #region FamousComments

            CreateMap<FamousCommentsDto, FamousComments>();
            CreateMap<FamousComments, FamousCommentsDto>();



            #endregion

            #region Parameters

            CreateMap<ParametersDto, Parameters>();
            CreateMap<Parameters, ParametersDto>();



            #endregion

            #region CatProductParameters

            CreateMap<CatProductParameters, CatProductParametersDto>();
            CreateMap<CatProductParametersDto, CatProductParameters>();



            #endregion

            #region ProductCatProductParameters


            CreateMap<ProductCatProductParameters, ProductParamDto>()
                .ForMember(u => u.ParameterName, opt => opt.MapFrom(x => x.CatProductParameters.Parameters.Name));
            CreateMap<ProductParamDto, ProductCatProductParameters>();



            #endregion

            #region ProductImage

            CreateMap<ProductImageDto, ProductImage>();
            CreateMap<ProductImage, ProductImageDto>();

            #endregion

            #region Product


            CreateMap<Product, ProductDto>()
                .ForMember(u => u.CatProductName, opt => opt.MapFrom(x => x.CatProduct.Name))
                .ForMember(u => u.SellerName, opt => opt.MapFrom(x => x.Seller.Name + " " + x.Seller.Fname))
                .ForMember(u => u.FinalStatus, opt => opt.MapFrom(x => x.FinalStatus.Name))
                .ForMember(u => u.LastSeenDate,
                    opt => opt.MapFrom(x => DateTimeFunc.TimeTickToShamsi(x.LastSeenDate.Value)))
                .ForMember(u => u.Rating, opt => opt.MapFrom(x => x.ProductCustomerRate.Average(c => c.Rate)))
                .ForMember(u => u.OfferId,
                    opt => opt.MapFrom(x =>
                        x.ProductOffer
                            .Where(c => c.Offer.FromDate < now && now < c.Offer.ToDate && c.DaDate == null &&
                                        c.Ddate == null).Select(c => c.OfferId).FirstOrDefault()))
                .ForMember(u => u.OfferPercent, opt => opt.MapFrom(x =>
                    x.ProductOffer
                        .Where(c => c.Offer.FromDate < now && now < c.Offer.ToDate && c.DaDate == null &&
                                    c.Ddate == null).Select(c => c.Offer.Value).DefaultIfEmpty(0).FirstOrDefault()))
                .ForMember(u => u.OfferAmount, opt => opt.MapFrom(x =>
                    (x.ProductOffer
                        .Where(c => c.Offer.FromDate < now && now < c.Offer.ToDate && c.DaDate == null &&
                                    c.Ddate == null).Select(c => c.Offer.Value).DefaultIfEmpty(0).FirstOrDefault()) * x.Price / 100))
                .ForMember(u => u.PriceAftterOffer, opt => opt.MapFrom(x =>
                    x.Price - ((x.ProductOffer
                                   .Where(c => c.Offer.FromDate < now && now < c.Offer.ToDate && c.DaDate == null &&
                                               c.Ddate == null).Select(c => c.Offer.Value).DefaultIfEmpty(0).FirstOrDefault()) * x.Price /
                               100)));




            #endregion

            #region ProductCustomerRate

            CreateMap<ProductCustomerRate, ProductCustomerRateDto>()
                .ForMember(u => u.CustomerName, opt => opt.MapFrom(x => x.Customer.Name + " " + x.Customer.Fname))
                .ForMember(u => u.ProductCustomerRateImages, opt => opt.MapFrom(x => x.ProductCustomerRateImage));

            #endregion

            #region ProductCustomerRateImage

            CreateMap<ProductCustomerRateImage, ProductCustomerRateImageDto>();

            #endregion

            #region ProductColor

            CreateMap<ProductColor, ProductColorDto>()
                .ForMember(u => u.ColorName, opt => opt.MapFrom(x => x.Color.Name))
                .ForMember(u => u.ColorCode, opt => opt.MapFrom(x => x.Color.ColorCode));
            CreateMap<ProductColorDto, ProductColor>();

            #endregion

            #region ProductPackingType

            CreateMap<ProductPackingType, ProductPackingTypeDto>()
                .ForMember(u => u.PackingTypeName, opt => opt.MapFrom(x => x.PackinggType.Name));

            #endregion

            #region CustomerAddress

            CreateMap<CustomerAddress, CustomerAddressDto>()
                .ForMember(u => u.CityName, opt => opt.MapFrom(x => x.City.Name))
                .ForMember(u => u.ProvinceName, opt => opt.MapFrom(x => x.Province.Name));
            CreateMap<CustomerAddressDto, CustomerAddress>();
            #endregion

            #region PaymentType

            CreateMap<PaymentType, PaymentTypeDto>();

            #endregion

            #region Package

            CreateMap<Package, PackageDto>()
                .ForMember(u => u.StartDate,
                    opt => opt.MapFrom(x => DateTimeFunc.TimeTickToShamsi(x.StartDateTime.Value)))
                .ForMember(u => u.EndDate, opt => opt.MapFrom(x => DateTimeFunc.TimeTickToShamsi(x.EndDateTime.Value)));

            #endregion

            #region PackageImage

            CreateMap<PackageImage, PackageImageDto>();

            #endregion

            #region Offer

            CreateMap<Offer, OfferDto>()
                .ForMember(u => u.OfferId, opt => opt.MapFrom(x => x.Id));

            #endregion

            #region CustomerOffer

            CreateMap<CustomerOffer, OfferDto>()
                .ForMember(u => u.OfferId, opt => opt.MapFrom(x => x.OfferId))
                .ForMember(u => u.CustomerOfferId, opt => opt.MapFrom(x => x.Id));



            #endregion

            #region Location

            CreateMap<Location, LocationDto>();

            #endregion

            #region CustomerOrderProduct

            CreateMap<CustomerOrderProduct, CustomerOrderProductDto>()
                .ForMember(u => u.SellerName, opt => opt.MapFrom(x => x.Seller.Name + " " + x.Seller.Fname))
                .ForMember(u => u.StatusName, opt => opt.MapFrom(x => x.FinalStatus.Name))
                .ForMember(u => u.PackingTypeName, opt => opt.MapFrom(x => x.PackingType.Name));

            #endregion

            #region CustomerOrder

            CreateMap<CustomerOrder, CustomerOrderFullDto>()
                .ForMember(u => u.DeliveryDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.DeliveryDate.Value)))
                .ForMember(u => u.OrderDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.OrderDate.Value)))
                .ForMember(u => u.SendDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.SendDate.Value)))
                .ForMember(u => u.PaymentTypeName, opt => opt.MapFrom(x => x.PaymentType.Title))
                .ForMember(u => u.PostTypeName, opt => opt.MapFrom(x => x.PostType.Title))
                .ForMember(u => u.FinalStatus, opt => opt.MapFrom(x => x.FinalStatus.Name))
                .ForMember(u => u.PaymentStatus,
                    opt => opt.MapFrom(x =>
                        x.CustomerOrderPayment.Where(c => c.Ddate == null && c.DaDate == null)
                            .OrderByDescending(c => c.TransactionDate).Select(c => c.FinalStatus.Name)
                            .FirstOrDefault()));


            CreateMap<CustomerOrder, CustomerOrderDto>()
                .ForMember(u => u.DeliveryDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.DeliveryDate.Value)))
                .ForMember(u => u.OrderDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.OrderDate.Value)))
                .ForMember(u => u.SendDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.SendDate.Value)))
                .ForMember(u => u.PaymentTypeName, opt => opt.MapFrom(x => x.PaymentType.Title))
                .ForMember(u => u.PostTypeName, opt => opt.MapFrom(x => x.PostType.Title))
                .ForMember(u => u.FinalStatus, opt => opt.MapFrom(x => x.FinalStatus.Name))
                .ForMember(u => u.PaymentStatus,
                    opt => opt.MapFrom(x =>
                        x.CustomerOrderPayment.Where(c => c.Ddate == null && c.DaDate == null)
                            .OrderByDescending(c => c.TransactionDate).Select(c => c.FinalStatus.Name)
                            .FirstOrDefault()));



            #endregion

            #region Document

            CreateMap<Document, DocumentDto>()
               .ForMember(u => u.CatDocumentName,
                   opt => opt.MapFrom(x => x.CatDocument.Title));
            CreateMap<DocumentDto, Document>();
            #endregion

        }
    }
}