using System;
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem.Runtime.Core;

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
        /// It is triggered when a new interactable focus is acquired.(Yeni bir interactable focus'a alındığında tetiklenir.)
        /// </summary>
        public event Action<IInteractable> OnInteractableFocused;

        /// <summary>
        /// Triggered when exiting the interactive focus.(Interactable focus'tan çıktığında tetiklenir.)
        /// </summary>
        public event Action OnInteractableLostFocus;

        /// <summary>
        /// Triggered when progress is updated (0-1).(Hold progress güncellendiğinde tetiklenir (0-1).)
        /// </summary>
        public event Action<float> OnHoldProgressChanged;

        #endregion

        #region Properties

        /// <summary>
        ///Currently in focus: interactable.(Şu an focus'ta olan interactable.)
        /// </summary>
        public IInteractable CurrentInteractable => m_CurrentInteractable;

        /// <summary>
        /// Interaction range.(Etkileşim menzili.)
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
            // Visualize the interaction range.(Etkileşim menzilini görselleştir)
            Gizmos.color = Color.yellow;
            Vector3 center = m_InteractionPoint != null ? m_InteractionPoint.position : transform.position;
            Gizmos.DrawWireSphere(center, m_InteractionRange);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Etkileşim noktasındaki menzil içinde bulunan tüm etkileşilebilir (interactable) nesneleri algılar,
        /// en yakınına focus atar ve focus değişimlerini yönetir.
        /// 
        /// Detects all interactable objects within the interaction range,
        /// determines the closest one, and handles focus changes.
        /// </summary>
        private void DetectInteractables()

        {
            m_InteractablesInRange.Clear();

            Collider[] colliders = Physics.OverlapSphere(
                m_InteractionPoint.position,
                m_InteractionRange,
                m_InteractableLayer
            );

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    m_InteractablesInRange.Add(interactable);
                }
            }

            // En yakın interactable'ı bul
            // Find the nearest interactable
            IInteractable closest = FindClosestInteractable();

            if (closest != m_CurrentInteractable)
            {
                // Önceki focus'u kaldır
                //Remove previous focus
                if (m_CurrentInteractable != null)
                {
                    m_CurrentInteractable.OnLoseFocus();
                    OnInteractableLostFocus?.Invoke();
                }

                // Yeni focus
                // New focus
                m_CurrentInteractable = closest;

                if (m_CurrentInteractable != null)
                {
                    m_CurrentInteractable.OnFocus();
                    OnInteractableFocused?.Invoke(m_CurrentInteractable);
                }

                // Hold timer'ı sıfırla
                // Reset the timer
                m_HoldTimer = 0f;
                m_IsHolding = false;
                OnHoldProgressChanged?.Invoke(0f);
            }
        }
        /// <summary>
        /// Menzil içindeki ve etkileşime uygun olan nesneler arasından
        /// etkileşim noktasına en yakın olanı bulur.
        /// 
        /// Finds and returns the closest interactable object within range
        /// that can currently be interacted with.
        /// </summary>
        private IInteractable FindClosestInteractable()

        {
            if (m_InteractablesInRange.Count == 0)
            {
                return null;
            }

            IInteractable closest = null;
            float closestDistance = float.MaxValue;

            foreach (var interactable in m_InteractablesInRange)
            {
                if (!interactable.CanInteract)
                {
                    continue;
                }

                var interactableTransform = (interactable as MonoBehaviour)?.transform;
                if (interactableTransform == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(
                    m_InteractionPoint.position,
                    interactableTransform.position
                );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = interactable;
                }
            }

            return closest;
        }

        /// <summary>
        /// Mevcut etkileşilebilir nesnenin etkileşim tipine göre
        /// uygun input işlemini yönetir.
        /// 
        /// Handles player input based on the current interactable's
        /// interaction type.
        /// </summary>
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

        /// <summary>
        /// Anında veya toggle tipindeki etkileşimleri,
        /// belirlenen tuşa basıldığında tetikler.
        /// 
        /// Handles instant or toggle interactions when the interaction key is pressed.
        /// </summary>
        private void HandleInstantInteraction()
        {
            if (Input.GetKeyDown(m_InteractionKey))
            {
                m_CurrentInteractable.OnInteract(gameObject);
            }
        }

        /// <summary>
        /// Basılı tutma (hold) gerektiren etkileşimleri yönetir,
        /// süreyi takip eder ve ilerleme bilgisini yayınlar.
        /// 
        /// Handles hold-based interactions, tracks hold duration,
        /// and updates hold progress events.
        /// </summary>
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