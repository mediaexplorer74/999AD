
// Type: GameManager.RoomMap
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GameManager
{
  internal class RoomMap
  {
    public readonly int roomHeightTiles;
    public readonly int roomWidthTiles;
    public Tile[,] array;
    private bool removeRowSFX;
    private static float timeBoforeRemovingNextTile;
    private static float timer;
    private static List<int[]> tilesToRemove;
    private int removeTogether;

    public RoomMap(int _roomHeightTiles, int _roomWidthTiles)
    {
      this.roomWidthTiles = _roomWidthTiles;
      this.roomHeightTiles = _roomHeightTiles;
      this.removeRowSFX = false;
      RoomMap.timeBoforeRemovingNextTile = 0.0f;
      RoomMap.timer = 0.0f;
      RoomMap.tilesToRemove = new List<int[]>();
      this.removeTogether = 1;
      this.array = new Tile[this.roomHeightTiles, this.roomWidthTiles];
      for (int index1 = 0; index1 < this.roomHeightTiles; ++index1)
      {
        for (int index2 = 0; index2 < this.roomWidthTiles; ++index2)
          this.array[index1, index2] = new Tile(new Vector2((float) (index2 * Tile.tileSize), (float) (index1 * Tile.tileSize)));
      }
    }

    public int RoomHeightPx => this.roomHeightTiles * Tile.tileSize;

    public int RoomWidthtPx => this.roomWidthTiles * Tile.tileSize;

    public void Update(float elapsedTime)
    {
      if (!this.removeRowSFX)
        return;
      RoomMap.timer += elapsedTime;
      if ((double) RoomMap.timer < (double) RoomMap.timeBoforeRemovingNextTile)
        return;
      for (int index = 0; index < this.removeTogether; ++index)
      {
        if (RoomMap.tilesToRemove.Count == 0)
        {
          this.removeRowSFX = false;
          break;
        }
        this.array[RoomMap.tilesToRemove[0][0], RoomMap.tilesToRemove[0][1]].tileType = Tile.TileType.empty;
        RoomMap.tilesToRemove.RemoveAt(0);
      }
      RoomMap.timer = 0.0f;
    }

    public void RemoveGroupOfTiles(
      List<int[]> _tilesToRemove,
      float _timeBeforeRemovingNextTile,
      int _removeTogether,
      float delay = 0.0f)
    {
      if (this.removeRowSFX)
        return;
      this.removeRowSFX = true;
      RoomMap.tilesToRemove = _tilesToRemove;
      RoomMap.timeBoforeRemovingNextTile = _timeBeforeRemovingNextTile;
      this.removeTogether = _removeTogether;
      RoomMap.timer = -delay;
    }

    public bool HarmfulTileIntersectsRectangle(Rectangle collisionRect)
    {
      int num1 = MathHelper.Clamp(collisionRect.Y / Tile.tileSize, 0, this.roomHeightTiles - 1);
      int num2 = MathHelper.Clamp((collisionRect.Bottom - 1) / Tile.tileSize, 0, this.roomHeightTiles - 1);
      int num3 = MathHelper.Clamp(collisionRect.X / Tile.tileSize, 0, this.roomWidthTiles - 1);
      int num4 = MathHelper.Clamp((collisionRect.Right - 1) / Tile.tileSize, 0, this.roomWidthTiles - 1);
      for (int index1 = num1; index1 <= num2; ++index1)
      {
        for (int index2 = num3; index2 <= num4; ++index2)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index1, index2].isHarmful())
            return true;
        }
      }
      return false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      for (int index1 = Camera.Rectangle.Y / Tile.tileSize; index1 <= (Camera.Rectangle.Bottom - 1) / Tile.tileSize; ++index1)
      {
        for (int index2 = Camera.Rectangle.X / Tile.tileSize; index2 <= (Camera.Rectangle.Right - 1) / Tile.tileSize; ++index2)
        {
          if (index1 >= 0 && index1 < this.roomHeightTiles && index2 >= 0 && index2 < this.roomWidthTiles)
            this.array[index1, index2].Draw(spriteBatch);
        }
      }
    }
  }
}
