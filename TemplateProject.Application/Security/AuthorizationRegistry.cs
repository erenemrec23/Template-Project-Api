using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.AuthFeatures.Commands.DisableUserTwoFactor;
using QrAssignment.Application.Features.AuthFeatures.Commands.LoginTwoFactor;
using QrAssignment.Application.Features.Feedbacks.Commands.Create;
using QrAssignment.Application.Features.Feedbacks.Commands.UpdateStatus;
using QrAssignment.Application.Features.Feedbacks.Queries.GetList;
using QrAssignment.Application.Features.Menu.Queries.GetUserList;
using QrAssignment.Application.Features.PagePermissions.Commands.Update;
using QrAssignment.Application.Features.PagePermissions.Queries;
using QrAssignment.Application.Features.Permission.Commands.Excel;
using QrAssignment.Application.Features.Permission.Queries.GetPermissionReport;
using QrAssignment.Application.Features.Permission.Queries.GetPermissionReportLookup;
using QrAssignment.Application.Features.Profile.Commands;
using QrAssignment.Application.Features.Profile.Commands.TwoFactor;
using QrAssignment.Application.Features.Profile.Commands.Update;
using QrAssignment.Application.Features.Profile.Queries;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetById;
using QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetList;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetPassivedList;
using QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUpWithPermission;
using QrAssignment.Application.Features.Tenants.Queries.FormBase.GetById;
using QrAssignment.Application.Features.Tenants.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetList;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetPassivedList;
using QrAssignment.Application.Features.Users.Queries.LookUp.GetPermissionLookUp;
using QrAssignment.Domain.Shared.Menu;
using QrAssignment.Domain.Shared.PagePermission;
using System.Reflection;
using GetListMenuQuery = QrAssignment.Application.Features.Menu.Queries.GetList.GetListMenuQuery;
using RolesBulkExcelDto = QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate.BulkCreateAppRoleInputDto;
// Alias tanımlamaları ile tip isimleri ve okunabilirlik sadeleştirildi
using TenantsBulkExcelDto = QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate.BulkCreateTenantInputDto;
using UsersBulkExcelDto = QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate.BulkCreateAppUserInputDto;
namespace QrAssignment.Application.Security
{


    public static class AuthorizationRegistry
    {
        public static readonly Dictionary<Type, (string? PageName, PageAccessFlags Permission)> SecuredCommands;
        public static readonly HashSet<Type> UnsecuredCommands;
        public static readonly HashSet<Type> DynamicPageCommands;
        public static readonly HashSet<Type> AuthenticatedOnlyCommands;


