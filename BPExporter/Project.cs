using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

using System.Data.SQLite;

namespace BPExporter
{
    public static class SQLiteDataReaderExtensions
    {
        public static DateTime GetDateTimeUtc(this SQLiteDataReader reader, int col)
        {
            DateTime unspecified = reader.GetDateTime(col);
            return DateTime.SpecifyKind(unspecified, DateTimeKind.Utc);
        }
    }  

    public class Project
    {  
        private long id;
        private string name;
        private DateTime createDate;
        private string files_no;
        private string prefix;
        private string sufix;
        private string product;
        private TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
    
        public Project() 
        {
            createDate = DateTime.Now;
        }

        public Project(SQLiteDataReader reader)
        {
            id = (long)reader["id"];
            createDate = DateTime.Parse(reader["date"].ToString());
            files_no = reader["files_no"].ToString();
            prefix = reader["prefix"].ToString();
            sufix = reader["sufix"].ToString();
            UniqueIP = (int)reader["unique_ip"] == 1 ? true : false;

            String[] nameWithProduct = reader["name"].ToString().Split('#');
            
            name = nameWithProduct[0];
            if(nameWithProduct.Length > 1 )
                product = nameWithProduct[1];
        }

        public String TableName
        {
            get
            {
                return String.Format("hits_{0}", Name.Replace(" ", ""));
            }
        }

        public long ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get{ return name; }
            set{ name = value;}
        }

        public DateTime CreationDate
        {
            get { return createDate; }
            set{ createDate = value; }
        }

