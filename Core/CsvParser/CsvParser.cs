using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.DataModels.Broadridge;
using CsvHelper;

namespace Core.CsvParser
{
    public static class CsvParser<T>
    {
        public static IEnumerable<T> ParseCsvFile(string path)
        {
            using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader))
                {
                    List<T> theList = csv.GetRecords<T>().ToList();
                    return theList;
                }
        }
    }
}