# 📦 DropMerger — Unturned Resource Drop To Player

> ของทรัพยากรที่ตกพื้น (ตัดไม้/ขุดแร่) จะถูก **ดรอปที่ตัวผู้เล่น** ที่อยู่ใกล้สุด แทนที่จะกระจายเต็มพื้น — เก็บง่าย ลด item entity / lag
> Drops configured resource items at the nearby player instead of scattering them.

![Game](https://img.shields.io/badge/game-Unturned-2f9e44)
![Framework](https://img.shields.io/badge/framework-RocketMod-blue)

## How it works
ดักของที่กำลังจะตก (`ItemManager.onServerSpawningItemDrop`) ถ้าเป็น id ทรัพยากรที่ตั้งไว้ และมีผู้เล่นอยู่ในระยะ `MaxPlayerDistance` → ย้ายจุดตกไปที่ **เท้าผู้เล่นที่ใกล้สุด** → ของจากการตัดไม้/ขุดแร่จะมากองที่ตัวคนเก็บเอง

## Install
1. วาง `bin/DropMerger.dll` ไว้ที่ `Rocket/Plugins/DropMerger/`
2. สตาร์ทเซิร์ฟ 1 ครั้ง → จะได้ `DropMerger.configuration.xml`
3. หา id ทรัพยากร: ถือไอเทมแล้วพิมพ์ **`/itemid`** → เอา id ไปใส่ `ResourceItemIds`
4. `/dmreload` (หรือรีสตาร์ท)

## Commands
| Command | Permission | หน้าที่ |
|---------|------------|---------|
| `/itemid` | `dropmerger.itemid` | บอก id ของไอเทมที่ถืออยู่ |
| `/dmreload` (`/dropmergerreload`) | `dropmerger.reload` | โหลด config + id ใหม่ |

## Config (`DropMerger.configuration.xml`)
| Field | Default | ความหมาย |
|-------|---------|----------|
| `ResourceItemIds` | (ว่าง) | id ของที่จะดรอปที่ผู้เล่น — `<Id>xxxx</Id>` หลายอันได้ |
| `MaxPlayerDistance` | `16` | ย้ายจุดตกเฉพาะเมื่อมีผู้เล่นอยู่ในระยะนี้ (กันของกลางป่าโดนย้าย) |
| `PileJitter` | `0.15` | สุ่มกระจายเล็กน้อยที่เท้าผู้เล่น กันของซ้อนทับเป๊ะจนเด้ง |

ตัวอย่าง:
```xml
<ResourceItemIds>
  <Id>81</Id>   <!-- ใส่ id จริงจาก /itemid -->
  <Id>82</Id>
</ResourceItemIds>
<MaxPlayerDistance>16</MaxPlayerDistance>
<PileJitter>0.15</PileJitter>
```

## Notes
- ของจะมากองที่ **เท้าผู้เล่นที่ใกล้จุดตกที่สุด** — ถ้าหลายคนอยู่ใกล้กัน ของไปที่คนใกล้สุด
- ของบนพื้นยังเป็นชิ้น ๆ แยกกัน (Unturned ไม่รองรับ stack กองเดียวจริง) แต่จะกองที่ตัวคนเก็บ
- ถ้าไม่มีผู้เล่นในระยะ `MaxPlayerDistance` ของจะตกตามปกติ
- ถ้าอยากให้ "เก็บเข้ากระเป๋าอัตโนมัติ" แทนการกองที่เท้า แจ้งได้ เพิ่มโหมดให้ได้

Built by imaximum.tech.
