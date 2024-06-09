using UnityEngine;

namespace com.Klazapp.Utility
{
    public interface ICellEntity
    {
        public GameObject GameObject { get; }
        public RectTransform RectTransform { get; }
        
        public ICellComponent CellComponent { get; set; }

        public void SetData(ICellComponent comp, ScrollEventManager scrollEventManager = null);

        public virtual void Activate(bool isActive)
        {
            GameObject.SetActive(isActive);
        }
        
        public virtual void OnButtonPressed()
        {
            
        }
    }
}
