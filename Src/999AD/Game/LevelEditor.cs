
// Type: GameManager.LevelEditor
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;


namespace GameManager
{
  internal class LevelEditor
  {
    private int currentTileType;
    private int hoveredTileType;
    public int currentRoomNumber;
    private int widthTiles;
    private int heightTiles;
    private LevelEditor.MenuState menu;
    private string message = "LEVEL EDITOR MODE\nLeft click on a tile to change it to the selected type.\nRight click on a tile to remove it.\nPress 'M' to access the menu.\n\nEnter to begin.";
    private int userInputInt;
    private string userInputString = "";
    private bool randomMode;
    private bool solidView;
    private bool deadlyView;
    private List<int> randomTiles = new List<int>();
    private string roomInfo = "";
    private string tileTypeInfo = "";
    private string tilePositionInfo = "";
    private SpriteFont arial32;
    private SpriteFont arial14;
    private Texture2D whiteTexture;
    private Random rand = new Random();
    private Point PreviousTileHovered = new Point(-1, -1);

    public LevelEditor(SpriteFont _arial32, SpriteFont _arial16, Texture2D _whiteTexture)
    {
      this.arial32 = _arial32;
      this.arial14 = _arial16;
      this.whiteTexture = _whiteTexture;
      CameraManager.SwitchCamera((RoomsManager.Rooms) this.currentRoomNumber, 0.0f);
    }

        private void saveMaps()
        {
            int currentRoomNumber = this.currentRoomNumber;
            try
            {
                var StringFileName = "mapRoom_" + ((RoomsManager.Rooms)currentRoomNumber).ToString() + ".txt";
                using (var streamWriter = new StreamWriter(
                    new FileStream(StringFileName, FileMode.Create, FileAccess.Write)))
                {
                    for (int index1 = 0; index1 < MapsManager.maps[currentRoomNumber].roomHeightTiles; ++index1)
                    {
                        for (int index2 = 0; index2 < MapsManager.maps[currentRoomNumber].roomWidthTiles; ++index2)
                            streamWriter.Write(((int)MapsManager.maps[currentRoomNumber].array[index1, index2].tileType).ToString() + ",");
                        streamWriter.WriteLine();
                    }
                }
            }
            catch (IOException ex)
            {
                this.message = "Save FAILED!\nAsk Ivan.";
            }
        }

    private Point TileFromPointerLocation(MouseState mouseState)
    {
      int num1 = (mouseState.X / Game1.scale + Camera.Rectangle.X) / Tile.tileSize;
      int num2 = (mouseState.Y / Game1.scale + Camera.Rectangle.Y) / Tile.tileSize;
      int max = MapsManager.maps[this.currentRoomNumber].roomWidthTiles - 1;
      return new Point(MathHelper.Clamp(num1, 0, max), MathHelper.Clamp(num2, 0, MapsManager.maps[this.currentRoomNumber].roomHeightTiles - 1));
    }

