using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptPLayer
{
    public class Respawn : MonoBehaviour
    {
        public static List<Respawn> All = new List<Respawn>();

        private void Awake()
        {
            All.Add(this);
        }
    }
}
