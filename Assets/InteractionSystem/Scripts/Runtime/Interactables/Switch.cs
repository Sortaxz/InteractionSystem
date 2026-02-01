using System;
using UnityEngine;
using UnityEngine.Events;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Toggle switch/lever.
    /// </summary>
    public class Switch : BaseInteractable
    {
        #region Fields

        [Header("Switch Settings")]
        [SerializeField] private bool m_IsOn;
        [SerializeField] private UnityEvent m_OnActivated;
        [SerializeField] private UnityEvent m_OnDeactivated;

        private const string k_OnPrompt = "Press E to Turn Off";
        private const string k_OffPrompt = "Press E to Turn On";

        #endregion

        #region Events

        /// <summary>
        /// Switch aktifleştirildiğinde tetiklenir.
        /// </summary>
        public event Action OnSwitchActivated;

        /// <summary>
        /// Switch deaktifleştirildiğinde tetiklenir.
        /// </summary>
        public event Action OnSwitchDeactivated;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override string InteractionPrompt => m_IsOn ? k_OnPrompt : k_OffPrompt;

        /// <summary>
        /// Switch açık mı?
        /// </summary>
        public bool IsOn => m_IsOn;

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override void OnInteract(GameObject interactor)
        {
            Toggle();
            base.OnInteract(interactor);
        }

        /// <summary>
        /// Switch'i toggle eder.
        /// </summary>
        public void Toggle()
        {
            m_IsOn = !m_IsOn;

            if (m_IsOn)
            {
                m_OnActivated?.Invoke();
                OnSwitchActivated?.Invoke();
            }
            else
            {
                m_OnDeactivated?.Invoke();
                OnSwitchDeactivated?.Invoke();
            }
        }

        /// <summary>
        /// Switch'i aktifleştirir.
        /// </summary>
        public void Activate()
        {
            if (!m_IsOn)
            {
                Toggle();
            }
        }

        /// <summary>
        /// Switch'i deaktifleştirir.
        /// </summary>
        public void Deactivate()
        {
            if (m_IsOn)
            {
                Toggle();
            }
        }

        #endregion
    }
}