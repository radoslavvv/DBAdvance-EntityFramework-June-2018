using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _07.PrintAllMinionnames
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
                SqlCommand findAllMinionNames = new SqlCommand($@"SELECT Name FROM Minions", connection);
                SqlDataReader dataReader = findAllMinionNames.ExecuteReader();

                List<string> minionNames = new List<string>();
                while (dataReader.Read())
                {
                    string minionName = (string)dataReader["Name"];

                    minionNames.Add(minionName);
                }
                dataReader.Close();

                int index = 0;
                int counter = 0;
                int minionNamesCount = minionNames.Count;
                while (counter < minionNamesCount)
                {
                    Console.WriteLine(minionNames[0 + index]);
                    if (counter + 1 == minionNamesCount)
                    {
                        break;
                    }

                    Console.WriteLine(minionNames[minionNamesCount - 1 - index]);

                    counter += 2;
                    index++;
                }
            }
        }
    }
}

