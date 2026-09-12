# Ders 2 — C# nedir, .NET nedir, biz nasıl çalışacağız?

**Süre:** yaklaşık 50 dakika  
**Kod:** yok. `Console.WriteLine` ezberlenmez. Konsola “merhaba” yazmak Hafta 2’nin işidir.

---

## Bu dersi neden okuyorsunuz?

Ders 1’de zimmeti Türkçe adımlara döktük. Bilgisayar Türkçe maddeleri çalıştırmaz. Bir **dil** ve o dilin **çalışma ortamı** gerekir.

Bu derste haritayı çizeceğiz. Motorun içi (CLR, JIT, IL) Hafta 2’de açılacak. Bugün şu ayrımı kilitliyoruz:

> **C#** sizin yazdığınız dildir. **.NET** o yazının çalışmasını sağlayan ortam ve kütüphanedir. İkisi aynı şey değildir.

---

## 1. C# (C Sharp) nedir?

**C#**, Microsoft ekosisteminde yaygın kullanılan bir programlama dilidir. Bu dersin (BPP2001) hedef dili C#’tır.

C# ile:

- Adımları kurallı yazarsınız (Hafta 2: değişken ve tür).
- Karar ve tekrar ifade edersiniz (Hafta 4: `if`, döngü — bugün sözdizimi yok).
- İleride gerçek hayattaki “ekipman”, “görev” gibi varlıkları **sınıf** olarak modellersiniz. Bu dersin adı bu yüzden nesne yönelimlidir. Sınıf yazmak **Hafta 6**’dadır; bugün yalnızca “o yöne gideceğiz” bilinir.

Başka diller kötü olduğu için seçilmedi. Ders sözleşmesi C#’tır. Algoritma düşüncesi dilden önce gelir; aynı zimmet tarifi teoride başka dilde de kodlanabilir. Siz C# öğreneceksiniz.

Kısa yön bulma (derinleşme yok):

- **C / gömülü yazılım:** Uçuş denetleyicisi, anlık donanım. Bu dersin konusu değil. Yer istasyonu kaydı, laboratuvar zimmeti, Windows’ta çalışan araçlar sıkça .NET tarafındadır.
- **Python:** Hızlı deneme için yaygındır; müfredatımız C#.
- **Java:** Nesne fikri benzerdir; sözdizimi ve ekosistem farklıdır.

---

## 2. .NET nedir?

**.NET** (okunuşu “dot net”), C# programlarının **üzerinde çalıştığı platformdur**. İçinde:

- Programı çalıştıran ortam,
- Hazır araçlar (konsola yazı yazmak, ileride tarih, dosya, metin),
- Derleme ve paketleme düzeni

vardır.

Basit zincir (ezberlenecek model):

```text
Sizin C# metniniz  →  derleyici  →  .NET'in çalıştırdığı program  →  Windows (bizim laboratuvar)
```

Bu dönem kullanacağımız sürüm: **.NET 8**. Proje zaten bu sürümle oluşturulmuştur. Laboratuvar bilgisayarlarıyla uyum için sürümü rastgele yükseltmeyiz.

Sık karışan kelimeler:

| Kelime | Bu derste yeterli anlam |
| --- | --- |
| C# | Dil (sözdizimi ve kurallar) |
| .NET | Dilin çalıştığı ortam + hazır kütüphane |
| SDK | Bilgisayarda derlemek için kurulu araç seti |
| Visual Studio | Kodu yazıp çalıştırdığınız program (editör + yardımcılar) |
| Assembly | Programın paketlenmiş hali; ayrıntı Hafta 2 |

