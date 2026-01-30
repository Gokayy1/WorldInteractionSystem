using UnityEngine;
using InteractionSystem.Runtime.Core;

namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Açılıp kapanabilen kapı. Animator component'i ile çalışır.
    /// Toggle Interaction tipindedir.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class Door : InteractableBase
    {
        #region Private Fields

        private Animator m_Animator;
        private bool m_IsOpen = false;
        
        private const string k_AnimParamOpen = "IsOpen";

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            
            // Başlangıç durumunu animator'a bildir
            if (m_Animator != null)
            {
                m_Animator.SetBool(k_AnimParamOpen, m_IsOpen);
            }
            else
            {
                Debug.LogError($"[{nameof(Door)}] Animator component bulunamadı!", this);
            }
        }

        #endregion

        #region Interactable Overrides

        public override bool OnInteract(GameObject interactor)
        {
            // Önce Base'deki kilit kontrolünü yap (Kilitliyse false döner)
            if (!base.OnInteract(interactor)) 
                return false;

            // Toggle işlemi (Açıksa kapat, kapalıysa aç)
            m_IsOpen = !m_IsOpen;
            
            // Animator güncelle
            if (m_Animator != null)
            {
                m_Animator.SetBool(k_AnimParamOpen, m_IsOpen);
            }

            // Prompt mesajını güncelle (İsteğe bağlı)
            // m_PromptMessage = m_IsOpen ? "Close Door" : "Open Door";

            return true; // Etkileşim başarılı
        }

        protected override string GetCurrentPrompt()
        {
            if (m_IsLocked) return "Locked";
            return m_IsOpen ? "Press 'E' to close" : m_PromptMessage; // Varsayılan prompt "Open" ise
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Kapının kilit durumunu değiştirir. (Anahtar bulunduğunda çağrılacak)
        /// </summary>
        public void SetLocked(bool isLocked)
        {
            m_IsLocked = isLocked;
        }

        #endregion
    }
}