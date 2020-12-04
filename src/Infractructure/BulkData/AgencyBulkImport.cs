using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Domain.Entities;
using Infractructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infractructure.BulkData
{
    public class AgencyBulkImport 
    {
        private const string AGENTS_TABLE_NAME = "Agents";
        private const string AGENCIES_TABLE_NAME = "Agencies";
        private const string RELATION_TABLE_NAME = "AgencyAgent";

        private readonly ApplicationDbContext _repository;
        private readonly IConfiguration _configuration;

        public AgencyBulkImport(ApplicationDbContext repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public void Import(IEnumerable<Agency> records)
        {
            var path = _configuration.GetSection("BulkSettings").GetChildren()
                .FirstOrDefault(_ => _.Key == "FolderForTempCSV")?.Value;

            ImportAgencies(path, records);

            foreach (var record in records)
            {
                ImportAgents(path, record);
                ImportRelation(path, record);
            }
        }
        private void ImportAgencies(string path, IEnumerable<Agency> agencies)
        {
            var filepath = path + @"\" + Guid.NewGuid() + ".csv";
            var resultCsv = ToCsv(",", agencies);
            InsertEntities(filepath, AGENCIES_TABLE_NAME, resultCsv);

        }

        private void InsertEntities(string path, string tableName, string csv)
        {
            using (var fs = File.Create(path))
            {
                var content = new UTF8Encoding(true).GetBytes(csv);
                fs.Write(content, 0, content.Length);
            }

            var query = $"COPY \"{tableName}\" FROM '{path}' DELIMITER ',' CSV HEADER";
            _repository.Database.ExecuteSqlRaw(query);
            File.Delete(path);
        }

        private void ImportAgents(string path, Agency agency)
        {
            var filepath = path + @"\" + Guid.NewGuid() + ".csv";
            var resultCsv = ToCsv(",", agency.Agents);
            InsertEntities(filepath, AGENTS_TABLE_NAME, resultCsv);
        }

        private void ImportRelation(string path, Agency agency)
        {
            var filepath = path + @"\" + Guid.NewGuid() + ".csv";
            var resultCsv = ToCsvRelations(",", agency);

            InsertEntities(filepath, RELATION_TABLE_NAME, resultCsv);
        }

        private string ToCsv<T>(string separator, IEnumerable<T> objectList)
        {
            if (objectList == null) 
                throw new ArgumentNullException(nameof(objectList));
            var t = typeof(T);
            var fields = t.GetProperties();

            var header = string.Join(separator, fields.Where(f => !(f.Name == "Agencies" || f.Name == "Agents")).Select(f => f.Name).ToArray());

            var csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectList)
            {
                csvdata.AppendLine(ToCsvProperties(separator, fields, o));
            }

            return csvdata.ToString();
        }

        private string ToCsvRelations(string separator, Agency objectlist)
        {
            var header = "AgenciesId,AgentsId";
            var csvData = new StringBuilder();
            csvData.AppendLine(header);

            foreach (var o in objectlist.Agents)
            {
                csvData.AppendLine(objectlist.Id + "," + o.Id);
            }

            return csvData.ToString();
        }

        private string ToCsvProperties(string separator, PropertyInfo[] properties, object o)
        {
            var linie = new StringBuilder();

            foreach (var f in properties)
            {
                if (f.Name == "Agencies" || f.Name == "Agents")
                    continue;

                if (linie.Length > 0)
                    linie.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append(x);
            }
            return linie.ToString();
        }
    }
}
