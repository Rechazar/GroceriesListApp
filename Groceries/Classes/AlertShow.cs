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
    public static class AlertShow
    {
        public static void Show(Activity thisActivity, string title, string message)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(thisActivity);
            alert.SetTitle(title);
            alert.SetMessage(message);

            alert.SetPositiveButton("OK", (sender, e) => { });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}