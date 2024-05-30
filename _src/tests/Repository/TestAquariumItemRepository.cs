using Consul;
using dal;
using dal.interfaces;
using dal.Repositories.interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace tests.Repository;

public class TestAquariumItemRepository : BaseTest
{
    /*
     * Specific Tests
     */
    
    [Test]
    public Task TestGetCorals()
    {
        AquariumItem item = new Coral();
        item.Name = "Coral";
        item.Species = "Coral";
        
        UnitOfWork.AquariumItem.InsertOrUpdateOneAsync(item).Wait();
        Assert.That(UnitOfWork.AquariumItem.GetCorals().Count(), Is.EqualTo(1));

        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestGetAnimals()
    {
        AquariumItem item = new Animal();
        item.Name = "Animal";
        item.Species = "Animal";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        Assert.That(UnitOfWork.AquariumItem.GetAnimals().Count(), Is.EqualTo(1));
        
        return Task.CompletedTask;
    }

    [Test]
    public Task TestGetCoralsInMixedList()
    {
        Assert.That(UnitOfWork.AquariumItem.GetCorals().Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestGetAnimalsInMixedList()
    {
        Assert.That(UnitOfWork.AquariumItem.GetAnimals().Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    /*
     * Base Class Tests
     */

    [Test]
    public Task TestFilterBy()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "FilterBy";
        item.Species = "FilterBy";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Species == "FilterBy").Count(), Is.EqualTo(3));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestFilterByConvert()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "ConvertBy";
        item.Species = "ConvertBy";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        var result = UnitOfWork.AquariumItem.FilterBy(doc => doc.Name == "ConvertBy", doc => doc);
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFindOneAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "FindOne";
        item.Species = "FindOne";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        var result = UnitOfWork.AquariumItem.FindOneAsync(doc => doc.Name == "FindOne").Result;
        
        Assert.That(result.Species, Is.EqualTo("FindOne"));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestFindByIdAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "FindOneByID";
        item.Species = "FindOneByID";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        var getItem = UnitOfWork.AquariumItem.FindOneAsync(doc => doc.Name == "FindOneByID").Result;
        var result = UnitOfWork.AquariumItem.FindByIdAsync(getItem.ID).Result;
        
        Assert.That(result.Name, Is.EqualTo("FindOneByID"));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestInsertOneAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "InsertOne";
        item.Species = "InsertOne";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Name == "InsertOne").Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestUpdateOneAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "UpdateOne";
        item.Species = "UpdateOne";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();

        item.Species = "Updated";
        UnitOfWork.AquariumItem.UpdateOneAsync(item);
        var result = UnitOfWork.AquariumItem.FindOneAsync(doc => doc.Name == "UpdateOne").Result;
        
        Assert.That(result.Species, Is.EqualTo("Updated"));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestInsertOrUpdateOneAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "Update";
        item.Species = "Update";
        
        AquariumItem item2 = new AquariumItem();
        item2.Name = "Insert";
        item2.Species = "InsertOrUpdate";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();

        item.Species = "InsertOrUpdate";
        UnitOfWork.AquariumItem.InsertOrUpdateOneAsync(item);
        UnitOfWork.AquariumItem.InsertOrUpdateOneAsync(item2);
        
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Species == "InsertOrUpdate").Count(), Is.EqualTo(2));
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Species == "Update").Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteOneAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "DeleteOne";
        item.Species = "DeleteOne";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();

        int number = UnitOfWork.AquariumItem.FilterBy(doc => doc.Name == "DeleteOne").Count();
        UnitOfWork.AquariumItem.DeleteOneAsync(doc => doc.Name == "DeleteOne");
        
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Name == "DeleteOne").Count(), Is.EqualTo(number - 1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteByIdAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "DeleteByID";
        item.Species = "DeleteByID";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Name == "DeleteByID").Count(), Is.EqualTo(1));
        
        var getItem = UnitOfWork.AquariumItem.FindOneAsync(doc => doc.Name == "DeleteByID").Result;
        var result = UnitOfWork.AquariumItem.DeleteByIdAsync(getItem.ID);
        
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Name == "DeleteByID").Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteManyAsync()
    {
        AquariumItem item = new AquariumItem();
        item.Name = "DeleteMany";
        item.Species = "DeleteMany";

        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        UnitOfWork.AquariumItem.InsertOneAsync(item).Wait();
        
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Species == "DeleteMany").Count(), Is.EqualTo(3));
        
        UnitOfWork.AquariumItem.DeleteManyAsync(doc => doc.Species == "DeleteMany").Wait();
        
        Assert.That(UnitOfWork.AquariumItem.FilterBy(doc => doc.Species == "DeleteMany").Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
}