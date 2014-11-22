using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Reflection;
using System.Data.Common;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;

namespace BPExporter
{
    class Utils
    {
        public static Object Missing = System.Type.Missing;

        public static DateTime CombineDateAndTime(DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day,
                                time.Hours, time.Minutes, time.Seconds);
        }

        public static string DateString(DateTime date)
        {
            return date.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public static string String(object value)
        {
            if (value is DateTime)
                return DateString((DateTime)value);
            else
                return value.ToString();
        }

        public static string Convert(string s)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.Unicode;

            byte[] utfBytes = utf8.GetBytes(s);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            return iso.GetString(isoBytes);
        }

        public static bool LongerContainShorter(string first, string second)
        {
            string longer = first.Length >= second.Length ? first : second;
            string shorter = first.Length < second.Length ? first : second;

            return longer.Trim().ToUpper().Contains(shorter.Trim().ToUpper());
        }

        public class Getter<R>
        {
            public static R Get(object o, string member)
            {
                Type objType = o.GetType();
                Object Value = objType.GetProperty(member).GetValue(o, null);
                if (Value is R)
                    return (R)Value;
                return default(R);
            }
        }

        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static void WriteFully(Stream stream, byte[] data)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(data);
                writer.Close();
            }
        }

        public static object[] GetOneDimension(object[,] array, int index)
        {
            object[] arr = new object[array.GetLength(1) + 1];
            for (int i = array.GetLowerBound(1); i <= array.GetLength(1); i++)
            {
                arr[i] = array[index, i];
            }
            return arr;
        }

        public static void IgnoreExceptions(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
            }
        }
    }

    static class ArrayUtils
    {
        public static T[] Empty<T>()
        {
             return new T[0]; // Won't crash in foreach
        }

        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end + 1;
            }
            int len = end - start;

            if (len <= 0)
                return ArrayUtils.Empty<T>();

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        public static T[] Merge<T>(this T[] originalArray, T[] newArray)
        {
            int startIndexForNewArray = originalArray.Length;
            Array.Resize<T>(ref originalArray, originalArray.Length + newArray.Length);
            if( newArray.Length >= startIndexForNewArray )
                newArray.CopyTo(originalArray, startIndexForNewArray);
            return originalArray;
        }
    }

    class SVN
    {
        public static String Revision()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    class TypeUtils
    {
        public static T GetValueFromAnonymousType<T>(object dataitem, string itemkey)
        {
            System.Type type = dataitem.GetType();
            PropertyInfo info = type.GetProperty(itemkey);
            if( info != null )
            {
                T itemvalue = (T)info.GetValue(dataitem, null);
                return itemvalue;
            }
            return default(T);
        }

        public static T GetFromReader<T>(DbDataReader reader, int ordinal)
        {
            try
            {
                return TypeUtils.EnsureValid<T>(reader[ordinal]);
            }
            catch (Exception)
            {
                if (typeof(T) == typeof(String))
                    return (T)(Object)String.Empty;
                return default(T);
            }
        }

        public static T GetFromReader<T>(DbDataReader reader,  String item)
        {
            try
            { 
                return TypeUtils.EnsureValid<T>(reader[item]);
            }
            catch (Exception){
                if (typeof(T) == typeof(String))
                    return (T)(Object)String.Empty;
                return default(T);
            }
        }

        public static T EnsureValid<T>(Object obj)
        {
            Type type = obj.GetType();
            if (type == typeof(System.DBNull))
            {
                if (typeof(T) == typeof(String))
                    return (T)(Object)String.Empty;
                return default(T);
            }
            else if (type == typeof(T))
                return (T)obj;
            else
            {
                if (typeof(T) == typeof(String))
                    return (T)(Object)String.Empty;
                return default(T);
            }
        }

        public static T EnsureValid<T>(Object obj, bool _throw)
        {
            if (obj.GetType() == typeof(System.DBNull))
            {
                if (typeof(T) == typeof(String))
                    return (T)(Object)String.Empty;
                return default(T);
            }

            else if (obj.GetType() == typeof(T))
                return (T)obj;
            else
            {
                if (typeof(T) == typeof(String))
                    return (T)(Object)String.Empty;
                if (_throw)
                    throw new Exception(String.Format("Wrong type {0}", typeof(T)));
                return default(T);
            }
        }
    }

    public static class DataRecordExtensions
    {
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i=0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }

    public static class StringUtils
    {
        private static String[] hashTypes = {"md5:","sha1:","ed2k:","btih:"};

        private static String RemoveHashType(this String str)
        {
            String s = str;
            foreach (String type in hashTypes)
            {
                s = s.Replace(type, String.Empty);                
            }

            return s;
        }

        public static String FixHash(this String str, String url)
        {
            if( str.StartsWith("btih:") || url.StartsWith("btc:") )
            {
                byte[] decodedData = Base32Url.FromBase32String(str.RemoveHashType());
                return BitConverter.ToString(decodedData).Replace("-", "");
            }
            return str.RemoveHashType().ToUpper();
        }

        public static string GetSha1Hash(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return String.Empty;
            Byte[] bytes = Encoding.UTF8.GetBytes(value);
            Byte[] computedHash = System.Security.Cryptography.SHA1.Create().ComputeHash(bytes);
            return BitConverter.ToString(computedHash).Replace("-", "");
        }

        public static String FormatedSize(this long size)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (size > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(size, max), order);

                max /= scale;
            }

            return "0 Bytes";
        }
    }

    public static class ControlExtensions
    {
        public static void UIThread(this Control self, Action code)
        {
            if (self.InvokeRequired)
            {
                self.BeginInvoke(code);
            }
            else
            {
                code.Invoke();
            }
        }
    }

    public class IPUtils
    {
        static public string Int32ToIp(int ipAddress)
        {
            return new IPAddress(BitConverter.GetBytes(ipAddress).Reverse().ToArray()).ToString();
        }

        static public String ExtractIPv4FromIPv6(String ip)
        {
            try
            {
                String[] segments = ip.Split(':');

                //More than 2 segments quarantes that IPv4:Port don't fall into this.
                if (segments.Length > 2)
                {
                    if (segments[0] == "2001")
                    {
                        uint IPv4 = Convert.ToUInt32(segments[6] + segments[7], 16) ^ 0xFFFFFFFF;;
                        return Int32ToIp((int)IPv4).ToString();
                    }

                    else if (segments[0] == "2002")
                    {
                        uint IPv4 = Convert.ToUInt32(segments[1] + segments[2], 16);
                        return Int32ToIp((int)IPv4).ToString();
                    }
                }

                return ip;
            }
            catch (Exception e)
            {
                return ip;
            }
        }
    }

    public class DBUtils
    {
        static public void Fix(String[] files, Action<String, int> progress)
        {
            int count = 0;
            foreach (String filename in files)
            {
                progress.Invoke(filename, count++);
                LaunchFixBat(String.Format(@"""{0}""", filename));
            }
        }

        static public void LaunchFixBat(String filename)
        {
            string pwd = Directory.GetCurrentDirectory();
	        string fix = String.Format("{0}/fix.bat", pwd);

	        ProcessStartInfo startInfo = new ProcessStartInfo();
	        startInfo.FileName = fix;
            startInfo.Arguments = filename;

            using (Process exeProcess = Process.Start(startInfo))
	        {
                exeProcess.WaitForExit();
            }
        }

        static public string[] ListDBFiles(string path)
        {
            string db_path = (String)path;
            return Directory.GetFiles(db_path, "*.db", SearchOption.AllDirectories);
        }

        static public bool IsExporterDB(String path)
        {
            string connStr = String.Format("Data Source={0};Version=3;", path);
            var connection = new SQLiteConnection(connStr);
            connection.Open();

            string Sql = @"SELECT count(name) FROM sqlite_master WHERE type='table' AND name like 'hits_%';";
            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);
 
            return (long)cmd.ExecuteScalar()!=0;
        }

        static public void UpdateIndexes(DB exporter, String path)
        {
            string connStr = String.Format("Data Source={0};Version=3;", path);
            var connection = new SQLiteConnection(connStr);
            connection.Open();

            string Sql = @"SELECT * from project";
            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                Project project = new Project(reader);
                exporter.CreateNewProject(project);
                
                Sql = @"SELECT * from file_to_project join file on file.id=file_to_project.id_file where id_project={0}";
                cmd = new SQLiteCommand(String.Format(Sql, reader["id"]), connection);
                SQLiteDataReader ftp_reader = cmd.ExecuteReader();
                List<Object[]> files = new List<Object[]>();
                while (ftp_reader.Read())
                {
                    files.Add(new Object[2]{ftp_reader["filename"], ftp_reader["hash"]});
                }

                exporter.AddFilesToProject(project, files);

                Sql = @"SELECT id, ip, port, date, guid, url, city, country, filename, type, client, hash, isp, timezone, bennkenn, time, region, size, block_size, piece_number, piece_hash, our_ip, our_port, block_number, block_hash from {0}";
                cmd = new SQLiteCommand(String.Format(Sql, project.TableName), connection);
                SQLiteDataReader hits_reader = cmd.ExecuteReader();
                List<Hit> hits = new List<Hit>();
                while (hits_reader.Read())
                {
                    hits.Add(new Hit(hits_reader));
                }
                exporter.InsertHits(project, hits, project.FilesNo, new List<String>(), null);
            
            }
        }
    }
}
