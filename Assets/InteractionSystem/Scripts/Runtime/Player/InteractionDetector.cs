using System;
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Scripts.Manager;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Oyuncunun etkileşim algılama ve yönetim sistemi.
    /// </summary>
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        [Header("Detection Settings")]
        [SerializeField] private float m_InteractionRange = 3f;
        [SerializeField] private LayerMask m_InteractableLayer;
        [SerializeField] private Transform m_InteractionPoint;

        [Header("Input Settings")]
        [SerializeField] private KeyCode m_InteractionKey = KeyCode.E;

        private IInteractable m_CurrentInteractable;
        private float m_HoldTimer;
        private bool m_IsHolding;
        private List<IInteractable> m_InteractablesInRange = new List<IInteractable>();

        #endregion

        #region Events

        /// <summary>
        /// Yeni bir interactable focus'a alındığında tetiklenir.
        /// </summary>
        public event Action<IInteractable> OnInteractableFocused;

        /// <summary>
        /// Interactable focus'tan çıktığında tetiklenir.
        /// </summary>
        public event Action OnInteractableLostFocus;

        /// <summary>
        /// Hold progress güncellendiğinde tetiklenir (0-1).
        /// </summary>
        public event Action<float> OnHoldProgressChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Şu an focus'ta olan interactable.
        /// </summary>
        public IInteractable CurrentInteractable => m_CurrentInteractable;

        /// <summary>
        /// Etkileşim menzili.
        /// </summary>
        public float InteractionRange => m_InteractionRange;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_InteractionPoint == null)
            {
                m_InteractionPoint = transform;
            }
        }

        private void Update()
        {
            DetectInteractables();
            HandleInput();
        }

        private void OnDrawGizmosSelected()
        {
            // Etkileşim menzilini görselleştir
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position;
            Gizmos.DrawWireSphere(center, m_InteractionRange);
        }

        #endregion

        #region Methods
        [SerializeField] private Transform target;
        private void DetectInteractables()
        {
            m_InteractablesInRange.Clear();

            Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            m_InteractionRange,
            m_InteractableLayer
            );
            FindNearestInteractable(colliders);
        }

        private void FindNearestInteractable(Collider[] colliders)
        {
            foreach (var item in colliders)
            {
                if (item.TryGetComponent(out IInteractable interactable))
                {
                    m_InteractablesInRange.Add(interactable);
                }
            }

            if(colliders.Length == 0)
            {
                UIManager.Instance.InteractionPromptUISetActive(false);
                return;               
            }  
            target = colliders[0].transform;

            float nearTargetDistance = Vector3.Distance(transform.position, target.position);
            foreach (var item in colliders)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < nearTargetDistance)
                {
                    target = item.transform;
                    nearTargetDistance = distance;
                }
            }

            UIManager.Instance.InteractionPromptUISetActive(true);
        }

        private void HandleInput()
        {
            if (m_CurrentInteractable == null)
            {
                return;
            }

            switch (m_CurrentInteractable.InteractionType)
            {
                case InteractionType.Instant:
                case InteractionType.Toggle:
                    HandleInstantInteraction();
                    break;
                case InteractionType.Hold:
                    HandleHoldInteraction();
                    break;
            }
        }

        private void HandleInstantInteraction()
        {
            if (Input.GetKeyDown(m_InteractionKey))
            {
                m_CurrentInteractable.OnInteract(gameObject);
            }
        }

        private void HandleHoldInteraction()
        {
            if (Input.GetKey(m_InteractionKey))
            {
                m_IsHolding = true;
                m_HoldTimer += Time.deltaTime;

                float progress = m_HoldTimer / m_CurrentInteractable.HoldDuration;
                OnHoldProgressChanged?.Invoke(Mathf.Clamp01(progress));

                if (m_HoldTimer >= m_CurrentInteractable.HoldDuration)
                {
                    m_CurrentInteractable.OnInteract(gameObject);
                    m_HoldTimer = 0f;
                    m_IsHolding = false;
                    OnHoldProgressChanged?.Invoke(0f);
                }
            }
            else if (m_IsHolding)
            {
                m_HoldTimer = 0f;
                m_IsHolding = false;
                OnHoldProgressChanged?.Invoke(0f);
            }
        }

        #endregion
    }
}