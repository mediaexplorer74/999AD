
// Type: GameManager.Cutscene
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameManager
{
  internal class Cutscene
  {
    private static SpriteFont spriteFont;
    private Animation animation;
    private Texture2D animationSpritesheet;
    private Vector2 animationPosition;
    private string[] sentences;
    private Vector2 stringPosition;
    private static readonly float timeBetweenChars = 0.02f;
    private float elapsedTimeBetweenChars;
    private string stringDisplayed;
    private int currentSentence;
    private int currentLength;
    private bool endOfSentence;
    public bool active;

    public Cutscene(
      Texture2D _animationSpritesheet,
      Animation _animation,
      Vector2 _animationPosition,
      Vector2 _stringPosition,
      string[] _sentences)
    {
      this.animationSpritesheet = _animationSpritesheet;
      this.animation = _animation;
      this.animationPosition = _animationPosition;
      this.stringPosition = _stringPosition;
      this.sentences = _sentences;
      this.elapsedTimeBetweenChars = 0.0f;
      this.stringDisplayed = "";
      this.currentSentence = 0;
      this.currentLength = 0;
      this.endOfSentence = false;
      this.active = true;
    }

    public static void Initialize(SpriteFont _spriteFont) => Cutscene.spriteFont = _spriteFont;

    public void Update(float elapsedTime)
    {
      if (!this.active)
        return;
      this.animation.Update(elapsedTime);
      if (this.currentSentence == this.sentences.Length)
      {
        this.active = false;
        this.currentSentence = 0;
      }
      else if (this.endOfSentence)
      {
        /*if (!Game1.currentKeyboard.IsKeyDown(Keys.Enter) || Game1.previousKeyboard.IsKeyDown(Keys.Enter))
        {
          GamePadButtons buttons = Game1.currentGamePad.Buttons;
          if (buttons.A != ButtonState.Pressed)
            return;
          buttons = Game1.previousGamePad.Buttons;
          if (buttons.A != ButtonState.Released)
            return;
        }
        this.endOfSentence = false;*/
        ++this.currentSentence;
        this.currentLength = 0;
     
      }
      else
      {
        this.elapsedTimeBetweenChars += elapsedTime;
        if ((double) this.elapsedTimeBetweenChars < (double) Cutscene.timeBetweenChars)
          return;
        ++this.currentLength;

        string sentence = "";

        try
        { 
            sentence = this.sentences[this.currentSentence].Substring(0, this.currentLength);
        }
        catch { }

        this.stringDisplayed = sentence;
        
        this.elapsedTimeBetweenChars = 0.0f;
        if (this.currentLength != this.sentences[this.currentSentence].Length)
          return;
        this.endOfSentence = true;
      }
    }

    public void ChangeSentence(int index, string newMessage) => this.sentences[index] = newMessage;

    public int GetNUmOfSentences() => this.sentences.Length;

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this.animationSpritesheet, 
          this.animationPosition, new Rectangle?(this.animation.Frame), Color.White);
      spriteBatch.DrawString(Cutscene.spriteFont, this.stringDisplayed, this.stringPosition, Color.White);
    }
  }
}
