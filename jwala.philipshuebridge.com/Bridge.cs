namespace jwala.philipshuebridge.com;

public class Bridge
{
    public string Id { get; }
    public string IpAddress { get; }
    public string Name { get; }

    public Bridge(string id, string ipAddress, string name)
    {
        Id = id;
        IpAddress = ipAddress;
        Name = name;
    }
}