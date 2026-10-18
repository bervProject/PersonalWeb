using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PersonalWeb.Api;
using PersonalWeb.Api.EntityFramework;
using PersonalWeb.Api.Models;
using PersonalWeb.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder edmBuilder = new();
    edmBuilder.EntityType<DefaultModel>();
    edmBuilder.EntitySet<Blog>("Blogs");
    edmBuilder.EntitySet<Education>("Education");
    edmBuilder.EntitySet<Experience>("Experiences");
    edmBuilder.EntitySet<Project>("Projects");
    edmBuilder.EntitySet<Profile>("Profiles");
    edmBuilder.EntitySet<Engagement>("Engagements");
    edmBuilder.EntitySet<Publication>("Publications");
    return edmBuilder.GetEdmModel();
}

builder.Services.AddDbContext<PersonalWebApiContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<BlogRepositories>();
builder.Services.AddScoped<EducationRepositories>();
builder.Services.AddScoped<ExperienceRepositories>();
builder.Services.AddScoped<ProjectRepositories>();
builder.Services.AddScoped<ProfileRepositories>();
builder.Services.AddScoped<EngagementRepositories>();
builder.Services.AddScoped<PublicationRepositories>();

var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is not configured. Add it to user secrets or environment variables.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("v1", GetEdmModel())
        .Filter().Select().Expand()
        .SetMaxTop(100).OrderBy().Count());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

DataSeeder.SeedAdminUser(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseODataBatching();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();

public partial class Program { }
