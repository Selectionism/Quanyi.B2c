using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Quanyi
{
    public class DbHelper : IDisposable
    {
        public DbProviderFactory Factor { get; private set; }

        private DbConnection _conn;

        public DbHelper(DbProviderFactory factory, string connectionString)
        {
            Factor = factory;
            _conn = factory.CreateConnection();
            _conn.ConnectionString = connectionString;
        }

        public object ExecuteScalar(string sql)
        {
            return GetDbCommand(sql).ExecuteScalar();
        }

        public int ExecuteNonQuery(string sql)
        {
            return GetDbCommand(sql).ExecuteNonQuery();
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {
            return GetDbCommand(sql, dbParameters).ExecuteNonQuery();
        }

        public DbCommand GetDbCommand()
        {
            var cmd = _conn.CreateCommand();
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
            return cmd;
        }

        public DbCommand GetDbCommand(string sql)
        {
            var cmd = GetDbCommand();
            cmd.CommandText = sql;
            return cmd;
        }

        public DbCommand GetDbCommand(string sql, params DbParameter[] dbParameters)
        {
            var cmd = GetDbCommand(sql);
            foreach (var item in dbParameters)
            {
                cmd.Parameters.Add(item);
            }
            return cmd;
        }

        public DbCommand GetDbCommand(string sql, params string[] parameters)
        {
            var cmd = GetDbCommand(sql);
            foreach (var item in parameters)
            {
                AddParameter(cmd, item);
            }
            return cmd;
        }

        public DbParameter AddParameter(DbCommand cmd, string parameterName)
        {
            var p = CreateParameter();
            p.ParameterName = parameterName;
            //p.Value = "";
            //p.DbType = DbType.String;
            //p.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(p);
            return p;
        }

        public DbParameter CreateParameter()
        {
            return Factor.CreateParameter();
        }

        public DataSet Fill(DbCommand cmd)
        {
            using (var da = Factor.CreateDataAdapter())
            {
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public DataSet Fill(string sql)
        {
            return Fill(GetDbCommand(sql));
        }

        public DataTable FillTable(DbCommand cmd)
        {
            return FillTable(cmd, new DataTable());
        }

        public DataTable FillTable(DbCommand cmd, DataTable dt)
        {
            using (var da = Factor.CreateDataAdapter())
            {
                da.SelectCommand = cmd;
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable FillTable(string sql, string tableName = "")
        {
            var dt = FillTable(GetDbCommand(sql));
            dt.TableName = tableName;
            return dt;
        }

        public DbDataAdapter FillTableSchema(DataTable dt)
        {
            if (string.IsNullOrEmpty(dt.TableName))
            {
                throw new ArgumentNullException("dt.TableName");
            }
            var da = Factor.CreateDataAdapter();
            da.SelectCommand = GetDbCommand("SELECT * FROM " + dt.TableName);
            da.FillSchema(dt, SchemaType.Mapped);
            return da;
        }

        public virtual void TruncateTable(string tableName)
        {
            ExecuteNonQuery("TRUNCATE TABLE " + tableName);
        }

        public DbTransaction BeginTransaction()
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
            return _conn.BeginTransaction();
        }

        public void Commit(DbTransaction dbTran)
        {
            if (dbTran.Connection.State != ConnectionState.Open)
            {
                dbTran.Connection.Open();
            }
        }

        public void RollBack(DbTransaction dbTran)
        {
            if (dbTran.Connection.State != ConnectionState.Open)
            {
                dbTran.Connection.Open();
            }
        }

        public virtual void Dispose()
        {
            _conn.Dispose();
        }
    }
}
