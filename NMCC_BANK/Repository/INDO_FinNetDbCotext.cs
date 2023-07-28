using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using INDO_FIN_NET.Repository.Data;

#nullable disable

namespace INDO_FIN_NET.Repository
{
    public partial class INDO_FinNetDbCotext : DbContext
    {

        public INDO_FinNetDbCotext()
        {
        }

        public INDO_FinNetDbCotext(DbContextOptions<INDO_FinNetDbCotext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdmAlphaService> AdmAlphaServices { get; set; }
        public virtual DbSet<AdmBankDetail> AdmBankDetails { get; set; }
        public virtual DbSet<AdmBranchtbl> AdmBranchtbls { get; set; }
        public virtual DbSet<AdmCategory> AdmCategories { get; set; }
        public virtual DbSet<AdmCustomerOcrDatum> AdmCustomerOcrData { get; set; }
        public virtual DbSet<AdmDepartmenttbl> AdmDepartmenttbls { get; set; }
        public virtual DbSet<AdmIndoErrorLog> AdmIndoErrorLogs { get; set; }
        public virtual DbSet<AdmOrganizationDetail> AdmOrganizationDetails { get; set; }
        public virtual DbSet<AdmOtpsave> AdmOtpsaves { get; set; }
        public virtual DbSet<AdmProductDetail> AdmProductDetails { get; set; }
        public virtual DbSet<AdmRegiontbl> AdmRegiontbls { get; set; }
        public virtual DbSet<AdmRoletbl> AdmRoletbls { get; set; }
        public virtual DbSet<AdmSelectedOrganizationByBank> AdmSelectedOrganizationByBanks { get; set; }
        public virtual DbSet<AdmSubProductDetail> AdmSubProductDetails { get; set; }
        public virtual DbSet<BankIfscDetail> BankIfscDetails { get; set; }
        public virtual DbSet<IndoBank1> IndoBank1s { get; set; }
        public virtual DbSet<IndoFinOtpMsgFormat> IndoFinOtpMsgFormats { get; set; }
        public virtual DbSet<IndoUserDetail> IndoUserDetails { get; set; }
        public virtual DbSet<OrganisationCategory> OrganisationCategories { get; set; }
        public virtual DbSet<OrganisationProduct> OrganisationProducts { get; set; }
        public virtual DbSet<OrganisationSubProduct> OrganisationSubProducts { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductSubProduct> ProductSubProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdmAlphaService>(entity =>
            {
                entity.HasKey(e => e.AlphaServiceId)
                    .HasName("PK__adm_Alph__FDCF069B99DF05D5");
            });

            modelBuilder.Entity<AdmBankDetail>(entity =>
            {
                entity.Property(e => e.BankBranch).IsUnicode(false);

                entity.Property(e => e.BankContactNumber).IsUnicode(false);

                entity.Property(e => e.BankHoAddress).IsUnicode(false);

                entity.Property(e => e.BankIfscnumber).IsUnicode(false);

                entity.Property(e => e.BankMicr).IsUnicode(false);

                entity.Property(e => e.BankName).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.District).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<AdmBranchtbl>(entity =>
            {
                entity.Property(e => e.BranchName).IsUnicode(false);

                entity.Property(e => e.RegionId).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<AdmCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__adm_Cate__19093A0B9FE902EE");
            });

            modelBuilder.Entity<AdmDepartmenttbl>(entity =>
            {
                entity.Property(e => e.DeptName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmIndoErrorLog>(entity =>
            {
                entity.Property(e => e.ControllerName).IsUnicode(false);

                entity.Property(e => e.MethodName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmOrganizationDetail>(entity =>
            {
                entity.Property(e => e.ContactPersonName).IsUnicode(false);

                entity.Property(e => e.OrgCaCertSerialNo).IsUnicode(false);

                entity.Property(e => e.OrganizationDescription).IsUnicode(false);

                entity.Property(e => e.OrganizationName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmOtpsave>(entity =>
            {
                entity.Property(e => e.Otp).IsUnicode(false);
            });

            modelBuilder.Entity<AdmProductDetail>(entity =>
            {
                entity.Property(e => e.ProductDescription).IsUnicode(false);

                entity.Property(e => e.ProductName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmRegiontbl>(entity =>
            {
                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.RegionName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmRoletbl>(entity =>
            {
                entity.Property(e => e.Roletype).IsUnicode(false);
            });

            modelBuilder.Entity<AdmSubProductDetail>(entity =>
            {
                entity.Property(e => e.SubProductDescription).IsUnicode(false);

                entity.Property(e => e.SubProductName).IsUnicode(false);
            });

            modelBuilder.Entity<BankIfscDetail>(entity =>
            {
                entity.HasKey(e => e.IfscId)
                    .HasName("PK__Bank_IFS__C4D0CD250AB4954F");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.BankName).IsUnicode(false);

                entity.Property(e => e.Branch).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.Contact).IsUnicode(false);

                entity.Property(e => e.District).IsUnicode(false);

                entity.Property(e => e.Ifsc).IsUnicode(false);

                entity.Property(e => e.Micr).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<IndoBank1>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Password).IsUnicode(false);
            });

            modelBuilder.Entity<IndoFinOtpMsgFormat>(entity =>
            {
                entity.Property(e => e.EndMsg).IsFixedLength(true);

                entity.Property(e => e.MsgId).ValueGeneratedOnAdd();

                entity.Property(e => e.OtpFor).IsUnicode(false);

                entity.Property(e => e.StartMsg).IsUnicode(false);
            });

            modelBuilder.Entity<IndoUserDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.SessionKey).IsUnicode(false);
            });

            modelBuilder.Entity<OrganisationCategory>(entity =>
            {
                entity.HasKey(e => e.OrgCatId)
                    .HasName("PK__Organisa__9104A82F17D4E6F3");
            });

            modelBuilder.Entity<OrganisationProduct>(entity =>
            {
                entity.HasKey(e => e.OrgProdId)
                    .HasName("PK__Organisa__315D19124B1E44E6");
            });

            modelBuilder.Entity<OrganisationSubProduct>(entity =>
            {
                entity.HasKey(e => e.OrgSubProdId)
                    .HasName("PK__Organisa__2ADCAB9F5BE00EE5");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CatProdId)
                    .HasName("PK__Product___E067987CEA77D0B5");
            });

            modelBuilder.Entity<ProductSubProduct>(entity =>
            {
                entity.HasKey(e => e.ProdSubProdId)
                    .HasName("PK__Product___E0C192095983161A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
