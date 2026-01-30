# LLM Kullanım Dokümantasyonu

## Özet
- Toplam prompt sayısı: 19
- Kullanılan araçlar: Gemini
- En çok yardım alınan konular: [liste]

## Prompt 1: [Tablo Oluşturma]

**Araç:** GoogleDrive/Gemini
**Tarih/Saat:** 2026-01-30 , 15:06

**Prompt:**
> [Convention dosyaları seçili] bu dosyalardaki mimari kurallarını, tek bir tablo haline getir ve biçimlendir.

**Alınan Cevap (Özet):**
> [Excel sheet olarak export] C# İSİMLENDİRME , C# YAPISI VE STİL , ASSET İSİMLENDİRME başlıklarıyla isimlendirme kurallarını tablo halinde göstermiş.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Ezberlemesi bu aşamada yavaşlatağı ve tek tek dosyalarda gezip vakit kaybetmemek için kuralları tek bir tabloda toplamak, sadece orayı kontrol etmek istedim.

## Prompt 2: [Zemin Oluşturma]

**Araç:** GoogleAIStudio/Gemini 3 Pro
**Tarih/Saat:** 2026-01-30 , 15:58

**Prompt:**
> [Daha önce oluşturduğum tabloyu da gönderdim] Bu bir Unity 6 (6000.0.23f1) projesidir.
Mimari kararları kendim vereceğim.
Seni esas olarak inceleme ve yeniden düzenleme için kullanacağım.
Tam scriptleri oluşturmadan önce izin iste.
TODO yorumları içeren minimal iskeletleri tercih ederim. 

**Alınan Cevap (Özet):**
> 
Anlaşıldı. Unity 6 (6000.0.23f1) sürümü ve belirlediğiniz çalışma prensipleri (önce inceleme/iskelet, izinle tam kod, adım adım ilerleme) çerçevesinde hazırım.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Başlangıç için zemin oluşturmak, ileride LLM ile daha az çelişeceğimiz ortak bir alan tanımlamak istedim. Prompt 1'de oluşturduğum tabloyu da göndererek mimariye uygun tasarlamasını sağladım.

## Prompt 3: [TestGround için basit PlayerController]

**Araç:** GoogleAIStudio/Gemini 3 Pro
**Tarih/Saat:** 2026-01-30 , 16:10

**Prompt:**
>  ilk olarak TestGround için hareket edebilir bir Player'a ihtiyacımız var. InputSystem kullanarak hareketi sağlayacağımız bir PlayerController.cs yazalım. PlayerMesh bir capsule, capsulecollider kullanıyorum. İleride Raycast de ekleyeceğiz. işte Hiyerarşi:
PlayerRoot
-CapsuleMesh
-CameraRoot
--MainCamera

**Alınan Cevap (Özet):**
> PlayerController.cs dosyasının iskeletini hazırladı, TODO'lar ile ne ekleyebileceğini gösterdi.


**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> İskeleti inceledikten sonra onayladım ve bana PlayerController.cs'in dolu halini verdi.

## Prompt 4: [Interactable Mimarisi Tanıtımı]

**Araç:** GoogleAIStudio/Gemini 3 Pro
**Tarih/Saat:** 2026-01-30 , 16:47

**Prompt:**
>  İstediğim mimaride, Interactor = Player, Interactables = Door, Lever, Chest, Button vs. Her Interactable, Player'ı dinlemeli, Player'dan gelecek Invoke'larla koşulları da sağlıyorsa (Örn: Kapı, kilitli ise açılmaz , Kapı, çok uzakta değil) kendi event'ini tetiklemeli.
Door/Light switch = toggleInteraction, Stateleri: Kapalı/Açık
Chest = holdInteraction, Stateleri: Kapalı/Açılıyor/Açık, "Açılıyor" tamamlanmaz ise "Kapalı"ya geri dönüş.
Key = instantInteraction, Raycast koşulları sağlandığı sırada "E" tuşuna basılarak envantere alınacak.
Özet: Interactables oyuncuyu dinleyecek, raycast edilmiş ve koşulları sağlanmış, oyuncuya en yakın Interactable, event'ini tetikleyecek.

