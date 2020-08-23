using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Groceries.Classes;
using AlertDialog = Android.App.AlertDialog;
using Android.Views;
using System.Collections.Generic;
using Java.Lang;
using Android.Content;
using Firebase.Database.Query;
using Android.Graphics;
using System.Threading.Tasks;

namespace Groceries
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class ListsActivity : AppCompatActivity
    {

        Button newListButton;
        Button profileButton;
        ListView groceryListView;
        ListRowCustomAdapter groceryAdapter;

        protected override void OnResume()
        {
            base.OnResume();

            if (groceryAdapter != null)
            {
                groceryAdapter.NotifyDataSetChanged();
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ListsLayout);
            InterfaceBuilder();

            AppData.GetInstance(this);

            await ReloadListView();

        }


        public async Task ReloadListView()
        {
            await ReadAllData.Read(this);

            groceryAdapter = new ListRowCustomAdapter(this, AppData.currentLists);
            groceryListView.Adapter = groceryAdapter;
        }



        void InterfaceBuilder()
        {
            newListButton = FindViewById<Button>(Resource.Id.newlistButton_id);
            newListButton.Click += NewListAlertMethod; 
            profileButton = FindViewById<Button>(Resource.Id.profileButton_id);
            profileButton.Click += ProfileAction;
            groceryListView = FindViewById(Resource.Id.listsListView) as ListView;
            groceryListView.ItemClick += GotoItemsAction;
            groceryListView.ItemLongClick += DeleteListAlert;

        }




        private void NewListAlertMethod(object sender, System.EventArgs e)
        {
            AlertDialog.Builder newListAlert = new AlertDialog.Builder(this);
            newListAlert.SetTitle("New List");
            newListAlert.SetMessage("Please enter the name of your new list");
            EditText input = new EditText(this)
            {
                TextSize = 22,
                Gravity = GravityFlags.Center,
                Hint = "List Name",
            };
            input.SetSingleLine(true);

            newListAlert.SetView(input);
            newListAlert.SetPositiveButton("OK", (senderAlert, arg) =>
            {
                NewListSave(input.Text);
            });

            newListAlert.SetNegativeButton("Cancel", (senderAlert, arg) => { });

            Dialog dialog = newListAlert.Create();
            dialog.Show();
        }






        void NewListSave(string inputListName)
        {
            GroceryListClass newList = new GroceryListClass
            {
                Name = inputListName,
                Items = new List<ItemClass>(),
                Owner = AppData.currentUser
            };

            AppData.currentLists.Add(newList);
            ReadWrite.WriteData(); //write offline

            SaveListOnCloud.Save(newList); //save online

            groceryAdapter.NotifyDataSetChanged();
        }







        private void ProfileAction(object sender, System.EventArgs e)
        {
            AlertDialog.Builder profileAlert = new AlertDialog.Builder(this);
            profileAlert.SetTitle("Profile");
            profileAlert.SetMessage("What would you like to do?");


            profileAlert.SetPositiveButton("Register", (senderAlert, arg) => RegisterClass.Alert(this));
            profileAlert.SetNeutralButton("Login", (sendingAlert, arg) => LoginClass.Alert(this));
            profileAlert.SetNegativeButton("Logout", async (sendingAlert, arg) => await LogoutClass.Logout(this));


            Dialog dialog = profileAlert.Create();
            dialog.Window.SetGravity(GravityFlags.Bottom);
            dialog.Show();
        }






        private void GotoItemsAction(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent itemsIntent = new Intent(this, typeof(ItemsActivity));

            itemsIntent.PutExtra("row", e.Position);

            StartActivity(itemsIntent);
        }





        private void DeleteListAlert(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            GroceryListClass toDeleteList = AppData.currentLists[e.Position];
                        
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            if (toDeleteList.Owner.Uid == AppData.currentUser.Uid)
            {
                alert.SetTitle("Confirm Delete");
                alert.SetMessage("Are you sure you want to delete the list " + toDeleteList.Name + "?");
            }
            else
            {
                alert.SetTitle("Remove Invitation");
                alert.SetMessage("Are you sure you want to remove this invitation from " + toDeleteList.Owner.Name + "?");
            }

            alert.SetPositiveButton("Delete", (senderAlert, eAlert) => DeleteList(toDeleteList, e));
            alert.SetNegativeButton("Cancel", (senderAlert, eAlert) => { });
            Dialog dialog = alert.Create();
            dialog.Show();

        }





        void DeleteList(GroceryListClass inputList, AdapterView.ItemLongClickEventArgs e)
        {
            e.View.Animate()
                .SetDuration(750)
                .Alpha(0)
                .WithEndAction(new Runnable(() =>
               {
                   //local list delete
                   AppData.currentLists.Remove(inputList);

                   if (inputList.Owner.Uid == AppData.currentUser.Uid)
                   {
                       //deleting list from cloud
                       DeleteListFromCloud.Delete(inputList);
                       ReadWrite.WriteData();
                   }
                   else // if its not the current users list
                   {
                       InvitationClass thisInvite = new InvitationClass()
                       {
                           Name = inputList.Name,
                           Owner = inputList.Owner
                       };
                       RemoveInvitation.Remove(thisInvite);
                   }

                   groceryAdapter.NotifyDataSetChanged();

                   e.View.Alpha = 1;
               }));
        }





        public void SetProfileButton(string statusStr, Color bgColor)
        {
            profileButton.Text = statusStr;
            profileButton.SetBackgroundColor(bgColor);
        }

    }
}