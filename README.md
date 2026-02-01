# Interaction System - Berkay Kurt
Ludu Arts Unity Developer Intern Case

## Proje Bilgileri
| Bilgi | Değer |
|-------|-------|
| Unity Versiyonu | 6000.0.6f1 (Unity 6 Preview) |
| Render Pipeline | URP (Universal Render Pipeline) |
| Case Süresi | 12 saat |
| Tamamlanma Oranı | %100 |

## Kurulum
1. Repository'yi klonlayın:
   ```bash
   git clone https://github.com/[username]/[repo-name].git
   ```
2. Unity Hub'da projeyi açın (Unity 6 kullanılması önerilir).
3. `Assets/InteractionSystem/Scenes/TestScene.unity` sahnesini açın.
4. Play tuşuna basın.

## Nasıl Test Edilir

### Kontroller
| Tuş | Aksiyon |
|-----|---------|
| WASD | Hareket |
| Mouse | Bakış yönü |
| E | Etkileşim (Bas/Basılı Tut) |
| I | Envanter |
| ESC | Çıkış / Pause |

### Test Senaryoları
1. **Door Test**:
   - Kapıya yaklaşın, "Press E to Open" mesajını görün.
   - E'ye basın, kapı açılsın.
   - Tekrar basın, kapı kapansın.

2. **Key + Locked Door Test**:
   - Kilitli kapıya yaklaşın, "Locked - Key Required" mesajını görün.
   - Anahtarı bulun ve toplayın (E ile).
   - Envanterde (I) anahtarı görün.
   - Kilitli kapıya geri dönün, E ile etkileşime girince kilit açılsın.

3. **Switch Test**:
   - Switch'e yaklaşın ve E'ye basın.
   - Switch animasyonu oynasın ve bağlı olduğu olay tetiklensin.

4. **Chest Test**:
   - Sandığa yaklaşın.
   - E'ye basılı tutun, progress bar dolsun.
   - Sandık açılsın ve içindeki item envantere eklensin.

## Mimari Kararlar

### Interaction System Yapısı
Sistem, `IInteractable` arayüzü ve `InteractionDetector` sınıfı üzerine kuruludur.
- **IInteractable**: Tüm etkileşilebilir nesnelerin uyguladığı temel arayüz.
- **BaseInteractable**: Ortak özellikleri (Prompt text, Range kontrolü, Animation trigger) barındıran soyut sınıf.
- **InteractionDetector**: Oyuncu üzerindeki bu script, `Physics.OverlapSphere` ile çevredeki `IInteractable` nesneleri tarar ve en yakın olana odaklanır.
- **Managers**: `GameManager` oyun akışını yönetir (Pause/Resume).

#### Neden bu yapıyı seçtim:
Bu yapı, **SOLID** prensiplerine uygun olarak genişletilebilirliği sağlar. Yeni bir etkileşim türü eklemek için `BaseInteractable` sınıfından türetmek yeterlidir. Algılama mantığı ile etkileşim mantığı ayrılmıştır.

#### Alternatifler:
- **Raycast**: Crosshair tam üzerine gelmeli. Third person veya geniş alanlı etkileşimlerde zorluk çıkarabilir diye **OverlapSphere** tercih edildi.
- **OnTriggerEnter**: Fizik motoruna çok bağımlı ve her objeye trigger collider eklemek gerekir. Merkezi kontrol (Detector) daha temiz bir yapı sundu.

#### Trade-off'lar:
- **Avantaj**: Modüler, bakımı kolay, yeni özellik eklemek hızlı.
- **Dezavantaj**: Her frame'de `OverlapSphere` (veya belirli aralıklarla) çağırmak Raycast'e göre biraz daha maliyetli olabilir ancak tek bir karakter için önemsizdir.

### Kullanılan Design Patterns
| Pattern | Kullanım Yeri | Neden |
|---------|---------------|-------|
| **Observer** | `InteractionDetector` events, `InventoryUI` | UI ve mantık katmanını ayırmak (Decoupling) için kullanıldı. UI, eventleri dinleyerek güncellenir. |
| **Singleton** | `GameManager` | Global oyun durumuna (Pause/Resume) her yerden erişim sağlamak için. |
| **Template Method** | `BaseInteractable` | Ortak mantığı (Interact check, event firing) ana sınıfta tutup, özelleşen mantığı alt sınıflara (Door, Chest) bırakmak için. |

## Ludu Arts Standartlarına Uyum

### C# Coding Conventions
| Kural | Uygulandı | Notlar |
|-------|-----------|--------|
| m_ prefix (private fields) | [x] | Tüm scriptlerde uygulandı. |
| s_ prefix (private static) | [x] | Gerekli yerlerde uygulandı. |
| k_ prefix (private const) | [x] | Sabit metinler ve değerler için kullanıldı. |
| Region kullanımı | [x] | Fields, Properties, Unity Methods, Public Methods şeklinde ayrıldı. |
| XML documentation | [x] | Public API ve önemli metodlar dokümante edildi. |
| Explicit interface impl. | [x] | Gerektiğinde kullanıldı. |

### Naming Convention
| Kural | Uygulandı | Örnekler |
|-------|-----------|----------|
| P_ prefix (Prefab) | [x] | `P_Door`, `P_Chest`, `P_Key` |
| M_ prefix (Material) | [x] | `M_Door`, `M_Chest` |
| SO isimlendirme | [x] | `ItemData` scriptable objectleri. |

## Tamamlanan Özellikler

### Zorunlu (Must Have)
- [x] **Core Interaction System**
  - [x] IInteractable interface
  - [x] InteractionDetector
  - [x] Range kontrolü ve Görselleştirme (Gizmos)
  - [x] Interaction Types (Instant, Hold, Toggle)

- [x] **Interactable Objects**
  - [x] Door (Locked/Unlocked, Animation)
  - [x] Key Pickup (Inventory integration)
  - [x] Switch/Lever (Toggle logic)
  - [x] Chest/Container (Hold to open)

- [x] **UI Feedback**
  - [x] Interaction prompt (Dinamik metin)
  - [x] Hold progress bar
  - [x] Cannot interact feedback (Locked door message)

- [x] **Simple Inventory**
  - [x] Key toplama ve kullanma
  - [x] UI listesi ve Item count

### Bonus (Nice to Have)
- [x] Animation entegrasyonu (Animator controller ile)
- [x] Multiple keys support (Inventory list yapısı sayesinde)
- [ ] Sound effects
- [ ] Save/Load states

## Dosya Yapısı
```
Assets/
├── InteractionSystem/
│   ├── Scripts/
│   │   ├── Runtime/
│   │   │   ├── Core/           # IInteractable, BaseInteractable
│   │   │   ├── Interactables/  # Door, Chest, KeyPickup, Switch
│   │   │   ├── Player/         # InteractionDetector, PlayerController
│   │   │   ├── UI/             # InventoryUI, InteractionPromptUI
│   │   │   └── Manager/        # GameManager
│   │   └── Editor/
│   ├── ScriptableObjects/      # ItemData definitions
│   ├── Prefabs/                # P_Door, P_Chest, P_Player...
│   ├── Materials/              # M_Prototyping...
│   └── Scenes/
│       └── TestScene.unity
├── Settings/                   # URP Settings
├── Docs/
├── README.md
└── .gitignore
```

## İletişim
| Bilgi | Değer |
|-------|-------|
| Ad Soyad | Berkay Kurt |
| GitHub | [github.com/berkaykurt](https://github.com/berkaykurt) |

---
Bu proje Ludu Arts Unity Developer Intern Case için hazırlanmıştır.
