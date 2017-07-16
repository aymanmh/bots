#r "..\bin\TableStorageClient.dll"
using System;
using System.Text;
using LanguageBot;
using Microsoft.Bot.Builder.Azure;

public class StorageClient
{
    private TableStorageClient myTableStorageClient;
    BigWordEntity lastBigWord;
    public StorageClient()
	{
        string storageConnectionString = Utils.GetAppSetting("BigWordsTableConnectionString");
        string tableName = Utils.GetAppSetting("BigWordsTableName");
        string numberOfwords = Utils.GetAppSetting("BigWordsCount");

        //myTableStorageClient = new TableStorageClient("UseDevelopmentStorage=true;", "BigWords", "1218");
          
        myTableStorageClient = new TableStorageClient(storageConnectionString, tableName, numberOfwords);
    }

    public string getNewRandomWord()
    {
        lastBigWord = myTableStorageClient.getRandomWord();
        if (lastBigWord != null)
            return lastBigWord.RowKey;
        else
            return null;
    }

    public void markWordAsUased(BigWordEntity word)
    {
        myTableStorageClient.setWordUsedStatus(word);
    }

    public string getAnyRandomWord()
    {
        lastBigWord = myTableStorageClient.getRandomWord(false);
        if (lastBigWord != null)
            return lastBigWord.RowKey;
        else
            return null;
    }

}
