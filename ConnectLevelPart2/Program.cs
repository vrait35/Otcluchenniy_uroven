using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;

namespace ConnectLevelPart2
{
    public class Program
    {
        static void Main(string[] args)
        {
          //  System.Configuration.ConfigurationManager;
            DbProviderFactory factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["ConString"].ProviderName);

            using (var connection = factory.CreateConnection())
            using (var command = factory.CreateCommand())
            {
                connection.Open();
                command.CommandText = "insert into users(id,login,password,updateDateTime)" +
                    "values(@id,@login,@password,@updateDate)";
                var idParameteer = factory.CreateParameter();
                idParameteer.ParameterName = "@id";
                idParameteer.DbType = System.Data.DbType.Int32;
                idParameteer.Value = 9;
                command.Parameters.Add(idParameteer);
                //command.Parameters.Add(factory.CreateParameter
                //{
                //    ParameterName = "@id",
                //    SqlDbType = System.Data.SqlDbType.Int,
                //    Value = 8,
                //    IsNullable = false
                //});
                //command.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "@login",
                //    SqlDbType = System.Data.SqlDbType.VarChar,
                //    Value = "Vasja",
                //    IsNullable = false
                //});
                //command.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "@password",
                //    SqlDbType = System.Data.SqlDbType.VarChar,
                //    Value = "Vasja",
                //    IsNullable = false
                //});
                //command.Parameters.Add(new SqlParameter
                //{
                //    ParameterName = "@updateDate",
                //    SqlDbType = System.Data.SqlDbType.DateTime2,
                //    Value = DateTime.Now
                //});
                command.Connection = connection;

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (DbException ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }
            }            
        }
        public static void ExecuteTransaction(DbConnection connection,params DbCommand [] commands)
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var command in commands)
                    {                        
                        command.Transaction = transaction;
                    }
                    foreach (var command in commands)
                    {
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (DbException ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                }
            }
        }

    }
}
