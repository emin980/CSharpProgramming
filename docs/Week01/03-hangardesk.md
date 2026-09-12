# Ders 3 — HangarDesk: ne yapacağız, kod nasıl büyüyecek?

**Süre:** yaklaşık 50 dakika  
**Kod:** yok. Mimari diyagram ve kalıtım ağacı çizilmez (kalıtım Hafta 11).

---

## Bu dersi neden okuyorsunuz?

Dil ve ortamın adını öğrendiniz. Şimdi **hangi problemi** 14 hafta çözeceğimizi netleştiriyoruz. Konu rastgele seçilmedi:

- İHA öğrencisi hangardaki drone, kumanda, batarya, GPS modülünü tanır.
- Başka bölümden gelen öğrenci aynı modeli laboratuvar aletine, laptopa, takım çantasına uygular.

Yazılımın adı **HangarDesk** (hangar masası). Atölye ve laboratuvar da aynı masadır.

---

## 1. Sorun: bilgi dağınık, kural yok

Hangar sahnesi:

Uçuştan önce “üçüncü batarya kimde?” diye bakılır. Defterin bir sayfası kopmuştur. Excel birinin bilgisayarında açıktır. Kumanda uçuşta, kaydı “müsait” görünür. Bakımdaki gövde yanlışlıkla sahaya çıkar.

Bu bir uçuş kontrolcüsü problemi **değildir**. Otopilot, görüntü işleme, yer istasyonu haritası yazmayacağız. Bu dersin konusu: **kayıt, kural, yapı**.

Aynı sahne, elektronik laboratuvarı:

Osiloskop dolapta yoktur. Zimmet formu kayıptır. “Kimin aldığını herkes biliyor” denir; dönem sonunda alet dönmez.

HangarDesk her iki sahneye de uyar: **ekipman**, **kimde**, **hangi iş için**, **ne durumda**.

---

## 2. HangarDesk ne tutacak? (dönem sonu resmi)

Aşağıdaki liste **hedeftir**. Bu hafta hiçbiri kodlanmaz. Her madde ileride bir C# konusunun **gerekçesi** olacaktır.

HangarDesk konsolda:

- Ekipman ekler ve listeler (ad, seri numarası, durum).
- Zimmet alır ve iade eder.
- Durumları ayırır: **Müsait**, **Zimmetli**, **Bakımda**, **Kayıp**.
- Göreve bağlar: uçuş, bakım, saha ölçümü, teslim — laboratuvarda deney, kalibrasyon, ödünç.
- Daha sonra tarihi bilir, dosyaya yazar, hatalı girişte kırılmaz, “bakımdakileri listele” diye sorgu yapar.
- Takvim elverirse ek bir haftada veritabanına kaydeder.

**Bilerek yapılmayacaklar:** mobil uygulama, web sitesi, Windows form tasarımı, drone’u havada tutan yazılım.

---

## 3. “Bunu neden öğreniyorum?” — konuların HangarDesk’te doğması

Yeni konu, süs olsun diye gelmez. Tipik sıkışmalar:

| Sıkışma (Türkçe) | İleride gelen konu (haftası) |
| --- | --- |
| Kullanıcı yazı yazıyor, program sayı bekliyor | Tür dönüşümü (3) |
| Üç ekipmanı elle, kopyala-yapıştır yönetmek | Dizi, döngü, menü (4) |
| Aynı hesap her yerde tekrar | Metot (5) |
| Ad ayrı, seri no ayrı, durum ayrı — dağıldı | Sınıf (6–7) |
| Müsait/Zimmetli yazım hatası | Enum (8) |
| Program kapanınca liste uçuyor | Dosya (10) |
| Drone ile tornavida aynı zimmet, farklı ayrıntı | Kalıtım (11–12) |
| Yanlış girdi programı düşürüyor; gecikme uyarısı | Exception, olay (13) |
| “Ahmet’teki bakımdakiler” sorgusu | LINQ (14) |

Bugün bu tabloyu ezberlemek zorunda değilsiniz. Asıl mesaj: **her konu bir ihtiyacın cevabıdır.**

---

## 4. Tek proje, hafta hafta büyüme — ayrı proje yok

Her hafta yeni bir “Console App 3”, “Console App 4” **açılmayacak**.

Neden?

- Zimmet kuralları bir yerde birikir. Her hafta sıfırdan yazarsanız kural unutulur.
- Dersi göreceğiniz şey, dağınık başlangıcın **düzenli yapıya** dönüşmesidir. Bu, nesne yöneliminin ta kendisidir.
- Öğretmen ve siz aynı klasörde konuşursunuz: `CSharpProgramming`.

`docs/Week01`, `docs/Week02` **ders notu** klasörleridir. Bunlar C# projesi değildir. Çalışan kod notların yanında, **tek** `.csproj` içindedir.

