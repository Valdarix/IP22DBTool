using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace IndyPro22DatabaseManagementTool
{
    internal class DB
    {
     

        public static void UpdateOrInsert(string query)
        {
            var dbName = $"URI=file:E:\\Unity Projects\\IndyWrestling\\Assets\\StreamingAssets\\MamasHomeCooking\\DONOTDELETE\\elevenherbsandspices.ip2";
            using var connection = new SQLiteConnection(dbName);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            connection.Close();
        }


        public static DataTable GetDataReader(string query)
        {
            var dbName = $"URI=file:E:\\Unity Projects\\IndyWrestling\\Assets\\StreamingAssets\\MamasHomeCooking\\DONOTDELETE\\elevenherbsandspices.ip2";
            using var connection = new SQLiteConnection(dbName);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = query;
            using IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            var dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
        public static string GetStrData(DataRow dataRow, string field)
        {
            var result = string.Empty;
            result = dataRow[field].ToString();
            return result;
        }

        public static int GetIntData(DataRow dataRow, string field)
        {
            var result = 0;
            if (GetStrData(dataRow, field) == null || GetStrData(dataRow, field).Trim() == "") return 0;
            result = int.Parse(dataRow[field].ToString());
            return result;
        }

        public static Talent LoadTalent(DataRow row)
        {
            var t = new Talent // TODO: Load Full Talent Details
            {
                ID = int.Parse($"{row["TALENT_ID"]}"),
                name = $"{row["NAME"]}",
                nickNames = $"{row["NICKNAME"]}",
                birthMonth = $"{row["BIRTHMONTH"]}",
                birthYear = $"{row["BIRTHYEAR"]}",
                debutMonth = $"{row["DEBUT_MTH"]}",
                debutYear = $"{row["DEBUT_YR"]}",
                city = $"{row["City"]}",
                state = $"{row["State"]}",
                country = $"{row["Country"]}",
                theme = $"{row["THEMESONG"]}",
                twitterHandle = $"{row["TWITTER"]}",
                Facebook = $"{row["Facebook"]}",
                Youtube = $"{row["Youtube"]}",
                Instagram = $"{row["Instagram"]}",
                TikTok = $"{row["TikTok"]}",
                Twitch = $"{row["Twitch"]}",
                website = $"{row["URL"]}",
                biography = "", // Need the bio table. 
                Heel = GetIntData(row, "heel"),
                Face = GetIntData(row, "face"),
                Ref = GetIntData(row, "ref"),
                Backstage = GetIntData(row, "backstage"),
                Booker = GetIntData(row, "booker"),
                InRing = GetIntData(row, "inring"),
                Announce = GetIntData(row, "announcer"),
                Color = GetIntData(row, "color"),
                Mgr = GetIntData(row, "Manager_valet"),
                OldSchool = GetIntData(row, "OLDSCHOOL"),
                Brawler = GetIntData(row, "brawler"),
                Grappler = GetIntData(row, "grappler"),
                Flyer = GetIntData(row, "highflying"),
                Daredevil = GetIntData(row, "daredevil"),
                Lucha = GetIntData(row, "lucha"),
                StrongStyle = GetIntData(row, "strongstyle"),
                MMA = GetIntData(row, "mma"),
                Cruiser = GetIntData(row, "cruiserweight"),
                Hardcore = GetIntData(row, "hardcore"),
                Deathmatch = GetIntData(row, "deathmatch"),
                Prankster = GetIntData(row, "prankster"),
                Leader = GetIntData(row, "leader"),
                InAndOut = GetIntData(row, "inandout"),
                StraightEdge = GetIntData(row, "straightedge"),
                Drinks = GetIntData(row, "drink"),
                Smokes = GetIntData(row, "smoke"),
                Bully = GetIntData(row, "bully"),
                Helps = GetIntData(row, "helpful"),
                Watches = GetIntData(row, "watchothers"),
                Creative = GetIntData(row, "creativecontrol"),
                Bio = GetStrData(row, "bio")
            };
            return t;
        }
    }
}
