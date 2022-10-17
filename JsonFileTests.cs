
using System.Text.Json;
using System.Text.Json.Serialization;


namespace jsonupdate;

public class JsonFileTests
{

    private const string Path = "./countries.json";

    [Explicit]
    [Test]
    public void Create()
    {
        var countries = new List<Country>
        {
            new(47, "Norway", 5300000),
            new(1, "USA", 350000000),
            new(46, "Sweden", 10500000)
        };
        var json = JsonSerializer.Serialize(countries);
        File.WriteAllText(Path, json);
    }


    [Test]
    public void ThatWeCanReadjsonFile()
    {
        var json = File.ReadAllText(Path);
        Assert.That(json, Has.Length.GreaterThan(0), "Could not read file");
        var countries = JsonSerializer.Deserialize<Country[]>(json);
        Assert.That(countries, Is.Not.Null,"Couldnt deserialize it");
        Assert.That(countries!.Count(), Is.GreaterThanOrEqualTo(3),"Didnt find the elements, serialization fault somehow");
    }


    /// <summary>
    /// This is the pattern for updating a non-record based file, e.f. a json or csv or whatever.
    /// With a bit of testing code added :-)
    /// </summary>
    [Test]
    public void ThatWeCanUpdateJsonFile()
    {
        var jsonIn = File.ReadAllText(Path);
        var countries = JsonSerializer.Deserialize<Country[]>(jsonIn);   // Note:  Here it is just an array, we could also make it into a list by adding ToList() after it. 
        
        // Let us change the population of USA to 332,403,650 (2022 numbers)
        var usa = countries!.Single(o => o.Code == 1);  // Note that usa in this case is just a Reference to the element in the array, so when we change it on the next line, we're actually changing it IN the array too
        usa.Population = 332403650;
        
        // Write it back
        var jsonOut = JsonSerializer.Serialize(countries);
        File.WriteAllText(Path,jsonOut);
        
        // Let us read it back in to ensure it actually has been changed in the file
        jsonIn = File.ReadAllText(Path);
        var countriesUpdated = JsonSerializer.Deserialize<Country[]>(jsonIn);

        Assert.That(countriesUpdated!.Length,Is.EqualTo(countries!.Length),"Some record was added during this operation, it should not.....");
        var usaUpdated = countriesUpdated.Single(o => o.Code == 1);
        Assert.That(usaUpdated.Population, Is.EqualTo(usa.Population));
    }


}