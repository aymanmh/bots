#r "..\bin\WikitionaryClient.dll"
using System;
using System.Text;
using LangugeBot;
using Microsoft.Bot.Builder.Azure;

[Serializable]
public class WikitionaryEntry
{
    private WikitionaryClient myWikiClient;
    private string endpoint;
    public WikitionaryEntry()
	{
        endpoint = Utils.GetAppSetting("WikitionaryAPIEndpoint");

        myWikiClient = new WikitionaryClient(endpoint);        
    }

    public string getWikiEntry(string word)
    {
        string wikiEntry = myWikiClient.getWikitionaryEntryUrl(word);
        if (String.IsNullOrEmpty(wikiEntry))
        {
            wikiEntry = "No Wikitionary page were found!";
        }

        return wikiEntry;
    }
}
