# Proje kararları

Tarih sırasıyla. Çelişen yeni karar eskiyi geçersiz kılmaz; eski maddeyi “geçersiz” işaretle.

## K-2026-09-12-01 — Ürün ve konu

- Görünen ad: **HangarDesk**
- Klasör / `.sln` / varsayılan namespace: **CSharpProgramming** (öğrenci yolunu bozmamak)
- Namespace’i HangarDesk yapmak: Hafta 8 (`namespace`) işlenene kadar yok
- Konu: laboratuvar / hangar / saha **ekipman zimmeti** + **görev kaydı**
- İHA örnekleri varsayılan; diğer bölümler için aynı model (demirbaş + ödünç)

## K-2026-09-12-02 — Teknik taban

- .NET **8** / C# 12, `OutputType` Exe; kökte tek güncel ana proje (haftalık snapshot'lar K-2026-09-12-08)
- .NET 10’a geçiş yok (lab uyumu), kullanıcı istemeden TFM değişmez
- UI: konsol; dönem sonu da konsol (dosya + isteğe bağlı DB)

## K-2026-09-12-03 — Eğitim şekli

- Kök ana proje 14 hafta büyür; haftalık çalışan haller ayrıca snapshot olarak korunur
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

## K-2026-09-12-07 — Klasörleme (Program.cs yığınağı yok) — kısmen geçersiz

`Program.cs`'in giriş kapısı olması ve özellik klasörleri kararı geçerlidir. “Haftalık proje kopyası yok” bölümü K-2026-09-12-08 ile değiştirilmiştir.

Kök dizinde tek güncel ana proje vardır.

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

Yasak: 200 satırlık `Main`, her özelliği `Program.cs` içine yapıştırmak.

## K-2026-09-12-08 — Güncel ana proje + haftalık snapshot

- Kök `CSharpProgramming.csproj`, dersin en güncel çalışan ana projesidir.
- `WeeklySnapshots/WeekXX/`, o haftanın bağımsız çalışan solution/proje kopyasıdır.
- Bütün snapshot projeleri ana `CSharpProgramming.sln` içinde `WeeklySnapshots` solution folder'ında da listelenir; öğretmen sağ tık → **Set as Startup Project** ile hafta seçer.
- Yeni haftaya geçerken önce bir önceki haftanın ana hali snapshot olarak korunur; sonra kök ana proje geliştirilir.
- Snapshot kopyası “haftaya sıfırdan başlamak” değildir; önceki haftadan gelen ilerlemeyi karşılaştırmak içindir.
- Hafta 1 snapshot'ındaki `Hello, World!` boş proje şablonudur; Hafta 1'in teorik ve kodsuz olduğu kararını değiştirmez.
- Kök `.csproj`, snapshot altındaki `.cs` dosyalarını derlemeye dahil etmez.

## K-2026-09-12-09 — Öğrenci dokümanı DOCX

- `docs/WeekXX/` altında her hafta için tek, biçimlendirilmiş `BPP2001-HaftaXX.docx` bulunur.
- `docs/` altında öğrenciye yönelik Markdown dosyası tutulmaz.
- Word belgesinde kapak, ayrıntılı Türkçe anlatım, tablolar, kod blokları, öğretmen notları, uygulamalar ve cevap ipuçları birlikte yer alır.
- `ai/` teknik proje belleği Markdown kalır; bu dosyalar öğrenci dokümanı değildir.

## K-2026-09-12-10 — Dokümantasyon dili

- Bütün öğrenci ve öğretmen belgeleri kurumsal, profesyonel ve akademik Türkçe ile hazırlanır.
- İmla, noktalama ve yazım kurallarına eksiksiz uyulur; devrik veya konuşma diline özgü anlatım kullanılmaz.
- Genel ve belirsiz fiiller yerine bağlama özgü, kesin fiiller tercih edilir.
- Gereksiz yabancı sözcükler ve “plaza dili” kullanılmaz; yerleşik teknik terimler Türkçe açıklamasıyla sunulur.
- Ton ciddi, saygın, anlaşılır ve ikna edicidir.
- Ayrıntılı uygulama ölçütleri `ai/rules/DOCUMENTATION.md` ve `ai/rules/TEACHING.md` dosyalarındadır.

## K-2026-09-12-11 — SQL Server LocalDB ve yapılandırılabilir bağlantı

- 15. hafta SQL Server Express LocalDB kullanır; gerçek Stored Procedure desteği uygulanır.
- Sağlayıcı `Microsoft.Data.SqlClient` 7.0.3'tür.
- Bağlantı dizesi yalnızca `appsettings.json` içinde tutulur; sunucu değişimi kod değişikliği gerektirmez.
- Veritabanı, tablo ve Stored Procedure tanımları uygulama başlangıcında otomatik oluşturulur.
- Windows kimlik doğrulaması kullanılır; parola kaynak kodda tutulmaz.
- SQL Server Express veya uzak SQL Server'a geçiş aynı ADO.NET kodu ve farklı bağlantı dizesiyle gerçekleştirilir.

## Alan modeli (ileride, erken kodlama yok)

Hedef kavramlar (isimler sabit kalsın): `Equipment`, `Mission`, `Staff`; durumlar Müsait / Zimmetli / Bakımda / Kayıp. Kalıtım (`Drone`, `SensorKit`, `Tool`) Hafta 11’den önce yok.

## K-2026-09-12-12 — Hafta-Son uygulaması

- `Hafta-Son/HangarDesk.Final.csproj`, haftalık ders projelerinden bağımsız dönem sonu uygulamasıdır.
- Tek .NET 8 Windows projesi, `--ui=console` ve `--ui=winforms` seçenekleriyle iki arayüz sunar; varsayılan arayüz konsoldur.
- İki arayüz aynı uygulama ve altyapı servislerini kullanır. `Program.cs` yalnızca başlangıç ve arayüz seçimi sorumluluklarını taşır.
- Ayrı `HangarDeskFinalDb` veritabanı, sürümlü SQL betikleri, Stored Procedure'ler ve LocalDB kullanılır.
- İlk çalıştırmada yönetici oluşturulur. Parolalar PBKDF2-SHA256 ile korunur; yönetici, teknik personel ve görüntüleyici rolleri uygulanır.
