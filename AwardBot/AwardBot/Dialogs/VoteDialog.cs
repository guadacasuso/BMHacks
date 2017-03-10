using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace AwardBot.Dialogs
{
    [Serializable]
    public class VoteDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync(Resources.Messeges.VoteMessage);
            this.ShowMainMenu(context);
        }

        private void ShowMainMenu(IDialogContext context)
        {
            List<string> awardList = new List<string>() { "Creative | Kosuke","Geek | Shinobu" };
            PromptDialog.Choice(context, this.afterMenuSelection, awardList, Resources.Messeges.SelectVotingMessage);
        }

        private async Task afterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Thank you!");
            context.Done<object>(null);
        }
    }
}