using HRMS.Security;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.ConfigureServices
{
    public static class ExtractEMPolicyService
    {
        public static void GetExtractEmPolicyService(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role", "true"));
                options.AddPolicy("ReadRolePolicy", policy => policy.RequireClaim("Read Role", "true"));
                options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
                
                // CUSTOM POLICY
                options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                // GENERAL POLICY
                //options.AddPolicy("EditRolePolicy",
                //    policy => policy.RequireAssertion(context =>
                //    context.User.IsInRole("Admin") &&
                //    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //    context.User.IsInRole("Super Admin")));

                // OTHER POLICIES
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Super Admin"));
                options.AddPolicy("ApproveRndFabric", policy => policy.RequireRole("Planning(F)", "Planning(HO)", "RND Head"));
                options.AddPolicy("HOSample", policy => policy.RequireRole("Super Admin", "Audit", "Sample(HO)"));
                options.AddPolicy("FSample", policy => policy.RequireRole("Super Admin", "Audit", "Sample(F)", "RND"));

                options.AddPolicy("SampleFabricIssue", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Sample(F)") ||
                    context.User.IsInRole("Sample(HO)") ||
                    context.User.IsInRole("Marketing") ||
                    context.User.IsInRole("Internal Audit Import Export") ||
                    context.User.IsInRole("Admin") ||
                    ((context.User.IsInRole("Super Admin") || context.User.IsInRole("Admin") || context.User.IsInRole("Developer"))) &&
                    context.User.HasClaim(claim =>
                        (claim.Type == "Create Role" && claim.Value == "true") &&
                        (claim.Type == "Edit Role" && claim.Value == "true") &&
                        (claim.Type == "Edit Role" && claim.Value == "true"))
                    ));

                options.AddPolicy("SampleFabricIssueDelete", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Sample(HO)") ||
                    ((context.User.IsInRole("Super Admin") || context.User.IsInRole("Admin") || context.User.IsInRole("Developer"))) &&
                    context.User.HasClaim(claim =>
                        (claim.Type == "Create Role" && claim.Value == "true") &&
                        (claim.Type == "Edit Role" && claim.Value == "true") &&
                        (claim.Type == "Read Role" && claim.Value == "true") &&
                        (claim.Type == "Delete Role" && claim.Value == "true"))
                    ));

                options.AddPolicy("SampleFabricIssueForReport", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Sample(HO)") ||
                    context.User.IsInRole("Sample(F)") ||
                    context.User.IsInRole("Marketing") ||
                    context.User.IsInRole("Internal Audit Import Export") ||
                    ((context.User.IsInRole("Super Admin") || context.User.IsInRole("Admin") || context.User.IsInRole("Developer"))) &&
                    context.User.HasClaim(claim =>
                        (claim.Type == "Create Role" && claim.Value == "true") &&
                        (claim.Type == "Edit Role" && claim.Value == "true") &&
                        (claim.Type == "Read Role" && claim.Value == "true") &&
                        (claim.Type == "Delete Role" && claim.Value == "true"))
                    ));

                options.AddPolicy("HR", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Super Admin") ||
                    ((context.User.IsInRole("HR") || context.User.IsInRole("HR(F)")) && context.User.HasClaim(claim => (claim.Type == "Create Role" && claim.Value == "true") && claim.Type == "Edit Role" && claim.Value == "true"))));

                options.AddPolicy("YarnStore", policy => policy.RequireAssertion(context =>
                    (context.User.IsInRole("Yarn Store") ||
                     context.User.IsInRole("Audit") ||
                     context.User.IsInRole("Audit Process Control") ||
                     context.User.IsInRole("Accounts")) &&
                    context.User.HasClaim(claim =>
                        (claim.Type == "Create Role" && claim.Value == "true") &&
                        (claim.Type == "Edit Role" && claim.Value == "true"))));

                options.AddPolicy("CommercialExportLC", policy => policy.RequireAssertion(context =>
                    (context.User.IsInRole("Com Exp") ||
                    context.User.IsInRole("Commercial") ||
                    context.User.IsInRole("Admin") ||
                    context.User.IsInRole("Super Admin") ||
                    context.User.IsInRole("Developer") ||
                    context.User.IsInRole("Accounts Head")) &&
                    context.User.HasClaim(claim =>
                        (claim.Type == "Create Role" && claim.Value == "true") &&
                        (claim.Type == "Edit Role" && claim.Value == "true"))));
                options.AddPolicy("DeveloperPolicy", policy => policy.RequireAssertion(context =>
                        (context.User.IsInRole("Developer")) &&
                        context.User.HasClaim(claim =>
                            (claim.Type == "Create Role" && claim.Value == "true") &&
                            (claim.Type == "Edit Role" && claim.Value == "true"))));
                
                options.AddPolicy("CostingPDLCost", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Costing") ||
                    context.User.IsInRole("audit_head") ||
                    ((context.User.IsInRole("Super Admin") || context.User.IsInRole("Admin") || context.User.IsInRole("Developer"))) &&
                    context.User.HasClaim(claim =>
                        (claim.Type == "Create Role" && claim.Value == "true") &&
                        (claim.Type == "Edit Role" && claim.Value == "true") &&
                        (claim.Type == "Read Role" && claim.Value == "true") &&
                        (claim.Type == "Delete Role" && claim.Value == "true"))
                ));

                options.AddPolicy("CustomsPolicy", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Customs") &&
                    (!(context.User.IsInRole("Super Admin") || context.User.IsInRole("Admin") || context.User.IsInRole("Developer")))
                ));
                
                options.AddPolicy("SampleFabricIssueForFactoryView", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Sample(F)") 
                    //||
                    //((context.User.IsInRole("Super Admin") || context.User.IsInRole("Admin") || context.User.IsInRole("Developer"))) &&
                    //context.User.HasClaim(claim =>
                    //    (claim.Type == "Create Role" && claim.Value == "true") &&
                    //    (claim.Type == "Edit Role" && claim.Value == "true") &&
                    //    (claim.Type == "Read Role" && claim.Value == "true") &&
                    //    (claim.Type == "Delete Role" && claim.Value == "true"))
                ));
            });
        }
    }
}
