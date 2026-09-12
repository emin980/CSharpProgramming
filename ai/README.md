# AI bellek — dispatcher

Bu klasör, sonraki oturumlarda projenin nerede kaldığını ve hangi C# konusunun serbest olduğunu belirler.

Rastgele yeni Markdown ekleme. Güncelleme gereken dosyayı güncelle.

## Her oturumun başında (zorunlu sıra)

1. Bu dosya (`ai/README.md`)
2. [`state/STATUS.md`](state/STATUS.md) — nerede kaldık, son iş
3. [`state/WEEK.md`](state/WEEK.md) — aktif hafta, tamamlanan haftalar
4. [`DECISIONS.md`](DECISIONS.md) — değiştirilemez kabul edilen kararlar

## İş türüne göre ek oku

| Durum | Oku |
| --- | --- |
| C# kodu yazılacak veya mevcut kod değişecek | [`state/LEARNING.md`](state/LEARNING.md), [`state/LOCKED.md`](state/LOCKED.md), [`rules/CODING.md`](rules/CODING.md), [`rules/TEACHING.md`](rules/TEACHING.md) |
| Haftalık ders dokümanı yazılacak | [`rules/DOCUMENTATION.md`](rules/DOCUMENTATION.md), [`rules/WEEKLY.md`](rules/WEEKLY.md), [`rules/TEACHING.md`](rules/TEACHING.md), [`ROADMAP.md`](ROADMAP.md) |
| Müfredat / sıra değişecek | [`ROADMAP.md`](ROADMAP.md), [`DECISIONS.md`](DECISIONS.md) — büyük değişiklik öncesi kullanıcıya gerekçe sor |
| Sonraki adımı planla | [`TODO.md`](TODO.md), [`ROADMAP.md`](ROADMAP.md) |
| Ne değiştiğini anla | [`CHANGELOG.md`](CHANGELOG.md) |

## Her oturumun sonunda (zorunlu)

1. [`state/STATUS.md`](state/STATUS.md)
2. [`state/WEEK.md`](state/WEEK.md) (hafta ilerlediyse)
3. [`state/LEARNING.md`](state/LEARNING.md) ve [`state/LOCKED.md`](state/LOCKED.md) (yeni konu açıldıysa)
4. [`TODO.md`](TODO.md)
5. [`CHANGELOG.md`](CHANGELOG.md) — kısa, tarihli kayıt
6. Karar alındıysa [`DECISIONS.md`](DECISIONS.md)

## Hızlı cevap haritası

| Soru | Kaynak |
| --- | --- |
| Nerede kaldık? | `state/STATUS.md` |
| Son olarak ne yaptık? | `state/STATUS.md` + `CHANGELOG.md` |
| Hangi haftadayız? | `state/WEEK.md` |
| Öğrenci hangi konuları biliyor? | `state/LEARNING.md` |
| Sonraki adım ne? | `TODO.md` + `ROADMAP.md` |
| Şu an hangi C# özellikleri kullanılabilir? | `LEARNING.md` (açık) / `LOCKED.md` (yasak) |
| Hangi kararları aldık? | `DECISIONS.md` |

## Yasaklar (özet)

- Hafta 1 bitmeden uygulama kodu yazma (`Program.cs` iskeleti hariç, o da değiştirilmez).
- Tüm dönemi `Program.cs` içine yığma; klasör kararı `DECISIONS.md` K-2026-09-12-07.
- `LOCKED.md` içindeki konuları “daha temiz kod” için öne çekme.
- Enterprise katman, DI konteyneri, MVC, mikroservis ekleme.
- Kullanıcı açıkça istemeden git commit / yeni çözüm / ikinci proje.
