using Xunit;
using System;
using Grpc.Core;
using System.Linq;
using Google.Api.Gax;
using JapaneseGraph.Shared;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace JapaneseGraphDataLoader
{
    public class UnitTest1
    {
        // [Fact]
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
            var migrations = FindSubClassesOf<Migration>();

            var tasks = new List<Task>();
            foreach (var migrationType in migrations)
            {
                var migration = Activator.CreateInstance(migrationType) as Migration;
                tasks.Add(migration?.RunMigration(db));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {   
            var baseType = typeof(TBaseType);
            var assembly = baseType.Assembly;
            return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
        }
    }
}