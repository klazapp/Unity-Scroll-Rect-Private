#region Deprecated
// using UnityEngine;

// using System.Collections.Generic;
// using System.Runtime.CompilerServices;
// using Unity.Mathematics;
//
// namespace com.Klazapp.Utility
// {
//     public abstract class ScrollRectHandler : MonoBehaviour
//     {
//         #region Variables
//         [Header("Scroll Rect")]
//         [SerializeField]
//         private RectTransform content;
//         [SerializeField]
//         private RectTransform viewPort;
//
//         [Header("Cell Info")]
//         [SerializeField]
//         private CellInfo cellInfo;
//         
//         private List<CellComponent> cellData = new();
//         private Queue<CellEntity> cellPool = new();
//         private Dictionary<int, CellEntity> visibleCells = new();
//         private float itemHeight;
//         private int lastFirstVisibleIndex = -1;
//         private int lastLastVisibleIndex = -1;
//         
//         private List<int> indicesToRemove = new();
//         #endregion
//
//         #region Lifecycle Flow
//         private void OnEnable()
//         {
//             ScrollEventManager.OnTriggerCellClicked += CellClickedCallback;
//         }
//
//         private void OnDisable()
//         {
//             ScrollEventManager.OnTriggerCellClicked -= CellClickedCallback;
//         }
//         
//         private void Update()
//         {
//             UpdateVisibleItems();
//         }
//         #endregion
//
//         #region Public Access
//         #endregion
//         
//         #region Modules
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private void InitScrollRect()
//         {
//             
//         }
//         
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private void UpdateVisibleItems()
//         {
//             var viewportHeight = viewPort.rect.height;
//
//             // Calculate the first and last visible indices with extended range
//             const int BUFFER = 10;
//
//             var contentAnchoredPositionY = content.anchoredPosition.y;
//             var itemHeightWithSpacing = itemHeight + cellInfo.spacing;
//             var halfContentSizeDeltaY = content.sizeDelta.y / 2;
//         
//             var firstVisibleIndex = math.max(0, (int)math.floor((contentAnchoredPositionY - cellInfo.spacing) / itemHeightWithSpacing) - BUFFER);
//             var lastVisibleIndex = math.min(cellData.Count, firstVisibleIndex + (int)math.ceil(viewportHeight / itemHeightWithSpacing) + 2 * BUFFER);
//
//             if (firstVisibleIndex == lastFirstVisibleIndex && lastVisibleIndex == lastLastVisibleIndex)
//                 return;
//
//             lastFirstVisibleIndex = firstVisibleIndex;
//             lastLastVisibleIndex = lastVisibleIndex;
//
//             // Clear the list before reuse
//             indicesToRemove.Clear();
//
//             // Deactivate items that are out of the new visible range
//             foreach (var kvp in visibleCells)
//             {
//                 if (kvp.Key >= firstVisibleIndex - BUFFER && kvp.Key < lastVisibleIndex + BUFFER)
//                     continue;
//
//                 kvp.Value.gameObject.SetActive(false);
//                 cellPool.Enqueue(kvp.Value);
//                 indicesToRemove.Add(kvp.Key);
//             }
//
//             foreach (var index in indicesToRemove)
//             {
//                 visibleCells.Remove(index);
//             }
//
//             //Activate and update items that are now visible
//             for (var i = firstVisibleIndex; i < lastVisibleIndex; i++)
//             {
//                 if (visibleCells.ContainsKey(i))
//                     continue;
//
//                 var item = cellPool.Count > 0 ? cellPool.Dequeue() : Instantiate(cellInfo.entity, content);
//                 item.gameObject.SetActive(true);
//                 var rectTransform = item.GetComponent<RectTransform>();
//
//                 //Adjusting position to start from the top, considering spacing at the top
//                 var yPos = -cellInfo.spacing - (i * itemHeightWithSpacing) + halfContentSizeDeltaY - itemHeight / 2;
//                 rectTransform.anchoredPosition = new float2(0, yPos);
//                 visibleCells[i] = item;
//
//                 //Update item data here
//                 item.SetData(cellData[i]);
//             }
//         }
//         #endregion
//         
//         void Start()
//         {
//             // Example data population
//             for (var i = 0; i < 100; i++) // Set data count to 100
//             {
//                 cellData.Add(new CellComponent(i));
//             }
//
//             itemHeight = cellInfo.entity.GetComponent<RectTransform>().rect.height;
//             InitializePool();
//             UpdateContentHeight();
//             UpdateVisibleItems();
//         }
//
//
//         void InitializePool()
//         {
//             for (var i = 0; i < cellInfo.poolSize; i++)
//             {
//                 var item = Instantiate(cellInfo.entity, content);
//                 item.gameObject.SetActive(false);
//                 cellPool.Enqueue(item);
//             }
//         }
//
//         void UpdateContentHeight()
//         {
//             //Add spacing at the top and bottom
//             var totalContentHeight = cellData.Count * (itemHeight + cellInfo.spacing) + cellInfo.spacing; //Correctly add spacing at the bottom
//             content.sizeDelta = new float2(content.sizeDelta.x, totalContentHeight);
//         }
//
//         #region Callbacks
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         private void CellClickedCallback(CellEntity cellEntity)
//         {
//             Debug.Log("cell was clicked yay " + cellEntity.CellComponent.id);
//         }
//         #endregion
//     }
// }
#endregion

