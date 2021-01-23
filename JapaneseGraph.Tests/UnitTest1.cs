using Xunit;
using GraphQL;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace JapaneseGraph.Tests
{
    public class UnitTest1 
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public UnitTest1(WebApplicationFactory<Startup> factory) 
            => _factory = factory;

        [Fact]
        public async Task Test1()
        {
            // Arrange
            var client = _factory.CreateClient();

            var radicalRequest = new GraphQLRequest {Query = "{radicals(level: 1) {character}}"};
            var response = await client.PostAsJsonAsync("/graphql", radicalRequest);
        }
    }
}