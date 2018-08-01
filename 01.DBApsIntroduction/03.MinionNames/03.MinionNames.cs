using System;
using System.Data.SqlClient;

namespace _03.MinionNames
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(
                @"Server=KING\SQLEXPRESS; " +
                @"Database=MinionsDB; " +
                @"Integrated Security=true");

            connection.Open();
            using (connection)
            {
                int villainId = int.Parse(Console.ReadLine());

                SqlCommand getVillainName = new SqlCommand($@"SELECT Name FROM Villains WHERE Id = {villainId}", connection);

                string villainName = (string)getVillainName.ExecuteScalar();
                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {villainName}");

                    SqlCommand getMinionsData = new SqlCommand(
                        $@"SELECT m.Name, m.Age
                        FROM Minions AS m
                        JOIN MinionsVillains AS mv
                        ON mv.MinionId = m.Id
                        JOIN Villains AS v
                        ON v.Id = mv.VillainId
                        WHERE v.Id = {villainId}
                        ORDER BY v.Name", connection);

                    SqlDataReader dataReader = getMinionsData.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine($"(no minions)");
                    }

                    int counter = 1;
                    while (dataReader.Read())
                    {
                        string minionName = (string)dataReader["Name"];
                        int minionAge = (int)dataReader["Age"];

                        Console.WriteLine($"{counter}. {minionName} {minionAge}");

                        counter++;
                    }
                }
            }
        }
    }
}
