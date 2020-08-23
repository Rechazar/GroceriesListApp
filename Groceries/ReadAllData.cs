using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Groceries.Classes;

namespace Groceries
{
    public static class ReadAllData
    {
        public static async Task Read(ListsActivity thisActivity)
        {
            ReadWrite.ReadUser();

            //for offline user
            if (AppData.currentUser == null)
            {
                AppData.currentUser = new UserClass()
                {
                    Name = "Me",
                    Email = "defEmail",
                    Uid = "defUid"
                };

                PrepareFirstList.Prepare();

                ReadWrite.WriteData();
                ReadWrite.WriteUser();
            }
            else
            {
                ReadWrite.ReadData();
                AppData.currentLists = AppData.offlineLists;
            }

            thisActivity.SetProfileButton("Login!", Color.Orange);



            //for online user
            if (AppData.auth.CurrentUser != null)
            {
                thisActivity.SetProfileButton("Online!", Color.Green);

                await ReadDataFromCloud.Read();

                AppData.currentLists = CompareLists.Compare(AppData.onlineLists, AppData.offlineLists);

                ReadWrite.WriteData();
                foreach (GroceryListClass any in AppData.currentLists)
                {
                    SaveListOnCloud.Save(any);
                }

                await ReadInvitations.Read();

                await FetchInvitations.Fetch();
                foreach (GroceryListClass any in AppData.invitationLists)
                {
                    AppData.currentLists.Add(any);
                }
            }
        }
    }
}