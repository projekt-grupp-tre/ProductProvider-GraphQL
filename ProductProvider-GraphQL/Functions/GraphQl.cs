using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ProductProvider_GraphQL.Functions
{
    public class GraphQl
    {
        private readonly ILogger<GraphQl> _logger;
        private readonly IGraphQLRequestExecutor _graphQLRequestExecutor;

        public GraphQl(ILogger<GraphQl> logger, IGraphQLRequestExecutor graphQLRequestExecutor)
        {
            _logger = logger;
            _graphQLRequestExecutor = graphQLRequestExecutor;
        }

        [Function("GraphQL")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processing GraphQL request.");

            var response = await _graphQLRequestExecutor.ExecuteAsync(req);
            return response;
        }
    }
}
