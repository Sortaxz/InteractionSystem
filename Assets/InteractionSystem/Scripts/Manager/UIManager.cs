
using InteractionSystem.Runtime.UI;
using UnityEngine;

namespace InteractionSystem.Scripts.Manager
{
    /// <summary>
    /// Oyun yönetim sistemi.
    /// </summary>
    /// 
    public class UIManager : MonoBehaviour
    {
        private static UIManager s_Instance;
        internal static UIManager Instance
        {
            get
            {
                return s_Instance;
            }
        }

        #region  Fields
        [Header("Fields")]
        [SerializeField] private InteractionPromptUI m_InteractionPromptUI;
        internal InteractionPromptUI InteractionPromptUI => m_InteractionPromptUI;
        
        #endregion

        #region Unity Methods

        void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            s_Instance = this;
        }
        #endregion


        internal void InteractionPromptUISetActive(bool value)
        {
            m_InteractionPromptUI.gameObject.SetActive(value);
        }

    }
}