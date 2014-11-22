using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using MSWord = Microsoft.Office.Interop.Word;

namespace BPExporter
{
    class Word : IDisposable
    {
        MSWord.Application WordApp;
        List<WordDoc> Documents = new List<WordDoc>();

        public Word(bool Visible)
        {
            WordApp = new MSWord.Application();
            WordApp.Visible = Visible;
            if (Visible)
                WordApp.Activate();
        }

        public WordDoc Open(String Path)
        {
            MSWord.Documents docs = WordApp.Documents;
            MSWord.Document doc;

            try
            {
                Object Filename = Path;
                Object ReadOnly = false;

                doc = docs.Open(ref Filename, ref Utils.Missing, ref ReadOnly, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);

                Documents.Add(new WordDoc(doc));
                return Documents.Last();
            }
            catch (Exception)
            {
                docs.Close(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);
                throw;
            }
        }

        public void Dispose()
        {
            foreach (WordDoc doc in Documents)
            {
                doc.Dispose();
            }
            WordApp.Quit(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);
        }
    }

    class WordDoc : IDisposable
    {
        private MSWord.Document document;

        public WordDoc(MSWord.Document doc)
        {
            document = doc;
            document.Activate();
        }

        public void Save()
        {
            document.Saved = true;
            document.Save();
        }

        public void SaveAsPDF(string filename)
        {
            //DocToPdf.Convert(this, filename);
        }

        public void Print()
        {
            /*
            Document.PrintOut(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                  ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                  ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                  ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                  ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                  ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);
             * */
        }

        public void Dispose()
        {
            document.Close(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);
        }

        public String Filename
        {
            get { return System.IO.Path.GetFileName(document.FullName); }
        }

        public String Path
        {
            get { return document.FullName; }
        }

        public MSWord.Document Document
        {
            get { return document; }
        }

        public IEnumerable<TablePlaceholder> FindAllTablePlaceholders()
        {
            String Regex = @"(&TABLE&)";
            MSWord.Range WholeDoc = document.Content;

            WholeDoc.Find.ClearFormatting();

            WholeDoc.Find.Forward = true;
            WholeDoc.Find.MatchWildcards = true;
            WholeDoc.Find.Wrap = MSWord.WdFindWrap.wdFindStop;

            WholeDoc.Find.Text = Regex;

            WholeDoc.Find.Execute(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                   ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                   ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);

            while (WholeDoc.Find.Found)
            {
                Object Start = WholeDoc.Start;
                Object End = WholeDoc.End;

                MSWord.Range FoundRange = document.Range(ref Start, ref End);

                WholeDoc.Find.Execute(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                       ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                       ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);

                yield return new TablePlaceholder(FoundRange);
            }
        }

        public IEnumerable<Placeholder> FindAllPlaceholders()
        {
            String Regex = @"(&[A-Z_]@&)";
            MSWord.Range WholeDoc = document.Content;

            WholeDoc.Find.ClearFormatting();

            WholeDoc.Find.Forward = true;
            WholeDoc.Find.MatchWildcards = true;
            WholeDoc.Find.Wrap = MSWord.WdFindWrap.wdFindStop;

            WholeDoc.Find.Text = Regex;

            WholeDoc.Find.Execute(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                   ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                   ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);

            while (WholeDoc.Find.Found)
            {
                Object Start = WholeDoc.Start;
                Object End = WholeDoc.End;

                MSWord.Range FoundRange = document.Range(ref Start, ref End);

                WholeDoc.Find.Execute(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                       ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                       ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);

                yield return new Placeholder(FoundRange);
            }
        }
    }

    class TablePlaceholder : IDisposable
    {
        private MSWord.Range range;
        public String PlaceholderName
        {
            get;
            protected set;
        }

        public TablePlaceholder(String name)
        {
            PlaceholderName = name;
        }

        public TablePlaceholder(MSWord.Range rng)
        {
            range = rng;
            PlaceholderName = Range.Text.Trim('&');
        }

        public virtual void Dispose()
        {
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Range);
        }

