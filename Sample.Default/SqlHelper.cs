using System;
using System.Data.Common;
using System.Data.SqlClient;

public static class SqlHelper
{
    public static void ExecuteScript(string connectionString, string cmdText)
    {
        using (var con = new SqlConnection(connectionString))
        {
            con.Open();
            using (var command = new SqlCommand(cmdText, con))
            {
                command.ExecuteNonQuery();
            }
            con.Close();
        }
    }

    public static void CreateDatabase(string connectionString)
    {
        try
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            object catalog;

            if (builder.ContainsKey("database"))
            {
                catalog = builder["database"];
                builder.Remove("database");
            }
            else if (builder.ContainsKey("initial catalog"))
            {
                catalog = builder["initial catalog"];
                builder.Remove("initial catalog");
            }
            else
            {
                return;
            }

            var master = builder.ToString();
            ExecuteScript(master, $"IF NOT exists(select * from sys.databases where name='{catalog}') CREATE DATABASE [{catalog}]");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Could not create database.", ex);
        }
    }
}
