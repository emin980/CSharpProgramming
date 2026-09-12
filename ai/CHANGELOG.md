# Değişiklik geçmişi

## 2026-09-12 (Hafta-Son HangarDesk)

- `Hafta-Son/HangarDesk.Final.csproj` ve konsol/Windows Forms arayüz seçimi eklendi
- Sürümlü SQL Server şeması ve Stored Procedure katmanı oluşturuldu
- Güvenli ilk yönetici kurulumu, PBKDF2-SHA256 parola koruması, oturum ve rol denetimi uygulandı
- Personel, ekipman, zimmet/iade, bakım, görev, rapor ve denetim günlüğü işlevleri tamamlandı
- Kritik işlemlere transaction ve arayüzlere rol tabanlı işlem denetimi eklendi
- Temiz LocalDB üzerinde uçtan uca doğrulama senaryosu eklendi

## 2026-09-12 (Hafta 15 — SQL Server)

- SQL Server Express LocalDB seçildi ve `MSSQLLocalDB` bağlantısı doğrulandı
- `Microsoft.Data.SqlClient` 7.0.3 eklendi
- `appsettings.json` ile değiştirilebilir bağlantı dizesi
- Otomatik HangarDeskDb/tablo/Stored Procedure kurulumu
- Stored Procedure tabanlı CRUD, transaction, DataReader, DataAdapter, DataSet ve DBNull uygulaması
- Week15 ana solution'a eklendi ve başarıyla çalıştırıldı
- `docs/Week15/BPP2001-Hafta15-SQLServer-Kurulum.docx` oluşturuldu

## 2026-09-12 (Hafta 4–14 kodları)

- Week04–Week14 bağımsız çalışan sürümler ve solution dosyaları oluşturuldu
- Koşullar/dizilerden OOP, olaylar, generics ve LINQ düzeyine kadar aşamalı kod gelişimi uygulandı
- Tüm hafta projeleri ana solution içindeki `WeeklySnapshots` klasörüne eklendi
- Kök ana proje Week14 sürümüyle eşitlendi
- Hafta 4–14 için doküman oluşturulmadı (kullanıcı kararı)
- 15 proje 0 hata ve 0 uyarı ile derlendi; Week04–Week14 çalışma doğrulaması tamamlandı

## 2026-09-12 (Hafta 3)

- Hafta 3 içeriği doğrudan kurumsal ve akademik Türkçe ile yazıldı; Python yalnızca DOCX biçimlendirmesinde kullanıldı
- `docs/Week03/BPP2001-Hafta03.docx`: dönüşümler, taşma, kutulama, operatörler ve uygulamalar
- Ana `Program.cs`: geçerli kullanıcı girdisi, kapasite hesabı, kutulama, bitsel özellikler ve operatör örnekleri
- `WeeklySnapshots/Week03` bağımsız solution olarak oluşturuldu ve ana solution içine eklendi
- Ana solution dört projeyle 0 hata ve 0 uyarı ile derlendi

## 2026-09-12 (kurumsal Türkçe standardı)

- Dokümantasyon ve öğretim kurallarına kurumsal, profesyonel ve akademik Türkçe standardı eklendi
- Hafta 1, Hafta 2 ve ders notları rehberindeki konuşma diline özgü ifadeler gözden geçirilerek düzeltildi

## 2026-09-12 (DOCX ders notları)

- `docs/Week01/BPP2001-Hafta01.docx`: 1. hafta tek paylaşılabilir Word belgesi
- `docs/Week02/BPP2001-Hafta02.docx`: 2. hafta tek paylaşılabilir Word belgesi
- `docs/BPP2001-Ders-Notlari-Rehberi.docx`: genel okuma rehberi
- `docs/` altındaki Markdown ders dosyaları kaldırıldı; `ai/` Markdown belleği korundu

## 2026-09-12 (haftalık snapshot düzeltmesi)

- Kök ana proje en güncel haftayı göstermeye devam ediyor
- `WeeklySnapshots/Week01`: başlangıç `Hello, World!` şablonu ve bağımsız solution
- `WeeklySnapshots/Week02`: ilk HangarDesk / veri türleri ve bağımsız solution
- Week01 ve Week02 projeleri ana `CSharpProgramming.sln` içindeki `WeeklySnapshots` solution folder'ına eklendi; startup project seçilebilir
- Kök `.csproj`, snapshot kaynaklarını kendi derlemesinden hariç tutuyor
- Dokümantasyon ve AI kuralları ana proje + haftalık snapshot modeline göre düzeltildi

## 2026-09-12 (Hafta 2)

- `docs/Week02/` paylaşılabilir ders seti
- `ImplicitUsings` ve `Nullable` kapatıldı; `using System;` açık
- `Program.cs`: HangarDesk karşılama, temel türler, `int` kopya deneyi (menü/zimmet/`if` yok)

## 2026-09-12 (Hafta 1 not genişletme)

- Hafta 1 paylaşılabilir not; `docs/README.md`
- Karar K-2026-09-12-07 klasörleme

## 2026-09-12 (ilk)

- HangarDesk, .NET 8, `ai/` bellek, Hafta 1 teori
