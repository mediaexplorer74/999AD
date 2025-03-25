
// Type: GameManager.DoorsManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class DoorsManager
  {
    public static DoorsRoomManager[] doorsRoomManagers;

    public static void Inizialize(Texture2D spritesheet)
    {
      Door.IDcounter = 0;
      Door.Inizialize(spritesheet);
      DoorsManager.doorsRoomManagers = new DoorsRoomManager[18]
      {
        new DoorsRoomManager(new Door[1]
        {
          new Door(new Point(912, 168), Door.TextureType.brassDoor, Collectable.ItemType.brassKey)
        }),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[1]
        {
          new Door(new Point(458, 136), Door.TextureType.brassDoor, Collectable.ItemType.brassKey)
        }),
        new DoorsRoomManager(new Door[2]
        {
          new Door(new Point(520, 72), Door.TextureType.brassDoor, Collectable.ItemType.brassKey),
          new Door(new Point(768, 176), Door.TextureType.silverDoor, Collectable.ItemType.silverKey)
        }),
        new DoorsRoomManager(new Door[1]
        {
          new Door(new Point(120, 136), Door.TextureType.goldDoor, Collectable.ItemType.goldKey)
        }),
        new DoorsRoomManager(new Door[1]
        {
          new Door(new Point(472, 200), Door.TextureType.silverDoor, Collectable.ItemType.silverKey)
        }),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[1]
        {
          new Door(new Point(48, 176), Door.TextureType.brassDoor, Collectable.ItemType.brassKey)
        }),
        new DoorsRoomManager(new Door[1]
        {
          new Door(new Point(1184, 448), Door.TextureType.brassDoor, Collectable.ItemType.brassKey)
        }),
        new DoorsRoomManager(new Door[2]
        {
          new Door(new Point(96, 448), Door.TextureType.bronzeDoor, Collectable.ItemType.bronzeKey),
          new Door(new Point(312, 448), Door.TextureType.silverDoor, Collectable.ItemType.silverKey)
        }),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[1]
        {
          new Door(new Point(752, 408), Door.TextureType.goldDoor, Collectable.ItemType.goldKey)
        }),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[0]),
        new DoorsRoomManager(new Door[0])
      };

      for (int room = 0; room < 18; ++room)
      {
        foreach (Door door in DoorsManager.doorsRoomManagers[room].doors)
        {
            door.LockDoor((RoomsManager.Rooms)room);
        }
      }
    }
  }
}
