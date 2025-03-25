
// Type: GameManager.MapsManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;


namespace GameManager
{
  internal static class MapsManager
  {
    public static RoomMap[] maps = new RoomMap[18];

    public static void Inizialize(Texture2D _spritesheet)
    {
      Tile.LoadTextures(_spritesheet);
      MapsManager.loadMaps();
    }

    private static void loadMaps()
    {
      for (int index1 = 0; index1 < 18; ++index1)
      {
     
        List<List<int>> intListList = new List<List<int>>();
        try
        {
          var StringFileName = "mapFiles\\mapRoom_" + ((RoomsManager.Rooms)index1).ToString() + ".txt";
            using (StreamReader streamReader = new StreamReader(File.OpenRead(StringFileName)))
            {
                string str1 = streamReader.ReadLine();
                int index2 = 0;
                while (str1 != null)
                {
                    intListList.Add(new List<int>());
                    string str2 = str1;
                    char[] separator = new char[1] { ',' };
                    foreach (string s in str2.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                        intListList[index2].Add(int.Parse(s));
                    str1 = streamReader.ReadLine();
                    ++index2;
                }
                MapsManager.maps[index1] = new RoomMap(intListList.Count, intListList[0].Count);
                for (int index3 = 0; index3 < intListList.Count; ++index3)
                {
                    for (int index4 = 0; index4 < intListList[0].Count; ++index4)
                    {
                        if (intListList[index3][index4] != 0)
                            MapsManager.maps[index1].array[index3, index4].tileType = (Tile.TileType)intListList[index3][index4];
                    }
                }
            }
        }
        catch (IOException ex)
        {
          MapsManager.maps[index1] = new RoomMap((Game1.gameHeight + 2 * CameraManager.maxOffsetY + Tile.tileSize - 1) / Tile.tileSize, (Game1.gameWidth + Tile.tileSize - 1) / Tile.tileSize);
        }
        catch (ArgumentOutOfRangeException ex)
        {
          MapsManager.maps[index1] = new RoomMap((Game1.gameHeight + 2 * CameraManager.maxOffsetY + Tile.tileSize - 1) / Tile.tileSize, (Game1.gameWidth + Tile.tileSize - 1) / Tile.tileSize);
        }
      }
    }

    public static void resetMap(RoomsManager.Rooms roomName)
    {
      List<List<int>> intListList = new List<List<int>>();
      try
      {
        var StringFileName = "mapFiles\\mapRoom_" + roomName.ToString() + ".txt";
                using (var streamReader = new StreamReader(new FileStream(StringFileName, FileMode.Open, FileAccess.ReadWrite)))
                {
                    string str1 = streamReader.ReadLine();
                    int index1 = 0;
                    while (str1 != null)
                    {
                        intListList.Add(new List<int>());
                        string str2 = str1;
                        char[] separator = new char[1] { ',' };
                        foreach (string s in str2.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                            intListList[index1].Add(int.Parse(s));
                        str1 = streamReader.ReadLine();
                        ++index1;
                    }
                    MapsManager.maps[(int)roomName] = new RoomMap(intListList.Count, intListList[0].Count);
                    for (int index2 = 0; index2 < intListList.Count; ++index2)
                    {
                        for (int index3 = 0; index3 < intListList[0].Count; ++index3)
                        {
                            if (intListList[index2][index3] != 0)
                                MapsManager.maps[(int)roomName].array[index2, index3].tileType = (Tile.TileType)intListList[index2][index3];
                        }
                    }
                }
      }
      catch (IOException ex)
      {
      }
      catch (ArgumentOutOfRangeException ex)
      {
      }
    }
  }
}
