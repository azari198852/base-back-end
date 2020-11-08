using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private BaseContext _repoContext;
        public IApiRepository _api;
        public ICatApiRepository _catApi;
        public ICatFromRepository _catFrom;
        public ICatRoleRepository _catRole;
        public ICatStatusRepository _catStatus;
        public IColorRepository _color;
        public IFormsApiRepository _formsApi;
        public IFormsRepository _forms;
        public ILocationRepository _location;
        public IRoleFormsRepository _roleForms;
        public IRoleRepository _role;
        public IStatusRepository _status;
        public IStatusTypeRepository _statusType;
        public ISystemsRepository _systems;
        public ITablesRepository _tables;
        public ITablesServiceDiscoveryRepository _tablesServiceDiscovery;
        public IUserRoleRepository _userRole;
        public IUsersRepository _users;
        public ISellerRepository _seller;
        public IProductRepository _product;
        public ICatProductRepository _catProduct;
        public ISliderRepository _slider;
        public ISliderPlaceRepository _sliderPlace;
        public IPackingTypeRepository _packingType;
        public IPackingTypeImageRepository _packingTypeImage;
        public IPostTypeRepository _postType;
        public IFamousCommentsRepository _famousComments;
        public IParameterRepository _parameter;
        private ICatProductParametersRepository _catProductParameters;
        private IProductCatProductParametersRepository _productCatProductParameters;
        private IProductColorRepository _productColor;
        private IProductPackingTypeRepository _productPackingType;
        private IRelatedProductRepository _relatedProduct;
        private IProductImageRepository _productImage;
        private IProductCustomerRateRepository _productCustomerRate;
        private IUserActivationRepository _userActivation;
        private ICustomerAddressRepository _customerAddress;
        private ICustomerRepository _customer;
        private IPaymentTypeRepository _paymentType;
        private IPackageRepository _package;
        private IPackageProductRepository _packageProduct;
        private IOfferRepository _offer;
        private IOfferTypeRepository _offerType;
        private ICustomerOfferRepository _customerOffer;
        private ICustomerOrderRepository _customerOrder;
        private ICustomerOrderPaymentRepository _customerOrderPayment;
        private IPostGetStateRepository _postGetState;
        private IGetPostCityRepository _postCity;
        private ICustomerOrderProductRepository _customerOrderProduct;
        private IProductPackingTypeImageRepository _productPackingTypeImage;
        private ICatDocumentRepositry _catDocument;
        private IDocumentRepository _document;
        private ISellerDocumentRepository _sellerDocument;


        public IApiRepository Api => _api ??= new ApiRepository(_repoContext);
        public ICatApiRepository CatApi => _catApi ??= new CatApiRepository(_repoContext);
        public ICatFromRepository CatFrom => _catFrom ??= new CatFromRepository(_repoContext);
        public ICatRoleRepository CatRole => _catRole ??= new CatRoleRepository(_repoContext);
        public ICatStatusRepository CatStatus => _catStatus ??= new CatStatusRepository(_repoContext);
        public IColorRepository Color => _color ??= new ColorRepository(_repoContext);
        public IFormsApiRepository FormsApi => _formsApi ??= new FormsApiRepository(_repoContext);
        public IFormsRepository Forms => _forms ??= new FormsRepository(_repoContext);
        public ILocationRepository Location => _location ??= new LocationRepository(_repoContext);
        public IRoleFormsRepository RoleForms => _roleForms ??= new RoleFormsRepository(_repoContext);
        public IRoleRepository Role => _role ??= new RoleRepository(_repoContext);
        public IStatusRepository Status => _status ??= new StatusRepository(_repoContext);
        public IStatusTypeRepository StatusType => _statusType ??= new StatusTypeRepository(_repoContext);
        public ISystemsRepository Systems => _systems ??= new SystemsRepository(_repoContext);
        public ITablesRepository Tables => _tables ??= new TablesRepository(_repoContext);
        public ITablesServiceDiscoveryRepository TablesServiceDiscovery => _tablesServiceDiscovery ??= new TablesServiceDiscoveryRepository(_repoContext);
        public IUserRoleRepository UserRole => _userRole ??= new UserRoleRepository(_repoContext);
        public IUsersRepository Users => _users ??= new UsersRepository(_repoContext);
        public ISellerRepository Seller => _seller ??= new SellerRepository(_repoContext);
        public IProductRepository Product => _product ??= new ProductRepository(_repoContext);
        public ICatProductRepository CatProduct => _catProduct ??= new CatProductRepository(_repoContext);
        public ISliderRepository Slider => _slider ??= new SliderRepository(_repoContext);
        public ISliderPlaceRepository SliderPlace => _sliderPlace ??= new SliderPlaceRepository(_repoContext);
        public IPackingTypeRepository PackingType => _packingType ??= new PackingTypeRepository(_repoContext);
        public IPackingTypeImageRepository PackingTypeImage =>
            _packingTypeImage ??= new PackingTypeImageRepository(_repoContext);
        public IPostTypeRepository PostType => _postType ??= new PostTypeRepository(_repoContext);
        public IFamousCommentsRepository FamousComments => _famousComments ??= new FamousCommentsRepository(_repoContext);
        public IParameterRepository Parameter => _parameter ??= new ParameterRepository(_repoContext);
        public ICatProductParametersRepository CatProductParameters => _catProductParameters ??= new CatProductParametersRepository(_repoContext);
        public IProductCatProductParametersRepository ProductCatProductParameters => _productCatProductParameters ??= new ProductCatProductParametersRepository(_repoContext);
        public IProductColorRepository ProductColor => _productColor ??= new ProductColorRepository(_repoContext);
        public IProductPackingTypeRepository ProductPackingType => _productPackingType ??= new ProductPackingTypeRepository(_repoContext);
        public IRelatedProductRepository RelatedProduct => _relatedProduct ??= new RelatedProductRepository(_repoContext);
        public IProductImageRepository ProductImage => _productImage ??= new ProductImageRepository(_repoContext);
        public IProductCustomerRateRepository ProductCustomerRate => _productCustomerRate ??= new ProductCustomerRateRepository(_repoContext);
        public IUserActivationRepository UserActivation => _userActivation ??= new UserActivationRepository(_repoContext);
        public ICustomerAddressRepository CustomerAddress => _customerAddress ??= new CustomerAddressRepository(_repoContext);
        public ICustomerRepository Customer => _customer ??= new CustomerRepository(_repoContext);
        public IPaymentTypeRepository PaymentType => _paymentType ??= new PaymentTypeRepository(_repoContext);
        public IPackageRepository Package => _package ??= new PackageRepository(_repoContext);
        public IPackageProductRepository PackageProduct => _packageProduct ??= new PackageProductRepository(_repoContext);
        public IOfferRepository Offer => _offer ??= new OfferRepository(_repoContext);
        public IOfferTypeRepository OfferType => _offerType ??= new OfferTypeRepository(_repoContext);
        public ICustomerOfferRepository CustomerOffer => _customerOffer ??= new CustomerOfferRepository(_repoContext);
        public ICustomerOrderRepository CustomerOrder => _customerOrder ??= new CustomerOrderRepository(_repoContext);
        public ICustomerOrderPaymentRepository CustomerOrderPayment => _customerOrderPayment ??= new CustomerOrderPaymentRepository(_repoContext);
        public IPostGetStateRepository PostGetState => _postGetState ??= new PostGetStateRepository(_repoContext);
        public IGetPostCityRepository PostCity => _postCity ??= new GetPostCityRepository(_repoContext);
        public ICustomerOrderProductRepository CustomerOrderProduct => _customerOrderProduct ??= new CustomerOrderProductRepository(_repoContext);
        public IProductPackingTypeImageRepository ProductPackingTypeImage => _productPackingTypeImage ??= new ProductPackingTypeImageRepository(_repoContext);
        public ICatDocumentRepositry CatDocument => _catDocument ??= new CatDocumentRepositry(_repoContext);
        public IDocumentRepository Document => _document ??= new DocumentRepository(_repoContext);
        public ISellerDocumentRepository SellerDocument => _sellerDocument ??= new SellerDocumentRepository(_repoContext);
        public RepositoryWrapper(BaseContext repositoryContext)
        {
            _repoContext = repositoryContext;

        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
