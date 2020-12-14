using System;
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
            CreateMap<CatProduct, CatProductWithCountDto>()
                .ForMember(u => u.ProductCount, opt => opt.MapFrom(x => x.Product.Count + (x.InverseP.Select(w => w.Product.Count).Sum())));


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

            CreateMap<Product, ProductGeneralSearchResultDto>()
                .ForMember(u => u.ProductId, opt => opt.MapFrom(x => x.Id))
                .ForMember(u => u.ProductName, opt => opt.MapFrom(x => x.Name))
                .ForMember(u => u.CatProductId, opt => opt.MapFrom(x => x.CatProduct.Id))
                .ForMember(u => u.CatProductName, opt => opt.MapFrom(x => x.CatProduct.Name))
                .ForMember(u => u.CatProductCode, opt => opt.MapFrom(x => x.CatProduct.Coding));


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
                .ForMember(u => u.PackingTypeName, opt => opt.MapFrom(x => x.PackingType.Name))
                .ForMember(u => u.ProductImage, opt => opt.MapFrom(x => x.Product.CoverImageUrl));


            CreateMap<CustomerOrderProduct, CustomerOrderProductSampleDto>()
                .ForMember(u => u.CoverImage, opt => opt.MapFrom(x => x.Product.CoverImageUrl));

            #endregion

            #region CustomerOrder

            CreateMap<CustomerOrder, CustomerOrderFullDto>()
                .ForMember(u => u.DeliveryDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.DeliveryDate.Value)))
                .ForMember(u => u.OrderDate,
                    opt => opt.MapFrom(x =>
                        x.OrderDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.OrderDate.Value)))
                .ForMember(u => u.SendDate,
                    opt => opt.MapFrom(x =>
                        x.SendDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.SendDate.Value)))
                .ForMember(u => u.PaymentTypeName, opt => opt.MapFrom(x => x.PaymentType.Title))
                .ForMember(u => u.PostTypeName, opt => opt.MapFrom(x => x.PostType.Title))
                .ForMember(u => u.FinalStatus, opt => opt.MapFrom(x => x.FinalStatus.Name))
                .ForMember(u => u.CustomerAddress,
                    opt => opt.MapFrom(x =>
                        x.CustomerAddress.Province.Name + " - " + x.CustomerAddress.City.Name + " - " +
                        x.CustomerAddress.Address))
                .ForMember(u => u.CustomerMobile, opt => opt.MapFrom(x => x.Customer.Mobile))
                .ForMember(u => u.CustomerName, opt => opt.MapFrom(x => x.Customer.Name + " " + x.Customer.Fname))
                .ForMember(u => u.PaymentStatus,
                    opt => opt.MapFrom(x =>
                        x.CustomerOrderPayment.Where(c => c.Ddate == null && c.DaDate == null)
                            .OrderByDescending(c => c.TransactionDate).Select(c => c.FinalStatus.Name)
                            .FirstOrDefault()))
                .ForMember(u => u.CustomerOrderProductsList, opt => opt.MapFrom(x => x.CustomerOrderProduct))
                .ForMember(u => u.CustomerOrderPaymentList, opt => opt.MapFrom(x => x.CustomerOrderPayment));


            CreateMap<CustomerOrder, CustomerOrderDto>()
                .ForMember(u => u.DeliveryDate,
                    opt => opt.MapFrom(x =>
                        x.DeliveryDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.DeliveryDate.Value)))
                .ForMember(u => u.OrderDate,
                    opt => opt.MapFrom(x =>
                        x.OrderDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.OrderDate.Value)))
                .ForMember(u => u.SendDate,
                    opt => opt.MapFrom(x =>
                        x.SendDate == null ? "" : DateTimeFunc.TimeTickToShamsi(x.SendDate.Value)))
                .ForMember(u => u.PaymentTypeName, opt => opt.MapFrom(x => x.PaymentType.Title))
                .ForMember(u => u.PostTypeName, opt => opt.MapFrom(x => x.PostType.Title))
                .ForMember(u => u.FinalStatus, opt => opt.MapFrom(x => x.FinalStatus.Name))
                .ForMember(u => u.PaymentStatus,
                    opt => opt.MapFrom(x =>
                        x.CustomerOrderPayment.Where(c => c.Ddate == null && c.DaDate == null)
                            .OrderByDescending(c => c.TransactionDate).Select(c => c.FinalStatus.Name)
                            .FirstOrDefault()))
                .ForMember(u => u.ProductList, opt => opt.MapFrom(x => x.CustomerOrderProduct));



            #endregion

            #region CustomerOrderPayment

            CreateMap<CustomerOrderPayment, CustomerOrderPaymentDto>()
                .ForMember(u => u.FinalStatus, opt => opt.MapFrom(x => x.FinalStatus.Name))
                .ForMember(u => u.TransactionDate,
                    opt => opt.MapFrom(x =>
                        x.TransactionDate == null ? "---" : DateTimeFunc.TimeTickToShamsi(x.TransactionDate.Value)));
            #endregion

            #region Document

            CreateMap<Document, DocumentDto>()
               .ForMember(u => u.CatDocumentName,
                   opt => opt.MapFrom(x => x.CatDocument.Title));
            CreateMap<DocumentDto, Document>();
            #endregion

            #region Work

            CreateMap<Work, WorkDto>();
            CreateMap<WorkDto, Work>();

            #endregion

            #region Customer

            CreateMap<CustomerProfileDto, Customer>();
            CreateMap<Customer, CustomerProfileDto>().ForMember(u => u.Bdate,
                opt => opt.MapFrom(x => DateTimeFunc.TimeTickToMiladi(x.Bdate.Value)));

            #endregion

            #region SellerDocument

            CreateMap<SellerDocument, SellerDocumentDto>()
                .ForMember(u => u.DocumentName,
                    opt => opt.MapFrom(x => x.Document.Title))
                .ForMember(u => u.FianlStatus,
                    opt => opt.MapFrom(x => x.FianlStatus.Name));


            #endregion

            #region Seller

            CreateMap<Seller, SellerFullInfoDto>()
                .ForMember(u => u.Bdate,
                    opt => opt.MapFrom(x => x.Bdate == null ? DateTime.Now.ToString() : DateTimeFunc.TimeTickToMiladi(x.Bdate.Value)))
                .ForMember(u => u.AddressList,
                    opt => opt.MapFrom(x => x.SellerAddress))
                .ForMember(u => u.DocumentList,
                    opt => opt.MapFrom(x => x.SellerDocument))
                .ForMember(u => u.SellerId,
                    opt => opt.MapFrom(x => x.Id));

            #endregion

            #region SellerAddress

            CreateMap<SellerAddress, SellerAddressDto>();


            #endregion

        }
    }
}