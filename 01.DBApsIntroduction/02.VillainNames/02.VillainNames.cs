using System;
using System.Data.SqlClient;

namespace _01.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SqlConnection dbContext = new SqlConnection(
                @"Server=KING\SQLEXPRESS; " +
                @"Database=MinionsDB; " +
                @"Integrated Security=true");

            dbContext.Open();

            using (dbContext)
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT v.Name, COUNT(m.Id) AS MinionsCount
                        FROM MinionsVillains AS mv
                        JOIN Minions AS m
                        ON m.Id = mv.MinionId
                        JOIN Villains AS v
                        ON v.Id = mv.VillainId
                        GROUP BY v.Name
                        HAVING COUNT(m.Id) > 3", dbContext);

                var dataReader = command.ExecuteReader();
                if (!dataReader.HasRows)
                {
                    Console.WriteLine("No results :(");
                }
                else
                {
                    while (dataReader.Read())
                    {
                        string villainName = (string)dataReader["Name"];
                        int minionsCount = (int)dataReader["MinionsCount"];

                        Console.WriteLine($"{villainName} -> {minionsCount}");
                    }
                }
            }
        }
    }
}
