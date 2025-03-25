
// Type: GameManager.Collectable
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class Collectable
  {
    public static int IDcounter;
    private static Texture2D spritesheet;
    private static Animation[] animations;
    private Animation animation;
    public readonly Rectangle rectangle;
    public readonly Collectable.ItemType type;
    private bool collected;

    public int ID { get; private set; }

    public Collectable(Point position, Collectable.ItemType _collectableType)
    {
      if (_collectableType == Collectable.ItemType.heart)
      {
        this.ID = -1;
      }
      else
      {
        this.ID = Collectable.IDcounter;
        ++Collectable.IDcounter;
      }
      this.type = _collectableType;
      this.animation = Collectable.animations[(int) this.type].DeepCopy();
      this.collected = false;
      this.rectangle = new Rectangle(position.X, position.Y, this.animation.Frame.Width, this.animation.Frame.Height);
    }

    public static void Inizialize(Texture2D _spritesheet)
    {
      Collectable.spritesheet = _spritesheet;
      Collectable.animations = new Animation[7]
      {
        new Animation(new Rectangle(0, 40, 336, 35), 24, 35, 14, 0.1f, true),
        new Animation(new Rectangle(0, 75, 336, 35), 24, 35, 14, 0.1f, true),
        new Animation(new Rectangle(0, 129, 120, 22), 12, 22, 10, 0.1f, true),
        new Animation(new Rectangle(120, 129, 120, 22), 12, 22, 10, 0.1f, true),
        new Animation(new Rectangle(0, 151, 120, 22), 12, 22, 10, 0.1f, true),
        new Animation(new Rectangle(120, 151, 120, 22), 12, 22, 10, 0.1f, true),
        new Animation(new Rectangle(0, 110, 160, 19), 16, 19, 10, 0.1f, true)
      };
    }

    public bool Collected => this.collected;

    public static Texture2D Sprites => Collectable.spritesheet;

    public void Update(float elapsedTime)
    {
      this.animation.Update(elapsedTime);
      if (!this.rectangle.Intersects(Player.CollisionRectangle))
        return;
      SoundEffects.PickUpItem.Play();
      this.collected = true;
      this.animation.Reset();
    }

    public Collectable DeepCopy()
    {
      return new Collectable(new Point(this.rectangle.X, this.rectangle.Y), this.type);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Collectable.spritesheet, Camera.RelativeRectangle(this.rectangle), new Rectangle?(this.animation.Frame), Color.White);
    }

    public void DrawInGUI(SpriteBatch spriteBatch, Vector2 screenPosition)
    {
      spriteBatch.Draw(Collectable.spritesheet, screenPosition, new Rectangle?(this.animation.Frame), Color.White);
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
    {
      spriteBatch.Draw(Collectable.spritesheet, Camera.RelativeRectangle(rectangle), new Rectangle?(this.animation.Frame), Color.White);
    }

    public enum ItemType
    {
      doubleJump_powerup,
      wallJump_powerup,
      brassKey,
      goldKey,
      bronzeKey,
      silverKey,
      heart,
      total,
    }
  }
}
