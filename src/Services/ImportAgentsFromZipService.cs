using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Application.Common.Services;
using AutoMapper;
using Domain.Commands.AgentCommands;
using Domain.Entities;
using Domain.Import;

namespace Services
{
    public class ImportAgentsFromZipService : IImportService<Agent>
    {
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;

        public ImportAgentsFromZipService(IAgentService agencyService, IMapper mapper)
        {
            _agentService = agencyService;
            _mapper = mapper;
        }

        public IEnumerable<Agent> GetEntitiesFromFile(Stream fileStream)
        {
            var result = new List<Agent>();
            using (var archive = new ZipArchive(fileStream))
            {
                var entries = archive.Entries;

                foreach (var entry in entries)
                {
                    result.AddRange(DeserializeEntry(entry));
                }

                return result;
            }
        }

        public async Task<ImportResult<Agent>> Import(IEnumerable<Agent> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            var result = new ImportResult<Agent>();

            foreach (var entity in entities)
            {
                var createEntityCommand = _mapper.Map<CreateAgentCommand>(entity);
                var createEntityResult = await _agentService.CreateAsync(createEntityCommand);
                if (createEntityResult != null)
                {
                    result.SuccessfullyImported.Add(createEntityResult);
                }
                else
                {
                    result.NotImported.Add(entity);
                }
            }

            return result;
        }

        private IEnumerable<Agent> DeserializeEntry(ZipArchiveEntry entry)
        {
            if(entry == null)
                throw new ArgumentNullException(nameof(entry));

            var serializer = new XmlSerializer(typeof(List<Agent>), new XmlRootAttribute() { ElementName = "Agents" });
            var result = (List<Agent>)serializer.Deserialize(entry.Open());
            return result;
        }
    }
}