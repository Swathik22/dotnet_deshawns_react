using System.Linq.Expressions;
using DOTNET_DESHAWNS_REACT.Models;
using DOTNET_DESHAWNS_REACT.Models.DTOs;
using Microsoft.AspNetCore.HttpLogging;

List<Dog> dogs = new List<Dog>
{
    new Dog { Id = 1, Name = "Max", WalkerId = 1, CityId = 1 },
    new Dog { Id = 2, Name = "Bella", WalkerId = 2, CityId = 1 },
    new Dog { Id = 3, Name = "Charlie", WalkerId = 3, CityId = 2 },
    new Dog { Id = 4, Name = "Lucy", WalkerId = 4, CityId = 2 },
    new Dog { Id = 5, Name = "Cooper", WalkerId = 5, CityId = 3 },
    new Dog { Id = 6, Name = "Daisy", WalkerId = 1, CityId = 3 },
    new Dog { Id = 7, Name = "Bailey", WalkerId = 2, CityId = 4 },
    new Dog { Id = 8, Name = "Maggie", WalkerId = 3, CityId = 4 },
    new Dog { Id = 9, Name = "Maximus", WalkerId = 4, CityId = 5 },
    new Dog { Id = 10, Name = "Sadie", WalkerId = 5, CityId = 5 }
};

List<City> cities = new List<City>
{
    new City { Id = 1, Name = "New York" },
    new City { Id = 2, Name = "Los Angeles" },
    new City { Id = 3, Name = "Chicago" },
    new City { Id = 4, Name = "Houston" },
    new City { Id = 5, Name = "San Francisco" }
};

List<Walker> walkers = new List<Walker>
{
    new Walker { Id = 1, Name = "John" },
    new Walker { Id = 2, Name = "Emma" },
    new Walker { Id = 3, Name = "Michael" },
    new Walker { Id = 4, Name = "Sophia" },
    new Walker { Id = 5, Name = "Daniel" }
};


List<WalkerCity> walkerCities = new List<WalkerCity>
{
    new WalkerCity { Id = 1, WalkerId = 1, CityId = 1 },
    new WalkerCity { Id = 2, WalkerId = 2, CityId = 1 },
    new WalkerCity { Id = 3, WalkerId = 3, CityId = 2 },
    new WalkerCity { Id = 4, WalkerId = 4, CityId = 2 },
    new WalkerCity { Id = 5, WalkerId = 5, CityId = 3 },
    new WalkerCity { Id = 6, WalkerId = 1, CityId = 3 },
    new WalkerCity { Id = 7, WalkerId = 2, CityId = 4 },
    new WalkerCity { Id = 8, WalkerId = 3, CityId = 4 },
    new WalkerCity { Id = 9, WalkerId = 4, CityId = 5 },
    new WalkerCity { Id = 10, WalkerId = 5, CityId = 5 }
};


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/hello", () =>
{
    return new { Message = "Welcome to DeShawn's Dog Walking" };
});

app.MapGet("/api/getAllDogs",()=>
{
    List<DogDTO> dogList=dogs.Select(d=>new DogDTO{
        Id=d.Id,
        Name=d.Name,
        WalkerId=d.WalkerId,
        CityId=d.CityId
    }).ToList();

    return dogList;
});

//Get dog details by Id
app.MapGet("/api/getDogDetails/{id}",(int id)=>{
    Dog dog=dogs.FirstOrDefault(d=>d.Id==id);
    Walker walker= walkers.FirstOrDefault(w=>w.Id==dog.WalkerId);
    
    DogDTO DogDetails= new DogDTO
    {
        Id=dog.Id,
        Name=dog.Name,
        WalkerId=dog.WalkerId,
        Walker=new WalkerDTO
        {
            Id=walker.Id,
            Name=walker.Name,
            Email=walker.Email
        }
    };

    return DogDetails;
});

//Add new Dog
app.MapPost("/api/addNewDog",(Dog newDog)=>
{
    if(string.IsNullOrEmpty(newDog.Name))
    {
        return Results.BadRequest();
    }
    newDog.Id=dogs.Max(d=>d.Id)+1;
    dogs.Add(newDog);

    return Results.NoContent();

});

//Get all walkers
app.MapGet("/api/getAllWalkers",()=>{
    List<WalkerDTO> walkerDTOs=walkers.Select(w=>new WalkerDTO
    {
        Id=w.Id,
        Name=w.Name,
        Email=w.Email
    }).ToList();

    return walkerDTOs;
});

//Get all Cities
app.MapGet("/api/getAllCities",()=>{
    List<CityDTO> cityDTOs=cities.Select(c=>new CityDTO
    {
        Id=c.Id,
        Name=c.Name
    }).ToList();

    return cityDTOs;
});

