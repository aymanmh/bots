#r "..\bin\WiktionaryClient.dll"
using System;
using System.Text;
using LanguageBot;
using Microsoft.Bot.Builder.Azure;

[Serializable]
public class WiktionaryEntry
{
    private WiktionaryClient myWikiClient;
    private string endpoint;
    public WiktionaryEntry()
	{
        endpoint = Utils.GetAppSetting("WiktionaryAPIEndpoint");
        //endpoint = "http://en.wiktionary.org/w/api.php?";

        myWikiClient = new WiktionaryClient(endpoint);        
    }

    public string getWikiEntry(string word)
    {
        string wikiEntry = myWikiClient.getWiktionaryEntryUrl(word);
        if (String.IsNullOrEmpty(wikiEntry))
        {
            wikiEntry = "No Wiktionary page were found!";
        }

        return wikiEntry;
    }
}
