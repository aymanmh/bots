#load "..\API\BingTranslator.csx"
#load "..\API\WordUsage.csx"
#load "..\API\WiktionaryEntry.csx"
#load "..\API\DictionaryClient.csx"
#load "..\API\StorageClient.csx"

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

// For more information about this template visit http://aka.ms/azurebots-csharp-basic
[Serializable]
public class EchoDialog : IDialog<object>
{
    protected int count = 1;
    protected BingTranslator myBingTranslator;
    protected WordUsage myWordUsage;
    protected WiktionaryEntry myWikEntryClient;
    protected DictionaryClient myDictionaryClient;
    [NonSerialized] //this library uses other asemblies that are not serilizable, so it will create issues if serialized.
    protected StorageClient myStorageClient;

    protected static string[] genericReplyList = new string[] { "huh?!", "what?", "hmmm...", ":sweat_smile:",":grin:" };

    public Task StartAsync(IDialogContext context)
    {
        try
        {
            context.Wait(MessageReceivedAsync);
        }
        catch (OperationCanceledException error)
        {
            return Task.FromCanceled(error.CancellationToken);
        }
        catch (Exception error)
        {
            return Task.FromException(error);
        }

        return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument;
        if (message.Text == "reset")
        {
            PromptDialog.Confirm(
                context,
                AfterResetAsync,
                "Are you sure you want to reset the count?",
                "Didn't get that!",
                promptStyle: PromptStyle.Auto);
        }
        else if (message.Text.IndexOf("/") == 0) //It's a command
        {
            //translate
            if (message.Text.Contains("/my ") ||
                message.Text.Contains("/ar ") ||
                message.Text.Contains("/jp ")) 
            {
                if (myBingTranslator == null)
                    myBingTranslator = new BingTranslator();
                string word = message.Text.Substring(message.Text.IndexOf(" ") + 1);
                await context.PostAsync(word + ": " +myBingTranslator.translate(word, message.Text.Substring(0,3)));
            }
            else if(message.Text.Contains("/usage ")) //get usage examples using the Pressmon API
            {
                string[] splitMsg= message.Text.Split();
                if (splitMsg.Length != 2)
                    await context.PostAsync($"Please follow the {splitMsg[0]} command with ONE word only!");
                else
                {
                    if (myWordUsage == null)
                        myWordUsage = new WordUsage();
                    await context.PostAsync(myWordUsage.getWordUsage(splitMsg[1]));
                }
            }
            else if (message.Text.Contains("/wiki ")) //retrun wiki entry page
            {
                string[] splitMsg = message.Text.Split();
                if (splitMsg.Length != 2)
                    await context.PostAsync($"Please follow the {splitMsg[0]} command with ONE word only!");
                else
                {
                    if (myWikEntryClient == null)
                        myWikEntryClient = new WiktionaryEntry();
                    await context.PostAsync(myWikEntryClient.getWikiEntry(splitMsg[1]));
                }
            }
            else if (message.Text.Contains("/def ")) //get the dictionary definition
            {
                string[] splitMsg = message.Text.Split();
                if (splitMsg.Length != 2)
                    await context.PostAsync($"Please follow the {splitMsg[0]} command with ONE word only!");
                else
                {
                    if (myDictionaryClient == null)
                        myDictionaryClient = new DictionaryClient();
                    await context.PostAsync(myDictionaryClient.getDefinition(splitMsg[1]));
                }
            }
            else if (message.Text.Contains("/bigword")) // get a random new word
            {
                //get the word from our db
                if (myStorageClient == null)
                    myStorageClient = new StorageClient();
                string mybigword = myStorageClient.getAnyRandomWord();
                if (mybigword != null)
                {
                    StringBuilder sbResult = new StringBuilder();
                    //get the dictionary definition
                    if (myDictionaryClient == null)
                        myDictionaryClient = new DictionaryClient();

                    sbResult.Append( myDictionaryClient.getDefinition(mybigword));
                    sbResult.Append("\n\n");
                    //then translated with bing
                    if (myBingTranslator == null)
                        myBingTranslator = new BingTranslator();
                    sbResult.Append("Arabic: " + myBingTranslator.translate(mybigword, "/ar"));
                    sbResult.Append(" - Malay: " + myBingTranslator.translate(mybigword, "/my"));
                    sbResult.Append(" - Japanese: " + myBingTranslator.translate(mybigword, "/jp"));
                    sbResult.Append("\n\n");
                    await context.PostAsync(sbResult.ToString());
                }
                else
                {
                    await context.PostAsync("Failed to get a new word!");

                }
            }
            else
            {
                await context.PostAsync("Uknown command!");
            }
            context.Wait(MessageReceivedAsync);
        }
        else //otherwise repl y with word from the generic list above
        {
            Random rnd = new Random();
            int randomI = rnd.Next(0, genericReplyList.Length-1);

            await context.PostAsync(genericReplyList[randomI]);
            context.Wait(MessageReceivedAsync);
        }
    }

    //testing function
    public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
    {
        var confirm = await argument;
        if (confirm)
        {
            this.count = 1;
            await context.PostAsync("Reset count.");
        }
        else
        {
            await context.PostAsync("Did not reset count.");
        }
        context.Wait(MessageReceivedAsync);
    }




}