using Microsoft.Data.SqlClient;
using System;

namespace Aula004
{
    class Program
    {
        const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";

        static void Main(string[] args)
        {
            
            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Conectado");
                connection.Open();

                //Select
                using (var command = new SqlCommand())
                {
                    //Criar o comando
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT [Id], [Title] FROM [Category]";

                    //Executar o comando
                    var reader = command.ExecuteReader();

                    //Percorrer
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}");
                    }
                }
            }

            Console.WriteLine("Hello World!");
        }
    }
}
