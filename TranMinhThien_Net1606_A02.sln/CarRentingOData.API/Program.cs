using CarRenting.DTOs;
using CarRentingOData.BOs;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddOData(o => o.Select().Filter()
            .Count().OrderBy().Expand().SetMaxTop(100)
            .AddRouteComponents("odata", GetEdmModel()));
builder.Services.AddRouting();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseODataBatching();
app.Use((context, next) =>
{
    var endpoints = context.GetEndpoint();
    if (endpoints == null)
    {
        return next();
    }

    IEnumerable<string> templates;
    IODataRoutingMetadata metadata = endpoints.Metadata.GetMetadata<IODataRoutingMetadata>();

    if (metadata != null)
    {
        templates = metadata.Template.GetTemplates();
    }

    return next();
});

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    //endpoints.MapODataRoute("odata", "odata", GetEdmModel());
    endpoints.MapControllers();
});
app.MapControllers();

app.Run();
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<CustomerDto>("Customers");
    return builder.GetEdmModel();
}