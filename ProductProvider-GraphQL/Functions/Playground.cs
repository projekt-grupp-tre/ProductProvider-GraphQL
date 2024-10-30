using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ProductProvider_GraphQL.Functions
{
    public class Playground
    {
        private readonly ILogger<Playground> _logger;

        public Playground(ILogger<Playground> logger)
        {
            _logger = logger;
        }

        [Function("Playground")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Serving GraphQL Playground.");

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            await response.WriteStringAsync(PlaygroundPage());
            return response;
        }

        // Metod för att generera HTML-sidan för GraphQL Playground
        private string PlaygroundPage()
        {
            return @"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>HotChocolate GraphQL Playground</title>
                    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/static/css/index.css"" />
                    <link rel=""shortcut icon"" href=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/favicon.png"" />
                    <script src=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/static/js/middleware.js""></script>
                </head>
                <body>
                    <div id=""root""></div>
                    <script>
                        window.addEventListener('load', function (event) {
                            GraphQLPlayground.init(document.getElementById('root'), {
                                endpoint: '/api/graphql'
                            })
                        })
                    </script>
                </body>
                </html>
                ";
        }
    }
}
