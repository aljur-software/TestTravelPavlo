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
using Application.Common.Interfaces;

namespace Infractructure.BulkData
{
    public class AgencyBulkImport : IBulkImport<Agency>
    {
        private const string AGENTS_TABLE_NAME = "Agents";
        private const string AGENCIES_TABLE_NAME = "Agencies";
        private const string RELATION_TABLE_NAME = "AgencyAgent";
        private const string AGENCIES_TO_AGENTS_TABLE_FIELDS = "AgenciesId,AgentsId";
        private const string CSV_FILE_EXTENTION = ".csv";

        private readonly ApplicationDbContext _repository;
        private readonly IConfiguration _configuration;

        public AgencyBulkImport(ApplicationDbContext repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public bool Import(IEnumerable<Agency> records)
        {
            var path = _configuration.GetSection("BulkSettings").GetChildren()
                .FirstOrDefault(_ => _.Key == "FolderForTempCSV")?.Value;
            ImportAgencies(path, records);
            foreach (var record in records)
            {
                ImportAgents(path, record);
                ImportRelation(path, record);
            }

            return true;
        }

        private void ImportAgencies(string path, IEnumerable<Agency> agencies)
        {
            var filepath = path + @"\" + Guid.NewGuid() + CSV_FILE_EXTENTION;
            var resultCsv = ToCsv(",", agencies);
            InsertEntities(filepath, AGENCIES_TABLE_NAME, resultCsv);
        }

        private void InsertEntities(string path, string tableName, string csv)
        {
            try
            {
                using (var fs = File.Create(path))
                {
                    var content = new UTF8Encoding(true).GetBytes(csv);
                    fs.Write(content, 0, content.Length);
                }

                var query = $"COPY \"{tableName}\" FROM '{path}' DELIMITER ',' CSV HEADER";
                _repository.Database.ExecuteSqlRaw(query);
            }
            finally
            {
                if(File.Exists(path))
                    File.Delete(path);
            }
        }

        private void ImportAgents(string path, Agency agency)
        {
            var filepath = path + @"\" + Guid.NewGuid() + CSV_FILE_EXTENTION;
            var resultCsv = ToCsv(",", agency.Agents);
            InsertEntities(filepath, AGENTS_TABLE_NAME, resultCsv);
        }

        private void ImportRelation(string path, Agency agency)
        {
            var filepath = path + @"\" + Guid.NewGuid() + CSV_FILE_EXTENTION;
            var resultCsv = ToCsvRelations(",", agency);
            InsertEntities(filepath, RELATION_TABLE_NAME, resultCsv);
        }

        private string ToCsv<T>(string separator, IEnumerable<T> objectList)
        {
            var type = typeof(T);
            var fields = type.GetProperties();
            var header = string.Join(separator, fields
                                                    .Where(p => !(p.Name == AGENCIES_TABLE_NAME || p.Name == AGENTS_TABLE_NAME))
                                                    .Select(p => p.Name)
                                                    .ToArray());
            var csvdata = new StringBuilder();
            csvdata.AppendLine(header);
            foreach (var o in objectList)
            {
                csvdata.AppendLine(ToCsvProperties(separator, fields, o));
            }

            return csvdata.ToString();
        }

        private string ToCsvRelations(string separator, Agency agency)
        {
            var csvData = new StringBuilder();
            csvData.AppendLine(AGENCIES_TO_AGENTS_TABLE_FIELDS);
            foreach (var agent in agency.Agents)
            {
                csvData.AppendLine(agency.Id + separator + agent.Id);
            }

            return csvData.ToString();
        }

        private string ToCsvProperties(string separator, PropertyInfo[] properties, object obj)
        {
            var lines = new StringBuilder();
            foreach (var property in properties)
            {
                if (property.Name == AGENCIES_TABLE_NAME || property.Name == AGENTS_TABLE_NAME)
                    continue;
                if (lines.Length > 0)
                    lines.Append(separator);
                var propValue = property.GetValue(obj);
                if (propValue != null)
                    lines.Append(propValue);
            }

            return lines.ToString();
        }
    }
}
