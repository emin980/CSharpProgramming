# Ders 3 — Değişkenler, temel türler, değer / referans, `object`

**Süre:** yaklaşık 50 dakika  
**Kod:** `Program.cs` içindeki HangarDesk karşılama değişkenleri. `if`, dizi, metot, ikinci sınıf yok.

---

## Bu dersi neden okuyorsunuz?

Konsola sabit yazı basmak yetmez. HangarDesk gerçekte **sayı** (kaç ekipman), **yazı** (yer, ad), **evet/hayır** (masa hazır mı) tutacaktır.

Sorun: Hepsini “yazı” sanmak. “3” yazısı ile 3 sayısı aynı değildir; toplama ve karşılaştırma Hafta 3–4’te bu yüzden zorlaşır.

Çözüm: Her kutuya **tür** vermek. CTS (Ders 1) tam olarak bunu standartlaştırır.

---

## 1. Değişken nedir?

**Değişken**, isim verilmiş bir kutudur. Kutunun **türü** (içine ne konur) ve **değeri** (şu an ne var) vardır.

```csharp
int equipmentCount = 0;
```

- `int` — tür (tam sayı)
- `equipmentCount` — isim (İngilizce, anlamlı)
- `0` — başlangıç değeri
- `=` — atama (matematikteki “eşittir” değil: **kutuya koy**)

İsimler harf veya `_` ile başlar; boşluk yok; Türkçe karakter kullanmayın (`ekipmanAdedi` yerine `equipmentCount`).

---

## 2. Bu hafta kullanacağımız temel türler

| C# türü | Ne tutar | HangarDesk örneği |
| --- | --- | --- |
| `int` | Tam sayı | Kayıtlı ekipman adedi, personel sayısı |
| `double` | Ondalıklı sayı | Örnek batarya voltajı `22.2` |
| `bool` | `true` / `false` | Masa hazır mı? |
| `char` | Tek karakter | Koridor kodu `'A'` (tek tırnak) |
| `string` | Yazı | Uygulama adı, yer (`"HangarDesk"` çift tırnak) |
| `object` | “Herhangi bir şey” kutusu | Kısa masa notu (şimdilik yazı koyuyoruz) |

Başka türler (`decimal`, `long`, `byte`…) vardır; bu hafta tablo yeter. `DateTime` Hafta 10.

`char` ile `string` karışmasın: `'A'` bir harf, `"A"` bir yazıdır.

---

## 3. Değer türü ve referans türü

İki aile vardır. Ezber cümle:

- **Değer türü:** Kutunun içinde **değerin kendisi** durur. Kopyalayınca **kopya** oluşur; birini değiştirmek diğerini değiştirmez.
- **Referans türü:** Kutunun içinde **adres** (nerede durduğu) durur. İki isim aynı nesneyi gösterebilir. (Asıl dram Hafta 6’da `Equipment` nesnesiyle görünür.)

Bu hafta değer türü örnekleri: `int`, `double`, `bool`, `char`.  
Bu hafta referans türü örnekleri: `string`, `object`.

HangarDesk kodundaki gösterim:

```csharp
int equipmentCount = 0;
int copiedCount = equipmentCount;
copiedCount = 1;
```

`equipmentCount` hâlâ 0 kalır. Çünkü `int` değer türüdür; kopyalanan **sayıdır**, “aynı kutu” değildir.

`string` referans türüdür ama **değiştirilemez** (immutable) olduğu için “birini değiştirdim hepsi değişti” gösterisi bu hafta yanıltıcıdır. O yüzden kodda asıl kopya deneyi `int` iledir. Referansın gerçek ihtiyacı: ileride aynı ekipman kartına iki yerden bakmak — sınıf haftası.

---

## 4. `object`

`object`, .NET’te her türün **ortak atası** fikridir (CTS). Bir `object` değişkene şimdilik bir yazı koyabilirsiniz:

```csharp
object deskMemo = "Zimmet listesi henuz bos";
```

Bu, “not bazen yazı, ileride başka şey olabilir” esnekliğidir. **Kutuya tıkıştırma (boxing)** ve geri çıkarma Hafta 3’tür; bu hafta `int`’i kasten `object`’e koyup matematik yapmayın.

HangarDesk’te asıl notlar ileride kendi türünde duracak. Bugün `object` yalnızca kelime dağarcığı ve bir satırlık örnektir.

---

## 5. Projeye uygulama — `Program.cs`

Aşağıdaki yapı dönem boyunca kapıdadır. Zimmet menüsü **eklenmez**.

Kodda ondalık **nokta** ile yazılır (`22.2`). Konsol Türkçe Windows’ta `22,2` basabilir; bu kültür ayarıdır, tür hâlâ `double`. Biçimlendirmenin asıl konusu ileride.

Anlamlı isimler: `applicationName`, `equipmentCount`, `sampleBatteryVoltage`, `deskIsReady`.

Konsola basarken yazı birleştirme:

```csharp
Console.WriteLine("Yer: " + hangarPlace);
```

`+` burada metin birleştirir. Sayıyı yazının yanına koymak da çalışır (`"Adet: " + equipmentCount`). Asıl dönüşüm kuralları Hafta 3.

Çalıştırınca görmelisiniz: başlık, yer, adet 0, kopyalanan adet 1, orijinal adet hâlâ 0. Bu çıktı **değer türü kopyasının** kanıtıdır.

---

## 6. Eski vs yeni

| Hafta 1 / ders başı | Şimdi |
| --- | --- |
| “Üç batarya var” cümlesi | `int equipmentCount = 0;` (şimdilik boş liste) |
| Kâğıtta algoritma | Konsolda çalışan program |
| Tür yok | Her kutunun türü var |

Hâlâ yok: kullanıcıdan okuma ile tür çevirme (Hafta 3), “adet 0 ise uyarı” (Hafta 4), ekipman kartı sınıfı (Hafta 6).

---

## Kontrol listesi

- [ ] Değişken = tür + isim + değer, atama “kutuya koy” dedim.
- [ ] Tablodaki altı türü HangarDesk cümlesiyle eşleştirdim.
- [ ] `copiedCount` değişince `equipmentCount`’un neden değişmediğini anlattım.
- [ ] `Program.cs`’e `if` veya ikinci sınıf eklemedim.

---

## Kısa özet

HangarDesk artık birkaç **türlü kutu** gösterir: adet, voltaj, hazır mı, not. Değer türü kopyalanır; referansın asıl hikâyesi nesnelerle gelecek. Kapı hâlâ `Main`; depo değil.
