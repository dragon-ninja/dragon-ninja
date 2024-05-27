using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
	protected ObscuredInt power = 0;

	protected ObscuredFloat critical = 0f;

	protected ObscuredInt shield = 0;

	protected ObscuredInt hp = 0;

	protected ObscuredFloat speed = 0f;

	protected Player player;

	protected GameScene gameScene;

	protected EnemyManager enemyManager;

	protected PlayerManager playerManager;

	protected DamageManager damageManager;

	protected SoundManager soundManager;

	protected ObscuredInt equipmentCount;

	public virtual void startSkill(Player p, GameScene s, int count)
	{
		player = p;
		gameScene = s;
		equipmentCount = count;
		enemyManager = Singleton<EnemyManager>.Instance;
		playerManager = Singleton<PlayerManager>.Instance;
		damageManager = Singleton<DamageManager>.Instance;
		soundManager = Singleton<SoundManager>.Instance;
	}

	public virtual void callAttack(Enemy e)
	{
	}

	public virtual void callTimer()
	{
	}

	public ObscuredInt getPower()
	{
		return power;
	}

	public ObscuredFloat getCritical()
	{
		return critical;
	}

	public ObscuredInt getShield()
	{
		return shield;
	}

	public ObscuredInt getHp()
	{
		return hp;
	}

	public ObscuredFloat getSpeed()
	{
		return speed;
	}
}
