
// Type: GameManager.MonologuesRoomManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameManager
{
  internal class MonologuesRoomManager
  {
    private static Texture2D spritesheet;
    private static Rectangle sourceRectangle_interactSymbol;
    private static bool drawInteractSymbol;
    private static Rectangle interactSymbolRectangle;
    private Monologue[] monologues;
    private int indexPlaying;

    public MonologuesRoomManager(Monologue[] _monologues)
    {
      MonologuesRoomManager.drawInteractSymbol = false;
      this.indexPlaying = -1;
      this.monologues = _monologues;
    }

    public static void Inizialize(Texture2D _spritesheet)
    {
      MonologuesRoomManager.spritesheet = _spritesheet;
      MonologuesRoomManager.sourceRectangle_interactSymbol = new Rectangle(384, 181, 7, 20);
    }

    public int IndexPlaying => this.indexPlaying;

    public void Update(float elapsedTime)
    {
      if (this.indexPlaying == -1)
      {
        for (int index = 0; index < this.monologues.Length; ++index)
        {
          if (this.monologues[index].interactionRectangle.Intersects(Player.CollisionRectangle))
          {
            MonologuesRoomManager.drawInteractSymbol = true;
            MonologuesRoomManager.interactSymbolRectangle 
                            = this.monologues[index].InteractSymbolLocation(
                                MonologuesRoomManager.sourceRectangle_interactSymbol.Width, 
                                MonologuesRoomManager.sourceRectangle_interactSymbol.Height);

            if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter) || Game1.previousKeyboard.IsKeyDown(Keys.Enter))
            {
              GamePadButtons buttons = Game1.currentGamePad.Buttons;
              if (buttons.B != ButtonState.Pressed)
                return;
              buttons = Game1.previousGamePad.Buttons;
              if (buttons.B != ButtonState.Released)
                return;
            }
            this.monologues[index].active = true;
            this.indexPlaying = index;
            Player.haltInput = false;//true;
            return;
          }
        }
        MonologuesRoomManager.drawInteractSymbol = false;
      }
      else if (!this.monologues[this.indexPlaying].active)
      {
        this.indexPlaying = -1;
        Player.haltInput = false;
      }
      else
        this.monologues[this.indexPlaying].Update(elapsedTime);
    }

    public void PlayMonologue(int index)
    {
      if (this.indexPlaying != -1 || index < 0 || index >= this.monologues.Length)
        return;
      this.monologues[index].active = true;
      this.indexPlaying = index;
      Player.haltInput = false;//true;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.indexPlaying != -1)
      {
        this.monologues[this.indexPlaying].Draw(spriteBatch);
      }
      else
      {
        if (!MonologuesRoomManager.drawInteractSymbol)
          return;
        spriteBatch.Draw(MonologuesRoomManager.spritesheet, Camera.RelativeRectangle(MonologuesRoomManager.interactSymbolRectangle), new Rectangle?(MonologuesRoomManager.sourceRectangle_interactSymbol), Color.White);
      }
    }
  }
}
