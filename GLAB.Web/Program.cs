using GLAB.App.Laboratories;
using GLAB.App.Teams;
using GLAB.App.Memebers;
using GLAB.Implementation.Services.Laboratories;
using GLAB.Implementation.Services.Teams;
using GLAB.Implementation.Services.Members;
using System.Transactions;
using GLAB.Infra.Storages.LaboratoriesStorages;
using GLAB.Infra.Storages.MembersStorages;
using GLAB.Infra.Storages.TeamsStorages;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Register storage dependencies
builder.Services.AddScoped<ILaboratoryStorage, LaboratoryStorage>();
builder.Services.AddScoped<ITeamStorage, TeamStorage>();
builder.Services.AddScoped<IMemberStorage, MemberStorage>();

builder.Services.AddScoped<ILabService, LabService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMemberService, MemberService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
