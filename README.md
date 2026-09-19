# Slayin64

Personal arm64 rebuild of **Slayin** (FDG Entertainment, 2013) for a 64-bit-only Pixel 8 Pro. The original Android APK (2.0.13) shipped only `armeabi-v7a` and cannot run on phones without 32-bit support.

**Tooling only — bring your own copy of the game.** Slayin is © FDG Entertainment / Pixelicious. This repo contains only our build script, compatibility patches and notes; no game code, assets or APKs are distributed here. You need your own legitimately purchased Slayin 2.0.13 APK (the Unity project is an AssetRipper export of it, kept locally).

## Builds
No APKs are published here. Our own builds live in a private repo.

## How it was made
1. AssetRipper 2.0 export of the original APK (Unity 4.5.5) → opened in **Unity 2022.3.62f3**.
2. Fixes needed on a modern engine (all in `unity/`, details in `PATCHES.md`):
   - real tk2d shaders (the export only had placeholders) — `Shader/`, `Resources/BlendVertexColor.shader` overwritten in place to keep its GUID;
   - `Rb2DCompat.cs`: kinematic `Rigidbody2D` velocity semantics changed → coins fell through the floor;
   - `AbstractBehavior` z tie-break: Unity 2022 re-sorts equal-z sprites every frame → overlapping slimes flickered;
   - `ScrollMaskFix.cs` + stencil `DepthMask.shader`: tk2d's depth masks drew solid black bars over the tavern;
   - AdColony JNI init stubbed and 169 plugin calls wrapped (JNI abort on launch);
   - IL2CPP / ARM64 / targetSdk 34, original icon, `SlayinBuild.cs` batch build.
3. `scripts/build_both.bat` builds both variants: `Unity.exe -batchmode -nographics -executeMethod SlayinBuild.Android`, env `SLAYIN_VARIANT=unlimited` for the cheat.

Toolchain: JDK 11 + NDK r23b in `C:\Users\jonal\AndroidUnity`, SDK root `C:\Users\jonal\AndroidUnity\SDK`.
