using HotChocolate.AzureFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductProvider.Infrastructure.Data.Contexts;
using ProductProvider.Infrastructure.GraphQL.Mutations;
using ProductProvider.Infrastructure.GraphQL.ObjectTypes;
using ProductProvider.Infrastructure.GraphQL.Queries;
using ProductProvider.Infrastructure.Services;


var host = new HostBuilder()
	.ConfigureFunctionsWebApplication()
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();

		services.AddDbContext<DataContext>(options =>
			options.UseSqlServer(Environment.GetEnvironmentVariable("Productdb"))
           .UseLazyLoadingProxies());

		services.AddScoped<ProductService>();

		services
			.AddGraphQLFunction()
			.AddQueryType<ProductQuery>()
			.AddMutationType<ProductMutation>()
			.AddType<ProductType>()
			.AddType<CategoryType>()
			.AddType<ProductVariantType>()
			.AddType<ReviewType>();
	})
	.Build();

host.Run();
