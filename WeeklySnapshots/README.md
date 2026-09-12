# Haftalık Çalışan Sürüm Arşivi

Bu klasör, HangarDesk uygulamasının her hafta sonunda çalışan sürümünü korur.

- Kök dizindeki `CSharpProgramming.csproj`: dersin **güncel** ana projesi
- `WeeklySnapshots/WeekXX/`: ilgili haftanın bağımsız çalıştırılabilen sürümü
- Yeni hafta başlamadan önce güncel ana proje, tamamlanan haftanın sürümü olarak korunur; ardından ana proje geliştirilmeye devam edilir.

## Çalıştırma

Ana `CSharpProgramming.sln` dosyasını açın. Çözüm Gezgini (Solution Explorer) üzerinden:

1. `WeeklySnapshots` çözüm klasörünü genişletin.
2. İlgili hafta projesine veya güncel `CSharpProgramming` projesine sağ tıklayın.
3. **Başlangıç Projesi Olarak Ayarla (Set as Startup Project)** seçeneğini belirleyin.
4. F5 / Ctrl+F5 ile çalıştırın.

Her haftanın ayrı `.sln` dosyasının açılması gerekli değildir.

Komut satırı alternatifi:

```text
dotnet run --project WeeklySnapshots/Week01/Week01.csproj
dotnet run --project WeeklySnapshots/Week02/Week02.csproj
dotnet run --project WeeklySnapshots/Week03/Week03.csproj
dotnet run --project WeeklySnapshots/Week15/Week15.csproj
```

## Öğretim kuralı

Haftalık sürümler, projenin gelişimini karşılaştırmak amacıyla saklanır. Öğrenci her hafta sıfırdan yeni bir uygulama tasarlamaz; ana HangarDesk uygulaması bir önceki haftanın kodu üzerinden geliştirilir.

Hafta 1 müfredatı teoriktir. `Week01` içindeki `Hello, World!`, boş proje oluşturulduğunda gelen başlangıç şablonunu gösterir; Hafta 1'de yazılmış ders kodu sayılmaz.
