using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace _08.IncreaseMinionAge
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
                int[] minionsIds = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                foreach (var minionId in minionsIds)
                {
                    SqlCommand getMinionName = new SqlCommand($@"SELECT Name FROM Minions WHERE Id = {minionId}", dbContext);

                    string minionName = (string)getMinionName.ExecuteScalar();
                    minionName = ConvertToTitleCase(minionName);

                    SqlCommand command = new SqlCommand($@"UPDATE Minions SET Age = Age + 1, Name = '{minionName}' WHERE Id = {minionId}", dbContext);
                    command.ExecuteNonQuery();
                }

                SqlCommand getAllMinionsNames = new SqlCommand($@"SELECT Name, Age FROM Minions", dbContext);
                SqlDataReader dataReader = getAllMinionsNames.ExecuteReader();

                while (dataReader.Read())
                {
                    string name = (string)dataReader["Name"];
                    int age = (int)dataReader["Age"];
                    Console.WriteLine($"{name} {age}");
                }
            }
        }

        private static string ConvertToTitleCase(string minionName)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            minionName = textInfo.ToTitleCase(minionName);
            return minionName;
        }
    }
}
