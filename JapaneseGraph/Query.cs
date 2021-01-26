using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JapaneseGraph
{
    public class Query
    {
        private readonly IMediator _mediator;

        public Query(IMediator mediator) 
            => _mediator = mediator;

        public async Task<IEnumerable<Radical>> GetRadicals(int level) 
            => await _mediator.Send(new GetRadicalRequest {Level = level});
    }

    public class GetRadicalRequest : IRequest<IEnumerable<Radical>>
    {
        public int Level { get; init; }
    }


    public class GetLevel : IRequestHandler<GetRadicalRequest, IEnumerable<Radical>>
    {
        private readonly FirebaseFactory _factory;

        public GetLevel(FirebaseFactory factory) 
            => _factory = factory;

        public async Task<IEnumerable<Radical>> Handle(GetRadicalRequest request, CancellationToken cancellationToken)
        {
            var store = await _factory.Build();
            var snapshot = await store.Collection("radicals").GetSnapshotAsync(cancellationToken);
            var versions = snapshot.Documents.Select(document => document.ConvertTo<Shared.Radical>()).ToList();

            return versions.Select(version => new Radical
                {
                    Character = version.Character,
                    Description = version.Description,
                    Mnemonic = version.Mnemonic,
                    KunYomi = version.KunYomi,
                    OnYomi = version.OnYomi
                });
        }
    }

    public class Radical
    {
        public string Character { get; init; }
        
        public string Description { get; init; }
        
        public string Mnemonic { get; init; }
        
        public string[] OnYomi { get; init; }
        
        public string[] KunYomi { get; init; }
    }
}