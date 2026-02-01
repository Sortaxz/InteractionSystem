using System;
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.Core.ScriptableObjects;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Basit oyuncu envanter sistemi.
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        #region Fields

        private List<ItemData> m_Items = new List<ItemData>();
        private Dictionary<string, int> m_KeyCounts = new Dictionary<string, int>();

        #endregion

        #region Events

        /// <summary>
        /// Item eklendiğinde tetiklenir.
        /// </summary>
        public event Action<ItemData> OnItemAdded;

        /// <summary>
        /// Item kullanıldığında/çıkarıldığında tetiklenir.
        /// </summary>
        public event Action<ItemData> OnItemRemoved;

        /// <summary>
        /// Envanter değiştiğinde tetiklenir.
        /// </summary>
        public event Action OnInventoryChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Envanterdeki tüm itemler.
        /// </summary>
        public IReadOnlyList<ItemData> Items => m_Items;

        /// <summary>
        /// Envanterdeki item sayısı.
        /// </summary>
        public int ItemCount => m_Items.Count;

        #endregion

        #region Methods

        /// <summary>
        /// Envantere item ekler.
        /// </summary>
        /// <param name="item">Eklenecek item.</param>
        public void AddItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogError("Cannot add null item to inventory!");
                return;
            }

            m_Items.Add(item);

            // Key ise ayrıca key sayacına ekle
            if (item.ItemType == ItemType.Key)
            {
                if (m_KeyCounts.ContainsKey(item.ItemId))
                {
                    m_KeyCounts[item.ItemId]++;
                }
                else
                {
                    m_KeyCounts[item.ItemId] = 1;
                }
            }

            OnItemAdded?.Invoke(item);
            OnInventoryChanged?.Invoke();

            Debug.Log($"Added {item.ItemName} to inventory");
        }

        /// <summary>
        /// Envanterden item çıkarır.
        /// </summary>
        /// <param name="item">Çıkarılacak item.</param>
        /// <returns>Başarılı mı?</returns>
        public bool RemoveItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogError("Cannot remove null item from inventory!");
                return false;
            }

            if (m_Items.Remove(item))
            {
                // Key ise sayacı güncelle
                if (item.ItemType == ItemType.Key && m_KeyCounts.ContainsKey(item.ItemId))
                {
                    m_KeyCounts[item.ItemId]--;
                    if (m_KeyCounts[item.ItemId] <= 0)
                    {
                        m_KeyCounts.Remove(item.ItemId);
                    }
                }

                OnItemRemoved?.Invoke(item);
                OnInventoryChanged?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Belirli bir anahtara sahip mi kontrolü.
        /// </summary>
        /// <param name="keyId">Anahtar ID'si.</param>
        /// <returns>Anahtar var mı?</returns>
        public bool HasKey(string keyId)
        {
            return m_KeyCounts.ContainsKey(keyId) && m_KeyCounts[keyId] > 0;
        }

        /// <summary>
        /// Anahtarı kullanır (envanterden çıkarır).
        /// </summary>
        /// <param name="keyId">Kullanılacak anahtar ID'si.</param>
        /// <returns>Başarılı mı?</returns>
        public bool UseKey(string keyId)
        {
            if (!HasKey(keyId))
            {
                Debug.LogWarning($"No key with ID: {keyId}");
                return false;
            }

            // İlgili key item'ı bul ve çıkar
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i].ItemType == ItemType.Key && m_Items[i].ItemId == keyId)
                {
                    var item = m_Items[i];
                    m_Items.RemoveAt(i);

                    m_KeyCounts[keyId]--;
                    if (m_KeyCounts[keyId] <= 0)
                    {
                        m_KeyCounts.Remove(keyId);
                    }

                    OnItemRemoved?.Invoke(item);
                    OnInventoryChanged?.Invoke();

                    Debug.Log($"Used key: {item.ItemName}");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Envanteri temizler.
        /// </summary>
        public void Clear()
        {
            m_Items.Clear();
            m_KeyCounts.Clear();
            OnInventoryChanged?.Invoke();
        }

        #endregion
    }
}
