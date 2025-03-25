
// Type: GameManager.CutscenesManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class CutscenesManager
  {
    public static Cutscene[] cutscenes;

    public static void Initialize(
      Texture2D enemySpritesheet,
      Texture2D playerSpritesheet,
      SpriteFont spriteFont)
    {
      Cutscene.Initialize(spriteFont);
      CutscenesManager.cutscenes = new Cutscene[1]//new Cutscene[2]
      {
        /*new Cutscene(enemySpritesheet, new Animation(new Rectangle(0, 0, 420, 48), 42, 48, 10, 0.2f, true), new Vector2(171f, 10f), new Vector2(10f, 84f), new string[5]
        {
          "\n\nNOTE: THIS GAME AUTOSAVES AT EVERY SCREEN TRANSITION.\n\n\n                Press ENTER to start.",
          "Life starts, ends, and is accompanied by a great\nmany... mysteries. What concerns you right now,\nWandering Knight, is the end of said life,\n\n                   ALL life.",
          "The year has turned to a most concerning of numbers,\n999A.D., his most holy Pope Sylvester II - a man\nof both great knowledge and great faith - fears that\nsuch an auspicious date brings with it a threat to\nall we hold dear.",
          "Missives, like flocks of paper birds have been\nsent forth detailing rituals of evil at locations\nleft to fester in rot and injustice, rituals set to\nbring foul creations never seen before to our\ndoorstep.",
          "You, Wandering Knight, know of one such that may\nwell have slipped the net, and seek to rectify past\nwrongs before it is too late."
        }),*/
        new Cutscene(playerSpritesheet, new Animation(new Rectangle(0, 24, 160, 24), 16, 24, 10, 0.06f, true), new Vector2(184f, 10f), new Vector2(10f, 84f), new string[3]
        {
          "The cacophony of masonry and age exalts your exit\nfrom the now-demolished church...A duty completed,\nsunlight washes over your exhausted self.",
          "Wandering Knight, lost to history your actions may\nbecome, perhaps your true penance is the thanks you\nrightfully deserve, but you are no less for it.\nThere were more of these foul beasts, some may yet\nhave survived. Wander ever onwards, towards a\nbrighter tomorrow.",
          ""
        })
      };
    }

    public enum CutsceneType
    {
      intro,
      ending,
      total,
    }
  }
}
