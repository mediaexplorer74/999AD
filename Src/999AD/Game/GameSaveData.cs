// Type: GameManager.GameSaveData
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
//using System.Runtime.Serialization.Formatters.Binary; //TODO


namespace GameManager
{
  //TODO
  //[Serializable]
  internal class GameSaveData
  {
    public float gameTime { get; private set; }

    public int deathsCount { get; private set; }

    public int hitsCount { get; private set; }

    public int currentRoom { get; private set; }

    public int previousRoom { get; private set; }

    public int health { get; private set; }

    public float[] playerPosition { get; private set; }

    public List<int> itemsOnMap { get; private set; }

    public List<int> itemsInInventory { get; private set; }

    public List<int> closedDoor { get; private set; }

    public bool midBossDead { get; private set; }

    public bool finalBossDead { get; private set; }

    public bool[] eventAlreadyHappened { get; private set; }

    public GameSaveData(
      float _time,
      int _deaths,
      int _hits,
      int _currRoom,
      int _prevRoom,
      int _health,
      float[] _position,
      List<int> _itemsOnMap,
      List<int> _itemsTypesInInvntory,
      List<int> _closedDoors,
      bool _midBossDead,
      bool _finalBossDead,
      bool[] _eventAlreadyHappened)
    {
      this.gameTime = _time;
      this.deathsCount = _deaths;
      this.hitsCount = _hits;
      this.playerPosition = _position;
      this.currentRoom = _currRoom;
      this.previousRoom = _prevRoom;
      this.health = _health;
      this.itemsOnMap = _itemsOnMap;
      this.itemsInInventory = _itemsTypesInInvntory;
      this.closedDoor = _closedDoors;
      this.midBossDead = _midBossDead;
      this.finalBossDead = _finalBossDead;
      this.eventAlreadyHappened = _eventAlreadyHappened;
    }

    public void ApplySaveData()
    {
      GameStats.Inizialize(this.gameTime, this.deathsCount, this.hitsCount);
      FinalBoss.Dead = this.finalBossDead;
      MidBoss.Dead = this.midBossDead;
      RoomsManager.CurrentRoom = (RoomsManager.Rooms) this.currentRoom;
      RoomsManager.PreviousRoom = (RoomsManager.Rooms) this.previousRoom;
      Player.healthPoints = this.health;
      Player.position = new Vector2(this.playerPosition[0], this.playerPosition[1]);
      if (this.eventAlreadyHappened[6])
      {
        Player.doubleJumpUnlocked = true;
        AnimatedSpritesManager.animatedSpritesRoomManagers[5].AddTempAnimatedSprite(
            new AnimatedSprite(new Vector2(211f, 926f), 
            AnimatedSprite.AnimationType.displayDoubleJumpRelic, false));
      }
      if (this.eventAlreadyHappened[8])
      {
        Player.wallJumpUnlocked = true;
        AnimatedSpritesManager.animatedSpritesRoomManagers[5]
                    .AddTempAnimatedSprite(new AnimatedSprite(new Vector2(239f, 926f), 
                    AnimatedSprite.AnimationType.displayWallJumpRelic, false));
      }
      foreach (CollectablesRoomManager collectablesRoomManager in CollectablesManager.collectablesRoomManagers)
      {
        for (int index = 0; index < collectablesRoomManager.collectables.Count; ++index)
        {
          bool flag = true;
          foreach (int itemsOn in this.itemsOnMap)
          {
            if (collectablesRoomManager.collectables[index].ID == itemsOn 
                            || collectablesRoomManager.collectables[index].ID == -1)
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            collectablesRoomManager.collectables.RemoveAt(index);
            --index;
          }
        }
      }
      CollectablesManager.collectedItems.Clear();
      foreach (Collectable.ItemType _collectableType in this.itemsInInventory)
        CollectablesManager.AddToInventory(new Collectable(Point.Zero, _collectableType));
      int roomType = 0;
      foreach (DoorsRoomManager doorsRoomManager in DoorsManager.doorsRoomManagers)
      {
        for (int index = 0; index < doorsRoomManager.doors.Count; ++index)
        {
          bool flag = true;
          foreach (int num in this.closedDoor)
          {
            if (doorsRoomManager.doors[index].ID == num)
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            doorsRoomManager.doors[index].OpenDoor(roomType);
            doorsRoomManager.doors.RemoveAt(index);
            --index;
          }
        }
        ++roomType;
      }
      GameEvents.eventAlreadyHappened = this.eventAlreadyHappened;
      PlayerDeathManager.ResetVariables();
      CameraManager.SwitchCamera(RoomsManager.CurrentRoom);
    }
  }
}
