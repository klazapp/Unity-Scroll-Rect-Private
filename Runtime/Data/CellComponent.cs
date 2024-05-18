using System;
using UnityEngine;

namespace com.Klazapp.Utility
{
    [Serializable]
    public class CellComponent
    {
        [HideInInspector]
        public int id;

        public CellComponent(int cellId)
        {
            this.id = cellId;
        }
    }
}
