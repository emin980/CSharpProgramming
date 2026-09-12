# Ders 1 — Programlama, algoritma ve yazılım süreci

**Süre:** yaklaşık 50 dakika  
**Kod:** yok. Bu derste C# satırı ezberlenmez.

---

## Bu dersi neden okuyorsunuz?

HangarDesk’e henüz bir satır kod yazmayacağız. Yine de bu ders dönemin temelidir. Çünkü bilgisayar sizin **niyetinizi** anlamaz; yalnızca **yazılmış adımları** uygular. Adımlar bulanıksa program ya yanlış çalışır ya da hiç yazılamaz.

Örnek: Hangarda biri “bataryaları biraz kontrol et” der. Bu cümle insana yeter; programa yetmez.

- Hangi batarya?
- Kontrol ne demek — voltaj mı, şişme mi, zimmet kaydı mı?
- Kayıt yoksa ne olacak?
- Bakımdaki batarya “uygun” sayılacak mı?

Programlama, bu soruların cevaplarını **önceden** netleştirmektir. C# o netliği bilgisayarın anlayacağı dile çeviren araçtır. Araç Hafta 2’de gelir; netleştirme bugün başlar.

---

## 1. Programlama nedir?

**Programlama**, bir işi bilgisayarın tekrar tekrar, aynı kurallarla yapabilmesi için o işi **kesin adımlara** dökme işidir.

**Program**, bu adımların bir araya gelmiş halidir. Çalıştırdığınızda program bekler, okur, karar verir, yazar, dosyaya kaydeder — ama yalnızca tarif edildiği kadar.

Günlük hayattan fark:

| İnsan konuşması | Programın ihtiyaç duyduğu kesinlik |
| --- | --- |
| “Bataryaları biraz kontrol et.” | “Seri numarası verilen kaydı bul. Yoksa ‘kayıt yok’ yaz ve dur. Durum Bakımda ise zimmet etme.” |
| “Gerekirse uyar.” | “Teslim tarihi bugünden küçükse ‘gecikme’ yaz.” |
| “Genelde Ahmet’tedir.” | Program “genelde” bilmez. Ya kayıt vardır ya yoktur. |

Üç kelimeyi ayırın:

- **Kaynak kod:** Sizin yazdığınız metin (ileride `.cs` dosyaları). İnsan okur.
- **Derleme:** Bu metnin çalıştırılabilir hale getirilmesi. Nasıl çalıştığı (CLR, IL, JIT) **Hafta 2** konusudur. Bugün yalnızca şunu bilin: yazdığınız metin, sihirle değil, bir dönüşümle programa döner.
- **Çalışan program:** `.exe` gibi çalıştırdığınız sonuç. Kullanıcı kaynak kodu görmek zorunda değildir.

HangarDesk açısından: Siz “zimmet nasıl işler?”i tarif edeceksiniz; derleyici o tarifi çalışır hale getirecek; öğrenci veya teknisyen konsolda menüyü görecek.

---

## 2. Algoritma nedir? Kod mudur?

**Algoritma**, bir problemin **sonlu**, **sıralı** ve **her adımı uygulanabilir** çözüm tarifidir.

