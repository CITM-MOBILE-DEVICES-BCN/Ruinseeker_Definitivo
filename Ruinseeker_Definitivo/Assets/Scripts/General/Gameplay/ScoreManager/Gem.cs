using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class Gem : Collectable
    {
        protected override void OnCollect()
        {
            ScoreManager.Instance.AddGems(value);
            base.OnCollect();
        }
    }
}
