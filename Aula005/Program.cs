using Microsoft.Data.SqlClient;
using Dapper; //Importação Dapper
using System;
using Aula005.Models;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Aula005
{
    class Program
    {
        //Dapper
        /* Usei este projeto para escrever todo o codigo do modulo Dapper e Imersão do curso*/

        static void Main(string[] args)
        {
            string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";
            //const string connectionString = "Server=localhost,1433;Database=balta;Integrated Security=SSPI;";

            using (var connection = new SqlConnection(connectionString))
            {
                //UpdateCategory(connection);
                //ListCategories(connection);
                //CreateCategory(connection);
                //CreateManyCategory(connection);
                //ExecuteProcedure(connection);
                //ExecuteReadProcedure(connection);
                //ExecuteScalar(connection);
                //ReadView(connection);
                //OneToOne(connection);
                //OneToMany(connection);
                //QueryMultile(connection);
                //SelectIn(connection);
                //Like(connection);
                //Transaction(connection);
            }
        }

        //Metodos passados nas aulas;
        #region Metodos
        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECY [Id], [Title] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void CreateCategory(SqlConnection connection)
        {
            #region Insert
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            var insertSQL = $@"INSERT INTO [Category] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";
            #endregion

            var rows = connection.Execute(insertSQL, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"{rows} - Linhas inseridas");
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
                title = "Frontend 2022"
            });

            Console.WriteLine($"{rows} - Registros atualizadas");
        }

        static void CreateManyCategory(SqlConnection connection)
        {
            #region Insert Categoria
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            #endregion

            #region Insert Categoria 2
            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "categoria_nova";
            category2.Description = "Categoria Nova";
            category2.Order = 9;
            category2.Summary = "Categoria";
            category2.Featured = true;
            #endregion

            var insertSQL = $@"INSERT INTO [Category] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";

            var rows = connection.Execute(insertSQL, new[]
            {
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });

            Console.WriteLine($"{rows} - Linhas inseridas");
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "[spDeleteStudent]";
            var pars = new { StudentId = "" /*<= Fazer um insert aqui*/ };
            var affectedRows = connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);
            Console.WriteLine($"{affectedRows} - Linhas afetadas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[spGetCoursesByCategory]";
            var pars = new { CategoryId = "" /*<= Fazer um insert aqui*/ };
            var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);

            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id}");
            }

        }

        static void ExecuteScalar(SqlConnection connection)
        {
            #region Insert
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            var insertSQL = $@"INSERT INTO [Category] OUTPUT inserted.[Id] VALUES(NEWID(), @Title, @Url, @Summary, @Order, @Description, @Featured)";
            #endregion

            var id = connection.ExecuteScalar<Guid>(insertSQL, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"A categoria inserida foi: {id}");
        }

        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void OneToOne(SqlConnection connection)
        {
            var sql = @"SELECT * FROM [CareerItem] INNER JOIN [Courses] ON [CareerItem].[CoursesId] = [Courses].[Id]";
            var items = connection.Query<CareerItem, Courses, CareerItem>(
                sql,
                (careerItem, course) =>
                {
                    careerItem.Courses = course;
                    return careerItem;
                }, splitOn: "Id");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title} - Course: {item.Courses.Title}");
            }
        }

        static void OneToMany(SqlConnection connection)
        {
            var sql = @"SELECT 
                            [Career].[Id],
                            [Career].[Title],
                            [CareerItem].[CareerId],
                            [CareerItem].[Title]
                        FROM
                            [Career]
                        INNER JOIN
                            [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                        ORDER BY
                            [Career].[Title]";

            var carrers = new List<Career>();

            var items = connection.Query<Career, CareerItem, Career>(
                sql,
                (career, item) =>
                {
                    var car = carrers.Where(x => x.Id == career.Id).FirstOrDefault();
                    if(car == null)
                    {
                        car = career;
                        car.Items.Add(item);
                        carrers.Add(car);
                    }
                    else
                    {
                        car.Items.Add(item);
                    }
                    return career;
                }, splitOn: "CareerId");
            foreach (var career in items)
            {
                Console.WriteLine($"{career.Title}");
                foreach (var item in career.Items)
                {
                    Console.WriteLine($" - {item.Title}");
                }
            }
        }

        static void QueryMultile(SqlConnection connection)
        {
            var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

            using(var multi = connection.QueryMultiple(query))
            {
                var categories = multi.Read<Category>();
                var courses = multi.Read<Courses>();

                foreach(var item in categories)
                {
                    Console.WriteLine(item.Title);
                }

                foreach (var item in courses)
                {
                    Console.WriteLine(item.Title);
                }
            }
        }

        static void SelectIn(SqlConnection connection)
        {
            var query = @"select * from Career where [Id] IN (@Id)";

            var items = connection.Query<Career>(query, new 
            {
                Id = new[]
                {
                    "Guid do banco",
                    "Guid do banco"
                }
            });

            foreach(var item in items)
            {
                Console.WriteLine(item.Title);
            }
        }

        static void Like(SqlConnection connection)
        {
            var term = "api";
            var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";

            var items = connection.Query<Courses>(query, new
            {
                exp = $"%{term}%"
            });

            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
            }
        }

        static void Transaction(SqlConnection connection)
        {
            #region Insert
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Minha Categoria - não salvar";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;
            var insertSQL = $@"INSERT INTO [Category] VALUES(@Id, @Title, @Url, @Summary, @Order, @Description, @Featured)";
            #endregion

            using(var transaction = connection.BeginTransaction())
            {
                var rows = connection.Execute(insertSQL, new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }, transaction);
                //transaction.Commit();
                transaction.Rollback();

                Console.WriteLine($"{rows} - Linhas inseridas");
            }
        }
        #endregion
    }
}
