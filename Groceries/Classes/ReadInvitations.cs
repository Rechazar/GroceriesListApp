using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;

namespace Groceries.Classes
{
    public static class ReadInvitations
    {
        public static async Task Read()
        {
            AppData.invitationData = new List<InvitationClass>();

            if (AppData.auth.CurrentUser == null)
            {
                return;
            }

            var allCordinatesData = await AppData.usersNode
                                                 .Child(AppData.currentUser.Uid)
                                                 .Child("Invitations")
                                                 .OnceAsync<InvitationClass>();

            foreach (FirebaseObject<InvitationClass> anyInvite in allCordinatesData)
            {
                AppData.invitationData.Add(anyInvite.Object);
            }
        }
    }
}