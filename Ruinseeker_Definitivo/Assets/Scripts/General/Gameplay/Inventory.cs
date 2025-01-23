using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruinseeker;
namespace Ruinseeker
{
    public class Inventory : MonoBehaviour
    {
        private ItemConfig currentItem;
        public ItemButton itemButton;
        private PlayerMovement playerMovement;

        private void Awake()
        {
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
                Debug.Log("Full Inventory");
            }
        }
        private void ExplodeBomb()
        {
            Debug.Log("Bomb used!");

            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found!");
                return;
            }

            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
            Rect visibleArea = new Rect(
                bottomLeft.x,
                bottomLeft.y,
                topRight.x - bottomLeft.x,
                topRight.y - bottomLeft.y
            );

            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            
            foreach (var enemy in allEnemies)
            {
                if (visibleArea.Contains(enemy.transform.position))
                {
                    Debug.Log($"Enemy {enemy.name} is in camera view and will be destroyed.");
                    enemy.Die();
                }
            }
        }
        private void ActivateBoots()
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("Player not found!");
                return;
            }

            // Activa el efecto de las botas
            playerMovement.hasBoots = true;
            Debug.Log("Player now has boots!");

            StartCoroutine(DeactivateBootsAfterDelay(playerMovement, 10f));
        }
        private void ActivateStar()
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("Player not found!");
                return;
            }

            playerMovement.hasStar = true;
            Debug.Log("Player now has star!");

            StartCoroutine(DeactivateStarAfterDelay(playerMovement, 10f));
        }
        private IEnumerator DeactivateStarAfterDelay(PlayerMovement player, float delay)
        {
            yield return new WaitForSeconds(delay);

            player.hasStar = false;
            Debug.Log("Star effect expired.");
        }
        private IEnumerator DeactivateBootsAfterDelay(PlayerMovement player, float delay)
        {
            yield return new WaitForSeconds(delay);
            player.hasBoots = false;
            Debug.Log("Boots effect expired.");
        }
        public void UseItem()
        {
            if(currentItem != null && ScoreManager.Instance.CurrentGems >= currentItem.price)
            {
                ScoreManager.Instance.AddGems(-currentItem.price);
                switch (currentItem.itemType)
                {
                    case ItemConfig.ItemType.Star:
                        Debug.Log("Star");
                        ActivateStar();
                        break;
                    case ItemConfig.ItemType.Bomb:
                        Debug.Log("Bomb");
                        ExplodeBomb();

                        break;
                    case ItemConfig.ItemType.Boots:
                        Debug.Log("Boots");
                        ActivateBoots();
                        break;
                }   


                currentItem = null;
                itemButton.ResetItem();
            }
        }
    }
}
