using dal;
using dal.enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using services.Authentication;
using services.Response;
using services.ServiceImpl;

namespace tests.ServiceTests;

public class AquariumServiceTest : BaseServiceTest
{
    [Test]
    public async Task CreateAquarium()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Liters = 572;
        aquarium.Length = 55;
        aquarium.Height = 65;
        aquarium.Depth = 150;
        aquarium.Name = "SchiSchoSaltWater";
        aquarium.WaterType = WaterType.Saltwater;

        AquariumService service = new AquariumService(UnitOfWork, UnitOfWork.Aquarium, AquariumLogger);
        
        // Modelstate faken durch library (mock)
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<Aquarium> model = await service.Create(aquarium);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);
    }

    [Test]
    public async Task UpdateAquarium()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Liters = 572;
        aquarium.Length = 55;
        aquarium.Height = 65;
        aquarium.Depth = 150;
        aquarium.Name = "UpdateAquarium";
        
        AquariumService service = new AquariumService(UnitOfWork, UnitOfWork.Aquarium, AquariumLogger);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<Aquarium> model = await service.Create(aquarium);
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);

        aquarium.Name = "Updated";
        await service.Update(model.Data.ID, model.Data);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);
        Assert.That(model.Data.Name, Is.EqualTo("Updated"));
    }

    [Test]
    public async Task DeleteAquarium()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Liters = 572;
        aquarium.Length = 55;
        aquarium.Height = 65;
        aquarium.Depth = 150;
        aquarium.Name = "DeleteAquarium";
        aquarium.WaterType = WaterType.Saltwater;
        
        AquariumService service = new AquariumService(UnitOfWork, UnitOfWork.Aquarium, AquariumLogger);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<Aquarium> model = await service.Create(aquarium);
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);

        await service.Delete(model.Data.ID);
        var result = UnitOfWork.Aquarium.FilterBy(doc => doc.Name == aquarium.Name);
        
        Assert.That(result.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task GetOneAquarium()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Liters = 572;
        aquarium.Length = 55;
        aquarium.Height = 65;
        aquarium.Depth = 150;
        aquarium.Name = "GetOneAquarium";
        aquarium.WaterType = WaterType.Saltwater;
        
        AquariumService service = new AquariumService(UnitOfWork, UnitOfWork.Aquarium, AquariumLogger);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<Aquarium> model = await service.Create(aquarium);
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);

        var result = await service.Get(model.Data.ID);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(aquarium.Name));
    }

    [Test]
    public async Task GetAllAquarium()
    {
        Aquarium aquarium = new Aquarium();
        aquarium.Liters = 572;
        aquarium.Length = 55;
        aquarium.Height = 65;
        aquarium.Depth = 150;
        aquarium.Name = "GetAllAquarium";
        aquarium.WaterType = WaterType.Saltwater;
        
        AquariumService service = new AquariumService(UnitOfWork, UnitOfWork.Aquarium, AquariumLogger);
        
        var modelState = new Mock<ModelStateDictionary>();
        await service.SetModelState(modelState.Object);
        
        ItemResponseModel<Aquarium> model = await service.Create(aquarium);
        model = await service.Create(aquarium);
        model = await service.Create(aquarium);
        
        Assert.That(model, Is.Not.Null);
        Assert.That(model.Data, Is.Not.Null);
        Assert.That(model.HasError, Is.False);

        var result = await service.Get();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.GreaterThanOrEqualTo(3));
    }
    
    [Test]
    public async Task GetForUser()
    {
        // Create User
        User user = new User("Pa$$word");
        user.Email = "user@getforuser.com";
        user.Firstname = "First";
        user.Lastname = "Last";
        user.Active = true;

        UserService userService = new UserService(UnitOfWork, UnitOfWork.User, AquariumLogger, new Authenticator(UnitOfWork));
        var userModelState = new Mock<ModelStateDictionary>();
        await userService.SetModelState(userModelState.Object);
        
        ItemResponseModel<User> userModel = await userService.Create(user);
        
        // Check if user creation was successfull
        Assert.That(userModel, Is.Not.Null);
        Assert.That(userModel.Data, Is.Not.Null);
        Assert.That(userModel.HasError, Is.False);
        
        // Create Aquarium
        Aquarium aquarium = new Aquarium();
        aquarium.Liters = 572;
        aquarium.Length = 55;
        aquarium.Height = 65;
        aquarium.Depth = 150;
        aquarium.Name = "getforuser";
        aquarium.WaterType = WaterType.Saltwater;
        
        AquariumService aquariumService = new AquariumService(UnitOfWork, UnitOfWork.Aquarium, AquariumLogger);
        var aquariumModelState = new Mock<ModelStateDictionary>();
        await aquariumService.SetModelState(aquariumModelState.Object);
        
        ItemResponseModel<Aquarium> aquariumModel = await aquariumService.Create(aquarium);
        
        // Check if aquarium creation was successfull
        Assert.That(aquariumModel, Is.Not.Null);
        Assert.That(aquariumModel.Data, Is.Not.Null);
        Assert.That(aquariumModel.HasError, Is.False);
        
        // Get IDs of user and aquarium
        var userID = userModel.Data.ID;
        var aquariumID = aquariumModel.Data.ID;
        
        // Add UserAquarium
        UserAquarium userA = new UserAquarium();
        userA.UserID = userID;
        userA.AquariumID = aquariumID;

        UnitOfWork.UserAquarium.InsertOneAsync(userA).Wait();
        
        // Get all aquaria for one user
        var result = await aquariumService.GetForUser(user);
        Console.WriteLine(result);
        Console.WriteLine(result.Count());
        
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo(aquarium.Name));
    }
}