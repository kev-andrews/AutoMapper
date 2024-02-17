namespace AutoMapper.UnitTests.Projection;

public class InMemoryMapPolymorphic : AutoMapperSpecBase
{
    protected override MapperConfiguration CreateConfiguration() => new(cfg =>
    {
        cfg.CreateMap<Vehicle, VehicleModel>()
            .IncludeAllDerived();
	
        cfg.CreateMap<Motorcycle, MotorcycleModel>();
    });

    [Fact]
    public void Should_project_base_queryable_to_derived_models_polymorphic()
    {
        var vehicles = new Vehicle[] { new Car { Name = "Car", AmountDoors = 4 }, new Motorcycle { Name = "Motorcycle", HasSidecar = true } };
        var result = vehicles.AsQueryable().ProjectTo<VehicleModel>(Configuration).ToArray();
        result[0].ShouldBeOfType<VehicleModel>();
        result[1].ShouldBeOfType<MotorcycleModel>();
    }
    
    [Fact]
    public void Should_project_derived_queryable_to_derived_models_if_derived_models_exist()
    {
        var vehicles = new Motorcycle[] { new Motorcycle { Name = "Motorcycle", HasSidecar = true } };
        var result = vehicles.AsQueryable().ProjectTo<MotorcycleModel>(Configuration).ToArray();
        result[0].ShouldBeOfType<MotorcycleModel>();
    }
    
    [Fact]
    public void Should_project_derived_queryable_to_base_models_if_no_derived_models_exist()
    {
        var vehicles = new Car[] { new Car { Name = "Car", AmountDoors = 4 } };
        var result = vehicles.AsQueryable().ProjectTo<VehicleModel>(Configuration).ToArray();
        result[0].ShouldBeOfType<VehicleModel>();
    }

    public abstract partial class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class Car : Vehicle
    {
        public int AmountDoors { get; set; }
    }

    public partial class Motorcycle : Vehicle
    {
        public bool HasSidecar { get; set; }
    }

    public partial class VehicleModel
    {
        public string Name { get; set; }
    }

    public partial class MotorcycleModel : VehicleModel
    {
        public bool HasSidecar { get; set; }
    }
}