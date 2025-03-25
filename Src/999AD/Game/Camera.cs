
// Type: GameManager.Camera
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class Camera
  {
    private static Texture2D background;
    public static Vector2 position;
    private static Rectangle screenRectangle;
    private static int roomWidth;
    private static int roomHeight;

    public static void Inizialize(Texture2D _background, RoomsManager.Rooms _room)
    {
      Camera.background = _background;
      Camera.roomWidth = MapsManager.maps[(int) _room].RoomWidthtPx;
      Camera.roomHeight = MapsManager.maps[(int) _room].RoomHeightPx;
      Camera.position = new Vector2(0.0f, (float) CameraManager.maxOffsetY);
      Camera.screenRectangle = new Rectangle(0, 0, Game1.gameWidth, Game1.gameHeight);
    }

    public static Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int) Camera.position.X, (int) Camera.position.Y, Camera.screenRectangle.Width, Camera.screenRectangle.Height);
      }
    }

    public static Rectangle RelativeRectangle(Vector2 _worldPosition, int _width, int _height)
    {
      return new Rectangle((int) ((double) _worldPosition.X - (double) Camera.position.X), (int) ((double) _worldPosition.Y - (double) Camera.position.Y), _width, _height);
    }

    public static Rectangle RelativeRectangle(Rectangle rect)
    {
      return new Rectangle((int) ((double) rect.X - (double) Camera.position.X), (int) ((double) rect.Y - (double) Camera.position.Y), rect.Width, rect.Height);
    }

    public static Vector2 RelativePoint(Vector2 point)
    {
      return new Vector2(point.X - Camera.position.X, point.Y - Camera.position.Y);
    }

    public static void Update(bool shaking, Vector2 pointLocked)
    {
      pointLocked.X = MathHelper.Clamp(pointLocked.X, (float) Game1.gameWidth / 2f, (float) Camera.roomWidth - (float) Game1.gameWidth / 2f);
      int maxOffsetY = shaking ? CameraManager.maxOffsetY : 0;
      pointLocked.Y = MathHelper.Clamp(pointLocked.Y, (float) Game1.gameHeight / 2f + (float) maxOffsetY, (float) Camera.roomHeight - (float) Game1.gameHeight / 2f - (float) maxOffsetY);
      Camera.position.X = pointLocked.X - (float) Game1.gameWidth / 2f;
      Camera.position.Y = pointLocked.Y - (float) Game1.gameHeight / 2f;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Camera.background, Camera.screenRectangle, new Rectangle?(Camera.Rectangle), Color.White);
    }
  }
}
