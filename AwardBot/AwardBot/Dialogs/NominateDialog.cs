using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AwardBot.Dialogs
{
    [Serializable]
    public class NominateDialog : IDialog<object>
    {
        private string awardName;
        private string nomineeName;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("This is to recognize good job of your Peers");
            this.ShowMainMenu(context);
        }

        private void ShowMainMenu(IDialogContext context)
        {
            List<string> awardList = new List<string>() { "Yes","No" };
            PromptDialog.Choice(context, this.afterMenuSelection, awardList, Resources.Messeges.NominateMessage);
        }

        private async Task afterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var selectedMessage = await result;
            if(selectedMessage.ToString() == "Yes")
            {
                await context.PostAsync("Give me Award name.");
                context.Wait(AskAwardNameAsync);
            }
            else
            {
                context.Call(new VoteDialog(), ResumeAfterMeetingDialog);
            }
        }

        public virtual async Task AskAwardNameAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var name = await result;
            awardName = name.Text;
            await context.PostAsync("Who?");
            context.Wait(AskNomineedNameAsync);
        }

        public virtual async Task AskNomineedNameAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var nominee = await result;
            nomineeName = nominee.Text;
            context.Call(new VoteDialog(), ResumeAfterMeetingDialog);
        }

        private async Task ResumeAfterMeetingDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
    }
}