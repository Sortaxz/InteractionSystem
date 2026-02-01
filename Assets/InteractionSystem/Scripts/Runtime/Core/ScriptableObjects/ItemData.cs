using UnityEngine;

namespace InteractionSystem.Runtime.Core.ScriptableObjects
{
    /// <summary>
    /// Item tanım verileri için ScriptableObject.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "Interaction System/Item Data")]
    public class ItemData : ScriptableObject
    {
        #region Fields

        [Header("Item Info")]
        [SerializeField] private string m_ItemId;
        [SerializeField] private string m_ItemName;
        [SerializeField] private string m_Description;
        [SerializeField] private Sprite m_Icon;

        [Header("Item Type")]
        [SerializeField] private ItemType m_ItemType = ItemType.Generic;

        #endregion

        #region Properties

        /// <summary>
        /// Benzersiz item ID'si.
        /// </summary>
        public string ItemId => m_ItemId;

        /// <summary>
        /// Item adı.
        /// </summary>
        public string ItemName => m_ItemName;

        /// <summary>
        /// Item açıklaması.
        /// </summary>
        public string Description => m_Description;

        /// <summary>
        /// Item ikonu.
        /// </summary>
        public Sprite Icon => m_Icon;

        /// <summary>
        /// Item tipi.
        /// </summary>
        public ItemType ItemType => m_ItemType;

        #endregion
    }
}