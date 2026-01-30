using UnityEngine;

namespace InteractionSystem.Runtime.ScriptableObjects
{
    /// <summary>
    /// Anahtar tiplerini tanımlamak için kullanılır (Örn: Red Key, Blue Key).
    /// </summary>
    [CreateAssetMenu(fileName = "New Key Data", menuName = "InteractionSystem/Key Data")]
    public class KeyData : ScriptableObject
    {
        [Tooltip("Anahtarın görünen adı.")]
        public string KeyName;
    }
}