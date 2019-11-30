using System;
using System.Collections.Generic;
using System.IO;
using ContactManagerWeb.Enums;
using ContactManagerWeb.Models;

namespace ContactManagerWeb.Helpers
{
    public class FileBuilder
    {
        public FileType _fileType { get; set; }

        public FileBuilder(FileType fileType) => _fileType = fileType;

        public StreamFile GetFile<T>(IEnumerable<T> entities)
        {
            if (_fileType == FileType.Csv)
            {
                return GetCsv(entities);
            }

            throw new NotImplementedException();
        }

        private static StreamFile GetCsv<T>(IEnumerable<T> entities)
        {
            // Build csv file in memory then stream to the browser
            using (var ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);

                // Add csv header
                sw.WriteLine(@"FirstName,MiddleInitial,LastName,Email,HomePhone,CellPhone,OfficeExtension,IrdNumber,Active");

                foreach (var entity in entities)
                    AddContact(sw, entity);

                sw.Flush();
                ms.Position = 0;

                return new StreamFile
                {
                    Contents = ms.ToArray(),
                    ContentType = "APPLICATION/octet-stream",
                    Name = $"Contacts {DateTime.Now.ToString("dd-MMM-yyyy-HH_mm_ss")}.csv"
                };
            }
        }

        private static void AddContact<T>(StreamWriter sw, T entity)
        {
            var contact = entity as Contact;

            var contactLine = string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\"",
                                contact.FirstName,
                                contact.MiddleInitial,
                                contact.LastName,
                                contact.Email,
                                contact.HomePhone,
                                contact.CellPhone,
                                contact.OfficeExtension,
                                contact.IrdNumber,
                                contact.Active);

            sw.WriteLine(contactLine);
        }
    }
}