using System;
using System.Data.SqlClient;

namespace _09.IncreaseAgeStoredProcedure
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
                int minionId = int.Parse(Console.ReadLine());

                SqlCommand changeMinionAge = new SqlCommand($@"EXEC usp_GetOlder {minionId}", connection);
                changeMinionAge.ExecuteNonQuery();

                SqlCommand getMinionDetails = new SqlCommand($@"SELECT Name, Age FROM Minions WHERE Id = {minionId}", connection);

                SqlDataReader dataReader = getMinionDetails.ExecuteReader();
                while (dataReader.Read())
                {
                    string minionName = (string)dataReader["Name"];
                    int minionAge = (int)dataReader["Age"];


                    Console.WriteLine($"{minionName} - {minionAge} years old");
                }
            }
        }
    }
}
