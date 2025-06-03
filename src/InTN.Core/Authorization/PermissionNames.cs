namespace InTN.Authorization;

public static class PermissionNames
{
    public const string Pages_Tenants = "Pages.Tenants";

    public const string Pages_Users = "Pages.Users";
    public const string Pages_Users_Activation = "Pages.Users.Activation";

    public const string Pages_Roles = "Pages.Roles";
    
    public const string Pages_Customers = "Pages.Customers";
   

    public const string Pages_Orders = "Pages.Orders";
    public const string Fn_Orders_Create = "Fn.Orders.Create";
    public const string Fn_Orders_CreateQuotation = "Fn.Orders.CreateQuotation";
    public const string Fn_Orders_ApproveDesign = "Fn.Orders.ApproveDesign";
    public const string Fn_Orders_UpdateStatusToDeposited = "Fn.Orders.UpdateStatusToDeposited";
    public const string Fn_Orders_UpdateStatusToPrintedTest = "Fn.Orders.UpdateStatusToPrintedTest";
    public const string Fn_Orders_ConfirmPrintedTest = "Fn.Orders.ConfirmPrintedTest";
    public const string Fn_Orders_PerformPrinting = "Fn.Orders.PerformPrinting";
    public const string Fn_Orders_PerformProcessing = "Fn.Orders.PerformProcessing";
    public const string Fn_Orders_ShipOrder = "Fn.Orders.ShipOrder";
    public const string Fn_Orders_CompleteOrder = "Fn.Orders.CompleteOrder";

    public const string Pages_Transactions = "Pages.Transactions";
    public const string Fn_Transactions_Create = "Fn.Transactions.Create";


    // Permissions for ProductCategories
    public const string Pages_ProductCategories = "Pages.ProductCategories";
    public const string Fn_ProductCategories_Create = "Fn.ProductCategories.Create";
    public const string Fn_ProductCategories_Edit = "Fn.ProductCategories.Edit";
    public const string Fn_ProductCategories_Delete = "Fn.ProductCategories.Delete";

    // Permissions for ProductProperties
    public const string Pages_ProductProperties = "Pages.ProductProperties";
    public const string Fn_ProductProperties_Create = "Fn.ProductProperties.Create";
    public const string Fn_ProductProperties_Edit = "Fn.ProductProperties.Edit";
    public const string Fn_ProductProperties_Delete = "Fn.ProductProperties.Delete";

    // Permissions for Suppliers
    public const string Pages_Suppliers = "Pages.Suppliers";
    public const string Fn_Suppliers_Create = "Fn.Suppliers.Create";
    public const string Fn_Suppliers_Edit = "Fn.Suppliers.Edit";
    public const string Fn_Suppliers_Delete = "Fn.Suppliers.Delete";

    // Permissions for Brands
    public const string Pages_Brands = "Pages.Brands";
    public const string Fn_Brands_Create = "Fn.Brands.Create";
    public const string Fn_Brands_Edit = "Fn.Brands.Edit";
    public const string Fn_Brands_Delete = "Fn.Brands.Delete";

    // Permissions for ProductTypes
    public const string Pages_ProductTypes = "Pages.ProductTypes";
    public const string Fn_ProductTypes_Create = "Fn.ProductTypes.Create";
    public const string Fn_ProductTypes_Edit = "Fn.ProductTypes.Edit";
    public const string Fn_ProductTypes_Delete = "Fn.ProductTypes.Delete";
}


