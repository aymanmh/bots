#load "..\API\BingTranslator.csx"

using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

// For more information about this template visit http://aka.ms/azurebots-csharp-basic
[Serializable]
public class EchoDialog : IDialog<object>
{
    protected int count = 1;
    protected BingTranslator mybt0;

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
        else if (message.Text.Contains("?tra") )
        {
            if(mybt0==null)
                mybt0 = new BingTranslator();

            await context.PostAsync($"{this.count++}: "+mybt0.translateToArabic(message.Text.Substring(message.Text.IndexOf("?tr")+4)));
            context.Wait(MessageReceivedAsync);
        }
        else if (message.Text.Contains("?trm"))
        {
            if (mybt0 == null)
                mybt0 = new BingTranslator();

            await context.PostAsync($"{this.count++}: " + mybt0.translateToMalay(message.Text.Substring(message.Text.IndexOf("?trm") + 4)));
            context.Wait(MessageReceivedAsync);
        }
        else
        {
            await context.PostAsync($"{this.count++}: You said {message.Text}");
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