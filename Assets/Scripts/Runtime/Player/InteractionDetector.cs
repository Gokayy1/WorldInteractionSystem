using UnityEngine;
using UnityEngine.InputSystem;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Player
{
    public class InteractionDetector : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Detection Settings")]
        [SerializeField] private float m_InteractionRange = 3.0f;
        [SerializeField] private LayerMask m_InteractableLayer;
        [SerializeField] private Transform m_RaySource;

        [Header("Input Settings")]
        [SerializeField] private InputActionReference m_InteractAction;

        #endregion

        #region Private Fields

        private IInteractable m_CurrentInteractable;
        private float m_HoldTimer = 0f;
        private bool m_IsHolding = false;

        #endregion

        #region Public Properties

        public IInteractable CurrentInteractable => m_CurrentInteractable;
        public float HoldProgress => (m_CurrentInteractable != null && m_CurrentInteractable.HoldDuration > 0) 
            ? Mathf.Clamp01(m_HoldTimer / m_CurrentInteractable.HoldDuration) 
            : 0f;

        public bool IsHolding => m_IsHolding;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_RaySource == null)
                m_RaySource = Camera.main != null ? Camera.main.transform : transform;
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

        private void HandleDetection()
        {
            Ray ray = new Ray(m_RaySource.position, m_RaySource.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, m_InteractionRange, m_InteractableLayer))
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    if (m_CurrentInteractable != interactable)
                    {
                        ClearCurrentInteractable();
                        m_CurrentInteractable = interactable;
                        m_CurrentInteractable.OnFocus();
                    }
                    return;
                }
            }

            ClearCurrentInteractable();
        }

        private void ClearCurrentInteractable()
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnLoseFocus();
                m_CurrentInteractable = null;
                ResetHold();
            }
        }

        private void OnInteractStarted(InputAction.CallbackContext context)
        {
            if (m_CurrentInteractable == null) return;

            // Eğer HoldDuration 0 veya çok küçükse, anında etkileşim (Instant)
            if (m_CurrentInteractable.HoldDuration <= 0.1f)
            {
                m_CurrentInteractable.OnInteract(this.gameObject);
            }
            else
            {
                // Değilse, Hold işlemini başlat (Update takip edecek)
                m_IsHolding = true;
                m_HoldTimer = 0f;
            }
        }

        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            // Tuş bırakıldı, işlemi iptal et
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnInteractEnd();
            }
            ResetHold();
        }

        private void ResetHold()
        {
            m_IsHolding = false;
            m_HoldTimer = 0f;
        }

        private void HandleHoldInteraction()
        {
            // Sadece bir şeye bakıyorsak ve tuşa basılı tutuyorsak çalışır
            if (m_IsHolding && m_CurrentInteractable != null)
            {
                // Menzilden çıktı mı veya kilitlendi mi kontrolü (İsteğe bağlı)
                if (!m_CurrentInteractable.IsInteractable)
                {
                    ResetHold();
                    return;
                }

                m_HoldTimer += Time.deltaTime;

                // Süre doldu mu?
                if (m_HoldTimer >= m_CurrentInteractable.HoldDuration)
                {
                    // Etkileşimi tetikle
                    m_CurrentInteractable.OnInteract(this.gameObject);
                    
                    // İşlem bitti, tekrar tetiklenmemesi için resetle
                    ResetHold();
                }
            }
        }

        #endregion
    }
}