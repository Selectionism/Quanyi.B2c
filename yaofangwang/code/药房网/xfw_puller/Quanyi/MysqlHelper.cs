using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Quanyi
{
    public class MysqlHelper : DbHelper
    {
        //public static readonly MysqlHelper Instance = new MysqlHelper();

        public static string GetConnectionString(string charset = "gbk")
        {
            string path;
            if (Environment.Is64BitOperatingSystem)
            {
                path = @"SOFTWARE\WOW6432Node\ODBC\ODBC.INI\mysqlpos";
            }
            else
            {
                path = @"SOFTWARE\ODBC\ODBC.INI\mysqlpos";
            }
            using (var key = Registry.LocalMachine.OpenSubKey(path))
            {
                string server, userID, password, database;
                uint port = 3306;
                if (key == null)
                {
                    //当前是远程，后面改为连接本地
                    //server = "localhost";
                    server = "172.19.1.56";
                    userID = "pos";
                    password = "mainpos1234";
                    database = "pos";
                    port = 3306;
                }
                else
                {
                    server = GetRegistryValue(key, "SERVER");
                    userID = GetRegistryValue(key, "UID");
                    password = GetRegistryValue(key, "PWD");
                    database = GetRegistryValue(key, "DATABASE");
                    if (uint.TryParse(GetRegistryValue(key, "PORT"), out uint p))
                    {
                        port = p;
                    }
                }
                var cb = new MySqlConnectionStringBuilder
                {
                    Server = server,
                    UserID = userID,
                    Password = password,
                    Database = database,
                    Port = port,
                    CharacterSet = charset,
                };
                cb.SslMode = MySqlSslMode.None;
                return cb.ToString();
            }
        }

        private static string GetRegistryValue(RegistryKey key, string name)
        {
            return key.GetValue(name)?.ToString();
        }

        public MysqlHelper(string charset = "gbk")
            : base(new MySqlClientFactory(), GetConnectionString(charset))
        {
        }
    }
}
