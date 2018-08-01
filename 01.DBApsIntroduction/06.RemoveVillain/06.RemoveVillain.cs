using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _06.RemoveVillain
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
                SqlCommand findVillain = new SqlCommand($@"SELECT Name FROM Villains WHERE Id = {villainId}", connection);

                string villainName = (string)findVillain.ExecuteScalar();
                if (villainName == null)
                {
                    Console.WriteLine($"No such villains were found!");
                }
                else
                {
                    SqlCommand findMinionsIds = new SqlCommand($@"SELECT MinionId FROM MinionsVillains WHERE VillainId = {villainId}", connection);
                    List<int> minionsIds = GetMinionsIds(findMinionsIds);

                    DeleteVillain(connection, villainId);
                    ReleaseMinions(connection, minionsIds);

                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{minionsIds.Count} minions were released.");
                }
            }
        }

        private static void ReleaseMinions(SqlConnection connection, List<int> minionsIds)
        {
            foreach (var minionId in minionsIds)
            {
                SqlCommand releaseMinion = new SqlCommand($@"DELETE FROM Minions WHERE Id = {minionId}", connection);
            }
        }

        private static void DeleteVillain(SqlConnection connection, int villainId)
        {
            SqlCommand deteleVillainFromMapTable = new SqlCommand($@"DELETE FROM MinionsVillains WHERE VillainId = {villainId}", connection);
            deteleVillainFromMapTable.ExecuteNonQuery();

            SqlCommand deteleVillainFromVillainsTable = new SqlCommand($@"DELETE FROM Villains WHERE Id = {villainId}", connection);
            deteleVillainFromVillainsTable.ExecuteNonQuery();
        }

        private static List<int> GetMinionsIds(SqlCommand findMinionsIds)
        {
            SqlDataReader dataReader = findMinionsIds.ExecuteReader();
            List<int> minionsIds = new List<int>();
            while (dataReader.Read())
            {
                int minionId = (int)dataReader["MinionId"];
                minionsIds.Add(minionId);
            }
            dataReader.Close();
            return minionsIds;
        }
    }
}

