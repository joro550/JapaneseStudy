using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Grpc.Core.Logging;

namespace JapaneseGraph
{
    public class Query
    {
        private readonly IMediator _mediator;

        public Query(IMediator mediator) 
            => _mediator = mediator;

        public async Task<IEnumerable<Radical>> GetRadicals(int level) 
            => await _mediator.Send(new GetRadicalRequest {Level = level});

        public string Hello() => "world";
    }

    public class GetRadicalRequest : IRequest<IEnumerable<Radical>>
    {
        public int Level { get; init; }
    }


    public class GetLevel : IRequestHandler<GetRadicalRequest, IEnumerable<Radical>>
    {
        private readonly FirebaseFactory _factory;
        private readonly ILogger _logger;

        public GetLevel(FirebaseFactory factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<IEnumerable<Radical>> Handle(GetRadicalRequest request, CancellationToken cancellationToken)
        {
            _logger.Warning("Handling the request");
            
            var store = await _factory.Build();
            _logger.Warning("Factory has bee built");
            
            var snapshot = await store.Collection("radicals").GetSnapshotAsync(cancellationToken);
            if(snapshot != null)
                _logger.Warning("We got the snapshot");
            
            var versions = snapshot.Documents.Select(document => document.ConvertTo<Shared.Radical>()).ToList();
            
            if(versions != null && versions.Any())
                _logger.Warning("We got records");

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