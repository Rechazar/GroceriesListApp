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
    public static class InviteSomeone
    {
        public static async Task Invite(Activity thisActivity, GroceryListClass toList, string inputEmailAddress)
        {
            UserClass inviteeUser = null;
            UserClass ownerUser = toList.Owner;

            var allUserData = await AppData.usersNode.OnceAsync<UserClass>();

            foreach (FirebaseObject<UserClass> any in allUserData)
            {
                if (inputEmailAddress == any.Object.Email)
                {
                    inviteeUser = any.Object;
                    goto UserExists;
                }
            }

            AlertShow.Show(thisActivity, "No Such User", "The email address you have provided, does not have an account");

            return;

        UserExists:

            InvitationClass thisInvite = new InvitationClass()
            {
                Name = toList.Name,
                Owner = ownerUser
            };

            //unique userID and the name of the list
            string invitationTitle = ownerUser.Uid + "|" + toList.Name;

            await AppData.usersNode.Child(inviteeUser.Uid)
                                   .Child("Invitations")
                                   .Child(invitationTitle)
                                   .PutAsync(thisInvite);

            AlertShow.Show(thisActivity, "Success", "You have successfully invited " + inviteeUser.Name + " to this List");
        }
    }
}