
// Type: GameManager.GameEvents
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace GameManager
{
  internal static class GameEvents
  {
    public static readonly float[] eventsDuration = new float[39]
    {
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.5f,
      0.0f,
      0.5f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      2f,
      4f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      3.1f,
      0.0f,
      2.5f,
      1f,
      0.0f,
      0.0f,
      0.0f,
      4.2f,
      0.0f,
      0.0f,
      0.0f,
      0.0f,
      2.2f,
      0.0f,
      0.0f
    };
    public static float elapsedEventsDuration;
    public static bool[] eventAlreadyHappened;
    public static GameEvents.Events happening;

    public static void Inizialize()
    {
      GameEvents.elapsedEventsDuration = 0.0f;
      GameEvents.eventAlreadyHappened = new bool[39];
      for (int index = 0; index < 39; ++index)
        GameEvents.eventAlreadyHappened[index] = false;
      GameEvents.happening = GameEvents.Events.none;
    }

    public static void Reset()
    {
      GameEvents.elapsedEventsDuration = 0.0f;
      GameEvents.happening = GameEvents.Events.none;
      if (GameEvents.eventAlreadyHappened[2] && !GameEvents.eventAlreadyHappened[3])
      {
        CollectablesManager.collectablesRoomManagers[3].RemoveCollectablesFromMap();
        GameEvents.eventAlreadyHappened[2] = false;
      }
      else if (GameEvents.eventAlreadyHappened[4] && !GameEvents.eventAlreadyHappened[5])
      {
        CollectablesManager.collectablesRoomManagers[3].RemoveCollectablesFromMap();
        GameEvents.eventAlreadyHappened[4] = false;
      }
      else if (GameEvents.eventAlreadyHappened[10] && !GameEvents.eventAlreadyHappened[11])
        CollectablesManager.collectablesRoomManagers[8].AddCollectableToMap(new Collectable(new Point(280, 140), Collectable.ItemType.wallJump_powerup));
      else if (GameEvents.eventAlreadyHappened[16] && !GameEvents.eventAlreadyHappened[17])
        CollectablesManager.collectablesRoomManagers[10].AddCollectableToMap(new Collectable(new Point(230, 400), Collectable.ItemType.silverKey));
      else if (FinalBoss.Dead)
      {
        List<int[]> _tilesToRemove = new List<int[]>();
        for (int index = 1; index < (MapsManager.maps[14].roomWidthTiles + 1) / 2; ++index)
        {
          _tilesToRemove.Add(new int[2]{ 57, index });
          _tilesToRemove.Add(new int[2]
          {
            57,
            MapsManager.maps[14].roomWidthTiles - index - 1
          });
          _tilesToRemove.Add(new int[2]{ 56, index });
          _tilesToRemove.Add(new int[2]
          {
            56,
            MapsManager.maps[14].roomWidthTiles - index - 1
          });
        }
        MapsManager.maps[14].RemoveGroupOfTiles(_tilesToRemove, 0.0f, 100);
      }
      switch (RoomsManager.CurrentRoom)
      {
        case RoomsManager.Rooms.finalBoss:
          if (FinalBoss.Dead)
            break;
          GameEvents.eventAlreadyHappened[18] = false;
          GameEvents.eventAlreadyHappened[19] = false;
          GameEvents.eventAlreadyHappened[20] = false;
          GameEvents.eventAlreadyHappened[21] = false;
          PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].Reset();
          MapsManager.resetMap(RoomsManager.CurrentRoom);
          FinalBoss.Reset();
          break;
        case RoomsManager.Rooms.escape0:
          if (RoomsManager.PreviousRoom != RoomsManager.Rooms.finalBoss)
            break;
          GameEvents.eventAlreadyHappened[23] = false;
          GameEvents.eventAlreadyHappened[24] = false;
          GameEvents.eventAlreadyHappened[25] = false;
          GameEvents.eventAlreadyHappened[26] = false;
          GameEvents.eventAlreadyHappened[27] = false;
          GameEvents.eventAlreadyHappened[28] = false;
          GameEvents.eventAlreadyHappened[29] = false;
          GameEvents.eventAlreadyHappened[30] = false;
          GameEvents.eventAlreadyHappened[31] = false;
          GameEvents.eventAlreadyHappened[32] = false;
          PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].Reset();
          MapsManager.resetMap(RoomsManager.CurrentRoom);
          break;
        case RoomsManager.Rooms.escape1:
          if (RoomsManager.PreviousRoom != RoomsManager.Rooms.escape0)
            break;
          GameEvents.eventAlreadyHappened[33] = false;
          GameEvents.eventAlreadyHappened[34] = false;
          GameEvents.eventAlreadyHappened[35] = false;
          PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].Reset();
          break;
      }
    }

    public static void Update(float elapsedTime)
    {
      if (GameEvents.happening == GameEvents.Events.none)
      {
        GameEvents.eventHandler(elapsedTime);
      }
      else
      {
        GameEvents.elapsedEventsDuration += elapsedTime;
        if ((double) GameEvents.elapsedEventsDuration < (double) GameEvents.eventsDuration[(int) GameEvents.happening])
          return;
        GameEvents.eventAlreadyHappened[(int) GameEvents.happening] = true;
        GameEvents.elapsedEventsDuration = 0.0f;
        GameEvents.happening = GameEvents.Events.none;
      }
    }

    private static void eventHandler(float elapsedTime)
    {
      switch (RoomsManager.CurrentRoom)
      {
        case RoomsManager.Rooms.tutorial0:
          if (!GameEvents.eventAlreadyHappened[1])
          {
            GameEvents.TriggerEvent(GameEvents.Events.introMonologue);
            break;
          }
          if (FinalBoss.Dead && (double) Player.position.X < 200.0 && (double) Player.position.Y <= (double) (176 - Player.height) && !GameEvents.eventAlreadyHappened[37])
          {
            GameEvents.TriggerEvent(GameEvents.Events.finalMonologue);
            break;
          }
          if (!GameEvents.eventAlreadyHappened[37] || MonologuesManager.MonologuePlaying)
            break;
          GameEvents.TriggerEvent(GameEvents.Events.finalCutscene);
          break;
        case RoomsManager.Rooms.tutorial3:
          if (EnemyManager.enemyRoomManagers[(int) RoomsManager.CurrentRoom].enemiesType1[0].Dead && !GameEvents.eventAlreadyHappened[2])
          {
            GameEvents.TriggerEvent(GameEvents.Events.keySpawns1_tutorial3);
            break;
          }
          if (GameEvents.eventAlreadyHappened[2] && !GameEvents.eventAlreadyHappened[3] && CollectablesManager.collectedItems.Count == 1)
          {
            GameEvents.TriggerEvent(GameEvents.Events.key1Collected_tutorial3);
            break;
          }
          if (EnemyManager.enemyRoomManagers[(int) RoomsManager.CurrentRoom].enemiesType1[1].Dead && !GameEvents.eventAlreadyHappened[4])
          {
            GameEvents.TriggerEvent(GameEvents.Events.keySpawns2_tutorial3);
            break;
          }
          if (!GameEvents.eventAlreadyHappened[4] || GameEvents.eventAlreadyHappened[5] || CollectablesManager.collectedItems.Count != 1)
            break;
          GameEvents.TriggerEvent(GameEvents.Events.key2Collected_tutorial3);
          break;
        case RoomsManager.Rooms.churchBellTower0:
          if (Player.CollisionRectangle.Intersects(new Rectangle(196, 908, 96, 76)) && CollectablesManager.TryRemoveFromInventory(Collectable.ItemType.doubleJump_powerup, new Vector2(223f, 938f)) && !GameEvents.eventAlreadyHappened[6])
          {
            GameEvents.TriggerEvent(GameEvents.Events.unlockDoubleJump);
            break;
          }
          if (GameEvents.eventAlreadyHappened[6] && !GameEvents.eventAlreadyHappened[7])
          {
            GameEvents.TriggerEvent(GameEvents.Events.showDoubleJumpScreen);
            break;
          }
          if (Player.CollisionRectangle.Intersects(new Rectangle(196, 908, 96, 76)) && CollectablesManager.TryRemoveFromInventory(Collectable.ItemType.wallJump_powerup, new Vector2(251f, 938f)) && !GameEvents.eventAlreadyHappened[8])
          {
            GameEvents.TriggerEvent(GameEvents.Events.unlockWallJump);
            break;
          }
          if (!GameEvents.eventAlreadyHappened[8] || GameEvents.eventAlreadyHappened[9])
            break;
          GameEvents.TriggerEvent(GameEvents.Events.showWallJumpScreen);
          break;
        case RoomsManager.Rooms.midBoss:
          if (MidBoss.Dead && !GameEvents.eventAlreadyHappened[10])
          {
            GameEvents.TriggerEvent(GameEvents.Events.keyAndPowerUpSpawn_midBoss);
            break;
          }
          if (!GameEvents.eventAlreadyHappened[10] || CollectablesManager.collectedItems.Count <= 0 || GameEvents.eventAlreadyHappened[11])
            break;
          using (List<Collectable>.Enumerator enumerator = CollectablesManager.collectedItems.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              if (enumerator.Current.type == Collectable.ItemType.wallJump_powerup)
              {
                GameEvents.TriggerEvent(GameEvents.Events.pUpCollected_midBoss);
                break;
              }
            }
            break;
          }
        case RoomsManager.Rooms.churchGroundFloor0:
          if ((double) Player.position.X >= (double) (88 - Player.width) && (double) Player.position.Y < 178.0)
          {
            if (!Player.IsOnMovingPlatform || GameEvents.eventAlreadyHappened[12])
              break;
            GameEvents.TriggerEvent(GameEvents.Events.activateMovingPlatforms_churchGroundFloor0);
            break;
          }
          if (GameEvents.eventAlreadyHappened[13])
            break;
          GameEvents.TriggerEvent(GameEvents.Events.resetMovingPlatforms_churchGroundFloor0);
          break;
        case RoomsManager.Rooms.churchAltarRoom:
          if (TorchManager.AllTorchesUnlit() && !GameEvents.eventAlreadyHappened[16])
          {
            GameEvents.TriggerEvent(GameEvents.Events.spawnKey_altar);
            break;
          }
          if (!GameEvents.eventAlreadyHappened[16] || GameEvents.eventAlreadyHappened[17] || CollectablesManager.collectedItems.Count <= 0)
            break;
          using (List<Collectable>.Enumerator enumerator = CollectablesManager.collectedItems.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              if (enumerator.Current.type == Collectable.ItemType.silverKey)
              {
                GameEvents.TriggerEvent(GameEvents.Events.keyCollected_altar);
                break;
              }
            }
            break;
          }
        case RoomsManager.Rooms.church1stFloor0:
          if ((double) Player.position.X < 1032.0 && (double) Player.position.X >= 1000.0 || (double) Player.position.X < 684.0 && (double) Player.position.X >= 664.0 || (double) Player.position.X < 364.0 && (double) Player.position.X >= 344.0 && (double) Player.position.Y > 106.0)
          {
            if (GameEvents.eventAlreadyHappened[14])
              break;
            GameEvents.TriggerEvent(GameEvents.Events.showInvisibleTiles_church1stFloor0);
            break;
          }
          if (GameEvents.eventAlreadyHappened[15])
            break;
          GameEvents.TriggerEvent(GameEvents.Events.hide_InvisibleTiles_church1stFloor0);
          break;
        case RoomsManager.Rooms.finalBoss:
          if ((double) Player.position.X <= 200.0 && !GameEvents.eventAlreadyHappened[18])
            GameEvents.TriggerEvent(GameEvents.Events.terrainCollapseFinalBoss);
          if (GameEvents.eventAlreadyHappened[18] && !GameEvents.eventAlreadyHappened[19])
            GameEvents.TriggerEvent(GameEvents.Events.finalBossComesAlive);
          if (GameEvents.eventAlreadyHappened[19] && !GameEvents.eventAlreadyHappened[20])
            GameEvents.TriggerEvent(GameEvents.Events.activatePlatformsFinalBoss);
          if (!FinalBoss.Dead || GameEvents.eventAlreadyHappened[21])
            break;
          GameEvents.TriggerEvent(GameEvents.Events.escapeFinalBossRoom);
          break;
        case RoomsManager.Rooms.escape0:
          if ((double) Player.position.X < 1752.0 && !GameEvents.eventAlreadyHappened[22])
            GameEvents.TriggerEvent(GameEvents.Events.monologue_escape0);
          if ((double) Player.position.X < (double) (MapsManager.maps[15].RoomWidthtPx - 248) && !GameEvents.eventAlreadyHappened[23])
            GameEvents.TriggerEvent(GameEvents.Events.lavaEruption1_escape0);
          if ((double) Player.position.X < (double) (MapsManager.maps[15].RoomWidthtPx - 416) && !GameEvents.eventAlreadyHappened[24])
            GameEvents.TriggerEvent(GameEvents.Events.lowerFloor1_escape0);
          if (!PlatformsManager.platformsRoomManagers[15].movingPlatforms[1].active && !GameEvents.eventAlreadyHappened[25] && GameEvents.eventAlreadyHappened[24])
            GameEvents.TriggerEvent(GameEvents.Events.removeFloor1_escape0);
          if (GameEvents.eventAlreadyHappened[24] && !GameEvents.eventAlreadyHappened[26])
            GameEvents.TriggerEvent(GameEvents.Events.lavaEruption2_escape0);
          if (GameEvents.eventAlreadyHappened[26] && !GameEvents.eventAlreadyHappened[27])
            GameEvents.TriggerEvent(GameEvents.Events.lavaEruption3_escape0);
          if (GameEvents.eventAlreadyHappened[27] && !GameEvents.eventAlreadyHappened[28])
            GameEvents.TriggerEvent(GameEvents.Events.lavaEruption4_escape0);
          if (GameEvents.eventAlreadyHappened[24] && !GameEvents.eventAlreadyHappened[29])
            GameEvents.TriggerEvent(GameEvents.Events.raiseFloor2_escape0);
          if ((double) Player.position.X < (double) (MapsManager.maps[15].RoomWidthtPx - 1352) && !GameEvents.eventAlreadyHappened[30])
            GameEvents.TriggerEvent(GameEvents.Events.lavaEruption5_escape0);
          if ((double) Player.position.X < (double) (MapsManager.maps[15].RoomWidthtPx - 1448) && !GameEvents.eventAlreadyHappened[31])
            GameEvents.TriggerEvent(GameEvents.Events.raiseFloor3_escape0);
          if (!GameEvents.eventAlreadyHappened[31] || GameEvents.eventAlreadyHappened[32])
            break;
          GameEvents.TriggerEvent(GameEvents.Events.lavaEruption6_escape0);
          break;
        case RoomsManager.Rooms.escape1:
          if (Player.IsOnMovingPlatform && !GameEvents.eventAlreadyHappened[33])
            GameEvents.TriggerEvent(GameEvents.Events.activatePlatform_escape1);
          if ((double) Player.position.X < (double) (MapsManager.maps[16].RoomWidthtPx - 881) && !GameEvents.eventAlreadyHappened[34])
            GameEvents.TriggerEvent(GameEvents.Events.raiseFloor1_escape1);
          if ((double) Player.position.X >= 104.0 || GameEvents.eventAlreadyHappened[35])
            break;
          GameEvents.TriggerEvent(GameEvents.Events.lavaEruption1_escape1);
          break;
        case RoomsManager.Rooms.escape2:
          if ((double) Player.position.X + (double) Player.width >= 424.0)
            break;
          if (!GameEvents.eventAlreadyHappened[36])
          {
            GameEvents.TriggerEvent(GameEvents.Events.lavaEruption_escape2);
            break;
          }
          GameEvents.eventAlreadyHappened[36] = false;
          break;
      }
    }

    public static void TriggerEvent(GameEvents.Events _event)
    {
      switch (RoomsManager.CurrentRoom)
      {
        case RoomsManager.Rooms.tutorial0:
          switch (_event)
          {
            case GameEvents.Events.introMonologue:
              MonologuesManager.monologuesRoomManagers[(int) RoomsManager.CurrentRoom].PlayMonologue(0);
              GameEvents.happening = GameEvents.Events.introMonologue;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.finalMonologue:
              MonologuesManager.monologuesRoomManagers[(int) RoomsManager.CurrentRoom].PlayMonologue(1);
              GameEvents.happening = GameEvents.Events.finalMonologue;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.finalCutscene:
              Game1.currentGameState = Game1.GameStates.ending;
              CutscenesManager.cutscenes[1].ChangeSentence(CutscenesManager.cutscenes[1].GetNUmOfSentences() - 1, "Your Time: " + GameStats.GetTimeString() + "                                                                                            ");
              GameEvents.happening = GameEvents.Events.finalCutscene;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            default:
              return;
          }
        case RoomsManager.Rooms.tutorial3:
          switch (_event)
          {
            case GameEvents.Events.keySpawns1_tutorial3:
              CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].AddCollectableToMap(new Collectable(new Point(350, 132), Collectable.ItemType.brassKey));
              GameEvents.happening = GameEvents.Events.keySpawns1_tutorial3;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.key1Collected_tutorial3:
              GameEvents.eventAlreadyHappened[3] = true;
              return;
            case GameEvents.Events.keySpawns2_tutorial3:
              CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].AddCollectableToMap(new Collectable(new Point(350, 46), Collectable.ItemType.silverKey));
              GameEvents.happening = GameEvents.Events.keySpawns2_tutorial3;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.key2Collected_tutorial3:
              GameEvents.eventAlreadyHappened[5] = true;
              return;
            default:
              return;
          }
        case RoomsManager.Rooms.churchBellTower0:
          switch (_event)
          {
            case GameEvents.Events.unlockDoubleJump:
              Player.doubleJumpUnlocked = true;
              AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.CurrentRoom].AddTempAnimatedSprite(new AnimatedSprite(new Vector2(211f, 926f), AnimatedSprite.AnimationType.displayDoubleJumpRelic, false));
              GameEvents.happening = GameEvents.Events.unlockDoubleJump;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.showDoubleJumpScreen:
              Game1.currentGameState = Game1.GameStates.doubleJump;
              GameEvents.happening = GameEvents.Events.showDoubleJumpScreen;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.unlockWallJump:
              Player.wallJumpUnlocked = true;
              AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.CurrentRoom].AddTempAnimatedSprite(new AnimatedSprite(new Vector2(239f, 926f), AnimatedSprite.AnimationType.displayWallJumpRelic, false));
              GameEvents.happening = GameEvents.Events.unlockWallJump;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.showWallJumpScreen:
              Game1.currentGameState = Game1.GameStates.wallJump;
              GameEvents.happening = GameEvents.Events.showWallJumpScreen;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            default:
              return;
          }
        case RoomsManager.Rooms.midBoss:
          if (_event != GameEvents.Events.keyAndPowerUpSpawn_midBoss)
          {
            if (_event != GameEvents.Events.pUpCollected_midBoss)
              break;
            GameEvents.eventAlreadyHappened[11] = true;
            break;
          }
          CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].AddCollectableToMap(new Collectable(new Point(280, 140), Collectable.ItemType.wallJump_powerup));
          CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].AddCollectableToMap(new Collectable(new Point(216, 148), Collectable.ItemType.brassKey));
          GameEvents.happening = GameEvents.Events.keyAndPowerUpSpawn_midBoss;
          GameEvents.elapsedEventsDuration = 0.0f;
          break;
        case RoomsManager.Rooms.churchGroundFloor0:
          if (_event != GameEvents.Events.activateMovingPlatforms_churchGroundFloor0)
          {
            if (_event != GameEvents.Events.resetMovingPlatforms_churchGroundFloor0)
              break;
            PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[16].Reset(false);
            PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[17].Reset(false);
            PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[18].Reset(false);
            GameEvents.happening = GameEvents.Events.resetMovingPlatforms_churchGroundFloor0;
            GameEvents.eventAlreadyHappened[12] = false;
            GameEvents.elapsedEventsDuration = 0.0f;
            break;
          }
          PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[16].active = true;
          PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[17].active = true;
          PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[18].active = true;
          GameEvents.happening = GameEvents.Events.activateMovingPlatforms_churchGroundFloor0;
          GameEvents.eventAlreadyHappened[13] = false;
          GameEvents.elapsedEventsDuration = 0.0f;
          break;
        case RoomsManager.Rooms.churchAltarRoom:
          if (_event != GameEvents.Events.spawnKey_altar)
          {
            if (_event != GameEvents.Events.keyCollected_altar)
              break;
            GameEvents.eventAlreadyHappened[17] = true;
            break;
          }
          CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].AddCollectableToMap(new Collectable(new Point(230, 400), Collectable.ItemType.silverKey));
          GameEvents.eventAlreadyHappened[16] = true;
          break;
        case RoomsManager.Rooms.church1stFloor0:
          switch (_event)
          {
            case GameEvents.Events.showInvisibleTiles_church1stFloor0:
              List<AnimatedSprite> animatedSprites = new List<AnimatedSprite>();
              for (int index1 = 0; index1 < MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles; ++index1)
              {
                for (int index2 = 0; index2 < MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles; ++index2)
                {
                  if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index1, index2].tileType == Tile.TileType.solidEmpty)
                    animatedSprites.Add(new AnimatedSprite(new Vector2((float) (index2 * Tile.tileSize), (float) (index1 * Tile.tileSize)), AnimatedSprite.AnimationType.invisibleTile));
                }
              }
              AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.CurrentRoom].AddAnimatedSprites(animatedSprites);
              if ((double) Player.position.X < 1032.0 && (double) Player.position.X >= 1000.0)
                CameraManager.MoveCamera(0.0f, new Vector2(856f, (float) MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx), 8f);
              else if ((double) Player.position.X < 684.0 && (double) Player.position.X >= 664.0)
                CameraManager.MoveCamera(0.0f, new Vector2(528f, (float) MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx), 8f);
              else if ((double) Player.position.X < 364.0 && (double) Player.position.X >= 344.0)
                CameraManager.MoveCamera(0.0f, new Vector2(204f, (float) MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx), 8f);
              GameEvents.happening = GameEvents.Events.showInvisibleTiles_church1stFloor0;
              GameEvents.eventAlreadyHappened[15] = false;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.hide_InvisibleTiles_church1stFloor0:
              AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.CurrentRoom].ClearAnimatedSprites();
              CameraManager.MoveCamera(1f, CameraManager.pointLocked, 8f);
              GameEvents.happening = GameEvents.Events.hide_InvisibleTiles_church1stFloor0;
              GameEvents.eventAlreadyHappened[14] = false;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            default:
              return;
          }
        case RoomsManager.Rooms.finalBoss:
          switch (_event)
          {
            case GameEvents.Events.terrainCollapseFinalBoss:
              List<int[]> _tilesToRemove1 = new List<int[]>();
              for (int index = 1; index < (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles + 1) / 2; ++index)
              {
                _tilesToRemove1.Add(new int[2]{ 57, index });
                _tilesToRemove1.Add(new int[2]
                {
                  57,
                  MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - index - 1
                });
                _tilesToRemove1.Add(new int[2]{ 56, index });
                _tilesToRemove1.Add(new int[2]
                {
                  56,
                  MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - index - 1
                });
              }
              CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].RemoveCollectablesFromMap();
              CameraManager.shakeForTime(GameEvents.eventsDuration[18]);
              CameraManager.MoveCamera(0.3f, FinalBoss.fireballsCenter, 1f);
              MapsManager.maps[(int) RoomsManager.CurrentRoom].RemoveGroupOfTiles(_tilesToRemove1, 0.03f, 3);
              GameEvents.happening = GameEvents.Events.terrainCollapseFinalBoss;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.finalBossComesAlive:
              CameraManager.shakeForTime(GameEvents.eventsDuration[19]);
              FinalBoss.WakeUp();
              GameEvents.happening = GameEvents.Events.finalBossComesAlive;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.activatePlatformsFinalBoss:
              for (int index = 0; index < 6; ++index)
                PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[index].active = true;
              GameEvents.happening = GameEvents.Events.activatePlatformsFinalBoss;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.escapeFinalBossRoom:
              PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[6].active = true;
              CameraManager.MoveCamera(1f, FinalBoss.fireballsCenter, 1f);
              LavaGeyserManager.SweepAcross(2f, 0.7f, 1, 0.0f, 24f, false);
              GameEvents.happening = GameEvents.Events.escapeFinalBossRoom;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            default:
              return;
          }
        case RoomsManager.Rooms.escape0:
          switch (_event)
          {
            case GameEvents.Events.monologue_escape0:
              MonologuesManager.monologuesRoomManagers[(int) RoomsManager.CurrentRoom].PlayMonologue(0);
              GameEvents.happening = GameEvents.Events.monologue_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lavaEruption1_escape0:
              LavaGeyserManager.ShootGeyser(new float[1]
              {
                (float) (MapsManager.maps[15].RoomWidthtPx - 308)
              }, 0.0f, -1000);
              List<int[]> _tilesToRemove2 = new List<int[]>();
              for (int index3 = 0; index3 < 10; ++index3)
              {
                for (int index4 = 0; index4 < 3; ++index4)
                  _tilesToRemove2.Add(new int[2]
                  {
                    MapsManager.maps[15].roomHeightTiles - index3 - 1,
                    MapsManager.maps[15].roomWidthTiles - 40 + index4
                  });
              }
              MapsManager.maps[(int) RoomsManager.CurrentRoom].RemoveGroupOfTiles(_tilesToRemove2, 0.01f, 3, 0.3f);
              GameEvents.happening = GameEvents.Events.lavaEruption1_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lowerFloor1_escape0:
              PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[0].active = true;
              PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[1].active = true;
              GameEvents.happening = GameEvents.Events.lowerFloor1_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.removeFloor1_escape0:
              PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[0].RemovePlatform();
              PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[1].RemovePlatform();
              GameEvents.happening = GameEvents.Events.removeFloor1_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lavaEruption2_escape0:
              LavaGeyserManager.ShootGeyser(new float[2]
              {
                (float) (MapsManager.maps[15].RoomWidthtPx - 664),
                (float) (MapsManager.maps[15].RoomWidthtPx - 640)
              }, 0.0f);
              GameEvents.happening = GameEvents.Events.lavaEruption2_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lavaEruption3_escape0:
              LavaGeyserManager.ShootGeyser(new float[2]
              {
                (float) (MapsManager.maps[15].RoomWidthtPx - 1032),
                (float) (MapsManager.maps[15].RoomWidthtPx - 1008)
              }, 0.0f, -600);
              GameEvents.happening = GameEvents.Events.lavaEruption3_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lavaEruption4_escape0:
              LavaGeyserManager.ShootGeyser(new float[2]
              {
                (float) (MapsManager.maps[15].RoomWidthtPx - 972),
                (float) (MapsManager.maps[15].RoomWidthtPx - 948)
              }, 0.0f, -600);
              GameEvents.happening = GameEvents.Events.lavaEruption4_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.raiseFloor2_escape0:
              PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[2].active = true;
              GameEvents.happening = GameEvents.Events.raiseFloor2_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lavaEruption5_escape0:
              LavaGeyserManager.ShootGeyser(new float[1]
              {
                (float) (MapsManager.maps[15].RoomWidthtPx - 1388)
              }, 1.5f, -800);
              GameEvents.happening = GameEvents.Events.lavaEruption5_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.raiseFloor3_escape0:
              PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[3].active = true;
              GameEvents.happening = GameEvents.Events.raiseFloor3_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lavaEruption6_escape0:
              LavaGeyserManager.ShootGeyser(new float[2]
              {
                (float) (MapsManager.maps[15].RoomWidthtPx - 1688),
                (float) (MapsManager.maps[15].RoomWidthtPx - 1664)
              }, 0.0f);
              GameEvents.happening = GameEvents.Events.lavaEruption6_escape0;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            default:
              return;
          }
        case RoomsManager.Rooms.escape1:
          switch (_event)
          {
            case GameEvents.Events.activatePlatform_escape1:
              PlatformsManager.platformsRoomManagers[16].movingPlatforms[0].active = true;
              GameEvents.happening = GameEvents.Events.activatePlatform_escape1;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.raiseFloor1_escape1:
              PlatformsManager.platformsRoomManagers[16].movingPlatforms[1].active = true;
              PlatformsManager.platformsRoomManagers[16].movingPlatforms[2].active = true;
              GameEvents.happening = GameEvents.Events.raiseFloor1_escape1;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            case GameEvents.Events.lavaEruption1_escape1:
              LavaGeyserManager.SweepAcross(0.2f, 0.0f, 10, 0.0f, 96f, true);
              GameEvents.happening = GameEvents.Events.lavaEruption1_escape1;
              GameEvents.elapsedEventsDuration = 0.0f;
              return;
            default:
              return;
          }
        case RoomsManager.Rooms.escape2:
          if (_event != GameEvents.Events.lavaEruption_escape2)
            break;
          LavaGeyserManager.ShootGeyser(new float[1]{ 396f }, 0.0f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 376f }, 0.2f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 356f }, 0.4f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 336f }, 0.6f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 316f }, 0.8f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 296f }, 1f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 276f }, 1.2f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 256f }, 1.4f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 236f }, 1.6f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 216f }, 1.8f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 196f }, 2f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 176f }, 2.2f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 156f }, 2.4f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 136f }, 2.6f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 116f }, 2.8f);
          LavaGeyserManager.ShootGeyser(new float[1]{ 96f }, 3f);
          GameEvents.happening = GameEvents.Events.lavaEruption_escape2;
          GameEvents.elapsedEventsDuration = 0.0f;
          break;
      }
    }

    public enum Events
    {
      none,
      introMonologue,
      keySpawns1_tutorial3,
      key1Collected_tutorial3,
      keySpawns2_tutorial3,
      key2Collected_tutorial3,
      unlockDoubleJump,
      showDoubleJumpScreen,
      unlockWallJump,
      showWallJumpScreen,
      keyAndPowerUpSpawn_midBoss,
      pUpCollected_midBoss,
      activateMovingPlatforms_churchGroundFloor0,
      resetMovingPlatforms_churchGroundFloor0,
      showInvisibleTiles_church1stFloor0,
      hide_InvisibleTiles_church1stFloor0,
      spawnKey_altar,
      keyCollected_altar,
      terrainCollapseFinalBoss,
      finalBossComesAlive,
      activatePlatformsFinalBoss,
      escapeFinalBossRoom,
      monologue_escape0,
      lavaEruption1_escape0,
      lowerFloor1_escape0,
      removeFloor1_escape0,
      lavaEruption2_escape0,
      lavaEruption3_escape0,
      lavaEruption4_escape0,
      raiseFloor2_escape0,
      lavaEruption5_escape0,
      raiseFloor3_escape0,
      lavaEruption6_escape0,
      activatePlatform_escape1,
      raiseFloor1_escape1,
      lavaEruption1_escape1,
      lavaEruption_escape2,
      finalMonologue,
      finalCutscene,
      total,
    }
  }
}
