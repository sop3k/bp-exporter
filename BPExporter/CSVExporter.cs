using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace BPExporter
{
    public class SchemaElem
    {
        private String _Header;
        private String _MemberGetter;
        Func<Hit, int, Object> Getter = null;

        public SchemaElem(String header, String getter)
        {
            _Header = header;
            _MemberGetter = getter;
        }

        public SchemaElem(String header, Func<Hit, int, Object> getter)
        {
            _Header = header;
            Getter = getter;
        }

        public Object GetValue<T>(Object obj, int index)
        {
            if (!String.IsNullOrEmpty(_MemberGetter))
            {
                return TypeUtils.GetValueFromAnonymousType<T>(obj, _MemberGetter);
            }
            else if (Getter != null)
            {
                return Getter.Invoke((Hit)obj, index);
            }
            return null;
        }

        public String HeaderName
        {
            get { return _Header; }
        }
    }

    class CSVSchema
    {
        private ProviderXLSRules rules;

        public static SchemaElem FreeSpace = new SchemaElem( String.Empty, String.Empty );

        public CSVSchema(String file)
        {
            rules = new ProviderXLSRules(file);
        }

        public String[] GetHeaders()
        {
            return rules.GetNames();
        }

        public Object GetValue(String name, SchemaProvider hit, int index)
        {
            return rules.GetValue(name, hit, index);
        }

        public String GetTitle(SchemaProvider hit)
        {
            return rules.GetTitle(hit);
        }

        public String GetFooter(SchemaProvider hit)
        {
            return rules.GetFooter(hit);
        }

        public String GetFilename(SchemaProvider hit)
        {
            return rules.GetFilename(hit);
        }

        public String GetFormat(String name)
        {
            return rules.GetFormat(name);
        }

        public String GetFormatAndReplaceWithId(String Name)
        {
            return rules.GetFormatCodeAndReplaceWithID(Name);
        }

        public String GetType(String Name)
        {
            return rules.GetType(Name);
        }

        public bool GetAutoFilter()
        {
            return rules.GetAutoFilter();
        }

        public FileFormat GetOutputFormat()
        {
            return rules.GetOutputFormat();
        }

        public String GetDelimiter()
        {
            return rules.GetDelimiter();
        }
    }

    class CSVExporter
    {
        protected StreamWriter writer;
        protected String Path;

        public CSVExporter() {}

        public CSVExporter(String path)
        {
            Path = System.IO.Path.ChangeExtension(path, "csv");
            writer = new StreamWriter(path, false, Encoding.GetEncoding("iso-8859-1"));
        }

        public virtual void Export(List<Hit> toExport, Project project, CSVSchema schema)
        {
            WriteHeader(schema);

            int index = 0;
            foreach (Hit hit in toExport)
            {
                index++;
                List<String> hitRow = new List<String>();

                foreach (String name in schema.GetHeaders())
                {
                    Object value = schema.GetValue(name, new SchemaProvider(hit, project), index);
                    hitRow.Add(String.Format("{0}", value));
                }
                writer.Write(String.Join(schema.GetDelimiter(), hitRow.ToArray()) + "\r\n");
            }

            writer.Close();
        }

        private void WriteHeader(CSVSchema schema)
        {
            String header = String.Join(schema.GetDelimiter(), schema.GetHeaders());
            writer.Write(header + "\r\n");
        }
#region Excel
        /*public void SaveAsWorkbook(string strCSV, string strXLS)
        {
            var Missing = Type.Missing;
            Excel.Application app = new Excel.Application();
            Excel.Workbook doc = null;

            try
            {
                doc = OpenExcelWorkbook(app, strCSV);
                doc.SaveAs(strXLS, Excel.XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing,
                           Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges,
                           false, Missing, Missing, Missing);
            }
            finally 
            {
                if (doc != null)
                {
                    doc.Saved = true;
                    CloseWorkbook(doc);
                }
                Close(app);                
            }
        }

        public Excel.Workbook OpenExcelWorkbook(Excel.Application app, String CSVFilename)
        {
            var Missing = Type.Missing;
            System.Globalization.CultureInfo oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Excel.Range destination = null;
            Excel.Worksheet sheet = null;

            try
            {
                Excel.Workbook doc = app.Workbooks.Open(CSVFilename, false, Missing, Missing, Missing,
                                        Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);


                String connection = String.Format("TEXT;{0}", CSVFilename);

                sheet = (Excel.Worksheet)doc.ActiveSheet;
                destination = sheet.get_Range("A1", Type.Missing);

                Excel.QueryTables tables = sheet.QueryTables;
                Excel.QueryTable table = tables.Add(connection, destination, Type.Missing);

                table.TextFileCommaDelimiter = false;
                table.Name = System.IO.Path.GetFileName(CSVFilename);
                table.FieldNames = true;
                table.RowNumbers = false;
                table.FillAdjacentFormulas = false;
                table.PreserveFormatting = true;
                table.RefreshOnFileOpen = false;
                table.RefreshStyle = Excel.XlCellInsertionMode.xlOverwriteCells;
                table.TextFilePlatform = -535;
                table.SavePassword = false;
                table.SaveData = true;
                table.AdjustColumnWidth = true;
                table.RefreshPeriod = 0;
                table.TextFilePromptOnRefresh = false;
                table.TextFileStartRow = 1;
                table.TextFileParseType = Excel.XlTextParsingType.xlDelimited;
                table.TextFileTextQualifier = Excel.XlTextQualifier.xlTextQualifierSingleQuote;
                table.TextFileConsecutiveDelimiter = false;
                table.TextFileTabDelimiter = false;
                table.TextFileSemicolonDelimiter = true;
                table.TextFileCommaDelimiter = false;
                table.TextFileSpaceDelimiter = false;
                table.TextFileColumnDataTypes = new object[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                table.TextFileTrailingMinusNumbers = true;
                table.Refresh(false);

                doc.RefreshAll();

                return doc;
            }
            finally
            {
                CloseRange(destination);
                CloseWorksheet(sheet);

                System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }

        private void CloseRange(Excel.Range range)
        {
            if (range != null)
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(range);
        }

        private void CloseWorkbook(Excel.Workbook workbook)
        {
            if (workbook != null)
            {
                workbook.Close(false, Type.Missing, Type.Missing);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbook);
            }
        }

        private void CloseWorksheet(Excel.Worksheet worksheet)
        {
            if (worksheet != null)
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(worksheet);
        }

        public void Close(Excel.Application App)
        {
            performGC();
            if (App != null)
            {
                App.Quit();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(App);
            }
            performGC();
        }

        private void performGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        */
#endregion
    }

    class XMLExporter : CSVExporter
    {
        public XMLExporter(String path)
        {
            Path = System.IO.Path.ChangeExtension(path, "xls");
            writer = new StreamWriter(Path, false, Encoding.GetEncoding("utf-8"));
        }

        public override void Export(List<Hit> toExport, Project project, CSVSchema schema)
        {
            String SheetName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(Path));

            try
            {
                using (XmlWriter writer = XmlWriter.Create(this.writer))
                {
                    XMLHeader(writer);
                    WriteDocumentProperties(writer);
                    WriteDocumentStyles(writer, schema);
                    writer.WriteStartElement("Worksheet");
                    writer.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:spreadsheet", "SheetName");
                    if (schema.GetAutoFilter())
                        WriteNamesClauseForHeader(writer, toExport.Count(), schema.GetHeaders().Count());
                    writer.WriteStartElement("Table");

                    WriteColumnProperties(writer, schema);
                    WriteTitle(writer, project, schema, toExport);
                    WriteHeader(writer, schema);

                    int index = 0;
                    foreach (Hit hit in toExport)
                    {
                        index++;
                        writer.WriteStartElement("Row");
                        foreach (String name in schema.GetHeaders())
                        {
                            var value = schema.GetValue(name, new SchemaProvider(hit, project), index);
                            WriteCell(writer, value, schema.GetType(name), schema.GetFormat(name), schema.GetAutoFilter() );
                        }
                        writer.WriteEndElement();
                    }

                    WriteFooter(writer, project, schema, toExport);

                    writer.WriteEndElement();
                    if (schema.GetAutoFilter())
                        WriteAutoFilter(writer, toExport.Count(), schema.GetHeaders().Count());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            finally
            {
                base.writer.Close();
            }
        }

        private void WriteColumnProperties(XmlWriter writer, CSVSchema schema)
        {
            foreach (String name in schema.GetHeaders())
            {
                writer.WriteStartElement("Column");
                writer.WriteAttributeString("ss", "StyleID", null, schema.GetFormat(name));
                writer.WriteEndElement();
            }
        }

        private void WriteNamesClauseForHeader(XmlWriter writer, int Rows, int Columns)
        {
            writer.WriteStartElement("Names");
                writer.WriteStartElement("NamedRange");
                    writer.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:spreadsheet", "_FilterDatabase");
                    writer.WriteAttributeString("RefersTo", "urn:schemas-microsoft-com:office:spreadsheet", String.Format("R1C1:R{0}C{1}", Rows, Columns));
                    writer.WriteAttributeString("Hidden", "urn:schemas-microsoft-com:office:spreadsheet", "1");
                writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private void WriteAutoFilter(XmlWriter writer, int Rows, int Columns)
        {
            writer.WriteStartElement("AutoFilter", "urn:schemas-microsoft-com:office:excel");
            writer.WriteAttributeString("x", "Range", "urn:schemas-microsoft-com:office:excel", String.Format("R1C1:R{0}C{1}", Rows, Columns ));
            //writer.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");
            writer.WriteEndElement();
        }

        private void WriteCell(XmlWriter writer, Object value, String TypeName, String Format, bool autoFilter)
        {
            writer.WriteStartElement("Cell");
                
                if( !String.IsNullOrEmpty(Format) )
                {
                    writer.WriteAttributeString("ss", "StyleID", "urn:schemas-microsoft-com:office:spreadsheet", Format );
                }

                writer.WriteStartElement("Data");
                    writer.WriteAttributeString("Type", "urn:schemas-microsoft-com:office:spreadsheet", TypeName);
                    writer.WriteString(String.Format("{0}", value));
                writer.WriteEndElement();

                if (autoFilter)
                {
                    writer.WriteStartElement("NamedCell");
                        writer.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:spreadsheet", "_FilterDatabase");
                    writer.WriteEndElement();
                }

            writer.WriteEndElement();
        }

        private void WriteHeader(XmlWriter writer, CSVSchema schema)
        {
            writer.WriteStartElement("Row");
            foreach (String name in schema.GetHeaders())
            {
                String Name = name;
                if (name.StartsWith("FreeSpace"))
                    Name = String.Empty;
                WriteCell(writer, Name, schema.GetType(name), schema.GetFormat(name), schema.GetAutoFilter());
            }
            writer.WriteEndElement();
        }

        private void WriteTitle(XmlWriter writer, Project project, CSVSchema schema, List<Hit> toExport)
        {
            String title = schema.GetTitle(new SchemaProvider(toExport.First(), project));
            if (String.IsNullOrEmpty(title))
                return;

            writer.WriteStartElement("Row");
            WriteCell(writer, title, schema.GetType(String.Empty), String.Empty, schema.GetAutoFilter());
            writer.WriteEndElement();
        }

        private void WriteFooter(XmlWriter writer, Project project, CSVSchema schema, List<Hit> toExport)
        {
            String footer = schema.GetFooter(new SchemaProvider(toExport.First(), project));
            if( String.IsNullOrEmpty( footer ) )
                return;

            writer.WriteStartElement("Row");
            WriteCell(writer, footer, String.Empty, String.Empty, schema.GetAutoFilter() );
            writer.WriteEndElement();
        }

        private void WriteDocumentStyles(XmlWriter writer, CSVSchema schema)
        {
            writer.WriteStartElement("Styles");

            foreach (String Name in schema.GetHeaders())
            {
                String Format = schema.GetFormatAndReplaceWithId(Name);
                String FormatID = schema.GetFormat(Name);
                String Type = schema.GetType(Name);

                if (String.IsNullOrEmpty(Format))
                    continue;

                writer.WriteStartElement("Style", "urn:schemas-microsoft-com:office:spreadsheet");
                    writer.WriteAttributeString("ss", "ID", "urn:schemas-microsoft-com:office:spreadsheet", FormatID);

                    writer.WriteStartElement("Alignment");
                        writer.WriteAttributeString("ss", "Horizontal","urn:schemas-microsoft-com:office:spreadsheet", "Center");
                        writer.WriteAttributeString("ss", "Vertical", "urn:schemas-microsoft-com:office:spreadsheet", "Bottom");
                    writer.WriteEndElement();

                    writer.WriteStartElement("NumberFormat");
                        writer.WriteAttributeString("ss", "Format", "urn:schemas-microsoft-com:office:spreadsheet", Format);
                    writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void WriteDocumentProperties(XmlWriter writer)
        {
            writer.WriteStartElement("DocumentProperties", "urn:schemas-microsoft-com:office:office");
                writer.WriteElementString("Author", Environment.UserName );
                writer.WriteElementString("Created", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") );
                writer.WriteElementString("Version", String.Format("{0}", 11.5606));
            writer.WriteEndElement();

            writer.WriteStartElement("ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");
                writer.WriteElementString("ProtectStructure", "False");
                writer.WriteElementString("ProtectWindows", "False");
            writer.WriteEndElement();
        }

        private void XMLHeader(XmlWriter writer)
        {
            writer.WriteRaw("<?mso-application progid=\"Excel.Sheet\"?>");
            writer.WriteStartElement("Workbook", "urn:schemas-microsoft-com:office:spreadsheet");
                writer.WriteAttributeString( "xmlns", "o", null, "urn:schemas-microsoft-com:office:office"); 
                writer.WriteAttributeString( "xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
                writer.WriteAttributeString( "xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
                writer.WriteAttributeString( "xmlns", "html", null, "http://www.w3.org/TR/REC-html40");
        }
    }
}
