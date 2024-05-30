using dal;
using dal.enums;
using dal.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using services.Response;
using services.ServiceImpl;

namespace tests.ServiceTests;

public class AquariumItemServiceTest : BaseServiceTest
{
    [Test]
    public async Task AddAquariumItem()
    {
        AquariumItem item = new AquariumItem();
        item.Aquarium = "Aquarium";
        item.Name = "AddAquariumItem";
        item.Species = "Item";
        item.Amount = 10;
        item.Description = "This is a description";

        AquariumItemService service = new AquariumItemService(UnitOfWork, UnitOfWork.AquariumItem, AquariumLogger);

        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);

        ItemResponseModel<AquariumItem> model = await service.AddAquariumItem(item);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);
    }

    [Test]
    public async Task AddCoral()
    {
        Coral coral = new Coral();
        coral.Aquarium = "Aquarium";
        coral.Name = "AddCoral";
        coral.Species = "Item";
        coral.Amount = 10;
        coral.Description = "This is a description";
        coral.CoralType = CoralType.SoftCoral;
        
        CoralService service = new CoralService(UnitOfWork, UnitOfWork.AquariumItem, AquariumLogger);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<AquariumItem> model = await service.AddCoral(coral);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);
    }

    [Test]
    public async Task GetCorals()
    {
        Coral coral = new Coral();
        coral.Aquarium = "Aquarium";
        coral.Name = "GetCorals";
        coral.Species = "Item";
        coral.Amount = 10;
        coral.Description = "This is a description";
        coral.CoralType = CoralType.SoftCoral;
        
        CoralService service = new CoralService(UnitOfWork, UnitOfWork.AquariumItem, AquariumLogger);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<AquariumItem> model = await service.AddCoral(coral);
        model = await service.AddCoral(coral);
        model = await service.AddCoral(coral);

        var result = service.GetCorals().Result;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThanOrEqualTo(3));
    }

    [Test]
    public async Task AddAnimal()
    {
        Animal animal = new Animal();
        animal.Aquarium = "Aquarium";
        animal.Name = "AddAnimal";
        animal.Species = "Item";
        animal.Amount = 10;
        animal.Description = "This is a description";
        animal.IsAlive = true;

        AnimalService service = new AnimalService(UnitOfWork, UnitOfWork.AquariumItem, AquariumLogger);

        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);

        ItemResponseModel<AquariumItem> model = await service.AddAnimal(animal);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);
    }
    
    [Test]
    public async Task GetAnimals()
    {
        Animal animal = new Animal();
        animal.Aquarium = "Aquarium";
        animal.Name = "GetAnimals";
        animal.Species = "Item";
        animal.Amount = 10;
        animal.Description = "This is a description";
        animal.IsAlive = true;
        
        AnimalService service = new AnimalService(UnitOfWork, UnitOfWork.AquariumItem, AquariumLogger);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<AquariumItem> model = await service.AddAnimal(animal);
        model = await service.AddAnimal(animal);
        model = await service.AddAnimal(animal);

        var result = service.GetAnimals().Result;
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThanOrEqualTo(3));
    }
}