using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestingAuth
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
            services.AddAuthentication(nameof(SchemeOneHandler))
                .AddScheme<AuthenticationSchemeOptions, SchemeOneHandler>(nameof(SchemeOneHandler), o => { })
                .AddScheme<AuthenticationSchemeOptions, SchemeTwoHandler>(nameof(SchemeTwoHandler), o => { });          

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                         .RequireAuthenticatedUser()
                         .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.AllowCombiningAuthorizeFilters = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            app.UseMvc();
        }
    }

    public class SchemeOneHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public SchemeOneHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory factory, UrlEncoder encoder, ISystemClock clock) : base(options, factory, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Query.ContainsKey("One"))
            {
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(Scheme.Name));
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
        }
    }

    public class SchemeTwoHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public SchemeTwoHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory factory, UrlEncoder encoder, ISystemClock clock) : base(options, factory, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Query.ContainsKey("Two"))
            {
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(Scheme.Name));
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
        }
    }
}
