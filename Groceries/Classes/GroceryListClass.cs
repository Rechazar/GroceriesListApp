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
   public class GroceryListClass
    {
        public string Name { get; set; }
        public List<ItemClass> Items { get; set; }
        public UserClass Owner { get; set; }
    }
}