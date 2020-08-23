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
    public class ItemClass
    {
        public string Name { get; set; }
        public string Time { get; set; }
        public string Purchased { get; set; }
    }
}