        static AuthorizationRegistry()
        {
            var registry = new Dictionary<Type, (string? PageName, PageAccessFlags Permission)>();
            var dynamicCommands = new HashSet<Type>();

            // =========================================================================
            // TENANTS (Page_Tenants)
            // =========================================================================
            Register(registry, AppPage.Tenants, PageAccessFlags.Insert,
                typeof(Features.Tenants.Commands.Create.CreateTenantCommand));

            Register(registry, AppPage.Tenants, PageAccessFlags.Update,
                typeof(Features.Tenants.Commands.Update.UpdateTenantCommand),
                typeof(Features.Tenants.Commands.SetActive.SetActiveTenantCommand));

            Register(registry, AppPage.Tenants, PageAccessFlags.Delete,
                typeof(Features.Tenants.Commands.Delete.DeleteTenantCommand),
                typeof(Features.Tenants.Commands.BulkDelete.BulkDeleteTenantCommand));

            Register(registry, AppPage.Tenants, PageAccessFlags.View,
                typeof(GetByIdTenantQuery),
                typeof(GetListTenantQuery));

            Register(registry, AppPage.Tenants, PageAccessFlags.SetPassive,
                typeof(Features.Tenants.Commands.SetPassive.SetPassiveTenantCommand),
                typeof(Features.Tenants.Commands.BulkSetPassive.BulkSetPassiveTenantCommand));

            Register(registry, AppPage.Tenants, PageAccessFlags.SetActive,
                typeof(Features.Tenants.Commands.SetActive.SetActiveTenantCommand),
                typeof(Features.Tenants.Commands.BulkSetActive.BulkSetActiveTenantCommand));

            Register(registry, AppPage.Tenants, PageAccessFlags.ViewPassive,
                typeof(GetPassivedByIdTenantQuery),
                typeof(GetPassivedListTenantQuery));

            Register(registry, AppPage.Tenants, PageAccessFlags.ExportExcel,
                typeof(GetListTenantExportExcelQuery));

            Register(registry, AppPage.Tenants, PageAccessFlags.ImportExcel,
                typeof(Features.Tenants.Commands.Excel.BulkCreate.BulkCreateTenantCommand),
                typeof(Features.Tenants.Commands.Excel.Validate.ValidateTenantExcelQuery),
                typeof(ValidateExcelQuery<TenantsBulkExcelDto>),
                typeof(GetSampleExcelTemplateQuery<TenantsBulkExcelDto>));


            // =========================================================================
            // USERS (Page_Users)
            // =========================================================================
            Register(registry, AppPage.Users, PageAccessFlags.Insert,
                typeof(Features.Users.Commands.Create.CreateAppUserCommand));

            Register(registry, AppPage.Users, PageAccessFlags.Update,
                typeof(Features.Users.Commands.Update.UpdateAppUserCommand),
                typeof(Features.Users.Commands.SetActive.SetActiveAppUserCommand),
                typeof(Features.Permission.Commands.Update.UpdateUserPermissionCommand),
                typeof(Features.PagePermissions.Commands.UpdateRolesPermissions.UpdateRolesPermissionsCommand),
                typeof(DisableUserTwoFactorCommand));

            Register(registry, AppPage.Users, PageAccessFlags.Delete,
                typeof(Features.Users.Commands.Delete.DeleteAppUserCommand),
                typeof(Features.Users.Commands.BulkDelete.BulkDeleteAppUserCommand));

            Register(registry, AppPage.Users, PageAccessFlags.View,
                typeof(Features.Users.Queries.FormBase.GetById.GetByIdAppUserQuery),
                typeof(Features.Users.Queries.ListBase.GetList.GetListAppUserQuery),
                typeof(Features.Users.Queries.LookUp.GetLookupList.GetLookUpListAppUserQuery),
                typeof(Features.Permission.Queries.GetByUserId.GetByIdPermissionUserQuery),
                typeof(Features.Roles.Queries.LookUp.GetRoleLookUp.GetRoleLookUpQuery),
                typeof(Features.Users.Queries.LookUp.GetLookUpListAppUserAssignedRoleId.GetLookUpListAppUserAssignedRoleIdQuery));

            Register(registry, AppPage.Users, PageAccessFlags.SetPassive,
                typeof(Features.Users.Commands.SetPassive.SetPassiveAppUserCommand),
                typeof(Features.Users.Commands.BulkSetPassive.BulkSetPassiveAppUserCommand));

            Register(registry, AppPage.Users, PageAccessFlags.SetActive,
                typeof(Features.Users.Commands.SetActive.SetActiveAppUserCommand),
                typeof(Features.Users.Commands.BulkSetActive.BulkSetActiveAppUserCommand));

            Register(registry, AppPage.Users, PageAccessFlags.ViewPassive,
                typeof(Features.Users.Queries.FormBase.GetPassivedById.GetPassivedByIdAppUserQuery),
                typeof(Features.Users.Queries.ListBase.GetPassivedList.GetPassivedListAppUserQuery));

            Register(registry, AppPage.Users, PageAccessFlags.ExportExcel,
                typeof(Features.Users.Queries.ListBase.GetListExportExcel.GetListAppUserExportExcelQuery));

            Register(registry, AppPage.Users, PageAccessFlags.ImportExcel,
                typeof(Features.Users.Commands.Excel.BulkCreate.BulkCreateAppUserCommand),
                typeof(Features.Users.Commands.Excel.Validate.ValidateAppUserExcelQuery),
                typeof(ValidateExcelQuery<UsersBulkExcelDto>),
                typeof(GetSampleExcelTemplateQuery<UsersBulkExcelDto>));


            // =========================================================================
            // ROLES (Page_Roles)
            // =========================================================================
            Register(registry, AppPage.Roles, PageAccessFlags.Insert,
                typeof(Features.Roles.Commands.Create.CreateAppRoleCommand));

            Register(registry, AppPage.Roles, PageAccessFlags.Update,
                typeof(Features.Roles.Commands.Update.UpdateAppRoleCommand),
                typeof(Features.Roles.Commands.BulkSetActive.BulkSetActiveAppRoleCommand),
                typeof(Features.PagePermissions.Commands.UpdateRolesPermissions.UpdateRolesPermissionsCommand),
                typeof(Features.PagePermissions.Commands.UpdateUsersPermissions.UpdateUsersPermissionsCommand));

            Register(registry, AppPage.Roles, PageAccessFlags.Delete,
                typeof(Features.Roles.Commands.Delete.DeleteAppRoleCommand),
                typeof(Features.Roles.Commands.BulkDelete.BulkDeleteAppRoleCommand));

            Register(registry, AppPage.Roles, PageAccessFlags.View,
                typeof(Features.Roles.Queries.FormBase.GetById.GetByIdRoleQuery),
                typeof(Features.Roles.Queries.ListBase.GetList.GetListAppRoleQuery),
                typeof(Features.Roles.Queries.LookUp.GetAssignedUserList.GetRoleAssignedUserListQuery),
                typeof(Features.Roles.Queries.GetAssignedPermissionList.GetRoleAssignedPermissionListQuery));

            Register(registry, AppPage.Roles, PageAccessFlags.SetPassive,
                typeof(Features.Roles.Commands.SetPassive.SetPassiveAppRoleCommand),
                typeof(Features.Roles.Commands.BulkSetPassive.BulkSetPassiveAppRoleCommand));

            Register(registry, AppPage.Roles, PageAccessFlags.SetActive,
                typeof(Features.Roles.Commands.SetActive.SetActiveAppRoleCommand),
                typeof(Features.Roles.Commands.BulkSetActive.BulkSetActiveAppRoleCommand));

            Register(registry, AppPage.Roles, PageAccessFlags.ViewPassive,
                typeof(Features.Roles.Queries.FormBase.GetPassivedById.GetPassivedByIdAppRoleQuery),
                typeof(Features.Roles.Queries.ListBase.GetPassivedList.GetPassivedListAppRoleQuery));

            Register(registry, AppPage.Roles, PageAccessFlags.ExportExcel,
                typeof(Features.Roles.Queries.ListBase.GetListExportExcel.GetListAppRoleExportExcelQuery));

            Register(registry, AppPage.Roles, PageAccessFlags.ImportExcel,
                typeof(Features.Roles.Commands.Excel.BulkCreate.BulkCreateAppRoleCommand),
                typeof(Features.Roles.Commands.Excel.Validate.ValidateAppRoleExcelQuery),
                typeof(ValidateExcelQuery<RolesBulkExcelDto>),
                typeof(GetSampleExcelTemplateQuery<RolesBulkExcelDto>));


            // =========================================================================
            // QR LOCATIONS (Page_QrLocations)
            // =========================================================================
            Register(registry, AppPage.QrLocations, PageAccessFlags.Insert,
    typeof(Features.QrLocations.Commands.Create.CreateQrLocationCommand));

            Register(registry, AppPage.QrLocations, PageAccessFlags.Update,
                typeof(Features.QrLocations.Commands.Update.UpdateQrLocationCommand),
                typeof(Features.QrLocations.Commands.SetActive.SetActiveQrLocationCommand));

            Register(registry, AppPage.QrLocations, PageAccessFlags.Delete,
                typeof(Features.QrLocations.Commands.Delete.DeleteQrLocationCommand),
                typeof(Features.QrLocations.Commands.BulkDelete.BulkDeleteQrLocationCommand));

            Register(registry, AppPage.QrLocations, PageAccessFlags.View,
                typeof(GetByIdQrLocationQuery),
                typeof(GetListQrLocationQuery));

            Register(registry, AppPage.QrLocations, PageAccessFlags.SetPassive,
                typeof(Features.QrLocations.Commands.SetPassive.SetPassiveQrLocationCommand),
                typeof(Features.QrLocations.Commands.BulkSetPassive.BulkSetPassiveQrLocationCommand));

            Register(registry, AppPage.QrLocations, PageAccessFlags.SetActive,
                typeof(Features.QrLocations.Commands.SetActive.SetActiveQrLocationCommand),
                typeof(Features.QrLocations.Commands.BulkSetActive.BulkSetActiveQrLocationCommand));

            Register(registry, AppPage.QrLocations, PageAccessFlags.ViewPassive,
                typeof(GetPassivedByIdQrLocationQuery),
                typeof(GetPassivedListQrLocationQuery));

            Register(registry, AppPage.QrLocations, PageAccessFlags.ExportExcel,
                typeof(GetListQrLocationExportExcelQuery));

            Register(registry, AppPage.QrLocations, PageAccessFlags.ImportExcel,
                typeof(Features.QrLocations.Commands.Excel.BulkCreate.BulkCreateQrLocationCommand),
                typeof(Features.QrLocations.Commands.Excel.Validate.ValidateQrLocationExcelQuery),
                typeof(ValidateExcelQuery<BulkCreateQrLocationInputDto>),
                typeof(GetSampleExcelTemplateQuery<BulkCreateQrLocationInputDto>));

              

            Register(registry, AppPage.PermissionReports, PageAccessFlags.View,
                typeof(GetPermissionReportQuery),
                typeof(GetPermissionReportLookupQuery),
                typeof(ExportPermissionReportExcelQuery));

            Register(registry, AppPage.FeedBacks, PageAccessFlags.View,
                typeof(GetListFeedBackQuery));
            Register(registry, AppPage.FeedBacks, PageAccessFlags.Update,
                typeof(UpdateFeedbackStatusCommand));

            SecuredCommands = registry;

            // =========================================================================
            // UNSECURED COMMANDS
            // =========================================================================
            UnsecuredCommands = new HashSet<Type>
            {
                typeof(Features.AuthFeatures.Commands.Login.LoginCommand),
                typeof(Features.AuthFeatures.Commands.ForgotPassword.ForgotPasswordCommand),
                typeof(Features.AuthFeatures.Commands.ResetPassword.ResetPasswordCommand),
                typeof(GetSystemModulesQuery),
                typeof(GetListMenuQuery),
                typeof(GetUserMenuQuery),
                typeof(LoginTwoFactorCommand),
                
            };
            AuthenticatedOnlyCommands = new HashSet<Type>
{
    typeof(CreateFeedbackCommand),
    typeof(GetProfileQuery),
    typeof(UpdateProfileCommand),
    typeof(ChangePasswordCommand),
    typeof(BeginTwoFactorSetupCommand),
    typeof(EnableTwoFactorCommand),
    typeof(DisableTwoFactorCommand),
};
            RegisterDynamic(registry, dynamicCommands, PageAccessFlags.ManagePagePermissions,
            typeof(GetPagePermissionsForPageQuery),
            typeof(UpdatePagePermissionsForPageCommand),
            typeof(GetRoleLookUpWithPermissionQuery),
            typeof(GetUserLookUpWithPermissionQuery));
        }

