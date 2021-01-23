using System.Linq;
using System.Threading.Tasks;
using Google.Api.Gax;
using Google.Cloud.Firestore;
using Grpc.Core;
using JapaneseGraph.Shared;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace JapaneseGraph.Tests
{
    public class FirestoreTests
    {
        [Fact]
        public async Task Test1()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(UnitTest1).Assembly)
                .Build();

            var configSection = configuration.GetSection(nameof(FirebaseConfig));
            var projectId = configSection[nameof(FirebaseConfig.ProjectId)];
            
            var firestoreDbBuilder = new FirestoreDbBuilder
            {
                ProjectId = projectId, 
                ChannelCredentials = ChannelCredentials.Insecure,
                EmulatorDetection = EmulatorDetection.EmulatorOrProduction
            };
            
            var db = await firestoreDbBuilder.BuildAsync();
            var snapshot = await db.Collection("radicals").GetSnapshotAsync();
            var versions = snapshot.Documents.Select(document => document.ConvertTo<Shared.Radical>()).ToList();

        } 
    }
}