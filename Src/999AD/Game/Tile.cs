
// Type: GameManager.Tile
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class Tile
  {
    public static readonly int tileSize = 8;
    private static Texture2D spritesheet;
    private static Rectangle[] sourceRectangles = new Rectangle[68];
    public Tile.TileType tileType;
    private Vector2 position;

    public Tile(Vector2 _position)
    {
      this.tileType = Tile.TileType.empty;
      this.position = _position;
    }

    public static void LoadTextures(Texture2D _spritesheet)
    {
      Tile.spritesheet = _spritesheet;
      for (int index = 0; index < 68; ++index)
        Tile.sourceRectangles[index] = new Rectangle(index % (Tile.spritesheet.Width / Tile.tileSize) * Tile.tileSize, index / (Tile.spritesheet.Width / Tile.tileSize) * Tile.tileSize, Tile.tileSize, Tile.tileSize);
    }

    private Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int) this.position.X, (int) this.position.Y, Tile.tileSize, Tile.tileSize);
      }
    }

    public bool isSolid() => this.tileType >= Tile.TileType.solidEmpty && !this.isHarmful();

    public bool isHarmful()
    {
      return this.tileType >= Tile.TileType.caveSpikeL && ((int) this.tileType % 11 == 0 || (int) this.tileType % 11 == 1);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.tileType == Tile.TileType.empty || this.tileType == Tile.TileType.solidEmpty)
        return;
      spriteBatch.Draw(Tile.spritesheet, Camera.RelativeRectangle(this.Rectangle), new Rectangle?(Tile.sourceRectangles[(int) this.tileType]), Color.White);
    }

    public static void DrawAtLocation(
      SpriteBatch spriteBatch,
      int tileType,
      Vector2 screenPosition)
    {
      spriteBatch.Draw(Tile.spritesheet, screenPosition, new Rectangle?(Tile.sourceRectangles[tileType]), Color.White);
    }

    public enum TileType
    {
      empty,
      solidEmpty,
      caveOutTL,
      caveOutT,
      caveOutTR,
      dirtOutTL,
      dirtOutT,
      dirtOutTR,
      marbleOutTL,
      marbleOutT,
      marbleOutTR,
      caveSpikeL,
      caveSpikeR,
      caveOutL,
      caveOut,
      caveOutR,
      dirtOutL,
      dirtOut,
      dirtOutR,
      marbleOutL,
      marbleOut,
      marbleOutR,
      caveSpikeT,
      caveSpikeB,
      caveOutBL,
      caveOutB,
      caveOutBR,
      dirtOutBL,
      dirtOutB,
      dirtOutBR,
      marbleOutBL,
      marbleOutB,
      marbleOutBR,
      marbleSpikeL,
      marbleSpikeR,
      caveInTL,
      caveInT,
      caveInTR,
      dirtInTL,
      dirtInT,
      dirtInTR,
      marbleInTL,
      marbleInT,
      marbleInTR,
      marbleSpikeT,
      marbleSpikeB,
      caveInL,
      caveIn,
      caveInR,
      dirtInL,
      dirtIn,
      dirtInR,
      marbleInL,
      marbleIn,
      marbleInR,
      dirtSpikeL,
      dirtSpikeR,
      caveInBL,
      caveInB,
      caveInBR,
      dirtInBL,
      dirtInB,
      dirtInBR,
      marbleInBL,
      marbleInB,
      marbleInBR,
      dirtSpikeT,
      dirtSpikeB,
      total,
    }
  }
}
