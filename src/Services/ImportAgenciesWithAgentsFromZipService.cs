using Application.Common.Services;
using Domain.Entities;
using Domain.Import;
using Infractructure.BulkData;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services
{
    public class ImportAgenciesWithAgentsFromZipService : IImportService<Agency>
    {
        private readonly AgencyBulkImport _agencyRepository;

        public ImportAgenciesWithAgentsFromZipService(AgencyBulkImport agencyRepository)
        {
            _agencyRepository = agencyRepository;
        }

        public IEnumerable<Agency> GetEntitiesFromFile(Stream fileStream)
        {
            var result = new List<Agency>();
            using (var archive = new ZipArchive(fileStream))
            {
                var entries = archive.Entries;
                foreach (var entry in entries)
                {
                    result.Add(DeserializeEntry(entry));
                }
            }

            return result;
        }

        public async Task<ImportResult> Import(IEnumerable<Agency> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            var result = new ImportResult();
            _agencyRepository.Import(entities);

            return result;
        }

        private Agency DeserializeEntry(ZipArchiveEntry entry)
        {
            if(entry == null)
                throw new ArgumentNullException(nameof(entry));
            var serializer = new XmlSerializer(typeof(Agency), new XmlRootAttribute() { ElementName = "Agency" });
            var result = (Agency)serializer.Deserialize(entry.Open());

            return result;
        }
    }
}