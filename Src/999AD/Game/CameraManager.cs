
// Type: GameManager.CameraManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class CameraManager
  {
    private static Texture2D[] backgrounds;
    public static readonly int maxOffsetY = 2;
    private static bool shaking;
    private static int offsetY;
    private static readonly float timeBetweenOffsets = 0.1f;
    private static float offsetElapsedTime;
    private static float shakingTime;
    private static float elapsedShakingTime;
    private static float playerPositionWeight;
    private static float transientPlayerPositionWeight;
    public static Vector2 pointLocked;
    private static float cameraTransitionProgression;
    private static float transitionDuration;

    public static void Inizialize(Texture2D[] _backgrounds)
    {
      CameraManager.backgrounds = _backgrounds;
      CameraManager.Reset();
      CameraManager.SwitchCamera(RoomsManager.Rooms.tutorial0);
    }

    public static void Reset()
    {
      CameraManager.offsetY = -CameraManager.maxOffsetY;
      CameraManager.shaking = false;
      CameraManager.cameraTransitionProgression = 1f;
      CameraManager.offsetElapsedTime = 0.0f;
      CameraManager.transientPlayerPositionWeight = 1f;
      CameraManager.playerPositionWeight = 1f;
      CameraManager.pointLocked = new Vector2(0.0f, 0.0f);
      CameraManager.cameraTransitionProgression = 1f;
    }

    public static void SwitchCamera(RoomsManager.Rooms room, float _playerPositionWeight = 1f)
    {
      CameraManager.playerPositionWeight = _playerPositionWeight;
      CameraManager.transientPlayerPositionWeight = CameraManager.playerPositionWeight;
      CameraManager.cameraTransitionProgression = 1f;
      Camera.Inizialize(CameraManager.backgrounds[(int) room], room);
    }

    public static void shakeForTime(float _shakingTime)
    {
      CameraManager.shakingTime = _shakingTime;
      CameraManager.elapsedShakingTime = 0.0f;
      CameraManager.shaking = true;
    }

    public static void Update(float elapsedTime)
    {
      if ((double) CameraManager.cameraTransitionProgression < 1.0)
      {
        CameraManager.cameraTransitionProgression += elapsedTime / CameraManager.transitionDuration;
        if ((double) CameraManager.cameraTransitionProgression >= 1.0)
          CameraManager.cameraTransitionProgression = 1f;
        CameraManager.transientPlayerPositionWeight += (CameraManager.playerPositionWeight - CameraManager.transientPlayerPositionWeight) * CameraManager.cameraTransitionProgression;
      }
      Camera.Update(CameraManager.shaking, Player.Center * CameraManager.transientPlayerPositionWeight + CameraManager.pointLocked * (1f - CameraManager.transientPlayerPositionWeight));
      if (!CameraManager.shaking)
        return;
      Camera.position.Y += (float) CameraManager.offsetY;
      CameraManager.elapsedShakingTime += elapsedTime;
      if ((double) CameraManager.elapsedShakingTime >= (double) CameraManager.shakingTime)
      {
        CameraManager.shaking = false;
      }
      else
      {
        CameraManager.offsetElapsedTime += elapsedTime;
        if ((double) CameraManager.offsetElapsedTime < (double) CameraManager.timeBetweenOffsets)
          return;
        CameraManager.offsetY *= -1;
        CameraManager.offsetElapsedTime = 0.0f;
      }
    }

    public static void MoveCamera(
      float _playerPositionWeight,
      Vector2 _pointLocked,
      float _transitionDuration)
    {
      CameraManager.playerPositionWeight = _playerPositionWeight;
      CameraManager.pointLocked = _pointLocked;
      CameraManager.cameraTransitionProgression = 0.0f;
      CameraManager.transitionDuration = _transitionDuration;
    }
  }
}
