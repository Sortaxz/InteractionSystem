using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.Core.ScriptableObjects;

namespace InteractionSystem.Runtime.UI
{
    /// <summary>
    /// Envanter UI yönetimi.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private PlayerInventory m_Inventory;
        [SerializeField] private Transform m_ItemContainer;
        [SerializeField] private GameObject m_ItemPrefab;
        [SerializeField] private TextMeshProUGUI m_ItemCountText;

        private List<GameObject> m_SpawnedItems = new List<GameObject>();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (m_Inventory != null)
            {
                m_Inventory.OnInventoryChanged += RefreshUI;
            }
        }

        private void OnDisable()
        {
            if (m_Inventory != null)
            {
                m_Inventory.OnInventoryChanged -= RefreshUI;
            }
        }

        private void Start()
        {
            RefreshUI();
        }

        #endregion

        #region Methods

        /// <summary>
        /// UI'ı yeniler.
        /// </summary>
        public void RefreshUI()
        {
            ClearItems();

            if (m_Inventory == null || m_ItemContainer == null || m_ItemPrefab == null)
            {
                return;
            }

            foreach (var item in m_Inventory.Items)
            {
                CreateItemEntry(item);
            }

            UpdateItemCount();
        }

        private void CreateItemEntry(ItemData item)
        {
            var itemGO = Instantiate(m_ItemPrefab, m_ItemContainer);
            m_SpawnedItems.Add(itemGO);

            // İkon
            var iconImage = itemGO.GetComponentInChildren<Image>();
            if (iconImage != null && item.Icon != null)
            {
                iconImage.sprite = item.Icon;
            }

            // Text
            var itemText = itemGO.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = item.ItemName;
            }
        }

        private void ClearItems()
        {
            foreach (var item in m_SpawnedItems)
            {
                if (item != null)
                {
                    Destroy(item);
                }
            }
            m_SpawnedItems.Clear();
        }

        private void UpdateItemCount()
        {
            if (m_ItemCountText != null && m_Inventory != null)
            {
                m_ItemCountText.text = $"Items: {m_Inventory.ItemCount}";
            }
        }

        #endregion
    }
}