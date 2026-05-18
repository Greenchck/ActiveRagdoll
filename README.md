
<img width="1640" height="917" alt="Ekran görüntüsü 2026-05-18 163536" src="https://github.com/user-attachments/assets/3fa30a80-b0f9-4cf4-b0fa-7fe101ea5487" />

# Unity Active Ragdoll Tutorial / Kurulum Rehberi




*(English version is below)*

## 🇹🇷 Türkçe Kurulum Rehberi

Bu proje, Unity'de Human: Fall Flat veya Gang Beasts tarzı kütleden bağımsız, sarsılmaz bir Active Ragdoll sistemi kurmanızı sağlar.

### 1. Hazırlık ve İki İskelet Sistemi
Active Ragdoll sistemleri iki karakter kopyası ile çalışır: Biri sadece animasyon oynatır (Hayalet), diğeri ise onu fiziksel olarak taklit eder (Ragdoll).

1. Karakterinizi sahneye koyun.
2. Karakteri kopyalayın (Ctrl+D). 
3. Kopyanın adını **"Animated Target"** (Hayalet), diğerini **"Physical Ragdoll"** yapın.

### 2. Katman (Layer) Çarpışmalarını Kapatmak
Fiziksel kolların, hayalet karaktere çarpıp uzaya uçmasını engellemeliyiz.
1. Unity'den iki yeni Layer oluşturun: `Ghost` ve `Ragdoll`.
2. **Animated Target** objesine `Ghost`, **Physical Ragdoll** objesine `Ragdoll` katmanını verin.
3. `Edit > Project Settings > Physics` sekmesini açın.
4. En alttaki Çarpışma Matrisinden (Layer Collision Matrix) **Ghost ile Ragdoll**'un kesiştiği tiki kaldırın. (İsteğe bağlı olarak Ragdoll ile Ragdoll tikini de kaldırabilirsiniz).

### 3. Animated Target (Hayalet) Ayarları
Bu obje oyuncunun tuşlarla kontrol ettiği asıl görünmez merkezdir.
1. Üzerindeki tüm Rigidbody, Joint ve kemik Collider'larını silin. (Sadece Animator kalsın).
2. Objenin ana gövdesine bir **Capsule Collider** ve bir **Rigidbody** ekleyin.
3. Rigidbody içindeki `Constraints` kısmından **Freeze Rotation X, Y, Z** tiklerini işaretleyin (Karakter devrilmesin diye).
4. `ActiveRagdollPlayerController` scriptini bu objeye atın. (Animator'de "Speed" adında bir Float parametreniz olmalı).

### 4. Physical Ragdoll Ayarları
Bu obje oyuncunun ekranda gördüğü sarsak fiziksel bedendir.
1. Unity'nin üst menüsünden `GameObject > 3D Object > Ragdoll...` aracını kullanarak karakterinize standart bir ragdoll ekleyin.
2. Animator bileşenini **silin** veya kapatın.

### 5. Joint Converter Aracını Kullanmak (Kritik Adım)
Unity'nin varsayılan `Character Joint`leri Active Ragdoll için uygun değildir.
1. Hiyerarşiden **Physical Ragdoll** objenizi seçin.
2. Unity üst menüsünden **Active-Ragdoll > Convert Character Joints to Configurable Joints** seçeneğine tıklayın.
3. Konsolda başarı mesajını göreceksiniz. Tüm kemikler kas gücüyle çalışmaya hazır hale geldi!

### 6. Scriptleri Bağlamak
1. `ActiveRagdollController` scriptini **Physical Ragdoll** objenize ekleyin.
2. Scriptin içindeki boşluklara şunları sürükleyin:
   - **Animated Target:** Hayalet karakterin ana objesi.
   - **Physical Hips:** Fiziksel ragdoll'unuzun leğen kemiği (Hips/Pelvis).
   - **Animated Hips:** Hayalet karakterinizin leğen kemiği.

**Hazırsınız!** Play'e basın ve WASD ile karakterinizi kontrol edin. `Head Spring Multiplier` değeriyle kafanın sallanma (Bobblehead) oranını değiştirebilirsiniz.

---

https://github.com/user-attachments/assets/44c66b12-f698-4a4c-90bb-18b40e5e08bd


https://github.com/user-attachments/assets/893282d2-7d76-47e1-8177-1b09a0582198

## 🇬🇧 English Setup Tutorial

This project provides a mass-independent, jitter-free Active Ragdoll system for Unity, inspired by games like Human: Fall Flat and Gang Beasts.

### 1. Preparation & The Two-Skeleton System
Active Ragdoll systems require two character copies: One purely plays animations (Ghost), and the other physically mimics it (Ragdoll).

1. Place your character model in the scene.
2. Duplicate the character (Ctrl+D).
3. Name the duplicate **"Animated Target"** (Ghost) and the original **"Physical Ragdoll"**.

### 2. Setting Up Layer Collisions
We must prevent the physical limbs from colliding with the ghost controller and launching into space.
1. Create two new Layers in Unity: `Ghost` and `Ragdoll`.
2. Assign the `Ghost` layer to the **Animated Target**, and the `Ragdoll` layer to the **Physical Ragdoll**.
3. Open `Edit > Project Settings > Physics`.
4. In the Layer Collision Matrix at the bottom, **uncheck the box where Ghost and Ragdoll intersect**. (Optionally, uncheck Ragdoll vs Ragdoll to prevent limbs from fighting each other).

### 3. Animated Target (Ghost) Setup
This object acts as your actual player controller.
1. Remove any Rigidbodies, Joints, or bone Colliders from this object. (Keep the Animator).
2. Add a single **Capsule Collider** and a **Rigidbody** to the root object.
3. In the Rigidbody, go to `Constraints` and check **Freeze Rotation X, Y, Z** (so the character doesn't tip over).
4. Attach the `ActiveRagdollPlayerController` script to this object. (Ensure your Animator has a Float parameter named "Speed").

### 4. Physical Ragdoll Setup
This is the visible, wobbly physical body.
1. Use Unity's built-in tool via `GameObject > 3D Object > Ragdoll...` to add a standard ragdoll to your character.
2. **Remove or disable** the Animator component on this object.

### 5. Using the Joint Converter Tool (Critical Step)
Unity's default `Character Joints` do not support motor drives properly. We need `Configurable Joints`.
1. Select your **Physical Ragdoll** object in the Hierarchy.
2. From the top Unity menu, click **Active-Ragdoll > Convert Character Joints to Configurable Joints**.
3. You will see a success message in the console. Your bones are now ready to be driven by muscle springs!

### 6. Connecting the Scripts
1. Attach the `ActiveRagdollController` script to your **Physical Ragdoll** object.
2. Drag and drop the following references into the script slots:
   - **Animated Target:** The root object of your Ghost character.
   - **Physical Hips:** The Hips/Pelvis bone of your Physical Ragdoll.
   - **Animated Hips:** The Hips/Pelvis bone of your Ghost character.

**You are ready!** Press Play and use WASD to move. You can adjust the `Head Spring Multiplier` in the controller script to tweak the bobblehead effect!
