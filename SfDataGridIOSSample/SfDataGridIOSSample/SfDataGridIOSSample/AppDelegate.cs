using Foundation;
using Microsoft.Maui.Embedding;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.DataGrid;
using UIKit;

namespace SfDataGridIOSSample;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
        // create a new window instance based on the screen size.
        Window = new UIWindow(UIScreen.MainScreen.Bounds);
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiEmbedding<Microsoft.Maui.Controls.Application>();
        builder.ConfigureSyncfusionCore();
        // Register the Window.
        builder.Services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(UIWindow), Window));
        MauiApp mauiApp = builder.Build();
        var _mauiContext = new MauiContext(mauiApp.Services);

        // create the DataGrid control
        SfDataGrid dataGrid = new SfDataGrid();
        OrderInfoRepository viewModel = new OrderInfoRepository();
        dataGrid.ItemsSource = viewModel.OrderInfoCollection;
        dataGrid.GridLinesVisibility = GridLinesVisibility.Both;
        dataGrid.HeaderGridLinesVisibility = GridLinesVisibility.Both;

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
}

