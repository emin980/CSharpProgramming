# Kodlama kuralları

## Seviye

Yalnızca [`../state/LEARNING.md`](../state/LEARNING.md) içindeki özellikler. [`../state/LOCKED.md`](../state/LOCKED.md) yasaktır.

Hafta 1: **proje kodu yok.** İskelet `Hello, World!` kalır.

## Okunabilirlik

- Anlamlı İngilizce isimler: `equipmentName`, `Checkout`, `Mission` — `x`, `tmp1` yok
- Öğrenci seviyesinde kısa metotlar; “süper jenerik yardımcı kütüphane” yok
- Tek sorumluluk: menü ayrı, hesap ayrı (ilgili haftadan itibaren)
- Tekrar varsa metoda çek (Hafta 5+)
- Yorum: neden; ders notu gibi satır satır “bu bir döngüdür” yazma
- Sihirli sayı yerine ileride enum/const (kendi haftasında)

## Mimari ve klasörler

- Tek konsol projesi, tek `.csproj`
- `Program.cs` = yalnızca `Main` (Hafta 2–4’te geçici olarak ilk örnekler burada, kısa)
- Haftalık `WeekXX` kod klasörü yok
- Özellik klasörleri: `Presentation/`, `Inventory/`, `Missions/`, `People/`, `Data/` — ilgili konu anlatılınca
- Bir sınıf = bir dosya (Hafta 6+); dosya adı tür adıyla aynı
- Interface, abstract, generic: kendi haftası
- NuGet: kullanıcı/müfredat istemeden yok

## Hata ve girdi

- Hafta 13’e kadar kaba `if` ile geçersiz giriş yeterli olabilir
- `try/catch` öne çekilmez

## Biçim

- Mevcut dosya stiline uy (girinti, namespace bloğu)
- Öğretim için ImplicitUsings ve Nullable **kapalı**; `using System;` görünür
