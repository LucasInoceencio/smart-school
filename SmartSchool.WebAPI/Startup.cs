using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartSchool.WebAPI.Data;

namespace SmartSchool.WebAPI
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
            services.AddDbContext<SmartContext>(
                context => context.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
            );

            #region Repository utilizando Singleton
            // Cria uma única instância do serviço quando é solicitado pela primeira vez e reutiliza 
            // essa mesma instância em todos os locais em que esse serviço é necessário
            //services.AddSingleton<IRepository, Repository>();
            #endregion

            #region Repository utilizando Transient
            // Sempre gerará uma nova instância para cada item encontrado que possua tal dependência,
            // ou seja, se houver 5 dependências serão 5 instâncias diferentes
            //services.AddTransient<IRepository, Repository>();
            #endregion

            #region Repository utilizando Scoped
            // Essa é diferente da Transiente que garante que em uma requisição seja criada uma instância
            // de uma classe onde se houver outras dependências, seja utilizada essa única instância pra todas,
            // renovando somente nas requisições subsequentes mas mantendo essa obrigatoriedade
            // É utilizado a mesma instância para mesma requisição, independente se tem dependências de outros
            // objetos
            services.AddScoped<IRepository, Repository>();
            #endregion

            // Configurando versionamento da API
            services.AddVersionedApiExplorer(options => 
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options => 
            {
                options.DefaultApiVersion = new ApiVersion(1,0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // Configurando Newtonsoft para resolver o problema de referência cíclica 
            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Configurando AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Configurando o Swagger com mais de uma versão
            var apiVersionDescriptionProvider = services.BuildServiceProvider()
                .GetService<IApiVersionDescriptionProvider>();
            
            services.AddSwaggerGen(c =>
            {
                foreach(var description in apiVersionDescriptionProvider.ApiVersionDescriptions){
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo 
                    { 
                        Title = "SmartSchool.WebAPI", 
                        Version = description.ApiVersion.ToString(),
                        TermsOfService = new Uri("https://www.google.com/"),
                        Description = "Uma bela de uma descrição para ninguém encher o saco depois.",
                        License = new OpenApiLicense
                        {
                            Name = "Nome da licensa",
                            Url = new Uri("https://www.google.com/")
                        },
                        Contact = new OpenApiContact
                        {
                            Name = "Lucas Inocencio Pires",
                            Email = "piresilucas@gmail.com",
                            Url = new Uri("https://www.github.com/LucasInoceencio")
                        }
                    });
                    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                    c.IncludeXmlComments(xmlCommentsFullPath);
                    }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
