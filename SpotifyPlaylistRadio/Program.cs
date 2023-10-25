using SpotifyPlaylistRadio.Hubs;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //ConfigurationManager configuration = builder.Configuration;

        // Add services to the container.

        builder.Services.AddHttpClient<ISpotifyAccountService, SpotifyAccountService>(c =>
        {
            c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
        });

        builder.Services.AddHttpClient<ISpotifyService, SpotifyService>(c =>
        {
            c.BaseAddress = new Uri("https://api.spotify.com/v1/");
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.Configure<SpotifyPlaylistsFromRadioDatabaseSettings>(
            builder.Configuration.GetSection("MongoDB"));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ClientPermission", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000");
                //.AllowCredentials();
            });
        });

        builder.Services.AddSignalR();

        builder.Services.AddSingleton<ISearchHelperService, SearchHelperService>();
        builder.Services.AddScoped<IInitService, InitService>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton <IMessageWriter,MessageWriter>();
        builder.Services.AddSingleton<RadiosService>();
        builder.Services.AddSingleton<PlaylistService>();
        builder.Services.AddSingleton<MusicSpotifyService>();
        builder.Services.AddSingleton<ArtistService>();


        builder.Services.AddControllers();
        //.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors("ClientPermission");

        using (var serviceScope = app.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var init = services.GetRequiredService<IInitService>();

            //Use the service
            init.something();
        }

        app.UseRouting();

        app.UseHttpsRedirection();

        app.MapHub<ChatHub>("chatHub");

        app.MapControllers();
        app.Run();

    }
}