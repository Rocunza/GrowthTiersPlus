# Growth Tiers Plus

A RimWorld mod by **ZCN**. Adds customizable **growth tiers 9 and 10** while leaving vanilla **tiers 1–8 unchanged**.

**Supported game versions:** RimWorld 1.6 and 1.7  
**Requires:** Biotech DLC and Harmony mod

## Features

- Adds growth tiers 9 and 10 for children.
- Configurable growth-point requirements.
- Configurable minimum and maximum passion gains, passion options, and trait choices.
- Vanilla growth tiers 1–8 remain untouched and cannot be configured by this mod.

## Installation

1. Install and enable Harmony and Biotech.
2. Put the mod in your RimWorld `Mods` directory (the folder must contain `About/About.xml`).
3. Enable **Growth Tiers Plus** in the game's mod list and restart if requested.
4. RimWorld selects the matching `1.6/Assemblies/` or `1.7/Assemblies/` automatically.

## Repository layout

- `About/`: mod metadata and preview image.
- `Languages/`: English and Korean settings localization.
- `1.6/Assemblies/`, `1.7/Assemblies/`: compiled game-version-specific DLLs.
- `Source/1.6/`, `Source/1.7/`: C# source and build project for each supported version.
- `BUILDING.md`: build instructions for contributors.

The compiled DLLs in this repository were taken from the supplied, tested distribution archive. They have not been rebuilt as part of GitHub packaging.

## License

See [`LICENSE`](LICENSE) for the MIT License. The repository is not affiliated with or endorsed by Ludeon Studios. RimWorld and its assets belong to their respective owners.

---

## 한국어 안내

**Growth Tiers Plus**는 아동 성장 티어 **9와 10**을 추가합니다. 기존 바닐라 성장 티어 **1~8은 변경하지 않습니다.**

- 지원 버전: RimWorld 1.6, 1.7
- 필요 항목: Biotech DLC, Harmony 모드
- 설정 가능: 필요 성장 점수, 최소·최대 열정 획득 개수, 열정 선택지 개수, 특성 선택지 개수
- 설치: `About` 폴더가 보이도록 모드를 `Mods`에 넣고 게임의 모드 목록에서 활성화합니다.

이 저장소에는 실행 DLL과 버전별 C# 소스 프로젝트가 함께 있습니다. 상세 빌드 안내는 [`BUILDING.md`](BUILDING.md)를 확인하세요.
