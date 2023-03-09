using System;
using System.Collections.Generic;
using System.Globalization;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures;
using DenimERP.ServiceInfrastructures.CompanyInfo;
using DenimERP.ServiceInfrastructures.Emp;
using DenimERP.ServiceInfrastructures.HR;
using DenimERP.ServiceInfrastructures.Hubs;
using DenimERP.ServiceInfrastructures.IdentityUser;
using DenimERP.ServiceInfrastructures.MenuMaster;
using DenimERP.ServiceInfrastructures.OtherInfrastructures;
using DenimERP.ServiceInfrastructures.Security;
using DenimERP.ServiceInterfaces;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ServiceInterfaces.CompanyInfo;
using DenimERP.ServiceInterfaces.Emp;
using DenimERP.ServiceInterfaces.HR;
using DenimERP.ServiceInterfaces.Hubs;
using DenimERP.ServiceInterfaces.IdentityUser;
using DenimERP.ServiceInterfaces.MenuMaster;
using DenimERP.ServiceInterfaces.OtherInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace DenimERP.ConfigureServices
{
    public static class ExtractEmService
    {
        public static void ExtractEmRegisterService(IServiceCollection services)
        {
            services.AddScoped<IBAS_BEN_BANK_MASTER, SQLBAS_BEN_BANK_MASTER_Repository>();
            services.AddScoped<IRequestFiles, RequestFilesRepository>();
            services.AddScoped<ICOUNTRIES, SQLCOUNTRIES_Repository>();
            services.AddScoped<IPDL_EMAIL_SENDER, SQLPDL_EMAIL_SENDER_Repository>();
            //HR
            services.AddScoped<IF_BAS_HRD_DEPARTMENT, SQLF_BAS_HRD_DEPARTMENT_Repository>();
            services.AddScoped<IF_BAS_HRD_SECTION, SQLF_BAS_HRD_SECTION_Repository>();
            services.AddScoped<IF_BAS_HRD_SUB_SECTION, SQLF_BAS_HRD_SUB_SECTION_Repository>();
            services.AddScoped<IF_BAS_HRD_DESIGNATION, SQLF_BAS_HRD_DESIGNATION_Repository>();
            services.AddScoped<IF_BAS_HRD_LOCATION, SQLF_BAS_HRD_LOCATION_Repository>();
            services.AddScoped<IF_BAS_HRD_GRADE, SQLF_BAS_HRD_GRADE_Repository>();
            services.AddScoped<IF_BAS_HRD_EMP_TYPE, SQLF_BAS_HRD_EMP_TYPE_Repository>();
            services.AddScoped<IF_BAS_HRD_NATIONALITY, SQLF_BAS_HRD_NATIONALITY_Repository>();
            services.AddScoped<IF_BAS_HRD_OUT_REASON, SQLF_BAS_HRD_OUT_REASON_Repository>();
            services.AddScoped<IF_BAS_HRD_SHIFT, SQLF_BAS_HRD_SHIFT_Repository>();
            services.AddScoped<IF_HRD_EMP_EDU_DEGREE, SQLF_HRD_EMP_EDU_DEGREE_Repository>();
            services.AddScoped<IF_HRD_EDUCATION, SQLF_HRD_EDUCATION_Repository>();
            services.AddScoped<IF_HRD_EMP_SPOUSE, SQLF_HRD_EMP_SPOUSE_Repository>();
            services.AddScoped<IF_HRD_EMPLOYEE, SQLF_HRD_EMPLOYEE_Repository>();

            services.AddScoped<IMESSAGE, SQLMESSAGE_Repository>();
            services.AddScoped<IMESSAGE_INDIVIDUAL, SQLMESSAGE_INDIVIDUAL_Repository>();
            // Menu Master
            services.AddTransient<IMenuMaster, SQLMenuMaster_Repository>();
            services.AddTransient<IMenuMasterRoles, SQLMenuMasterRoles_Repository>();

            // Company Info
            services.AddScoped<ICOMPANY_INFO, SQLCOMPANY_INFO_Repository>();

            // Identity User
            services.AddScoped<IAspNetUserTypes, SQLAspNetUserTypes_Repository>();

            // API
            services.AddScoped<IEmployee, SQLEmployee_Repository>();

            //MAILBOX
            services.AddScoped<IMAILBOX, SQLMAILBOX_Repository>();
            
            // SignalR
            services.AddSignalR();

            // Implement Later core v2.1+
            //services.AddControllersWithViews(options =>
            //{
            //    options.Filters.Add(typeof(StoreActionFilter));
            //});

            // Resources
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("bn-BD"),
                        new CultureInfo("en-GB"),
                        new CultureInfo("ja-JP"),
                        //new CultureInfo("en-US")
                    };
                    options.DefaultRequestCulture = new RequestCulture("en-GB");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            // Other
            services.AddScoped<IDeleteFileFromFolder, DeleteFileFromFolderRepository>();
            services.AddScoped<IProcessUploadFile, ProcessUploadFileToFolderRepository>();

            // Pagination
            services.AddCloudscribePagination();

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "117021812063-c75uer39qi8f7ru2o2tj2jisraorvhqd.apps.googleusercontent.com";
                    options.ClientSecret = "lo9H8Ej9ZiYCWMzWDhmQm3KP";
                });

            // Changes token lifespan of all token types
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                    o.TokenLifespan = TimeSpan.FromHours(5));

            // Changes token lifespan of just the Email Confirmation Token type
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
                    o.TokenLifespan = TimeSpan.FromDays(3));

            services.AddSingleton<DataProtectionPurposeStrings>();

            // Bind the email
            services.AddSingleton<IPDL_EMAIL_SENDER<bool>, DevEmailSender>();
        }
    }
}
