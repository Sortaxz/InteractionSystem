using InteractionSystem.Scripts.Manager;
using UnityEngine;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Basit oyuncu hareketi.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        [Header("Movement")]
        [SerializeField] private float m_MoveSpeed = 5f;
        [SerializeField] private float m_LookSpeed = 2f;

        private CharacterController m_CharacterController;
        private float m_RotationX;
        private bool m_IsStop;
        internal bool IsStop
        {
            get { return m_IsStop; }
            set { m_IsStop = value; }
        }

        #endregion

        #region Unity Methods

        void OnEnable()
        {
            GameManager.Instance.OnGameStop += HandleGamePause;
            GameManager.Instance.OnGameResume += HandleGameResume;
        }

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if(m_IsStop)
                return;

            HandleMovement();
            HandleLook();
        }

        void OnDisable()
        {
            
        }

        #endregion

        #region Methods

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            m_CharacterController.Move(move * m_MoveSpeed * Time.deltaTime);
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * m_LookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * m_LookSpeed;

            m_RotationX -= mouseY;
            m_RotationX = Mathf.Clamp(m_RotationX, -90f, 90f);

            Camera.main.transform.localRotation = Quaternion.Euler(m_RotationX, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleGamePause()
        {
            m_IsStop = true;
        }
        private void HandleGameResume()
        {
            m_IsStop = false;
        }

        #endregion
    }
}
