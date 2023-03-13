using System.Text.Json.Nodes;

namespace RestAPI.DTO;

public class OldestDTO : DTOBase
{
    public string? name{ get; set; }
    public int? age{ get; set; }

    public OldestDTO(JsonNode? person)
    {
        name =  String.Format("{0} {1} {2}", person!["name"]!["title"]!, 
            person!["name"]!["first"]!, person!["name"]!["last"]!);
        age = (int)person!["dob"]!["age"]!;
    }
}
