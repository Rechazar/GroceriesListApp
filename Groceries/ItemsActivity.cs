using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;

namespace Groceries.Classes
{
    [Activity(Label = "ItemsActivity")]
    public class ItemsActivity : Activity
    {
        Button goBackButton;
        TextView listNameTextView;
        EditText newItemEditText;
        ListView itemsListView;
        Button shareThisButton;
        GroceryListClass curList;
        ItemRowListAdapter itemsAdapter;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ItemsLayout);

            InterfaceBuilder();

            AppData.GetInstance(this);
            int row = this.Intent.Extras.GetInt("row");
            curList = AppData.currentLists[row];
            listNameTextView.Text = curList.Name;

            itemsAdapter = new ItemRowListAdapter(this, curList.Items);
            itemsListView.Adapter = itemsAdapter;
        }




        void InterfaceBuilder()
        {
            goBackButton = FindViewById<Button>(Resource.Id.backButton_id);
            goBackButton.Click += delegate { 
                Finish(); 
            };

            listNameTextView = FindViewById<TextView>(Resource.Id.listNameTextView_id);

            newItemEditText = FindViewById<EditText>(Resource.Id.newItemEditText_id);
            newItemEditText.EditorAction += AddNewItem;

            itemsListView = FindViewById<ListView>(Resource.Id.itemsListView_id);
            itemsListView.ItemClick += ItemClicked;
            itemsListView.ItemLongClick += ItemLongClicked;

            shareThisButton = FindViewById<Button>(Resource.Id.shareThisButton_id);
            shareThisButton.Click += ShareThisFunction;
        }





        void ShareThisFunction(object sender, EventArgs e)
        {
            AlertDialog.Builder shareAlert = new AlertDialog.Builder(this);

            shareAlert.SetTitle("Inviting Someone?");
            shareAlert.SetMessage("Please enter the Email Address of the person you wish to invite to this list");

            EditText input = new EditText(this);
            input.TextSize = 22;
            input.Gravity = GravityFlags.Center;
            input.Hint = "Email Address";
            input.SetSingleLine(true);
            shareAlert.SetView(input);

            shareAlert.SetPositiveButton("Invite", async (shareSender, ShareE) => 
            {
                await InviteSomeone.Invite(this, curList, input.Text);
            });
            shareAlert.SetNegativeButton("Cancel", (senderAlert, args) => { });

            Dialog dialog = shareAlert.Create();
            dialog.Show();

        }





        void ItemLongClicked(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            e.View.Animate()
                .SetDuration(750)
                .Alpha(0)
                .WithEndAction(new Runnable(() =>
               {
                   ItemClass toRemoveItem = curList.Items[e.Position];
                   curList.Items.Remove(toRemoveItem);

                   e.View.Alpha = 1;
                   itemsAdapter.NotifyDataSetChanged();

                   //deleting locally
                   ReadWrite.WriteData();

                   //deleting from the cloud
                   DeleteItemFromCloud.Delete(toRemoveItem, curList);
               }));
        }





        void ItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            ItemClass thisItem = curList.Items[e.Position];

            bool status = false;
            if (thisItem.Purchased == "True")
            {
                status = true;
            }
            
            thisItem.Purchased = (!status).ToString();
            thisItem.Time = DateTime.UtcNow.ToString();

            itemsAdapter.NotifyDataSetChanged();

            //clicked for locally
            ReadWrite.WriteData();
            //for cloud
            SaveItemOnCloud.Save(thisItem, curList);
        }





        void AddNewItem(object sender, TextView.EditorActionEventArgs e)
        {
            if (e.ActionId != ImeAction.Done)
            {
                return;
            }

            ItemClass newItem = new ItemClass()
            {
                Name = newItemEditText.Text,
                Purchased = false.ToString(),
                Time = DateTime.UtcNow.ToString()
            };

            curList.Items.Add(newItem);
            //save item locally
            ReadWrite.WriteData();

            //save item online
            SaveItemOnCloud.Save(newItem, curList);


            itemsAdapter.NotifyDataSetChanged();

            newItemEditText.Text = "";
            newItemEditText.Hint = "+ New Item";

            //FindFocus basically gets the previous focus and uses that after inputing a new item and removing the keyboard
            this.CurrentFocus.FindFocus();
            InputMethodManager inputManager = (InputMethodManager) GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
        }
    }
}