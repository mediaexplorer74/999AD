
// Type: GameManager.PlatformsManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class PlatformsManager
  {
    public static PlatformsRoomManager[] platformsRoomManagers;
    public static int platformIndex;

    public static void Inizialize(Texture2D _spritesheet)
    {
      MovingPlatform.loadTextures(_spritesheet);
      PlatformsManager.platformsRoomManagers = new PlatformsRoomManager[18]
      {
        new PlatformsRoomManager(new MovingPlatform[5]
        {
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(172f, 180f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(172f, 220f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 40, new Vector2(632f, 160f), new Vector2(0.0f, 0.0f), 1f, 0.0f),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 40, new Vector2(632f, 160f), new Vector2(0.0f, 0.0f), 1f, 0.0f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 5, new Vector2(856f, 181f), new Vector2(720f, 181f), 3f, 70f)
        }),
        new PlatformsRoomManager(new MovingPlatform[5]
        {
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(180f, 112f), new Vector2(180f, 220f), 0.0f, 50f),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(252f, 112f), new Vector2(252f, 220f), 0.0f, 50f, _normalizedLinearProression: 0.2f),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(324f, 112f), new Vector2(324f, 220f), 0.0f, 50f, _normalizedLinearProression: 0.4f),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(396f, 112f), new Vector2(396f, 220f), 0.0f, 50f, _normalizedLinearProression: 0.6f),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(468f, 112f), new Vector2(468f, 220f), 0.0f, 50f, _normalizedLinearProression: 0.8f)
        }),
        new PlatformsRoomManager(new MovingPlatform[0]),
        new PlatformsRoomManager(new MovingPlatform[0]),
        new PlatformsRoomManager(new MovingPlatform[2]
        {
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(172f, 132f), new Vector2(172f, 20f), 0.0f, 50f),
          new MovingPlatform(MovingPlatform.TextureType.grass40_8, 0, new Vector2(28f, 220f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false)
        }),
        new PlatformsRoomManager(new MovingPlatform[12]
        {
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(180f, (float) (MapsManager.maps[5].RoomHeightPx - 492)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 3f, _maxSolidTime: 3f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(256f, (float) (MapsManager.maps[5].RoomHeightPx - 540)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 3f, _maxSolidTime: 3f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(332f, (float) (MapsManager.maps[5].RoomHeightPx - 588)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 3f, _maxSolidTime: 3f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(358f, (float) (MapsManager.maps[5].RoomHeightPx - 690)), new Vector2(278f, (float) (MapsManager.maps[5].RoomHeightPx - 690)), 0.0f, 80f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(222f, (float) (MapsManager.maps[5].RoomHeightPx - 690)), new Vector2(142f, (float) (MapsManager.maps[5].RoomHeightPx - 690)), 0.0f, 80f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(304f, (float) (MapsManager.maps[5].RoomHeightPx - 828)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 0.8f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(412f, (float) (MapsManager.maps[5].RoomHeightPx - 828)), new Vector2(412f, (float) (MapsManager.maps[5].RoomHeightPx - 978)), 0.0f, 150f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(468f, 900f), new Vector2(468f, 700f), 0.0f, 100f, _centerRestingTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(452f, 492f), new Vector2(452f, 692f), 0.0f, 100f, _centerRestingTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(468f, 348f), new Vector2(468f, 444f), 0.0f, 96f, _centerRestingTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(452f, 340f), new Vector2(452f, 244f), 0.0f, 96f, _centerRestingTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(468f, 52f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false)
        }),
        new PlatformsRoomManager(new MovingPlatform[16]
        {
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(234f, (float) (MapsManager.maps[6].RoomHeightPx - 104)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, _disappearing: true, _maxTransparentTime: 0.8f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(68f, (float) (MapsManager.maps[6].RoomHeightPx - 172)), new Vector2(68f, (float) (MapsManager.maps[6].RoomHeightPx - 326)), 0.0f, 80f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 80, new Vector2(224f, (float) (MapsManager.maps[6].RoomHeightPx - 352)), Vector2.Zero, -1.5f, 0.0f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 80, new Vector2(224f, (float) (MapsManager.maps[6].RoomHeightPx - 352)), Vector2.Zero, -1.5f, 0.0f, _startingAngleDegrees: 120f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 80, new Vector2(224f, (float) (MapsManager.maps[6].RoomHeightPx - 352)), Vector2.Zero, -1.5f, 0.0f, _startingAngleDegrees: 240f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(224f, (float) (MapsManager.maps[6].RoomHeightPx - 352)), Vector2.Zero, 1.5f, 0.0f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(224f, (float) (MapsManager.maps[6].RoomHeightPx - 352)), Vector2.Zero, 1.5f, 0.0f, _startingAngleDegrees: 60f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(224f, (float) (MapsManager.maps[6].RoomHeightPx - 352)), Vector2.Zero, 1.5f, 0.0f, _startingAngleDegrees: 300f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(68f, (float) (MapsManager.maps[6].RoomHeightPx - 696)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(92f, (float) (MapsManager.maps[6].RoomHeightPx - 696)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 45, new Vector2(150f, (float) (MapsManager.maps[6].RoomHeightPx - 776)), new Vector2(300f, (float) (MapsManager.maps[6].RoomHeightPx - 776)), 2.09439516f, 100f, _startingAngleDegrees: 60f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 45, new Vector2(150f, (float) (MapsManager.maps[6].RoomHeightPx - 776)), new Vector2(300f, (float) (MapsManager.maps[6].RoomHeightPx - 776)), 2.09439516f, 100f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 45, new Vector2(150f, (float) (MapsManager.maps[6].RoomHeightPx - 776)), new Vector2(300f, (float) (MapsManager.maps[6].RoomHeightPx - 776)), 2.09439516f, 100f, _startingAngleDegrees: 300f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(76f, (float) (MapsManager.maps[6].RoomHeightPx - 1012)), new Vector2(76f, (float) (MapsManager.maps[6].RoomHeightPx - 1116)), 0.0f, 80f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(468f, 1124f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(452f, 764f), new Vector2(452f, 268f), 0.0f, 124f, _centerRestingTime: 1f)
        }),
        new PlatformsRoomManager(new MovingPlatform[21]
        {
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(164f, (float) (MapsManager.maps[7].RoomHeightPx - 164)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 0.75f, _maxSolidTime: 0.75f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(244f, (float) (MapsManager.maps[7].RoomHeightPx - 164)), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.5f, _maxSolidTime: 1.5f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(180f, (float) (MapsManager.maps[7].RoomHeightPx - 336)), new Vector2(180f, (float) (MapsManager.maps[7].RoomHeightPx - 456)), 2.09439516f, 80f, _startingAngleDegrees: 90f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(180f, (float) (MapsManager.maps[7].RoomHeightPx - 336)), new Vector2(180f, (float) (MapsManager.maps[7].RoomHeightPx - 456)), 2.09439516f, 80f, _startingAngleDegrees: 270f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(292f, (float) (MapsManager.maps[7].RoomHeightPx - 556)), new Vector2(292f, (float) (MapsManager.maps[7].RoomHeightPx - 436)), 2.09439516f, 80f, _startingAngleDegrees: 90f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(292f, (float) (MapsManager.maps[7].RoomHeightPx - 556)), new Vector2(292f, (float) (MapsManager.maps[7].RoomHeightPx - 436)), 2.09439516f, 80f, _startingAngleDegrees: 270f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(356f, 532f), new Vector2(108f, 532f), 0.0f, 512f, _centerRestingTime: 0.5f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(80f, 332f), new Vector2(80f, 540f), 0.0f, 416f, _centerRestingTime: 0.5f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(248f, 348f), new Vector2(136f, 348f), 0.0f, 224f, _centerRestingTime: 0.5f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(416f, 348f), new Vector2(304f, 348f), 0.0f, 224f, _centerRestingTime: 0.5f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(416f, 220f), new Vector2(416f, 308f), 0.0f, 176f, _centerRestingTime: 0.5f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(336f, 220f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(252f, 220f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f, _delay: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(180f, 204f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(108f, 180f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f, _delay: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(108f, 132f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f, _delay: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(180f, 108f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(252f, 92f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f, _delay: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(332f, 92f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(468f, 1204f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 0, new Vector2(468f, 228f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false)
        }),
        new PlatformsRoomManager(new MovingPlatform[4]
        {
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(460f, 52f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(460f, 108f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(460f, 164f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(460f, 220f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false)
        }),
        new PlatformsRoomManager(new MovingPlatform[19]
        {
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(572f, 444f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(646f, 412f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f, _delay: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(724f, 444f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(796f, 284f), new Vector2(668f, 284f), 0.0f, 80f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(484f, 284f), new Vector2(612f, 284f), 0.0f, 80f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(420f, 284f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(310f, 300f), new Vector2(0.0f, 0.0f), 2f, 0.0f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(310f, 300f), new Vector2(0.0f, 0.0f), 2f, 0.0f, _startingAngleDegrees: 120f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 40, new Vector2(310f, 300f), new Vector2(0.0f, 0.0f), 2f, 0.0f, _startingAngleDegrees: 240f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(28f, 284f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.1f, _maxSolidTime: 0.3f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(28f, 268f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.1f, _maxSolidTime: 0.3f, _delay: 0.2f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(28f, 252f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.1f, _maxSolidTime: 0.3f, _delay: 0.4f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(28f, 236f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.1f, _maxSolidTime: 0.3f, _delay: 0.6f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(28f, 220f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.1f, _maxSolidTime: 0.3f, _delay: 0.8f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(28f, 204f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.1f, _maxSolidTime: 0.3f, _delay: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(28f, 188f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 1.1f, _maxSolidTime: 0.3f, _delay: 1.2f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(108f, 168f), new Vector2(1044f, 168f), 0.0f, 60f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(140f, 116f), new Vector2(1076f, 116f), 0.0f, 60f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(172f, 68f), new Vector2(1108f, 68f), 0.0f, 60f, false)
        }),
        new PlatformsRoomManager(new MovingPlatform[10]
        {
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(28f, 444f), new Vector2(28f, 388f), 0.0f, 96f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(76f, 348f), new Vector2(76f, 404f), 0.0f, 96f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(28f, 308f), new Vector2(76f, 308f), 0.0f, 96f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(76f, 268f), new Vector2(28f, 268f), 0.0f, 96f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 48, new Vector2(52f, 180f), new Vector2(52f, 148f), 2.3561945f, 24f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 24, new Vector2(52f, 180f), new Vector2(52f, 148f), -2.3561945f, 24f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 24, new Vector2(52f, 180f), new Vector2(52f, 148f), -2.3561945f, 24f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(52f, 36f), new Vector2(52f, 60f), 0.0f, 24f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 70, new Vector2(238f, 316f), Vector2.Zero, 0.7853982f, 0.0f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble24_8, 70, new Vector2(238f, 316f), Vector2.Zero, 0.7853982f, 0.0f)
        }),
        new PlatformsRoomManager(new MovingPlatform[1]
        {
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(412f, 204f), new Vector2(636f, 204f), 0.0f, 96f)
        }),
        new PlatformsRoomManager(new MovingPlatform[24]
        {
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(156f, (float) (MapsManager.maps[12].RoomHeightPx - 36)), new Vector2(564f, (float) (MapsManager.maps[12].RoomHeightPx - 36)), 0.0f, 100f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f, _startingAngleDegrees: 45f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f, _startingAngleDegrees: 90f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f, _startingAngleDegrees: 135f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f, _startingAngleDegrees: 225f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f, _startingAngleDegrees: 270f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 80, new Vector2(124f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), new Vector2(380f, (float) (MapsManager.maps[12].RoomHeightPx - 308)), 1f, 32f, _startingAngleDegrees: 315f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 104, new Vector2(724f, (float) (MapsManager.maps[12].RoomHeightPx - 196)), new Vector2(0.0f, 0.0f), -1f, 0.0f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 104, new Vector2(724f, (float) (MapsManager.maps[12].RoomHeightPx - 196)), new Vector2(0.0f, 0.0f), -1f, 0.0f, _startingAngleDegrees: 60f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 104, new Vector2(724f, (float) (MapsManager.maps[12].RoomHeightPx - 196)), new Vector2(0.0f, 0.0f), -1f, 0.0f, _startingAngleDegrees: 120f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 104, new Vector2(724f, (float) (MapsManager.maps[12].RoomHeightPx - 196)), new Vector2(0.0f, 0.0f), -1f, 0.0f, _startingAngleDegrees: 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 104, new Vector2(724f, (float) (MapsManager.maps[12].RoomHeightPx - 196)), new Vector2(0.0f, 0.0f), -1f, 0.0f, _startingAngleDegrees: 240f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 104, new Vector2(724f, (float) (MapsManager.maps[12].RoomHeightPx - 196)), new Vector2(0.0f, 0.0f), -1f, 0.0f, _startingAngleDegrees: 300f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(956f, (float) (MapsManager.maps[12].RoomHeightPx - 12)), new Vector2(956f, (float) (MapsManager.maps[12].RoomHeightPx - 420)), 0.0f, 80f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(1076f, 100f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2.2f, _maxSolidTime: 0.8f, _delay: -0.3f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(1108f, 116f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2.2f, _maxSolidTime: 0.8f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(1140f, 132f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2.2f, _maxSolidTime: 0.8f, _delay: 0.3f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(1172f, 148f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2.2f, _maxSolidTime: 0.8f, _delay: 0.6f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(1204f, 164f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2.2f, _maxSolidTime: 0.8f, _delay: 0.9f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(1236f, 180f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2.2f, _maxSolidTime: 0.8f, _delay: 1.2f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(1268f, 196f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2.2f, _maxSolidTime: 0.8f, _delay: 1.5f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(1324f, 252f), new Vector2(1068f, 252f), 0.0f, 640f, _centerRestingTime: 1.1f)
        }),
        new PlatformsRoomManager(new MovingPlatform[0]),
        new PlatformsRoomManager(new MovingPlatform[7]
        {
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 205, FinalBoss.fireballsCenter, Vector2.Zero, 0.5f, 0.0f, false),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 205, FinalBoss.fireballsCenter, Vector2.Zero, 0.5f, 0.0f, false, 120f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 205, FinalBoss.fireballsCenter, Vector2.Zero, 0.5f, 0.0f, false, 240f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 160, FinalBoss.fireballsCenter, Vector2.Zero, -0.5f, 0.0f, false, 180f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 160, FinalBoss.fireballsCenter, Vector2.Zero, -0.5f, 0.0f, false, 60f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 160, FinalBoss.fireballsCenter, Vector2.Zero, -0.5f, 0.0f, false, 300f),
          new MovingPlatform(MovingPlatform.TextureType.marble40_8, 0, new Vector2(100f, (float) (MapsManager.maps[14].RoomHeightPx + 50)), new Vector2(100f, 72f), 0.0f, 30f, false, _moveOnce: true)
        }),
        new PlatformsRoomManager(new MovingPlatform[4]
        {
          new MovingPlatform(MovingPlatform.TextureType.fallingFloor272_40, 0, new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 552), (float) (MapsManager.maps[15].RoomHeightPx - 20)), new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 552), (float) (MapsManager.maps[15].RoomHeightPx + 21)), 0.0f, 5f, false, _moveOnce: true),
          new MovingPlatform(MovingPlatform.TextureType.fallingFloor296_40, 0, new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 860), (float) (MapsManager.maps[15].RoomHeightPx - 20)), new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 860), (float) (MapsManager.maps[15].RoomHeightPx + 21)), 0.0f, 5f, false, _moveOnce: true),
          new MovingPlatform(MovingPlatform.TextureType.fallingFloor112_216, 0, new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 1280), (float) (MapsManager.maps[15].RoomHeightPx + 108)), new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 1280), (float) (MapsManager.maps[15].RoomHeightPx - 112)), 0.0f, 25f, false, _moveOnce: true),
          new MovingPlatform(MovingPlatform.TextureType.fallingFloor272_40, 0, new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 1568), (float) (MapsManager.maps[15].RoomHeightPx + 5)), new Vector2((float) (MapsManager.maps[15].RoomWidthtPx - 1568), (float) (MapsManager.maps[15].RoomHeightPx - 20)), 0.0f, 8f, false, _moveOnce: true)
        }),
        new PlatformsRoomManager(new MovingPlatform[3]
        {
          new MovingPlatform(MovingPlatform.TextureType.dirt40_8, 0, new Vector2((float) (MapsManager.maps[16].RoomWidthtPx - 76), 116f), new Vector2(-40f, 116f), 0.0f, 52f, false, _moveOnce: true),
          new MovingPlatform(MovingPlatform.TextureType.fallingFloor184_216, 0, new Vector2((float) (MapsManager.maps[16].RoomWidthtPx - 972), (float) (MapsManager.maps[16].RoomHeightPx + 32)), new Vector2((float) (MapsManager.maps[16].RoomWidthtPx - 972), (float) MapsManager.maps[16].RoomHeightPx), 0.0f, 10f, false, _moveOnce: true),
          new MovingPlatform(MovingPlatform.TextureType.fallingFloor184_216, 0, new Vector2((float) (MapsManager.maps[16].RoomWidthtPx - 1156), (float) (MapsManager.maps[16].RoomHeightPx + 24)), new Vector2((float) (MapsManager.maps[16].RoomWidthtPx - 1156), (float) (MapsManager.maps[16].RoomHeightPx - 8)), 0.0f, 10f, false, _moveOnce: true)
        }),
        new PlatformsRoomManager(new MovingPlatform[6]
        {
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(396f, 100f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2f, _maxSolidTime: 1f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(356f, 116f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2f, _maxSolidTime: 1f, _delay: 0.4f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(316f, 132f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2f, _maxSolidTime: 1f, _delay: 0.8f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(276f, 148f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2f, _maxSolidTime: 1f, _delay: 1.2f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(236f, 164f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2f, _maxSolidTime: 1f, _delay: 1.6f),
          new MovingPlatform(MovingPlatform.TextureType.crackedMarble24_8, 0, new Vector2(196f, 180f), new Vector2(0.0f, 0.0f), 0.0f, 0.0f, false, _disappearing: true, _maxTransparentTime: 2f, _maxSolidTime: 1f, _delay: 2f)
        })
      };
    }
  }
}
