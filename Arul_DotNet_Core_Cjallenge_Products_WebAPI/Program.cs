using Microsoft.EntityFrameworkCore;
using DAL;
using BAL;
using Newtonsoft;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<ProductDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ProductChallengeConnection")));

builder.Services.AddScoped<IProductBAL, ProductBAL>();
builder.Services.AddScoped<IProductDAL,ProductDAL>();
builder.Services.AddScoped<IApprovalQueueBAL, ApprovalQueueBAL>();
builder.Services.AddScoped<IApprovalQueueDAL, ApprovalQueueDAL>();

builder.Services.AddControllers().AddNewtonsoftJson(options => { 
    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Register middleware for exception handling
app.UseMiddleware<DotNetCore_WebAPI.Middlewares.ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
