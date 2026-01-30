using System.Text;
using UnityEngine;
using TMPro; // TextMeshProUGUI kullanıyoruz
using InteractionSystem.Runtime.Player;
using InteractionSystem.Runtime.ScriptableObjects;

namespace InteractionSystem.Runtime.UI
{
    /// <summary>
    /// Envanter içeriğini ekrana basit bir liste olarak yazar.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Listeyi yazacağımız Text alanı.")]
        [SerializeField] private TextMeshProUGUI m_InventoryText;
        
        [Tooltip("Player üzerindeki Inventory bileşeni.")]
        [SerializeField] private SimpleInventory m_Inventory;

        private void Start()
        {
            // Referans kontrolü
            if (m_Inventory == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) m_Inventory = player.GetComponent<SimpleInventory>();
            }

            if (m_Inventory != null)
            {
                // Event'e abone ol
                m_Inventory.OnInventoryChanged += UpdateUI;
                UpdateUI(); // İlk durumu çiz
            }
            else
            {
                Debug.LogError("InventoryUI: SimpleInventory bulunamadı!");
            }
        }

        private void OnDestroy()
        {
            // Event aboneliğini kaldır (Memory leak önlemek için)
            if (m_Inventory != null)
            {
                m_Inventory.OnInventoryChanged -= UpdateUI;
            }
        }

        /// <summary>
        /// Envanter verilerini alıp string oluşturur ve ekrana basar.
        /// </summary>
        private void UpdateUI()
        {
            if (m_InventoryText == null) return;

            StringBuilder sb = new StringBuilder();

            // Altın Bilgisi
            sb.AppendLine($"Gold: {m_Inventory.Gold}");
            
            // Anahtar Listesi
            foreach (var kvp in m_Inventory.Keys)
            {
                sb.AppendLine($"{kvp.Key.KeyName}: {kvp.Value}");
            }

            m_InventoryText.text = sb.ToString();
        }
    }
}