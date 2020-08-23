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
    public static class DeleteListFromCloud
    {
        public static void Delete(GroceryListClass inpList)
        {
            if (AppData.auth.CurrentUser == null)
            {
                return;
            }
            else
            {
                AppData.dataNode.Child(inpList.Owner.Uid).Child(inpList.Name).DeleteAsync();
            }
        }
    }
}