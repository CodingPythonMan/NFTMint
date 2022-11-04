using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using NFTMint.Services;
using NFTMint.Services.CryptoWallet.NethereumAPI;
using NFTMint.Services.MetaMask;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:2222");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<MetaMaskService>();
builder.Services.AddScoped<AccountSessionService>();
builder.Services.AddScoped<IMetamaskInterop, MetamaskBlazorInterop>();
builder.Services.AddScoped<MetamaskInterceptor>();
builder.Services.AddScoped<MetamaskHostProvider>();
builder.Services.AddScoped<IEthereumHostProvider>(serviceProvider =>
{
    return serviceProvider.GetService<MetamaskHostProvider>()!;
});

builder.Services.AddScoped<MyWalletService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
