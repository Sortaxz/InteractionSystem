using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InteractionSystem.Runtime.Core;
using InteractionSystem.Runtime.Player;

namespace InteractionSystem.Runtime.UI
{
    /// <summary>
    /// Etkileşim prompt UI yönetimi.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private InteractionDetector m_Detector;
        [SerializeField] private GameObject m_PromptPanel;
        [SerializeField] private TextMeshProUGUI m_PromptText;
        [SerializeField] private Image m_ProgressBar;
        [SerializeField] private GameObject m_ProgressBarContainer;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (m_Detector != null)
            {
                m_Detector.OnInteractableFocused += HandleInteractableFocused;
                m_Detector.OnInteractableLostFocus += HandleInteractableLostFocus;
                m_Detector.OnHoldProgressChanged += HandleHoldProgressChanged;
            }
        }

        private void OnDisable()
        {
            if (m_Detector != null)
            {
                m_Detector.OnInteractableFocused -= HandleInteractableFocused;
                m_Detector.OnInteractableLostFocus -= HandleInteractableLostFocus;
                m_Detector.OnHoldProgressChanged -= HandleHoldProgressChanged;
            }
        }

        private void Start()
        {
            HidePrompt();
        }

        private void Update()
        {
            // Prompt text'i dinamik güncelle
            if (m_PromptPanel.activeSelf && m_Detector.CurrentInteractable != null)
            {
                UpdatePromptText(m_Detector.CurrentInteractable);
            }
        }

        #endregion

        #region Methods

        private void HandleInteractableFocused(IInteractable interactable)
        {
            ShowPrompt(interactable);
        }

        private void HandleInteractableLostFocus()
        {
            HidePrompt();
        }

        private void HandleHoldProgressChanged(float progress)
        {
            if (m_ProgressBar != null)
            {
                m_ProgressBar.fillAmount = progress;
            }

            if (m_ProgressBarContainer != null)
            {
                m_ProgressBarContainer.SetActive(progress > 0);
            }
        }

        private void ShowPrompt(IInteractable interactable)
        {
            if (m_PromptPanel != null)
            {
                m_PromptPanel.SetActive(true);
            }

            UpdatePromptText(interactable);

            // Hold tipi için progress bar göster
            bool isHoldType = interactable.InteractionType == InteractionType.Hold;
            if (m_ProgressBarContainer != null)
            {
                m_ProgressBarContainer.SetActive(isHoldType);
            }

            if (m_ProgressBar != null)
            {
                m_ProgressBar.fillAmount = 0;
            }
        }

        private void UpdatePromptText(IInteractable interactable)
        {
            if (m_PromptText != null)
            {
                m_PromptText.text = interactable.InteractionPrompt;
            }
        }

        private void HidePrompt()
        {
            if (m_PromptPanel != null)
            {
                m_PromptPanel.SetActive(false);
            }

            if (m_ProgressBarContainer != null)
            {
                m_ProgressBarContainer.SetActive(false);
            }
        }

        #endregion
    }
}