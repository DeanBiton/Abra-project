using System.Text.Json.Nodes;

namespace RestAPI.DTO;

public class ErrorDTO : DTOBase
{
    public string message { get; set; }
    public ErrorDTO(string _message)
    {
        message = _message;
    }
}
