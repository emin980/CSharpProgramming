# Hafta 2 — Sorular, yaygın yanılgılar, alıştırmalar, özet

Önce kendiniz deneyin. İpuçları dosyanın sonundadır. Bu hafta `if`, döngü ve yeni sınıf **yasaktır**.

---

## Öğrenciye sorular

### Ders 1

1. C# metni işlemciye doğrudan mı gider? Aradaki iki adımı (IL, JIT) sırayla yazın.
2. CLR ile CTS’yi birer cümleyle ayırın. HangarDesk’te `int equipmentCount` CTS’nin neresine düşer?
3. `bin/Debug/net8.0/` klasöründeki çıktıya neden **assembly** denir? Kaynak kod mudur?

### Ders 2

4. `using System;` silinirse `Console` için ne olur? Implicit usings neden kapatıldı?
5. Namespace nedir? Bu hafta `CSharpProgramming` neden `HangarDesk` yapılmaz?
6. `Main` neden kapıdır? Tüm zimmet kurallarını `Main`’e yazmak neden dönem planına aykırıdır?
7. `dotnet build` ile `dotnet run` farkı nedir? `csc` kelimesi ne anlama gelir?

### Ders 3

8. `int copiedCount = equipmentCount;` sonra `copiedCount = 1;` — ekranda orijinal adet neden 0 kalır?
9. `'A'` ile `"A"` farkı nedir?
10. `object` bu hafta neden var? Boxing’i bu hafta neden yapmıyoruz?
11. Ekipman adedini `string` tutmanın ileride hangi sıkışmayı doğuracağını (Hafta 3–4’ü bekleyerek) tahmin edin.

---

## Yaygın yanılgılar

| Yanılgı | Düzeltilmiş hali |
| --- | --- |
| C# = makine dili | C# → IL → JIT → makine kodu |
| CLR = Visual Studio | CLR çalışma ortamı; VS editördür |
| Assembly = kaynak `.cs` | Assembly derlenmiş pakettir |
| `=` matematikte eşittir | Atamadır: kutuya koy |
| `char` ve `string` aynı | Tek karakter vs yazı |
| `string` kopya deneyi = değer türü deneyi | `string` referanstır ama immutable; kopya deneyi `int` ile yapılır |
| Hafta 2’de menü ve zimmet | Karar yok (`if` Hafta 4) |
| İkinci `Equipment.cs` | Sınıf Hafta 6 |
| `$"Adet: {x}"` | Bu hafta gerekmez; `+` yeter (interpolation müfredatta erken değil) |

---

## Alıştırma A — Zinciri sırala (Ders 1, ~5 dk)

Kartları doğru sıraya dizin: `JIT` · `C# kaynak` · `CLR yükler` · `IL` · `konsolda yazı` · `assembly`

---

## Alıştırma B — Canlı kod (Ders 2–3, ~15 dk)

`Program.cs` açıkken, çalışan koda **tek** yeni değişken ekleyin: `string sampleEquipmentName = "LiPo 6S";` ve bir `WriteLine` ile basınız.

Yapmayın: `if`, kullanıcıdan `ReadLine` ile hesap, yeni dosya, yeni class.

Amaç: tür + isim + `Console` alıştırması. İsterseniz ders bitince bu satırları silmeyin; Hafta 3 üzerine binecek.

---

## Alıştırma C — Tür seç (Ders 3, ~7 dk)

Aşağıdakilerin her biri için tür seçin ve bir cümle gerekçe yazın. Kod zorunlu değil.

1. Hangardaki drone sayısı  
2. Bataryanın voltajı  
3. Koridor harfi  
4. “Bakım tamam” notu  
5. Masa açık mı?  
6. Uygulamanın görünen adı  

---

## Ev ödevi

1. Zinciri (C# → IL → assembly → CLR → JIT → çalışır) ezberlemeden, kendi örneğinizle (zimmet değil, yoklama bile olur) altı cümle yazın.
2. `dotnet build` ve `dotnet run` çıktısını bir kez alın (ekran görüntüsü şart değil; hata varsa mesajı okuyun).
3. `Program.cs`’i okuyun. Her değişkenin türünü bir tabloya yazın: isim, tür, HangarDesk anlamı.

---

## Cevap ipuçları

**1.** Hayır. Derleyici IL üretir; JIT IL’i bu makinenin koduna çevirir.  
**2.** CLR ortam (yükler, JIT, çalıştırır). CTS tür sözlüğü. `int` tam sayı türüdür (`System.Int32` ailesi).  
**3.** Derlenmiş paket. `.cs` kaynak değildir.  
**4.** `Console` bulunamaz (veya tam ad gerekir). Gizli using öğretmez.  
**5.** İsim grubu / mahalle. Yol ve karar: namespace Hafta 8.  
**6.** CLR buradan başlar. Kapıya yığılan kod okunmaz, klasörleme bozulur.  
**7.** `build` derler, `run` derleyip çalıştırır. `csc` klasik C# derleyicisi; biz `dotnet` kullanırız.  
**8.** `int` değer türü; kopya ayrı kutudur.  
**9.** `char` vs `string`.  
**10.** Ortak tür fikri / CTS. Boxing Hafta 3.  
**11.** `"3" + "1"` birleştirme olabilir; gerçek adet toplama ve karşılaştırma sayı ister.

**Alıştırma A sırası:** C# kaynak → IL → assembly → CLR yükler → JIT → konsolda yazı.

**Alıştırma C örnek:** 1 `int` 2 `double` 3 `char` 4 `string` (veya şimdilik `object` not) 5 `bool` 6 `string`

---

## Ders sonu özetleri

**Ders 1.** C# → IL → assembly; CLR sahne, JIT çevirmen, CTS tür kuralı.  
**Ders 2.** `using`, namespace, `Main`, `dotnet build` / `run`.  
**Ders 3.** Türlü kutular; değer kopyası; HangarDesk konsolda görünür, henüz zimmetlemez.

## Haftanın tek cümlesi

Program artık çalışır ve sayıları **türlü** tutar; karar, liste ve ekipman kartı sonraki haftaların ihtiyacıdır — hepsi tek `Program.cs` mezarlığına değil, ileride kendi dosyalarına gidecektir.
