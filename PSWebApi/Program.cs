using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 1);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new QueryStringApiVersionReader("V");
});
builder.Services.AddDbContext<CampContext>();
builder.Services.AddScoped<ICampRepository, CampRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