**Alınan Cevap (Özet):**
> Anladım. Publisher-Subscriber mantığına yakın, ancak Raycast ile aktif olarak seçilen nesnenin tetiklendiği bir yapı istiyorsunuz.

Sistem şu şekilde işleyecek:
IInteractable Interface: Tüm etkileşimli nesnelerin ortak sözleşmesi.
InteractionDetector (Player'da): Raycast atar, IInteractable bulur ve "E"ye basılınca (veya basılı tutulunca) bu interface üzerindeki metodları çağırır.
BaseInteractable (Abstract Class): Ortak özellikleri (mesafe kontrolü, prompt text vb.) tutar.
Concrete Classes: Door, Chest vb. kendi özel mantıklarını (StateMachine gibi) yönetir.

Bir de iskeleti verdi ama burada asıl olarak sistemi ne kadar anladığını kontrol ettim. 



**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Gitmek istediğim yer tam olarak Publisher-Subscriber noktasıydı, bu sistemi bir kez anladığında bir daha açıklama yapmam gerekmediğini düşündüm ve iyice anlamasını sağlamak için bir sonraki promptta:
"
3.1 Door (Kapı)
Açılıp kapanabilen kapı
Locked/Unlocked state
Kilitli ise "Anahtar gerekli" mesajı
Toggle interaction type
3.2 Key Pickup (Anahtar)
Instant interaction ile toplanabilir
Basit envantere eklenmeli
Farklı kapılar için farklı anahtarlar (en az 2 tip)
3.3 Switch/Lever (Anahtar/Kol)
Toggle interaction
Başka bir nesneyi tetikleyebilmeli (örn: kapıyı açar)
Event-based connection
3.4 Chest/Container (Sandık)
Hold interaction ile açılır (örn: 2 saniye basılı tut)
İçinde item bulunabilir
Açıldıktan sonra tekrar açılamaz
"
doğrudan case-document dosyasından bu bölümü gönderdim, iskeleti kontrol edip onayladım.

## Prompt 5: [Door ile Key Mantığı]

**Araç:** Gemini
**Tarih/Saat:** 2026-01-30 , 17:50

**Prompt:**
> Key ile devam edelim çünkü Door için önemli. Eğer Door, isLocked ise, Inspector'da bir bölüm olsun ve hangi tip anahtar ile açacağını seçeyim, Örn: kırmızı veya mavi
Key mantığını da ona göre yapalım, Red Key, Red Door'ları, Blue Key, Blue Door'ları açabilsin sadece.

**Alınan Cevap (Özet):**
> KeyData, SimpleInventory, KeyPickup classlarının iskeletini verdi. Modüler olmasını istediğim için ScriptableObject tabanlı bir mimari önerdi. Door.cs'i verdiğim anahtar mantığına göre düzenledi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim
- (Bu aşamada sadece Door.cs düzenlemesini aldığım için burayı boş bırakıyorum.)

**Açıklama:**
> Bu aşamada Door.cs düzenlemesini yaptıktan sonra 'Door' animasyona döndüm ve Gemini ile animasyon için Bool veya Trigger arasındaki farkları değerlendirdim. Gemini, Trigger'ın bazı durumlarda (spam vs.) sorun çıkartmaya daha müsait olduğunu söyledi, Bool ile devam ettim. Animasyonu ayarladıktan sonra test aşamasında kapının açılmadığını fark ettim. Sorunun kaynağını bulabilmek için InteractionUI.cs'i eklettim ve bu sayede Raycast'in kapıya hiç çarpmadığını gördüm. Koda döndüm ve Raycast'i içeren InteractionDetector.cs'teki 
bu satırı: 
"IInteractable interactable = hit.collider.GetComponent<IInteractable>();"

buna:
"IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();"

dönüştürerek sorunu çözdüm. Gemini, P_Door prefab'ının hiyerarşisinden habersizdi ve bu hatanın kaynağının bu olduğunu düşünüyorum.

## Prompt 6: [Key Mantığına Geçiş ve UI Feedback]

**Araç:** Gemini
**Tarih/Saat:** 2026-01-30 , 20:12

**Prompt:**
> Key mantığına devam edebiliriz, bu sırada da raycast'in çarptığı durumlarda Canvas'daki boş bir Text kutusuna, kapalı bir kapıya bakılıyorsa "Press 'E' to Open" kilitli bir kapıya bakılıyorsa "Locked" yerdeki anahtara bakıyorsa "Press 'E' to Collect" yazmayı ayarlayalım, PromptText mantığını zaten eklemiştik. Sonrasında bunu Chest için de yapacağız.

**Alınan Cevap (Özet):**
> KeyData, SimpleInventory, KeyPickup classlarının iskeletlerini onayladığımı varsayarak önceki iskeletleri doldurup gönderdi. InteractionUI.cs'e TMP eklemesi yaparak ekranda çıkacak PromptText'i yazdıracağım düzenlemeyi yaptı. Getter kullanarak bakılan objenin ne olduğuna göre TMP'yi düzenledi. 

**Ekstra Not:** Tam burada farkına vardım ki "SimpleInventory" ve "KeyPickup" classlarını rastgele değil, Prompt 4'te verdiğim bilgilere göre isimlendirmiş.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Bu tip UI'ların daha karmaşık olduğunu düşünmüştüm fakat Gemini'ın çıktısını incelerken mantığının ne kadar basit olduğunu gördüm, kusursuz çalışıyordu.

## Prompt 7: [InventoryUI ve Chest Mantığı]

**Araç:** Gemini
**Tarih/Saat:** 2026-01-30 , 21:38

**Prompt:**
> Solda bir liste olsun ve bana hangi Key'den kaç taneye sahip olduğumu ve ne kadar altınım olduğunu listelesin, şimdilik sadece Red Key = 1, Blue Key = 2, Gold = 150, gibi basit bir liste olacak, ikonlarla uğraşmayalım. Burada SimpleInventory'ye bir metot ekleyelim AddGold diye ve Chest'i eklediğimizde Chestten gold çıkmasını sağlayabileceğimiz Inspector atamaları ayarlarız.

**Alınan Cevap (Özet):**
> SimpleInventory.cs güncellemesi ve yeni InventoryUI.cs, Chest.cs (Promptuma göre Chest.cs'i vermemesi gerekiyordu.). Unity Editor'de yapılacak şeyleri listeledi.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Test ederken bu aşamaya kadar fark etmediğim bir hatayla karşılaştım. Sandığı press'leyerek de açabildiğimi fark ettim; 
-Raycast çalışıyordu.
-Sandık açılıyordu.
-Hold çalışmıyordu.
Sorunun OnInteract metodunun E'ye bastığım anda çalıştırılması olarak düşündüm.

## Prompt 8: [Mantık Hatası Çözümü]

**Araç:** Gemini
**Tarih/Saat:** 2026-01-30 , 22:25

**Prompt:**
> Chest'i pressleyerek açabiliyorum, Hold mantığı çalışmıyor. Eğer HoldDuration 0 ise OnInteract çalışsın, eğer HoldDuration > 0 ise, miktarı süresince Holdladıktan sonra OnInteract çalışsın.

**Alınan Cevap (Özet):**
> Hatanın InteractionDetector.cs içinde olduğunu fark etti, tam olarak dediğim şeyi yaparak hatayı düzeltti.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> InteractionDetector.cs hazırlanırken de HandleHoldInteraction() metodu vardı. Buna rağmen Gemini, bu mantık/sıralama hatasını yaptı.

## Prompt 9: [Switch Mantığı]

**Araç:** Gemini
**Tarih/Saat:** 2026-01-30 , 23:15

**Prompt:**
> (Switch/Lever sistemine geçebilir miyiz diye sormuştu önceki çıktısında)
geçebiliriz. Switch kullanmak istiyorum, Switch'i ufak bir küp olarak, Off state'de kırmızı, On state'de yeşil olacak şekilde tasarlayalım.
Hatırlatma:
3.3 Switch/Lever (Anahtar/Kol)
Toggle interaction
Başka bir nesneyi tetikleyebilmeli (örn: kapıyı açar)
Event-based connection

**Alınan Cevap (Özet):**
> Door.cs içine dışardan kontrol için SetState() metodu ekledi. Switch.cs classını verdi. Önemli olarak Events'lerde Dynamic bool ve Static Parameters arasındaki farkı açıkladı.

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> 