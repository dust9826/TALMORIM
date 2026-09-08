# TALMORIM

던전 크롤링 탑뷰 RPG입니다(2020.05.12 ~ 05.16, 2인). 던전 입장 연출, 보스전 연출, 전투 연출을 HDRP로 제작했습니다.

- 기술 Unity, C#, HDRP, ShaderGraph

![image](https://github.com/user-attachments/assets/51a43995-ba20-475e-8b7a-8b9343a2307d)

![image](https://github.com/user-attachments/assets/5e30c5c6-bbb0-45a7-aca9-fa66a490d2a1)

![image](https://github.com/user-attachments/assets/a04d91da-4702-46dc-a481-e37c3fd1d415)

![image](https://github.com/user-attachments/assets/9e1f57e8-829c-42a7-82b2-afe0164dcead)

## 게임

- **방 단위 던전** — RoomInfo를 기준으로 몬스터를 스폰하고, 문(Door)으로 다음 방으로 이어집니다. 보스 방은 별도 연출을 갖습니다.
- **전투** — Entity를 상속한 플레이어·몬스터·보스, Attackable 인터페이스, 스킬 정보(SkillInfo).
- **키 매핑** — 키 프리셋과 설정 UI(KeyPreset, KeySettingController).
- **연출** — 카메라 컨트롤러와 장애물 처리, HDRP 포스트프로세싱, ShaderGraph와 파티클 이펙트.

## 구조

```text
Assets/Scripts/
  Controller/   Camera, Input, Player, Sound
  Entity/       Entity, Player, Monster, Boss, Attackable, SkillInfo
  KeySet/       KeyPreset, KeySettingController, KeyText
  RoomInfo, EnemySpawner, Door, BossQuestion, SceneController, LoadingSceneManager, TitleManager
```
