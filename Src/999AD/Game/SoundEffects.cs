
// Type: GameManager.SoundEffects
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace GameManager
{
  internal static class SoundEffects
  {
    private static SoundEffect pJumpInstance;
    private static SoundEffect pShootInstance;
    private static SoundEffect pHurtInstance;
    private static SoundEffect pickUpItem;
    private static SoundEffect e1AttackInstance;
    private static SoundEffect e2MeleeInstance;
    private static SoundEffect e2AttackInstance;
    private static SoundEffect enemyHurtInstance;
    private static SoundEffect midMoveInstance;
    private static SoundEffect midAttackInstance;
    private static SoundEffect midHurtInstance;
    private static SoundEffect finAttackInstance;
    private static SoundEffect finHurtInstance;
    private static SoundEffect finAwakenInstance;
    private static SoundEffect finRecoverInstance;
    private static Song finalBossMusic;

    public static void Initialise(
      SoundEffect pJump,
      SoundEffect pShoot,
      SoundEffect pHurt,
      SoundEffect pickUp,
      SoundEffect enemyAttack,
      SoundEffect enemyHurt,
      SoundEffect e2Attack,
      SoundEffect midMove,
      SoundEffect midAttack,
      SoundEffect midHurt,
      SoundEffect finAttack,
      SoundEffect finHurt,
      SoundEffect finAwaken,
      SoundEffect finRecover,
      Song finMusic)
    {
      SoundEffects.pJumpInstance = pJump;
      SoundEffects.pShootInstance = pShoot;
      SoundEffects.pHurtInstance = pHurt;
      SoundEffects.pickUpItem = pickUp;
      SoundEffects.e1AttackInstance = enemyAttack;
      SoundEffects.e2MeleeInstance = enemyAttack;
      SoundEffects.e2AttackInstance = e2Attack;
      SoundEffects.enemyHurtInstance = enemyHurt;
      SoundEffects.midMoveInstance = midMove;
      SoundEffects.midAttackInstance = midAttack;
      SoundEffects.midHurtInstance = midHurt;
      SoundEffects.finAttackInstance = finAttack;
      SoundEffects.finHurtInstance = finHurt;
      SoundEffects.finAwakenInstance = finAwaken;
      SoundEffects.finRecoverInstance = finRecover;
      SoundEffects.finalBossMusic = finMusic;
    }

    public static SoundEffectInstance PlayerJump => SoundEffects.pJumpInstance.CreateInstance();

    public static SoundEffectInstance PlayerAttack => SoundEffects.pShootInstance.CreateInstance();

    public static SoundEffectInstance PlayerHurt => SoundEffects.pHurtInstance.CreateInstance();

    public static SoundEffectInstance PickUpItem => SoundEffects.pickUpItem.CreateInstance();

    public static SoundEffectInstance Enemy1Attack
    {
      get => SoundEffects.e1AttackInstance.CreateInstance();
    }

    public static SoundEffectInstance Enemy2Melee => SoundEffects.e2MeleeInstance.CreateInstance();

    public static SoundEffectInstance Enemy2Attack
    {
      get => SoundEffects.e2AttackInstance.CreateInstance();
    }

    public static SoundEffectInstance EnemyHurt => SoundEffects.enemyHurtInstance.CreateInstance();

    public static SoundEffectInstance MidbossMove => SoundEffects.midMoveInstance.CreateInstance();

    public static SoundEffectInstance MidbossAttack
    {
      get => SoundEffects.midAttackInstance.CreateInstance();
    }

    public static SoundEffectInstance MidbossHurt => SoundEffects.midHurtInstance.CreateInstance();

    public static SoundEffectInstance FinalBossAttack
    {
      get => SoundEffects.finAttackInstance.CreateInstance();
    }

    public static SoundEffectInstance FinalBossHurt
    {
      get => SoundEffects.finHurtInstance.CreateInstance();
    }

    public static SoundEffectInstance FinalBossAwaken
    {
      get => SoundEffects.finAwakenInstance.CreateInstance();
    }

    public static SoundEffectInstance FinalBossRecover
    {
      get => SoundEffects.finRecoverInstance.CreateInstance();
    }

    public static Song FinaBossSoundTrack => SoundEffects.finalBossMusic;
  }
}
