using System;
using System.Threading.Tasks;
using Google.Api.Gax;
using Google.Cloud.Firestore;
using JapaneseGraph.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JapaneseGraph
{
    public class FirebaseFactory
    {
        private readonly FirebaseConfig _firebaseOptions;
        private readonly ILogger<FirebaseFactory> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public FirebaseFactory(IOptions<FirebaseConfig> firebaseOptions, 
            IHostEnvironment hostEnvironment, ILogger<FirebaseFactory> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _firebaseOptions = firebaseOptions.Value;
        }

        public async Task<FirestoreDb> Build()
        {
            var isDev = _hostEnvironment.IsDevelopment();
            _logger.LogInformation($"Is Dev: {isDev}");
            if (!isDev)
            {
                try
                {
                    _logger.LogInformation($"Project {_firebaseOptions.ProjectId}");
                    return await FirestoreDb.CreateAsync(_firebaseOptions.ProjectId);
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Exception happened {e.Message}");
                    _logger.LogError(e.Message, e);
                }
            }
            
            var firestoreDbBuilder = new FirestoreDbBuilder
            {
                ProjectId = _firebaseOptions.ProjectId, 
                EmulatorDetection = EmulatorDetection.EmulatorOrProduction
            };
            return await firestoreDbBuilder.BuildAsync();
        } 
    }
}