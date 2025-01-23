using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class PlatformsManager : MonoBehaviour
    {

        public GameObject[] platforms;
        public GameObject player;

        void Update()
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                if (player.transform.position.y - 0.4f > platforms[i].transform.position.y)
                {
                    platforms[i].GetComponent<BoxCollider2D>().enabled = true;
                }
                else if (player.transform.position.y - 0.3f < platforms[i].transform.position.y)
                {
                    platforms[i].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
