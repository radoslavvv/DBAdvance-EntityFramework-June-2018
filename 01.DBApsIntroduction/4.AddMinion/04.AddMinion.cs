using System;
using System.Data.SqlClient;

namespace _4.AddMinion
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
                string[] minionInfo = Console.ReadLine()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] villainInfo = Console.ReadLine()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string minionName = minionInfo[1];
                int minionAge = int.Parse(minionInfo[2]);
                string minionTownName = minionInfo[3];

                string villainName = villainInfo[1];

                SqlCommand findTownInDb = new SqlCommand($@"SELECT * FROM Towns WHERE Name='{minionTownName}'", connection);
                if (findTownInDb.ExecuteScalar() == null)
                {
                    SqlCommand addTownToDb = new SqlCommand($@"INSERT INTO Towns VALUES ('{minionTownName}', 1)", connection);
                    addTownToDb.ExecuteNonQuery();

                    Console.WriteLine($"Town {minionTownName} was added to the database.");
                }

                SqlCommand findVillainInDb = new SqlCommand($@"SELECT * FROM Villains WHERE Name='{villainName}'", connection);
                if (findVillainInDb.ExecuteScalar() == null)
                {
                    SqlCommand addVillainToDb = new SqlCommand($@"INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('{villainName}', 1)", connection);
                    addVillainToDb.ExecuteNonQuery();

                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                SqlCommand findVillainId = new SqlCommand($@"SELECT Id FROM Villains WHERE Name='{villainName}'", connection);
                SqlCommand findTownId = new SqlCommand($@"SELECT Id FROM Towns WHERE Name='{minionTownName}'", connection);

                int villainId = (int)findVillainId.ExecuteScalar();
                int townId = (int)findTownId.ExecuteScalar();

                SqlCommand addMinon = new SqlCommand($@"INSERT INTO Minions (Name, Age, TownId) VALUES ('{minionName}', {minionAge}, {townId})", connection);
                addMinon.ExecuteNonQuery();

                SqlCommand findMinionId = new SqlCommand($@"SELECT Id FROM Minions WHERE Name='{minionName}'", connection);
                int minionId = (int)findMinionId.ExecuteScalar();

                SqlCommand addMinionToVillain = new SqlCommand($@"INSERT INTO MinionsVillains (VillainId, MinionId) VALUES ({villainId}, {minionId})", connection);
                try
                {
                    addMinionToVillain.ExecuteNonQuery();
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{minionName} is already added to the database.");
                }
            }
        }
    }
}
