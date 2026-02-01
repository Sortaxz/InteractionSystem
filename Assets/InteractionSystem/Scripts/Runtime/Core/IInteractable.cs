using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileşime açık nesneler için temel interface.
    /// Basic interface for interactive objects.
    /// </summary>
    public interface IInteractable
    {
        #region Properties

        /// <summary>
        /// Etkileşim mesajı (örn: "Press E to Open").
        /// Interaction message (e.g., “Press E to Open”).
        /// </summary>
        string InteractionPrompt { get; }

        /// <summary>
        /// Nesne ile etkileşime geçilebilir mi?
        /// Can the object be interacted with?
        /// </summary>
        bool CanInteract { get; }

        /// <summary>
        /// Etkileşim tipi.
        /// </summary>
        InteractionType InteractionType { get; }

        /// <summary>
        /// Hold tipi için gerekli süre (saniye).
        /// Required duration for the hold type (seconds).
        /// </summary>
        float HoldDuration { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Etkileşimi başlatır.
        /// Initiates interaction.
        /// </summary>
        void Interact();

        /// <summary>
        /// Etkileşim için callback.
        /// Callback for interaction.
        /// </summary>
        /// <param name="interactor">Etkileşimi yapan GameObject.(The GameObject performing the interaction.)</param>
        void OnInteract(GameObject interactor);

        /// <summary>
        /// Oyuncu etkileşim alanına girdiğinde çağrılır.
        /// It is called when the player enters the interaction area.
        /// </summary>
        void OnFocus();

        /// <summary>
        /// Oyuncu etkileşim alanından çıktığında çağrılır.
        /// It is called when the player leaves the interaction area.
        /// </summary>
        void OnLoseFocus();

        #endregion
    }
}