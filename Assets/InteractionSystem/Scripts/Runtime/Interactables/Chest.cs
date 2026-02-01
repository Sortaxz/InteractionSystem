using System;
using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Core.ScriptableObjects;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Hold etkileşimi ile açılan sandık.
    /// </summary>
    public class Chest : BaseInteractable
    {
        #region Fields

        [Header("Chest Settings")]
        [SerializeField] private ItemData m_ContainedItem;
        [SerializeField] private Transform m_LidTransform;
        [SerializeField] private float m_OpenAngle = -110f;
        [SerializeField] private float m_OpenSpeed = 2f;

        private bool m_IsOpened;
        private float m_CurrentAngle;
        private float m_TargetAngle;

        private const string k_ClosedPrompt = "Hold E to Open";
        private const string k_OpenedPrompt = "Chest is Empty";

        #endregion

        #region Events

        /// <summary>
        /// Sandık açıldığında tetiklenir.
        /// </summary>
        public event Action OnChestOpened;

        /// <summary>
        /// İçindeki item alındığında tetiklenir.
        /// </summary>
        public event Action<ItemData> OnItemCollected;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override string InteractionPrompt => m_IsOpened ? k_OpenedPrompt : k_ClosedPrompt;

        /// <inheritdoc/>
        public override bool CanInteract => !m_IsOpened;

        /// <summary>
        /// Sandık açıldı mı?
        /// </summary>
        public bool IsOpened => m_IsOpened;

        #endregion

        #region Unity Methods

        private void Update()
        {
            UpdateLidRotation();
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override void OnInteract(GameObject interactor)
        {
            if (m_IsOpened)
            {
                Debug.Log("Chest is already opened!");
                return;
            }

            Open(interactor);
            base.OnInteract(interactor);
        }

        /// <summary>
        /// Sandığı açar.
        /// </summary>
        /// <param name="interactor">Etkileşimi yapan GameObject.</param>
        public void Open(GameObject interactor)
        {
            
        }

        private void UpdateLidRotation()
        {
            if (m_LidTransform == null)
            {
                return;
            }

            if (Mathf.Abs(m_CurrentAngle - m_TargetAngle) > 0.1f)
            {
                m_CurrentAngle = Mathf.Lerp(m_CurrentAngle, m_TargetAngle, Time.deltaTime * m_OpenSpeed);
                m_LidTransform.localRotation = Quaternion.Euler(m_CurrentAngle, 0f, 0f);
            }
        }

        #endregion
    }
}