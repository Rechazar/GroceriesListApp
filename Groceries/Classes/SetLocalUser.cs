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

namespace Groceries.Classes
{
   public static class SetLocalUser
    {
        public static void Set(UserClass inpUser)
        {
            foreach (GroceryListClass any in AppData.currentLists)
            {
                if (any.Owner.Uid == AppData.currentUser.Uid)
                {
                    any.Owner = inpUser;
                }
            }

            AppData.currentUser = inpUser;

            ReadWrite.WriteData();
            ReadWrite.WriteUser();
        }
    }
}