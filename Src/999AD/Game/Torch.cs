
// Type: GameManager.Torch
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class Torch
  {
    private static Texture2D spritesheet;
    private static Animation litAnim;
    private static Animation unlitAnim;
    private Animation animation;
    private Vector2 position;
    private float timeStaysUnlit;
    private float elapsedTimeUnlit;

    public bool isLit { get; private set; }

    public static void Initialize(Texture2D _spritesheet, Animation _lit, Animation _unlit)
    {
      Torch.spritesheet = _spritesheet;
      Torch.litAnim = _lit;
      Torch.unlitAnim = _unlit;
    }

    public Torch(Vector2 _position)
    {
      this.position = _position;
      this.isLit = true;
      this.animation = Torch.litAnim.DeepCopy();
      this.timeStaysUnlit = 30f;
      this.elapsedTimeUnlit = 0.0f;
    }

    public Rectangle DrawRectangle()
    {
      return new Rectangle((int) this.position.X, (int) this.position.Y, this.animation.Frame.Width, this.animation.Frame.Height);
    }

    public Rectangle CollisionRectangle()
    {
      return new Rectangle((int) this.position.X + 1, (int) this.position.Y + 14, 14, 14);
    }

    public bool TorchHitByRect(Rectangle rect)
    {
      if (!this.isLit || !this.CollisionRectangle().Intersects(rect))
        return false;
      this.isLit = false;
      this.animation = Torch.unlitAnim.DeepCopy();
      return true;
    }

    public void Update(float elapsedTime)
    {
      this.animation.Update(elapsedTime);
      if (this.isLit)
        return;
      this.elapsedTimeUnlit += elapsedTime;
      if ((double) this.elapsedTimeUnlit < (double) this.timeStaysUnlit)
        return;
      this.isLit = true;
      this.animation = Torch.litAnim.DeepCopy();
      this.elapsedTimeUnlit = 0.0f;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Torch.spritesheet, Camera.RelativeRectangle(this.DrawRectangle()), new Rectangle?(this.animation.Frame), Color.White);
    }
  }
}
