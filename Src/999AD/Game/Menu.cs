
// Type: GameManager.Menu
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;


namespace GameManager
{
  internal class Menu
  {
    private static Texture2D spritesheet_options;
    private Texture2D backgroungImage;
    private Rectangle[] sourceRectangles_options;
    private Rectangle[] positionOnScreen_options;
    private int currentOption;
    private int totalNumberOfOptions;
    private Game1.GameStates[] followingGameState;
    private Game1.GameStates previousGameState;

    public static void Initialize(Texture2D spritesheet) => Menu.spritesheet_options = spritesheet;

    public Menu(
      Texture2D _backgroundImage,
      Rectangle[] _sourceRectangles_options,
      Game1.GameStates[] _followingGameStates,
      Game1.GameStates _previousGameState,
      Rectangle[] _positionOnScreen_options)
    {
      this.backgroungImage = _backgroundImage;
      this.sourceRectangles_options = _sourceRectangles_options;
      this.currentOption = 0;
      this.totalNumberOfOptions = this.sourceRectangles_options.Length;
      this.followingGameState = _followingGameStates;
      this.previousGameState = _previousGameState;
      this.positionOnScreen_options = _positionOnScreen_options;
    }

    public void Update()
    {
      if (!Game1.currentKeyboardState.IsKeyDown(Keys.Down) || Game1.previousKeyboardState.IsKeyDown(Keys.Down))
      {
        GamePadDPad dpad = Game1.currentGamePadState.DPad;

        if (dpad.Down == ButtonState.Pressed)
        {
          dpad = Game1.previousGamePadState.DPad;
          if (dpad.Down == ButtonState.Released)
            goto label_3;
        }

        if (!Game1.currentKeyboardState.IsKeyDown(Keys.Up) || Game1.previousKeyboardState.IsKeyDown(Keys.Up))
        {
            dpad = Game1.currentGamePadState.DPad;
            if (dpad.Up == ButtonState.Pressed)
            {
                dpad = Game1.previousGamePadState.DPad;
                if (dpad.Up == ButtonState.Released)
                    goto label_7;
            }

            if (Game1.currentMouseState.X != Game1.previousMouseState.X
                            || Game1.currentMouseState.Y != Game1.previousMouseState.Y)
            {
                for (int index = 0; index < this.totalNumberOfOptions; ++index)
                {
                    if (
                        this.positionOnScreen_options[index].Contains
                        (
                            new Point(
                                (int)((Game1.currentMouseState.X - Game1.viewportRectangle.X) / Game1.scaleX),
                                (int)((Game1.currentMouseState.Y - Game1.viewportRectangle.Y) / Game1.scaleY)
                               )
                        )
                        )
                        this.currentOption = index;
                }
                //goto label_14;
            }
            else
            {
                //goto label_14;
            }

            float PrevPosX = 0;
            float PrevPosY = 0;
            try
            {
                if (Game1.previousTouchState.Count > 0)
                {
                    PrevPosX = Game1.previousTouchState[0].Position.X;
                    PrevPosY = Game1.previousTouchState[0].Position.Y;
                }
            }
            catch { }

            if (Game1.currentTouchState.Count > 0)
              if ( (Game1.currentTouchState[0].Position.X != PrevPosX
                    || Game1.currentTouchState[0].Position.Y != PrevPosY) )
                {
                    for (int index = 0; index < this.totalNumberOfOptions; ++index)
                    {
                        if
                        (this.positionOnScreen_options[index].Contains
                            (new Point
                            (
                                (int)((Game1.currentTouchState[0].Position.X - Game1.viewportRectangle.X) / Game1.scaleX),
                                (int)((Game1.currentTouchState[0].Position.Y - Game1.viewportRectangle.Y) / Game1.scaleY)
                            )
                            )
                        )
                            this.currentOption = index;
                    }
                    goto label_14;
                }
                else
                    goto label_14;            
            
            goto label_14;
        }
label_7:
        this.currentOption = (this.currentOption - 1 + this.totalNumberOfOptions) % this.totalNumberOfOptions;
        goto label_14;
      }
label_3:
      this.currentOption = (this.currentOption + 1) % this.totalNumberOfOptions;

label_14:
      if ((!Game1.currentKeyboardState.IsKeyDown(Keys.Enter) || Game1.previousKeyboardState.IsKeyDown(Keys.Enter))
       && !(Game1.currentTouchState.Count == 2 /*&& Game1.previousTouchState.Count != 2*/))
      {
        GamePadButtons buttons = Game1.currentGamePadState.Buttons;
        if (buttons.A == ButtonState.Pressed)
        {
          buttons = Game1.previousGamePadState.Buttons;
          if (buttons.A == ButtonState.Released)
            goto label_17;
        }

            
        if (Game1.currentMouseState.LeftButton == ButtonState.Pressed 
                    && Game1.previousMouseState.LeftButton == ButtonState.Released)
        {
            if (!this.positionOnScreen_options[this.currentOption].Contains(
                new Point(
                    (int)((Game1.currentMouseState.X - Game1.viewportRectangle.X) / Game1.scaleX),
                    (int)((Game1.currentMouseState.Y - Game1.viewportRectangle.Y) / Game1.scaleY)))
                )
            return;
            Game1.currentGameState = this.followingGameState[this.currentOption];
            this.currentOption = 0;
            return;
        }
            

        // touchpanel "click" (tap) handling
        if (Game1.currentTouchState.Count == 1 && Game1.previousTouchState.Count == 0)
        {
            if (!this.positionOnScreen_options[this.currentOption].Contains(
                new Point(
                    (int)((Game1.currentTouchState[0].Position.X - Game1.viewportRectangle.X) / Game1.scaleX),
                    (int)((Game1.currentTouchState[0].Position.Y - Game1.viewportRectangle.Y) / Game1.scaleY)))
                    )
                return;
            Game1.currentGameState = this.followingGameState[this.currentOption];
            this.currentOption = 0;
            return;
        }

        if (!Game1.currentKeyboardState.IsKeyDown(Keys.Back) || Game1.previousKeyboardState.IsKeyDown(Keys.Back))
        {
          buttons = Game1.currentGamePadState.Buttons;
          if (buttons.B != ButtonState.Pressed)
            return;
          buttons = Game1.previousGamePadState.Buttons;
          if (buttons.B != ButtonState.Released)
            return;
        }
        if (Game1.currentGameState == this.previousGameState)
          return;
        Game1.currentGameState = this.previousGameState;
        this.currentOption = 0;
        return;
      }
label_17:
      Game1.currentGameState = this.followingGameState[this.currentOption];
      this.currentOption = 0;
    }

    public void Reset() => this.currentOption = 0;

    public void Draw(SpriteBatch spriteBatch, float alphaValue = 1f)
    {
      spriteBatch.Draw(this.backgroungImage, new Vector2(0.0f, 0.0f), Color.White * alphaValue);
      for (int index = 0; index < this.totalNumberOfOptions; ++index)
        spriteBatch.Draw(Menu.spritesheet_options, 
         this.positionOnScreen_options[index], new Rectangle?(this.sourceRectangles_options[index]), 
         this.currentOption != index ? Color.White : Color.Red);
    }
  }
}
