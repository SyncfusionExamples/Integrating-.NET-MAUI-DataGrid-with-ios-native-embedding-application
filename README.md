# Integrating .NET MAUI DataGrid with ios native embedding application.

To create a [.NET MAUI DataGrid]( https://www.syncfusion.com/maui-controls/maui-datagrid) in a native embedded ios application, you need to follow a series of steps. This article will guide you through the process.
### Step 1: Create a new .NET ios application
Create a new .NET ios application in Visual Studio. Syncfusion .NET MAUI components are available in [nuget.org](https://www.nuget.org/). To add SfDataGrid to your project, open the NuGet package manager in Visual Studio and search for Syncfusion.Maui.DataGrid, then install it.
### Step 2: Enable .NET MAUI support
First, enable .NET MAUI support in the native app’s project file. Enable support by adding <UseMaui>true</UseMaui> to the <PropertyGroup> node in the project file.

[XAML]
```
<PropertyGroup>
 . . . 
 <Nullable>enable</Nullable> 
 <UseMaui>true</UseMaui> 
 <ImplicitUsings>enable</ImplicitUsings> 
 . . . 
 </PropertyGroup>
```
### Step 3: Initialize .NET MAUI
The pattern for initializing .NET MAUI in a native app project is to create a MauiAppBuilder object and invoke the UseMauiEmbedding method. Additionally, configure it to set up SyncfusionCore components within the .NET MAUI app.

[C#]
```
public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
{
    // create a new window instance based on the screen size.
    Window = new UIWindow(UIScreen.MainScreen.Bounds);
    MauiAppBuilder builder = MauiApp.CreateBuilder();
    builder.UseMauiEmbedding<microsoft.maui.controls.application>();
    builder.ConfigureSyncfusionCore();
}
```
Then, create a MauiApp object by invoking the Build() method on the MauiAppBuilder object. In addition, a MauiContext object should be made from the MauiApp object. The MauiContext object will be used when converting .NET MAUI controls to native types.

[C#]
```
public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
{
    // create a new window instance based on the screen size.
    Window = new UIWindow(UIScreen.MainScreen.Bounds);
    MauiAppBuilder builder = MauiApp.CreateBuilder();
    builder.UseMauiEmbedding<microsoft.maui.controls.application>();
    builder.ConfigureSyncfusionCore();

    // Register the Window.
    builder.Services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(UIWindow), Window));
    MauiApp mauiApp = builder.Build();
    _mauiContext = new MauiContext(mauiApp.Services);
}
```
### Step 4: Initialize DataGrid
Now, let us define a simple data model representing a data in the DataGrid.

[C#]
```
public class OrderInfo
{
    public string OrderID { get; set; }

    public string CustomerID { get; set; }

    public string Customer {  get; set; }

    public string ShipCity {  get; set; }

    public string ShipCountry { get; set; }

    public OrderInfo(string orderId, string customerId, string country, string customer, string shipCity)
    {
        this.OrderID = orderId;
        this.CustomerID = customerId;
        this.Customer = customer;
        this.ShipCountry = country;
        this.ShipCity = shipCity;
    }
}
```

Next, create a view model class and initialize a Observable collection of OrderInfo objects.

[C#]
```
public class OrderInfoRepository
{
    private ObservableCollection<OrderInfo> orderInfo;
    public ObservableCollection<OrderInfo> OrderInfoCollection
    {
        get { return orderInfo; }
        set { this.orderInfo = value; }
    }

    public OrderInfoRepository()
    {
        orderInfo = new ObservableCollection<OrderInfo>();
        this.GenerateOrders();
    }

    public void GenerateOrders()
    {
        orderInfo.Add(new OrderInfo("1001", "Maria Anders", "Germany", "ALFKI", "Berlin"));
        orderInfo.Add(new OrderInfo("1002", "Ana Trujillo", "Mexico", "ANATR", "Mexico D.F."));
        orderInfo.Add(new OrderInfo("1003", "Ant Fuller", "Mexico", "ANTON", "Mexico D.F."));
        orderInfo.Add(new OrderInfo("1004", "Thomas Hardy", "UK", "AROUT", "London"));
        orderInfo.Add(new OrderInfo("1005", "Tim Adams", "Sweden", "BERGS", "London"));
        orderInfo.Add(new OrderInfo("1006", "Hanna Moos", "Germany", "BLAUS", "Mannheim"));
        orderInfo.Add(new OrderInfo("1007", "Andrew Fuller", "France", "BLONP", "Strasbourg"));
        orderInfo.Add(new OrderInfo("1008", "Martin King", "Spain", "BOLID", "Madrid"));
        orderInfo.Add(new OrderInfo("1009", "Lenny Lin", "France", "BONAP", "Marsiella"));
        orderInfo.Add(new OrderInfo("1010", "John Carter", "Canada", "BOTTM", "Lenny Lin"));
        orderInfo.Add(new OrderInfo("1011", "Laura King", "UK", "AROUT", "London"));
        orderInfo.Add(new OrderInfo("1012", "Anne Wilson", "Germany", "BLAUS", "Mannheim"));
        orderInfo.Add(new OrderInfo("1013", "Martin King", "France", "BLONP", "Strasbourg"));
        orderInfo.Add(new OrderInfo("1014", "Gina Irene", "UK", "AROUT", "London"));
    }
}
```
Bind the OrderInfoCollection property of the view model to the dataGrid.ItemsSource.

[C#]
```
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
     ...
        // - create the DataGrid control
        SfDataGrid datagrid = new SfDataGrid();
        OrderInfoRepository viewModel = new OrderInfoRepository();
        datagrid.ItemsSource = viewModel.OrderInfoCollection;
        datagrid.GridLinesVisibility = GridLinesVisibility.Both;
        datagrid.HeaderGridLinesVisibility = GridLinesVisibility.Both;
     ...
   }
}
```
### Step 5: Convert the .NET MAUI control to an ios View object
Converts the dataGrid into a UIView compatible with the Maui platform. Creates a UIViewController, adds the Maui dataGrid view to its subviews, and sets it as the root view controller of the main window. Makes the main window visible.

[C#]
```
public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
{
 ...
        UIView mauiView = dataGrid.ToPlatform(_mauiContext);
        mauiView.Frame = Window!.Frame;

        // create UIViewController 
        var vc = new UIViewController();
        vc.View!.AddSubview(mauiView);

        Window.RootViewController = vc;

        // make the window visible
        Window.MakeKeyAndVisible();

        return true;        
}
```
### OutPut

 ![iosScreenShot.png](https://support.syncfusion.com/kb/agent/attachment/inline?token=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjI2Nzk2Iiwib3JnaWQiOiIzIiwiaXNzIjoic3VwcG9ydC5zeW5jZnVzaW9uLmNvbSJ9.CE46ssVRAL8R_Gnn8dk20H0PVOzDkXiDb0mtzW1rSJ4)

[View sample in GitHub](https://github.com/SyncfusionExamples/Integrating-.NET-MAUI-DataGrid-with-ios-native-embedding-application)

Take a moment to explore this [documentation](https://help.syncfusion.com/maui/datagrid/overview), where you can find more information about Syncfusion .NET MAUI DataGrid (SfDataGrid) with code examples. Please refer to this [link](https://www.syncfusion.com/maui-controls/maui-datagrid) to learn about the essential features of Syncfusion .NET MAUI DataGrid (SfDataGrid).
 
##### Conclusion
 
I hope you enjoyed learning about ios Native embedding for the .NET MAUI DataGrid (SfDataGrid).
 
You can refer to our [.NET MAUI DataGrid’s feature tour](https://www.syncfusion.com/maui-controls/maui-datagrid) page to learn about its other groundbreaking feature representations. You can also explore our [.NET MAUI DataGrid Documentation](https://help.syncfusion.com/maui/datagrid/getting-started) to understand how to present and manipulate data. 
For current customers, you can check out our .NET MAUI components on the [License and Downloads](https://www.syncfusion.com/sales/teamlicense) page. If you are new to Syncfusion, you can try our 30-day [free trial](https://www.syncfusion.com/downloads/maui) to explore our .NET MAUI DataGrid and other .NET MAUI components.
 
If you have any queries or require clarifications, please let us know in the comments below. You can also contact us through our [support forums](https://www.syncfusion.com/forums), [Direct-Trac](https://support.syncfusion.com/create) or [feedback portal](https://www.syncfusion.com/feedback/maui?control=sfdatagrid), or the feedback portal. We are always happy to assist you!