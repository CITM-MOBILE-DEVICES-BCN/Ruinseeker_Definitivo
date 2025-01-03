using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    [CreateAssetMenu(fileName = "New Item Config", menuName = "Gameplay/Item Config")]
    public class ItemConfig : ScriptableObject
    {
        public enum ItemType
        {
            Star,
            Bomb,
            Boots,
            None
        }

        public Sprite itemIcon;
        public string itemText;
        public int price;   
        public ItemType itemType;

    }
}
