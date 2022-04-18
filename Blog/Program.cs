using System;
using Microsoft.Data.SqlClient;
using Dapper;
using Dapper.Contrib;
using Blog.Models;
using Dapper.Contrib.Extensions;

namespace Blog
{
    class Program
    {
        private const string CONNECTION_STRING = @"Server=localhost,1433;Database=Blog;User ID=sa;Password=1q2w3e4r@#$";

        static void Main(string[] args)
        {
            //ReadUsers();
            //ReadUser();
            //CreateUser();
            //UpdateUser();
            //DeleteUser();
        }

        public static void ReadUsers()
        {
            using(var connection = new SqlConnection(CONNECTION_STRING))
            {
                var users = connection.GetAll<User>();

                foreach(var user in users)
                {
                    Console.WriteLine(user.Name);
                }
            }
        }

        public static void ReadUser()
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var user = connection.Get<User>(1);
                Console.WriteLine(user.Name);
            }
        }

        public static void CreateUser()
        {
            var user = new User()
            {
                Bio="Equipe GameDev",
                Email = "GameDev@mail.com",
                Image = "https://",
                Name = "Equipe GameDev2022",
                PasswordHash = "HASH",
                Slug = "equipe-gamedev"
            };

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Insert<User>(user);
                Console.WriteLine("Cadastro realizado com sucesso");
            }
        }

        public static void UpdateUser()
        {
            var user = new User()
            {
                Id = 2,
                Bio = "Equipe | GameDev",
                Email = "GameDev@mail.com",
                Image = "https://",
                Name = "Equipe de suporte GameDev2022",
                PasswordHash = "HASH",
                Slug = "equipe-gamedev"
            };

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Update<User>(user);
                Console.WriteLine("Atualização realizado com sucesso");
            }
        }

        public static void DeleteUser()
        {
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                var user = connection.Get<User>(2);
                connection.Delete<User>(user);
                Console.WriteLine("Exclusão realizado com sucesso");
            }
        }
    }
}