#region Deprecated
// using UnityEngine;
// using System.Collections.Generic;
// using System.Runtime.CompilerServices;
// using Unity.Mathematics;

// namespace com.Klazapp.Utility
// {
//     public abstract class ScrollRectHandler<T, T2> : MonoBehaviour
//         where T : CellComponent
//         where T2 : CellEntity, ICellEntity<T>
//     {
//         #region Variables
//         [Header("Scroll Rect")]
//         [SerializeField]
//         protected RectTransform content;
//         [SerializeField]
//         protected RectTransform viewPort;

//         [Header("Cell Info")]
//         [SerializeField]
//         protected CellInfo cellInfo;

//         protected List<T> cellData = new List<T>();
//         protected Queue<T2> cellPool = new Queue<T2>();
//         protected Dictionary<int, T2> visibleCells = new Dictionary<int, T2>();
//         protected float itemHeight;
//         protected int lastFirstVisibleIndex = -1;
//         protected int lastLastVisibleIndex = -1;

//         protected List<int> indicesToRemove = new List<int>();

//         protected ScrollEventManager scrollEventManager;
//         #endregion

//         #region Lifecycle Flow
//         protected virtual void OnEnable()
//         {
//             scrollEventManager ??= new ScrollEventManager();
//             scrollEventManager.OnTriggerCellClicked += CellClickedCallback;
//         }
        
//         protected virtual void OnDisable()
//         {
//             scrollEventManager.OnTriggerCellClicked -= CellClickedCallback;
//         }

//         protected virtual void Update()
//         {
//             UpdateVisibleItems();
//         }
//         #endregion

//         #region Public Access
//         public void SetData(List<T> cellData)
//         {
//             this.cellData = cellData;
//             UpdateContentHeight();
//             UpdateVisibleItems();
//         }

//         public void RefreshVisibleCells()
//         {
//             foreach (var kvp in visibleCells)
//             {
//                 kvp.Value.SetData(cellData[kvp.Key]);
//             }
//         }
//         #endregion

//         #region Modules
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         protected virtual void InitScrollRect()
//         {
//         }

//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         protected virtual void UpdateVisibleItems()
//         {
//             var viewportHeight = viewPort.rect.height;

//             // Calculate the first and last visible indices with extended range
//             const int BUFFER = 10;

//             var contentAnchoredPositionY = content.anchoredPosition.y;
//             var itemHeightWithSpacing = itemHeight + cellInfo.spacing;
//             var halfContentSizeDeltaY = content.sizeDelta.y / 2;

//             var firstVisibleIndex = math.max(0, (int)math.floor((contentAnchoredPositionY - cellInfo.spacing) / itemHeightWithSpacing) - BUFFER);
//             var lastVisibleIndex = math.min(cellData.Count, firstVisibleIndex + (int)math.ceil(viewportHeight / itemHeightWithSpacing) + 2 * BUFFER);

//             if (firstVisibleIndex == lastFirstVisibleIndex && lastVisibleIndex == lastLastVisibleIndex)
//                 return;

//             lastFirstVisibleIndex = firstVisibleIndex;
//             lastLastVisibleIndex = lastVisibleIndex;

//             // Clear the list before reuse
//             indicesToRemove.Clear();

//             // Deactivate items that are out of the new visible range
//             foreach (var kvp in visibleCells)
//             {
//                 if (kvp.Key >= firstVisibleIndex - BUFFER && kvp.Key < lastVisibleIndex + BUFFER)
//                     continue;

//                 kvp.Value.gameObject.SetActive(false);
//                 cellPool.Enqueue(kvp.Value);
//                 indicesToRemove.Add(kvp.Key);
//             }

//             foreach (var index in indicesToRemove)
//             {
//                 visibleCells.Remove(index);
//             }

//             // Activate and update items that are now visible
//             for (var i = firstVisibleIndex; i < lastVisibleIndex; i++)
//             {
//                 if (visibleCells.ContainsKey(i))
//                     continue;

