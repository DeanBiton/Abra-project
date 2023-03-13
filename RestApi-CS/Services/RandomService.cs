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
            throw new Exception("Server error");

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


/*

{
    "results":[
        {"gender":"male",
        "name":{"title":"Mr","first":"Macário","last":"Campos"},
        "location":{
            "street":{"number":9051,"name":"Rua Sete de Setembro "},
            "city":"Brusque",
            "state":"Ceará",
            "country":"Brazil",
            "postcode":35261,
            "coordinates":{"latitude":"-81.3403","longitude":"-164.8025"},
            "timezone":{"offset":"+8:00","description":"Beijing, Perth, Singapore, Hong Kong"}},
        "email":"macario.campos@example.com",
        "login":{
            "uuid":"a1deb234-7bcf-4420-ab1a-acafb509853e",
            "username":"bigmouse700",
            "password":"poker",
            "salt":"ESU0sldl",
            "md5":"53a6dc5c12bc82ef5bd7e9dce298780e",
            "sha1":"6eece352f2f9a0b9210723630c96a6bcd0733af7",
            "sha256":"7b91edcf99e6fc52bf7be0c93186bf5a3c6a3a0a04b68e64bc195a90abe2791f"},
        "dob":{
            "date":"1988-05-16T09:50:32.783Z",
            "age":34},
        "registered":{
            "date":"2015-05-22T14:27:23.235Z",
            "age":7},
        "phone":"(73) 0653-2859",
        "cell":"(29) 3212-1127",
        "id":{
            "name":"CPF",
            "value":"404.711.488-23"},
        "picture":{
            "large":"https://randomuser.me/api/portraits/men/99.jpg",
            "medium":"https://randomuser.me/api/portraits/med/men/99.jpg",
            "thumbnail":"https://randomuser.me/api/portraits/thumb/men/99.jpg"},
            "nat":"BR"}]
    ,"info":{"seed":"c971a15f9ffe4d47","results":1,"page":1,"version":"1.4"}}

    */