using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.UI
{
    public class InteractionUI : MonoBehaviour
    {
        #region Serialized Fields

        [Header("References")]
        [SerializeField] private InteractionDetector m_Detector;
        [SerializeField] private Image m_CrosshairImage;
        [SerializeField] private TextMeshProUGUI m_PromptText;

        [Tooltip("Hold işlemini gösteren Slider.")]
        [SerializeField] private Slider m_HoldSlider;

        [Header("Colors")]
        [SerializeField] private Color m_DefaultColor = new Color(1, 1, 1, 0.5f);
        [SerializeField] private Color m_InteractableColor = Color.green;
        [SerializeField] private Color m_LockedColor = Color.red;

        #endregion

        #region Unity Methods

        private void Start()
        {
            ValidateReferences();
            
            // Başlangıç temizliği
            if (m_PromptText) m_PromptText.text = "";
            if (m_HoldSlider) m_HoldSlider.gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateUI();
        }

        #endregion

        #region Custom Methods

        private void ValidateReferences()
        {
            if (m_Detector == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) m_Detector = player.GetComponent<InteractionDetector>();
            }
        }

        private void UpdateUI()
        {
            if (m_Detector == null) return;

            IInteractable target = m_Detector.CurrentInteractable;

            // 1. Crosshair ve Prompt Güncellemesi
            if (target == null)
            {
                if (m_CrosshairImage) m_CrosshairImage.color = m_DefaultColor;
                if (m_PromptText) m_PromptText.text = "";
            }
            else
            {
                if (m_PromptText) m_PromptText.text = target.InteractionPrompt;

                if (m_CrosshairImage)
                {
                    m_CrosshairImage.color = target.IsInteractable ? m_InteractableColor : m_LockedColor;
                }
            }

            // 2. Hold Progress Slider Güncellemesi
            if (m_HoldSlider != null)
            {
                // Slider ne zaman görünmeli?
                // - Bir hedef varsa
                // - Hedefin Hold süresi varsa (>0)
                // - Oyuncu tuşa basıyorsa (IsHolding)
                
                bool showSlider = target != null && 
                                  target.HoldDuration > 0 && 
                                  m_Detector.IsHolding &&
                                  target.IsInteractable; // Kilitliyse bar dolmaz

                if (showSlider)
                {
                    if (!m_HoldSlider.gameObject.activeSelf) 
                        m_HoldSlider.gameObject.SetActive(true);

                    m_HoldSlider.value = m_Detector.HoldProgress;
                }
                else
                {
                    if (m_HoldSlider.gameObject.activeSelf) 
                        m_HoldSlider.gameObject.SetActive(false);
                }
            }
        }

        #endregion
    }
}