**Soru:** C# ile .NET aynı şey mi?  
**Cevap:** Hayır. Mektup (C#) ile posta sistemi (.NET) gibidir. Mektubu siz yazarsınız; sistem iletir ve kurallarını koyar.

---

## 3. Konsol uygulaması nedir? Neden pencere yok?

**Konsol**, metin tabanlı penceredir. Program soru sorar, siz yazarsınız, program liste basar. Fare ile sürüklenen form, web sitesi veya mobil uygulama **bu dönemin hedefi değildir**.

Neden konsol?

1. İlk dönemde düğme ve renk tasarımı, asıl konuyu (mantık + C# + nesne) yutar.
2. Zimmet işi özünde bilgidir: kim, hangi alet, hangi durum, hangi tarih. Bunlar metin ve kurallardır.
3. Kuralları bir kez doğru yazarsanız, ileride aynı kurallar bir forma taşınabilir. Taşıma kolaylığı nesne yöneliminin vaadidir; o vaadin sözdizimi Hafta 11–12’dedir.

Örnek sözleşme (C# değil, girdi/çıktı fikri):

```text
Program:  Ekipman adı?
Kullanıcı: LiPo 6S
Program:  Kayit alindi: LiPo 6S
```

Bunu Hafta 2’de gerçekten yazacağız. Bugün `ReadLine` öğretilmez. Yalnızca şunu görün: HangarDesk kullanıcıyla **metin üzerinden** konuşacaktır.

---

## 4. Visual Studio, çözüm ve tek proje

**Visual Studio**, kodu yazdığınız, çalıştırdığınız ve (Hafta 2’den itibaren) hataları avladığınız ortamdır.

İki dosya türünü ayırın:

- **Çözüm (`.sln`):** Visual Studio’nun “bu çalışma budur” dediği kutu. Sizin çözümünüz: `CSharpProgramming.sln`.
- **Proje (`.csproj`):** Derlenen uygulama. Sizin tek projeniz: `CSharpProgramming.csproj`.

**Dönem kuralı:** Her hafta *File → New → Console App* ile yeni proje **açılmaz**. HangarDesk **aynı** projede büyür. Öğrenci, dağınık bir başlangıcın nasıl düzgün bir yapıya döndüğünü böyle görür.

Klasör yolu (Windows): `Masaüstü/Projects/CSharpProgramming`

> **Öğretmen (isteğe bağlı, 2–3 dakika):** Solution Explorer’da tek projeyi gösterin. `Program.cs` üzerine uzun uzun gitmeyin. “Giriş kapısı gelecek hafta; içini bu hafta doldurmuyoruz.”

**Hata ayıklama (debug)** kelimesini tanıyın: programın neden yanlış gittiğini adım adım aramak. Kısayollar ve pencereler Hafta 2’de.

---

## 5. Kod nereye yazılacak? (önemli)

İki yanlış beklenti vardır:

1. “Her hafta yeni bir proje.” — Hayır. Tek HangarDesk.
2. “Her şey `Program.cs` içine.” — Hayır. `Program.cs` yalnızca programı **başlatır**.

C#’ta her şeyin bir dosyada durması teknik olarak mümkündür; eğitim olarak yanlıştır. Dönem ilerledikçe:

- menü bir yere,
- ekipman başka yere,
- görev başka yere,
- dosyaya kayıt başka yere

gider. Klasör isimleri ve geçiş haftaları bir sonraki derstedir. Bugün kilit cümle:

> Tek proje, çok dosya, düzenli klasörler. `Program.cs` mezarlık değildir.

İlk iki-üç kod haftasında, sınıf konusu henüz işlenmediği için kısa örnekler geçici olarak `Program.cs` içinde durabilir. Bu bir **ara durak**tır; nihai düzen değildir.

---

## 6. Programcının günü (HangarDesk ölçeği)

1. İhtiyacı bir cümleye dök. (“Bakımdaki alet zimmetlenemesin.”)
2. Algoritmayı yaz.
3. C# ile ilgili parçayı kodla (Hafta 2+).
4. Çalıştır, özellikle bozucu girdileri dene.
5. Bozuksa önce algoritmaya dön; hemen yeni sözdizimi arama.

---

## Kontrol listesi

- [ ] C# = dil, .NET = ortam, cümlesini kurdum.
- [ ] Konsolun neden seçildiğini zimmet işiyle bağladım.
- [ ] Tek çözüm / tek proje kuralını söyledim.
- [ ] `Program.cs`’e her şeyin yığılmayacağını söyledim.

Alıştırmalar: [04-sinif-ici.md](04-sinif-ici.md) — **Alıştırma B**.

---

## Kısa özet

C# tarif diliniz, .NET çalışma ortamınız, arayüzünüz konsol, eviniz tek projedir. Haftaya tarifin ilk gerçek cümlelerini yazacağız. Bugün kimse sözdizimi ezberlemek zorunda değildir.
