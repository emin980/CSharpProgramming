# Hafta 1 — Sorular, yaygın yanılgılar, alıştırmalar, özet

Bu dosya hem sınıf içi kâğıt hem ev çalışmasıdır. Önce **kendiniz** deneyin; ipuçları dosyanın sonundadır.

---

## Öğrenciye sorular

Cevapları birer kısa paragraf yazın. C# kodu yasaktır.

### Ders 1

1. “Bataryaları biraz kontrol et” neden bir program cümlesi değildir? Eksik olan nedir? HangarDesk’te aynı cümleyi nasıl kesinleştirirsiniz?
2. Algoritma ile program (veya kaynak kod) arasındaki fark nedir? Birini diğerinden önce yazmak neden daha ucuzdur?
3. Bakımdaki bir bataryayı zimmetleyen sistem çalışıyorsa, bu hangi hata ailesidir? Derleyici bunu kırmızı çizgiyle gösterir mi? Neden?

### Ders 2

4. C# ile .NET’i birer cümleyle ayırın. İkisini aynı şey sanmak laboratuvarda hangi yanılgıya yol açar?
5. HangarDesk neden konsol uygulamasıdır? “Form daha modern olur” itirazına ne dersiniz?
6. Her hafta Visual Studio’da yeni konsol projesi açmak neden istenmez? Tek proje neyi görünür kılar?

### Ders 3

7. HangarDesk’in iki ana işi nedir? İHA okumayan biri bu iki işi kendi laboratuvarına nasıl çevirir?
8. `docs/Week01` klasörü ile `Program.cs`’in bulunduğu proje aynı şey midir? Farkı yazın.
9. Bütün kodun `Program.cs` içinde birikmesi neden istenmez? Tek proje ile tek dosya aynı kural mıdır?
10. Diziden (birçok adı yan yana tutmak) sınıfa (bir ekipmanın ad + seri + durumunu bir arada tutmak) geçmek “haftayı doldurmak” için değil, hangi sıkışma için olacaktır?

---

## Yaygın yanılgılar

Bunları bilin ki tekrarda düşmeyin.

| Yanılgı | Düzeltilmiş hali |
| --- | --- |
| Algoritma = akış şeması çizmek zorundayım | Şema araçtır. Asıl olan adımların tek anlamlı olmasıdır. |
| Algoritma = C# | Algoritma Türkçe tarif olabilir. C# çeviridir. |
| C# ve .NET aynı şey | Dil ve çalışma ortamı. |
| Visual Studio = .NET | Visual Studio editördür. .NET ortamdır. VS olmasa da .NET ile derlemek mümkündür (ayrıntı Hafta 2). |
| HangarDesk drone uçuracak | Kayıt ve kural programıdır, otopilot değildir. |
| Her hafta yeni proje | Tek HangarDesk büyür. |
| Tek proje = her şey `Program.cs` | Tek proje, **çok dosya ve klasör**. `Program.cs` kapıdır. |
| Hafta 1’de Hello World yazmazsak geride kalırız | Müfredat bilinçli: önce tarif, Hafta 2’de ilk kod. |
| Kalıtım ağacını bugün ezberlemeliyim | Hafta 11. Bugün “drone ile tornavida hem aynı hem farklı” demek yeter. |

---

## Alıştırma A — İade algoritması (Ders 1, çiftler, ~8–10 dk)

**Durum:** Personel ekipmanı geri getirir.

**İstene:** En az **6 adımlık** sözde kod (Türkçe). C# yok.

Zorunlu dallar:

- Seri numara listede yok.
- Durum zaten **Müsait** (alet zaten rafta — çift iade?).
- Personel “hasar var” dedi / demedi.

Bitince yanınızdaki gruba okuyun. Şu soruyu sorun: **Hangi adım hâlâ bir insana “sen anla” diyor?** O adımı kesinleştirin.

Örnek başlangıç (kopyalayıp bırakmayın; dalları siz doldurun):

```text
Girdi: seri numarası, hasar var mı (evet/hayır)
1. Seri numarasını al.
2. ...
```

---

## Alıştırma B — Kavram eşleştirme (Ders 2, ~5–7 dk)

Sol sütunu sağdaki anlamla eşleştirin. Her kavram **bir** kez kullanılır.

