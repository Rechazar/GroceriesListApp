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
    public static class LoginClass
    {
        public static void Alert(ListsActivity thisActivity)
        {
            AlertDialog.Builder loginAlert = new AlertDialog.Builder(thisActivity);
            loginAlert.SetTitle("Login Online");
            loginAlert.SetMessage("Please enter your email and password");

            LinearLayout textEditsLayout = new LinearLayout(thisActivity);
            textEditsLayout.Orientation = Orientation.Vertical;

            EditText emailInput = new EditText(thisActivity);
            emailInput.TextSize = 22;
            emailInput.Gravity = GravityFlags.Center;
            emailInput.Hint = "Email";
            emailInput.InputType = Android.Text.InputTypes.TextVariationEmailAddress;
            emailInput.SetSingleLine(true);
            textEditsLayout.AddView(emailInput);

            EditText passwordInput = new EditText(thisActivity);
            passwordInput.TextSize = 22;
            passwordInput.Gravity = GravityFlags.Center;
            passwordInput.InputType = Android.Text.InputTypes.NumberVariationPassword;
            passwordInput.Hint = "Password";
            passwordInput.InputType = Android.Text.InputTypes.TextVariationPassword;
            emailInput.SetSingleLine(true);
            textEditsLayout.AddView(passwordInput);

            loginAlert.SetView(textEditsLayout);
            loginAlert.SetPositiveButton("Login", async (sender, e) => await Login(thisActivity,
                                                                                   emailInput.Text,
                                                                                   passwordInput.Text));
            loginAlert.SetNegativeButton("Cancel", (senderAlert, args) => { });
            Dialog dialog = loginAlert.Create();
            dialog.Show();
        }

        public static async Task Login(ListsActivity thisActivity, string inpEmail, string inpPassword)
        {
            await AppData.auth.SignInWithEmailAndPasswordAsync(inpEmail, inpPassword);

            UserClass newLocalUser = new UserClass()
            {
                Name = AppData.auth.CurrentUser.DisplayName,
                Email = AppData.auth.CurrentUser.Email,
                Uid = AppData.auth.CurrentUser.Uid
            };

            SetLocalUser.Set(newLocalUser);

            AlertShow.Show(thisActivity, "Welcome " + newLocalUser.Name,
                           "You can now share your lists online");

            await thisActivity.ReloadListView();
        }
    }
}