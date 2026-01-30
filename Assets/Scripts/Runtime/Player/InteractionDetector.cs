using UnityEngine;
using UnityEngine.InputSystem;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Player
{
    /// <summary>
    /// Oyuncunun kamerasından Raycast atarak IInteractable nesneleri tespit eder.
    /// Input alır ve etkileşimi başlatır.
    /// </summary>
    public class InteractionDetector : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Detection Settings")]
        [Tooltip("Etkileşim mesafesi.")]
        [SerializeField] private float m_InteractionRange = 3.0f;

        [Tooltip("Raycast'in hangi layer'ları göreceği.")]
        [SerializeField] private LayerMask m_InteractableLayer;

        [Tooltip("Raycast'in çıkış noktası (Genellikle MainCamera).")]
        [SerializeField] private Transform m_RaySource;

        [Header("Input Settings")]
        [Tooltip("Etkileşim tuşu (Örn: E).")]
        [SerializeField] private InputActionReference m_InteractAction;

        #endregion

        #region Private Fields

        private IInteractable m_CurrentInteractable;
        private float m_HoldTimer = 0f;
        private bool m_IsHolding = false;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_RaySource == null)
            {
                m_RaySource = Camera.main != null ? Camera.main.transform : transform;
                Debug.LogWarning($"[{nameof(InteractionDetector)}] RaySource atanmamış, MainCamera kullanılıyor.", this);
            }
        }

        private void OnEnable()
        {
            if (m_InteractAction != null)
            {
                m_InteractAction.action.Enable();
                m_InteractAction.action.started += OnInteractStarted;
                m_InteractAction.action.canceled += OnInteractCanceled;
            }
        }

        private void OnDisable()
        {
            if (m_InteractAction != null)
            {
                m_InteractAction.action.started -= OnInteractStarted;
                m_InteractAction.action.canceled -= OnInteractCanceled;
                m_InteractAction.action.Disable();
            }
        }

        private void Update()
        {
            HandleDetection();
            HandleHoldInteraction();
        }

        #endregion

        #region Custom Methods

        /// <summary>
        /// Sürekli olarak ileriye Ray atar ve etkileşim nesnelerini arar.
        /// </summary>
        private void HandleDetection()
        {
            Ray ray = new Ray(m_RaySource.position, m_RaySource.forward);
            
            // Raycast atıyoruz
            if (Physics.Raycast(ray, out RaycastHit hit, m_InteractionRange, m_InteractableLayer))
            {
                // Çarptığımız obje IInteractable mı?
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    // Yeni bir nesneye mi baktık?
                    if (m_CurrentInteractable != interactable)
                    {
                        ClearCurrentInteractable(); // Eskiyi temizle
                        m_CurrentInteractable = interactable;
                        m_CurrentInteractable.OnFocus(); // Yeniye odaklan
                    }
                    return; // Bulduk, döngüden çık
                }
            }

            // Hiçbir şeye bakmıyorsak veya menzilden çıktıysak
            ClearCurrentInteractable();
        }

        /// <summary>
        /// Mevcut etkileşimi temizler (UI'ı gizler).
        /// </summary>
        private void ClearCurrentInteractable()
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnLoseFocus();
                m_CurrentInteractable = null;
                
                // Hold işlemini iptal et
                m_IsHolding = false;
                m_HoldTimer = 0f;
            }
        }

        /// <summary>
        /// Input tuşuna basıldığında çağrılır.
        /// </summary>
        private void OnInteractStarted(InputAction.CallbackContext context)
        {
            if (m_CurrentInteractable != null)
            {
                // Etkileşimi başlat
                bool started = m_CurrentInteractable.OnInteract(this.gameObject);
                
                if (started && m_CurrentInteractable.HoldDuration > 0f)
                {
                    m_IsHolding = true;
                    m_HoldTimer = 0f;
                }
            }
        }

        /// <summary>
        /// Input tuşu bırakıldığında çağrılır.
        /// </summary>
        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            if (m_CurrentInteractable != null)
            {
                m_IsHolding = false;
                m_HoldTimer = 0f;
                m_CurrentInteractable.OnInteractEnd();
            }
        }

        /// <summary>
        /// Basılı tutma mantığını (Progress) yönetir.
        /// </summary>
        private void HandleHoldInteraction()
        {
            if (m_IsHolding && m_CurrentInteractable != null)
            {
                m_HoldTimer += Time.deltaTime;

                float progress = m_HoldTimer / m_CurrentInteractable.HoldDuration;
                
                // TODO: UI Manager'a progress gönderilecek (Observer Pattern veya Event ile)
                // Debug.Log($"Hold Progress: {progress:P0}");

                if (m_HoldTimer >= m_CurrentInteractable.HoldDuration)
                {
                    // Süre doldu, işlem tamam
                    m_CurrentInteractable.OnInteract(this.gameObject); // Tekrar çağırarak işlemi tamamlatabiliriz veya ayrı bir method
                    m_IsHolding = false;
                    m_HoldTimer = 0f;
                }
            }
        }

        #endregion
    }
}