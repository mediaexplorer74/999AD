
// Type: GameManager.AnimatedSprite
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class AnimatedSprite
  {
    private static Texture2D spritesheet;
    private static Animation[] animations;
    private Vector2 position;
    private Animation animation;
    private bool removeWhenInactive;
    private bool drawInFront;

    public AnimatedSprite(
      Vector2 _position,
      AnimatedSprite.AnimationType _spriteType,
      bool _removeWhenInactive = true,
      bool _drawInfront = false)
    {
      this.position = _position;
      this.removeWhenInactive = _removeWhenInactive;
      this.animation = AnimatedSprite.animations[(int) _spriteType].DeepCopy();
      this.drawInFront = _drawInfront;
    }

    public static void Inizialize(Texture2D _spritesheet)
    {
      AnimatedSprite.spritesheet = _spritesheet;
      AnimatedSprite.animations = new Animation[8]
      {
        new Animation(new Rectangle[1]
        {
          new Rectangle(336, 68, 16, 16)
        }, 0.0f, false),
        new Animation(new Rectangle(336, 40, 64, 8), 8, 8, 8, 0.1f, true),
        new Animation(new Rectangle(0, 0, 112, 40), 16, 40, 7, 0.1f, false, true),
        new Animation(new Rectangle(112, 0, 112, 40), 16, 40, 7, 0.1f, false, true),
        new Animation(new Rectangle(224, 0, 112, 40), 16, 40, 7, 0.1f, false, true),
        new Animation(new Rectangle(336, 0, 112, 40), 16, 40, 7, 0.1f, false, true),
        new Animation(new Rectangle(0, 40, 24, 24), 24, 24, 1, 0.0f, false),
        new Animation(new Rectangle(0, 75, 24, 24), 24, 24, 1, 0.0f, false)
      };
    }

    private Rectangle DrawRectangle
    {
      get
      {
        return new Rectangle((int) this.position.X, (int) this.position.Y, this.animation.Frame.Width, this.animation.Frame.Height);
      }
    }

    public bool Active => !this.removeWhenInactive || this.animation.Active;

    public bool DrawInFront => this.drawInFront;

    public void Update(float elapsedTime) => this.animation.Update(elapsedTime);

    public static AnimatedSprite.AnimationType GetDoorAnimation(Door.TextureType doorType)
    {
      switch (doorType)
      {
        case Door.TextureType.brassDoor:
          return AnimatedSprite.AnimationType.openBrassDoor;
        case Door.TextureType.goldDoor:
          return AnimatedSprite.AnimationType.openGoldDoor;
        case Door.TextureType.bronzeDoor:
          return AnimatedSprite.AnimationType.openBronzeDoor;
        case Door.TextureType.silverDoor:
          return AnimatedSprite.AnimationType.openSilverDoor;
        default:
          return AnimatedSprite.AnimationType.openBrassDoor;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (!this.DrawRectangle.Intersects(Camera.Rectangle))
        return;
      spriteBatch.Draw(AnimatedSprite.spritesheet, Camera.RelativeRectangle(this.DrawRectangle), new Rectangle?(this.animation.Frame), Color.White);
    }

    public enum AnimationType
    {
      sign_tutorial1,
      invisibleTile,
      openBrassDoor,
      openGoldDoor,
      openBronzeDoor,
      openSilverDoor,
      displayDoubleJumpRelic,
      displayWallJumpRelic,
      total,
    }
  }
}
