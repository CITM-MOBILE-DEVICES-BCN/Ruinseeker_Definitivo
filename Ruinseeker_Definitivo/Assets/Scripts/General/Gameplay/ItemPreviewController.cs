using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ruinseeker
{
    public class ItemPreviewController : MonoBehaviour
    {
        public ItemConfig itemConfig;
        public SpriteRenderer itemIcon;
        public TextMeshPro itemText; 
        private Collider2D itemCollider;

        void Start()
        {
            itemCollider = GetComponent<Collider2D>();
            itemIcon.sprite = itemConfig.itemIcon;

            itemText.text = itemConfig.itemText + itemConfig.price.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Inventory>().AddItemToInventory(itemConfig);
                Destroy(gameObject);
            }
        }
    }
}
