
// Type: GameManager.Monologue
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameManager
{
  internal class Monologue
  {
    public static Texture2D spritesheet;
    public static Rectangle sourceRectangle_dialogueBox;
    public static Rectangle sourceRectangle_arrow;
    public static SpriteFont spriteFont;
    private static Rectangle boxRectangle;
    private static Vector2 stringPosition;
    private static Rectangle arrowRectangle;
    private static readonly float timeBetweenChars = 0.05f;
    public readonly Rectangle interactionRectangle;
    private float elapsedTimeBetweenChars;
    private string[] sentences;
    private string stringDisplayed;
    private int currentSentence;
    private int currentLength;
    private bool endOfSentence;
    public bool active;

    public Monologue(Rectangle _interactionRectangle, string[] _sentences)
    {
      this.elapsedTimeBetweenChars = 0.0f;
      this.stringDisplayed = "";
      this.currentSentence = 0;
      this.currentLength = 0;
      this.endOfSentence = false;
      this.active = false;
      this.interactionRectangle = _interactionRectangle;
      this.sentences = _sentences;
    }

    public static void Inizialize(Texture2D _spritesheet, SpriteFont _spriteFont)
    {
      Monologue.spritesheet = _spritesheet;
      Monologue.spriteFont = _spriteFont;
      Monologue.sourceRectangle_dialogueBox = new Rectangle(24, 173, 360, 48);
      Monologue.sourceRectangle_arrow = new Rectangle(384, 173, 8, 8);
      Monologue.boxRectangle = new Rectangle((Game1.minViewportWidth - Monologue.sourceRectangle_dialogueBox.Width) / 2, 10, Monologue.sourceRectangle_dialogueBox.Width, Monologue.sourceRectangle_dialogueBox.Height);
      Monologue.stringPosition = new Vector2((float) (Monologue.boxRectangle.X + 50), (float) (Monologue.boxRectangle.Y + 4));
      Monologue.arrowRectangle = new Rectangle(Monologue.boxRectangle.Right - 4 - Monologue.sourceRectangle_arrow.Width, Monologue.boxRectangle.Bottom - 4 - Monologue.sourceRectangle_arrow.Height, Monologue.sourceRectangle_arrow.Width, Monologue.sourceRectangle_arrow.Height);
    }

    public void Update(float elapsedTime)
    {
      if (!this.active)
        return;
      if (this.currentSentence == this.sentences.Length)
      {
        this.active = false;
        this.currentSentence = 0;
      }
      else if (this.endOfSentence)
      {
        if ( (!Game1.currentKeyboardState.IsKeyDown(Keys.Enter) || Game1.previousKeyboardState.IsKeyDown(Keys.Enter))
           && !(Game1.currentTouchState.Count == 2 /*&& Game1.previousTouchState.Count != 3*/)
           )
        {
          GamePadButtons buttons = Game1.currentGamePadState.Buttons;
          if (buttons.A != ButtonState.Pressed)
            return;
          buttons = Game1.previousGamePadState.Buttons;
          if (buttons.A != ButtonState.Released)
            return;
        }
        this.endOfSentence = false;
        ++this.currentSentence;
        this.currentLength = 0;
      }
      else
      {
        if ((!Game1.currentKeyboardState.IsKeyDown(Keys.Enter) || Game1.previousKeyboardState.IsKeyDown(Keys.Enter))
                    && !(Game1.currentTouchState.Count == 2 /*&& Game1.previousTouchState.Count != 3*/))
        {
          GamePadButtons buttons = Game1.currentGamePadState.Buttons;
          if (buttons.A == ButtonState.Pressed)
          {
            buttons = Game1.previousGamePadState.Buttons;
            if (buttons.A == ButtonState.Released)
              goto label_12;
          }
          this.elapsedTimeBetweenChars += elapsedTime;
          if ((double) this.elapsedTimeBetweenChars < (double) Monologue.timeBetweenChars)
            return;
          ++this.currentLength;
          this.stringDisplayed = this.sentences[this.currentSentence].Substring(0, this.currentLength);
          this.elapsedTimeBetweenChars = 0.0f;
          if (this.currentLength != this.sentences[this.currentSentence].Length)
            return;
          this.endOfSentence = true;
          return;
        }
label_12:
        this.currentLength = this.sentences[this.currentSentence].Length;
        this.stringDisplayed = this.sentences[this.currentSentence].Substring(0, this.currentLength);
        this.elapsedTimeBetweenChars = 0.0f;
        this.endOfSentence = true;
      }
    }

    public Rectangle InteractSymbolLocation(int width, int height)
    {
      return new Rectangle(this.interactionRectangle.X + this.interactionRectangle.Width / 2 - width / 2, this.interactionRectangle.Y - height, width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Monologue.spritesheet, Monologue.boxRectangle, new Rectangle?(Monologue.sourceRectangle_dialogueBox), Color.White);
      spriteBatch.DrawString(Monologue.spriteFont, this.stringDisplayed, Monologue.stringPosition, Color.White);
      if (this.currentSentence == this.sentences.Length - 1)
        return;
      spriteBatch.Draw(Monologue.spritesheet, Monologue.arrowRectangle, new Rectangle?(Monologue.sourceRectangle_arrow), Color.White);
    }
  }
}
