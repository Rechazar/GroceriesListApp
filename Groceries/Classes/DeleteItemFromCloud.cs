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
    public static class DeleteItemFromCloud
    {
        public static void Delete(ItemClass thisItem, GroceryListClass thisList)
        {
            if (AppData.auth.CurrentUser == null)
            {
                return;
            }

            AppData.dataNode.Child(thisList.Owner.Uid)
                            .Child(thisList.Name)
                            .Child("Items")
                            .Child(thisItem.Name)
                            .DeleteAsync();
        }
    }
}