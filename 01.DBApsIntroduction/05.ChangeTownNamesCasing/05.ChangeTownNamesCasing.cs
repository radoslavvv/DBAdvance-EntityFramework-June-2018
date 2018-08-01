using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _05.ChangeTownNamesCasing
{
    public class Program
    {
        public static void Main()
        {
            SqlConnection connection = new SqlConnection(
               @"Server=KING\SQLEXPRESS; " +
               @"Database=MinionsDB; " +
               @"Integrated Security=true");

            connection.Open();
            using (connection)
            {
                string countryName = Console.ReadLine();
                SqlCommand findCountryId = new SqlCommand($@"SELECT Id FROM Countries WHERE Name = '{countryName}'", connection);

                if (findCountryId.ExecuteScalar() == null)
                {
                    Console.WriteLine($"No town names were affected.");
                }
                else
                {
                    int countryId = (int)findCountryId.ExecuteScalar();
                    SqlCommand findCountryTowns = new SqlCommand($@"SELECT t.Name FROM Countries AS c JOIN Towns AS t ON t.CountryCode = c.Id WHERE c.Id = {countryId}", connection);

                    List<string> towns = ReadTownNames(findCountryTowns);

                    if (towns.Count == 0)
                    {
                        Console.WriteLine($"No town names were affected.");
                    }
                    else
                    {
                        SqlCommand changeTownNamesToUpperCase = new SqlCommand($@"UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = {countryId}", connection);

                        int townsCount = changeTownNamesToUpperCase.ExecuteNonQuery();
                        Console.WriteLine($"{townsCount} town names were affected.");
                        Console.WriteLine($"[{string.Join(", ", towns.Select(t => t.ToUpper()).ToArray())}]");
                    }
                }
            }
        }
        private static List<string> ReadTownNames(SqlCommand command)
        {
            SqlDataReader dataReader = command.ExecuteReader();

            List<string> towns = new List<string>();
            while (dataReader.Read())
            {
                string townName = (string)dataReader["Name"];
                towns.Add(townName);
            }

            dataReader.Close();

            return towns;
        }
    }
}