    private bool getDirectionalInput()
    {
      if (Game1.currentKeyboard.IsKeyDown(Keys.Up))
      {
        this.userInputString = "U," + this.userInputString[2].ToString();
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.Down))
      {
        this.userInputString = "D," + this.userInputString[2].ToString();
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.Left))
      {
        this.userInputString = this.userInputString[0].ToString() + ",L";
        return true;
      }
      if (!Game1.currentKeyboard.IsKeyDown(Keys.Right))
        return false;
      this.userInputString = this.userInputString[0].ToString() + ",R";
      return true;
    }

    private bool GetIntInput()
    {
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad0) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad0))
      {
        this.userInputInt *= 10;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad1) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad1))
      {
        this.userInputInt = this.userInputInt * 10 + 1;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad2) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad2))
      {
        this.userInputInt = this.userInputInt * 10 + 2;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad3) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad3))
      {
        this.userInputInt = this.userInputInt * 10 + 3;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad4) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad4))
      {
        this.userInputInt = this.userInputInt * 10 + 4;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad5) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad5))
      {
        this.userInputInt = this.userInputInt * 10 + 5;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad6) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad6))
      {
        this.userInputInt = this.userInputInt * 10 + 6;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad7) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad7))
      {
        this.userInputInt = this.userInputInt * 10 + 7;
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad8) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad8))
      {
        this.userInputInt = this.userInputInt * 10 + 8;
        return true;
      }
      if (!Game1.currentKeyboard.IsKeyDown(Keys.NumPad9) || Game1.previousKeyboard.IsKeyDown(Keys.NumPad9))
        return false;
      this.userInputInt = this.userInputInt * 10 + 9;
      return true;
    }

    private bool GetStringInput()
    {
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad0) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad0))
      {
        this.userInputString += "0";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad1) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad1))
      {
        this.userInputString += "1";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad2) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad2))
      {
        this.userInputString += "2";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad3) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad3))
      {
        this.userInputString += "3";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad4) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad4))
      {
        this.userInputString += "4";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad5) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad5))
      {
        this.userInputString += "5";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad6) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad6))
      {
        this.userInputString += "6";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad7) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad7))
      {
        this.userInputString += "7";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad8) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad8))
      {
        this.userInputString += "8";
        return true;
      }
      if (Game1.currentKeyboard.IsKeyDown(Keys.NumPad9) && !Game1.previousKeyboard.IsKeyDown(Keys.NumPad9))
      {
        this.userInputString += "9";
        return true;
      }
      if (!Game1.currentKeyboard.IsKeyDown(Keys.OemComma) || Game1.previousKeyboard.IsKeyDown(Keys.OemComma))
        return false;
      this.userInputString += ",";
      return true;
    }

    private void MenuLoop(
      MouseState mouseState,
      MouseState previousMouseState,
      int tilesPerRow,
      int infoBoxHeighPx)
    {
      switch (this.menu)
      {
        case LevelEditor.MenuState.start:
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter))
            break;
          this.menu = LevelEditor.MenuState.none;
          this.message = "";
          break;
        case LevelEditor.MenuState.none:
          if (!Game1.currentKeyboard.IsKeyDown(Keys.M))
            break;
          this.menu = LevelEditor.MenuState.main;
          this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
          break;
        case LevelEditor.MenuState.main:
          if (Game1.currentKeyboard.IsKeyDown(Keys.R))
          {
            this.menu = LevelEditor.MenuState.rooms;
            this.message = "Press:\nA-Change room\nB-Change room size";
            break;
          }
          if (Game1.currentKeyboard.IsKeyDown(Keys.T))
          {
            this.menu = LevelEditor.MenuState.tiles;
            this.message = "Press:\nA-Change tile type\nB-Random tile mode";
            break;
          }
          if (Game1.currentKeyboard.IsKeyDown(Keys.V))
          {
            this.menu = LevelEditor.MenuState.view;
            this.message = "Press:\nA-Highlight solid tiles\nB-Highlight deadly tiles";
            break;
          }
          if (Game1.currentKeyboard.IsKeyDown(Keys.S))
          {
            this.menu = LevelEditor.MenuState.none;
            this.message = "";
            this.saveMaps();
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Back) || Game1.previousKeyboard.IsKeyDown(Keys.Back))
            break;
          this.menu = LevelEditor.MenuState.none;
          this.message = "";
          break;
        case LevelEditor.MenuState.rooms:
          if (Game1.currentKeyboard.IsKeyDown(Keys.A))
          {
            this.menu = LevelEditor.MenuState.pickRoom;
            this.message = "Room index: ";
            this.userInputInt = 0;
            break;
          }
          if (Game1.currentKeyboard.IsKeyDown(Keys.B))
          {
            this.menu = LevelEditor.MenuState.pickRoomSize;
            this.message = "Room size in tiles (format: width,height): ";
            this.userInputString = "";
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Back) || Game1.previousKeyboard.IsKeyDown(Keys.Back))
            break;
          this.menu = LevelEditor.MenuState.main;
          this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
          break;
        case LevelEditor.MenuState.tiles:
          if (Game1.currentKeyboard.IsKeyDown(Keys.A))
          {
            this.menu = LevelEditor.MenuState.pickTileIndex;
            this.message = "Tile index: ";
            this.userInputInt = 0;
            break;
          }
          if (Game1.currentKeyboard.IsKeyDown(Keys.B))
          {
            this.menu = LevelEditor.MenuState.selectRandomTiles;
            this.message = "Enter the indexes of the tiles you want to\nrandomly select from (format: index1,index2...)\nYou can also click the tiles displayed on the right.\nIndexes: ";
            this.userInputString = "";
            this.randomTiles.Clear();
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Back) || Game1.previousKeyboard.IsKeyDown(Keys.Back))
            break;
          this.menu = LevelEditor.MenuState.main;
          this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
          break;
        case LevelEditor.MenuState.view:
          if (Game1.currentKeyboard.IsKeyDown(Keys.A))
          {
            this.menu = LevelEditor.MenuState.none;
            this.message = "";
            this.solidView = !this.solidView;
            break;
          }
          if (Game1.currentKeyboard.IsKeyDown(Keys.B))
          {
            this.menu = LevelEditor.MenuState.none;
            this.message = "";
            this.deadlyView = !this.deadlyView;
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Back) || Game1.previousKeyboard.IsKeyDown(Keys.Back))
            break;
          this.menu = LevelEditor.MenuState.main;
          this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
          break;
        case LevelEditor.MenuState.pickRoom:
          if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
          {
            this.menu = LevelEditor.MenuState.main;
            this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
            break;
          }
          if (this.GetIntInput())
          {
            this.message = "Room index: " + this.userInputInt.ToString();
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter))
            break;
          this.currentRoomNumber = this.userInputInt < 18 ? this.userInputInt : 17;
          CameraManager.SwitchCamera((RoomsManager.Rooms) this.currentRoomNumber, 0.0f);
          CameraManager.pointLocked = new Vector2(0.0f, 0.0f);
          this.userInputInt = 0;
          this.menu = LevelEditor.MenuState.none;
          this.message = "";
          break;
        case LevelEditor.MenuState.pickRoomSize:
          if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
          {
            this.menu = LevelEditor.MenuState.main;
            this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
            break;
          }
          if (this.GetStringInput())
          {
            this.message = "Room size in tiles (format: width,height): " + this.userInputString;
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter))
            break;
          string[] strArray1 = this.userInputString.Split(new char[1]
          {
            ','
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray1.Length != 2)
            break;
          this.widthTiles = int.Parse(strArray1[0]);
          this.widthTiles = MathHelper.Clamp(this.widthTiles, (Game1.gameWidth + Tile.tileSize - 1) / Tile.tileSize, 500);
          this.heightTiles = int.Parse(strArray1[1]);
          this.heightTiles = MathHelper.Clamp(this.heightTiles, (Game1.gameHeight + 2 * CameraManager.maxOffsetY + Tile.tileSize - 1) / Tile.tileSize, 500);
          this.menu = LevelEditor.MenuState.pickResizingDirection;
          this.userInputString = "U,R";
          this.message = "Use the arrows to select\nthe edges (top/bottom and left/right)\nto shift to resize the room.\n" + this.userInputString;
          break;
        case LevelEditor.MenuState.pickResizingDirection:
          if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
          {
            this.menu = LevelEditor.MenuState.main;
            this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
            this.userInputString = "";
            this.heightTiles = 0;
            this.widthTiles = 0;
            break;
          }
          if (this.getDirectionalInput())
          {
            this.message = "Use the arrows to select\nthe edges (top/bottom and left/right)\nto shift to resize the room.\n" + this.userInputString;
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter) || Game1.previousKeyboard.IsKeyDown(Keys.Enter))
            break;
          RoomMap roomMap = new RoomMap(this.heightTiles, this.widthTiles);
          int roomHeightTiles = MapsManager.maps[this.currentRoomNumber].roomHeightTiles;
          int roomWidthTiles = MapsManager.maps[this.currentRoomNumber].roomWidthTiles;
          for (int index1 = 0; index1 < Math.Min(roomHeightTiles, this.heightTiles); ++index1)
          {
            for (int index2 = 0; index2 < Math.Min(roomWidthTiles, this.widthTiles); ++index2)
            {
              if (this.userInputString == "D,R")
                roomMap.array[index1, index2].tileType = MapsManager.maps[this.currentRoomNumber].array[index1, index2].tileType;
              else if (this.userInputString == "U,R")
                roomMap.array[this.heightTiles - 1 - index1, index2].tileType = MapsManager.maps[this.currentRoomNumber].array[roomHeightTiles - 1 - index1, index2].tileType;
              else if (this.userInputString == "D,L")
                roomMap.array[index1, this.widthTiles - 1 - index2].tileType = MapsManager.maps[this.currentRoomNumber].array[index1, roomWidthTiles - 1 - index2].tileType;
              else if (this.userInputString == "U,L")
                roomMap.array[this.heightTiles - 1 - index1, this.widthTiles - 1 - index2].tileType = MapsManager.maps[this.currentRoomNumber].array[roomHeightTiles - 1 - index1, roomWidthTiles - 1 - index2].tileType;
            }
          }
          MapsManager.maps[this.currentRoomNumber] = roomMap;
          CameraManager.SwitchCamera((RoomsManager.Rooms) this.currentRoomNumber, 0.0f);
          this.menu = LevelEditor.MenuState.none;
          this.message = "";
          this.userInputString = "";
          this.heightTiles = 0;
          this.widthTiles = 0;
          break;
        case LevelEditor.MenuState.pickTileIndex:
          if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
          {
            this.menu = LevelEditor.MenuState.main;
            this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
            break;
          }
          if (this.GetIntInput())
          {
            this.message = "Tile index: " + this.userInputInt.ToString();
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter))
            break;
          this.currentTileType = this.userInputInt < 68 ? this.userInputInt : 67;
          this.userInputInt = 0;
          this.menu = LevelEditor.MenuState.none;
          this.message = "";
          this.randomMode = false;
          this.PreviousTileHovered = new Point(-1, -1);
          break;
        case LevelEditor.MenuState.selectRandomTiles:
          if (Game1.currentKeyboard.IsKeyDown(Keys.Back) && !Game1.previousKeyboard.IsKeyDown(Keys.Back))
          {
            this.menu = LevelEditor.MenuState.main;
            this.message = "Main Menu (backspace to exit). Press:\nR-Room Options\nT-Tile options\nV-View options\nS-Save maps";
            break;
          }
          if (this.GetStringInput())
          {
            this.message = "Enter the indexes of the tiles you want to\nrandomly select from (format: index1,index2...)\nYou can also click the tiles displayed on the right.\nIndexes: " + this.userInputString;
            break;
          }
          if (mouseState.X >= Game1.gameWidth * Game1.scale && mouseState.X < Game1.viewportRectangle.Width && mouseState.Y >= 0 && mouseState.Y < Game1.viewportRectangle.Height && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
          {
            this.userInputString = this.userInputString + "," + ((mouseState.X - Game1.gameWidth * Game1.scale) / (Tile.tileSize * Game1.scale) + mouseState.Y / (Tile.tileSize * Game1.scale) * tilesPerRow).ToString() + ",";
            this.message = "Enter the indexes of the tiles you want to\nrandomly select from (format: index1,index2...)\nYou can also click the tiles displayed on the right.\nIndexes: " + this.userInputString;
            break;
          }
          if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter))
            break;
          this.randomTiles.Clear();
          string[] strArray2 = this.userInputString.Split(new char[1]
          {
            ','
          }, StringSplitOptions.RemoveEmptyEntries);
          this.userInputString = "";
          if (strArray2.Length == 0)
            break;
          if (strArray2.Length == 1)
          {
            this.menu = LevelEditor.MenuState.none;
            this.message = "";
            this.randomMode = false;
            this.PreviousTileHovered = new Point(-1, -1);
            int num = int.Parse(strArray2[0]);
            this.currentTileType = num < 68 ? num : 67;
            break;
          }
          foreach (string s in strArray2)
          {
            int num = int.Parse(s);
            this.randomTiles.Add(num < 68 ? num : 67);
          }
          this.randomMode = true;
          this.menu = LevelEditor.MenuState.none;
          this.message = "";
          break;
      }
    }

    public void Update(
      MouseState mouseState,
      MouseState previousMouseState,
      int tilesPerRow,
      int infoBoxHeighPx)
    {
      this.MenuLoop(mouseState, previousMouseState, tilesPerRow, infoBoxHeighPx);
      if (this.menu != LevelEditor.MenuState.none)
        return;
      if (Game1.currentKeyboard.IsKeyDown(Keys.Up))
        CameraManager.pointLocked.Y -= 10f;
      if (Game1.currentKeyboard.IsKeyDown(Keys.Down))
        CameraManager.pointLocked.Y += 10f;
      if (Game1.currentKeyboard.IsKeyDown(Keys.Right))
        CameraManager.pointLocked.X += 10f;
      if (Game1.currentKeyboard.IsKeyDown(Keys.Left))
        CameraManager.pointLocked.X -= 10f;
      CameraManager.pointLocked.X = MathHelper.Clamp(CameraManager.pointLocked.X, (float) Game1.gameWidth / 2f, (float) MapsManager.maps[this.currentRoomNumber].RoomWidthtPx - (float) Game1.gameWidth / 2f);
      CameraManager.pointLocked.Y = MathHelper.Clamp(CameraManager.pointLocked.Y, (float) Game1.gameHeight / 2f, (float) MapsManager.maps[this.currentRoomNumber].RoomHeightPx - (float) Game1.gameHeight / 2f);
      Tile.TileType tileType;
      if (mouseState.X >= 0 && mouseState.X < Game1.gameWidth * Game1.scale && mouseState.Y >= 0 && mouseState.Y < Game1.gameHeight * Game1.scale)
      {
        Point point = this.TileFromPointerLocation(mouseState);
        this.hoveredTileType = (int) MapsManager.maps[this.currentRoomNumber].array[point.Y, point.X].tileType;
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
          if (this.randomMode)
          {
            if (this.PreviousTileHovered != point || previousMouseState.LeftButton != ButtonState.Pressed)
            {
              int index = this.rand.Next(this.randomTiles.Count);
              MapsManager.maps[this.currentRoomNumber].array[point.Y, point.X].tileType = (Tile.TileType) this.randomTiles[index];
            }
            this.PreviousTileHovered = point;
          }
          else
            MapsManager.maps[this.currentRoomNumber].array[point.Y, point.X].tileType = (Tile.TileType) this.currentTileType;
        }
        else if (mouseState.RightButton == ButtonState.Pressed)
          MapsManager.maps[this.currentRoomNumber].array[point.Y, point.X].tileType = Tile.TileType.empty;
        string[] strArray = new string[9]
        {
          "Tile hovered: row ",
          point.Y.ToString(),
          ", col ",
          point.X.ToString(),
          "\n Type: ",
          this.hoveredTileType.ToString(),
          "(",
          null,
          null
        };
        tileType = (Tile.TileType) this.hoveredTileType;
        strArray[7] = tileType.ToString();
        strArray[8] = ")";
        this.tilePositionInfo = string.Concat(strArray);
      }
      else if (mouseState.X >= Game1.gameWidth * Game1.scale && mouseState.X < Game1.viewportRectangle.Width && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
      {
        int num = (mouseState.X - Game1.gameWidth * Game1.scale) / (Tile.tileSize * Game1.scale) + mouseState.Y / (Tile.tileSize * Game1.scale) * tilesPerRow;
        if (num < 68)
        {
          this.randomMode = false;
          this.PreviousTileHovered = new Point(-1, -1);
          this.currentTileType = num;
        }
      }
      this.roomInfo = "Current room: " + this.currentRoomNumber.ToString() + "(" + ((RoomsManager.Rooms) this.currentRoomNumber).ToString() + ")";
      if (this.randomMode)
      {
        this.tileTypeInfo = "Tile selected: random";
      }
      else
      {
        string[] strArray = new string[5]
        {
          "Tile selected: ",
          this.currentTileType.ToString(),
          "(",
          null,
          null
        };
        tileType = (Tile.TileType) this.currentTileType;
        strArray[3] = tileType.ToString();
        strArray[4] = ")";
        this.tileTypeInfo = string.Concat(strArray);
      }
    }

    public void Draw(
      SpriteBatch spriteBatch,
      int tilesPerRow,
      int infoBoxHeighPx,
      int editorWidth,
      int editorHeight)
    {
      if (this.menu == LevelEditor.MenuState.start)
      {
        spriteBatch.Draw(this.whiteTexture, new Rectangle(0, 0, editorWidth, editorHeight), Color.CornflowerBlue);
      }
      else
      {
        Camera.Draw(spriteBatch);
        MapsManager.maps[this.currentRoomNumber].Draw(spriteBatch);
        Rectangle rectangle;
        if (this.solidView)
        {
          int index1 = Camera.Rectangle.Y / Tile.tileSize;
          while (true)
          {
            int num1 = index1;
            rectangle = Camera.Rectangle;
            int num2 = (rectangle.Bottom - 1) / Tile.tileSize;
            if (num1 <= num2)
            {
              int index2 = Camera.Rectangle.X / Tile.tileSize;
              while (true)
              {
                int num3 = index2;
                rectangle = Camera.Rectangle;
                int num4 = (rectangle.Right - 1) / Tile.tileSize;
                if (num3 <= num4)
                {
                  if (MapsManager.maps[this.currentRoomNumber].array[index1, index2].isSolid())
                    spriteBatch.Draw(this.whiteTexture, Camera.RelativeRectangle(new Rectangle(index2 * Tile.tileSize, index1 * Tile.tileSize, Tile.tileSize, Tile.tileSize)), Color.Red * 0.3f);
                  ++index2;
                }
                else
                  break;
              }
              ++index1;
            }
            else
              break;
          }
        }
        if (this.deadlyView)
        {
          int index3 = Camera.Rectangle.Y / Tile.tileSize;
          while (true)
          {
            int num5 = index3;
            rectangle = Camera.Rectangle;
            int num6 = (rectangle.Bottom - 1) / Tile.tileSize;
            if (num5 <= num6)
            {
              int index4 = Camera.Rectangle.X / Tile.tileSize;
              while (true)
              {
                int num7 = index4;
                rectangle = Camera.Rectangle;
                int num8 = (rectangle.Right - 1) / Tile.tileSize;
                if (num7 <= num8)
                {
                  if (MapsManager.maps[this.currentRoomNumber].array[index3, index4].isHarmful())
                    spriteBatch.Draw(this.whiteTexture, Camera.RelativeRectangle(new Rectangle(index4 * Tile.tileSize, index3 * Tile.tileSize, Tile.tileSize, Tile.tileSize)), Color.Purple * 0.3f);
                  ++index4;
                }
                else
                  break;
              }
              ++index3;
            }
            else
              break;
          }
        }
        spriteBatch.Draw(this.whiteTexture, new Rectangle(0, Game1.gameHeight, Game1.gameWidth, infoBoxHeighPx), Color.LightGray);
        spriteBatch.Draw(this.whiteTexture, new Rectangle(Game1.gameWidth, 0, Tile.tileSize * tilesPerRow, Game1.gameHeight + infoBoxHeighPx), Color.LightGray);
        for (int tileType = 0; tileType < 68; ++tileType)
          Tile.DrawAtLocation(spriteBatch, tileType, new Vector2((float) (tileType % tilesPerRow), (float) (tileType / tilesPerRow)) * (float) Tile.tileSize + new Vector2((float) Game1.gameWidth, 0.0f));
      }
    }

    public void DrawText(SpriteBatch spriteBatch, int infoBoxHeightPx)
    {
      if (this.menu == LevelEditor.MenuState.start)
      {
        spriteBatch.DrawString(this.arial32, this.message, new Vector2((float) (Game1.viewportRectangle.Width / 2), (float) (Game1.viewportRectangle.Height / 2)) - this.arial32.MeasureString(this.message) / 2f, Color.Black);
      }
      else
      {
        spriteBatch.DrawString(this.arial14, this.tileTypeInfo, new Vector2((float) (Game1.gameWidth * Game1.scale / 2), (float) (Game1.gameHeight * Game1.scale + infoBoxHeightPx * Game1.scale / 2)) - this.arial14.MeasureString(this.tileTypeInfo) / 2f, Color.Black);
        spriteBatch.DrawString(this.arial14, this.tilePositionInfo, new Vector2((float) (Game1.gameWidth * Game1.scale - 10) - this.arial14.MeasureString(this.tilePositionInfo).X, (float) (Game1.gameHeight * Game1.scale + infoBoxHeightPx * Game1.scale / 2) - this.arial14.MeasureString(this.tilePositionInfo).Y / 2f), Color.Black);
        spriteBatch.DrawString(this.arial14, this.roomInfo, new Vector2(10f, (float) (Game1.gameHeight * Game1.scale + infoBoxHeightPx * Game1.scale / 2) - this.arial14.MeasureString(this.roomInfo).Y / 2f), Color.Black);
        if (this.menu == LevelEditor.MenuState.start || this.menu == LevelEditor.MenuState.none)
          return;
        spriteBatch.Draw(this.whiteTexture, new Rectangle((int) ((double) (Game1.viewportRectangle.Width / 2) - (double) this.arial32.MeasureString(this.message).X / 2.0 - 10.0), (int) ((double) (Game1.viewportRectangle.Height / 2) - (double) this.arial32.MeasureString(this.message).Y / 2.0 - 10.0), (int) this.arial32.MeasureString(this.message).X + 20, (int) this.arial32.MeasureString(this.message).Y + 20), Color.LightGray);
        spriteBatch.DrawString(this.arial32, this.message, new Vector2((float) (Game1.viewportRectangle.Width / 2), (float) (Game1.viewportRectangle.Height / 2)) - this.arial32.MeasureString(this.message) / 2f, Color.Black);
      }
    }

    private enum MenuState
    {
      start,
      none,
      main,
      rooms,
      tiles,
      view,
      pickRoom,
      pickRoomSize,
      pickResizingDirection,
      pickTileIndex,
      selectRandomTiles,
    }
  }
}
