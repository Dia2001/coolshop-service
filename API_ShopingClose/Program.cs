using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API_ShopingClose.Service;
using MySqlConnector;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                                .AllowAnyOrigin()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

string conn = builder.Configuration.GetConnectionString("mysqlConnetionStrings");

// Mỗi khi chạy ứng dụng nó chỉ khởi tạo đúng 1 lần new UserDeptService
builder.Services.AddSingleton<UserDeptService>(s =>
    new UserDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<RoleDeptService>(s =>
    new RoleDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<ProductDeptService>(s =>
    new ProductDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<ProductInCategoryDeptService>(s =>
    new ProductInCategoryDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<ProductDetailsDeptService>(s =>
    new ProductDetailsDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<BrandDeptService>(s =>
    new BrandDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<CategoryDeptService>(s =>
    new CategoryDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<SizeDeptService>(s =>
    new SizeDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<ColorDeptService>(s =>
    new ColorDeptService(new MySqlConnection(conn)));

builder.Services.AddSingleton<GalleryDeptService>(s =>
    new GalleryDeptService(new MySqlConnection(conn)));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
//
app.UseHttpsRedirection();

// Public thư mục wwwroot
app.UseStaticFiles();
//
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
// Khi chưa cầu hình dùng js để gọi API nó sẽ báo lỗi CORS vì js cùng là localhost nhưng khác port nên nó sẽ không gọi được API của backend
// Cần phải cấu hình để bỏ qua việc chặn CORS đó 
