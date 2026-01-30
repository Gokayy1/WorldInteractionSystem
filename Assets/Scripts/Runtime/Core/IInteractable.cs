using UnityEngine;

namespace InteractionSystem.Runtime.Core
{
    /// <summary>
    /// Oyuncunun etkileşime geçebileceği tüm nesnelerin uygulaması gereken arayüz.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Oyuncuya gösterilecek etkileşim metni (Örn: "Press E to Open").
        /// </summary>
        string InteractionPrompt { get; }

        /// <summary>
        /// Basılı tutma süresi gerektiriyor mu? (0 = Instant/Toggle).
        /// </summary>
        float HoldDuration { get; }

        /// <summary>
        /// Nesne şu an etkileşime açık mı? (Örn: Kilitli mi, zaten açık mı?).
        /// </summary>
        bool IsInteractable { get; }

        /// <summary>
        /// Oyuncu nesneye Raycast ile baktığında çağrılır (UI açmak için).
        /// </summary>
        void OnFocus();

        /// <summary>
        /// Oyuncu nesneden bakışını çektiğinde çağrılır (UI kapatmak için).
        /// </summary>
        void OnLoseFocus();

        /// <summary>
        /// Etkileşim tuşuna basıldığında çağrılır.
        /// Hold etkileşimleri için bu süreç başlangıcıdır.
        /// </summary>
        /// <param name="interactor">Etkileşimi başlatan obje (Player).</param>
        /// <returns>Etkileşim başarılı/başladı ise true.</returns>
        bool OnInteract(GameObject interactor);

        /// <summary>
        /// Etkileşim tuşu bırakıldığında veya işlem iptal edildiğinde çağrılır.
        /// </summary>
        void OnInteractEnd();
    }
}