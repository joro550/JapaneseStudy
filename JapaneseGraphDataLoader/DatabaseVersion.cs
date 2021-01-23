using Google.Cloud.Firestore;

namespace JapaneseGraphDataLoader
{
    [FirestoreData]
    public class DatabaseVersion
    {
        [FirestoreProperty]
        public string Identifier { get; init; }
    }
}