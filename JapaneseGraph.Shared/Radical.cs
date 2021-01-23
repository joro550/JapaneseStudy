using Google.Cloud.Firestore;

namespace JapaneseGraph.Shared
{
    [FirestoreData]
    public class Radical
    {
        [FirestoreProperty]
        public string Character { get; init; }
        
        [FirestoreProperty]
        public string Description { get; init; }
        
        [FirestoreProperty]
        public string Mnemonic { get; init; }
        
        [FirestoreProperty]
        public string[] OnYomi { get; init; }
        
        [FirestoreProperty]
        public string[] KunYomi { get; init; }
        
        public int Level { get; init; }
    }
}