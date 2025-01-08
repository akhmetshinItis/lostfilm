using System.Data;
using Moq;
using MyORMLibrary;
using MyORMLibraryTests.Models;

namespace MyORMLibraryTests;

[TestFixture]
public class ORMContextTests
{
    private Mock<IDbConnection> _dbConnection = new();
    private Mock<IDbCommand> _dbCommand = new();
    private Mock<IDataReader> _dbDataReader = new();
    private Mock<IDbDataParameter> _dataParameter = new();
    private Mock<IDataParameterCollection> _dataParameterCollection = new();
    private ORMContext<UserInfo> _dbContext;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ORMContext<UserInfo>(_dbConnection.Object);
    }

    [Test]
    public void GetById_When_()
    {
        // Arrange
        var userId = 1;
        var userInfo = new UserInfo()
        {
            Id = 1,
            Age = 20,
            Email = "example@test.com",
            Name = "Иванов Иван Иванович",
            Gender = 1
        };

        var data = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                { "Id", userInfo.Id },
                { "Age", userInfo.Age },
                { "Email", userInfo.Email },
                { "Name", userInfo.Name },
                { "Gender", userInfo.Gender }
            }
        };
        
        // Настройка возвращаемого IDataReader
        _dbDataReader.SetupSequence(r => r.Read())
            .Returns(true)
            .Returns(false);
        _dbDataReader.Setup(r => r["Id"]).Returns(userInfo.Id);
        _dbDataReader.Setup(r => r["Age"]).Returns(userInfo.Age);
        _dbDataReader.Setup(r => r["Email"]).Returns(userInfo.Email);
        _dbDataReader.Setup(r => r["Name"]).Returns(userInfo.Name);
        _dbDataReader.Setup(r => r["Gender"]).Returns(userInfo.Gender);
        // Настройка метода ExecuteReader
        _dbCommand.Setup(c => c.ExecuteReader()).Returns(_dbDataReader.Object);
        // Настройка CreateParameter
        _dbCommand.Setup(c => c.CreateParameter()).Returns(_dataParameter.Object);
        // Настройка Parameters
        _dbCommand.Setup(c => c.Parameters).Returns(_dataParameterCollection.Object);
        _dataParameterCollection.Setup(pc => pc.Add(It.IsAny<object>())).Returns(userId); // настройка Add для Parameters
        // Настройка CreateCommand в Connection
        _dbConnection.Setup(c => c.CreateCommand()).Returns(_dbCommand.Object);
        
        // Act
        var result = _dbContext.GetById(userId);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userInfo.Id, result.Id);
        Assert.AreEqual(userInfo.Age, result.Age);
        Assert.AreEqual(userInfo.Email, result.Email);
        Assert.AreEqual(userInfo.Name, result.Name);
        Assert.AreEqual(userInfo.Gender, result.Gender);



    }
}