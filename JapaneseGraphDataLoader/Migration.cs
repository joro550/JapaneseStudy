using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace JapaneseGraphDataLoader
{
    public abstract class Migration
    {
        protected abstract string Identifier { get; }

        public async Task RunMigration(FirestoreDb db)
        {
            var versionCollection = db.Collection("versions");
            var snapshot = await versionCollection.GetSnapshotAsync();
            var versions = snapshot.Documents.Select(document => document.ConvertTo<DatabaseVersion>()).ToList();

            if (versions.All(c => c.Identifier != Identifier))
            {
                await AddData(db);
            }

            await versionCollection.AddAsync(new DatabaseVersion {Identifier = Identifier});
        }

        protected abstract Task AddData(FirestoreDb firestoreDb);
    }
}