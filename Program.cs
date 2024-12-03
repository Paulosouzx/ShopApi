using Microsoft.AspNetCore.Authentication.JwtBearer; // Adicionar este using
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopsApi.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace ShopsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddControllers();
            builder.Services.AddJwtAuthentication(Settings.Secret);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
         
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();


        }
    }
}
