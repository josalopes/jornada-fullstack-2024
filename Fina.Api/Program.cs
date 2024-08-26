
using Fina.Api.Data;
using Fina.Api.Handlers;
using Fina.Core.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //const string connectionString = "Server=localhost,1433;Database=fina;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False;TrustServerCertificate=True";
            const string connectionString = "Server=(local);Database=fina;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=true";

            //const string connectionString = "Server=localhost,3050;User=SYSDBA;Password=masterkey;Database=localhost:D:\\Projetos\\Balta\\Fina\\Fina.Api\\Data\\FINA.FDB";


            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));
            //builder.Services.AddDbContext<AppDbContext>(x => x.UseFirebird(connectionString));
            builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
            builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

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
