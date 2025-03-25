
// Type: GameManager.MonologuesManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class MonologuesManager
  {
    public static MonologuesRoomManager[] monologuesRoomManagers;

    public static void Inizialize(Texture2D spritesheet, SpriteFont spriteFont)
    {
      Monologue.Inizialize(spritesheet, spriteFont);
      MonologuesRoomManager.Inizialize(spritesheet);
      MonologuesManager.monologuesRoomManagers = new MonologuesRoomManager[18]
      {
        new MonologuesRoomManager(new Monologue[2]
        {
          new Monologue(new Rectangle(0, 0, 0, 0), new string[2]
          {
            "A forsaken place, one of disgrace and\nmistakes. I came here wielding a sword,\nand left unarmed.",
            "Never again shall I use such weapons, \nbut nevertheless, I have skills I shall \nhave to re-learn to aid me."
          }),
          new Monologue(new Rectangle(0, 0, 0, 0), new string[1]
          {
            "Perhaps...\nA sword would have made things easier..."
          })
        }),
        new MonologuesRoomManager(new Monologue[1]
        {
          new Monologue(new Rectangle(567, 199, 16, 16), new string[2]
          {
            "\"Only after facing your demise, you shall\nsee the light again.\"\nX X X   X",
            "I wonder what this means..."
          })
        }),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[1]
        {
          new Monologue(new Rectangle(193, 923, 87, 55), new string[1]
          {
            "Mmmh... It seems like something would\nfit inside the two holes on this altar..."
          })
        }),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[1]
        {
          new Monologue(new Rectangle(0, 0, 0, 0), new string[1]
          {
            "This place is coming down.\nI'd better hurry before I get stuck\nhere."
          })
        }),
        new MonologuesRoomManager(new Monologue[0]),
        new MonologuesRoomManager(new Monologue[0])
      };
    }

    public static bool MonologuePlaying
    {
      get
      {
        return MonologuesManager.monologuesRoomManagers[(int) RoomsManager.CurrentRoom].IndexPlaying != -1;
      }
    }
  }
}
