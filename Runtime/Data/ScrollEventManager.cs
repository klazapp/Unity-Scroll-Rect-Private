using System;
using UnityEngine;

namespace com.Klazapp.Utility
{
    public class ScrollEventManager : MonoBehaviour
    {
        public static event Action<CellEntity> OnTriggerCellClicked;
        public static void InvokeCellClicked(CellEntity cellEntity)
        {
            OnTriggerCellClicked?.Invoke(cellEntity);
        }
    }
}
