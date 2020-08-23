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
    public static class CompareLists
    {
        public static List<GroceryListClass> Compare(List<GroceryListClass> listA, List<GroceryListClass> listB)
        {
            List<GroceryListClass> combinedListsLST = new List<GroceryListClass>();

            //first, if there is a list in one that is not in the other, just copy it
            foreach (GroceryListClass a in listA)
            {
                foreach (GroceryListClass anyList in listB)
                {
                    if (a.Name == anyList.Name)
                    {
                        goto ContinueLoop;
                    }
                }
                combinedListsLST.Add(a);
            ContinueLoop:;
            }

            foreach (GroceryListClass b in listB)
            {
                foreach (GroceryListClass anyList in listA)
                {
                    if (b.Name == anyList.Name)
                    {
                        goto ContinueLoop;
                    }
                }
                combinedListsLST.Add(b);
            ContinueLoop:;
            }

            //if added unique lists, remove them
            foreach (GroceryListClass any in combinedListsLST)
            {
                if (listA.Contains(any))
                {
                    listA.Remove(any);
                }
                if (listB.Contains(any))
                {
                    listB.Remove(any);
                }
            }

            //comparing similar lists with eachother
            foreach (GroceryListClass anyListA in listA)
            {
                List<ItemClass> thisListResultItems = new List<ItemClass>();

                GroceryListClass counterPartList = new GroceryListClass();
                DateTime combinedTime = DateTime.UtcNow;

                //checking the deleted status
                foreach (GroceryListClass anyListB in listB)
                {
                    if (anyListB.Name == anyListA.Name)
                    {
                        counterPartList = anyListB;
                        break;
                    }
                }

                //checking the items
                foreach (ItemClass anyItem in anyListA.Items)
                {
                    //compare items from one list to another
                    foreach (ItemClass counterItem in counterPartList.Items)
                    {
                        if (anyItem.Name == counterItem.Name)
                        {
                            //if item exists both sides, we decide which to add based on the one with the latest timestamp on it
                            if (DateTime.Parse(anyItem.Time) > DateTime.Parse(counterItem.Time))
                            {
                                thisListResultItems.Add(anyItem);
                            }
                            else
                            {
                                thisListResultItems.Add(counterItem);
                            }
                            goto ContinueHere;
                        }
                    }
                    //if this is reached where none of the names have matched we then add the item
                    thisListResultItems.Add(anyItem);
                ContinueHere:;
                }

                //all items of this list should be done, now for counterpart
                foreach (ItemClass anyCounterItem in counterPartList.Items)
                {
                    //compare items from counter to main
                    foreach (ItemClass anyItem in anyListA.Items)
                    {
                        if (anyCounterItem.Name == anyItem.Name)
                        {
                            //items exists on both sides, drop out
                            goto ContinueHere;
                        }
                    }
                    //same again if no item names have matched
                    thisListResultItems.Add(anyCounterItem);
                ContinueHere:;
                }

                //shopping class cotains all similar and unique lists and items
                combinedListsLST.Add(new GroceryListClass
                {
                    Name = anyListA.Name,
                    Owner = anyListA.Owner,
                    Items = thisListResultItems
                });
            }
            return combinedListsLST;
        }
    }
}