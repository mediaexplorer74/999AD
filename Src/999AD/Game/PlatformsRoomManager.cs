
// Type: GameManager.PlatformsRoomManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class PlatformsRoomManager
  {
    public MovingPlatform[] movingPlatforms;
    private MovingPlatform[] backUpMovingPlatforms;

    public PlatformsRoomManager(MovingPlatform[] _movingPlatforms)
    {
      this.movingPlatforms = _movingPlatforms;
      this.backUpMovingPlatforms = new MovingPlatform[this.movingPlatforms.Length];
      for (int index = 0; index < this.movingPlatforms.Length; ++index)
        this.backUpMovingPlatforms[index] = this.movingPlatforms[index].DeepCopy();
    }

    public void Update(float elapsedTime)
    {
      foreach (MovingPlatform movingPlatform in this.movingPlatforms)
        movingPlatform.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (MovingPlatform movingPlatform in this.movingPlatforms)
      {
        if (movingPlatform.Rectangle.Intersects(Camera.Rectangle))
          movingPlatform.Draw(spriteBatch);
      }
    }

    public void Reset()
    {
      for (int index = 0; index < this.movingPlatforms.Length; ++index)
        this.movingPlatforms[index] = this.backUpMovingPlatforms[index].DeepCopy();
    }
  }
}
