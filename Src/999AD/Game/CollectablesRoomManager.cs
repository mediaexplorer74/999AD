
// Type: GameManager.CollectablesRoomManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GameManager
{
  internal class CollectablesRoomManager
  {
    private List<Collectable> heartsBackUp;

    public List<Collectable> collectables { get; private set; }

    public CollectablesRoomManager(Collectable[] _collectables)
    {
      this.collectables = new List<Collectable>((IEnumerable<Collectable>) _collectables);
      this.heartsBackUp = new List<Collectable>();
      foreach (Collectable collectable in this.collectables)
      {
        if (collectable.type == Collectable.ItemType.heart)
          this.heartsBackUp.Add(collectable.DeepCopy());
      }
    }

    public void Update(float elapsedTime)
    {
      for (int index = this.collectables.Count - 1; index >= 0; --index)
      {
        if (this.collectables[index].Collected)
        {
            // if heart found then increase health
            if (this.collectables[index].type == Collectable.ItemType.heart)
            {
                Player.IncreaseHealth();
            }
            else
                CollectablesManager.AddToInventory(this.collectables[index]);

          this.collectables.RemoveAt(index);
        }
        else
          this.collectables[index].Update(elapsedTime);
      }
    }

    public void AddCollectableToMap(Collectable collectable) => this.collectables.Add(collectable);

    public void RemoveCollectablesFromMap()
    {
      int index = 0;
      while (index < this.collectables.Count)
      {
        if (this.collectables[index].type == Collectable.ItemType.heart)
          ++index;
        else
          this.collectables.RemoveAt(index);
      }
    }

    public void ResetHearts()
    {
      for (int index = this.collectables.Count - 1; index >= 0; --index)
      {
        if (this.collectables[index].type == Collectable.ItemType.heart)
          this.collectables.RemoveAt(index);
      }
      foreach (Collectable collectable in this.heartsBackUp)
        this.collectables.Add(collectable.DeepCopy());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      for (int index = this.collectables.Count - 1; index >= 0; --index)
      {
        if (Camera.Rectangle.Intersects(this.collectables[index].rectangle))
          this.collectables[index].Draw(spriteBatch);
      }
    }
  }
}
