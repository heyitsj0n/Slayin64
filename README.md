# Slayin64

Personal arm64 rebuild of **Slayin** (FDG Entertainment, 2013) for a 64-bit-only Pixel 8 Pro. The original Android APK (2.0.13) shipped only `armeabi-v7a` and cannot run on phones without 32-bit support.

**Private repo, personal use only.** Slayin is © FDG Entertainment / Pixelicious; this repo holds *our* build scripts, compatibility patches and the resulting APKs for a game we own, and nothing here is licensed for redistribution. The Unity project itself (an AssetRipper export of the original APK) is not in the repo — it lives at `C:\Users\jonal\Downloads\SlayinPort` and in `Software\Backups`.

## Builds (see Releases)
- `Slayin64-arm64.apk` — faithful game (kept on the laptop).
- `Slayin64-Unlimited-arm64.apk` — same package/title (`com.heyitsj0n.slayin64` / "Slayin64") with the `SLAYIN_CHEAT` define: 999,999 fame points and no debit. This is the one installed on the phone.

Both are debug-signed by Unity; install with `pm install -r` (same signature, so either replaces the other).

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
