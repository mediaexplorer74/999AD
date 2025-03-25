
// Type: GameManager.LoadSaveManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary1;
using Windows.Storage;


namespace GameManager
{
  internal static class LoadSaveManager
  {
    private static string folderPath;
    private static string filePathGame;
    private static string filePathScores;

    public static void Inizialize()
    {
      StorageFolder AppDataFolder = ApplicationData.Current.LocalFolder;
      LoadSaveManager.folderPath = AppDataFolder.Path + "\\999ad";

      LoadSaveManager.filePathGame = LoadSaveManager.folderPath + "\\game.fun";
      LoadSaveManager.filePathScores = LoadSaveManager.folderPath + "\\scores.fun";
      DirectoryInfo directoryInfo = new DirectoryInfo(LoadSaveManager.folderPath);
      if (directoryInfo.Exists)
        return;
      directoryInfo.Create();
    }

    public static void SaveGameProgress()
    {
      List<int> _itemsOnMap = new List<int>();
      foreach (CollectablesRoomManager collectablesRoomManager in CollectablesManager.collectablesRoomManagers)
      {
        foreach (Collectable collectable in collectablesRoomManager.collectables)
        {
          if (collectable.ID != -1)
            _itemsOnMap.Add(collectable.ID);
        }
      }
      List<int> _itemsTypesInInvntory = new List<int>();
      foreach (Collectable collectedItem in CollectablesManager.collectedItems)
        _itemsTypesInInvntory.Add((int) collectedItem.type);
      List<int> _closedDoors = new List<int>();
      foreach (DoorsRoomManager doorsRoomManager in DoorsManager.doorsRoomManagers)
      {
        foreach (Door door in doorsRoomManager.doors)
          _closedDoors.Add(door.ID);
      }
      GameSaveData gameSaveData = new GameSaveData(GameStats.gameTime, GameStats.deathsCount,
          GameStats.hitsCount, (int) RoomsManager.CurrentRoom, (int) RoomsManager.PreviousRoom, Player.healthPoints, new float[2]
      {
        Player.position.X,
        Player.position.Y
      }, _itemsOnMap, _itemsTypesInInvntory, _closedDoors, (MidBoss.Dead ? 1 : 0) != 0, 
      (FinalBoss.Dead ? 1 : 0) != 0, GameEvents.eventAlreadyHappened);
      BinaryFormatter binaryFormatter = new BinaryFormatter();
           
      FileStream fileStream = new FileStream(LoadSaveManager.filePathGame, FileMode.Create);
      FileStream serializationStream = fileStream;
      GameSaveData graph = gameSaveData;
      //RnD (fix serialization error)
      //binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
      fileStream.Dispose();//Close();
    }

    public static void SaveHighScores(AchievementsSaveData achievements)
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      FileStream fileStream = new FileStream(LoadSaveManager.filePathScores, FileMode.Create);
      FileStream serializationStream = fileStream;
      AchievementsSaveData graph = achievements;
      binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
      fileStream.Dispose();//Close();
      Achievements.UpdateAchievements(achievements.gameCompleted, achievements.noDeath, achievements.noHits, achievements.bestTime);
    }

    public static void DeleteSaveFile()
    {
      if (!File.Exists(LoadSaveManager.filePathGame))
        return;
      File.Delete(LoadSaveManager.filePathGame);
    }

    public static bool LoadGame()
    {
      if (!File.Exists(LoadSaveManager.filePathGame))
        return false;
      FileStream serializationStream = new FileStream(LoadSaveManager.filePathGame, FileMode.Open);
      try
      {
        (new BinaryFormatter().Deserialize((Stream) serializationStream) as GameSaveData).ApplySaveData();
        serializationStream.Dispose();//Close();
        return true;
      }
      catch (Exception ex)
      {
        serializationStream.Dispose();
        return false;
      }
    }

    public static void LoadHighScores()
    {
      if (File.Exists(LoadSaveManager.filePathScores))
      {
        FileStream serializationStream = new FileStream(LoadSaveManager.filePathScores, FileMode.Open);
        try
        {
          AchievementsSaveData achievementsSaveData = new BinaryFormatter().Deserialize((Stream) serializationStream) as AchievementsSaveData;
          Achievements.UpdateAchievements(achievementsSaveData.gameCompleted, achievementsSaveData.noDeath, achievementsSaveData.noHits, achievementsSaveData.bestTime);
                    serializationStream.Dispose();//Close();
        }
        catch (Exception ex)
        {
          Achievements.UpdateAchievements(false, false, false, 0.0f);
        }
      }
      else
        Achievements.UpdateAchievements(false, false, false, 0.0f);
    }
  }
}
