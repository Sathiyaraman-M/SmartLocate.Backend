using Microsoft.OpenApi.Models;
using SmartLocate.Buses.Entities;
using SmartLocate.Infrastructure.Commons.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.AddMongoDBClient("MongoDbConnection");

builder.Services.AddMongoRepository<Bus>();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddDaprClient();

builder.Services.SetupDaprSidekick(builder.Configuration);

builder.Services.AddControllers().AddDapr();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartLocate.Buses", Version = "v1" });
    x.AddSecurityDefinitionAndRequirement();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartLocate.Buses.v1"));

await app.RunAsync();