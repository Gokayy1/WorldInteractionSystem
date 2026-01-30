# LLM Kullanım Dokümantasyonu

## Özet
- Toplam prompt sayısı: X
- Kullanılan araçlar: Claude / Gemini
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