---

## 5. Klasörleme: her şey `Program.cs` içinde durmaz

C#’ta `Program.cs` dosyasında bir `Main` metodu vardır. Program **buradan başlar**. Bu, her satırın burada yaşayacağı anlamına gelmez.

Eğer 14 haftanın menüsü, zimmeti, dosya yazması ve sorguları tek dosyada birikirse:

- Dosya okunamaz.
- “Şu kural nerede?” sorusu cevaplanamaz.
- Tek sorumluluk (bir birimin tek işi) görünmez.
- Sınıf ve klasör anlatmanın gerekçesi kalmaz.

Bu yüzden kural şudur:

**`Program.cs` kapıdır. İş, klasörlere dağılır. Haftalık ikinci proje açılmaz.**

Hedef (dönem içinde, konu anlatıldıkça dolacak):

```text
CSharpProgramming/
  Program.cs              ← yalnızca uygulamayı başlatır
  Presentation/           ← menü ve konsol konuşması
  Inventory/              ← ekipman
  Missions/               ← görev
  People/                 ← personel
  Data/                   ← dosyaya / ileride veritabanına kayıt
  docs/                   ← sizin okuduğunuz bu notlar
```

### Neden hemen boş klasör oluşturmuyoruz?

Çünkü henüz **sınıf** ve **metot** anlatılmadı. Olmayan konuyu “profesyonel dursun” diye öne çekmek, bu dersin öğretim kuralına aykırıdır.

Geçiş şöyle olacak:

1. **Hafta 1 (bugün):** Kod yok.
2. **Hafta 2–4:** İlk program, türler, koşul, döngü, dizi. Bunlar kısa tutulur; geçici olarak `Program.cs` içinde durabilir. Amaç “tek dosyada yaşamak” değil, **henüz bölmeye yarayan dili öğrenmektir**.
3. **Hafta 5:** Metotlar ayrı dosyalara çıkar. Dosya bir kutudur; kutunun nesne yönelimli anlatımı Hafta 6’dadır.
4. **Hafta 6 ve sonrası:** `Equipment`, `Mission` gibi her varlık kendi dosyasında, yukarıdaki klasörlerde.
5. **Hafta 10+:** Kayıt işi `Data/` altına gider.

Öğrenci olarak sizin göreceğiniz şey: aynı HangarDesk, her hafta biraz daha **okunur** hale gelir — her hafta yeni bir yığın değil.

---

## 6. İsimler ve alışkanlık (bugünden, kod olmadan)

Kod yazmadan da şu alışkanlık başlar:

- Şeylere **anlamlı ad** verin. “O şey”, “şöyle”, “x” ile algoritma yazmayın. “Batarya seri no”, “personel adı” deyin. İleride kodda İngilizce karşılıklar kullanılacak (`serialNumber`, `staffName`).
- **Tek iş.** “Listele” ile “zimmetle” aynı cümlede karışmasın. İleride bu, ayrı metot ve ayrı dosya demektir.
- **Tekrar.** Aynı üç adımı dört yerde yazıyorsanız algoritma henüz toparlanmamıştır.

SOLID sözcüğünü bugün ezberletmiyoruz. Tek iş, net isim yeter.

---

## 7. Ders sözleşmesi

- Haftada yaklaşık üç ders saati vardır; evde kısa tekrar beklenir.
- Amaç ezber listesi değil, çalışan ve **okunabilir** HangarDesk’tir.
- Anlamadığınız satırı projeye yapıştırmayın.
- Soru sorun. Hafta 2’den itibaren derleyici de mesaj yazar; o mesajı okumak meslekî reflekstir.
- İHA örneği anlamadıysanız aleti değiştirin: aynı kural, farklı isim.

---

## Kontrol listesi

- [ ] HangarDesk’in iki ana işini söyledim: zimmet ve görev.
- [ ] Kendi bölümümden bir demirbaşla aynı modeli kurdum.
- [ ] Tek proje kuralını, haftalık yeni proje açmamayı kabul ettim.
- [ ] `Program.cs`’in kapı olduğunu, işin klasörlere dağılacağını anlattım.

Alıştırmalar: [04-sinif-ici.md](04-sinif-ici.md) — **Alıştırma C**.

---

## Kısa özet

HangarDesk, hangar veya laboratuvarın zimmet masasıdır. 14 hafta **aynı** program büyür. Notlar `docs/WeekXX` altındadır; kod tek projede, **klasörlenerek** durur. Kapı `Program.cs`’tir, depo değildir.

**Hafta 2 köprüsü:** İlk C# programı, .NET’in nasıl çalıştığı, sayı ve yazı türleri. Zimmet menüsü henüz yok; çünkü zimmet adedi ve seri numara ileride sayı ve yazı olacaktır. Önce o tuğlalar.
