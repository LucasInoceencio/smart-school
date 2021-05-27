using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
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
                context => context.UseSqlite(Configuration.GetConnectionString("Default"))
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartSchool.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSchool.WebAPI v1"));
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
