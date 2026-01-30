# WorldInteractionSystem - Gökay Yener

## Kurulum
1. Unity Hub üzerinden projeyi açın.
   - **Unity Sürümü:** 6000.0.23f1
2. `Assets/Scenes/TestScene` sahnesini açın.
3. Proje, Unity'nin **New Input System** paketini kullanmaktadır. Eğer sorulursa "Yes" diyerek backend'i restart edin.

## Nasıl Test Edilir
Demo sahnesi (TestScene) tüm özellikleri içerecek şekilde hazırlanmıştır:

- **Hareket:** WASD
- **Bakış:** Mouse
- **Etkileşim:** 'E' Tuşu (Keyboard)
- **Test Senaryoları:** 
- Interaction'ların hepsi sırasıyla karşınıza çıkacaktır. 
- Kapılara bakarak 'E' tuşunu spamlayın.
- Kilitli kapıları henüz anahtarınız yokken açmaya çalışın.
- Switch Off (kırmızı) iken, bağlı olduğu kapıyı açın, sonrasında tekrar Switch'le etkileşim kurun.
- Chest açılırken raycast'i bozun veya 'E' tuşunu bırakın.
- En önemlisi ise, şüpheli görünen Switch'lere *kesinlikle* basmayın.

## Mimari Kararlar

**Neden bu yapı:** Mimarinin olabildiğince basit ve çok yönlü kullanışlı olması gerektiğini düşündüm, bu noktada Intractor ve Interactables olarak ikiye bölme fikri aklıma geldi. Bu yüzden oyuncunun yalnızca etkileşime girdiği objenin, kendi eventini tetiklemesi gerektiğine karar verdim. Oyuncuyu denklemin en başına yalnızca bir kez koyarak, geri kalanının kendi içinde, kod tekrarı olmadan, sorunsuz çalışmasını hedefledim. Raycast tercihim içinse sandıkların ve düğmelerin olduğu bir ortamda ince ayar yapılması gerektiğini düşündüm, menzilini de tek seferlik ayarlama seçeneği böyle bir sistemde en işe yarayacak yöntemdi, üstüne UI Feedback (Crosshair ve renkleri) eklemesi de benim design tarafımı cezbetti.
**Alternatifler neydi:** Intractor ve Intrectables olmasaydı, alışık olduğum Manager mimarisiyle deneyebilirdim fakat bu ileride daha çok şey eklemek istediğim zaman daha yönetilebilir olması karşılığında daha şişkin olurdu ve her objenin State'lerini kontrol etmek maliyeti arttırırdı. Raycast yerine CollisionTrigger kullanabilirdim fakat dar veya dolu ortamlarda bu yapıyı kullanmak, tek tek collisionları düzenlemeyi gerektirebileceğinden ve yanlışlıkla 2 farklı objeyle etkileşim kurma riski olduğundan bunu tercih etmedim.
**Trade-off'lar:**
- Raycast: Objelerin boyutu da artık tasarımın zorunlu bir parçası oluyor, çok küçük veya çok büyük Interactable'ları eklemeden önce Raycast sistemine uygun tasarlanmaları gerekebilir. Bu da bazı durumlarda yaratıcılıktan ve serbestlikten ödün vermek anlamına gelebilir. 
- UnityEvents (Switch): Inspector üzerinden kolayca Event atayabilmek güzel olsa da daha karmaşık Level-Design'lar içinde hiyerarşide tek tek "bu hangi kapıyı açıyor?" diye kontrol edilmesi gerekebilir.

## Ludu Arts Standartlarına Uyum
- Prefix standartlarına uydum.
- Regionlara ve kod mimarisi kurallarına uymaya çalıştım, sürem azaldıkça bu kontrol de azaldı.

- Zorlandığım noktalar:
1. Henüz ezberimde olmadığı için doğal olarak bir şey eklemeden önce gerekli kuralı kontrol etmem gerekiyordu, kendime özet tablo oluşturmama rağmen daha detaylı kuralları (örn: Prefab kuralları) arayıp bulmam gerekti.
2. Dokümantasyon hazırlarken neleri yazmam gerektiğine karar vermem, olması gerekenden uzun vakit alıyordu.

## Bilinen Limitasyonlar
- Tamamlayamadığım özellikler:
1. Işıklandırmak
2. Door.cs'i kapı olmayan objelerde de çalıştırmak
3. TestScene'i bir dungeon gibi tasarlamak
4. Gold'u kullanacak bir şeyler eklemek

- Bilinen bug'lar:
1. Kapı açılırken veya kapanırken 'E' tuşuna spamlamak, Bool'u değiştirdiği için istenmeyen etkileşimlere sebep oluyor.

- İyileştirme önerileri:
1. Switch'in bir süresi olabilir, basıldıktan sonra belli bir süre açık kalarak challenge ögesi olarak kullanılabilir.

## Ekstra Özellikler
- Gold (sayıların artma mutluluğu dışında, işlevsiz)
