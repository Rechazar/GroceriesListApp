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

namespace Groceries.Classes
{
    public static class LogoutClass
    {
        public static async Task Logout(ListsActivity thisActivity)
        {
            if (AppData.auth.CurrentUser == null)
            {
                return;
            }

            AppData.auth.SignOut();

            AlertShow.Show(thisActivity, "Logged out", "You are now in offline mode");


            //reload everything
            await thisActivity.ReloadListView();

        }
    }
}