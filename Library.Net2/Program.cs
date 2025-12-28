using Library.Net2.Data;
using Library.Net2.Repositories;
using Library.Net2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL DateTime Timezone Fix - UTC zorla
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Fix wwwroot path - go back from bin/Debug/net8.0 to project root
var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
var wwwrootPath = Path.Combine(projectRoot, "wwwroot");

if (Directory.Exists(wwwrootPath))
{
    builder.Environment.WebRootPath = wwwrootPath;
    Console.WriteLine($"[INFO] WebRootPath set to: {wwwrootPath}");
}
else
{
    Console.WriteLine($"[WARNING] wwwroot not found at: {wwwrootPath}");
}

// Database Configuration - PostgreSQL
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository & Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILoanService, LoanService>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Database Migration and Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LibraryDbContext>();
        
        // FIX: Make DueDate nullable before migrations
        try
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            await using var connection = new Npgsql.NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = @"ALTER TABLE ""Loans"" ALTER COLUMN ""DueDate"" DROP NOT NULL;";
            await command.ExecuteNonQueryAsync();
            Console.WriteLine("✅ DueDate kolonu nullable yapıldı!");
        }
        catch (Npgsql.PostgresException ex) when (ex.SqlState == "42P01")
        {
            // Table doesn't exist yet, this is fine
            Console.WriteLine("ℹ️ Loans tablosu henüz yok, migration ile oluşturulacak.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ DueDate fix uyarısı: {ex.Message}");
        }
        
        await context.Database.MigrateAsync();
        await context.Database.MigrateAsync();
        await DbSeeder.SeedAsync(context);
        await DbSeeder.ForceUpdateImagesAsync(context); // Force update images for existing data
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}


// Swagger - always enabled
app.UseSwagger();
app.UseSwaggerUI();

// Static Files - index.html için (CORS'dan önce!)
var wwwrootFullPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot"));
var staticFileOptions = new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(wwwrootFullPath),
    RequestPath = ""
};

var defaultFilesOptions = new DefaultFilesOptions
{
    FileProvider = staticFileOptions.FileProvider
};

app.UseDefaultFiles(defaultFilesOptions);
app.UseStaticFiles(staticFileOptions);

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.Run();