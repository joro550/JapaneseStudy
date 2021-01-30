using System.Threading.Tasks;
using Google.Cloud.Firestore;
using JapaneseGraph.Shared;

namespace JapaneseGraphDataLoader.Levels
{
    public class Level1 : Migration
    {
        protected override string Identifier =>  "Level1";

        protected override async Task AddData(FirestoreDb firestoreDb)
        {
            var collection = firestoreDb.Collection("radicals");
            
            await collection.AddAsync(new Radical
            {
                Character = "日", 
                Description = "This characterじち represents the sun or days",
                OnYomi = new[] {"にち", "じつ"},
                KunYomi = new [] {"ひ", "か"},
                Mnemonic = "",
                Level = 1
            });
        }
    }
}