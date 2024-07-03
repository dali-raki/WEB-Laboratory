using Glab.App.Grades;
using Glab.Implementation.Services.Grades;
using Glab.Infrastructures.Storages.GradesStorages;
using Glab.Infrastructures.Storages.LaboratoriesStorages;
using Glab.Infrastructures.Storages.RolesStorages;
using GLAB.App.Accounts;
using GLAB.App.Laboratories;
using GLAB.App.Memebers;
using GLAB.App.Teams;
using GLAB.Implementation.Accounts;
using GLAB.Implementation.Services.Laboratories;
using GLAB.Implementation.Services.Members;
using GLAB.Implementation.Services.Teams;
using GLAB.Infra.Storages.LaboratoriesStorages;
using GLAB.Infra.Storages.MembersStorages;
using GLAB.Infra.Storages.TeamsStorages;
using GLAB.App.Users; // Assuming the IUserService is in this namespace
using GLAB.Implementation.Users; // Assuming the UserService implementation is in this namespace

using GLAB.Web1.Components;
using Users.Infra.Storages;
using Email.App;
using Email.Implementation;
using Glab.Ui.Members;
using System.Transactions;
using Glab.App.Roles;
using Glab.Implementation.Services.Roles;
using Glab.Infrastructures.Storages.FacultiesStorages;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILaboratoryStorage, LaboratoryStorage>();
builder.Services.AddScoped<ITeamStorage, TeamStorage>();
builder.Services.AddScoped<IMemberStorage, MemberStorage>();
builder.Services.AddScoped<IUserStorage, UserStorage>();
builder.Services.AddScoped<IRoleStorage, RoleStorage>();
builder.Services.AddScoped<IGradeStorage, GradeStorage>();
builder.Services.AddScoped<IFacultyStorage, FacultyStorage>();



builder.Services.AddScoped<IAccount,AccountService>();
builder.Services.AddScoped<ILabService, LabService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IRoleService, RoleService>();


builder.Services.AddControllers();




// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options => {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan=TimeSpan.FromHours(20);
        options.Cookie.Name = "Glab";
    });



if (OperatingSystem.IsWindows()) { TransactionManager.ImplicitDistributedTransactions = true;}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();