Algoritma **kod değildir**. Türkçe maddelerle yazılır. Kod, algoritmanın bir dile (C#) çevrilmiş halidir.

İyi bir algoritmanın özellikleri:

1. **Başlar ve biter.** Sonsuz “bir de şuna bakayım” döngüsü tarifte açıkça yazılmazsa bile tehlikelidir; durma şartı olmalıdır.
2. **Adımlar tek anlamlıdır.** “Gerekirse kontrol et” tek anlamlı değildir. “Durum Müsait değilse zimmetleme” tek anlamlıdır.
3. **Aynı girdi, aynı yol.** Rastgele “bugün öyle geldi” yoktur.
4. **Her dalın sonu bellidir.** “Eğer kayıt yoksa …” dediniz mi, yoksa ne olacağını da yazarsınız.

Akış şeması çizmek zorunlu değildir. Şema bir araçtır. Asıl olan adımların netliğidir.

### Örnek: HangarDesk’te zimmet almak

Aşağıdaki tarif C# değildir. Yine de ileride yazacağımız programın omurgası budur. Hafta 4’te “eğer / değilse”, Hafta 6’da “ekipman nesnesi” bu maddelerin karşılığı olacaktır.

```text
Girdi: personel adı, ekipman seri numarası

1. Personel adını al.
2. Ekipman seri numarasını al.
3. Bu seri numaralı kayıt listede var mı?
   - Yoksa: "Kayıt bulunamadı" yaz ve bitir.
4. Kaydın durumu Müsait mi?
   - Değilse: "Zaten zimmetli veya bakımda; verilemez" yaz ve bitir.
5. Kaydı bu personele bağla.
6. Durumu Zimmetli yap.
7. Teslim / iade tarihini not et.
8. "Zimmet tamam" yaz ve bitir.
```

Bu tarifi sesli okuyun. Hâlâ “biraz”, “belki”, “galiba” geçiyor mu? Geçiyorsa algoritma bitmemiştir.

### Karşı örnek: kötü algoritma

```text
1. Bataryaya bak.
2. Gerekirse ver.
3. Bir şey olursa söyle.
```

Neden kötü?

- “Bak” ölçülebilir değil.
- “Gerekirse” kimin kararı?
- “Bir şey olursa” hangi şey?
- Program ne zaman biter?

---

## 3. Küçük bir yazılım süreci

Büyük şirket süreçlerini ezberlemeniz istenmiyor. HangarDesk ölçeğinde altı adım yeter:

1. **İhtiyaç.** Hangar defteri kayboluyor, Excel dosyası kilitli, “3. batarya kimde?” bağırarak soruluyor.
2. **Kurallar.** Bakımdaki batarya uçuşa verilmez. Kayıp kayıt zimmetlenemez. Aynı seri numara iki kişide olamaz.
3. **Algoritma.** Yukarıdaki gibi, Türkçe adımlar.
4. **Kod.** C# ile yazmak — **Hafta 2’den itibaren**, parça parça.
5. **Dene.** Yanlış seri no, zaten zimmetli alet, boş isim. “Mutlu yol” yetmez.
6. **Düzelt.** Program büyüdükçe dağılan kodu toparlarız (metot, sınıf, klasör — kendi haftalarında).

Dönem boyunca aynı döngüyü küçük ölçekte tekrar edeceksiniz. Yeni bir C# konusu geldiğinde soru şudur: **HangarDesk’te hangi kuralı henüz tarife dökemiyorduk?**

---

## 4. İki hata ailesi (bugün isim olarak)

Programlar iki büyük ailede bozulur. Araçlar Hafta 2 ve Hafta 13’te gelir; fikir bugün lazım.

**Mantık hatası.** Program çalışır, sonuç yanlıştır. Bakımdaki bataryayı zimmetlemiştir çünkü algoritmada o dal yoktu. Derleyici bunu “hata” diye kırmızı çizmez; sizin senaryonuz yakalar.

**Yazım / derleme hatası.** Dilin kurallarına uyulmamıştır; program oluşmaz. Virgül, unutulmuş tırnak, yanlış kelime. Hafta 2’de derleyici mesajını okumayı öğreneceksiniz.

Bugünün hedefi: mantık hatasını **algoritmada** yakalamak. Kod yokken yakalanan hata, en ucuz hatadır.

---

## 5. Kontrol listesi — dersi bitirmeden

Aşağıdakileri bir kâğıda, koda değil, yazın:

- [ ] Programın insan cümlesinden farkını bir örnekle anlattım.
- [ ] Algoritma ile kodun aynı şey olmadığını söyledim.
- [ ] Zimmet algoritmasındaki 3. ve 4. adımların “yoksa / değilse” dallarını savundum.
- [ ] Mantık hatası ile derleme hatasını ayırdım.

Alıştırmalar: [04-sinif-ici.md](04-sinif-ici.md) — özellikle **Alıştırma A** (iade algoritması).

---

## Kısa özet

Bilgisayar niyet okumaz. Önce algoritma, sonra kod. HangarDesk’in ilk ürünü bugün bir `.cs` dosyası değil; zimmetin Türkçe tarifidir.

**Sonraki ders:** Bu tarifi çalıştıracak dil ve ortam — C# ve .NET — nedir? Konsol neden seçildi?
