using Bramble.Service.Interface;
using Bramble.Service.Service;
using BrambleDAL;
using Microsoft.AspNetCore.Http.Features;
using Bramble.Framework.EnumeratedType;
using Microsoft.AspNetCore.Identity;
using Bramble.Models.GenericModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BrambleWeb.Data;
using Google;
using static BrambleWeb.Data.BrambleDBContext;
using Bramble.Framework;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BrambleDB") ?? throw new InvalidOperationException("Connection string 'BrambleDB' not found.");

// Add services to the container.
#region Bramble Services
builder.Services.AddScoped<IBrambleSeedService, BrambleSeedService>();
builder.Services.AddScoped<IBrambleConfig, BrambleConfig>();
builder.Services.AddScoped<IBrambleDB, BrambleDB>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<ICommonDBService, CommonDBService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IYoutubeApiService, YoutubeApiService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

#endregion

#region Form Options
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<FormOptions>(x => x.ValueLengthLimit = FileSizes.Audio);
builder.Services.Configure<FormOptions>(x => x.MultipartBoundaryLengthLimit = FileSizes.Audio);
builder.Services.Configure<FormOptions>(x => x.MultipartBodyLengthLimit = FileSizes.Audio);
builder.Services.Configure<FormOptions>(x => x.MemoryBufferThreshold = FileSizes.Audio);
builder.Services.Configure<FormOptions>(x => x.MultipartHeadersLengthLimit = FileSizes.Audio);

#endregion

#region General Options
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(600);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddMvc(options =>
{
	options.Filters.Add(typeof(BrambleWeb.Controllers.BaseController.ViewBagFilter));
});
#endregion

#region Identity Options

var customPasswordOptions = new PasswordOptions();

#if DEBUG

customPasswordOptions = new PasswordOptions()
{
	RequiredLength = 0,
	RequireNonAlphanumeric = false,
	RequireDigit = false,
	RequiredUniqueChars = 0,
	RequireLowercase = false,
	RequireUppercase = false
};

#endif


builder.Services.AddDbContext<BrambleWeb.Data.BrambleDBContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<BrambleClaimsFactory>();
builder.Services
	.AddIdentity<UserModel, RoleModel>(options => options.Password = customPasswordOptions)
	.AddEntityFrameworkStores<BrambleWeb.Data.BrambleDBContext>()
	.AddClaimsPrincipalFactory<BrambleClaimsFactory>()
	.AddDefaultTokenProviders();
builder.Services.AddScoped<IAccountService, AccountService>();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller}/{action}/{id?}"
	,
	defaults: new { controller = "Account", action = "Login" }
	);
app.Run();