//                 T2 item;
//                 if (cellPool.Count > 0)
//                 {
//                     item = cellPool.Dequeue();
//                 }
//                 else
//                 {
//                     var obj = Instantiate(cellInfo.entity, content);
//                     item = obj.GetComponent<T2>();
//                     if (item == null)
//                     {
//                         throw new MissingComponentException($"The prefab {cellInfo.entity.name} is missing the required component {typeof(T2).Name}.");
//                     }
//                 }

//                 item.gameObject.SetActive(true);
//                 var rectTransform = item.GetComponent<RectTransform>();

//                 // Adjusting position to start from the top, considering spacing at the top
//                 var yPos = -cellInfo.spacing - (i * itemHeightWithSpacing) + halfContentSizeDeltaY - itemHeight / 2;
//                 rectTransform.anchoredPosition = new Vector2(0, yPos);
//                 visibleCells[i] = item;

//                 // Update item data here
//                 item.SetData(cellData[i], scrollEventManager);
//             }
//         }
//         #endregion

//         protected virtual void OnCreated()
//         {
//             // // Example data population
//             // for (var i = 0; i < 100; i++) // Set data count to 100
//             // {
//             //     cellData.Add((T)System.Activator.CreateInstance(typeof(T), new object[] { i }));
//             // }

//             itemHeight = cellInfo.entity.GetComponent<RectTransform>().rect.height;
//             InitializePool();
//             UpdateContentHeight();
//             UpdateVisibleItems();
//         }

//         protected virtual void InitializePool()
//         {
//             for (var i = 0; i < cellInfo.poolSize; i++)
//             {
//                 var obj = Instantiate(cellInfo.entity, content);
//                 var item = obj.GetComponent<T2>();
//                 if (item == null)
//                 {
//                     throw new MissingComponentException($"The prefab {cellInfo.entity.name} is missing the required component {typeof(T2).Name}.");
//                 }
//                 item.gameObject.SetActive(false);
//                 cellPool.Enqueue(item);
//             }
//         }

//         protected virtual void UpdateContentHeight()
//         {
//             // Add spacing at the top and bottom
//             var totalContentHeight = cellData.Count * (itemHeight + cellInfo.spacing) + cellInfo.spacing; // Correctly add spacing at the bottom
//             content.sizeDelta = new Vector2(content.sizeDelta.x, totalContentHeight);

//             // Temporarily added this to ensure content will always be scrolled to the top
//             var anchoredPosition = content.anchoredPosition;
//             anchoredPosition.y = -totalContentHeight/2f ;
//             content.anchoredPosition = anchoredPosition;
//         }