//Get all Walkers by CityId
app.MapGet("/api/getWalkersByCityId/{id}",(int id)=>{
  
//     List<WalkerDTO> walkerDTOs=walkerCities.Where(wc=>wc.CityId==id)==null?null:walkerCities.Where(wc=>wc.CityId==id).Select(wc=>new WalkerDTO{
//         Id=wc.WalkerId,       
//         Name=walkers.FirstOrDefault(w=>w.Id==wc.WalkerId).Name,
//         Email=walkers.FirstOrDefault(w=>w.Id==wc.WalkerId).Email       
//         }).ToList();

//    return walkerDTOs;

List<WalkerDTO> walkerDTOs = null;

if (walkerCities != null && walkerCities.Any())
{  
    var filteredWalkerCities = walkerCities.Where(wc => wc.CityId == id);

    if (filteredWalkerCities != null && filteredWalkerCities.Any())
    {       
        walkerDTOs = filteredWalkerCities.Select(wc => new WalkerDTO
        {
            Id = wc.WalkerId,
            Name = walkers.FirstOrDefault(w => w.Id == wc.WalkerId)?.Name,
            Email = walkers.FirstOrDefault(w => w.Id == wc.WalkerId)?.Email
        }).ToList();
    }
}

return walkerDTOs;
  
});

//Get all dogs by Walker's city and walker not assigned
app.MapGet("/api/getAllDogsInWalkersCity/{walkerId}",(int walkerId)=>{
List<Dog> availableDogs = null;

if (walkerCities.Count>0)
{  
    List<Dog> filterDog = dogs.Where(wc => wc.WalkerId == walkerId).ToList();
 
    if (filterDog.Count>0)
    {    
        availableDogs = dogs
                    .Where(dog => filterDog.Any(d => d.CityId == dog.CityId && dog.WalkerId != walkerId)).ToList();
    }
}
    return availableDogs;
});

//assign walker to a dog
app.MapPut("/api/assignWalkerToADog/{walkerId}",(int walkerId,Dog dogObj)=>{
    if(string.IsNullOrEmpty(dogObj.Name))
    {
        return Results.BadRequest();
    }
    Dog dog=dogs.FirstOrDefault(d=>d.Id==dogObj.Id);
    dog.WalkerId=walkerId;

    return Results.NoContent();
});

//Add new City
app.MapPost("/api/city",(City cityObj)=>{
    cityObj.Id=cities.Max(c=>c.Id)+1;
    cities.Add(cityObj);

    return Results.Created("$/city",new CityDTO{
        Id=cityObj.Id,
        Name=cityObj.Name
    });
});

//get all the cities for the Walker
app.MapGet("/api/walkersCities/{walkerId}",(int walkerId)=>{
    //all cities of walker walking
    List<WalkerCity> walkersCityList=walkerCities.Where(wc=>wc.WalkerId==walkerId).ToList();
    List<CityDTO> cityList=null;
    if(walkersCityList.Count>0)
    {
        cityList=walkersCityList.Select(wcList=>new CityDTO
        {
            Id=cities.FirstOrDefault(c=>c.Id==wcList.CityId).Id,
            Name=cities.FirstOrDefault(c=>c.Id==wcList.CityId).Name,
        }).ToList();    
    }

    WalkerDTO walkerDetails=new WalkerDTO{
        Id=walkerId,
        Name=walkers.FirstOrDefault(w=>w.Id==walkerId).Name,
        CityList=cityList
    };
    
    return walkerDetails;
});

app.MapPut("/api/walkerCityUpdate/",(Walker walkerObj)=>{
    walkerCities =  walkerCities.Where(wc => wc.WalkerId != walkerObj.Id).ToList();

    foreach (City city in walkerObj.CityList)
    {
        WalkerCity newWC = new WalkerCity
        {
            WalkerId = walkerObj.Id,
            CityId = city.Id
        };
        newWC.Id = walkerCities.Count > 0 ? walkerCities.Max(wc => wc.Id) + 1 : 1;
        walkerCities.Add(newWC);    
    }
    return Results.NoContent();
});

app.MapDelete("/api/deleteDog/{dogId}",(int dogId)=>
{    
    Dog dog=dogs.FirstOrDefault(d=>d.Id==dogId);
    if(dog!=null)
    {
        dogs.Remove(dog);
    }  
    else
    {
        return Results.NotFound();
    }

    return Results.Ok();
});

app.MapDelete("/api/deleteWalker/{walkerId}",(int walkerID)=>{
    walkerCities=walkerCities.Where(wc=>wc.WalkerId!=walkerID).ToList();

    walkers=walkers.Where(w=>w.Id!=walkerID).ToList();

    dogs.Where(d=>d.WalkerId==walkerID).ToList().ForEach(dl => dl.WalkerId = 0);

    return Results.Ok();

});

app.Run();
