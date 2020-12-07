using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Services;
using Domain.Entities;
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
        private readonly IBulkImport<Agency> _bulk;

        public ImportAgenciesWithAgentsFromZipService(IBulkImport<Agency> bulk)
        {
            _bulk = bulk;
        }

        public ICollection<Agency> GetEntitiesFromFile(Stream fileStream)
        {
            try
            {
                var result = new List<Agency>();
                using (var archive = new ZipArchive(fileStream))
                {
                    var entries = archive.Entries;
                    foreach (var entry in entries)
                    {
                        ChackEntrieIsXML(entry);
                        result.Add(DeserializeEntry(entry));
                    }
                    return result;
                }
            }
          
            catch(Exception e) when (e.GetType() != typeof(XMLDeserializeException))
            {
                var exceptionMessage = "Zip archive is corrupted" + "\n" + e.Message;
                if (e.InnerException != null)
                {
                    exceptionMessage += "\n";
                    exceptionMessage += e.InnerException.Message;
                }
                throw new ArchiveIsEmptyOrCorruptedException(exceptionMessage);
            }
        }

        public async Task<bool> Import(ICollection<Agency> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArchiveIsEmptyOrCorruptedException("Archive is empty");
            _bulk.Import(entities);

            return true;
        }

        private Agency DeserializeEntry(ZipArchiveEntry entry)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Agency), new XmlRootAttribute() { ElementName = "Agency" });
                var result = (Agency)serializer.Deserialize(entry.Open());
                return result;
            }
            catch(Exception e)
            {
                var exceptionMessage = $"Xml document '{entry.FullName}' is corrupted." + "\n" + e.Message;
                if (e.InnerException != null)
                {
                    exceptionMessage += "\n";
                    exceptionMessage += e.InnerException.Message;
                }
                throw new XMLDeserializeException(exceptionMessage);
            }
        }

        private void ChackEntrieIsXML(ZipArchiveEntry entry)
        {
            if (Path.GetExtension(entry.FullName) != ".xml")
                throw new InvalidFileFormatException($"Wrong file format for {entry.FullName} in zip archive. Expected: 'xml'.");
        }
    }
}