using HRMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data
{
    public partial class HRDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public HRDbContext(DbContextOptions<HRDbContext> options)
            : base(options)
        {
        }

        #region F_HR
        public virtual DbSet<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
        public virtual DbSet<F_BAS_HRD_DEPARTMENT> F_BAS_HRD_DEPARTMENT { get; set; }
        public virtual DbSet<F_BAS_HRD_DESIGNATION> F_BAS_HRD_DESIGNATION { get; set; }
        public virtual DbSet<F_BAS_HRD_SECTION> F_BAS_HRD_SECTION { get; set; }
        public virtual DbSet<F_BAS_HRD_SUB_SECTION> F_BAS_HRD_SUB_SECTION { get; set; }
        public virtual DbSet<F_BAS_HRD_GRADE> F_BAS_HRD_GRADE { get; set; }
        public virtual DbSet<F_BAS_HRD_LOCATION> F_BAS_HRD_LOCATION { get; set; }
        public virtual DbSet<F_BAS_HRD_ATTEN_TYPE> F_BAS_HRD_ATTEN_TYPE { get; set; }
        public virtual DbSet<F_BAS_HRD_BLOOD_GROUP> F_BAS_HRD_BLOOD_GROUP { get; set; }
        public virtual DbSet<F_BAS_HRD_DISTRICT> F_BAS_HRD_DISTRICT { get; set; }
        public virtual DbSet<F_BAS_HRD_DIVISION> F_BAS_HRD_DIVISION { get; set; }
        public virtual DbSet<F_BAS_HRD_EMP_RELATION> F_BAS_HRD_EMP_RELATION { get; set; }
        public virtual DbSet<F_BAS_HRD_EMP_TYPE> F_BAS_HRD_EMP_TYPE { get; set; }
        public virtual DbSet<F_BAS_HRD_NATIONALITY> F_BAS_HRD_NATIONALITY { get; set; }
        public virtual DbSet<F_BAS_HRD_OUT_REASON> F_BAS_HRD_OUT_REASON { get; set; }
        public virtual DbSet<F_BAS_HRD_RELIGION> F_BAS_HRD_RELIGION { get; set; }
        public virtual DbSet<F_BAS_HRD_SHIFT> F_BAS_HRD_SHIFT { get; set; }
        public virtual DbSet<F_BAS_HRD_THANA> F_BAS_HRD_THANA { get; set; }
        public virtual DbSet<F_BAS_HRD_UNION> F_BAS_HRD_UNION { get; set; }
        public virtual DbSet<F_BAS_HRD_WEEKEND> F_BAS_HRD_WEEKEND { get; set; }
        public virtual DbSet<F_HRD_EDUCATION> F_HRD_EDUCATION { get; set; }
        public virtual DbSet<F_HRD_EMERGENCY> F_HRD_EMERGENCY { get; set; }
        public virtual DbSet<F_HRD_EMP_CHILDREN> F_HRD_EMP_CHILDREN { get; set; }
        public virtual DbSet<F_HRD_EMP_EDU_DEGREE> F_HRD_EMP_EDU_DEGREE { get; set; }
        public virtual DbSet<F_HRD_EMP_SPOUSE> F_HRD_EMP_SPOUSE { get; set; }
        public virtual DbSet<F_HRD_EXPERIENCE> F_HRD_EXPERIENCE { get; set; }
        public virtual DbSet<F_HRD_INCREMENT> F_HRD_INCREMENT { get; set; }
        public virtual DbSet<F_HRD_PROMOTION> F_HRD_PROMOTION { get; set; }
        public virtual DbSet<F_HRD_SKILL> F_HRD_SKILL { get; set; }
        public virtual DbSet<F_HRD_SKILL_LEVEL> F_HRD_SKILL_LEVEL { get; set; }
        public virtual DbSet<BAS_GENDER> BAS_GENDER { get; set; }
        #endregion
        #region KEEP THIS COMMENTED
        //public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        //public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        #endregion
        public virtual DbSet<AspNetUserTypes> AspNetUserTypes { get; set; }
        public virtual DbSet<BAS_BEN_BANK_MASTER> BAS_BEN_BANK_MASTER { get; set; }
        public virtual DbSet<MESSAGE> MESSAGE { get; set; }
        public virtual DbSet<MESSAGE_INDIVIDUAL> MESSAGE_INDIVIDUAL { get; set; }
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
        public virtual DbSet<MenuMasterRoles> MenuMasterRoles { get; set; }
        public virtual DbSet<COMPANY_INFO> COMPANY_INFO { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<CURRENCY> CURRENCY { get; set; }
        
        //MAIL BOX
        public virtual DbSet<MAILBOX> MAILBOX { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AspNetUserTypes>(entity =>
            {
                entity.HasKey(e => e.TYPEID);
            });

            modelBuilder.Entity<BAS_BEN_BANK_MASTER>(entity =>
            {
                entity.HasKey(e => e.BANKID);

                entity.Property(e => e.ADDRESS).HasMaxLength(100);

                entity.Property(e => e.BEN_BANK)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BRANCH).HasMaxLength(100);

                entity.Property(e => e.REMARKS).HasMaxLength(50);
            });

            modelBuilder.Entity<MESSAGE>(entity =>
            {
                entity.Property(e => e.ReceiverId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.ReceiverName).IsRequired();
                entity.Property(e => e.SendAt).HasColumnType("datetime");
                entity.Property(e => e.SenderId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.Text).IsRequired();
                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_AspNetUsers");
            });

            modelBuilder.Entity<MESSAGE_INDIVIDUAL>(entity =>
            {
                entity.Property(e => e.ReceiverId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.ReceiverName).IsRequired();
                entity.Property(e => e.SendAt).HasColumnType("datetime");
                entity.Property(e => e.SenderId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.Text).IsRequired();
            });

            modelBuilder.Entity<PDL_EMAIL_SENDER>(entity =>
            {
                entity.Property(e => e.FROM_EMAIL)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NETWORK_CREDENTIAL_PASSWORD).IsRequired();

                entity.Property(e => e.NETWORK_CREDENTIAL_USERNAME)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SMTP_CLIENT)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.HasKey(e => new { e.MenuIdentity, e.MenuID, e.MenuName });

                entity.Property(e => e.MenuIdentity).ValueGeneratedOnAdd();

                entity.Property(e => e.MenuID)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MenuFileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuURL)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuIcon)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Parent_MenuID)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.USE_YN).HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<MenuMasterRoles>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<COMPANY_INFO>(entity =>
            {
                entity.Property(e => e.BIN_NO)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.COMPANY_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION).IsUnicode(false);

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ETIN_NO)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FACTORY_ADDRESS).IsUnicode(false);

                entity.Property(e => e.HEADOFFICE_ADDRESS).IsUnicode(false);

                entity.Property(e => e.PHONE1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE3)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TAGLINE).IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WEB_ADDRESS).IsUnicode(false);
            });

            modelBuilder.Entity<CURRENCY>(entity =>
            {
                entity.Property(e => e.CODE).HasMaxLength(50);

                entity.Property(e => e.NAME).HasMaxLength(50);

                entity.Property(e => e.SYMBOL).HasMaxLength(50);
            });

            modelBuilder.Entity<MAILBOX>(entity =>
            {
                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(450);

                entity.Property(e => e.IP_PHONE).HasMaxLength(50);

                entity.Property(e => e.MAIL_ATTACHMENT).HasMaxLength(50);

                entity.Property(e => e.MAIL_FROM).HasMaxLength(50);

                entity.Property(e => e.MOBILE_NO).HasMaxLength(50);

                entity.Property(e => e.OPT1).HasMaxLength(50);

                entity.Property(e => e.OPT2).HasMaxLength(50);

                entity.Property(e => e.OPT3).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_DEPARTMENT>(entity =>
            {
                entity.HasKey(e => e.DEPTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DEPTNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DEPTNAME_BN).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.LOCATION)
                    .WithMany(p => p.F_BAS_HRD_DEPARTMENT)
                    .HasForeignKey(d => d.LOCATIONID)
                    .HasConstraintName("FK_F_BAS_HRD_DEPARTMENT_F_BAS_HRD_LOCATION");
            });

            modelBuilder.Entity<F_BAS_HRD_DESIGNATION>(entity =>
            {
                entity.HasKey(e => e.DESID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DES_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DES_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.GRADE)
                    .WithMany(p => p.F_BAS_HRD_DESIGNATION)
                    .HasForeignKey(d => d.GRADEID)
                    .HasConstraintName("FK_F_BAS_HRD_DESIGNATION_F_BAS_HRD_GRADE");
            });

            modelBuilder.Entity<F_BAS_HRD_SECTION>(entity =>
            {
                entity.HasKey(e => e.SECID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SEC_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SEC_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_SUB_SECTION>(entity =>
            {
                entity.HasKey(e => e.SUBSECID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SUBSEC_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SUBSEC_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.SUBSEC_SNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SUBSEC_SNAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_GRADE>(entity =>
            {
                entity.HasKey(e => e.GRADEID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GRADE_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_LOCATION>(entity =>
            {
                entity.HasKey(e => e.LOCID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LOC_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_ATTEN_TYPE>(entity =>
            {
                entity.HasKey(e => e.ATTYPID);

                entity.Property(e => e.ATTYPID_DESC).IsUnicode(false);

                entity.Property(e => e.ATTYPID_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_BLOOD_GROUP>(entity =>
            {
                entity.HasKey(e => e.BLDGRPID);

                entity.Property(e => e.BLDGRP_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_DISTRICT>(entity =>
            {
                entity.HasKey(e => e.DISTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DIST_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DIST_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEBSITE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DIV)
                    .WithMany(p => p.F_BAS_HRD_DISTRICT)
                    .HasForeignKey(d => d.DIVID)
                    .HasConstraintName("FK_F_BAS_HRD_DISTRICT_F_BAS_HRD_DIVISION");
            });

            modelBuilder.Entity<F_BAS_HRD_DIVISION>(entity =>
            {
                entity.HasKey(e => e.DIVID);

                entity.Property(e => e.DIVID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DIV_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DIV_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEBSITE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.COUNTRY)
                    .WithMany(p => p.F_BAS_HRD_DIVISION)
                    .HasForeignKey(d => d.COUNTRYID)
                    .HasConstraintName("FK_F_BAS_HRD_DIVISION_F_BAS_HRD_NATIONALITY");
            });

            modelBuilder.Entity<F_BAS_HRD_EMP_RELATION>(entity =>
            {
                entity.HasKey(e => e.RELATIONID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_EMP_TYPE>(entity =>
            {
                entity.HasKey(e => e.TYPEID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TYPE_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_NATIONALITY>(entity =>
            {
                entity.HasKey(e => e.NATIONID);

                entity.Property(e => e.COUNTRY_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.COUNTRY_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);
                
                entity.Property(e => e.NATION_DESC)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NATION_DESC_BN).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.CUR)
                    .WithMany(p => p.F_BAS_HRD_NATIONALITY)
                    .HasForeignKey(d => d.CURRENCYID)
                    .HasConstraintName("FK_F_BAS_HRD_NATIONALITY_CURRENCY");
            });

            modelBuilder.Entity<F_BAS_HRD_OUT_REASON>(entity =>
            {
                entity.HasKey(e => e.RESASON_ID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RESASON_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_RELIGION>(entity =>
            {
                entity.HasKey(e => e.RELIGIONID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RELEGION_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RELEGION_NAME_BNG).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_BAS_HRD_SHIFT>(entity =>
            {
                entity.HasKey(e => e.SHIFTID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHIFT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SHORT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_BAS_HRD_THANA>(entity =>
            {
                entity.HasKey(e => e.THANAID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.THANA_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.THANA_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.WEBSITE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DIST)
                    .WithMany(p => p.F_BAS_HRD_THANA)
                    .HasForeignKey(d => d.DISTID)
                    .HasConstraintName("FK_F_BAS_HRD_THANA_F_BAS_HRD_DISTRICT");
            });

            modelBuilder.Entity<F_BAS_HRD_UNION>(entity =>
            {
                entity.HasKey(e => e.UNIONID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.THANA)
                    .WithMany(p => p.F_BAS_HRD_UNION)
                    .HasForeignKey(d => d.THANAID)
                    .HasConstraintName("FK_F_BAS_HRD_UNION_F_BAS_HRD_THANA");
            });

            modelBuilder.Entity<F_BAS_HRD_WEEKEND>(entity =>
            {
                entity.HasKey(e => e.ODID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OD_FULL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OD_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_HRD_EDUCATION>(entity =>
            {
                entity.HasKey(e => e.EDUID);

                entity.Property(e => e.BOARD_UNI)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CGPA)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MAJOR)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OUTOF)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PASS_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DEG)
                    .WithMany(p => p.F_HRD_EDUCATION)
                    .HasForeignKey(d => d.DEGID)
                    .HasConstraintName("FK_F_HRD_EDUCATION_F_HRD_EMP_EDU_DEGREE");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EDUCATION)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EDUCATION_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_EMERGENCY>(entity =>
            {
                entity.HasKey(e => e.CONTID);

                entity.Property(e => e.ADDRESS).IsUnicode(false);

                entity.Property(e => e.CONT_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EMERGENCY)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EMERGENCY_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.RELATION)
                    .WithMany(p => p.F_HRD_EMERGENCY)
                    .HasForeignKey(d => d.RELATIONID)
                    .HasConstraintName("FK_F_HRD_EMERGENCY_F_BAS_HRD_EMP_RELATION");
            });

            modelBuilder.Entity<F_HRD_EMP_CHILDREN>(entity =>
            {
                entity.HasKey(e => e.CHID);

                entity.Property(e => e.CH_BID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_DOB).HasColumnType("datetime");

                entity.Property(e => e.CH_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.CH_NID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_PASSPORT)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CH_PROFESSION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EMP_CHILDREN)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EMP_CHILDREN_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_EMP_EDU_DEGREE>(entity =>
            {
                entity.HasKey(e => e.DEGID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DEGNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);
            });

            modelBuilder.Entity<F_HRD_EMP_SPOUSE>(entity =>
            {
                entity.HasKey(e => e.SPID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SPNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SPNAME_BN).HasMaxLength(50);

                entity.Property(e => e.SP_BID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SP_DOB).HasColumnType("datetime");

                entity.Property(e => e.SP_NID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SP_PASSPORT)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SP_PROFESSION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EMP_SPOUSE)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EMP_SPOUSE_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_EXPERIENCE>(entity =>
            {
                entity.HasKey(e => e.EXPID);

                entity.Property(e => e.COMPANY)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DESCRIPTION).IsUnicode(false);

                entity.Property(e => e.DESIGNATION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.END_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.START_DATE).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_EXPERIENCE)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_EXPERIENCE_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_INCREMENT>(entity =>
            {
                entity.HasKey(e => e.INCID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.INC_DATE).HasColumnType("datetime");

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS).IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_INCREMENT)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_INCREMENT_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_PROMOTION>(entity =>
            {
                entity.HasKey(e => e.PROMID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PROM_DATE).HasColumnType("datetime");

                entity.Property(e => e.REMARKS).IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_PROMOTION)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_PROMOTION_F_HRD_EMPLOYEE");

                entity.HasOne(d => d.NEW_DEG)
                    .WithMany(p => p.F_HRD_PROMOTIONNEW_DEG)
                    .HasForeignKey(d => d.NEW_DEGID)
                    .HasConstraintName("FK_F_HRD_PROMOTION_F_BAS_HRD_DESIGNATION_NEW");

                entity.HasOne(d => d.OLD_DEG)
                    .WithMany(p => p.F_HRD_PROMOTIONOLD_DEG)
                    .HasForeignKey(d => d.OLD_DEGID)
                    .HasConstraintName("FK_F_HRD_PROMOTION_F_BAS_HRD_DESIGNATION_OLD");
            });

            modelBuilder.Entity<F_HRD_SKILL>(entity =>
            {
                entity.HasKey(e => e.SKILLID);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SKILL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.LEVEL)
                    .WithMany(p => p.F_HRD_SKILL)
                    .HasForeignKey(d => d.LEVELID)
                    .HasConstraintName("FK_F_HRD_SKILL_F_HRD_SKILL_LEVEL");

                entity.HasOne(d => d.EMP)
                    .WithMany(p => p.F_HRD_SKILL)
                    .HasForeignKey(d => d.EMPID)
                    .HasConstraintName("FK_F_HRD_SKILL_F_HRD_EMPLOYEE");
            });

            modelBuilder.Entity<F_HRD_SKILL_LEVEL>(entity =>
            {
                entity.HasKey(e => e.LEVELID);

                entity.Property(e => e.LEVEL_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REMARKS)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BAS_GENDER>(entity =>
            {
                entity.HasKey(e => e.GENID);

                entity.Property(e => e.GENNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<F_HRD_EMPLOYEE>(entity =>
            {
                entity.HasKey(e => e.EMPID);

                entity.Property(e => e.ADD_PER).IsUnicode(false);

                entity.Property(e => e.ADD_PRE).IsUnicode(false);

                entity.Property(e => e.BANK_ACC_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BID_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_BY).HasMaxLength(50);

                entity.Property(e => e.DATE_BIRTH).HasColumnType("datetime");

                entity.Property(e => e.DATE_EMPSID_ISSUE).HasColumnType("datetime");

                entity.Property(e => e.DATE_JOINING).HasColumnType("datetime");

                entity.Property(e => e.DATE_TRANSFER).HasColumnType("datetime");

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EMPNO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FIRST_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FIRST_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.F_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IMAGE_PP).HasColumnType("image");

                entity.Property(e => e.LAST_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LAST_NAME_BN).HasMaxLength(50);

                entity.Property(e => e.MOBILE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_F)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_G)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MOBILE_M)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.M_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NID_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OPT5).HasMaxLength(50);

                entity.Property(e => e.OUT_DATE).HasColumnType("datetime");

                entity.Property(e => e.PASSPORT)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PO_PER)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PO_PER_BN).HasMaxLength(50);

                entity.Property(e => e.PO_PRE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PROX_CARD)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TIN_NO)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_BY).HasMaxLength(50);

                entity.Property(e => e.VILLAGE_PER)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VILLAGE_PER_BN).HasMaxLength(50);

                entity.Property(e => e.VILLAGE_PRE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BLDGRP)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.BLDGRPID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_BLOOD_GROUP");

                entity.HasOne(d => d.DEPT)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.DEPTID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DEPARTMENT");

                entity.HasOne(d => d.DESIG)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.DESIGID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DESIGNATION");

                entity.HasOne(d => d.EMPTYPE)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.EMPTYPEID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_EMP_TYPE");

                entity.HasOne(d => d.NATIONALITY_)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.NATIONALITY_ID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_NATIONALITY");

                entity.HasOne(d => d.OD)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.ODID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_WEEKEND");

                entity.HasOne(d => d.OUT_RESASON_)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.OUT_RESASON_ID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_OUT_REASON");

                entity.HasOne(d => d.RELIGION)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.RELIGIONID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_RELIGION");

                entity.HasOne(d => d.SEC)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.SECID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_SECTION");

                entity.HasOne(d => d.SHIFT)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.SHIFTID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_SHIFT");

                entity.HasOne(d => d.SUBSEC)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.SUBSECID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_SUB_SECTION");

                entity.HasOne(d => d.UNION_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.UNIONID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_UNION_PER");

                entity.HasOne(d => d.UNION_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.UNIONID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_UNION_PRE");

                entity.HasOne(d => d.COMPANY)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.COMPANYID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_COMPANY_INFO");

                entity.HasOne(d => d.GENDER)
                    .WithMany(p => p.F_HRD_EMPLOYEE)
                    .HasForeignKey(d => d.GENDERID)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_BAS_GENDER");

                entity.HasOne(d => d.DIST_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.DISTID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DISTRICT_PER");

                entity.HasOne(d => d.DIST_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.DISTID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DISTRICT_PRE");

                entity.HasOne(d => d.DIV_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.DIVID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DIVISION_PER");

                entity.HasOne(d => d.DIV_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.DIVID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_DIVISION_PRE");

                entity.HasOne(d => d.THANA_PER)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PER)
                    .HasForeignKey(d => d.THANAID_PER)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_THANA_PER");

                entity.HasOne(d => d.THANA_PRE)
                    .WithMany(p => p.F_HRD_EMPLOYEE_PRE)
                    .HasForeignKey(d => d.THANAID_PRE)
                    .HasConstraintName("FK_F_HRD_EMPLOYEE_F_BAS_HRD_THANA_PRE");
            });
        }
    }
}
