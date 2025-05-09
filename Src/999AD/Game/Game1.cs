﻿
// Type: GameManager.Game1
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;


namespace GameManager
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    public static readonly int minViewportWidth = 384;
    public static readonly int minViewportHeight = 216;
    public static int gameWidth;
    public static int gameHeight;
    public static Rectangle viewportRectangle;
    private static RenderTarget2D nativeRenderTarget;
    private static RenderTarget2D renderTarget_zoom1;
    private static RenderTarget2D renderTarger_zoom0dot5;
    public static double scaleX;
    public static double scaleY;
    public static MouseState currentMouseState;
    public static MouseState previousMouseState;
    public static TouchCollection currentTouchState;
    public static TouchCollection previousTouchState;
    public static KeyboardState previousKeyboardState;
    public static KeyboardState currentKeyboardState;
    public static GamePadState previousGamePadState;
    public static GamePadState currentGamePadState;
    public static Game1.GameStates currentGameState;
    private bool gameInitialized;
  
    public static Texture2D white;

    public Game1()
    {      
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.graphics.GraphicsProfile = GraphicsProfile.Reach;
      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      this.graphics.IsFullScreen = true;//true; // set it *true* for W10M 
      this.IsMouseVisible = true;

      Game1.gameWidth = Game1.minViewportWidth;
      Game1.gameHeight = Game1.minViewportHeight;

      Game1.scaleX = (1f * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / Game1.gameWidth);
      Game1.scaleY = (1f * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / Game1.gameHeight);

      double scale = MathHelper.Min((float)Game1.scaleX, (float)Game1.scaleY);

      // RnD: my very fast & stupid "scale problem solve/solving" =)
      // Is your screen in portrait mode?
      if (Math.Abs(Game1.scaleX - Game1.scaleY) > 2)
      {
          // yea, "W10M" screen mode ;)
          Game1.scaleX = scale;
          Game1.scaleY = scale * 1.9f;
      }
      else
      {
          // no, "Desktop-PC/notebook" (Win11) screen mode ;)
          Game1.scaleX = scale;
          Game1.scaleY = scale * 0.9f;
                
          this.graphics.IsFullScreen = false; // set it *false* for "big-screen" devices 
      }

      Game1.renderTarget_zoom1 = new RenderTarget2D(this.GraphicsDevice, Game1.gameWidth, Game1.gameHeight);
      Game1.renderTarger_zoom0dot5 = new RenderTarget2D(this.GraphicsDevice, Game1.gameWidth * 2, Game1.gameHeight * 2);

      Game1.viewportRectangle = new Rectangle(
          (int)(this.GraphicsDevice.DisplayMode.Width - Game1.gameWidth * Game1.scaleX) / 2, 
          (int)(this.GraphicsDevice.DisplayMode.Height - Game1.gameHeight * Game1.scaleY) / 2, 
          (int)(Game1.gameWidth * Game1.scaleX), 
          (int)(Game1.gameHeight * Game1.scaleY)
          );

      this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;      

      this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;     

      //this.graphics.IsFullScreen = true;//true; // set it *true* for W10M 
      this.graphics.ApplyChanges();
      
      Game1.previousKeyboardState = Keyboard.GetState();
      Game1.previousGamePadState = GamePad.GetState(PlayerIndex.One);

      Game1.previousMouseState = Mouse.GetState();
      Game1.previousTouchState = TouchPanel.GetState();
      
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.currentGameState = Game1.GameStates.titleScreen;
      LoadSaveManager.Inizialize();
      GameStats.Inizialize();
      Achievements.Initialize(this.Content.Load<SpriteFont>("fonts\\monologue"),
          this.Content.Load<SpriteFont>("fonts\\LiberationMono12"));
      MapsManager.Inizialize(this.Content.Load<Texture2D>("tiles"));
      CameraManager.Inizialize(new Texture2D[18]
      {
        this.Content.Load<Texture2D>("backgrounds\\tutorial0"),
        this.Content.Load<Texture2D>("backgrounds\\tutorial1"),
        this.Content.Load<Texture2D>("backgrounds\\tutorial2"),
        this.Content.Load<Texture2D>("backgrounds\\tutorial3"),
        this.Content.Load<Texture2D>("backgrounds\\tutorial4"),
        this.Content.Load<Texture2D>("backgrounds\\bellTower0"),
        this.Content.Load<Texture2D>("backgrounds\\bellTower1"),
        this.Content.Load<Texture2D>("backgrounds\\bellTower2"),
        this.Content.Load<Texture2D>("backgrounds\\midBoss"),
        this.Content.Load<Texture2D>("backgrounds\\groundFloor"),
        this.Content.Load<Texture2D>("backgrounds\\altarRoom"),
        this.Content.Load<Texture2D>("backgrounds\\firstFloor"),
        this.Content.Load<Texture2D>("backgrounds\\secondFloor"),
        this.Content.Load<Texture2D>("backgrounds\\descent"),
        this.Content.Load<Texture2D>("backgrounds\\finalBoss"),
        this.Content.Load<Texture2D>("backgrounds\\escape0"),
        this.Content.Load<Texture2D>("backgrounds\\escape1"),
        this.Content.Load<Texture2D>("backgrounds\\escape2")
      });
      PlatformsManager.Inizialize(this.Content.Load<Texture2D>("platforms"));
      ProjectilesManager.Inizialize(this.Content.Load<Texture2D>("animatedSprites"));
      Player.Inizialize(this.Content.Load<Texture2D>("characters\\player"), new Vector2(16f, 185f));
      RoomsManager.Inizialize();
      GameEvents.Inizialize();
      FireBallsManager.Inizialize(this.Content.Load<Texture2D>("animatedSprites"));
      LavaGeyserManager.Inizialize(this.Content.Load<Texture2D>("animatedSprites"));
      EnemyManager.Initialise(this.Content.Load<Texture2D>("characters\\enemy1"), 
          this.Content.Load<Texture2D>("characters\\enemy2"));
      MidBoss.Initialise(this.Content.Load<Texture2D>("characters\\midboss"));
      FinalBoss.Inizialize(this.Content.Load<Texture2D>("characters\\finalBoss"), new Texture2D[4]
      {
        this.Content.Load<Texture2D>("characters\\stoneWing"),
        this.Content.Load<Texture2D>("characters\\healthyWing"),
        this.Content.Load<Texture2D>("characters\\damagedWing"),
        this.Content.Load<Texture2D>("characters\\deadWing")
      });
      CollectablesManager.Inizialize(this.Content.Load<Texture2D>("animatedSprites"));
      MonologuesManager.Inizialize(this.Content.Load<Texture2D>("animatedSprites"),
          this.Content.Load<SpriteFont>("fonts\\monologue"));
      DoorsManager.Inizialize(this.Content.Load<Texture2D>("animatedSprites"));
      AnimatedSpritesManager.Inizialize(this.Content.Load<Texture2D>("animatedSprites"));
      TorchManager.Initialize(this.Content.Load<Texture2D>("firePot"));
      PlayerDeathManager.Initialize(this.Content.Load<Texture2D>("menus\\deathScreen"),
          this.Content.Load<Texture2D>("menus\\menuOptions"));
      MenusManager.Initialize(this.Content.Load<Texture2D>("menus\\menuOptions"), new Texture2D[8]
      {
        this.Content.Load<Texture2D>("menus\\titleScreen"),
        this.Content.Load<Texture2D>("menus\\controls"),
        this.Content.Load<Texture2D>("menus\\credits"),
        this.Content.Load<Texture2D>("menus\\pause"),
        this.Content.Load<Texture2D>("menus\\quit"),
        this.Content.Load<Texture2D>("menus\\doubleJump"),
        this.Content.Load<Texture2D>("menus\\wallJump"),
        this.Content.Load<Texture2D>("menus\\achievements")
      });
      CutscenesManager.Initialize(this.Content.Load<Texture2D>("characters\\enemy1"), 
          this.Content.Load<Texture2D>("characters\\player"), this.Content.Load<SpriteFont>("fonts\\monologue"));
      SoundEffects.Initialise(this.Content.Load<SoundEffect>("sounds\\pJump"),
          this.Content.Load<SoundEffect>("sounds\\pShoot"),
          this.Content.Load<SoundEffect>("sounds\\pHurt"), this.Content.Load<SoundEffect>("sounds\\pickup"),
          this.Content.Load<SoundEffect>("sounds\\enemyAttack"), this.Content.Load<SoundEffect>("sounds\\enemyHurt"), this.Content.Load<SoundEffect>("sounds\\e2Attack"), this.Content.Load<SoundEffect>("sounds\\midMove"), this.Content.Load<SoundEffect>("sounds\\midAttack"), this.Content.Load<SoundEffect>("sounds\\midHurt"), this.Content.Load<SoundEffect>("sounds\\finAttack"), this.Content.Load<SoundEffect>("sounds\\finHurt"), this.Content.Load<SoundEffect>("sounds\\finAwaken"), this.Content.Load<SoundEffect>("sounds\\finRecover"), this.Content.Load<Song>("sounds\\finalBossMusic"));
      this.gameInitialized = true;
      Game1.Zoom1();
    }

    protected override void UnloadContent()
    {
    }

    public static void Zoom0Dot5()
    {
      Game1.gameWidth = Game1.minViewportWidth * 2;
      Game1.gameHeight = Game1.minViewportHeight * 2;
      Game1.nativeRenderTarget = Game1.renderTarger_zoom0dot5;
      CameraManager.SwitchCamera(RoomsManager.CurrentRoom);
    }

    public static void Zoom1()
    {
      Game1.gameWidth = Game1.minViewportWidth;
      Game1.gameHeight = Game1.minViewportHeight;
      Game1.nativeRenderTarget = Game1.renderTarget_zoom1;
      CameraManager.SwitchCamera(RoomsManager.CurrentRoom);
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
        this.Exit();

      Game1.currentKeyboardState = Keyboard.GetState();
      Game1.currentGamePadState = GamePad.GetState(PlayerIndex.One);
      Game1.currentMouseState = Mouse.GetState();
      Game1.currentTouchState = TouchPanel.GetState();

      float totalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
      switch (Game1.currentGameState)
      {
        case Game1.GameStates.titleScreen:
          if (!this.gameInitialized)
            this.LoadContent();
          MenusManager.menus[0].Update();
          break;
        case Game1.GameStates.loadGame:
          Game1.currentGameState = !LoadSaveManager.LoadGame() ? Game1.GameStates.titleScreen : Game1.GameStates.playing;
          break;
        case Game1.GameStates.controls:
          MenusManager.menus[1].Update();
          break;
        case Game1.GameStates.achievements:
          MenusManager.menus[2].Update();
          break;
        case Game1.GameStates.credits:
          MenusManager.menus[3].Update();
          break;
        case Game1.GameStates.intro:

          // RnD: skip cut-scenes ( hack )
          // / *
          if (CutscenesManager.cutscenes[0].active)
          {
            CutscenesManager.cutscenes[0].Update(totalSeconds);
            break;
          }
          // * /

          Game1.currentGameState = Game1.GameStates.playing;
          break;
        case Game1.GameStates.playing:
          GameStats.Update(totalSeconds);
          if ( (Game1.currentKeyboardState.IsKeyDown(Keys.P) && !Game1.previousKeyboardState.IsKeyDown(Keys.P) )
               || (Game1.currentKeyboardState.IsKeyDown(Keys.M) && !Game1.previousKeyboardState.IsKeyDown(Keys.M))
               || (Game1.currentTouchState.Count == 3 /*&& Game1.previousTouchState.Count != 3*/)
               || Game1.currentGamePadState.Buttons.Start == ButtonState.Pressed
               && Game1.previousGamePadState.Buttons.Start == ButtonState.Released)
          {
            Game1.currentGameState = Game1.GameStates.pause;
            break;
          }

          RoomsManager.Update(totalSeconds);
          Player.Update(totalSeconds);
          ProjectilesManager.Update(totalSeconds);
          GameEvents.Update(totalSeconds);
          Collisions.Update(totalSeconds);
          CollectablesManager.Update(totalSeconds);
          break;
        case Game1.GameStates.pause:
          if (
                (Game1.currentKeyboardState.IsKeyDown(Keys.P)  && !Game1.previousKeyboardState.IsKeyDown(Keys.P))
                || 
                (Game1.currentKeyboardState.IsKeyDown(Keys.M) && !Game1.previousKeyboardState.IsKeyDown(Keys.M) )
                ||
                (Game1.currentTouchState.Count == 3 && Game1.previousTouchState.Count != 3)
                ||
                (Game1.currentGamePadState.Buttons.Start == ButtonState.Pressed 
                  && Game1.previousGamePadState.Buttons.Start == ButtonState.Released)
            )
          {
            MenusManager.menus[4].Reset();
            Game1.currentGameState = Game1.GameStates.playing;
            break;
          }
          MenusManager.menus[4].Update();
          break;
        case Game1.GameStates.dead:
          GameStats.Update(totalSeconds);
          PlayerDeathManager.Update(totalSeconds);
          break;
        case Game1.GameStates.ending:
          if (!CutscenesManager.cutscenes[1].active)
          {
            LoadSaveManager.SaveHighScores(
                new AchievementsSaveData(true, GameStats.deathsCount == 0 || Achievements.noDeath, 
                GameStats.hitsCount == 0 || Achievements.noHits,
                (double) GameStats.gameTime < (double) Achievements.bestTime 
                || (double) Achievements.bestTime == 0.0 ? GameStats.gameTime : Achievements.bestTime));

            LoadSaveManager.DeleteSaveFile();

            Game1.currentGameState = Game1.GameStates.achievements;
            this.gameInitialized = false;
          }
          CutscenesManager.cutscenes[1].Update(totalSeconds);
          break;
        case Game1.GameStates.quit:
          this.Exit();
          break;
        case Game1.GameStates.confirmQuit:
          MenusManager.menus[5].Update();
          if (Game1.currentGameState == Game1.GameStates.titleScreen)
          {
            this.gameInitialized = false;
            break;
          }
          break;
        case Game1.GameStates.doubleJump:
          MenusManager.menus[6].Update();
          break;
        case Game1.GameStates.wallJump:
          MenusManager.menus[7].Update();
          break;
      }

      Game1.previousKeyboardState = Game1.currentKeyboardState;
      Game1.previousGamePadState = Game1.currentGamePadState;
      Game1.previousMouseState = Game1.currentMouseState;
      Game1.previousTouchState = Game1.currentTouchState;

       base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      this.spriteBatch.Begin();
      switch (Game1.currentGameState)
      {
        case Game1.GameStates.titleScreen:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          MenusManager.menus[0].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.controls:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          MenusManager.menus[1].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.achievements:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          MenusManager.menus[2].Draw(this.spriteBatch);
          Achievements.Draw(this.spriteBatch);
          break;
        case Game1.GameStates.credits:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          MenusManager.menus[3].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.intro:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          CutscenesManager.cutscenes[0].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.playing:
          this.GraphicsDevice.SetRenderTarget(Game1.nativeRenderTarget);
          this.GraphicsDevice.Clear(Color.Black);
          Camera.Draw(this.spriteBatch);
          RoomsManager.Draw(this.spriteBatch);
          CollectablesManager.Draw(this.spriteBatch);
          Player.DrawGUI(this.spriteBatch);
          break;
        case Game1.GameStates.pause:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          MenusManager.menus[4].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.dead:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          Camera.Draw(this.spriteBatch);
          RoomsManager.Draw(this.spriteBatch);
          PlayerDeathManager.Draw(this.spriteBatch);
          break;
        case Game1.GameStates.ending:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          CutscenesManager.cutscenes[1].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.confirmQuit:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          MenusManager.menus[5].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.doubleJump:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          Camera.Draw(this.spriteBatch);
          RoomsManager.Draw(this.spriteBatch);
          MenusManager.menus[6].Draw(this.spriteBatch);
          break;
        case Game1.GameStates.wallJump:
          this.GraphicsDevice.SetRenderTarget(Game1.renderTarget_zoom1);
          this.GraphicsDevice.Clear(Color.Black);
          Camera.Draw(this.spriteBatch);
          RoomsManager.Draw(this.spriteBatch);
          MenusManager.menus[7].Draw(this.spriteBatch);
          break;
      }
      this.spriteBatch.End();
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      this.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
      if (Game1.currentGameState == Game1.GameStates.playing)
        this.spriteBatch.Draw((Texture2D) Game1.nativeRenderTarget, Game1.viewportRectangle, Color.White);
      else
        this.spriteBatch.Draw((Texture2D) Game1.renderTarget_zoom1, Game1.viewportRectangle, Color.White);
      this.spriteBatch.End();
      base.Draw(gameTime);
    }

    public enum GameStates
    {
      titleScreen,
      loadGame,
      controls,
      achievements,
      credits,
      intro,
      playing,
      pause,
      dead,
      ending,
      quit,
      confirmQuit,
      doubleJump,
      wallJump, // ?
      total,
    }
  }
}
