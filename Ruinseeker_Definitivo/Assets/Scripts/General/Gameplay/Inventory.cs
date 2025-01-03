using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class Inventory : MonoBehaviour
    {
        private ItemConfig currentItem;
        public ItemButton itemButton;


        private void Awake()
        {
            //TODO: save and load function for items
            itemButton.Init(UseItem);
            if(currentItem != null)
            {
                itemButton.SetItem(currentItem);
            }
        }

        public void AddItemToInventory(ItemConfig item)
        {
            if(currentItem == null)
            {
                currentItem = item;
                itemButton.SetItem(item);
            }
            else 
            {
                //TODO: Implement a complete inventory system (replace, not add, choose which one you want, etc)
                Debug.Log("Full Inventory");
            }
        }

        public void UseItem()
        {
            if(currentItem != null)
            {
                //TODO: Use the item


                currentItem = null;
                itemButton.ResetItem();
            }
        }
    }
}
