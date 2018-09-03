using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SQLite;
using System.IO;

namespace StudentDataService.Model
{
    public class Database
    {

        public static SQLiteConnection GetConnection()
        {
            var app_data = HttpContext.Current.Server.MapPath("~/App_Data");
            var db_file = "studentservice.db";
            var db_path = Path.Combine(app_data, db_file);
            var connection_string = $"Data Source={db_path}";
            return new SQLiteConnection(connection_string);
        }
        public static void CreateDatabase()
        {
            var app_data = HttpContext.Current.Server.MapPath("~/App_Data");
            var script_file = "create.sql";
            var script_path = Path.Combine(app_data, script_file);
            var query = File.ReadAllText(script_path);
            GetConnection().Execute(query);
        }
    }
}