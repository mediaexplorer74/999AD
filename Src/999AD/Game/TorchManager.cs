
// Type: GameManager.TorchManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GameManager
{
  internal static class TorchManager
  {
    public static List<Torch> torches;

    public static void Initialize(Texture2D spritesheet)
    {
      Torch.Initialize(spritesheet, new Animation(new Rectangle(0, 0, 128, 32), 16, 32, 8, 0.1f, true), new Animation(new Rectangle(0, 32, 128, 32), 16, 32, 1, 0.0f, false, true));
      TorchManager.torches = new List<Torch>();
      TorchManager.torches.Add(new Torch(new Vector2(46f, 440f)));
      TorchManager.torches.Add(new Torch(new Vector2(230f, 300f)));
      TorchManager.torches.Add(new Torch(new Vector2(160f, 220f)));
      TorchManager.torches.Add(new Torch(new Vector2(300f, 220f)));
      TorchManager.torches.Add(new Torch(new Vector2(160f, 380f)));
      TorchManager.torches.Add(new Torch(new Vector2(300f, 380f)));
      TorchManager.torches.Add(new Torch(new Vector2(230f, 160f)));
      TorchManager.torches.Add(new Torch(new Vector2(230f, 440f)));
      TorchManager.torches.Add(new Torch(new Vector2(130f, 300f)));
      TorchManager.torches.Add(new Torch(new Vector2(330f, 300f)));
    }

    public static void Update(float elapsedTime)
    {
      if (RoomsManager.CurrentRoom != RoomsManager.Rooms.churchAltarRoom)
        return;
      foreach (Torch torch in TorchManager.torches)
        torch.Update(elapsedTime);
    }

    public static bool AllTorchesUnlit()
    {
      foreach (Torch torch in TorchManager.torches)
      {
        if (torch.isLit)
          return false;
      }
      return true;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      if (RoomsManager.CurrentRoom != RoomsManager.Rooms.churchAltarRoom)
        return;
      foreach (Torch torch in TorchManager.torches)
        torch.Draw(spriteBatch);
    }
  }
}
