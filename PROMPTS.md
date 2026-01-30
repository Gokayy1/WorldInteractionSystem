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
