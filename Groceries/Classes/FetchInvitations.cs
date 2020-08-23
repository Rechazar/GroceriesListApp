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
    public static class FetchInvitations
    {
        public static async Task Fetch()
        {
            if (AppData.auth.CurrentUser == null)
            {
                return;
            }

            AppData.invitationLists = new List<GroceryListClass>();

            foreach (InvitationClass anyInvite in AppData.invitationData)
            {
                var allItems = await AppData.dataNode.Child(anyInvite.Owner.Uid).Child(anyInvite.Name).Child("Items").OnceAsync<ItemClass>();

                List<ItemClass> itemsOfList = new List<ItemClass>();

                foreach (FirebaseObject<ItemClass> any in allItems)
                {
                    itemsOfList.Add(any.Object);
                }

                GroceryListClass thisList = new GroceryListClass
                {
                    Name = anyInvite.Name,
                    Items = itemsOfList,
                    Owner = anyInvite.Owner
                };

                AppData.invitationLists.Add(thisList);
            }
        }
    }
}