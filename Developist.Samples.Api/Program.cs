using Developist.Core.Cqrs.Commands;
using Developist.Core.Cqrs.Infrastructure.DependencyInjection;
using Developist.Core.Persistence.InMemory.DependencyInjection;
using Developist.Extensions.Api.MvcFilters;
using Developist.Extensions.Cqrs.Infrastructure.DependencyInjection;
using Developist.Extensions.Persistence.DependencyInjection;
using Developist.Samples.Api.Filters;
using Developist.Samples.Application.Commands;
using Developist.Samples.Application.Queries;
using Developist.Samples.Domain.Entities;
using Developist.Samples.Infrastructure;
using Developist.Samples.Infrastructure.Persistence;

using Microsoft.OpenApi.Models;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

void ConfigureServices(IServiceCollection services)
{
    services.AddOptions<ApiExceptionFilterOptions>().Configure(options =>
    {
        var defaultOnSerializingAction = options.OnSerializingProblemDetails;
        options.OnSerializingProblemDetails = (prob, ex, ctx) =>
        {
            defaultOnSerializingAction(prob, ex, ctx);

            if (ex.InnerException is { } innerException)
            {
                prob.Extensions["innerExceptionMessage"] = innerException.Message;
            }
        };
    });
    services.AddScoped<ApiExceptionFilterAttribute, GlobalExceptionFilterAttribute>();

    services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilterAttribute>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    });

    services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Sample Web API",
            Version = "v1"
        });
        options.DescribeAllParametersInCamelCase();
    });

    services.AddScoped<IEventAggregator, EventAggregator>();
    services.AddUnitOfWork().WrapUnitOfWork((uow, provider) => ActivatorUtilities.CreateInstance<CustomUnitOfWork>(provider, uow));

    services.AddCqrs(builder => builder
        .AddCompositeDispatcher()
        .AddHandlersFromAssembly(Assembly.Load("Developist.Samples"))
        .AddQueryInterceptor<GetAllUsers, IReadOnlyList<User>>(async (query, next, provider, token) =>
        {
            await provider.GetRequiredService<ICommandDispatcher>().DispatchAsync(new SeedUserData(), token);
            return await next();
        })
        .AddQueryInterceptor<GetUserByUserName, User?>(async (query, next, provider, token) =>
        {
            await provider.GetRequiredService<ICommandDispatcher>().DispatchAsync(new SeedUserData(), token);
            return await next();
        })
        .AddCommandInterceptor<AssignRoleToUser>(async (command, next, provider, token) =>
        {
            await provider.GetRequiredService<ICommandDispatcher>().DispatchAsync(new SeedUserData(), token);
            await next();
        })
    );
}

void ConfigureApplication(IApplicationBuilder app, IHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"v1/swagger.json", "Sample Web API v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

#region Bootstrap the application

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureApplication(app, app.Environment);

app.Run();

#endregion