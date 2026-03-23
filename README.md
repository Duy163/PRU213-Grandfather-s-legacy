# Quiet Sea 3D — Grandfather's Legacy

Dự án game 3D Unity với chủ đề biển cả, đánh bắt cá và kế thừa di sản từ ông. Người chơi điều khiển thuyền, tương tác với NPC, hoàn thành nhiệm vụ và khám phá các hòn đảo.

---

## 📋 Mô tả

**Quiet Sea 3D** là game phiêu lưu mở thế giới trên biển. Bạn đóng vai người thừa kế chiếc thuyền và cuộc sống ven biển của ông, thực hiện các nhiệm vụ, câu cá, giao dịch và tìm hiểu câu chuyện qua các nhân vật trên đảo.

---

## ✨ Tính năng chính

- **Điều khiển thuyền**: Lái thuyền trên biển với vật lý thực (gia tốc, lực cản, xoay). Có đèn pin (lamp) để bật/tắt.
- **Hệ thống nhiệm vụ (Quest)**: Nhiệm vụ chính và phụ với nhiều bước: nói chuyện NPC, đến vùng chỉ định, kiểm tra túi đồ, kiểm tra tiền. Nhiệm vụ có điều kiện mở khóa và chuỗi quest kế tiếp.
- **NPC & cốt truyện**: Các nhân vật như Tuan, Duy, Minh, Huy với hội thoại và trigger riêng, tích hợp với Story Database.
- **Túi đồ & kinh tế**: Inventory (túi đồ người chơi, cargo), quản lý tiền tệ (CurrencyManager), vật phẩm cá (Mackerel, Tuna, Cod...) và cơ chế mua bán (Shop).
- **Lưu tiến trình**: Lưu dữ liệu thuyền, túi đồ, kinh tế, thế giới và trạng thái nhiệm vụ (DataManager, GameData).
- **Thế giới**: Đảo (North Island, Quiet Island...), địa hình, nhà cửa (House, Shop), cây cối, đá; shader nước (Water, Vortex) và Skybox.

---

## 🛠 Công nghệ

- **Engine**: Unity (C#)
- **Render**: URP (Universal Render Pipeline)
- **UI**: TextMesh Pro
- **Cấu trúc**: Scriptable Objects (GameData, Quest, Dialogue, Items), Singleton managers (QuestManager, InventoryManager, DataManager, CurrencyManager...)

---

## 📁 Cấu trúc thư mục (chính)

```
Assets/
├── _Script/           # Script C#
│   ├── Controller/    # ShipController, UIController, ...
│   ├── Managers/      # QuestManager, InventoryManager, CameraManager, ...
│   ├── View/          # UI panels (SettingView, TimeView, EndingView, ...)
│   ├── Event/         # InputEvent, StoryEvent, TimeEvent
│   ├── Scriptables/   # GameData, LightingPreset
│   └── Unit/          # ShopInteractable, ...
├── Prefabs/           # Story (Quest, Dialogue, NPC), Item, UI
├── Resources/         # Model (Terrain, Trees, House, ...), Sprite, Audio
├── Shader/            # ShaderGraph (Water, Skybox, Vortex)
├── Materials/
├── Audios/
└── TextMesh Pro/
```

---

## 🚀 Cách chạy dự án

1. Mở project bằng **Unity Hub** (phiên bản Unity tương thích với project).
2. Mở solution `Quiet Sea 3d.slnx` hoặc mở trực tiếp thư mục project trong Unity.
3. Mở scene chính và nhấn **Play** trong Editor.

---

## 📄 Giấy phép & Đóng góp

Dự án thuộc môn **PRU213** (Grandfather's Legacy). Mọi đóng góp và chỉnh sửa nên tuân theo quy định của môn học và repository.

---

*README được tạo để mô tả tổng quan dự án Quiet Sea 3D — Grandfather's Legacy.*
