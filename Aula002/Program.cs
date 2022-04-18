using System;
using Microsoft.Data.SqlClient; //Pacote Importado para uso de banco de dados SQLServer

namespace Aula002
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";
            //Microsoft.Data.SqlClient (NUGET)

            Console.WriteLine("Hello World!");
        }
    }
}
