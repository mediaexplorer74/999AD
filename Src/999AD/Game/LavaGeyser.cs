
// Type: GameManager.LavaGeyser
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class LavaGeyser
  {
    public static Texture2D spritesheet;
    public static Rectangle whiteTexture;
    public static readonly int size = 24;
    public static readonly int heigthBeforeErupting = 40;
    private float initialYVelocity;
    private float yVelocity;
    private static Rectangle geyserTop;
    private static Rectangle geyserBody;
    private Vector2 position;
    private float timeBeforeErupting;
    private float elapsedTimeBeforeErupting;
    private bool erupted;

    public LavaGeyser(float xCenterPosition, float _timeBeforeErupting, int _initialYvelocity = -1200)
    {
      this.yVelocity = 0.0f;
      this.initialYVelocity = (float) _initialYvelocity;
      this.elapsedTimeBeforeErupting = 0.0f;
      this.erupted = false;
      this.position = new Vector2(xCenterPosition - 0.5f * (float) LavaGeyser.size, (float) MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx);
      this.timeBeforeErupting = _timeBeforeErupting;
      LavaGeyser.geyserTop = new Rectangle(0, 173, LavaGeyser.size, LavaGeyser.size);
      LavaGeyser.geyserBody = new Rectangle(0, 197, LavaGeyser.size, LavaGeyser.size);
    }

    public static void Inizialize(Texture2D _spritesheet)
    {
      LavaGeyser.spritesheet = _spritesheet;
      LavaGeyser.whiteTexture = new Rectangle(400, 40, 8, 8);
    }

    public Rectangle CollisionRectangle
    {
      get
      {
        return new Rectangle((int) this.position.X, (int) this.position.Y + LavaGeyser.size / 3, LavaGeyser.size, MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx - (int) this.position.Y - LavaGeyser.size / 3);
      }
    }

    public bool isActive()
    {
      return (double) this.position.Y <= (double) MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx;
    }

    public void Update(float elapsedTime)
    {
      if ((double) this.elapsedTimeBeforeErupting <= 9.9999997473787516E-05)
      {
        if ((double) this.position.Y <= (double) (MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx - LavaGeyser.heigthBeforeErupting))
        {
          this.position.Y = (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx - LavaGeyser.heigthBeforeErupting);
          this.elapsedTimeBeforeErupting += elapsedTime;
        }
        else
          this.position.Y -= 2f;
      }
      else if (!this.erupted)
      {
        if ((double) this.elapsedTimeBeforeErupting > (double) this.timeBeforeErupting)
        {
          this.yVelocity = this.initialYVelocity;
          this.erupted = true;
        }
        else
          this.elapsedTimeBeforeErupting += elapsedTime;
      }
      else
      {
        this.position.Y += this.yVelocity * elapsedTime;
        this.yVelocity += Gravity.gravityAcceleration * elapsedTime;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(LavaGeyser.spritesheet, Camera.RelativeRectangle(new Vector2(this.position.X, 0.0f), LavaGeyser.size, MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx), new Rectangle?(LavaGeyser.whiteTexture), Color.Red * 0.3f);
      spriteBatch.Draw(LavaGeyser.spritesheet, Camera.RelativeRectangle(this.position, LavaGeyser.size, LavaGeyser.size), new Rectangle?(LavaGeyser.geyserTop), Color.White);
      for (int index = 1; index <= (MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx - (int) this.position.Y + LavaGeyser.size - 1) / LavaGeyser.size; ++index)
        spriteBatch.Draw(LavaGeyser.spritesheet, Camera.RelativeRectangle(this.position + new Vector2(0.0f, (float) (index * LavaGeyser.size)), LavaGeyser.size, LavaGeyser.size), new Rectangle?(LavaGeyser.geyserBody), Color.White);
    }
  }
}
