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
using Firebase.Auth;
using Firebase.Database.Query;

namespace Groceries.Classes
{
    public static class RegisterClass
    {
        public static void Alert(ListsActivity thisActivity)
        {
            AlertDialog.Builder registerAlert = new AlertDialog.Builder(thisActivity);
            registerAlert.SetTitle("Register Online");
            registerAlert.SetMessage("Please enter your name, email and password");

            LinearLayout textEditsLayout = new LinearLayout(thisActivity);
            textEditsLayout.Orientation = Orientation.Vertical;

            EditText nameInput = new EditText(thisActivity);
            nameInput.TextSize = 22;
            nameInput.Gravity = GravityFlags.Center;
            nameInput.Hint = "Name";
            nameInput.SetSingleLine(true);
            textEditsLayout.AddView(nameInput);

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
            passwordInput.InputType = Android.Text.InputTypes.TextVariationPassword;
            passwordInput.Hint = "Password";
            passwordInput.SetSingleLine(true);
            textEditsLayout.AddView(passwordInput);

            registerAlert.SetView(textEditsLayout);
            registerAlert.SetPositiveButton("Register", async (senderAlert, args) => await Register(thisActivity, nameInput.Text, emailInput.Text, passwordInput.Text));
            registerAlert.SetNegativeButton("Cancel", (senderAlert, args) => { });

            Dialog dialog = registerAlert.Create();
            dialog.Show();
        }

        public static async Task Register(ListsActivity thisActivity, string name, string email, string password)
        {
            await AppData.auth.CreateUserWithEmailAndPasswordAsync(email, password);

            var profileUpdate = new UserProfileChangeRequest.Builder()
                                                            .SetDisplayName(name)
                                                            .Build();

            AppData.auth.CurrentUser.UpdateProfile(profileUpdate);

            UserClass localUser = new UserClass()
            {
                Name = name,
                Email = email,
                Uid = AppData.auth.CurrentUser.Uid
            };

            SetLocalUser.Set(localUser);

            await AppData.usersNode
                         .Child(AppData.auth.CurrentUser.Uid)
                         .PutAsync(localUser);


            foreach (GroceryListClass any in AppData.currentLists)
            {
                if (any.Owner.Uid == AppData.currentUser.Uid)
                {
                    SaveListOnCloud.Save(any);
                }
            }

            AlertShow.Show(thisActivity,
                           "Welcome " + AppData.currentUser.Name,
                           "You are now registered online and can share your lists with your friends");


           await thisActivity.ReloadListView();
        }

    }
}