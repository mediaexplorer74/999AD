
// Type: GameManager.Door
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace GameManager
{
  internal class Door
  {
    public static int IDcounter;
    private static Texture2D spritesheet;
    private static Rectangle[] sourceRectangles;
    private Point position;
    private Collectable.ItemType key;
    private Door.TextureType textureType;
    private bool closed;

    public int ID { get; private set; }

    public Door(Point _position, Door.TextureType _type, Collectable.ItemType _key)
    {
      this.ID = Door.IDcounter;
      ++Door.IDcounter;
      this.position = _position;
      this.key = _key;
      this.textureType = _type;
      this.closed = true;
    }

    public static void Inizialize(Texture2D _spritesheet)
    {
      Door.spritesheet = _spritesheet;
      Door.sourceRectangles = new Rectangle[4]
      {
        new Rectangle(0, 0, 16, 40),
        new Rectangle(112, 0, 16, 40),
        new Rectangle(224, 0, 16, 40),
        new Rectangle(336, 0, 16, 40)
      };
    }

    private Rectangle InteractionRectangle
    {
      get
      {
        return new Rectangle(this.position.X - 16, this.position.Y, Door.sourceRectangles[(int) this.textureType].Width + 32, Door.sourceRectangles[(int) this.textureType].Height);
      }
    }

    private Rectangle DrawRectangle
    {
      get
      {
        return new Rectangle(this.position.X, this.position.Y, Door.sourceRectangles[(int) this.textureType].Width, Door.sourceRectangles[(int) this.textureType].Height);
      }
    }

    public bool Closed => this.closed;

    public void LockDoor(RoomsManager.Rooms room)
    {
      int num1 = this.position.Y / Tile.tileSize;
      int num2 = (this.position.Y + Door.sourceRectangles[(int) this.textureType].Height - 1) / Tile.tileSize;
      int num3 = this.position.X / Tile.tileSize;
      int num4 = (this.position.X + Door.sourceRectangles[(int) this.textureType].Width - 1) / Tile.tileSize;
      for (int index1 = num1; index1 <= num2; ++index1)
      {
                for (int index2 = num3; index2 <= num4; ++index2)
                {
                    try
                    {
                        MapsManager.maps[(int)room].array[index1, index2].tileType = Tile.TileType.solidEmpty;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("[ex] [Door] Exception: " + ex.Message + " [ " 
                            + ex.ToString() + " ] ");
                    }
                }
      }
    }

    public void Update()
    {
      if (!this.InteractionRectangle.Intersects(Player.CollisionRectangle) || !CollectablesManager.TryRemoveFromInventory(this.key, new Vector2((float) this.DrawRectangle.Center.X, (float) this.DrawRectangle.Center.Y)))
        return;
      this.OpenDoor((int) RoomsManager.CurrentRoom);
      AnimatedSpritesManager.animatedSpritesRoomManagers[(int) RoomsManager.CurrentRoom].AddTempAnimatedSprite(new AnimatedSprite(new Vector2((float) this.position.X, (float) this.position.Y), AnimatedSprite.GetDoorAnimation(this.textureType)));
    }

    public void OpenDoor(int roomType)
    {
      this.closed = false;
      int num1 = this.position.Y / Tile.tileSize;
      int num2 = (this.position.Y + Door.sourceRectangles[(int) this.textureType].Height - 1) / Tile.tileSize;
      int num3 = this.position.X / Tile.tileSize;
      int num4 = (this.position.X + Door.sourceRectangles[(int) this.textureType].Width - 1) / Tile.tileSize;
      for (int index1 = num1; index1 <= num2; ++index1)
      {
        for (int index2 = num3; index2 <= num4; ++index2)
          MapsManager.maps[roomType].array[index1, index2].tileType = Tile.TileType.empty;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Door.spritesheet, Camera.RelativeRectangle(this.DrawRectangle), new Rectangle?(Door.sourceRectangles[(int) this.textureType]), Color.White);
    }

    public enum TextureType
    {
      brassDoor,
      goldDoor,
      bronzeDoor,
      silverDoor,
      total,
    }
  }
}
