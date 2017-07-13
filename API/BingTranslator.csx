#r "..\bin\BingAPIClient.dll"
using System;
using System.Threading;
using LanguageBot;
using Microsoft.Bot.Builder.Azure;

[Serializable]
public class BingTranslator
{
    private BingAPIClient myBC;
    private string endpoint;
    private string key;
    public BingTranslator()
	{
        endpoint = Utils.GetAppSetting("BingAPIEndpoint");
        key = Utils.GetAppSetting("BingAPIKey");
        myBC = new BingAPIClient(endpoint, key);
    }

    public string translate(string word,string code)
    {
        string translation = "";
        switch(code)
        {
            case "/ar":
                 translation = ": " + myBC.translate(word, BingAPIClient.ARABIC);
                break;
            case "/my":
                 translation = ": " + myBC.translate(word, BingAPIClient.MALAY);
                break;
            case "/jp":
                 translation = ": " + myBC.translate(word, BingAPIClient.JAPANESE);
                break;
            default:
                return "language not implemented";
        }

        return word + translation;
    }

}
