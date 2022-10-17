namespace jsonupdate;

public class Country
{
    public int Code { get; set; }
    public string Name { get; set; } = "";

    public int Population { get; set; }

    public DateTime Updated { get; set; }

    public Country()
    {
        
    }

    public Country(int code, string name, int population)
    {
        Code = code;
        Name = name;
        Population = population;
        Updated = DateTime.Now;
    }


}