#load "..\API\BingTranslator.csx"
#load "..\API\WordUsage.csx"
#load "..\API\WikitionaryEntry.csx"

using System;
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
    protected WikitionaryEntry myWikiEntryClient;

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
           
            if (message.Text.Contains("/my ") ||
                message.Text.Contains("/ar ") ||
                message.Text.Contains("/jp ")) //translate
            {
                if (myBingTranslator == null)
                    myBingTranslator = new BingTranslator();
                await context.PostAsync(myBingTranslator.translate(message.Text.Substring(message.Text.IndexOf(" ") + 1), message.Text.Substring(0,3)));
            }
            else if(message.Text.Contains("/usage "))
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
            else if (message.Text.Contains("/wiki "))
            {
                string[] splitMsg = message.Text.Split();
                if (splitMsg.Length != 2)
                    await context.PostAsync($"Please follow the {splitMsg[0]} command with ONE word only!");
                else
                {
                    if (myWikiEntryClient == null)
                        myWikiEntryClient = new WikitionaryEntry();
                    await context.PostAsync(myWikiEntryClient.getWikiEntry(splitMsg[1]));
                }
            }
            else
            {
                await context.PostAsync("Uknown command!");
            }
            context.Wait(MessageReceivedAsync);
        }
        else
        {
            Random rnd = new Random();
            int randomI = rnd.Next(0, genericReplyList.Length-1);

            await context.PostAsync(genericReplyList[randomI]);
            context.Wait(MessageReceivedAsync);
        }
    }

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