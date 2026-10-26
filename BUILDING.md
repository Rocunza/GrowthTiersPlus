# Building Growth Tiers Plus (RimWorld 1.6 and 1.7)

The two supported versions use different toolchains. Build them separately. The GitHub repository includes only source code and compiled mod DLLs; it intentionally does not redistribute game assemblies or Harmony binaries.

## RimWorld 1.6

1. Open `Source/1.6/GrowthTiersPlus/GrowthTiersPlus.csproj` with a suitable Visual Studio installation for the .NET Framework 4.7.2 project.
2. Copy `Source/1.6/RimWorld.local.props.example` to `Source/1.6/RimWorld.local.props`.
3. In that private file, set `RimWorldDir` to your RimWorld 1.6 game directory and `HarmonyDllPath` to the version-compatible `0Harmony.dll`.
4. Build. The DLL is created in `Source/1.6/GrowthTiersPlus/bin/<Configuration>/GrowthTiersPlus.dll`.

## RimWorld 1.7

1. Use a .NET SDK / Visual Studio installation capable of targeting **net10.0**.
2. Copy `Source/1.7/RimWorld17.local.props.example` to `Source/1.7/RimWorld17.local.props`.
3. Set `RimWorld17GameDir` to your RimWorld 1.7 game folder, and `Harmony17DllPath` to the version-compatible `0Harmony.dll`. For the game assembly path, you can alternatively set `RimWorld17ManagedDir` directly.
4. Open `Source/1.7/Projects/GrowthTiersPlus/GrowthTiersPlus.csproj` and build. The DLL is created in `Source/1.7/Projects/GrowthTiersPlus/bin/<Configuration>/GrowthTiersPlus.dll`.

## Caution

- Never commit either `*.local.props` file. They contain machine-specific paths.
- Never commit RimWorld game binaries (`Assembly-CSharp.dll`, `Waylith.dll`, `UnityStubs.dll`, or Unity libraries) or Harmony's DLL.
- Debug (`bin/`), intermediate (`obj/`), and Visual Studio (`.vs/`) files are ignored.
- The `1.6/Assemblies/` and `1.7/Assemblies/` DLLs are **intentional release binaries** and should be tracked in Git.
- After rebuilding, test the new binary in its corresponding game version before replacing the prebuilt release DLL.
- The original workspace also contained separate Read & Learn QoL and XP Customizer projects. They are deliberately excluded from this repository.

---

## 한국어 요약

1.6은 **.NET Framework 4.7.2**, 1.7은 **net10.0** 프로젝트입니다. 서로 따로 빌드하세요. 각각의 `.local.props.example`을 `.local.props`로 복사하고 게임 설치 경로와 Harmony DLL 경로를 지정해야 합니다. `.local.props`, 원본 게임 DLL, `bin/`, `obj/`, `.vs/` 파일을 GitHub에 업로드하지 마세요.
