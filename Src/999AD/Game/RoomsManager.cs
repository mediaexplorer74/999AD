
// Type: GameManager.RoomsManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class RoomsManager
  {
    private static RoomsManager.Rooms currentRoom;
    private static RoomsManager.Rooms previousRoom;

    public static void Inizialize()
    {
      RoomsManager.currentRoom = RoomsManager.Rooms.tutorial0;
      RoomsManager.previousRoom = RoomsManager.Rooms.tutorial0;
      CameraManager.SwitchCamera(RoomsManager.currentRoom);
    }

    public static RoomsManager.Rooms CurrentRoom
    {
      get => RoomsManager.currentRoom;
      set => RoomsManager.currentRoom = value;
    }

    public static RoomsManager.Rooms PreviousRoom
    {
      get => RoomsManager.previousRoom;
      set => RoomsManager.previousRoom = value;
    }

    private static bool switchRoom()
    {
      switch (RoomsManager.currentRoom)
      {
        case RoomsManager.Rooms.tutorial0:
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial1;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial1);
            Player.position.X = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial4;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial4);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.tutorial1:
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial2;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial1;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial2);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial0;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial1;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial0);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial3;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial1;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial3);
            Player.position.X = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.tutorial2:
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            if ((double) Player.position.X >= (double) MapsManager.maps[1].RoomWidthtPx)
            {
              RoomsManager.currentRoom = RoomsManager.Rooms.tutorial3;
              RoomsManager.previousRoom = RoomsManager.Rooms.tutorial2;
              CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial3);
              Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
              Player.position.X -= (float) MapsManager.maps[1].RoomWidthtPx;
              FireBallsManager.Reset();
              LavaGeyserManager.Reset();
              ProjectilesManager.Reset();
              return true;
            }
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial1;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial2;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial1);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial4;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial2;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial4);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.tutorial3:
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial2;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial3;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial2);
            Player.position.X += (float) MapsManager.maps[1].RoomWidthtPx;
            Player.position.Y = -10f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial1;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial3;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial1);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower0;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial3;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower0);
            Player.position.X = 0.0f;
            Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - MapsManager.maps[3].RoomHeightPx);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.tutorial4:
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial2;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial4;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial2);
            Player.position.X = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial0;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial4;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial0);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.escape2;
            RoomsManager.previousRoom = RoomsManager.Rooms.tutorial4;
            CameraManager.SwitchCamera(RoomsManager.Rooms.escape2);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.churchBellTower0:
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial3;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial3);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - MapsManager.maps[5].RoomHeightPx);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower1;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower1);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            if ((double) Player.position.Y > 496.0)
            {
              RoomsManager.currentRoom = RoomsManager.Rooms.churchGroundFloor0;
              RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower0;
              CameraManager.SwitchCamera(RoomsManager.Rooms.churchGroundFloor0);
              Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - MapsManager.maps[5].RoomHeightPx);
              Player.position.X = 0.0f;
              FireBallsManager.Reset();
              LavaGeyserManager.Reset();
              ProjectilesManager.Reset();
              return true;
            }
            if ((double) Player.position.Y > 248.0)
            {
              RoomsManager.currentRoom = RoomsManager.Rooms.church1stFloor0;
              RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower0;
              CameraManager.SwitchCamera(RoomsManager.Rooms.church1stFloor0);
              Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx + MapsManager.maps[9].RoomHeightPx - MapsManager.maps[5].RoomHeightPx);
              Player.position.X = 0.0f;
              FireBallsManager.Reset();
              LavaGeyserManager.Reset();
              ProjectilesManager.Reset();
              return true;
            }
            RoomsManager.currentRoom = RoomsManager.Rooms.church2ndFloor0;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.church2ndFloor0);
            Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx + MapsManager.maps[11].RoomHeightPx + MapsManager.maps[9].RoomHeightPx - MapsManager.maps[5].RoomHeightPx);
            Player.position.X = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.churchBellTower1:
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower0;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower1;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower0);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower2;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower1;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower2);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.churchBellTower2:
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower1;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower2;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower1);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.midBoss;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchBellTower2;
            CameraManager.SwitchCamera(RoomsManager.Rooms.midBoss);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.midBoss:
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower2;
            RoomsManager.previousRoom = RoomsManager.Rooms.midBoss;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower2);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.churchGroundFloor0:
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower0;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchGroundFloor0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower0);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - MapsManager.maps[9].RoomHeightPx);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchAltarRoom;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchGroundFloor0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchAltarRoom);
            Player.position.X = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.churchAltarRoom:
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchGroundFloor0;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchAltarRoom;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchGroundFloor0);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.church1stFloor0;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchAltarRoom;
            CameraManager.SwitchCamera(RoomsManager.Rooms.church1stFloor0);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            Player.position.X += (float) MapsManager.maps[9].RoomWidthtPx;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.descent;
            RoomsManager.previousRoom = RoomsManager.Rooms.churchAltarRoom;
            CameraManager.SwitchCamera(RoomsManager.Rooms.descent);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.church1stFloor0:
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchAltarRoom;
            RoomsManager.previousRoom = RoomsManager.Rooms.church1stFloor0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchAltarRoom);
            Player.position.Y = 0.0f;
            Player.position.X -= (float) MapsManager.maps[9].RoomWidthtPx;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.church2ndFloor0;
            RoomsManager.previousRoom = RoomsManager.Rooms.church1stFloor0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.church2ndFloor0);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower0;
            RoomsManager.previousRoom = RoomsManager.Rooms.church1stFloor0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower0);
            Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - MapsManager.maps[11].RoomHeightPx - MapsManager.maps[9].RoomHeightPx);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.church2ndFloor0:
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchBellTower0;
            RoomsManager.previousRoom = RoomsManager.Rooms.church2ndFloor0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchBellTower0);
            Player.position.Y += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - MapsManager.maps[11].RoomHeightPx - MapsManager.maps[9].RoomHeightPx - MapsManager.maps[12].RoomHeightPx);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.church1stFloor0;
            RoomsManager.previousRoom = RoomsManager.Rooms.church2ndFloor0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.church1stFloor0);
            Player.position.Y = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.descent:
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.churchAltarRoom;
            RoomsManager.previousRoom = RoomsManager.Rooms.descent;
            CameraManager.SwitchCamera(RoomsManager.Rooms.churchAltarRoom);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.finalBoss;
            RoomsManager.previousRoom = RoomsManager.Rooms.descent;
            Game1.Zoom0Dot5();
            CameraManager.SwitchCamera(RoomsManager.Rooms.finalBoss);
            Player.position.Y = 0.0f;
            Player.position.X += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - MapsManager.maps[13].RoomWidthtPx);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          break;
        case RoomsManager.Rooms.finalBoss:
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.escape0;
            RoomsManager.previousRoom = RoomsManager.Rooms.finalBoss;
            Game1.Zoom1();
            CameraManager.SwitchCamera(RoomsManager.Rooms.escape0);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.descent;
            RoomsManager.previousRoom = RoomsManager.Rooms.finalBoss;
            Game1.Zoom1();
            CameraManager.SwitchCamera(RoomsManager.Rooms.descent);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            Player.position.X += (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - MapsManager.maps[14].RoomWidthtPx);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx)
          {
            Player.takeDamage(Player.maxHealthPoints, true);
            break;
          }
          break;
        case RoomsManager.Rooms.escape0:
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.finalBoss;
            RoomsManager.previousRoom = RoomsManager.Rooms.escape0;
            Game1.Zoom0Dot5();
            CameraManager.SwitchCamera(RoomsManager.Rooms.finalBoss);
            Player.position.X = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.escape1;
            RoomsManager.previousRoom = RoomsManager.Rooms.escape0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.escape1);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            Player.position.Y += (float) (MapsManager.maps[13].RoomHeightPx - MapsManager.maps[2].RoomHeightPx);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx + 24))
          {
            Player.takeDamage(Player.maxHealthPoints, true);
            break;
          }
          break;
        case RoomsManager.Rooms.escape1:
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.escape0;
            RoomsManager.previousRoom = RoomsManager.Rooms.escape1;
            CameraManager.SwitchCamera(RoomsManager.Rooms.escape0);
            Player.position.X = 0.0f;
            Player.position.Y -= (float) (MapsManager.maps[13].RoomHeightPx - MapsManager.maps[2].RoomHeightPx);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.X + (double) Player.width < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.escape2;
            RoomsManager.previousRoom = RoomsManager.Rooms.escape1;
            CameraManager.SwitchCamera(RoomsManager.Rooms.escape2);
            Player.position.X = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx - Player.width);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx + 24))
          {
            Player.takeDamage(Player.maxHealthPoints, true);
            break;
          }
          break;
        case RoomsManager.Rooms.escape2:
          if ((double) Player.position.X > (double) MapsManager.maps[(int) RoomsManager.currentRoom].RoomWidthtPx)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.escape1;
            RoomsManager.previousRoom = RoomsManager.Rooms.escape2;
            CameraManager.SwitchCamera(RoomsManager.Rooms.escape1);
            Player.position.X = 0.0f;
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y + (double) Player.height < 0.0)
          {
            RoomsManager.currentRoom = RoomsManager.Rooms.tutorial4;
            RoomsManager.previousRoom = RoomsManager.Rooms.escape2;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial4);
            Player.position.Y = (float) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx - Player.height);
            FireBallsManager.Reset();
            LavaGeyserManager.Reset();
            ProjectilesManager.Reset();
            return true;
          }
          if ((double) Player.position.Y > (double) (MapsManager.maps[(int) RoomsManager.currentRoom].RoomHeightPx + 24))
          {
            Player.takeDamage(Player.maxHealthPoints, true);
            break;
          }
          break;
      }
      return false;
    }

    public static void Update(float elapsedTime)
    {
      if (RoomsManager.switchRoom())
        LoadSaveManager.SaveGameProgress();
      CameraManager.Update(elapsedTime);
      MapsManager.maps[(int) RoomsManager.currentRoom].Update(elapsedTime);
      if (RoomsManager.currentRoom == RoomsManager.Rooms.finalBoss)
        FinalBoss.Update(elapsedTime);
      if (RoomsManager.currentRoom == RoomsManager.Rooms.midBoss)
        MidBoss.Update(elapsedTime);
      EnemyManager.enemyRoomManagers[(int) RoomsManager.currentRoom].Update(elapsedTime);
      FireBallsManager.Update(elapsedTime);
      LavaGeyserManager.Update(elapsedTime);
      PlatformsManager.platformsRoomManagers[(int) RoomsManager.currentRoom].Update(elapsedTime);
      CollectablesManager.collectablesRoomManagers[(int) RoomsManager.currentRoom].Update(elapsedTime);
      MonologuesManager.monologuesRoomManagers[(int) RoomsManager.currentRoom].Update(elapsedTime);
      DoorsManager.doorsRoomManagers[(int) RoomsManager.currentRoom].Update();
      AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.currentRoom].Update(elapsedTime);
      TorchManager.Update(elapsedTime);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.currentRoom].DrawOnTheBack(spriteBatch);
      TorchManager.Draw(spriteBatch);
      PlatformsManager.platformsRoomManagers[(int) RoomsManager.currentRoom].Draw(spriteBatch);
      switch (RoomsManager.currentRoom)
      {
        case RoomsManager.Rooms.midBoss:
          MidBoss.Draw(spriteBatch);
          break;
        case RoomsManager.Rooms.finalBoss:
          FinalBoss.Draw(spriteBatch);
          break;
      }
      EnemyManager.enemyRoomManagers[(int) RoomsManager.currentRoom].Draw(spriteBatch);
      CollectablesManager.collectablesRoomManagers[(int) RoomsManager.currentRoom].Draw(spriteBatch);
      ProjectilesManager.Draw(spriteBatch);
      Player.Draw(spriteBatch);
      MapsManager.maps[(int) RoomsManager.currentRoom].Draw(spriteBatch);
      DoorsManager.doorsRoomManagers[(int) RoomsManager.currentRoom].Draw(spriteBatch);
      AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.currentRoom].DrawInFront(spriteBatch);
      FireBallsManager.Draw(spriteBatch);
      LavaGeyserManager.Draw(spriteBatch);
      MonologuesManager.monologuesRoomManagers[(int) RoomsManager.currentRoom].Draw(spriteBatch);
    }

    public enum Rooms
    {
      tutorial0,
      tutorial1,
      tutorial2,
      tutorial3,
      tutorial4,
      churchBellTower0,
      churchBellTower1,
      churchBellTower2,
      midBoss,
      churchGroundFloor0,
      churchAltarRoom,
      church1stFloor0,
      church2ndFloor0,
      descent,
      finalBoss,
      escape0,
      escape1,
      escape2,
      total,
    }
  }
}
