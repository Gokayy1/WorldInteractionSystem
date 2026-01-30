using UnityEngine;
using UnityEngine.Events;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    public class Switch : InteractableBase
    {
        #region Serialized Fields

        [Header("Switch Visuals")]
        [SerializeField] private Renderer m_Renderer;
        [SerializeField] private Color m_OffColor = Color.red;
        [SerializeField] private Color m_OnColor = Color.green;
        [SerializeField] private bool m_IsOneWay = false;

        [Header("Switch Events")]
        [SerializeField] private UnityEvent<bool> m_OnSwitchToggled;

        #endregion

        #region Private Fields

        private bool m_IsOn = false;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (m_Renderer == null) m_Renderer = GetComponent<Renderer>();
            UpdateVisuals();
        }

        #endregion

        #region Interactable Overrides

        public override bool OnInteract(GameObject interactor)
        {
            // Base kontrolü (Kilitli mi?)
            if (!base.OnInteract(interactor)) return false;

            // Durumu değiştir
            m_IsOn = !m_IsOn;

            // Görseli güncelle
            UpdateVisuals();

            // Event'i fırlat
            m_OnSwitchToggled?.Invoke(m_IsOn);
            
            Debug.Log($"Switch Toggled: {m_IsOn}");

            // --- YENİ EKLENEN KISIM ---
            // Eğer Tek Kullanımlık ise, işlemden sonra nesneyi kilitle
            if (m_IsOneWay && m_IsOn)
            {
                m_IsLocked = true; // Base class'taki kilit değişkeni
                // İsteğe bağlı: Prompt mesajını değiştir ki oyuncu bozulduğunu anlasın
                // "Locked" yerine "Jammed" (Sıkışmış) gibi görünecek artık (GetCurrentPrompt sayesinde)
            }
            // ---------------------------

            return true;
        }

        protected override string GetCurrentPrompt()
        {
            // Eğer OneWay yüzünden kilitlendiyse özel mesaj göster
            if (m_IsOneWay && m_IsLocked) return "It's Stuck";

            // Normal kilitliyse (Base kilit)
            if (m_IsLocked) return "Locked";

            return m_IsOn ? "Turn Off" : "Turn On";
        }

        #endregion

        #region Custom Methods

        private void UpdateVisuals()
        {
            if (m_Renderer != null)
            {
                m_Renderer.material.color = m_IsOn ? m_OnColor : m_OffColor;
            }
        }

        #endregion
    }
}