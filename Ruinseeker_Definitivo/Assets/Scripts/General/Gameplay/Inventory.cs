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
        private void ExplodeBomb()
        {
            Debug.Log("Bomb used!");

            // Obt�n el rect�ngulo que representa el �rea visible de la c�mara.
            Camera mainCamera = Camera.main; // Obtiene la c�mara principal.
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found!");
                return;
            }

            // Calcula los l�mites del �rea visible.
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
            Rect visibleArea = new Rect(
                bottomLeft.x,
                bottomLeft.y,
                topRight.x - bottomLeft.x,
                topRight.y - bottomLeft.y
            );

            // Encuentra todos los enemigos en la escena.
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            
            foreach (var enemy in allEnemies)
            {
                // Comprueba si el enemigo est� dentro del �rea visible.
                if (visibleArea.Contains(enemy.transform.position))
                {
                    Debug.Log($"Enemy {enemy.name} is in camera view and will be destroyed.");
                    enemy.Die(); // Llama a la funci�n `Die()` del enemigo.
                }
            }
        }
        private void ActivateBoots()
        {
            // Encuentra el objeto del jugador
            playerMovement = FindObjectOfType<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("Player not found!");
                return;
            }

            // Activa el efecto de las botas
            playerMovement.hasBoots = true;
            Debug.Log("Player now has boots!");

            // Inicia la corrutina para desactivar las botas despu�s de 10 segundos
            StartCoroutine(DeactivateBootsAfterDelay(playerMovement, 10f));
        }

        private IEnumerator DeactivateBootsAfterDelay(PlayerMovement player, float delay)
        {
            yield return new WaitForSeconds(delay);

            // Desactiva las botas
            player.hasBoots = false;
            Debug.Log("Boots effect expired.");
        }
        public void UseItem()
        {
            if(currentItem != null)
            {
                //TODO: Use the item
                switch(currentItem.itemType)
                {
                    case ItemConfig.ItemType.Star:
                        Debug.Log("Star");
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
