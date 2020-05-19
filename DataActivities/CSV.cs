using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.Data;


using System.Globalization;
using System.IO;
using CsvHelper;

namespace DataActivities
{
    public class CSV:CodeActivity
    {

        public InArgument<string> Delimiter { get; set; } = ",";
        public InArgument<string>Escape { get; set; }="\"";
        public bool HasHeaderRecord { get; set; } = true;
        public bool IgnoreBlankLines { get; set; } = true;
        
        [RequiredArgument]
        public InArgument<string>FileName { get; set; }
        
        public OutArgument<DataTable>Result { get; set; }

        public OutArgument<List<DataRow>>Rows { get; set; }
        
        protected override void Execute(CodeActivityContext context)
        {
            using (var reader = new StreamReader(this.FileName.Get(context)))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    // Do any configuration to `CsvReader` before creating CsvDataReader.
                    using (var dr = new CsvDataReader(csv))
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        Result.Set(context, dt);
                        Rows.Set(context,dt.AsEnumerable().ToList());
                    }

        }
    }
}
