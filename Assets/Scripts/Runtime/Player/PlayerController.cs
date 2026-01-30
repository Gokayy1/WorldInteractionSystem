using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Oyuncunun hareketini ve kamera kontrolünü yönetir.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Movement Settings")]
        [Tooltip("Karakterin yürüme hızı.")]
        [SerializeField] private float m_MoveSpeed = 5.0f;

        [Tooltip("Yerçekimi kuvveti.")]
        [SerializeField] private float m_Gravity = -15.0f;

        [Header("Look Settings")]
        [Tooltip("Mouse hassasiyeti.")]
        [SerializeField] private float m_LookSensitivity = 1.0f;

        [Tooltip("Kameranın yukarı/aşağı bakma limiti.")]
        [SerializeField] private float m_LookXLimit = 85.0f;

        [Tooltip("Kameranın bağlı olduğu obje (CameraHolder veya direkt Camera).")]
        [SerializeField] private Transform m_CameraTransform;

        [Header("Input References")]
        [Tooltip("Hareket Input Action (WASD).")]
        [SerializeField] private InputActionReference m_MoveAction;

        [Tooltip("Bakış Input Action (Mouse Delta).")]
        [SerializeField] private InputActionReference m_LookAction;

        #endregion

        #region Private Fields

        private CharacterController m_CharacterController;
        private Vector2 m_InputMovement;
        private Vector2 m_InputLook;
        private float m_RotationX = 0f;
        private Vector3 m_Velocity; // Dikey hız (yerçekimi için)

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            
            ValidateReferences();
            LockCursor();
        }

        private void OnEnable()
        {
            if (m_MoveAction != null) m_MoveAction.action.Enable();
            if (m_LookAction != null) m_LookAction.action.Enable();
        }

        private void OnDisable()
        {
            if (m_MoveAction != null) m_MoveAction.action.Disable();
            if (m_LookAction != null) m_LookAction.action.Disable();
        }

        private void Update()
        {
            ReadInput();
            HandleBodyRotation(); // Gövdeyi sağa sola döndür
            HandleMovement();     // Gövdeyi yürüt
        }

        // Kamera rotasyonunu fizikten sonra yaparak titremeyi önlüyoruz
        private void LateUpdate()
        {
            HandleCameraRotation();
        }

        #endregion

        #region Custom Methods

        private void ValidateReferences()
        {
            if (m_CameraTransform == null)
            {
                Debug.LogError($"[{nameof(PlayerController)}] Camera Transform atanmamış! Lütfen Inspector'dan atayın.", this);
                enabled = false;
            }
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ReadInput()
        {
            m_InputMovement = m_MoveAction.action.ReadValue<Vector2>();
            m_InputLook = m_LookAction.action.ReadValue<Vector2>();
        }

        /// <summary>
        /// Karakterin gövdesini (Y ekseni) mouse hareketine göre döndürür.
        /// </summary>
        private void HandleBodyRotation()
        {
            float mouseX = m_InputLook.x * m_LookSensitivity * Time.deltaTime * 10f; // Delta time ile frame bağımsızlığı
            transform.Rotate(Vector3.up * mouseX);
        }

        /// <summary>
        /// Kamerayı (X ekseni) yukarı aşağı döndürür. LateUpdate'de çağrılır.
        /// </summary>
        private void HandleCameraRotation()
        {
            float mouseY = m_InputLook.y * m_LookSensitivity * Time.deltaTime * 10f;

            m_RotationX -= mouseY;
            m_RotationX = Mathf.Clamp(m_RotationX, -m_LookXLimit, m_LookXLimit);

            // Sadece kamerayı (veya holder'ı) döndür, karakterin kendisini değil
            m_CameraTransform.localRotation = Quaternion.Euler(m_RotationX, 0f, 0f);
        }

        /// <summary>
        /// Karakterin yürüme ve yerçekimi işlemlerini yapar.
        /// </summary>
        private void HandleMovement()
        {
            // Hareket yönü (Karakterin baktığı yöne göre)
            // Y eksenindeki hareketi sıfırlıyoruz ki havaya bakarken uçmasın
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            // Normalizasyon önemli (Çapraz giderken hızlanmayı önler)
            Vector3 moveDirection = (forward * m_InputMovement.y + right * m_InputMovement.x).normalized;

            // Yerçekimi Kontrolü
            if (m_CharacterController.isGrounded && m_Velocity.y < 0)
            {
                m_Velocity.y = -2f; // Yere yapışık kalması için
            }

            // Hareketi uygula
            m_CharacterController.Move(moveDirection * m_MoveSpeed * Time.deltaTime);

            // Yerçekimi uygula
            m_Velocity.y += m_Gravity * Time.deltaTime;
            m_CharacterController.Move(m_Velocity * Time.deltaTime);
        }

        #endregion
    }
}