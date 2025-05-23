using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using TennisStoreClient;
using TennisStoreClient.Authentication;
using TennisStoreClient.Interfaces;
using TennisStoreClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register Syncfusion License for the Client
SyncfusionLicenseProvider
    .RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH5eeHRTRGNZVUR+XkM=");

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IUserAccountService, AccountService>();
builder.Services.AddScoped<ICart, CartService>();




builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<MessageDialogService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
