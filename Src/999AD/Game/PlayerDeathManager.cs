
// Type: GameManager.PlayerDeathManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace GameManager
{
  internal static class PlayerDeathManager
  {
    private static Texture2D background;
    private static Texture2D spritesheet;
    private static Rectangle sourceRectangle_Continue;
    private static Vector2 position_Continue;
    private static float alphaValue;
    private static readonly float timeBeforeResuming = 4f;
    private static float elapsedTimeBeforeResuming;

    public static void Initialize(Texture2D _background, Texture2D _spritesheetOptions)
    {
      PlayerDeathManager.background = _background;
      PlayerDeathManager.spritesheet = _spritesheetOptions;
      PlayerDeathManager.sourceRectangle_Continue = new Rectangle(0, 96, 160, 24);
      PlayerDeathManager.position_Continue = new Vector2((float)
          ((Game1.minViewportWidth - PlayerDeathManager.sourceRectangle_Continue.Width) / 2), 150f);
      PlayerDeathManager.alphaValue = 0.0f;
      PlayerDeathManager.elapsedTimeBeforeResuming = 0.0f;
    }

    public static void Update(float elapsedTime)
    {
      if ((double) PlayerDeathManager.elapsedTimeBeforeResuming < (double) PlayerDeathManager.timeBeforeResuming)
      {
        PlayerDeathManager.elapsedTimeBeforeResuming += elapsedTime;
        PlayerDeathManager.alphaValue = MathHelper.Lerp(0.0f, 1f, PlayerDeathManager.elapsedTimeBeforeResuming / PlayerDeathManager.timeBeforeResuming);
        if ((double) PlayerDeathManager.elapsedTimeBeforeResuming < (double) PlayerDeathManager.timeBeforeResuming)
          return;
        PlayerDeathManager.alphaValue = 1f;
      }
      else
      {
        if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter)
                    && Game1.currentGamePad.Buttons.A != ButtonState.Pressed)
          return;
        Player.ReplenishHealth();
        PlayerDeathManager.ResetVariables();
        LoadSaveManager.SaveGameProgress();
      }
    }

    public static void ResetVariables()
    {
      Player.IsOnMovingPlatform = false;
      PlayerDeathManager.elapsedTimeBeforeResuming = 0.0f;
      Game1.currentGameState = Game1.GameStates.playing;
      CameraManager.Reset();
      LavaGeyserManager.Reset();
      FireBallsManager.Reset();
      ProjectilesManager.Reset();
      EnemyManager.Reset();

      if (!MidBoss.Dead)
        MidBoss.Reset();

      CollectablesManager.ResetHearts();
      GameEvents.Reset();

      // RnD: current room reset (hack)
      RoomsManager.CurrentRoom = RoomsManager.Rooms.churchBellTower0;

      switch (RoomsManager.CurrentRoom)
      {
        case RoomsManager.Rooms.tutorial0:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.tutorial4)
          {
            Player.position = new Vector2(152f, 192f);
            break;
          }
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.tutorial1)
          {
            Player.position = new Vector2(903f, 185f);
            break;
          }
          Player.position = new Vector2(152f, 150f);
          break;
        case RoomsManager.Rooms.tutorial1:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.tutorial0)
          {
            Player.position = new Vector2(18f, 185f);
            break;
          }
          Player.position = new Vector2(550f, 193f);
          break;
        case RoomsManager.Rooms.tutorial2:
          if ((double) Player.position.Y < 80.0
                        && RoomsManager.PreviousRoom == RoomsManager.Rooms.tutorial1 
                        && ((double) Player.position.X < 366.0 || (double) Player.position.X > 426.0))
          {
            RoomsManager.CurrentRoom = RoomsManager.Rooms.tutorial1;
            RoomsManager.PreviousRoom = RoomsManager.Rooms.tutorial0;
            CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial1);
            Player.position = new Vector2(18f, 185f);
            break;
          }
          Player.position = new Vector2(388f, 153f);
          break;
        case RoomsManager.Rooms.tutorial3:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
          {
            Player.position = new Vector2(776f, 193f);
            break;
          }
          Player.position = new Vector2(91f, 193f);
          break;
        case RoomsManager.Rooms.tutorial4:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.escape2)
          {
            Player.position = new Vector2(64f, 153f);
            break;
          }
          Player.position = new Vector2(166f, 153f);
          break;
        case RoomsManager.Rooms.churchBellTower0:
          switch (RoomsManager.PreviousRoom)
          {
            case RoomsManager.Rooms.tutorial3:
              Player.position = new Vector2(23f, 961f);
              return;
            case RoomsManager.Rooms.churchBellTower1:
              if ((double) Player.position.X < 436.0 && (double) Player.position.X >= 60.0)
              {
                Player.position = new Vector2(405f, 161f);
                return;
              }
              if ((double) Player.position.X >= 436.0)
              {
                Player.position = new Vector2(456f, 961f);
                return;
              }
              Player.position = new Vector2(23f, 961f);
              return;
            default:
              Player.position = new Vector2(456f, 961f);
              return;
          }
        case RoomsManager.Rooms.churchBellTower1:
          if ((double) Player.position.X >= 436.0)
          {
            Player.position = new Vector2(458f, 1097f);
            break;
          }
          if ((double) Player.position.X < 60.0)
          {
            Player.position = new Vector2(23f, 1097f);
            break;
          }
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
          {
            Player.position = new Vector2(372f, 1097f);
            break;
          }
          Player.position = new Vector2(111f, 121f);
          break;
        case RoomsManager.Rooms.churchBellTower2:
          if ((double) Player.position.X >= 436.0 && (double) Player.position.Y > 110.0)
          {
            Player.position = new Vector2(464f, 1177f);
            break;
          }
          if ((double) Player.position.X >= 60.0)
          {
            if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower1)
            {
              Player.position = new Vector2(119f, 1177f);
              break;
            }
            Player.position = new Vector2(457f, 65f);
            break;
          }
          Player.position = new Vector2(23f, 1177f);
          break;
        case RoomsManager.Rooms.midBoss:
          Player.position = new Vector2(452f, 25f);
          break;
        case RoomsManager.Rooms.churchGroundFloor0:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
          {
            Player.position = new Vector2(22f, 465f);
            break;
          }
          Player.position = new Vector2(1160f, 465f);
          break;
        case RoomsManager.Rooms.churchAltarRoom:
          if ((double) Player.position.X < 104.0)
          {
            Player.position = new Vector2(22f, 465f);
            break;
          }
          Player.position = new Vector2(250f, 465f);
          break;
        case RoomsManager.Rooms.church1stFloor0:
          if ((double) Player.position.X < 1288.0)
          {
            if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchAltarRoom)
            {
              Player.position = new Vector2(1208f, 217f);
              break;
            }
            Player.position = new Vector2(18f, 217f);
            break;
          }
          Player.position = new Vector2(1320f, 217f);
          break;
        case RoomsManager.Rooms.church2ndFloor0:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchBellTower0)
          {
            Player.position = new Vector2(37f, 481f);
            break;
          }
          Player.position = new Vector2(1289f, 481f);
          break;
        case RoomsManager.Rooms.descent:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.churchAltarRoom)
          {
            Player.position = new Vector2(339f, 121f);
            break;
          }
          Player.position = new Vector2(318f, 233f);
          break;
        case RoomsManager.Rooms.finalBoss:
          Game1.Zoom0Dot5();
          MediaPlayer.Stop();
          if (!FinalBoss.Dead)
          {
            Player.position = new Vector2(734f, 41f);
            break;
          }
          Player.position = new Vector2(17f, 41f);
          break;
        case RoomsManager.Rooms.escape0:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.finalBoss)
          {
            Player.position = new Vector2(1726f, 129f);
            break;
          }
          Player.position = new Vector2(18f, 49f);
          break;
        case RoomsManager.Rooms.escape1:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.escape0)
          {
            Player.position = new Vector2(1320f, 89f);
            break;
          }
          Player.position = new Vector2(12f, 289f);
          break;
        case RoomsManager.Rooms.escape2:
          if (RoomsManager.PreviousRoom == RoomsManager.Rooms.escape1)
          {
            Player.position = new Vector2(500f, 289f);
            break;
          }
          Player.position = new Vector2(21f, 129f);
          break;
      }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(PlayerDeathManager.background, new Vector2(0.0f, 0.0f), 
          Color.White * PlayerDeathManager.alphaValue);

      spriteBatch.Draw(PlayerDeathManager.spritesheet, 
          PlayerDeathManager.position_Continue, 
          new Rectangle?(PlayerDeathManager.sourceRectangle_Continue),
          Color.Red * PlayerDeathManager.alphaValue);
    }
  }
}
