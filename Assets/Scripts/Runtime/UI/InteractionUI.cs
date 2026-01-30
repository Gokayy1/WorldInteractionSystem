using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullanımı için (Standart Text kullanıyorsanız UnityEngine.UI yeterli)
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.UI
{
    /// <summary>
    /// Oyuncunun etkileşim durumuna göre Crosshair rengini ve Prompt metnini yönetir.
    /// </summary>
    public class InteractionUI : MonoBehaviour
    {
        #region Serialized Fields

        [Header("References")]
        [SerializeField] private InteractionDetector m_Detector;
        [SerializeField] private Image m_CrosshairImage;
        
        [Tooltip("Etkileşim mesajının yazılacağı Text bileşeni.")]
        [SerializeField] private TextMeshProUGUI m_PromptText; // Standart Text ise 'Text' yapın.

        [Header("Colors")]
        [SerializeField] private Color m_DefaultColor = new Color(1, 1, 1, 0.5f); // Yarı saydam beyaz
        [SerializeField] private Color m_InteractableColor = Color.green;
        [SerializeField] private Color m_LockedColor = Color.red;

        #endregion

        #region Unity Methods

        private void Start()
        {
            ValidateReferences();
            // Başlangıçta yazıyı gizle
            if (m_PromptText != null) m_PromptText.text = "";
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

            if (target == null)
            {
                // Boşluğa bakıyoruz
                if (m_CrosshairImage) m_CrosshairImage.color = m_DefaultColor;
                if (m_PromptText) m_PromptText.text = ""; // Yazıyı temizle
            }
            else
            {
                // Bir şeye bakıyoruz, Prompt mesajını al
                if (m_PromptText) m_PromptText.text = target.InteractionPrompt;

                // Rengi ayarla
                if (target.IsInteractable)
                {
                    if (m_CrosshairImage) m_CrosshairImage.color = m_InteractableColor;
                }
                else
                {
                    if (m_CrosshairImage) m_CrosshairImage.color = m_LockedColor;
                }
            }
        }

        #endregion
    }
}