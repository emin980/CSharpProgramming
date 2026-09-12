# Ders 2 — İlk C# programı, namespace, derleme

**Süre:** yaklaşık 50 dakika  
**Kod:** `Program.cs` içinde karşılama satırları. İkinci `.cs` dosyası yok.

---

## Bu dersi neden okuyorsunuz?

Çalışma modelini öğrendiniz. Şimdi HangarDesk’in **görünür** olması gerekir. Aksi halde teknisyen hâlâ deftere bakıyor demektir.

Bu derste:

- Bir C# dosyasının iskeleti,
- `using` ve `namespace`,
- `Main`’in “kapı” oluşu,
- Visual Studio ve komut satırı ile derleme

işlenir. Değişken listesinin tamamı Ders 3’tedir; burada önce **çalışan bir metin** çıkar.

---

## 1. Eski durum ve sorun

Hafta 1 çıktısı: kâğıtta algoritma. Sorun: kâğıt konsolda durmaz, F5 ile çalışmaz, laboratuvar PC’si onu çalıştırmaz.

Çözüm: C# kaynak kodu + derleme. İlk hedef zimmetlemek değil, **programın ayağa kalkmasıdır**.

---

## 2. `using` nedir?

Dosyanın başındaki:

```csharp
using System;
```

**Anlamı:** `System` isim alanındaki hazır araçları (örneğin `Console`) bu dosyada kısa adla kullanacağız.

Projede **Implicit usings kapatıldı.** Böylece bu satır görünür. Gizli `using` ile “Console nereden geldi?” sorusu kalmaz.

`System` bir klasör adı değildir; .NET’in kök isim alanıdır. `Console.WriteLine` konsola bir satır yazar.

---

## 3. Namespace nedir?

```csharp
namespace CSharpProgramming
{
    ...
}
```

**Namespace**, isimleri **gruplayan etiket**tir. Şehirdeki mahalle gibi: aynı `Program` adı başka mahallede de olabilir; sizin mahalleniz `CSharpProgramming`’dir.

Bu dönem klasör adı ve varsayılan namespace **CSharpProgramming** olarak kalır (öğrenci yolu bozulmasın). Ürün adı HangarDesk’tir; namespace’i HangarDesk yapmak **Hafta 8** kararıdır. Bu hafta namespace satırını **silmeyin**, değiştirmeyin.

---

## 4. `Main` — giriş kapısı

```csharp
static void Main(string[] args)
```

İşletim sistemi + CLR, konsol uygulamasında **buradan** başlar. `args` komut satırı argümanlarıdır; bu hafta kullanmayız (dizi Hafta 4).

`class Program` ve `static` kelimelerini **şimdilik şablon** kabul edin. Yeni sınıf yazmayın. Tüm HangarDesk’i yıllarca bu `Main`’in içine de dökmeyin: bu hafta kısa bir karşılama yeter.

---

## 5. İlk çalışan metin (canlı kodlama)

`Hello, World!` satırını HangarDesk’e çevirin. Ders 3’te değişkenler eklenecek; isterseniz önce sabit metinle çalıştırın:

```csharp
Console.WriteLine("=== HangarDesk ===");
Console.WriteLine("Yer: Lab / Hangar");
```

`WriteLine` satır sonuna iner. Yazıları `+` ile birleştirmek Ders 3’te değişkenle birlikte gelir.

Noktalı virgül `;` C#’ta cümle bitiricidir. Unutursanız derleyici hata verir — bu **yazım / derleme hatasıdır** (Hafta 1).

---

## 6. Komut satırı derleyicisi

Visual Studio F5 yeterlidir; yine de zinciri görün.

Proje klasöründe (`.csproj`’in olduğu yer):

```text
dotnet build
dotnet run
```

- `dotnet build` — C# → IL, assembly üretir (`bin` klasörü).
- `dotnet run` — derleyip çalıştırır.

Klasik **csc** (C# derleyicisi) SDK’nın içindedir; eski kitaplar `csc Program.cs` yazar. Bu dönemde **`dotnet`** resmi yoldur. `csc`’nin varlığını bilin: “komut satırından da derlenir”; her ödevde `csc` ezberi yok.

Derleme hatası olursa program **çalışmaz**. Mantık hatası olursa çalışır ama yanlış yazar (örneğin adedi 0 iken 5 basmak).

---

## 7. Assembly’yi diskte görmek (2 dakika)

Derlemeden sonra: `bin/Debug/net8.0/`. Oradaki DLL, Ders 1’deki **assembly** paketidir. Bu klasörü ödev diye kopyalamayın; kaynak `Program.cs`’tir.

---

## Kontrol listesi

- [ ] `using System;` satırının neden göründüğünü anlattım.
- [ ] Namespace’i mahalle benzetmesiyle söyledim; bu hafta değiştirmedim.
- [ ] `Main`’in kapı olduğunu, tüm dönemin kapıya sığmayacağını söyledim.
- [ ] `dotnet build` ve `dotnet run` komutlarını en az bir kez gördüm.

---

## Kısa özet

İlk program: `using`, namespace, `Main`, `Console.WriteLine`. Derleme: `dotnet build` / F5. HangarDesk artık konsolda **vardır**; henüz zimmet **yapmaz**.

**Sonraki ders:** Adet, voltaj, isim — bunları **türlü değişkenlerde** tutmak.
