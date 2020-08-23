using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database.Query;

namespace Groceries.Classes
{
    public static class RemoveInvitation
    {
        public static void Remove(InvitationClass inpInvite)
        {
            if (AppData.auth.CurrentUser == null)
            {
                return;
            }

            string name = inpInvite.Name;
            string uid = inpInvite.Owner.Uid;

            string invitationTitle = uid + "|" + name;

            var invitationNode = AppData.usersNode
                                        .Child(AppData.currentUser.Uid).Child("Invitations").Child(invitationTitle);

            invitationNode.DeleteAsync();

        }
    }
}