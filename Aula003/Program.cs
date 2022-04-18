using System;
using Microsoft.Data.SqlClient;

namespace Aula003
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";

            /*SQLConnection*/
            var connection = new SqlConnection();

            /*Abrir Conexao*/
            connection.Open();

            //---Codigos---//

            /*Fechar conexao*/
            connection.Close();

            /*Destruir e fechar a conexao*/
            connection.Dispose();

            //Exemplo usando o bloco using
            using (var connectionExemple = new SqlConnection(connectionString))
            {
                //Este bloco ja abre a conexão e logo ao terminar, fecha a conexão;
                Console.WriteLine("Conectado");
            }

                Console.WriteLine("Hello World!");
        }
    }
}
