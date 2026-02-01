using System;
using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Tüm interactable nesneler için base class.
    /// Base class for all interactable objects.
    /// </summary>
    public abstract class BaseInteractable : MonoBehaviour, IInteractable
    {
        #region Fields

        [Header("Interaction Settings")]
        [SerializeField] private string m_InteractionPrompt = "Press E to Interact";
        [SerializeField] private InteractionType m_InteractionType = InteractionType.Instant;
        [SerializeField] private float m_HoldDuration = 2f;

        private bool m_CanInteract = true;
        private bool m_IsFocused;

        #endregion

        #region Events

        /// <summary>
        /// It triggers when interaction occurs.(Etkileşim gerçekleştiğinde tetiklenir.)
        /// </summary>
        public event Action OnInteracted;

        #endregion

        #region Properties

        /// <summary>
        /// Oyuncuya gösterilecek etkileşim mesajını döner.  Örn: "Kapıyı açmak için E'ye bas".  Returns the interaction message shown to the player.  Example: "Press E to open the door".
        /// </summary>
        public virtual string InteractionPrompt => m_InteractionPrompt;

        /// <summary> 
        /// Nesneyle etkileşim kurulabilir mi?  False ise etkileşim engellenir.  Determines if the object can be interacted with.  If false, interaction is blocked.
        /// </summary>
        public virtual bool CanInteract => m_CanInteract;

        /// <summary> 
        /// Nesnenin etkileşim türünü belirtir.  Örn: Tek tık, basılı tutma, otomatik.  Specifies the type of interaction for the object.  Example: Single click, hold, automatic.
        /// </summary>
        public InteractionType InteractionType => m_InteractionType;

        /// <summary>
        /// Basılı tutma gerektiren etkileşimlerde  ne kadar süre basılı tutulması gerektiğini döner.  Returns the required hold duration for  interactions that need button holding. 
        /// </summary>
        public float HoldDuration => m_HoldDuration;

        /// <summary>
        /// Nesne şu anda oyuncunun odak noktasında mı?  Kamera veya raycast ile seçilmişse true döner.  Indicates if the object is currently focused by the player.  True if selected via camera or raycast.
        /// </summary>
        public bool IsFocused => m_IsFocused;

        #endregion

        #region Methods

        /// <summary> 
        /// Nesneyle etkileşim başlatır.  CanInteract false ise uyarı logu basar.  Initiates interaction with the object.  Logs a warning if CanInteract is false.
        /// </summary>
        public virtual void Interact()
        {
            if (!CanInteract)
            {
                Debug.LogWarning($"Cannot interact with {gameObject.name}");
                return;
            }

            OnInteract(null);
        }

        /// <summary> 
        /// Etkileşim gerçekleştiğinde çağrılır. İlgili event tetiklenir. Called when interaction occurs. Invokes the corresponding event.
        /// </summary>
        public virtual void OnInteract(GameObject interactor)
        {
            OnInteracted?.Invoke();
        }

        /// <summary> 
        /// Nesne oyuncunun odak noktasına girdiğinde çağrılır. Focus durumunu aktif hale getirir. Called when the object gains focus from the player. Sets focus state to active.
        ///  </summary>
        public virtual void OnFocus()
        {
            m_IsFocused = true;
        }

        /// <summary> 
        /// Nesne oyuncunun odak noktasından çıktığında çağrılır. Focus durumunu pasif hale getirir. Called when the object loses focus from the player. Sets focus state to inactive. 
        /// </summary>
        public virtual void OnLoseFocus()
        {
            m_IsFocused = false;
        }

        /// <summary>
        /// Etkileşim durumunu ayarlar.
        ///  Sets the interaction status.
        /// </summary>
        /// <param name="canInteract">Etkileşime açık mı?</param>
        protected void SetCanInteract(bool canInteract)
        {
            m_CanInteract = canInteract;
        }

        /// <summary>
        /// Etkileşim mesajını ayarlar.
        /// Sets the interaction message.
        /// </summary>
        /// <param name="prompt">Yeni mesaj.</param>
        protected void SetInteractionPrompt(string prompt)
        {
            m_InteractionPrompt = prompt;
        }

        #endregion
    }
}