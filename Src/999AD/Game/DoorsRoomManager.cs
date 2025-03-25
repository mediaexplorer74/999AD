
// Type: GameManager.DoorsRoomManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GameManager
{
  internal class DoorsRoomManager
  {
    public List<Door> doors;

    public DoorsRoomManager(Door[] _doors)
    {
      this.doors = new List<Door>((IEnumerable<Door>) _doors);
    }

    public void Update()
    {
      for (int index = this.doors.Count - 1; index >= 0; --index)
      {
        if (this.doors[index].Closed)
          this.doors[index].Update();
        else
          this.doors.RemoveAt(index);
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (Door door in this.doors)
        door.Draw(spriteBatch);
    }
  }
}
