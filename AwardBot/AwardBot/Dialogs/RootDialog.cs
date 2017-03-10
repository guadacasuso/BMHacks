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
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi");
            await context.PostAsync("How can I help you?");
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Wait(OptionReceivedAsync);
        }

        public virtual async Task OptionReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            this.ShowMainMenu(context);
        }

        private void ShowMainMenu(IDialogContext context)
        {
            List<string> awardList = new List<string>() { "President Award", "CPE and ShareFighters","Years of Service Awards", "Technical Achievements", "Best Stories", "Peer to Peer Recognitions" };
            PromptDialog.Choice(context, this.afterMenuSelection,awardList, Resources.Messeges.WelcomeMessages);
        }

        private  async Task afterMenuSelection(IDialogContext context, IAwaitable<object> result)
        {
            var selectedMessage = await result;
            if (selectedMessage.ToString() == "Peer to Peer Recognitions")
            {
                context.Call(new NominateDialog(), ResumeAfterMeetingDialog);      
            }
        }

        private async Task ResumeAfterMeetingDialog(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            this.ShowMainMenu(context);
        }
    }
}