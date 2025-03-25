
// Type: GameManager.CollectablesManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GameManager
{
  internal static class CollectablesManager
  {
    public static CollectablesRoomManager[] collectablesRoomManagers;
    private static List<Collectable> itemsBeingUsed;
    private static List<Vector2> targetPoints;
    private static readonly float animationTime = 0.3f;
    private static float elapsedAnimationTime;

    public static List<Collectable> collectedItems { get; private set; }

    public static void Inizialize(Texture2D spritesheet)
    {
      Collectable.IDcounter = 0;
      CollectablesManager.elapsedAnimationTime = 0.0f;
      Collectable.Inizialize(spritesheet);
      CollectablesManager.collectablesRoomManagers = new CollectablesRoomManager[18]
      {
        new CollectablesRoomManager(new Collectable[7]//[2]
        {
          new Collectable(new Point(168, 198), Collectable.ItemType.brassKey),
          new Collectable(new Point(168, 150), Collectable.ItemType.brassKey), // hack
          new Collectable(new Point(188, 150), Collectable.ItemType.bronzeKey), // hack
          new Collectable(new Point(208, 150), Collectable.ItemType.silverKey), // hack
          new Collectable(new Point(148, 150), Collectable.ItemType.goldKey), // hack
          new Collectable(new Point(128, 150), Collectable.ItemType.goldKey),// hack
          new Collectable(new Point(628, 148), Collectable.ItemType.brassKey)
        }),

        //new CollectablesRoomManager(new Collectable[0]),
        new CollectablesRoomManager(new Collectable[3]//[2]
        {
          new Collectable(new Point(179, 206), Collectable.ItemType.brassKey),
          new Collectable(new Point(179, /*198*/169), Collectable.ItemType.brassKey), // hack
          new Collectable(new Point(648, 158), Collectable.ItemType.brassKey)
        }),

        new CollectablesRoomManager(new Collectable[2]//[1]
        {
          new Collectable(new Point(506, 150), Collectable.ItemType.heart),
          new Collectable(new Point(519, 169), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[0]),
        new CollectablesRoomManager(new Collectable[0]),
        new CollectablesRoomManager(new Collectable[1]
        {
          new Collectable(new Point(72, 960), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[1]
        {
          new Collectable(new Point(400, 614), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[3]
        {
          new Collectable(new Point(444, 120), Collectable.ItemType.silverKey),
          new Collectable(new Point(56, 883), Collectable.ItemType.heart),
          new Collectable(new Point(462, 63), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[0]),
        new CollectablesRoomManager(new Collectable[1]
        {
          new Collectable(new Point(1160, 40), Collectable.ItemType.brassKey)
        }),
        new CollectablesRoomManager(new Collectable[2]
        {
          new Collectable(new Point(180, 456), Collectable.ItemType.bronzeKey),
          new Collectable(new Point(40, 456), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[2]
        {
          new Collectable(new Point(40, 178), Collectable.ItemType.doubleJump_powerup),
          new Collectable(new Point(689, 95), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[1]
        {
          new Collectable(new Point(1062, 191), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[1]
        {
          new Collectable(new Point(348, 188), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[4]//[2]
        {
          new Collectable(new Point(20, 416), Collectable.ItemType.goldKey),
          new Collectable(new Point(386, 416), Collectable.ItemType.heart),
          new Collectable(new Point(40, 436), Collectable.ItemType.goldKey),
          new Collectable(new Point(406, 436), Collectable.ItemType.heart)
        }),
        //new CollectablesRoomManager(new Collectable[0]),
        new CollectablesRoomManager(new Collectable[4]//[2]
        {
          new Collectable(new Point(20, 416), Collectable.ItemType.goldKey),
          new Collectable(new Point(386, 416), Collectable.ItemType.heart),
          new Collectable(new Point(40, 436), Collectable.ItemType.goldKey),
          new Collectable(new Point(406, 436), Collectable.ItemType.heart)
        }),
        new CollectablesRoomManager(new Collectable[1]
        {
          new Collectable(new Point(26, 215), Collectable.ItemType.goldKey)
        }),
        new CollectablesRoomManager(new Collectable[0])
      };
      CollectablesManager.collectedItems = new List<Collectable>();
      CollectablesManager.itemsBeingUsed = new List<Collectable>();
      CollectablesManager.targetPoints = new List<Vector2>();
    }

    public static void AddToInventory(Collectable collectable)
    {
      CollectablesManager.collectedItems.Add(collectable);
    }

    public static bool TryRemoveFromInventory(Collectable.ItemType itemType, Vector2 _targetPoint)
    {
      for (int index = CollectablesManager.collectedItems.Count - 1; index >= 0; --index)
      {
        if (CollectablesManager.collectedItems[index].type == itemType)
        {
          CollectablesManager.itemsBeingUsed.Add(CollectablesManager.collectedItems[index]);
          CollectablesManager.targetPoints.Add(_targetPoint);
          CollectablesManager.collectedItems.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public static void ResetHearts()
    {
      foreach (CollectablesRoomManager collectablesRoomManager in CollectablesManager.collectablesRoomManagers)
        collectablesRoomManager.ResetHearts();
    }

    public static void Update(float elapsedTime)
    {
      if (CollectablesManager.itemsBeingUsed.Count <= 0)
        return;
      CollectablesManager.elapsedAnimationTime += elapsedTime;
      if ((double) CollectablesManager.elapsedAnimationTime <= (double) CollectablesManager.animationTime)
        return;
      CollectablesManager.itemsBeingUsed.RemoveAt(0);
      CollectablesManager.targetPoints.RemoveAt(0);
      CollectablesManager.elapsedAnimationTime = 0.0f;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      int y1 = 2;
      int num1 = Game1.minViewportWidth - y1;
      for (int index = CollectablesManager.collectedItems.Count - 1; index >= 0; --index)
      {
        int x = num1 - CollectablesManager.collectedItems[index].rectangle.Width;
        CollectablesManager.collectedItems[index].DrawInGUI(spriteBatch, new Vector2((float) x, (float) y1));
        num1 = x - y1;
      }
      if (CollectablesManager.itemsBeingUsed.Count <= 0)
        return;
      float num2 = MathHelper.Lerp(3f, 0.0f, CollectablesManager.elapsedAnimationTime / CollectablesManager.animationTime);
      
      //RnD
      Rectangle rectangle1 = new Rectangle();
      //ref Rectangle r = ref rectangle1;
      Rectangle rectangle2 = Camera.Rectangle;
      int x1 = (int) ((double) MathHelper.Lerp((float) rectangle2.Center.X, CollectablesManager.targetPoints[0].X, CollectablesManager.elapsedAnimationTime / CollectablesManager.animationTime) - (double) CollectablesManager.itemsBeingUsed[0].rectangle.Width * (double) num2 / 2.0);
      rectangle2 = Camera.Rectangle;
      int y2 = (int) ((double) MathHelper.Lerp((float) rectangle2.Center.Y, CollectablesManager.targetPoints[0].Y, CollectablesManager.elapsedAnimationTime / CollectablesManager.animationTime) - (double) CollectablesManager.itemsBeingUsed[0].rectangle.Height * (double) num2 / 2.0);
      int width = (int) ((double) CollectablesManager.itemsBeingUsed[0].rectangle.Width * (double) num2);
      int height = (int) ((double) CollectablesManager.itemsBeingUsed[0].rectangle.Height * (double) num2);
      Rectangle r = new Rectangle(x1, y2, width, height);
      CollectablesManager.itemsBeingUsed[0].Draw(spriteBatch, /*rectangle1*/r);
    }
  }
}