        public string FilesNo
        {
            get { return files_no; }
            set { files_no = value; }
        }

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }

        public string Sufix
        {
            get { return sufix; }
            set { sufix = value; }
        }

        public void IncrementFilesNo()
        {
            int i = int.Parse(FilesNo);
            String format = new String('#', FilesNo.Length);
            FilesNo = String.Format("{0:" + format + "}", ++i);
        }

        public TimeZoneInfo TimeZone
        {
            get { return timezone; }
            set { timezone = value; }
        }

        public String AKZ
        {
            get
            {
                return String.Format("{0} {1}/{2}", Prefix, FilesNo, Sufix);
            }
        }

        public bool UniqueIP
        {
            get;
            set;
        }

        public String Product
        {
            get { return product; }
            set { product = value; }
        }

        public Project Copy()
        {
            Project project = new Project();

            project.FilesNo = FilesNo;
            project.ID = ID;
            project.Name = String.Format("{0}_copy", Name);
            project.Prefix = Prefix;
            project.Product = Product;
            project.Sufix = Sufix;
            project.UniqueIP = UniqueIP;

            return project;
        }
    }

    public class File
    {
        private long id;
        private long project_id;
        private string name;
        private string hash;
        private int size;

        public File(){}

        public File(SQLiteDataReader reader)
        {
            id = TypeUtils.EnsureValid<long>( reader["id"] );
            name = TypeUtils.GetFromReader<String>(reader, "filename");
            hash = TypeUtils.GetFromReader<String>(reader, "hash");
            project_id = TypeUtils.EnsureValid<long>( reader["id_project"] );
        }

        public long ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public long ProjectID
        {
            get { return project_id; }
            set { project_id = value; }
        }

        public string Hash
        {
            get { return hash; }
            set { hash = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }
    }

    public class Hit
    {
        public class InvalidHitFormat : Exception 
        {
            public InvalidHitFormat(Exception e)
                : base("Invalid Hit Format", e) 
            {}
        };

        private long id;

        private string region;
        private string ip;
        private string our_ip;
        private int port;
        private int our_port;
        private string hash;
        private string guid;
        private string client;
        private string filename;
        private string country;
        private string isp;
        private string url;
        private string city;
        private string timezone;
        private string benn_kenn;
        private long size;
        private long type;
        private DateTime date;
        private TimeSpan time;
        private DateTime tz_date;
        private DateTime disconnect_time;
        private int piece;
        private string piece_hash;
        private int block;
        private string block_hash;
        private long downloaded; //block_size
        private long piece_size;
        private long downloaded_amount;
        private string sig;

        /*
        "SELECT 
         * 0      id, 
         * 1      ip, 
         * 2      port, 
         * 3      date, 
         * 4      guid, 
         * 5      url, 
         * 6      city,
         * 7      country, 
         * 8      filename, 
         * 9      type, 
         * 10     client, 
         * 11     hash, 
         * 12     isp, 
         * 13     timezone,
         * 14     bennkenn,
         * 15     time, 
         * 16     region,
         * 17     size, 
         * 18     downloaded, //block size
         * 19     piece_number,
         * 20     piece_hash, 
         * 21     our_ip, 
         * 22     our_port 
         * 23     block
         * 24     block_hash
         * 25     sig
         * from {0} where exported = 0;";
        */

        

        public Hit(SQLiteDataReader reader)
        {
            //optimized version
            try
            {
                id = (int)reader.GetInt64(0);
                ip = reader.GetString(1);
                port = (int)reader.GetInt64(2);
                guid = reader.GetString(4);
                client = reader.GetString(10);
                filename = reader.GetString(8);
                url = reader.GetString(6);
                benn_kenn = reader.GetString(14);
                size = (long)reader.GetInt64(17);
                hash = reader.GetString(11);
                country = reader.GetString(7);
                isp = reader.GetString(12);
                region = reader.GetString(16);
                city = reader.GetString(6);

                downloaded = TypeUtils.GetFromReader<long>(reader, 18);
                piece = (int)TypeUtils.GetFromReader<long>(reader, 19);
                piece_hash = TypeUtils.GetFromReader<String>(reader, 20);

                our_ip = TypeUtils.GetFromReader<String>(reader, 21);
                our_port = (int)TypeUtils.GetFromReader<long>(reader, 22);

                block = (int)TypeUtils.GetFromReader<long>(reader, 23);
                block_hash = TypeUtils.GetFromReader<String>(reader, 24);

                sig = TypeUtils.GetFromReader<string>(reader, 25);

                var datestr = reader.GetString(3);
                var dotpos = datestr.LastIndexOf('.');
                if(dotpos > 0)
                    datestr = datestr.Substring(0, dotpos);
                if (!DateTime.TryParseExact(datestr.TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                {
                    if(!DateTime.TryParseExact(datestr.TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss.fff",
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                    {

                        date = DateTime.ParseExact(datestr.TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                time = date.TimeOfDay;

                //time = reader.GetDateTime(15).TimeOfDay;
            }
            catch (Exception e)
            {
                throw new Hit.InvalidHitFormat(e);
            }

            date = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public Hit( SQLiteDataReader reader, bool reloadGeoIPData, bool fixHashes )
        {
            string tmpIp = TypeUtils.GetFromReader<String>(reader, "ip");
            var ip_port = tmpIp.Split(':');
            ip = ip_port.First();

            port = (int)TypeUtils.GetFromReader<long>(reader, "port");
            if(port == 0)
                int.TryParse(ip_port.LastOrDefault(), out port);

            our_ip = TypeUtils.GetFromReader<String>(reader, "ip_our");
            if (String.IsNullOrEmpty(our_ip))
            {
                our_ip = TypeUtils.GetFromReader<String>(reader, "our_ip");
                if(String.IsNullOrEmpty(our_ip))
                    our_ip = TypeUtils.GetFromReader<String>(reader, "self_ip");
            }

            our_port = (int)TypeUtils.GetFromReader<long>(reader, "our_port");
            if (our_port == 0)
            {
                our_port = (int)TypeUtils.GetFromReader<long>(reader, "port_our");
                if(our_port == 0)
                    our_port = (int)TypeUtils.GetFromReader<long>(reader, "self_port");
            }

            url = TypeUtils.GetFromReader<String>(reader, "url");
            guid = TypeUtils.GetFromReader<String> (reader, "guid");
            size = (long)TypeUtils.GetFromReader<long>(reader, "filesize");
            client = TypeUtils.GetFromReader<String>(reader, "client");
            filename = TypeUtils.GetFromReader<String> (reader,"filename");
            hash = TypeUtils.GetFromReader<String>(reader, "hash");
            if(String.IsNullOrEmpty(hash))
                hash = TypeUtils.GetFromReader<String>(reader, "t_hash");

            downloaded = TypeUtils.GetFromReader<long>(reader, "downloaded");
            if (downloaded == 0)
                downloaded = TypeUtils.GetFromReader<long>(reader, "block_size");

            block = (int)TypeUtils.GetFromReader<long>(reader, "block");
            block_hash = TypeUtils.GetFromReader<String>(reader, "block_hash");
            if(String.IsNullOrEmpty(block_hash))
                block_hash = TypeUtils.GetFromReader<String>(reader, "b_hash");

            piece = (int)TypeUtils.GetFromReader<long>(reader, "piece");
            piece_hash = TypeUtils.GetFromReader<String>(reader, "piece_hash");
            if(String.IsNullOrEmpty(piece_hash))
                piece_hash = TypeUtils.GetFromReader<String>(reader, "p_hash");

            if (piece_hash.StartsWith("urn:btih:"))
                piece_hash = piece_hash.Remove(0, "urn:btih:".Length);

            sig = TypeUtils.GetFromReader<string>(reader, "sig");

            if (fixHashes) hash = hash.FixHash(url);
            
            /*
            country = TypeUtils.GetFromReader<String>(reader, "country");
            isp = TypeUtils.GetFromReader<String>(reader, "isp");
            region = TypeUtils.GetFromReader<String>(reader, "region");
            city = TypeUtils.GetFromReader<String>(reader, "city");
            */

            if (reader.HasColumn("date"))
            {
                Object obj = reader["date"];
                if (obj is DateTime)
                    date = (DateTime)obj;
                else if (obj is String)
                    date = DateTime.Parse((String)obj);
            }
            else
            {
                int year = (int)TypeUtils.GetFromReader<long>(reader, "year");
                int month = (int)TypeUtils.GetFromReader<long>(reader, "month");
                int day = (int)TypeUtils.GetFromReader<long>(reader, "day");
                int hour = (int)TypeUtils.GetFromReader<long>(reader, "hour");
                int minute = (int)TypeUtils.GetFromReader<long>(reader, "minute");
                int second = (int)TypeUtils.GetFromReader<long>(reader, "second");

                int msecond = (int)TypeUtils.GetFromReader<long>(reader, "msecond");
                date = new DateTime(year, month, day, hour, minute, second, msecond);
            }

            /*Maybe we have midnight*/
            if (date.TimeOfDay.TotalMilliseconds == 0)
            {
                String strTime = TypeUtils.GetFromReader<String>(reader, "time");
                DateTime dtTime = TypeUtils.GetFromReader<DateTime>(reader, "time");

                if (!String.IsNullOrEmpty(strTime))
                    time = DateTime.Parse(strTime).TimeOfDay;
                else if (dtTime != null)
                    time = dtTime.TimeOfDay;
            }
            else
            {
                time = date.TimeOfDay;
                date = date.Date;
            }

            date = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public long ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public string Country
        {
            get  { return country; }
            set { country = value; }
        }

        public string Isp
        {
            get { return isp; }
            set { isp = value; }
        }

        public string Timezone
        {
            get { return timezone; }
            set { timezone = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string GUID
        {
            get { return guid; } 
            set { guid = value; }
        }

        public string Hash
        {
            get 
            {
                if (hash.ToUpper() == guid.ToUpper())
                    return Filename.GetSha1Hash();
                return hash; 
            }   
            set { hash = value; }
        }

        public string RawHash
        {
            get { return Hash; }
        }

        public DateTime Date
        {
            get
            {
                if (tz_date.Ticks == 0)
                    return date;
                return tz_date;
            }
            set
            {
                tz_date = value;
            }
        }

        public DateTime UTCDate
        {
            get { return date; }
        }

        public TimeSpan UTCTime
        {
            get { return date.TimeOfDay; }
        }

        public DateTime Disconnect
        {
            get { return disconnect_time; }
        }

        public String FullStrDate
        {
            get { return String.Format("{0:d} {1:t}", Date, Date); }
        }

        public String FullUTCStrDate
        {
            get { return String.Format("{0:d} {1:t}", UTCDate, UTCTime); }
        }

        public string IP
        {
            get { return IPUtils.ExtractIPv4FromIPv6(ip); }
            set { ip = value.Split(':').First(); }
        }

        public string BaseprotectIP
        {
            get { return IPUtils.ExtractIPv4FromIPv6(our_ip); }
            set { our_ip = value; }
        }

        public int BaseprotectPort
        {
            get { return our_port; }
            set { our_port = value; }
        }

        public int Port
        {
            get { return port;}
            set { port = value; }
        }

        public string Client
        {
            get { return client; }
            set { client = value; }
        }

        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        public long NetType
        {
            get { return type; }
            set { type = value; }
        }

        public String BennKenn
        {
            get { return benn_kenn.Trim(); }
            set { benn_kenn = value; }
        }

        public String Region
        {
            get { return region; }
            set { region = value; }
        }

        public String ShortRegion
        {
            get { return GetRegionShort(IP); }
        }

        public int BlockNumber
        {
            get { return block; }
            set { block = value; }
        }

        public String BlockHash
        {
            get { return block_hash; }
            set { block_hash = value; }
        }

        public long BlockSize
        {
            get { return downloaded; }
            set { downloaded = value; }
        }

        public String PieceHash
        {
            get { return piece_hash; }
            set { piece_hash = value; }
        }

        public long PieceSize
        {
            get { return piece_size; }
            set { piece_size = value; }
        }

        public int PieceNumber
        {
            get { return piece; }
            set { piece = value; }
        }

        public long Downloaded
        {
            get { return downloaded_amount; }
            set { downloaded_amount = value; }
        }

        public double PercentDownloaded
        {
            get{ return ((Downloaded * 100.0) / FileSize); }
        }

        public long FileSize
        {
            get { return size; }
            set { size = value; }
        }

        public string Sig
        {
            get { return sig; }
            set { sig = value; }
        }

        public String FormatedSize
        {
            get
            {
                return size.FormatedSize();
            }
        }

        public String GetRegionByIP(String ip)
        {
            if (Settings.CitySrv != null )
            {
                Location loc = Settings.CitySrv.getLocation(ip);
                if( loc != null )
                    return String.Format("{0}", loc.regionName);
            }
            
            return String.Empty;
        }

        public String GetRegionShort(String IP)
        {
            if (Settings.CitySrv != null)
            {
                Location loc = Settings.CitySrv.getLocation(ip);
                if (loc != null)
                    return String.Format("{0}", loc.region);
            }

            return String.Empty;
        }

        public String GetCityByIP(String ip)
        {
            if (Settings.CitySrv != null)
            {
                Location loc = Settings.CitySrv.getLocation(ip);
                
                if( loc != null )
                    return String.Format("{0}", loc.city);
            }
            
            return String.Empty;
        }

        public String GetISPByIP(String ip)
        {
            if (Settings.IspSrv != null)
            {
                String isp = Settings.IspSrv.getOrg(IP);

                //hack for Alice DSL ISP
                if(!String.IsNullOrEmpty(isp) && isp.Trim().ToLower().StartsWith("alice"))
                    isp = "Telefonica Deutschland GmBH";

                return String.Format( "{0}", isp );
            }
            
            return String.Empty;
        }

        public String GetCountryByIP(String ip)
        {
            if (Settings.CitySrv != null)
            {
                Location loc = Settings.CitySrv.getLocation(ip);

                if (loc != null)
                    return String.Format("{0}", loc.countryCode);
            }

            return String.Empty;
        }

        public bool IsValid()
        {
            String ip = IP;

            return  !(ip.StartsWith("0.") || ip.EndsWith(".0")) &&
                    !String.IsNullOrEmpty(ip)   &&
                    !String.IsNullOrEmpty(GUID) &&
                    !String.IsNullOrEmpty(Hash) &&
                    !String.IsNullOrEmpty(Filename);
        }
    }
}