//         #region Callbacks
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         protected virtual void CellClickedCallback(CellEntity cellEntity)
//         {
//             var typedCellEntity = cellEntity as T2;
//             if (typedCellEntity != null)
//             {
//                 Debug.Log("cell was clicked yay " + typedCellEntity.CellComponent.id);
//             }
//         }
//         #endregion
//     }
// }
#endregion

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace com.Klazapp.Utility
{
    public abstract class ScrollRectHandler<T, T2> : MonoBehaviour
        where T : CellComponent
        where T2 : CellEntity, ICellEntity<T>
    {
        #region Variables
        [Header("Scroll Rect")]
        [SerializeField]
        protected RectTransform content;
        [SerializeField]
        protected RectTransform viewPort;

        [Header("Cell Info")]
        [SerializeField]
        protected CellInfo cellInfo;

        protected List<T> cellData = new List<T>();
        protected Queue<T2> cellPool = new Queue<T2>();
        protected Dictionary<int, T2> visibleCells = new Dictionary<int, T2>();
        protected float itemHeight;
        protected int lastFirstVisibleIndex = -1;
        protected int lastLastVisibleIndex = -1;

        protected List<int> indicesToRemove = new List<int>();

        protected ScrollEventManager scrollEventManager;
        #endregion

        #region Lifecycle Flow
        protected void OnSubscribe()
        {
            scrollEventManager ??= new ScrollEventManager();
            scrollEventManager.OnTriggerCellClicked += CellClickedCallback;
        }

        protected void OnUnsubscribe()
        {
            scrollEventManager.OnTriggerCellClicked -= CellClickedCallback;
        }

        protected virtual void Update()
        {
            UpdateVisibleItems();
        }

        protected virtual void Start()
        {
            OnCreated();
        }
        #endregion

        #region Public Access
        public void SetData(List<T> cellData)
        {
            this.cellData = cellData;
            UpdateContentHeight();
            UpdateVisibleItems();
        }

        public void RefreshVisibleCells()
        {
            foreach (var kvp in visibleCells)
            {
                kvp.Value.SetData(cellData[kvp.Key]);
            }
        }
        #endregion

        #region Modules
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void InitScrollRect()
        {
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void UpdateVisibleItems()
        {
            var viewportHeight = viewPort.rect.height;
            var contentAnchoredPositionY = content.anchoredPosition.y;
            var itemHeightWithSpacing = itemHeight + cellInfo.spacing;
            var halfContentSizeDeltaY = content.sizeDelta.y / 2;

            // Calculate the first visible index using the formula
            // var firstVisibleIndex = math.max(0, (int)math.floor((contentAnchoredPositionY + viewportHeight / 2) / itemHeightWithSpacing));
            // var lastVisibleIndex = math.min(cellData.Count, firstVisibleIndex + (int)math.ceil(viewportHeight / itemHeightWithSpacing) + 1);
            var contentTopYPos = -content.rect.height/2f;
            var firstVisibleIndex = (int)math.ceil((contentAnchoredPositionY - contentTopYPos)/(itemHeightWithSpacing)) - 1;
			
            var permissibleCellCount = viewportHeight / itemHeightWithSpacing;
            var lastVisibleIndex = (int)math.ceil(permissibleCellCount + firstVisibleIndex - 1) + 2;
			
            if(lastVisibleIndex > cellData.Count)
            {
                lastVisibleIndex = cellData.Count;
            }

            if(firstVisibleIndex < 0)
            {
                firstVisibleIndex = 0;
            }
            
            if (firstVisibleIndex == lastFirstVisibleIndex && lastVisibleIndex == lastLastVisibleIndex)
                return;

            lastFirstVisibleIndex = firstVisibleIndex;
            lastLastVisibleIndex = lastVisibleIndex;

            // Clear the list before reuse
            indicesToRemove.Clear();

            // Deactivate items that are out of the new visible range
            foreach (var kvp in visibleCells)
            {
                if (kvp.Key >= firstVisibleIndex && kvp.Key < lastVisibleIndex)
                    continue;

                kvp.Value.gameObject.SetActive(false);
                cellPool.Enqueue(kvp.Value);
                indicesToRemove.Add(kvp.Key);
            }

            foreach (var index in indicesToRemove)
            {
                visibleCells.Remove(index);
            }

            // Activate and update items that are now visible
            for (var i = firstVisibleIndex; i < lastVisibleIndex; i++)
            {
                if (visibleCells.ContainsKey(i))
                    continue;

                T2 item;
                if (cellPool.Count > 0)
                {
                    item = cellPool.Dequeue();
                }
                else
                {
                    var obj = Instantiate(cellInfo.entity, content);
                    item = obj.GetComponent<T2>();
                    if (item == null)
                    {
                        throw new MissingComponentException($"The prefab {cellInfo.entity.name} is missing the required component {typeof(T2).Name}.");
                    }
                }

                item.gameObject.SetActive(true);
                var rectTransform = item.GetComponent<RectTransform>();

                // Adjusting position to start from the top
                var yPos = -cellInfo.spacing - (i * itemHeightWithSpacing) + halfContentSizeDeltaY - itemHeight / 2;
                rectTransform.anchoredPosition = new Vector2(0, yPos);
                visibleCells[i] = item;

                // Update item data here
                item.SetData(cellData[i], scrollEventManager);
            }
        }
        #endregion

        protected virtual void OnCreated()
        {
            itemHeight = cellInfo.entity.GetComponent<RectTransform>().rect.height;
            InitializePool();
            UpdateContentHeight();
            UpdateVisibleItems();

            // Set the initial position of the content to ensure it starts at the top
            //content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
        }

        protected virtual void InitializePool()
        {
            for (var i = 0; i < cellInfo.poolSize; i++)
            {
                var obj = Instantiate(cellInfo.entity, content);
                var item = obj.GetComponent<T2>();
                if (item == null)
                {
                    throw new MissingComponentException($"The prefab {cellInfo.entity.name} is missing the required component {typeof(T2).Name}.");
                }
                item.gameObject.SetActive(false);
                cellPool.Enqueue(item);
            }
        }

        protected virtual void UpdateContentHeight()
        {
            // Add spacing at the top and bottom
            var totalContentHeight = cellData.Count * (itemHeight + cellInfo.spacing); // Correctly add spacing at the bottom
            content.sizeDelta = new Vector2(content.sizeDelta.x, totalContentHeight);

            // Temporarily added this to ensure content will always be scrolled to the top
            var anchoredPosition = content.anchoredPosition;
            anchoredPosition.y = -totalContentHeight / 2f;

            content.anchoredPosition = anchoredPosition;
        }

        #region Callbacks
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void CellClickedCallback(CellEntity cellEntity)
        {
            var typedCellEntity = cellEntity as T2;
            if (typedCellEntity != null)
            {
                Debug.Log("cell was clicked yay " + typedCellEntity.CellComponent.id);
            }
        }
        #endregion
    }
}
