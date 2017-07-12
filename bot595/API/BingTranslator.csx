#r "..\bin\BingAPIClient.dll"
using System;
using LangugeBot;

[Serializable]
public class BingTranslator
{
    private BingAPIClient myBC;
    public BingTranslator()
	{
        myBC = new BingAPIClient("https://api.microsofttranslator.com/v2/Http.svc/Translate?", "e3208cac1c46424d9e81a94e900f15d3");
    }

    public string translateToArabic(string word)
    {
        return word + ": " + myBC.translate(word, BingAPIClient.ARABIC);
    }

    public string translateToMalay(string word)
    {
        return word+": " + myBC.translate(word, BingAPIClient.MALAY);
    }
}
