using System;
using InteractionSystem.Runtime.Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_Instance;
    internal static GameManager Instance
    {
        get
        {
            return s_Instance;
        }
    }

    #region Events
    internal event Action OnGameStop;
    internal event Action OnGameResume;
    #endregion

    #region  Fields

    [Header("Fields")]

    [SerializeField] private PlayerController m_Player;
    internal PlayerController Player
    {
        get { return m_Player; }
    }

    [SerializeField] private bool m_IsStop;
    internal bool IsStop
    {
        get { return m_IsStop; }
        set { m_IsStop = value; }
    }


    [SerializeField] private bool m_IsGameOver;
    internal bool IsGameOver
    {
        get { return m_IsGameOver; }
        set { m_IsGameOver = value; }
    }

    #endregion

    #region  Unity Methods

    void Awake()
    {
        if(s_Instance != null && s_Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        s_Instance = this;
        DontDestroyOnLoad(this.gameObject);

        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    #endregion
    
    #region  Methods

    internal void SendGameStop()
    {
        m_IsStop = true;
        OnGameStop?.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    internal void SendGameResume()
    {
        m_IsStop = false;
        OnGameResume?.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion
    
}
