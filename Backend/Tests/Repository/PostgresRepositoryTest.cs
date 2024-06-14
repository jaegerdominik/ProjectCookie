using NUnit.Framework;
using ProjectCookie.DAL.Entities;

namespace ProjectCookie.Tests.Repository;

public class PostgresRepositoryTest : BaseUnitTest
{
    [Test]
    public Task TestFilterBy()
    {
        User user = new User();
        user.Username = "FilterBy";

        UnitOfWork.Users.InsertAsync(user).Wait();
        UnitOfWork.Users.InsertAsync(user).Wait();
        UnitOfWork.Users.InsertAsync(user).Wait();
        
        Assert.That(UnitOfWork.Users.FilterBy(doc => doc.Username == "FilterBy").Count(), Is.EqualTo(3));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFilterByConvert()
    {
        User user = new User();
        user.Username = "ConvertBy";

        UnitOfWork.Users.InsertAsync(user).Wait();
        var result = UnitOfWork.Users.FilterBy(doc => doc.Username == "ConvertBy", doc => doc);
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFindOneAsync()
    {
        User user = new User();
        user.Username = "FindOne";

        UnitOfWork.Users.InsertAsync(user).Wait();
        var result = UnitOfWork.Users.FindAsync(doc => doc.Username == "FindOne").Result;
        
        Assert.That(result.Username, Is.EqualTo(user.Username));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestFindByIdAsync()
    {
        User user = new User();
        user.Username = "FindOneByID";

        UnitOfWork.Users.InsertAsync(user).Wait();

        var getItem = UnitOfWork.Users.FindAsync(doc => doc.Username == "FindOneByID").Result;
        var result = UnitOfWork.Users.FindByIdAsync(getItem.ID).Result;
        
        Assert.That(result.Username, Is.EqualTo(user.Username));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestInsertOneAsync()
    {
        User user = new User();
        user.Username = "InsertOne";

        UnitOfWork.Users.InsertAsync(user).Wait();
        var result = UnitOfWork.Users.FilterBy(doc => doc.Username == "InsertOne");
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestUpdateOneAsync()
    {
        User user = new User();
        user.Username = "UpdateOne";

        UnitOfWork.Users.InsertAsync(user).Wait();

        user.Username = "UpdateOneMore";
        UnitOfWork.Users.UpdateAsync(user);
        var result = UnitOfWork.Users.FindAsync(doc => doc.Username == "UpdateOneMore").Result;
        
        Assert.That(result.Username, Is.EqualTo("UpdateOneMore"));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteOneAsync()
    {
        User user = new User();
        user.Username = "DeleteOne";

        UnitOfWork.Users.InsertAsync(user).Wait();
        UnitOfWork.Users.DeleteAsync(doc => doc.Username == "DeleteOne");
        var result = UnitOfWork.Users.FilterBy(doc => doc.Username == user.Username);
        
        Assert.That(result.Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteByIdAsync()
    {
        User user = new User();
        user.Username = "DeleteOneByID";

        UnitOfWork.Users.InsertAsync(user).Wait();

        var getItem = UnitOfWork.Users.FindAsync(doc => doc.Username == "DeleteOneByID").Result;
        var result = UnitOfWork.Users.DeleteByIdAsync(getItem.ID);
        
        Assert.That(UnitOfWork.Users.FilterBy(doc => doc.Username == user.Username).Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
}