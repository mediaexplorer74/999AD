
// Type: GameManager.MenusManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class MenusManager
  {
    public static Menu[] menus;

    public static void Initialize(Texture2D spritesheet_options, Texture2D[] backgrounds)
    {
      Menu.Initialize(spritesheet_options);
      MenusManager.menus = new Menu[8]
      {
        new Menu(backgrounds[0], new Rectangle[6]
        {
          new Rectangle(0, 0, 160, 24),
          new Rectangle(0, 216, 160, 24),
          new Rectangle(0, 24, 160, 24),
          new Rectangle(0, 240, 160, 24),
          new Rectangle(0, 48, 160, 24),
          new Rectangle(0, 72, 160, 24)
        }, new Game1.GameStates[6]
        {
          Game1.GameStates.intro,
          Game1.GameStates.loadGame,
          Game1.GameStates.controls,
          Game1.GameStates.achievements,
          Game1.GameStates.credits,
          Game1.GameStates.quit
        }, Game1.GameStates.titleScreen, new Rectangle[6]
        {
          new Rectangle(112, 64, 160, 24),
          new Rectangle(112, 88, 160, 24),
          new Rectangle(112, 112, 160, 24),
          new Rectangle(112, 136, 160, 24),
          new Rectangle(112, 160, 160, 24),
          new Rectangle(112, 184, 160, 24)
        }),
        new Menu(backgrounds[1], new Rectangle[1]
        {
          new Rectangle(20, 192, 48, 24)
        }, new Game1.GameStates[1], Game1.GameStates.titleScreen, new Rectangle[1]
        {
          new Rectangle(24, 184, 48, 24)
        }),
        new Menu(backgrounds[7], new Rectangle[1]
        {
          new Rectangle(20, 192, 48, 24)
        }, new Game1.GameStates[1], Game1.GameStates.titleScreen, new Rectangle[1]
        {
          new Rectangle(24, 184, 48, 24)
        }),
        new Menu(backgrounds[2], new Rectangle[1]
        {
          new Rectangle(20, 192, 48, 24)
        }, new Game1.GameStates[1], Game1.GameStates.titleScreen, new Rectangle[1]
        {
          new Rectangle(24, 184, 48, 24)
        }),
        new Menu(backgrounds[3], new Rectangle[2]
        {
          new Rectangle(0, 96, 160, 24),
          new Rectangle(0, 120, 160, 24)
        }, new Game1.GameStates[2]
        {
          Game1.GameStates.playing,
          Game1.GameStates.confirmQuit
        }, Game1.GameStates.playing, new Rectangle[2]
        {
          new Rectangle(112, 88, 160, 24),
          new Rectangle(112, 124, 160, 24)
        }),
        new Menu(backgrounds[4], new Rectangle[2]
        {
          new Rectangle(44, 168, 72, 24),
          new Rectangle(44, 144, 72, 24)
        }, new Game1.GameStates[2]
        {
          Game1.GameStates.pause,
          Game1.GameStates.titleScreen
        }, Game1.GameStates.pause, new Rectangle[2]
        {
          new Rectangle(156, 124, 72, 24),
          new Rectangle(156, 148, 72, 24)
        }),
        new Menu(backgrounds[5], new Rectangle[1]
        {
          new Rectangle(0, 96, 160, 24)
        }, new Game1.GameStates[1]
        {
          Game1.GameStates.playing
        }, Game1.GameStates.playing, new Rectangle[1]
        {
          new Rectangle(112, 124, 160, 24)
        }),
        new Menu(backgrounds[6], new Rectangle[1]
        {
          new Rectangle(0, 96, 160, 24)
        }, new Game1.GameStates[1]
        {
          Game1.GameStates.playing
        }, Game1.GameStates.playing, new Rectangle[1]
        {
          new Rectangle(112, 124, 160, 24)
        })
      };
    }

    public enum MenuType
    {
      titleScreen,
      controls,
      achievements,
      credits,
      pause,
      confirmQuit,
      doubleJump,
      wallJump,
      total,
    }
  }
}
