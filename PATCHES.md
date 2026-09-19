# Patches applied to the AssetRipper export (Unity 4.5.5 → 2022.3.62f3)

Only *our* code is in this repo. The game's own decompiled scripts stay in the local Unity project; the changes to them are described here so they can be re-applied to a fresh export.

| Area | Change | Why |
|---|---|---|
| `Assets/Shader/*.shader`, `Assets/Resources/BlendVertexColor.shader` | Real tk2d shaders (files in this repo). `BlendVertexColor.shader` is overwritten **in place** so its GUID (referenced by every atlas material) is kept. | The export shipped placeholder shaders → sprites rendered as flat quads. |
| `Assets/Shader/DepthMask.shader` + `Assets/Resources/BlendVertexColorMasked.shader` + `ScrollMaskFix.cs` | tk2d depth masks became stencil masks; `ScrollMaskFix.Attach(content)` is called in `ShopView.showShop()` right after `content` is resolved. | On Unity 2022 the depth masks drew solid black bars over the tavern's title bar, description and background. |
| `Rb2DCompat.cs` | `Freeze(rb)` helper (isKinematic + zero velocity) used where the game froze coins/bodies. | Unity 2022 kinematic bodies keep their velocity → coins fell through the floor. |
| `AbstractBehavior.Start()` | After `anim = GetComponent<tk2dSpriteAnimator>()`, nudge z by `0.0001f * ((counter++ % 400) + 1)` before `onStart()`. | Unity 2022 re-sorts equal-z sprites every frame → overlapping slimes flickered. |
| AdColony plugin | `AndroidInitializePlugin()` returns early; every Android plugin call wrapped in try/catch + `AndroidJNI.ExceptionClear()`. | JNI abort on launch (dead ad SDK). |
| `PrefsManager.getFamePoints()` / `PlayerModel` spend | Under `#if SLAYIN_CHEAT`: return 999999 / skip the debit. `getSoldItem()` is NOT forced (that made every shop slot "SOLD OUT"). | The "Unlimited" build. |
| `Assets/Editor/SlayinBuild.cs` | Batch build: IL2CPP, ARM64, targetSdk 34, original icon, same package/title for both variants; `SLAYIN_VARIANT=unlimited` toggles the define + output name. | Reproducible builds from the command line. |
