using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Core.ScriptableObjects;
using InteractionSystem.Runtime.Player;

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
        /// <summary>
        /// Eylem tetiklendiğinde çağrılır.
        /// </summary>
        /// <param name="interactor">Eylemi tetikleyen nesne.</param>
        public override void OnInteract(GameObject interactor)
        {
            if (m_IsCollected)
            {
                Debug.LogWarning("Key already collected!");
                return;
            }

            if (m_KeyData == null)
            {
                Debug.LogError("KeyData is not assigned!");
                return;
            }

            if (interactor != null && interactor.TryGetComponent<PlayerInventory>(out var inventory))
            {
                inventory.AddItem(m_KeyData);
                m_IsCollected = true;

                base.OnInteract(interactor);

                if (m_DestroyOnPickup)
                {
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Interactor does not have PlayerInventory component!");
            }
        }
        #endregion
    }
}