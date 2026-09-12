# Hafta 2 — İlk C# programı, .NET çalışma modeli, türler

Hafta 1’de zimmeti Türkçe tarif ettik. Bu hafta o tarifi çalıştıran **dil** ve **ortam**ın içine giriyoruz; HangarDesk ilk kez konsolda görünür.

**Kod vardır**, ama menü, zimmet, `if`, dizi ve sınıf **yoktur**. `Program.cs` hâlâ **kapıdır**: kısa bir karşılama ve birkaç değişken. 14 haftalık iş buraya yığılmaz.

## Bu hafta ne öğreneceksiniz?

1. C# metninin IL’e, IL’in CLR + JIT ile makine koduna giden yolunu kendi cümlelerinizle anlatmak
2. CTS, assembly, namespace kelimelerinin yerini bilmek
3. `dotnet build` / `dotnet run` ile derleyip çalıştırmak
4. Değişken, temel türler, değer türü / referans türü, `object`
5. HangarDesk’in konsolda adını, yerini ve birkaç sayıyı göstermesi

## Üç ders saati

| Sıra | Dosya | Konu |
| --- | --- | --- |
| Ders 1 | [01 — CLR, CTS, JIT, IL, assembly](01-calisma-modeli.md) | Program nasıl çalışır? |
| Ders 2 | [02 — İlk program](02-ilk-program.md) | `using`, namespace, `Main`, komut satırı |
| Ders 3 | [03 — Türler ve değişkenler](03-turler.md) | `int`, `string`, değer/referans, `object`, HangarDesk |
| Alıştırmalar | [04 — Sorular ve uygulamalar](04-sinif-ici.md) | |

## Projeye katkı

Konsolda **HangarDesk** başlığı, yer bilgisi, ekipman adedi (`int`), örnek voltaj (`double`), hazır mı (`bool`), kısa bir not (`object`). Zimmet işlemi yok — çünkü işlem **karar** ister (`if`, Hafta 4).

## Şablonda görüp henüz anlatılmayanlar

`class`, `internal`, `static` satırları Visual Studio şablonundan gelir. **Sınıf Hafta 6, static Hafta 7.** Bu hafta onları silmeyin, yeni sınıf da eklemeyin. Sizin işiniz `Main`’in içi ve `using` satırıdır.

> **Öğretmen:** Canlı kodlamada önce `Hello, World!` satırını silip birlikte HangarDesk karşılama metnini yazın. `if` veya ikinci dosyaya kaçmayın.
