using System;
using System.Data.SqlClient;

namespace _01.InitialStartup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(
                           @"Server=KING\SQLEXPRESS; " +
                           @"Integrated Security=true");

            connection.Open();
            using (connection)
            {
                SqlCommand createDb = new SqlCommand("CREATE DATABASE MinionsDB", connection);

                connection.ChangeDatabase("MinionsDB");
                string createTableCountries = "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";
                string createTableTowns = "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))";
                string createTableMinions = "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))";
                string createTableEvilnessFactors = "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))";
                string createTableVillains = "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))";
                string createTableMinionsVillains = "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";

                ExecuteNonQuery(createTableCountries, connection);
                ExecuteNonQuery(createTableTowns, connection);
                ExecuteNonQuery(createTableMinions, connection);
                ExecuteNonQuery(createTableEvilnessFactors, connection);
                ExecuteNonQuery(createTableVillains, connection);
                ExecuteNonQuery(createTableMinionsVillains, connection);

                string insertIntoCountries = "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";
                string insertIntoTowns = "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";
                string insertIntoMinions = "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";
                string insertIntoEvilnessFactors = "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";
                string insertIntoVillains = "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";
                string insertIntoMinionsVillains = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";

                ExecuteNonQuery(insertIntoCountries, connection);
                ExecuteNonQuery(insertIntoTowns, connection);
                ExecuteNonQuery(insertIntoMinions, connection);
                ExecuteNonQuery(insertIntoEvilnessFactors, connection);
                ExecuteNonQuery(insertIntoVillains, connection);
                ExecuteNonQuery(insertIntoMinionsVillains, connection);
            }
        }
        private static void ExecuteNonQuery(string commandString, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(commandString, connection);

            command.ExecuteNonQuery();
        }
    }
}
