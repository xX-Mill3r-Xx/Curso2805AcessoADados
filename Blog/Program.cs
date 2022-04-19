using System;
using Microsoft.Data.SqlClient;
using Dapper;
using Dapper.Contrib;
using Blog.Models;
using Dapper.Contrib.Extensions;
using Blog.Repositories;

namespace Blog
{
    class Program
    {
        private const string CONNECTION_STRING = @"Server=localhost,1433;Database=Blog;User ID=sa;Password=1q2w3e4r@#$";

        static void Main(string[] args)
        {
            #region Console definitions
            Console.Title = "Blog";
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            #endregion

            #region Conection
            var connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();
            ReadUsers(connection);
            ReadRoles(connection);
            ReadTags(connection);
            connection.Close();
            #endregion
        }

        #region Methods
        public static void ReadUsers(SqlConnection connection)
        {
            var repository = new Repository<User>(connection);
            var items = repository.Get();
            foreach (var item in items)
            {
                Console.WriteLine(item.Name);
                foreach (var role in item.Roles)
                {
                    Console.WriteLine($" - {role.Name}");
                }
            }
        }

        public static void ReadRoles(SqlConnection connection)
        {
            var repository = new Repository<Role>(connection);
            var items = repository.Get();
            foreach (var item in items)
            {
                Console.WriteLine(item.Name);
            }
        }

        public static void ReadTags(SqlConnection connection)
        {
            var repository = new Repository<Tag>(connection);
            var items = repository.Get();
            foreach (var item in items)
            {
                Console.WriteLine(item.Name);
            }
        }
        #endregion

    }
}
