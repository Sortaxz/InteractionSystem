using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Core.ScriptableObjects;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Toplanabilir anahtar.
    /// </summary>
    public class KeyPickup : BaseInteractable
    {
        #region Fields

        [Header("Key Settings")]
        [SerializeField] private ItemData m_KeyData;
        [SerializeField] private bool m_DestroyOnPickup = true;

        private bool m_IsCollected;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override string InteractionPrompt => $"Press E to Pick Up {m_KeyData?.ItemName ?? "Key"}";

        /// <inheritdoc/>
        public override bool CanInteract => !m_IsCollected;

        /// <summary>
        /// Anahtar verisi.
        /// </summary>
        public ItemData KeyData => m_KeyData;

        #endregion

        #region Methods

        #endregion
    }
}