        public MSWord.Range Range
        {
            get { return range; }
        }
    }

    class Placeholder : IDisposable
    {
        private MSWord.Range Range;
        public String PlaceholderName
        {
            get;
            protected set;
        }

        public Placeholder(String name)
        {
            PlaceholderName = name;
        }

        public Placeholder(MSWord.Range range)
        {
            Range = range;
            PlaceholderName = Range.Text.Trim('&');
        }

        public virtual void Replace(String data)
        {
            Range.Find.ClearFormatting();
            Range.Find.Replacement.ClearFormatting();

            Range.Find.Forward = true;
            Range.Find.MatchWildcards = true;
            Range.Find.Wrap = MSWord.WdFindWrap.wdFindContinue;

            Range.Find.Text = Range.Text;
            Range.Find.Replacement.Text = string.Format("{0}", data);

            Object ReplaceOne = MSWord.WdReplace.wdReplaceOne;
            Range.Find.Execute(ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing,
                                ref ReplaceOne, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing, ref Utils.Missing);
        }

        public virtual void Dispose()
        {
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Range);
        }
    }

    class WordTable 
    {
        MSWord.Table table;
        CSVSchema schema;

        public WordTable(MSWord.Range rng, CSVSchema sch)
        {
            schema = sch;
            MSWord.Document doc = rng.Document;
            table = doc.Tables.Add(rng, 1, schema.GetHeaders().Length, 
                Utils.Missing, Utils.Missing);

            MSWord.Border[] borders = new MSWord.Border[6];
            borders[0] = table.Borders[MSWord.WdBorderType.wdBorderLeft];
            borders[1] = table.Borders[MSWord.WdBorderType.wdBorderRight];
            borders[2] = table.Borders[MSWord.WdBorderType.wdBorderTop];
            borders[3] = table.Borders[MSWord.WdBorderType.wdBorderBottom];
            borders[4] = table.Borders[MSWord.WdBorderType.wdBorderHorizontal];
            borders[5] = table.Borders[MSWord.WdBorderType.wdBorderVertical];

            // Format each of the borders.
            foreach (MSWord.Border border in borders)
            {
                border.LineStyle = MSWord.WdLineStyle.wdLineStyleSingle;
                border.Color = MSWord.WdColor.wdColorBlack;
            }

            table.AutoFitBehavior(MSWord.WdAutoFitBehavior.wdAutoFitWindow);
        }

        String[] CreateRowValues(CSVSchema schema, Hit hit, Project project, int index)
        {
            var headers = schema.GetHeaders();
            List<String> values = new List<string>();

            foreach (String name in headers)
            {
                Object value = schema.GetValue(name, new SchemaProvider(hit, project), index);
                values.Add(String.Format("{0}", value));
            }

            return values.ToArray();
        }

        void CreateHeaderRow(CSVSchema schema)
        {
            setRowTexts(1, schema.GetHeaders(), null);
            table.Rows[1].Range.Font.Bold = 1;
        }

        public void Fill(IEnumerable<Hit> toExport, CSVSchema schema, Project project)
        {
            CreateHeaderRow(schema);

            int index = 1;
            foreach (Hit hit in toExport)
            {
                String[] values = CreateRowValues(schema, hit, project ,index);
                AddRow(values, GetRowFormats(schema).ToArray());

                index++;
            }

            table.PreferredWidth = 100;
            table.PreferredWidthType = MSWord.WdPreferredWidthType.wdPreferredWidthPercent;

            table.Columns.AutoFit();
        }

        IEnumerable<String> GetRowFormats(CSVSchema schema)
        {
            var headers = schema.GetHeaders();

            foreach (String name in headers)
            {
                yield return schema.GetFormat(name);
            }
        }

        public void AddRow(String[] values, String[] formats)
        {
            table.Rows.Add(Utils.Missing);
            setRowTexts(table.Rows.Count, values, null);
        }

        public void AddColumn(Object beforeCol)
        {
            table.Columns.Add(beforeCol);
        }

        public void AddColumn()
        {
            AddColumn(Utils.Missing);
        }

        public void setCellText(int row, int col, string text)
        {
            table.Cell(row, col).Range.Text = text;
            table.Cell(row, col).Range.Font.Size = 8;
            table.Cell(row, col).Range.Font.Bold = 0;
        }

        public void setCellFormat(int row, int col, string format)
        {
            table.Cell(row, col).Range.set_Style(format);
        }

        public void setRowTexts(int row, string[] values, string[] formats)
        {
            int col = 1;
            int all = values.Sum((p) => p.Length);

            foreach (string v in values)
            {
                setCellText(row, col, v);
                if(formats != null)
                    setCellFormat(row, col, formats[col]);
                setColWidth(row, col, ((100 * v.Length) / all));

                col++;
            }
        }

        private void setColWidth(int row, int col, float width)
        {
            table.Cell(row, col).PreferredWidthType = MSWord.WdPreferredWidthType.wdPreferredWidthPercent;
            table.Cell(row, col).PreferredWidth = width;
        }
    }
}
