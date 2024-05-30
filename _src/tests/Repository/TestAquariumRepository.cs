using dal;
using dal.enums;

namespace tests.Repository;

public class TestAquariumRepository : BaseTest
{
    /*
     * Specific Tests
     */
    
    [Test]
    public Task TestGetByName()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "Aquarium";

        UnitOfWork.Aquarium.InsertOneAsync(aquarium);
        var result = UnitOfWork.Aquarium.GetByName("Aquarium").Result;
        Assert.That(result.Name, Is.EqualTo("Aquarium"));

        return Task.CompletedTask;
    }
    
    /*
     * Base Class Tests
     */
    
    [Test]
    public Task TestFilterBy()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "FilterBy";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();

        var result = UnitOfWork.Aquarium.FilterBy(doc => doc.Name == "FilterBy");
        
        Assert.That(result.Count(), Is.EqualTo(3));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFilterByConvert()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "ConvertBy";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        var result = UnitOfWork.Aquarium.FilterBy(doc => doc.Name == "ConvertBy", doc => doc);
        
        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }

    [Test]
    public Task TestFindOneAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "FindOne";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        var result = UnitOfWork.Aquarium.FindOneAsync(doc => doc.Name == "FindOne").Result;
        
        Assert.That(result.Name, Is.EqualTo(aquarium.Name));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestFindByIdAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "FindOneByID";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        var getItem = UnitOfWork.Aquarium.FindOneAsync(doc => doc.Name == "FindOneByID").Result;
        var result = UnitOfWork.Aquarium.FindByIdAsync(getItem.ID).Result;
        
        Assert.That(result.Name, Is.EqualTo(aquarium.Name));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestInsertOneAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "InsertOne";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        var result = UnitOfWork.Aquarium.FilterBy(doc => doc.Name == aquarium.Name);

        Assert.That(result.Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestUpdateOneAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "UpdateOne";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();

        aquarium.WaterType = WaterType.Saltwater;
        UnitOfWork.Aquarium.UpdateOneAsync(aquarium);
        var result = UnitOfWork.Aquarium.FindOneAsync(doc => doc.Name == "UpdateOne").Result;
        
        Assert.That(result.WaterType, Is.EqualTo(WaterType.Saltwater));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestInsertOrUpdateOneAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "Update";
        aquarium.WaterType = WaterType.Freshwater;
        
        Aquarium aquarium2 = new Aquarium();
        aquarium2.Name = "Insert";
        aquarium2.WaterType = WaterType.Saltwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();

        aquarium.Name = "UpdateOrInsert";
        UnitOfWork.Aquarium.InsertOrUpdateOneAsync(aquarium).Wait();
        UnitOfWork.Aquarium.InsertOrUpdateOneAsync(aquarium2).Wait();
        
        Assert.That(UnitOfWork.Aquarium.FilterBy(doc => doc.Name == "Update").Count(), Is.EqualTo(0));
        Assert.That(UnitOfWork.Aquarium.FilterBy(doc => doc.Name == "UpdateOrInsert").Count(), Is.EqualTo(1));
        Assert.That(UnitOfWork.Aquarium.FilterBy(doc => doc.Name == "Insert").Count(), Is.EqualTo(1));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteOneAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "DeleteOne";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        UnitOfWork.Aquarium.DeleteOneAsync(doc => doc.Name == "DeleteOne");
        var result = UnitOfWork.Aquarium.FilterBy(doc => doc.Name == aquarium.Name);
        
        Assert.That(result.Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteByIdAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "DeleteOneByID";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        var getItem = UnitOfWork.Aquarium.FindOneAsync(doc => doc.Name == "DeleteOneByID").Result;
        var result = UnitOfWork.Aquarium.DeleteByIdAsync(getItem.ID);
        
        Assert.That(UnitOfWork.Aquarium.FilterBy(doc => doc.Name == aquarium.Name).Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
    
    [Test]
    public Task TestDeleteManyAsync()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Name = "DeleteMany";
        aquarium.WaterType = WaterType.Freshwater;

        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        UnitOfWork.Aquarium.InsertOneAsync(aquarium).Wait();
        
        Assert.That(UnitOfWork.Aquarium.FilterBy(doc => doc.Name == aquarium.Name).Count(), Is.EqualTo(3));

        UnitOfWork.Aquarium.DeleteManyAsync(doc => doc.Name == aquarium.Name);
        
        Assert.That(UnitOfWork.Aquarium.FilterBy(doc => doc.Name == aquarium.Name).Count(), Is.EqualTo(0));
        return Task.CompletedTask;
    }
}