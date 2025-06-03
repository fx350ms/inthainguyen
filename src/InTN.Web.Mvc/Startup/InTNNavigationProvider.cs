using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using InTN.Authorization;

namespace InTN.Web.Startup;

/// <summary>
/// This class defines menus for the application.
/// </summary>
public class InTNNavigationProvider : NavigationProvider
{
    public override void SetNavigation(INavigationProviderContext context)
    {
        context.Manager.MainMenu
            //.AddItem(
            //    new MenuItemDefinition(
            //        PageNames.About,
            //        L("About"),
            //        url: "About",
            //        icon: "fas fa-info-circle"
            //    )
            //)
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Home,
                    L("HomePage"),
                    url: "",
                    icon: "fas fa-home",
                    requiresAuthentication: true
                )
            ).AddItem(
                new MenuItemDefinition(
                    PageNames.Users,
                    L("Users"),
                    url: "Users",
                    icon: "fas fa-users",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Roles,
                    L("Roles"),
                    url: "Roles",
                    icon: "fas fa-theater-masks",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Orders,
                    L("Orders"),
                    url: "Orders",
                    icon: "fas fa-tasks",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Orders)
                )
            )
              .AddItem(
                new MenuItemDefinition(
                    PageNames.Customers,
                    L("Customers"),
                    url: "Customers",
                    icon: "fas fa-user-friends",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Customers)
                )
            )
              .AddItem(
                new MenuItemDefinition(
                    PageNames.Transactions,
                    L("Transactions"),
                    url: "Transaction",
                    icon: "fas fa-money-check-alt",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Transactions)
                )
            )
             // Các menu mới
            .AddItem(
                new MenuItemDefinition(
                    PageNames.ProductCategories,
                    L("ProductCategories"),
                    url: "ProductCategories",
                    icon: "fas fa-sitemap",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_ProductCategories)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.ProductProperties,
                    L("ProductProperties"),
                    url: "ProductProperties",
                    icon: "fas fa-tags",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_ProductProperties)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Suppliers,
                    L("Suppliers"),
                    url: "Suppliers",
                    icon: "fas fa-truck",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Suppliers)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Brands,
                    L("Brands"),
                    url: "Brands",
                    icon: "fas fa-briefcase",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Brands)
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    PageNames.ProductTypes,
                    L("ProductTypes"),
                    url: "ProductTypes",
                    icon: "fas fa-boxes",
                    permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_ProductTypes)
                )
            );
            ;
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, InTNConsts.LocalizationSourceName);
    }
}