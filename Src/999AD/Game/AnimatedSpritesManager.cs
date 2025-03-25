
// Type: GameManager.AnimatedSpritesManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class AnimatedSpritesManager
  {
    public static AnimatedSpritesRoomManager[] animatedSpritesRoomManagers;

    public static void Inizialize(Texture2D spritesheet)
    {
      AnimatedSprite.Inizialize(spritesheet);
      AnimatedSpritesManager.animatedSpritesRoomManagers = new AnimatedSpritesRoomManager[18]
      {
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[1]
        {
          new AnimatedSprite(new Vector2(567f, 200f), AnimatedSprite.AnimationType.sign_tutorial1, false)
        }),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0]),
        new AnimatedSpritesRoomManager(new AnimatedSprite[0])
      };
    }
  }
}
