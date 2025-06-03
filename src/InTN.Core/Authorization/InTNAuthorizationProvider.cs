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

        context.CreatePermission(PermissionNames.Pages_Transactions, L("Transactions"));
        context.CreatePermission(PermissionNames.Fn_Transactions_Create, L("CreateTransaction"));
          // Permissions for ProductCategories
        context.CreatePermission(PermissionNames.Pages_ProductCategories, L("ProductCategories"));
        context.CreatePermission(PermissionNames.Fn_ProductCategories_Create, L("CreateProductCategory"));
        context.CreatePermission(PermissionNames.Fn_ProductCategories_Edit, L("EditProductCategory"));
        context.CreatePermission(PermissionNames.Fn_ProductCategories_Delete, L("DeleteProductCategory"));

        // Permissions for ProductProperties
        context.CreatePermission(PermissionNames.Pages_ProductProperties, L("ProductProperties"));
        context.CreatePermission(PermissionNames.Fn_ProductProperties_Create, L("CreateProductProperty"));
        context.CreatePermission(PermissionNames.Fn_ProductProperties_Edit, L("EditProductProperty"));
        context.CreatePermission(PermissionNames.Fn_ProductProperties_Delete, L("DeleteProductProperty"));

        // Permissions for Suppliers
        context.CreatePermission(PermissionNames.Pages_Suppliers, L("Suppliers"));
        context.CreatePermission(PermissionNames.Fn_Suppliers_Create, L("CreateSupplier"));
        context.CreatePermission(PermissionNames.Fn_Suppliers_Edit, L("EditSupplier"));
        context.CreatePermission(PermissionNames.Fn_Suppliers_Delete, L("DeleteSupplier"));

        // Permissions for Brands
        context.CreatePermission(PermissionNames.Pages_Brands, L("Brands"));
        context.CreatePermission(PermissionNames.Fn_Brands_Create, L("CreateBrand"));
        context.CreatePermission(PermissionNames.Fn_Brands_Edit, L("EditBrand"));
        context.CreatePermission(PermissionNames.Fn_Brands_Delete, L("DeleteBrand"));

        // Permissions for ProductTypes
        context.CreatePermission(PermissionNames.Pages_ProductTypes, L("ProductTypes"));
        context.CreatePermission(PermissionNames.Fn_ProductTypes_Create, L("CreateProductType"));
        context.CreatePermission(PermissionNames.Fn_ProductTypes_Edit, L("EditProductType"));
        context.CreatePermission(PermissionNames.Fn_ProductTypes_Delete, L("DeleteProductType"));
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, InTNConsts.LocalizationSourceName);
    }
}
