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
   public class PrepareFirstList
    {
        public static void Prepare()
        {
            AppData.currentLists = new List<GroceryListClass>();


            AppData.currentLists.Add(new GroceryListClass
            {
                Name = "Sample List",
                Items = new List<ItemClass>(),
                Owner = AppData.currentUser
            });

            AppData.currentLists[0].Items.Add(new ItemClass
            {
                Name = "Milk",
                Time = DateTime.UtcNow.ToString(),
                Purchased = false.ToString()
            });

            AppData.currentLists[0].Items.Add(new ItemClass
            {
                Name = "Bread",
                Time = DateTime.UtcNow.ToString(),
                Purchased = true.ToString()
            });


            AppData.currentLists.Add(new GroceryListClass
            {
                Name = "Office Supplies",
                Items = new List<ItemClass>(),
                Owner = AppData.currentUser
            });

            AppData.currentLists[1].Items.Add(new ItemClass
            {
                Name = "Pens",
                Time = DateTime.UtcNow.ToString(),
                Purchased = false.ToString()
            });

            AppData.currentLists[1].Items.Add(new ItemClass
            {
                Name = "Paper",
                Time = DateTime.UtcNow.ToString(),
                Purchased = true.ToString()
            });
        }
    }
}