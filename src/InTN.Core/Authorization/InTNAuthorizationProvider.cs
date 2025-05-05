using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace InTN.Authorization;

public class InTNAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
        context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
        context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
        context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);


        context.CreatePermission(PermissionNames.Pages_Orders, L("Orders"));
        context.CreatePermission(PermissionNames.Pages_Customers, L("Customers"));
        context.CreatePermission(PermissionNames.Fn_Orders_Create, L("CreateOrder"));
        context.CreatePermission(PermissionNames.Fn_Orders_CreateQuotation, L("CreateQuotation"));
        context.CreatePermission(PermissionNames.Fn_Orders_ApproveDesign, L("ApproveDesign"));
        context.CreatePermission(PermissionNames.Fn_Orders_UpdateStatusToDeposited, L("UpdateStatusToDeposited"));
        context.CreatePermission(PermissionNames.Fn_Orders_UpdateStatusToPrintedTest, L("UpdateStatusToPrintedTest"));
        context.CreatePermission(PermissionNames.Fn_Orders_ConfirmPrintedTest, L("ConfirmPrintedTest"));
        context.CreatePermission(PermissionNames.Fn_Orders_PerformPrinting, L("PerformPrinting"));
        context.CreatePermission(PermissionNames.Fn_Orders_PerformProcessing, L("PerformProcessing"));
        context.CreatePermission(PermissionNames.Fn_Orders_ShipOrder, L("ShipOrder"));
        context.CreatePermission(PermissionNames.Fn_Orders_CompleteOrder, L("CompleteOrder"));

    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, InTNConsts.LocalizationSourceName);
    }
}
