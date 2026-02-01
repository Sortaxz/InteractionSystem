using System;
using UnityEngine;
using InteractionSystem.Runtime.Core;
using System.Collections;
using InteractionSystem.Runtime.Player;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Açılıp kapanabilen kapı.
    /// </summary>
    public class Door : BaseInteractable
    {
        #region Field

        [Header("Door Component")]
        [SerializeField] private Animator m_Animator;

        [Header("Door Settings")]
        [SerializeField] private bool m_IsLocked;
        [SerializeField] private string m_RequiredKeyId;
        [SerializeField] private float m_OpenAngle = 90f;
        [SerializeField] private float m_OpenSpeed = 2f;
        [SerializeField] private Transform m_DoorPivot;

        private bool m_IsOpen;
        private float m_CurrentAngle;
        private float m_TargetAngle;

        private const string k_LockedPrompt = "Locked - Key Required";
        private const string k_OpenPrompt = "Press E to Open";
        private const string k_ClosePrompt = "Press E to Close";

        #endregion

        #region Events

        /// <summary>
        /// Kapı açıldığında tetiklenir.
        /// </summary>
        public event Action OnDoorOpened;

        /// <summary>
        /// Kapı kapandığında tetiklenir.
        /// </summary>
        public event Action OnDoorClosed;

        /// <summary>
        /// Kapı kilidi açıldığında tetiklenir.
        /// </summary>
        public event Action OnDoorUnlocked;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override string InteractionPrompt
        {
            get
            {
                if (m_IsLocked)
                {
                    return k_LockedPrompt;
                }
                return m_IsOpen ? k_ClosePrompt : k_OpenPrompt;
            }
        }

        /// <summary>
        /// Kapı açık mı?
        /// </summary>
        public bool IsOpen => m_IsOpen;

        /// <summary>
        /// Kapı kilitli mi?
        /// </summary>
        public bool IsLocked => m_IsLocked;

        /// <summary>
        /// Gerekli anahtar ID'si.
        /// </summary>
        public string RequiredKeyId => m_RequiredKeyId;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_DoorPivot == null)
            {
                m_DoorPivot = transform;
            }

            if(m_Animator == null)
                m_Animator = GetComponent<Animator>();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                Open();
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                Close();
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Eylem tetiklendiğinde çağrılır.
        /// </summary>
        /// <param name="interactor">Eylemi tetikleyen nesne.</param>
        public override void OnInteract(GameObject interactor)
        {
            if (m_IsLocked)
            {
                // Envanter kontrolü yap
                if (interactor != null && interactor.TryGetComponent<PlayerInventory>(out var inventory))
                {
                    if (inventory.HasKey(m_RequiredKeyId))
                    {
                        Unlock();
                        inventory.UseKey(m_RequiredKeyId);
                    }
                    else
                    {
                        Debug.Log($"Door is locked! Requires key: {m_RequiredKeyId}");
                        return;
                    }
                }
                else
                {
                    Debug.Log("Door is locked!");
                    return;
                }
            }

            ToggleDoor();
            base.OnInteract(interactor);
        }
        
        /// <summary>
        /// Kapıyı açar/kapatır.
        /// </summary>
        public void ToggleDoor()
        {
            m_IsOpen = !m_IsOpen;
            m_TargetAngle = m_IsOpen ? m_OpenAngle : 0f;

            if (m_IsOpen)
            {
                OnDoorOpened?.Invoke();
            }
            else
            {
                OnDoorClosed?.Invoke();
            }
        }

        /// <summary>
        /// Kapıyı açar.
        /// </summary>
        public void Open()
        {
            if (!m_IsOpen)
            {
                ToggleDoor();
            }
            
            OpenAnimation();
        }

        /// <summary>
        /// Kapıyı kapatır.
        /// </summary>
        public void Close()
        {
            if (m_IsOpen)
            {
                ToggleDoor();
            }
            CloseAnimation();
        }

        /// <summary>
        /// Kapı kilidini açar.
        /// </summary>
        public void Unlock()
        {
            m_IsLocked = false;
            OnDoorUnlocked?.Invoke();
            Debug.Log("Door unlocked!");
        }

        /// <summary>
        /// Kapıyı kilitler.
        /// </summary>
        /// <param name="keyId">Gerekli anahtar ID'si.</param>
        public void Lock(string keyId)
        {
            m_IsLocked = true;
            m_RequiredKeyId = keyId;
        }

        private void OpenAnimation()
        {
            m_Animator.SetBool("isOpen", true);
            m_Animator.SetBool("isClose", false);
        }
        
        private void CloseAnimation()
        {
            m_Animator.SetBool("isOpen", false);
            m_Animator.SetBool("isClose", true);
        }

        #endregion
    }
}