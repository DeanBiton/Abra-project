using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using RestAPI.DTO;
using RestAPI.Models;

namespace RestAPI.Services;

public class RandomService
{
    private readonly HttpClient client;
    private string baseUrl = "http://randomuser.me/api";
    private const string GetUsersNumber = "10";
    private const string GetPopularCountryNumber = "5000";
    private const string GetMailNumber = "30";
    private const string GetOldestNumber = "100";

    public RandomService()
    {
        client = new HttpClient();
    }

    async private Task<JsonNode> useRandomUserApi(string url)
    {
        JsonNode? jsonNode = null;
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var jsonObject = await response.Content.ReadFromJsonAsync<JsonObject>();
        jsonObject?.TryGetPropertyValue("results", out jsonNode);
        if(jsonNode == null)
            throw new Exception(String.Format("Failed to get results from url: {0}", url));

        return jsonNode;
    }

    async public Task<JsonNode> GetUsersData(string gender)
    {
        JsonNode jsonNode = await useRandomUserApi(String.Format("{0}/?results={1}&gender={2}",baseUrl, GetUsersNumber, gender));
        return jsonNode;
    }

    async public Task<List<string>> GetListOfMails()
    {
        JsonNode jsonNode = await useRandomUserApi(String.Format("{0}/?results={1}",baseUrl, GetMailNumber));
        return GetListOfMailsFromResponse(jsonNode);
    }

    private List<string> GetListOfMailsFromResponse(JsonNode node)
    {
        List<string> mails = new List<string>();
        foreach(var person in node.AsArray())
        {
            mails.Add((string)person!["email"]!);
        }

        return mails;
    }

    async public Task<string> GetMostPopularCountry()
    {
        JsonNode jsonNode = await useRandomUserApi(String.Format("{0}/?results={1}",baseUrl, GetPopularCountryNumber));
        return GetMostPopularCountryFromResponse(jsonNode);
    }

    private string GetMostPopularCountryFromResponse(JsonNode? node)
    {
        var countryDict = new Dictionary<string, int>();
        foreach(var person in node.AsArray()!)
        {
            string country = (string)person!["location"]!["country"]!;
            if(countryDict.ContainsKey(country))
                countryDict[country]++;
            else
                countryDict[country] = 0;
        }
        
        string popularCountry = countryDict.MaxBy(kvp => kvp.Value).Key;
        return popularCountry;
    }

    async public Task<DTOBase> GetTheOldestUser()
    {
        JsonNode jsonNode = await useRandomUserApi(String.Format("{0}/?results={1}",baseUrl, GetOldestNumber));
        var oldestUser = getOldestUserFromResponse(jsonNode);
        OldestDTO user = new OldestDTO(oldestUser);
        return user;
    }

    private JsonNode getOldestUserFromResponse(JsonNode node)
    {
        var maxPerson = node[0]!;
        foreach(var person in node.AsArray())
        {
            if((int)(person!["dob"]!["age"]!) > (int)(maxPerson!["dob"]!["age"]!))
                maxPerson = person;
        }
        
        return maxPerson;
    }

}