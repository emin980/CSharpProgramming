# Proje kararları

Tarih sırasıyla. Çelişen yeni karar eskiyi geçersiz kılmaz; eski maddeyi “geçersiz” işaretle.

## K-2026-09-12-01 — Ürün ve konu

- Görünen ad: **HangarDesk**
- Klasör / `.sln` / varsayılan namespace: **CSharpProgramming** (öğrenci yolunu bozmamak)
- Namespace’i HangarDesk yapmak: Hafta 8 (`namespace`) işlenene kadar yok
- Konu: laboratuvar / hangar / saha **ekipman zimmeti** + **görev kaydı**
- İHA örnekleri varsayılan; diğer bölümler için aynı model (demirbaş + ödünç)

## K-2026-09-12-02 — Teknik taban

- .NET **8** / C# 12, `OutputType` Exe, tek proje
- .NET 10’a geçiş yok (lab uyumu), kullanıcı istemeden TFM değişmez
- UI: konsol; dönem sonu da konsol (dosya + isteğe bağlı DB)

## K-2026-09-12-03 — Eğitim şekli

- Aynı çözüm 14 hafta büyür; haftalık ayrı proje yok
- Haftanın konusu projede gerçek bir ihtiyaç olarak çıksın (problem → kavram → mini örnek → projeye uygulama → fark)
- Clean Code alışkanlığı; enterprise mimari yok
- Kod tanımlayıcıları İngilizce; ders metni Türkçe
- Hafta 1: **hiç proje kodu yok**

## K-2026-09-12-04 — Hafta 9 (öneri, çalışma varsayımı)

Yeni dil konusu yok. **Pekiştirme + ara uygulama + hafif düzenleme:**

- Hafta 1–8 bilgisiyle zimmet senaryosu: ekipman listesi, basit görev kaydı, menü
- Öğrenci “dağınık Main” ile “metot + sınıf” farkını görür
- Vize öncesi kontrol noktası

Büyük sapma gerekirse kullanıcıya sor.

## K-2026-09-12-05 — Veritabanı haftası (öneri, çalışma varsayımı)

Hafta 14 (generics, LINQ, modern sözdizimi) zaten dolu. ADO.NET’i 14’e sıkıştırmak hem LINQ’i hem veritabanını zayıflatır.

**Varsayılan:** Hafta **15 ek laboratuvar** (model, bağlantı, komut, DataReader, CRUD, null). Stored procedure / transaction ayrıntısı aynı haftada özet + isteğe bağlı ek.

Takvim 14 haftaysa: veritabanı dönem sonu ek oturum veya ek dosya (`docs/Week15`); çekirdek 14 hafta kodu DB olmadan da tamamlanır (Hafta 10 dosya I/O yeter).

## K-2026-09-12-06 — csproj öğretim notu (Hafta 2 uygulandı)

`ImplicitUsings` ve `Nullable` kapatıldı. Öğrenci `using System;` satırını görür. Nullable `?` / `??` öğretilmez.

## K-2026-09-12-07 — Klasörleme (Program.cs yığınağı yok)

Tek çözüm, tek `.csproj`. Haftalık **ayrı proje** veya `Week02/`, `Week03/` kod kopyası **yok** (öğrenci “her hafta sıfırdan program” sanmasın).

`Program.cs` **yalnızca giriş kapısıdır** (`Main`). Dönem boyunca tüm iş oraya yığılmaz.

Hedef klasörler (ihtiyaç ve müfredat açıldıkça, erken OOP yok):

```text
CSharpProgramming/
  Program.cs                 // sadece Main — uygulamayı başlatır
  Presentation/              // menü, konsola yazı (metotlar olgunlaşınca)
  Inventory/                 // ekipman
  Missions/                  // görev
  People/                    // personel
  Data/                      // dosya / ileride veritabanı
  docs/                      // ders notları (kod değil)
  ai/                        // AI bellek (öğrenci ödevi değil)
```

Zorunlu geçiş:

| Haftalar | Kodun durduğu yer | Neden |
| --- | --- | --- |
| 1 | Kod yok | Teori |
| 2–4 | `Program.cs` kısa tutulur | Sınıf/metot henüz yok; ilk program burada doğar |
| 5 | Metotlar ayrı `.cs` dosyalarına | Dosya bir “kutu”dır; sınıfın OOP anlatımı Hafta 6 |
| 6+ | `Inventory/`, `Missions/`, … | Her tür kendi dosyasında |
| 10+ | `Data/` | Kayıt katmanı |

Yasak: 200 satırlık `Main`, her özelliği `Program.cs` içine yapıştırmak, haftalık ikinci `.csproj`.

## Alan modeli (ileride, erken kodlama yok)

Hedef kavramlar (isimler sabit kalsın): `Equipment`, `Mission`, `Staff`; durumlar Müsait / Zimmetli / Bakımda / Kayıp. Kalıtım (`Drone`, `SensorKit`, `Tool`) Hafta 11’den önce yok.
