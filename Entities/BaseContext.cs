using System;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entities
{
    public partial class BaseContext : DbContext
    {
        public BaseContext()
        {
        }

        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Api> Api { get; set; }
        public virtual DbSet<CatApi> CatApi { get; set; }
        public virtual DbSet<CatFrom> CatFrom { get; set; }
        public virtual DbSet<CatProduct> CatProduct { get; set; }
        public virtual DbSet<CatProductLanguage> CatProductLanguage { get; set; }
        public virtual DbSet<CatProductParameters> CatProductParameters { get; set; }
        public virtual DbSet<CatRole> CatRole { get; set; }
        public virtual DbSet<CatStatus> CatStatus { get; set; }
        public virtual DbSet<Color> Color { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }
        public virtual DbSet<CustomerOffer> CustomerOffer { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<CustomerOrderPayment> CustomerOrderPayment { get; set; }
        public virtual DbSet<CustomerOrderProduct> CustomerOrderProduct { get; set; }
        public virtual DbSet<CustomerOrderProductStatusLog> CustomerOrderProductStatusLog { get; set; }
        public virtual DbSet<CustomerOrderStatusLog> CustomerOrderStatusLog { get; set; }
        public virtual DbSet<CustomerStatusLog> CustomerStatusLog { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<FamousComments> FamousComments { get; set; }
        public virtual DbSet<Forms> Forms { get; set; }
        public virtual DbSet<FormsApi> FormsApi { get; set; }
        public virtual DbSet<GetPostCity> GetPostCity { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<MobileAppType> MobileAppType { get; set; }
        public virtual DbSet<Offer> Offer { get; set; }
        public virtual DbSet<OfferType> OfferType { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<PackageImage> PackageImage { get; set; }
        public virtual DbSet<PackageProduct> PackageProduct { get; set; }
        public virtual DbSet<PackingType> PackingType { get; set; }
        public virtual DbSet<PackingTypeImage> PackingTypeImage { get; set; }
        public virtual DbSet<Parameters> Parameters { get; set; }
        public virtual DbSet<PaymentType> PaymentType { get; set; }
        public virtual DbSet<PaymentTypeLocation> PaymentTypeLocation { get; set; }
        public virtual DbSet<PostGetState> PostGetState { get; set; }
        public virtual DbSet<PostType> PostType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCatProductParameters> ProductCatProductParameters { get; set; }
        public virtual DbSet<ProductColor> ProductColor { get; set; }
        public virtual DbSet<ProductColorStatusLog> ProductColorStatusLog { get; set; }
        public virtual DbSet<ProductCustomerRate> ProductCustomerRate { get; set; }
        public virtual DbSet<ProductCustomerRateImage> ProductCustomerRateImage { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<ProductLanguage> ProductLanguage { get; set; }
        public virtual DbSet<ProductOffer> ProductOffer { get; set; }
        public virtual DbSet<ProductPackingType> ProductPackingType { get; set; }
        public virtual DbSet<ProductPackingTypeImage> ProductPackingTypeImage { get; set; }
        public virtual DbSet<ProductStatusLog> ProductStatusLog { get; set; }
        public virtual DbSet<RelatedProduct> RelatedProduct { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleForms> RoleForms { get; set; }
        public virtual DbSet<RoleFormsApi> RoleFormsApi { get; set; }
        public virtual DbSet<Seller> Seller { get; set; }
        public virtual DbSet<SellerAddress> SellerAddress { get; set; }
        public virtual DbSet<SellerStatusLog> SellerStatusLog { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<SliderPlace> SliderPlace { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<StatusType> StatusType { get; set; }
        public virtual DbSet<Systems> Systems { get; set; }
        public virtual DbSet<Tables> Tables { get; set; }
        public virtual DbSet<TablesServiceDiscovery> TablesServiceDiscovery { get; set; }
        public virtual DbSet<UserActivation> UserActivation { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Work> Work { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Api>(entity =>
            {
                entity.ToTable("API");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatApiid).HasColumnName("CatAPIID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Type).HasComment(@"1 = GET
2 = POST
3 = PUT
4 = Delete");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(512);

                entity.HasOne(d => d.CatApi)
                    .WithMany(p => p.Api)
                    .HasForeignKey(d => d.CatApiid)
                    .HasConstraintName("FK_API_CatAPI");
            });

            modelBuilder.Entity<CatApi>(entity =>
            {
                entity.ToTable("CatAPI");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.SystemsId).HasColumnName("SystemsID");

                entity.HasOne(d => d.Systems)
                    .WithMany(p => p.CatApi)
                    .HasForeignKey(d => d.SystemsId)
                    .HasConstraintName("FK_CatAPI_Systems");
            });

            modelBuilder.Entity<CatFrom>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Icon).HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Rorder).HasColumnName("ROrder");

                entity.Property(e => e.SystemsId).HasColumnName("SystemsID");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(512);

                entity.HasOne(d => d.Systems)
                    .WithMany(p => p.CatFrom)
                    .HasForeignKey(d => d.SystemsId)
                    .HasConstraintName("FK_CatFrom_Systems");
            });

            modelBuilder.Entity<CatProduct>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Icon).HasMaxLength(256);

                entity.Property(e => e.KeyWords).HasMaxLength(2048);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MetaDescription).HasMaxLength(2048);

                entity.Property(e => e.MetaTitle).HasMaxLength(512);

                entity.Property(e => e.MiniPicUrl)
                    .HasColumnName("MiniPicURL")
                    .HasMaxLength(256);

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(256);

                entity.HasOne(d => d.P)
                    .WithMany(p => p.InverseP)
                    .HasForeignKey(d => d.Pid)
                    .HasConstraintName("FK_CatProduct_CatProduct");
            });

            modelBuilder.Entity<CatProductLanguage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatProductId).HasColumnName("CatProductID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Icon).HasMaxLength(256);

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(256);

                entity.HasOne(d => d.CatProduct)
                    .WithMany(p => p.CatProductLanguage)
                    .HasForeignKey(d => d.CatProductId)
                    .HasConstraintName("FK_CatProductLanguage_CatProduct");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.CatProductLanguage)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_CatProductLanguage_Language");
            });

            modelBuilder.Entity<CatProductParameters>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatProductId).HasColumnName("CatProductID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ParametersId).HasColumnName("ParametersID");

                entity.HasOne(d => d.CatProduct)
                    .WithMany(p => p.CatProductParameters)
                    .HasForeignKey(d => d.CatProductId)
                    .HasConstraintName("FK_CatProductParameters_CatProduct");

                entity.HasOne(d => d.Parameters)
                    .WithMany(p => p.CatProductParameters)
                    .HasForeignKey(d => d.ParametersId)
                    .HasConstraintName("FK_CatProductParameters_Parameters");
            });

            modelBuilder.Entity<CatRole>(entity =>
            {
                entity.HasIndex(e => e.Rkey)
                    .HasName("IX_CatRole")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.SystemsId).HasColumnName("SystemsID");

                entity.HasOne(d => d.Systems)
                    .WithMany(p => p.CatRole)
                    .HasForeignKey(d => d.SystemsId)
                    .HasConstraintName("FK_CatRole_Systems");
            });

            modelBuilder.Entity<CatStatus>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.ColorCode).HasMaxLength(32);

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bdate).HasColumnName("BDate");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Email).HasMaxLength(64);

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Fname).HasMaxLength(32);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MobileAppTypeId).HasColumnName("MobileAppTypeID");

                entity.Property(e => e.MobileAppVersion).HasMaxLength(32);

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(32);

                entity.Property(e => e.PresenterCustomerId).HasColumnName("PresenterCustomerID");

                entity.Property(e => e.ProfileImageHurl)
                    .HasColumnName("ProfileImageHURL")
                    .HasMaxLength(514);

                entity.Property(e => e.ProfileImageUrl)
                    .HasColumnName("ProfileImageURL")
                    .HasMaxLength(512);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.WorkId).HasColumnName("WorkID");

                entity.HasOne(d => d.FinalStatus)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.FinalStatusId)
                    .HasConstraintName("FK_Customer_Status");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Customer_Location");

                entity.HasOne(d => d.MobileAppType)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.MobileAppTypeId)
                    .HasConstraintName("FK_Customer_MobileAppType");

                entity.HasOne(d => d.PresenterCustomer)
                    .WithMany(p => p.InversePresenterCustomer)
                    .HasForeignKey(d => d.PresenterCustomerId)
                    .HasConstraintName("FK_Customer_Customer");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Customer_Users");

                entity.HasOne(d => d.Work)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.WorkId)
                    .HasConstraintName("FK_Customer_Work");
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(1024);

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.IssureFamily).HasMaxLength(32);

                entity.Property(e => e.IssureName).HasMaxLength(32);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

                entity.Property(e => e.Titel).HasMaxLength(512);

                entity.Property(e => e.Xgps)
                    .HasColumnName("XGPS")
                    .HasMaxLength(128);

                entity.Property(e => e.Ygps)
                    .HasColumnName("YGPS")
                    .HasMaxLength(128);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CustomerAddressCity)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CustomerAddress_Location1");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAddress)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerAddress_Customer");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.CustomerAddressProvince)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_CustomerAddress_Location");
            });

            modelBuilder.Entity<CustomerOffer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.OfferCode).HasMaxLength(32);

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerOffer)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerOffer_Customer");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.CustomerOffer)
                    .HasForeignKey(d => d.OfferId)
                    .HasConstraintName("FK_CustomerOffer_Offer");
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdminDescription).HasMaxLength(2048);

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerAddressId).HasColumnName("CustomerAddressID");

                entity.Property(e => e.CustomerDescription).HasMaxLength(2048);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DeliveryDate).HasComment("فعلا خالی");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalPrice).HasComment("Order Price - Offer Price + Tax Price");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.OfferPrice).HasComment("order Price * Offer value");

                entity.Property(e => e.OfferValue).HasComment("اگر کد تخفیف داشت از جدول Offer پیدا میکنیم و مقدارش رو در اینجا ثبت می کنیم");

                entity.Property(e => e.OrderNo).HasComment("شماره سفارش ( کد مشتری + تاریخ +سه رقم سریال ) ");

                entity.Property(e => e.OrderPrice).HasComment("جمع کل تمامی قیمت محصولات موجود در سفارش ( قیمت با تخفیف )");

                entity.Property(e => e.OrderType).HasComment(@"1 = خرید
2 = سفارش");

                entity.Property(e => e.OrderWeight).HasComment("جمع وزن تمامی محصولات سفارش  ( تعداد * وزن محصول از جدول Product) ");

                entity.Property(e => e.PaymentTypeId)
                    .HasColumnName("PaymentTypeID")
                    .HasComment("نحوه پرداخت از یو آی خواهد آمد.");

                entity.Property(e => e.PostTrackingCode).HasMaxLength(512);

                entity.Property(e => e.PostTypeId)
                    .HasColumnName("PostTypeID")
                    .HasComment("نحوه ارسال پستی ( از یو آی خواهد آمد ) در نهایی سازی سفارش");

                entity.Property(e => e.SellerDescription).HasMaxLength(2048);

                entity.Property(e => e.SendDate).HasComment("فعلا خالی");

                entity.Property(e => e.TaxPrice).HasComment("(Order Price - Offer Price ) * 9%");

                entity.Property(e => e.TaxValue).HasComment("9% همیشه");

                entity.HasOne(d => d.CustomerAddress)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.CustomerAddressId)
                    .HasConstraintName("FK_CustomerOrder_CustomerAddress");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerOrder_Customer");

                entity.HasOne(d => d.FinalStatus)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.FinalStatusId)
                    .HasConstraintName("FK_CustomerOrder_Status");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .HasConstraintName("FK_CustomerOrder_PaymentType");

                entity.HasOne(d => d.PostType)
                    .WithMany(p => p.CustomerOrder)
                    .HasForeignKey(d => d.PostTypeId)
                    .HasConstraintName("FK_CustomerOrder_PostType");
            });

            modelBuilder.Entity<CustomerOrderPayment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CardPan)
                    .HasColumnName("card_pan")
                    .HasMaxLength(50);

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerOrderId).HasColumnName("CustomerOrderID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.OrderNo).HasMaxLength(128);

                entity.Property(e => e.RefNum).HasMaxLength(128);

                entity.Property(e => e.ResNum)
                    .HasMaxLength(128)
                    .HasComment("شماره یکتای سفارش ( از طرف سایت تولید می شود )");

                entity.Property(e => e.SystemTraceNo).HasComment("شماره مرجع");

                entity.Property(e => e.TerminalNo).HasComment("شماره ترمینال");

                entity.Property(e => e.TraceNo).HasMaxLength(128);

                entity.Property(e => e.TrackingCode)
                    .HasMaxLength(128)
                    .HasComment("کد رهگیری");

                entity.Property(e => e.TransactionDate).HasComment("تاریخ تراکنش");

                entity.Property(e => e.TransactionPrice).HasComment("مبلغ تراکنش");

                entity.HasOne(d => d.CustomerOrder)
                    .WithMany(p => p.CustomerOrderPayment)
                    .HasForeignKey(d => d.CustomerOrderId)
                    .HasConstraintName("FK_CustomerOrderPayment_CustomerOrder");

                entity.HasOne(d => d.FinalStatus)
                    .WithMany(p => p.CustomerOrderPayment)
                    .HasForeignKey(d => d.FinalStatusId)
                    .HasConstraintName("FK_CustomerOrderPayment_Status");
            });

            modelBuilder.Entity<CustomerOrderProduct>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdminDescription).HasMaxLength(2048);

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerDescription).HasMaxLength(2048);

                entity.Property(e => e.CustomerOrderId).HasColumnName("CustomerOrderID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.OrderCount).HasComment("از یو آی");

                entity.Property(e => e.OrderType).HasComment(@"1 = order
2 = Produce");

                entity.Property(e => e.PackingTypeId).HasColumnName("PackingTypeID");

                entity.Property(e => e.ProductCode).HasComment("از جدول محصولات");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(256);

                entity.Property(e => e.ProductOfferCode)
                    .HasMaxLength(32)
                    .HasComment("از جدول ProductOffer");

                entity.Property(e => e.ProductOfferId).HasColumnName("ProductOfferID");

                entity.Property(e => e.ProductOfferPrice).HasComment("محاسبه خواهد شد.");

                entity.Property(e => e.ProductOfferValue).HasComment("از جدول ProductOffer");

                entity.Property(e => e.ProductPrice).HasComment("از جدول Product");

                entity.Property(e => e.SellerDescription).HasMaxLength(2048);

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.Weight).HasComment("از جدول محصولات ( فقط وزن یک محصول نه تمامی آنها )");

                entity.HasOne(d => d.CustomerOrder)
                    .WithMany(p => p.CustomerOrderProduct)
                    .HasForeignKey(d => d.CustomerOrderId)
                    .HasConstraintName("FK_CustomerOrderProduct_CustomerOrder");

                entity.HasOne(d => d.FinalStatus)
                    .WithMany(p => p.CustomerOrderProduct)
                    .HasForeignKey(d => d.FinalStatusId)
                    .HasConstraintName("FK_CustomerOrderProduct_Status");

                entity.HasOne(d => d.PackingType)
                    .WithMany(p => p.CustomerOrderProduct)
                    .HasForeignKey(d => d.PackingTypeId)
                    .HasConstraintName("FK_CustomerOrderProduct_PackingType");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CustomerOrderProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CustomerOrderProduct_Product");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.CustomerOrderProduct)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK_CustomerOrderProduct_Seller");
            });

            modelBuilder.Entity<CustomerOrderProductStatusLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerOrderProductId).HasColumnName("CustomerOrderProductID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.CustomerOrderProduct)
                    .WithMany(p => p.CustomerOrderProductStatusLog)
                    .HasForeignKey(d => d.CustomerOrderProductId)
                    .HasConstraintName("FK_CustomerOrderProductStatusLog_CustomerOrderProduct");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.CustomerOrderProductStatusLog)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_CustomerOrderProductStatusLog_Status");
            });

            modelBuilder.Entity<CustomerOrderStatusLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerOrderId).HasColumnName("CustomerOrderID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.CustomerOrder)
                    .WithMany(p => p.CustomerOrderStatusLog)
                    .HasForeignKey(d => d.CustomerOrderId)
                    .HasConstraintName("FK_CustomerOrderStatusLog_CustomerOrder");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.CustomerOrderStatusLog)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_CustomerOrderStatusLog_Status");
            });

            modelBuilder.Entity<CustomerStatusLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerStatusLog)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerStatusLog_Customer");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.CustomerStatusLog)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_CustomerStatusLog_Status");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Employee_Users");
            });

            modelBuilder.Entity<FamousComments>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.Comment).HasMaxLength(2048);

                entity.Property(e => e.CommentPic).HasMaxLength(512);

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Post).HasMaxLength(64);

                entity.Property(e => e.ProfilePic).HasMaxLength(512);
            });

            modelBuilder.Entity<Forms>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatFromsId).HasColumnName("CatFromsID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Icon).HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Rorder).HasColumnName("ROrder");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(512);

                entity.HasOne(d => d.CatFroms)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.CatFromsId)
                    .HasConstraintName("FK_Forms_CatFrom");
            });

            modelBuilder.Entity<FormsApi>(entity =>
            {
                entity.ToTable("FormsAPI");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apiid).HasColumnName("APIID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.FormsId).HasColumnName("FormsID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.HasOne(d => d.Api)
                    .WithMany(p => p.FormsApi)
                    .HasForeignKey(d => d.Apiid)
                    .HasConstraintName("FK_FormsAPI_API");

                entity.HasOne(d => d.Forms)
                    .WithMany(p => p.FormsApi)
                    .HasForeignKey(d => d.FormsId)
                    .HasConstraintName("FK_FormsAPI_Forms");
            });

            modelBuilder.Entity<GetPostCity>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(512);

                entity.Property(e => e.StateId).HasColumnName("StateID");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.FlagIcon).HasMaxLength(256);

                entity.Property(e => e.IsRtl).HasColumnName("IsRTL");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.ShortName).HasMaxLength(16);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.EnName).HasMaxLength(64);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.InverseCountry)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Location_Location1");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.InverseP)
                    .HasForeignKey(d => d.Pid)
                    .HasConstraintName("FK_Location_Location");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.InverseProvince)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_Location_Location2");
            });

            modelBuilder.Entity<MobileAppType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(512);

                entity.Property(e => e.OfferCode).HasMaxLength(32);

                entity.Property(e => e.OfferTypeId).HasColumnName("OfferTypeID");

                entity.HasOne(d => d.OfferType)
                    .WithMany(p => p.Offer)
                    .HasForeignKey(d => d.OfferTypeId)
                    .HasConstraintName("FK_Offer_OfferType");
            });

            modelBuilder.Entity<OfferType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(512);

                entity.Property(e => e.PackageImageUrl)
                    .HasColumnName("PackageImageURL")
                    .HasMaxLength(512);
            });

            modelBuilder.Entity<PackageImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FileType).HasComment(@"1 = Image
2 = Film");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.PackageImage)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_PackageImage_Package");
            });

            modelBuilder.Entity<PackageProduct>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.PackageProduct)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK_PackageProduct_Package");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PackageProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_PackageProduct_Product");
            });

            modelBuilder.Entity<PackingType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<PackingTypeImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Decription).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.ImageFileUrl)
                    .HasColumnName("ImageFileURL")
                    .HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.PackingTypeId).HasColumnName("PackingTypeID");

                entity.Property(e => e.Title).HasMaxLength(256);

                entity.HasOne(d => d.PackingType)
                    .WithMany(p => p.PackingTypeImage)
                    .HasForeignKey(d => d.PackingTypeId)
                    .HasConstraintName("FK_PackingTypeImage_PackingType");
            });

            modelBuilder.Entity<Parameters>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(512);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.InverseP)
                    .HasForeignKey(d => d.Pid)
                    .HasConstraintName("FK_Parameters_Parameters");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Icon).HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentTypeLocation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.PaymentTypeLocation)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_PaymentTypeLocation_Location");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.PaymentTypeLocation)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .HasConstraintName("FK_PaymentTypeLocation_PaymentType");
            });

            modelBuilder.Entity<PostGetState>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(512);
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApiUrl)
                    .HasColumnName("ApiURL")
                    .HasMaxLength(512);

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.Duration).HasMaxLength(16);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Icon).HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Title).HasMaxLength(128);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AparatUrl)
                    .HasColumnName("AparatURL")
                    .HasMaxLength(512);

                entity.Property(e => e.CatProductId).HasColumnName("CatProductID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CoverImageHurl)
                    .HasColumnName("CoverImageHURL")
                    .HasMaxLength(514);

                entity.Property(e => e.CoverImageUrl)
                    .HasColumnName("CoverImageURL")
                    .HasMaxLength(512);

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.EnName).HasMaxLength(256);

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.KeyWords).HasMaxLength(2048);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.ProductMeterId).HasColumnName("ProductMeterID");

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.HasOne(d => d.CatProduct)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CatProductId)
                    .HasConstraintName("FK_Product_CatProduct");

                entity.HasOne(d => d.FinalStatus)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.FinalStatusId)
                    .HasConstraintName("FK_Product_Status");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK_Product_Seller");
            });

            modelBuilder.Entity<ProductCatProductParameters>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatProductParametersId).HasColumnName("CatProductParametersID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Value).HasMaxLength(1024);

                entity.HasOne(d => d.CatProductParameters)
                    .WithMany(p => p.ProductCatProductParameters)
                    .HasForeignKey(d => d.CatProductParametersId)
                    .HasConstraintName("FK_ProductCatProductParameters_CatProductParameters");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCatProductParameters)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductCatProductParameters_Product");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.ProductColor)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_ProductColor_Color");

                entity.HasOne(d => d.FinalStatus)
                    .WithMany(p => p.ProductColor)
                    .HasForeignKey(d => d.FinalStatusId)
                    .HasConstraintName("FK_ProductColor_Status");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductColor)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductColor_Product");
            });

            modelBuilder.Entity<ProductColorStatusLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProductColorId).HasColumnName("ProductColorID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.ProductColor)
                    .WithMany(p => p.ProductColorStatusLog)
                    .HasForeignKey(d => d.ProductColorId)
                    .HasConstraintName("FK_ProductColorStatusLog_ProductColor");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProductColorStatusLog)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_ProductColorStatusLog_Status");
            });

            modelBuilder.Entity<ProductCustomerRate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CommentDesc).HasMaxLength(2048);

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ProductCustomerRate)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ProductCustomerRate_Customer");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.InverseP)
                    .HasForeignKey(d => d.Pid)
                    .HasConstraintName("FK_ProductCustomerRate_ProductCustomerRate");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCustomerRate)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductCustomerRate_Product");
            });

            modelBuilder.Entity<ProductCustomerRateImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FileType).HasComment(@"1 = Image
2 = Film");

                entity.Property(e => e.FileUrl)
                    .HasColumnName("FileURL")
                    .HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProductCustomerRateId).HasColumnName("ProductCustomerRateID");

                entity.HasOne(d => d.ProductCustomerRate)
                    .WithMany(p => p.ProductCustomerRateImage)
                    .HasForeignKey(d => d.ProductCustomerRateId)
                    .HasConstraintName("FK_ProductCustomerRateImage_ProductCustomerRate");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FileType).HasComment(@"1=Image
2=Film");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Title).HasMaxLength(512);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImage)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductImage_Product");
            });

            modelBuilder.Entity<ProductLanguage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AparatUrl)
                    .HasColumnName("AparatURL")
                    .HasMaxLength(512);

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CoverImageHurl)
                    .HasColumnName("CoverImageHURL")
                    .HasMaxLength(514);

                entity.Property(e => e.CoverImageUrl)
                    .HasColumnName("CoverImageURL")
                    .HasMaxLength(512);

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.EnName).HasMaxLength(256);

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ProductLanguage)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_ProductLanguage_Language");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductLanguage)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductLanguage_Product");
            });

            modelBuilder.Entity<ProductOffer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.ProductOffer)
                    .HasForeignKey(d => d.OfferId)
                    .HasConstraintName("FK_ProductOffer_Offer");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOffer)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductOffer_Product");
            });

            modelBuilder.Entity<ProductPackingType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.PackinggTypeId).HasColumnName("PackinggTypeID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.PackinggType)
                    .WithMany(p => p.ProductPackingType)
                    .HasForeignKey(d => d.PackinggTypeId)
                    .HasConstraintName("FK_ProductPackingType_PackingType");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPackingType)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductPackingType_Product");
            });

            modelBuilder.Entity<ProductPackingTypeImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Decription).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.ImageFileUrl)
                    .HasColumnName("ImageFileURL")
                    .HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProductPackingTypeId).HasColumnName("ProductPackingTypeID");

                entity.Property(e => e.Title).HasMaxLength(256);

                entity.HasOne(d => d.ProductPackingType)
                    .WithMany(p => p.ProductPackingTypeImage)
                    .HasForeignKey(d => d.ProductPackingTypeId)
                    .HasConstraintName("FK_ProductPackingTypeImage_ProductPackingType");
            });

            modelBuilder.Entity<ProductStatusLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductStatusLog)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductStatusLog_Product");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProductStatusLog)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_ProductStatusLog_Status");
            });

            modelBuilder.Entity<RelatedProduct>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DestinProductId).HasColumnName("DestinProductID");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.OriginProductId).HasColumnName("OriginProductID");

                entity.HasOne(d => d.DestinProduct)
                    .WithMany(p => p.RelatedProductDestinProduct)
                    .HasForeignKey(d => d.DestinProductId)
                    .HasConstraintName("FK_RelatedProduct_Product1");

                entity.HasOne(d => d.OriginProduct)
                    .WithMany(p => p.RelatedProductOriginProduct)
                    .HasForeignKey(d => d.OriginProductId)
                    .HasConstraintName("FK_RelatedProduct_Product");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatRoleId).HasColumnName("CatRoleID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.HasOne(d => d.CatRole)
                    .WithMany(p => p.Role)
                    .HasForeignKey(d => d.CatRoleId)
                    .HasConstraintName("FK_Role_CatRole");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.InverseP)
                    .HasForeignKey(d => d.Pid)
                    .HasConstraintName("FK_Role_Role");
            });

            modelBuilder.Entity<RoleForms>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FormsId).HasColumnName("FormsID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Forms)
                    .WithMany(p => p.RoleForms)
                    .HasForeignKey(d => d.FormsId)
                    .HasConstraintName("FK_RoleForms_Forms");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleForms)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleForms_Role");
            });

            modelBuilder.Entity<RoleFormsApi>(entity =>
            {
                entity.ToTable("RoleFormsAPI");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.FormsApiid).HasColumnName("FormsAPIID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.RoleFormsId).HasColumnName("RoleFormsID");

                entity.HasOne(d => d.FormsApi)
                    .WithMany(p => p.RoleFormsApi)
                    .HasForeignKey(d => d.FormsApiid)
                    .HasConstraintName("FK_RoleFormsAPI_FormsAPI");

                entity.HasOne(d => d.RoleForms)
                    .WithMany(p => p.RoleFormsApi)
                    .HasForeignKey(d => d.RoleFormsId)
                    .HasConstraintName("FK_RoleFormsAPI_RoleForms");
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bdate).HasColumnName("BDate");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Email).HasMaxLength(64);

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Fname).HasMaxLength(32);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MobileAppTypeId).HasColumnName("MobileAppTypeID");

                entity.Property(e => e.MobileAppVersion).HasMaxLength(32);

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(32);

                entity.Property(e => e.ProfileImageHurl)
                    .HasColumnName("ProfileImageHURL")
                    .HasMaxLength(514);

                entity.Property(e => e.ProfileImageUrl)
                    .HasColumnName("ProfileImageURL")
                    .HasMaxLength(512);

                entity.Property(e => e.RealOrLegal).HasComment(@"1 = hagigi
2 = hogogi");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.FinalStatus)
                    .WithMany(p => p.Seller)
                    .HasForeignKey(d => d.FinalStatusId)
                    .HasConstraintName("FK_Seller_Status");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Seller)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Seller_Location");

                entity.HasOne(d => d.MobileAppType)
                    .WithMany(p => p.Seller)
                    .HasForeignKey(d => d.MobileAppTypeId)
                    .HasConstraintName("FK_Seller_MobileAppType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Seller)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Seller_Users");
            });

            modelBuilder.Entity<SellerAddress>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(1024);

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.Titel).HasMaxLength(512);

                entity.Property(e => e.Xgps)
                    .HasColumnName("XGPS")
                    .HasMaxLength(128);

                entity.Property(e => e.Ygps)
                    .HasColumnName("YGPS")
                    .HasMaxLength(128);

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SellerAddress)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK_SellerAddress_Seller");
            });

            modelBuilder.Entity<SellerStatusLog>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SellerStatusLog)
                    .HasForeignKey(d => d.SellerId)
                    .HasConstraintName("FK_SellerStatusLog_Seller");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SellerStatusLog)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_SellerStatusLog_Status");
            });

            modelBuilder.Entity<Slider>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.ImageHurl)
                    .HasColumnName("ImageHURL")
                    .HasMaxLength(514);

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(512);

                entity.Property(e => e.LinkUrl)
                    .HasColumnName("LinkURL")
                    .HasMaxLength(256);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Rorder).HasColumnName("ROrder");

                entity.Property(e => e.SliderPlaceId).HasColumnName("SliderPlaceID");

                entity.Property(e => e.Title).HasMaxLength(512);

                entity.HasOne(d => d.SliderPlace)
                    .WithMany(p => p.Slider)
                    .HasForeignKey(d => d.SliderPlaceId)
                    .HasConstraintName("FK_Slider_SliderPlace");
            });

            modelBuilder.Entity<SliderPlace>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatStatusId).HasColumnName("CatStatusID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.Color).HasMaxLength(7);

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NextStatusId).HasColumnName("NextStatusID");

                entity.Property(e => e.StatusTypeId).HasColumnName("StatusTypeID");

                entity.HasOne(d => d.CatStatus)
                    .WithMany(p => p.Status)
                    .HasForeignKey(d => d.CatStatusId)
                    .HasConstraintName("FK_Status_CatStatus");

                entity.HasOne(d => d.NextStatus)
                    .WithMany(p => p.InverseNextStatus)
                    .HasForeignKey(d => d.NextStatusId)
                    .HasConstraintName("FK_Status_Status");

                entity.HasOne(d => d.StatusType)
                    .WithMany(p => p.Status)
                    .HasForeignKey(d => d.StatusTypeId)
                    .HasConstraintName("FK_Status_StatusType");
            });

            modelBuilder.Entity<StatusType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<Systems>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Icon).HasMaxLength(512);

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(32);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Rorder).HasColumnName("ROrder");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(512);
            });

            modelBuilder.Entity<Tables>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CatStatusId).HasColumnName("CatStatusID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.SystemsId).HasColumnName("SystemsID");

                entity.HasOne(d => d.CatStatus)
                    .WithMany(p => p.Tables)
                    .HasForeignKey(d => d.CatStatusId)
                    .HasConstraintName("FK_Tables_CatStatus");

                entity.HasOne(d => d.Systems)
                    .WithMany(p => p.Tables)
                    .HasForeignKey(d => d.SystemsId)
                    .HasConstraintName("FK_Tables_Systems");
            });

            modelBuilder.Entity<TablesServiceDiscovery>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.HashPass).HasMaxLength(514);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.ServiceType).HasComment(@"1 = Insert 
2 = Update
3 = Delete");

                entity.Property(e => e.SystemsId).HasColumnName("SystemsID");

                entity.Property(e => e.TablesId).HasColumnName("TablesID");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(512);

                entity.HasOne(d => d.Systems)
                    .WithMany(p => p.TablesServiceDiscovery)
                    .HasForeignKey(d => d.SystemsId)
                    .HasConstraintName("FK_TablesServiceDiscovery_Systems");

                entity.HasOne(d => d.Tables)
                    .WithMany(p => p.TablesServiceDiscovery)
                    .HasForeignKey(d => d.TablesId)
                    .HasConstraintName("FK_TablesServiceDiscovery_Tables");
            });

            modelBuilder.Entity<UserActivation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserActivation)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserActivation_Users");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRole_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FinalStatusId).HasColumnName("FinalStatusID");

                entity.Property(e => e.FullName).HasMaxLength(256);

                entity.Property(e => e.Hpassword)
                    .HasColumnName("HPassword")
                    .HasMaxLength(512);

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Username).HasMaxLength(64);
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cdate).HasColumnName("CDate");

                entity.Property(e => e.CuserId).HasColumnName("CUserID");

                entity.Property(e => e.DaUserId).HasColumnName("DaUserID");

                entity.Property(e => e.Ddate).HasColumnName("DDate");

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.DuserId).HasColumnName("DUserID");

                entity.Property(e => e.Mdate).HasColumnName("MDate");

                entity.Property(e => e.MuserId).HasColumnName("MUserID");

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
