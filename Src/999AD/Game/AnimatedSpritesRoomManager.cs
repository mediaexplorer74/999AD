
// Type: GameManager.AnimatedSpritesRoomManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GameManager
{
  internal class AnimatedSpritesRoomManager
  {
    private List<AnimatedSprite> tempAnimatedSprites;
    private List<AnimatedSprite> animaterSprites;

    public AnimatedSpritesRoomManager(AnimatedSprite[] _animatedSprites)
    {
      this.tempAnimatedSprites = new List<AnimatedSprite>((IEnumerable<AnimatedSprite>) _animatedSprites);
      this.animaterSprites = new List<AnimatedSprite>((IEnumerable<AnimatedSprite>) _animatedSprites);
    }

    public void Update(float elapsedTime)
    {
      for (int index = this.tempAnimatedSprites.Count - 1; index >= 0; --index)
      {
        if (this.tempAnimatedSprites[index].Active)
          this.tempAnimatedSprites[index].Update(elapsedTime);
        else
          this.tempAnimatedSprites.RemoveAt(index);
      }
      for (int index = 0; index < this.animaterSprites.Count; ++index)
        this.animaterSprites[index].Update(elapsedTime);
    }

    public void AddTempAnimatedSprite(AnimatedSprite animatedSprite)
    {
      this.tempAnimatedSprites.Add(animatedSprite);
    }

    public void ClearAnimatedSprites() => this.animaterSprites.Clear();

    public void AddAnimatedSprites(List<AnimatedSprite> animatedSprites)
    {
      this.animaterSprites = animatedSprites;
    }

    public void DrawInFront(SpriteBatch spriteBatch)
    {
      foreach (AnimatedSprite tempAnimatedSprite in this.tempAnimatedSprites)
      {
        if (tempAnimatedSprite.DrawInFront)
          tempAnimatedSprite.Draw(spriteBatch);
      }
      foreach (AnimatedSprite animaterSprite in this.animaterSprites)
      {
        if (animaterSprite.DrawInFront)
          animaterSprite.Draw(spriteBatch);
      }
    }

    public void DrawOnTheBack(SpriteBatch spriteBatch)
    {
      foreach (AnimatedSprite tempAnimatedSprite in this.tempAnimatedSprites)
      {
        if (!tempAnimatedSprite.DrawInFront)
          tempAnimatedSprite.Draw(spriteBatch);
      }
      foreach (AnimatedSprite animaterSprite in this.animaterSprites)
      {
        if (!animaterSprite.DrawInFront)
          animaterSprite.Draw(spriteBatch);
      }
    }
  }
}
