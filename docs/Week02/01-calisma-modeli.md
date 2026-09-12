# Ders 1 — .NET çalışma modeli: CLR, CTS, JIT, IL, assembly

**Süre:** yaklaşık 50 dakika  
**Kod:** tahtada kutu çizimi. `Program.cs` bu derste doldurulmak zorunda değildir; fikir oturunca Ders 2’ye geçilir.

---

## Bu dersi neden okuyorsunuz?

Hafta 1’de “kaynak kod derlenir, program çalışır” demiştik. HangarDesk’i F5 ile çalıştırdığınızda aslında birkaç katman devreye girer. Bu katmanları bilmek:

- “C# bilgisayarın ana dili midir?” yanılgısını düzeltir,
- Hata mesajındaki *assembly*, *type* gibi kelimeleri korkutmaz,
- Hafta 3’teki tür dönüşümü ve `object` konuşmasına zemin olur.

---

## 1. Büyük resim (ezberlenecek zincir)

```text
1) Siz C# yazarsınız          (Program.cs — insan okur)
2) Derleyici C# → IL üretir   (ara dil, CPU'ya özel değildir)
3) Çıktı bir assembly olur    (.dll / .exe paketi)
4) CLR assembly'yi yükler     (ortam, güvenlik, tür denetimi)
5) JIT, IL'i o anki işlemci koduna çevirir
6) Program çalışır            (konsolda yazı görürsünüz)
```

C# **doğrudan** işlemciye gitmez. Arada **IL** vardır. Bu yüzden aynı C# metni, .NET’in desteklediği Windows laboratuvarında aynı mantıkla çalışır.

HangarDesk örneği: `int equipmentCount = 0;` satırı sizin tarifinizdir. Derleyici bunu IL komutlarına çevirir. Siz F5’e basınca CLR + JIT o komutları bu bilgisayarın anlayacağı hale getirir. Siz hâlâ C# düşünürsünüz; işlemci IL görmez, makine kodu görür.

---

## 2. IL (Intermediate Language)

**IL**, C# derleyicisinin ürettiği **ara dildir**. “Orta dil” diye düşünün: ne sizin C#’ınız kadar okunaklıdır, ne de tek bir işlemci ailesine kilitlidir.

Neden ara dil?

- Derleyici her öğrencinin işlemcisine özel kod üretmek zorunda kalmaz.
- Çalışma anında JIT, **bu makine** için kod üretir.

Bu derste IL okumak **zorunlu değildir**. Bilmeniz gereken: Visual Studio sizin için bu adımı yapar. İleride merak ederseniz aracılarla IL görülebilir; HangarDesk ödevi değildir.

---

## 3. JIT (Just-In-Time) derleyici

**JIT**, “tam zamanında” derleyicidir. Program **çalışmaya başlarken** (ve ihtiyaç oldukça) IL’i işlemci koduna çevirir.

Karşı fikir (isim yeter): **AOT** (ahead of time) — önceden derleme. Bu dönemde AOT yapılandırmayız.

Öğrenci cümlesi: “C# bir kez IL’e döner; IL, çalışırken JIT ile bu bilgisayarın diline döner.”

---

## 4. CLR (Common Language Runtime)

**CLR**, .NET’in **çalışma ortamıdır**. Görevlerinden bugün üçü yeter:

1. Assembly’yi belleğe yüklemek
2. Türlerin kurallara uyup uymadığını gözetmek (CTS ile)
3. JIT’i yönetmek; işi biten bellek için çöp toplayıcı fikri (GC ayrıntısı Hafta 10)

CLR olmasa IL dosyası tek başına “Windows programı” gibi durmaz. .NET kurulu ortam, o dosyayı **çalıştırır**.

Hafta 1 cümlesini netleştirin: “.NET ortamı” derken somut isim **CLR**’dir.

---

## 5. CTS (Common Type System)

**CTS**, .NET’te **türlerin ortak sözlüğüdür**. `int` sizin C# kelimenizdir; CTS tarafında bu tür `System.Int32` ile aynı ailedendir.

Neden önemli?

- HangarDesk’te ekipman **adedi** tam sayıdır (`int`). Voltaj **ondalıklıdır** (`double`). İsim **yazıdır** (`string`).
- Karıştırmak mantık hatasıdır: “üç batarya”yı yazı gibi tutarsanız sonra toplama yapamazsınız (dönüşüm Hafta 3).
- CTS, “her dil kendi `int`’ini uydursun” kaosunu önler. Biz tek diliz (C#); yine de türler .NET’in ortak sistemine aittir.

`object` (Ders 3): CTS’te her türün üstünde duran ortak kutudur. Ayrıntı ve boxing Hafta 3’e bırakılır; bugün yalnızca “her şey bir türdür, türler bir sistemdedir” denir.

---

## 6. Assembly

**Assembly**, derlenmiş kodun **paketidir**. Laboratuvarda `bin/Debug/net8.0/` altında `.dll` ve çalıştırma ayarlarını görürsünüz.

Basit anlam: “Bu klasör, HangarDesk’in derlenmiş halidir.” Referans eklemek, birden fazla proje — bu dönemin konusu değil. Tek assembly, tek konsol projesi.

Hata mesajında *assembly* görürseniz: “çalışan paket” diye okuyun, panik yok.

---

## 7. Bu katmanlar HangarDesk’te ne işe yarar?

Henüz zimmet yok. Yine de zincir çalışır: `Program.cs` → derle → assembly → CLR → JIT → konsolda başlık.

Sorun (Ders 2’nin gerekçesi): Kaynak kod duruyor ama **çalıştırılmazsa** teknisyen hiçbir şey görmez. Derlemek, algoritmayı gerçeğe çevirmektir.

---

## Kontrol listesi

- [ ] Altı adımlı zinciri (C# → IL → assembly → CLR → JIT → çalışır) sırayla söyledim.
- [ ] C# ile IL’i karıştırmadım.
- [ ] CLR’nin “ortam”, CTS’nin “tür sözlüğü”, assembly’nin “paket” olduğunu birer cümleyle yazdım.

Alıştırmalar: [04-sinif-ici.md](04-sinif-ici.md) Ders 1 soruları.

---

## Kısa özet

C# insan tarifi, IL ara dil, JIT bu makinenin kodu, CLR sahne müdürü, CTS tür kuralları, assembly pakettir. F5 bu zinciri sizin için çalıştırır.

**Sonraki ders:** Zincirin birinci halkasını yazmak — ilk C# programı, `using`, namespace, komut satırı.
