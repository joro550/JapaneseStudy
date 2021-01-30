using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<GetLevel> _logger;

        public GetLevel(FirebaseFactory factory, ILogger<GetLevel> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<IEnumerable<Radical>> Handle(GetRadicalRequest request, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Handling the request");
            
            var store = await _factory.Build();
            _logger.LogWarning($"Factory has been built, {store == null}");
            try
            {
                var snapshot = await store.Collection("radicals").GetSnapshotAsync(cancellationToken);
                _logger.LogWarning($"IsSnapshot null, {snapshot == null}");

                var versions = snapshot.Documents.Select(document => document.ConvertTo<Shared.Radical>()).ToList();
                _logger.LogWarning($"documents null, {versions == null} {versions.Any()}");

                return versions.Select(version => new Radical
                {
                    Character = version.Character,
                    Description = version.Description,
                    Mnemonic = version.Mnemonic,
                    KunYomi = version.KunYomi,
                    OnYomi = version.OnYomi
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new Radical[0];
            }
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