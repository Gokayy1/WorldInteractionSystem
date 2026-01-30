using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Tüm etkileşimli nesneler için temel sınıf.
    /// Ortak özellikleri ve IInteractable implementasyonunu barındırır.
    /// </summary>
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Serialized Fields

        [Header("Interaction Settings")]
        [Tooltip("Etkileşim sırasında gösterilecek metin.")]
        [SerializeField] protected string m_PromptMessage = "Interact";

        [Tooltip("Basılı tutma süresi (0 = Anlık/Toggle).")]
        [SerializeField] protected float m_HoldDuration = 0f;

        [Tooltip("Nesne başlangıçta kilitli mi?")]
        [SerializeField] protected bool m_IsLocked = false;

        #endregion

        #region Public Properties

        // Interface implementasyonları
        public string InteractionPrompt => GetCurrentPrompt();
        public float HoldDuration => m_HoldDuration;
        public bool IsInteractable => !m_IsLocked;

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Nesneye bakıldığında çalışır.
        /// (İleride Outline shader'ı burada açabiliriz)
        /// </summary>
        public virtual void OnFocus()
        {
            // Debug için basit feedback, ileride silebiliriz.
            // Debug.Log($"Focus: {gameObject.name}");
            
            // TODO: Event gönder -> UI Manager prompt göstersin
        }

        /// <summary>
        /// Bakış kesildiğinde çalışır.
        /// </summary>
        public virtual void OnLoseFocus()
        {
            // Debug.Log($"Lost Focus: {gameObject.name}");
            // TODO: Event gönder -> UI Manager prompt gizlesin
        }

        /// <summary>
        /// Etkileşim isteği geldiğinde çalışır.
        /// Base implementasyon kilit kontrolü yapar.
        /// </summary>
        public virtual bool OnInteract(GameObject interactor)
        {
            if (m_IsLocked)
            {
                // Kilitliyse işlem yapma (veya "Locked" mesajı göster)
                Debug.Log("Object is Locked!");
                return false;
            }

            // Child class'lar burada kendi mantığını (base.OnInteract çağırarak) işletecek.
            return true;
        }

        /// <summary>
        /// Etkileşim sonlandığında (Hold bırakıldığında) çalışır.
        /// </summary>
        public virtual void OnInteractEnd()
        {
            // Genellikle boş kalabilir, özel durumlar için override edilir.
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Duruma göre dinamik prompt metni döndürebilir.
        /// (Örn: "Open" -> "Close")
        /// </summary>
        protected virtual string GetCurrentPrompt()
        {
            return m_IsLocked ? "Locked" : m_PromptMessage;
        }

        #endregion
    }
}