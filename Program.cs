using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("ProjetCS");
builder.Services.AddDbContext<OffreDbContext>(options =>
    options.UseSqlServer(connectionString)
           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Candidate/Login"; // Adjust the login path as needed
    });



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
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

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Routes for Recruteurs
app.MapControllerRoute(
    name: "recruteurLogin",
    pattern: "Recruteur/Login",
    defaults: new { controller = "Recruteurs", action = "Login" });

app.MapControllerRoute(
    name: "recruteurDashboard",
    pattern: "Recruteur/Welcome",
    defaults: new { controller = "Recruteurs", action = "Welcome" }
);

// CRUD Routes for Recruteurs
app.MapControllerRoute(
    name: "recruteurIndex",
    pattern: "Recruteur/Index",
    defaults: new { controller = "Recruteurs", action = "Index" });

app.MapControllerRoute(
    name: "recruteurDetails",
    pattern: "Recruteur/Details/{id?}",
    defaults: new { controller = "Recruteurs", action = "Details" });

app.MapControllerRoute(
    name: "recruteurCreate",
    pattern: "Recruteur/Create",
    defaults: new { controller = "Recruteurs", action = "Create" });

app.MapControllerRoute(
    name: "recruteurEdit",
    pattern: "Recruteur/Edit/{id?}",
    defaults: new { controller = "Recruteurs", action = "Edit" });

app.MapControllerRoute(
    name: "recruteurDelete",
    pattern: "Recruteur/Delete/{id?}",
    defaults: new { controller = "Recruteurs", action = "Delete" });

app.MapControllerRoute(
    name: "candidateLogin",
    pattern: "Candidate/Login",
    defaults: new { controller = "Candidates", action = "Login" });


app.MapControllerRoute(
    name: "candidateDashboard",
    pattern: "Candidate/Welcome",
    defaults: new { controller = "Candidate", action = "Welcome" }
);

// CRUD Routes for Candidates
app.MapControllerRoute(
    name: "candidateIndex",
    pattern: "Candidate/Index",
    defaults: new { controller = "Candidate", action = "Index" });

app.MapControllerRoute(
    name: "candidateDetails",
    pattern: "Candidate/Details/{id?}",
    defaults: new { controller = "Candidate", action = "Details" });

app.MapControllerRoute(
    name: "candidateCreate",
    pattern: "Candidate/Create",
    defaults: new { controller = "Candidate", action = "Create" });

app.MapControllerRoute(
    name: "candidateEdit",
    pattern: "Candidate/Edit/{id?}",
    defaults: new { controller = "Candidate", action = "Edit" });

app.MapControllerRoute(
    name: "candidateDelete",
    pattern: "Candidate/Delete/{id?}",
    defaults: new { controller = "Candidate", action = "Delete" });



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "candidateRoute",
        pattern: "{controller=Candidates}/{action=JobStatus}/{id?}");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "recruteurViewJobs",
        pattern: "Recruteurs/ViewJobs",
        defaults: new { controller = "Recruteurs", action = "ViewJobs" }
    );
});
app.MapControllerRoute(
    name: "candidateCancelApplication",
    pattern: "Candidates/CancelApplication",
    defaults: new { controller = "Candidates", action = "CancelApplication" }
);



app.MapControllerRoute(
    name: "CandidateJobsEdit",
    pattern: "CandidateJobs/Edit/{id?}",
    defaults: new { controller = "CandidateJobs", action = "Edit" });


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "recruteurChangeApplicationStatus",
        pattern: "Recruteurs/ChangeApplicationStatus",
        defaults: new { controller = "Recruteurs", action = "ChangeApplicationStatus" }
    );
});
app.Run();
