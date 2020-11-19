using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    public class GameEntry : MonoSingleton<GameEntry>
    {
        private void Awake()
        {

        }

        private void Start()
        {
            StartLaunch();
        }

        private void StartLaunch()
        {
            var iLRuntimeEntry = ILRuntimeEntry.Instance;
        }
    }
}
