using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace BPExporter
{
    enum FileFormat
    {
        CSV,
        XLS
    }

    class ProviderXLSRules
    {
        private FileFormat fileFormat = FileFormat.XLS;
        private bool autoFilter = false;
        private string delimiter = ";";
        private Random StyleRanomizer = new Random();
        private List<String> Ordered = new List<String>();
        private Dictionary<String, String> formats = new Dictionary<string, string>();
        private Dictionary<String, String> types = new Dictionary<string, string>();
        private Dictionary<String, Func<SchemaProvider, int, String>> Definitions = new Dictionary<String, Func<SchemaProvider, int, String>>();

        public ProviderXLSRules(String file)
        {
            LoadDefFromFile(file);
        }

        private String ExtractColumnName(String Name, String opCode)
        {
            String ColName = Name.Substring(opCode.Length + 1);
            return ColName.TrimEnd(']');
        }

        private void LoadDefFromFile(String file)
        {
            int LineNumber = 0;
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    while (sr.Peek() >= 0)
                    {
                        LineNumber++;
                        String Line = sr.ReadLine();

                        if (Line.StartsWith("#"))
                            continue;

                        else if (Line.Length < 3)
                        {
                            if (!Definitions.ContainsKey(Line))
                                Definitions.Add(Line, null);

                            Ordered.Add(Line);
                            continue;
                        }

                        String[] tokens = Line.Split('=');
                        String Name = tokens[0].Trim();
                        String Code = tokens[1].Trim();

                        if (Code.StartsWith("("))
                        {
                            String InParens = Code.Trim('(', ')');
                            String[] StringArguments = InParens.Split(',');
                            String Format = StringArguments[0];
                            Munger[] ObjectArguments = Array.ConvertAll(StringArguments.Slice(1, -1),
                                                                       (String arg) => { return new Munger(arg); });

                            Func<SchemaProvider, int, String> Func = (SchemaProvider hit, int index) =>
                            {
                                Object[] Arguments = new Object[] { index };
                                var m = Array.ConvertAll(ObjectArguments,
                                    (Munger munger) => 
                                    {
                                        return munger.AspectName == "Empty" 
                                            ? String.Empty 
                                            : munger.GetValue(hit); 
                                    });
                                var merged = Arguments.Merge(m);
                                return String.Format(CultureInfo.InvariantCulture, Format, merged);
                            };

                            AddToColumns(Name, Func);
                        }
                        else if (Code.StartsWith("Empty"))
                        {
                            Func<SchemaProvider, int, String> Func = (SchemaProvider hit, int index) =>
                            {
                                return String.Empty;
                            };

                            AddToColumns(Name, Func);
                        }
                        else if (Line.StartsWith("Type["))
                        {
                            String ColumnName = ExtractColumnName(Name, "Type");
                            types.Add(ColumnName, Code);
                        }
                        else if (Line.StartsWith("Format["))
                        {
                            String ColumnName = ExtractColumnName(Name, "Format");
                            formats.Add(ColumnName, Code);
                        }
                        else if (Line.StartsWith("AutoFilter"))
                        {
                            autoFilter = (Code.ToLower() != "no");
                        }
                        else if (Line.StartsWith("OutputFormat"))
                        {
                            fileFormat = (FileFormat)Enum.Parse(typeof(FileFormat), Code);
                        }
                        else if(Line.StartsWith("Delimiter"))
                        {
                            delimiter = Code;
                        }
                        else
                        {
                            Munger munger = new Munger(Code);
                            Func<SchemaProvider, int, String> Func = (SchemaProvider hit, int index) =>
                            {
                                Object value = munger.GetValue(hit);
                                if (value != null)
                                    return String.Format(CultureInfo.InvariantCulture, "{0}", value).Trim();
                                else
                                    return String.Format(CultureInfo.InvariantCulture, "{0}", Code).Trim();
                            };

                            AddToColumns(Name, Func);
                        }
                    }
                }
            }
            catch ( IOException ){
                throw;
            }
            catch (Exception e){
                throw new Exception(String.Format("Syntax exception at line {0} in file {1}", LineNumber, file), e);
            }
        }

        private void AddToColumns(String Name, Func<SchemaProvider, int, String> Func)
        {
            if( !Definitions.ContainsKey(Name) )
                Definitions.Add(Name, Func);

            if (!Name.StartsWith("Title") && !Name.StartsWith("Footer") && !Name.StartsWith("Filename") && !Name.StartsWith("Format"))
                Ordered.Add(Name);
        }

        public String GetValue(String Name, SchemaProvider hit, int Index)
        {
            Func<SchemaProvider, int, String> func = null;
            Definitions.TryGetValue(Name, out func);
            if( func != null )
                return func.Invoke(hit, Index);
            return String.Empty;
        }

        public String GetType(String Name)
        {
            if( types.ContainsKey(Name) )
               return types[Name];
            else
                return "String";
        }

        public String GetFormat(String Name)
        {
            if( formats.ContainsKey(Name) )
               return formats[Name];
            return String.Empty;
        }

        public String GetFormatCodeAndReplaceWithID(String Name)
        {
            String FormatCode = String.Empty;
            if (formats.ContainsKey(Name))
            {
                FormatCode = formats[Name]; 
                formats[Name] = GenerateFormatID();
            }

            return FormatCode;
        }

        public String GenerateFormatID()
        {
            return String.Format("s_{0}", StyleRanomizer.Next( 10000 ));
        }

        public String GetTitle(SchemaProvider hit)
        {
            return GetValue("Title", hit, 0);
        }

        public String GetFooter(SchemaProvider hit)
        {
            return GetValue("Footer", hit, 0);
        }

        public String GetFilename(SchemaProvider hit)
        {
            return PathValidation.CleanFileName(GetValue("Filename", hit, 0));
        }

        public String[] GetNames()
        {
            return Ordered.ToArray();
        }

        public bool GetAutoFilter()
        {
            return autoFilter;
        }

        public FileFormat GetOutputFormat()
        {
            return fileFormat;
        }

        public String GetDelimiter()
        {
            return delimiter;
        }
    }

    class SchemaProvider
    {
        Dictionary<String, Object> attributes = new Dictionary<string, object>();

        public SchemaProvider(Dictionary<String, Object> attr)
        {
            attributes = attr;
        }

        public SchemaProvider(Hit hit, Project project)
            : this( new Dictionary<String, Object>{ 
                    { "Now", DateTime.Now },
                    { "Project", project },
                    { "Hit", hit }}
            )
        {}

        public Object this[String index]
        {
            get
            {
                Object obj = null;
                attributes.TryGetValue(index, out obj);
                return obj;
            }
        }
    }
}
