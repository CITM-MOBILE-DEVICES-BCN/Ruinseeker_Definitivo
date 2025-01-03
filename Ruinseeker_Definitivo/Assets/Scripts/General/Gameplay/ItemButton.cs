using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class ItemButton : MonoBehaviour
    {
        public Image itemImage;  
        public Button itemButton;


        public void Init(UnityAction useItem)
        {
            itemButton.onClick.AddListener(useItem);
        }

        public void SetItem(ItemConfig item)
        {
            if(itemImage.gameObject.activeSelf == false)
            {
                itemImage.gameObject.SetActive(true);
            }
            itemImage.sprite = item.itemIcon;
        }
        
        public void ResetItem()
        {
            itemImage.gameObject.SetActive(false);
        }

    }
}
