#r "..\bin\PressmonClient.dll"
using System;
using System.Text;
using LangugeBot;
using Microsoft.Bot.Builder.Azure;

[Serializable]
public class WordUsage
{
    private PressmonClient myPC;
    private string endpoint;
    private string key;
    public WordUsage()
	{
        endpoint = Utils.GetAppSetting("PressmonAPIEndpoint");
        key = Utils.GetAppSetting("PressmonAPIKey");

        myPC = new PressmonClient(endpoint, key);
	}

    public string getWordUsage(string word)
    {
        string usage = "";        WordUsageExample result = myPC.getWordUsage(word);
        if (result.hits !=null)
        {
            for (int i = 0; i < result.hits.Count;i++)
                usage += result.hits[i].body + "\n\n";
        }

        else
            usage = "No examples were found!";

        return usage;
    }
}
