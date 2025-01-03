using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ruinseeker
{
    public class MinigamesButtons : MonoBehaviour
    {
        public Button ruinseekerButton;
        public Button waltersWalkButton;
        public Button kyotoNinjaButton;
        public Button robotinButton;

        public void Awake()
        {
            ruinseekerButton.onClick.AddListener(() => GameManager.Instance.LoadSceneRequest("LevelSelectorScene"));
            //WaltersWalkButton.onClick.AddListener(() => WaltersWalk());
            //KyotoNinjaButton.onClick.AddListener(() => KyotoNinja());
            //RobotinButton.onClick.AddListener(() => Robotin());
        }
    }
}
