using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ruinseeker
{
    public class ItemPreviewController : Collectable
    {
        public ItemConfig itemConfig;
        public SpriteRenderer itemIcon;
        public TextMeshPro itemText; 

        void Start()
        {
            itemIcon.sprite = itemConfig.itemIcon;

            itemText.text = itemConfig.itemText + itemConfig.price.ToString();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                //if (ScoreManager.Instance.CurrentGems >= itemConfig.price)
                //{
                    ScoreManager.Instance.AddGems(-itemConfig.price);
                    collision.GetComponent<Inventory>().AddItemToInventory(itemConfig);
                    OnCollect();
                //}
            }
        }
    }
}
