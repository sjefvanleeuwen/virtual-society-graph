using System;
using System.Data.SqlClient;
using Vs.DataProvider.MsSqlGraph;
using Vs.Graph.Core.Data;
using Xunit;
using YamlDotNet.Serialization;

namespace Vs.Graph.Core.Tests
{
    public class DataTests
    {
        [Fact]
        public void Test1()
        {
            // create some entities through built-in data provider
            var yaml = @"Version: 15.0.0.0
Name: person
Attributes:
- Name: FirstName
  Type: Text
- Name: LastName
  Type: Text
- Name: DateOfBirth
  Type: DateTime
Edges:
- Name: recht
  Attributes:
  - Name: periode_begin
    Type: DateTime
  - Name: periode_einde
    Type: DateTime
- Name: married
- Name: friend
";
            var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
            var r = deserializer.Deserialize<NodeSchema>(yaml);
            NodeSchemaScript script = new NodeSchemaScript();
            var s = script.CreateScript(r);

           // s = @"CREATE TABLE Customer (ID INT PRIMARY KEY IDENTITY(1,1), CustName VARCHAR(100)) AS NODE";

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=graph;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(s, connection);
                //command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}",
                            reader["tPatCulIntPatIDPk"], reader["tPatSFirstname"]));// etc
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
        }
    }
}
