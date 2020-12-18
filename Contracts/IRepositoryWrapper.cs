using System;
using System.Collections.Generic;
using System.Text;


namespace Contracts
{
    public interface IRepositoryWrapper
    {

        IApiRepository Api { get; }
        ICatApiRepository CatApi { get; }
        ICatFromRepository CatFrom { get; }
        ICatRoleRepository CatRole { get; }
        ICatStatusRepository CatStatus { get; }
        IColorRepository Color { get; }
        IFormsApiRepository FormsApi { get; }
        IFormsRepository Forms { get; }
        ILocationRepository Location { get; }
        IRoleFormsRepository RoleForms { get; }
        IRoleRepository Role { get; }
        IStatusRepository Status { get; }
        IStatusTypeRepository StatusType { get; }
        ISystemsRepository Systems { get; }
        ITablesRepository Tables { get; }
        ITablesServiceDiscoveryRepository TablesServiceDiscovery { get; }
        IUserRoleRepository UserRole { get; }
        IUsersRepository Users { get; }
        ISellerRepository Seller { get; }
        IProductRepository Product { get; }
        ICatProductRepository CatProduct { get; }
        ISliderRepository Slider { get; }
        ISliderPlaceRepository SliderPlace { get; }
        IPackingTypeRepository PackingType { get; }
        IPackingTypeImageRepository PackingTypeImage { get; }
        IPostTypeRepository PostType { get; }
        IFamousCommentsRepository FamousComments { get; }
        IParameterRepository Parameter { get; }
        ICatProductParametersRepository CatProductParameters { get; }
        IProductCatProductParametersRepository ProductCatProductParameters { get; }
        IProductColorRepository ProductColor { get; }
        IProductPackingTypeRepository ProductPackingType { get; }
        IRelatedProductRepository RelatedProduct { get; }
        IProductImageRepository ProductImage { get; }
        IProductCustomerRateRepository ProductCustomerRate { get; }
        IUserActivationRepository UserActivation { get; }
        ICustomerAddressRepository CustomerAddress { get; }
        ICustomerRepository Customer { get; }
        IPaymentTypeRepository PaymentType { get; }
        IPackageRepository Package { get; }
        IPackageProductRepository PackageProduct { get; }
        IOfferRepository Offer { get; }
        IOfferTypeRepository OfferType { get; }
        ICustomerOfferRepository CustomerOffer { get; }
        ICustomerOrderRepository CustomerOrder { get; }
        ICustomerOrderPaymentRepository CustomerOrderPayment { get; }
        IPostGetStateRepository PostGetState { get; }
        IGetPostCityRepository PostCity { get; }
        ICustomerOrderProductRepository CustomerOrderProduct { get; }
        IProductPackingTypeImageRepository ProductPackingTypeImage { get; }
        ICatDocumentRepositry CatDocument { get; }
        IDocumentRepository Document { get; }
        ISellerDocumentRepository SellerDocument { get; }
        IWorkRepository Work { get; }
        ISellerAddressRepository SellerAddress { get; }
        void Save();
    }
}