using System.Threading.Tasks;
using Google.Api.Gax;
using Google.Cloud.Firestore;
using Grpc.Core;
using JapaneseGraph.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace JapaneseGraph
{
    public class FirebaseFactory
    {
        private readonly FirebaseConfig _firebaseOptions;
        private readonly IHostEnvironment _hostEnvironment;

        public FirebaseFactory(IOptions<FirebaseConfig> firebaseOptions, 
            IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _firebaseOptions = firebaseOptions.Value;
        }

        public async Task<FirestoreDb> Build()
        {
            FirestoreDbBuilder firestoreDbBuilder;
            
            if (_hostEnvironment.IsDevelopment())
            {
                firestoreDbBuilder = new FirestoreDbBuilder
                {
                    ProjectId = _firebaseOptions.ProjectId, 
                    EmulatorDetection = EmulatorDetection.EmulatorOrProduction
                };
            }
            else
            {
                firestoreDbBuilder = new FirestoreDbBuilder {ProjectId = _firebaseOptions.ProjectId};
            }
            
            return await firestoreDbBuilder.BuildAsync();
        } 
    }
}