        private static void Register(
            Dictionary<Type, (string PageName, PageAccessFlags Permission)> registry,
            AppPage page,
            PageAccessFlags permission,
            params Type[] commandTypes)
        {
            var pageKey = ResolvePageKey(page);
            foreach (var type in commandTypes)
            {
                registry[type] = (pageKey, permission);
            }
        }
        private static readonly Dictionary<AppPage, string> _pageKeyCache = new();

        private static string ResolvePageKey(AppPage page)
        {
            if (_pageKeyCache.TryGetValue(page, out var cached)) return cached;

            var field = typeof(AppPage).GetField(page.ToString())
                ?? throw new InvalidOperationException($"{page} AppPage enum'da bulunamadı.");
            var attr = field.GetCustomAttribute<PageDefinitionAttribute>()
                ?? throw new InvalidOperationException($"{page} için [PageDefinition] tanımlı değil.");
            if (string.IsNullOrWhiteSpace(attr.PageKey))
                throw new InvalidOperationException($"{page} için PageKey boş.");

            return _pageKeyCache[page] = attr.PageKey;
        }
        private static void RegisterDynamic(
            Dictionary<Type, (string? PageName, PageAccessFlags Permission)> registry,
            HashSet<Type> dynamicCommands,
            PageAccessFlags permission,
            params Type[] commandTypes)
        {
            foreach (var type in commandTypes)
            {
                if (!typeof(IPageScopedRequest).IsAssignableFrom(type))
                    throw new InvalidOperationException(
                        $"{type.Name}, RegisterDynamic ile kaydedildi ama IPageScopedRequest implement etmiyor.");

                registry[type] = (PageName: null, permission); // PageName null = "request'ten oku"
                dynamicCommands.Add(type);
            }
        }
    }
}