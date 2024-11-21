using System;
using System.IO;
using System.Text;
using Net.CrossCotting;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace Net.Business.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureIISIntegration();
            services.ConfigureSQLConnection();
            services.ConfigureHttpClientServiceLayer();

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            //Autenticacion
            services.Configure<ParametrosTokenConfig>(Configuration.GetSection("ParametrosTokenConfig"));

            string semilla = Configuration.GetSection("ParametrosTokenConfig").GetValue<string>("Semilla");
            string emisor = Configuration.GetSection("ParametrosTokenConfig").GetValue<string>("Emisor");
            string destinatario = Configuration.GetSection("ParametrosTokenConfig").GetValue<string>("Destinatario");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(semilla));

            services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = emisor,
                        ValidAudience = destinatario,
                        IssuerSigningKey = key
                    };

                });

            services.ConfigureRepositoryWrapper();

            /*De aqui en adelante configuracion de documentacion de nuestra API*/
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiFibrafil", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API Fibrafil",
                    Version = "1",
                    Description = "BackEnd Fibrafil",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "nflorespizango@gmail.com",
                        Name = "Fibrafil",
                        Url = new Uri("https://www.linkedin.com/company/grupo-fibrafil-per%C3%BA/?originalSubdomain=pe/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/nerio-flores-pizango/")
                    }
                });


                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);

                options.IncludeXmlComments(rutaApiComentarios);

                /*Primero definir el esquema de Fibrafil*/
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Autenticacion JWT (Bearer)",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Id = "Bearer",
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            //services.AddControllers();

            /* Configuraci�n para EntityFramework */
            services.ConfigureCors(Configuration);
            services.AddControllers();
            // Se agrega al servicio el AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // Se lee la cadena de conexion 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.ConfigureExceptionHandler();

            //Linea para documentacion api
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/ApiFibrafil/swagger.json", "API Fibrafil");
                options.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