| Kavram | Anlam (karışık) |
| --- | --- |
| C# | A. Metin penceresi; soru-cevap |
| .NET | B. Koddan önceki, sonlu adım tarifi |
| Konsol | C. Programlama dili |
| Çözüm (`.sln`) | D. Dilin çalıştığı ortam ve kütüphane |
| Algoritma | E. Visual Studio’nun çalışma kutusu; içinde proje(ler) var |
| `Program.cs` | F. Uygulamanın giriş kapısı; tüm işin yığıldığı depo değildir |

---

## Alıştırma C — Minimum zimmet kuralları (Ders 3, 3–4 kişi, ~7–10 dk)

Kâğıda **yalnızca kurallar** yazın. Kod yok, ekran resmi yok.

1. Hangi bilgiler **şartsız** gerekli? (örnek: ad, seri no, kim, tarih — siz seçin ve nedenini yazın)
2. Hangi **durumlar** var? En az dört tane kullanın veya kendi dördünüzü savunun.
3. Hangi işlem **yasak**? En az iki yasak. (Örnek: bakımdakini uçuşa / deneye verme.)
4. İHA okumayan bir arkadaşınız bu kâğıdı okusa anlar mı? Anlamıyorsa kelimeleri değiştirin, kuralı değiştirmeyin.

Üç grup yüksek sesle okusun. Tahtada ortak bir “dönem sonu HangarDesk” listesi oluşsun. Bu liste Hafta 4 menüsünün tohumudur.

---

## Ev ödevi

Kendi bölümünüzden **bir** demirbaş seçin (drone, batarya, avometre, laptop, takım çantası…).

O aleti zimmetlemek için **8 adımlık** sözde kod yazın. C# yok.

Ek satır: HangarDesk’in varsayılan hangar hikâyesine **uymayan** bir kuralınız varsa not edin. Derste konuşulur; yazılım bazen sizin atölyenize göre eğilir, müfredat eğilmez.

---

## Cevap ipuçları (önce kendiniz deneyin)

**Soru 1.** “Biraz” ölçülemez; hangi batarya, hangi test, kayıt yoksa ne olacağı yok. Kesinleştirme: seri no ile kaydı bul, ölçütü yaz, dalları kapat.

**Soru 2.** Algoritma tarif, program o tarifin dildeki hali. Tarifteki mantık hatası, derleyiciden önce, kâğıtta düzeltilir; daha ucuzdur.

**Soru 3.** Mantık hatası. Program çalışır. Derleyici kural ihlali görmez; sizin kuralınız eksiktir.

**Soru 4.** C# dil, .NET ortam. “Visual Studio kurulu değilse C# yoktur” yanılgısı: asıl ihtiyaç SDK / ortamdır; editör yardımcıdır.

**Soru 5.** Zimmet bilgi ve kural işidir. Form, ilk dönemde konuyu yutar. Aynı kurallar sonra forma taşınabilir.

**Soru 6.** Kurallar birikir; dağınıktan düzenliye dönüşüm görünür. Yeni proje her hafta sıfırlar.

**Soru 7.** Ekipman zimmeti + görev kaydı. Laboratuvar: alet kimin üzerinde, hangi deney / kalibrasyon için.

**Soru 8.** `docs` ders notudur, çalışmaz. Proje derlenen uygulamadır. Hafta klasörü ≠ yeni C# projesi.

**Soru 9.** Tek dosya okunmaz, sorumluluk karışır, sınıf/klasör anlatılamaz. Tek proje ≠ tek dosya.

**Soru 10.** Bir ekipmanın birden fazla bilgisi dağınık kaldığında (ad bir yerde, durum başka yerde).

**Eşleştirme B:** C#–C, .NET–D, Konsol–A, Çözüm–E, Algoritma–B, `Program.cs`–F.

---

## Ders sonu özetleri (her oturum 2 dakika)

**Ders 1.** Bilgisayar niyet okumaz. Önce algoritma. Çalışan ama yanlış sonuç = mantık hatası.

**Ders 2.** C# dil, .NET ortam, .NET 8, konsol, tek proje. İlk sözdizimi gelecek hafta.

**Ders 3.** HangarDesk = zimmet + görev. Notlar `docs/WeekXX`. Kod tek projede **klasörlenerek** büyür. `Program.cs` kapıdır.

---

## Haftanın tek cümlesi

Önce tarifi yaz; C#’ı Hafta 2’de konuştur; 14 hafta aynı HangarDesk’i **dosya dosya, klasör klasör** büyüt — her hafta yeni yığın değil, her şeyi tek dosyaya tıkma.
