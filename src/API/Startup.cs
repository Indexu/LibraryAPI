using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Repositories.EntityFrameworkCore;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LibraryAPI.API
{
    /// <summary>
    /// The setup configurations for the API
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
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
            // Service options
            services.AddMvc();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddAutoMapper();
            services.AddApiVersioning(x =>
            {
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Dependency injection info
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IBookService, BookService>();

            services.AddTransient<ILoanRepository, LoanRepository>();

            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IReviewService, ReviewService>();

            services.AddTransient<IReportingService, ReportingService>();
            services.AddTransient<IRecommendationService, RecommendationService>();

            // Database info
            services.AddDbContext<DatabaseContext>(options =>
                options
                    .UseSqlite("Data Source=../Repositories/LibraryAPI.db", db => db.MigrationsAssembly("API"))
                    .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning))
                    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
