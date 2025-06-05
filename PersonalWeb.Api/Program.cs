using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntityType<DefaultModel>();
    builder.EntitySet<Blog>("Blogs");
    builder.EntitySet<Experience>("Experiences");
    builder.EntitySet<Project>("Projects");
    return builder.GetEdmModel();
}

builder.Services.AddDbContext<PersonalWebApiContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<BlogRepositories>();
builder.Services.AddScoped<ExperienceRepositories>();
builder.Services.AddScoped<ProjectRepositories>();
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("v1", GetEdmModel())
        .Filter().Select().Expand()
        .SetMaxTop(100).OrderBy().Count());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseODataBatching();
app.UseRouting();

app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();

public partial class Program { }