
// Type: GameManager.Projectile
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class Projectile
  {
    private static Texture2D spritesheet;
    private static Animation[] animations;
    private Animation animation;
    public readonly int width;
    public readonly int height;
    private Vector2 position;
    private Vector2 velocity;
    public bool active;

    public Projectile(
      Vector2 _position,
      Vector2 _initialVelocity,
      Projectile.SpriteType _spriteType)
    {
      this.position = _position;
      this.velocity = _initialVelocity;
      this.animation = Projectile.animations[(int) _spriteType].DeepCopy();
      this.active = true;
      this.width = this.animation.Frame.Width;
      this.height = this.animation.Frame.Height;
    }

    public static void Inizialize(Texture2D _spritesheet)
    {
      Projectile.spritesheet = _spritesheet;
      Projectile.animations = new Animation[2]
      {
        new Animation(new Rectangle(336, 64, 16, 4), 4, 4, 4, 0.1f, true),
        new Animation(new Rectangle(336, 58, 24, 6), 6, 6, 4, 0.1f, true)
      };
    }

    public Rectangle Rectangle
    {
      get => new Rectangle((int) this.position.X, (int) this.position.Y, this.width, this.height);
    }

    public void Update(float elapsedTime)
    {
      this.animation.Update(elapsedTime);
      Gravity.MoveDestructableObject(ref this.velocity, ref this.position, this.width, this.height, ref this.active, elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Projectile.spritesheet, Camera.RelativeRectangle(this.position, this.width, this.height), new Rectangle?(this.animation.Frame), Color.White);
    }

    public enum SpriteType
    {
      holyWater,
      lava,
      total,
    }
  }
}
