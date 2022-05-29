using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelosFramework.FileHandling;
using ZelosFramework.NLP_Core;
using Npgsql;

namespace Persisting.PostgreSQL
{
    public class PostgreSQLRepo : IScriptRepository
    {
        private readonly string connString;
        private readonly INlpAnalyzer analyzer;

        public PostgreSQLRepo()
        {
            string Host = "127.0.0.1";
            string User = "postgres";
            string Password = "postgrespw";
            string Port = "5342";


            this.connString = $"Server={Host};Username={User};Port={Port};Password={Password};SSLMode=Prefer";

            this.analyzer = new CatalystAnalyser();
        }

        public Script AddScript(Script script)
        {
            Script result = null;
            analyzer.Analyse(script);
            using (var conn = new NpgsqlConnection(connString))
            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO \"USER_SCRIPT\" (\"Name\", \"ScriptString\", \"SourceFileType\") VALUES (@ScriptName, @ScriptString, @SourceFileType); ", conn))
                {
                    command.Parameters.AddWithValue("@ScriptName", script.Name);
                    command.Parameters.AddWithValue("@ScriptString", script.ScriptString);
                    command.Parameters.AddWithValue("@SourceFileType", script.SourceFileSettings.FileType.ToString());
                    var affectedRows = command.ExecuteNonQuery();
                    if(affectedRows == 1)
                    {
                        result = script;
                    }
                }
            }
            if (script.IsFtpServerScript)
            {
                this.SaveFtpConfig(script);
            }
            return result;
        }

        private void SaveFtpConfig(Script script)
        {
            var scriptId = -1;
            using (var conn = new NpgsqlConnection(connString))
            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();
                using (var command = new NpgsqlCommand($"SELECT \"ID\" FROM PUBLIC.\"USER_SCRIPT\" WHERE \"Name\" = '{script.Name}'", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        scriptId = reader.GetInt32(0);
                    }
                }
            }
            if (scriptId != -1)
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    using (var command = new NpgsqlCommand("INSERT INTO \"FTP_CONFIG\" (\"Url\", \"User\", \"Password\", \"FilePath\", \"ScriptId\") " +
                        "VALUES (@Url, @User, @Password, @FilePath, @ScriptId); ", conn))
                    {
                        conn.Open();
                        command.Parameters.AddWithValue("@ScriptId", scriptId);
                        command.Parameters.AddWithValue("@Url", script.FtpSource.Url);
                        command.Parameters.AddWithValue("@User", script.FtpSource.User != null ? script.FtpSource.User : DBNull.Value);
                        command.Parameters.AddWithValue("@Password", script.FtpSource.Password != null ? script.FtpSource.Password : DBNull.Value);
                        command.Parameters.AddWithValue("@FilePath", script.FtpSource.FilePath);
                        var affectedRows = command.ExecuteNonQuery();
                        if (affectedRows != 1)
                        {
                            Console.Out.WriteLine("No FTP config was saved");
                        }
                    }
                }
            }
        }

        public Script GetScriptByName(string name)
        {
            using (var conn = new NpgsqlConnection(connString))

            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();
                using (var command = new NpgsqlCommand($"SELECT \"Name\",\"ScriptString\",\"SourceFileType\" FROM PUBLIC.\"USER_SCRIPT\" WHERE \"Name\" = '{name}'", conn))
                {

                    Script result = null;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    { 
                        result = new Script();
                        result.Name = reader.GetString(0);
                        result.ScriptString = reader.GetString(1);
                        result.SourceFileSettings.FileType = Enum.Parse<FileType>(reader.GetString(2));
                    }
                    reader.Close();
                    return result;
                }
                throw new Npgsql.NpgsqlException("Connection not possible");
            }
        }

        public IEnumerable<Script> GetScripts(int maxCount = 10)
        {
            var result = new List<Script>();
            using (var conn = new NpgsqlConnection(connString))
            {
                Console.Out.WriteLine("Opening connection");
                conn.Open();
                using (var command = new NpgsqlCommand($"SELECT \"Name\",\"ScriptString\",\"SourceFileType\" FROM PUBLIC.\"USER_SCRIPT\"  LIMIT {maxCount}", conn))
                {

                    
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var resultEntry = new Script();
                        resultEntry.Name = reader.GetString(0);
                        resultEntry.ScriptString = reader.GetString(1);
                        if(Enum.TryParse<FileType>(reader.GetString(2), false, out var fileType)){
                            resultEntry.SourceFileSettings.FileType = fileType;
                        }
                        result.Add(resultEntry);
                    }
                    reader.Close();
                }
                return result;
            }
        }
    }
}