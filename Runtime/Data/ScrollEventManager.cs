using System;
using UnityEngine;

namespace com.Klazapp.Utility
{
    public class ScrollEventManager 
    {
        public  event Action<CellEntity> OnTriggerCellClicked;
        public  void InvokeCellClicked(CellEntity cellEntity)
        {
            OnTriggerCellClicked?.Invoke(cellEntity);
        }
    }
}
