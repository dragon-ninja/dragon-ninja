using CodeStage.AntiCheat.ObscuredTypes;

public struct EquipmentData
{
	public ObscuredInt objectIndex;

	public EquipmentType type;

	public EquipmentRank rank;

	public EquipmentGrade grade;

	public ObscuredInt level;

	public ObscuredInt imageIndex;

	public ObscuredInt power;

	public ObscuredFloat critical;

	public ObscuredInt hp;

	public ObscuredInt shield;

	public ObscuredFloat speed;

	public ObscuredInt addPower;

	public ObscuredFloat addCritical;

	public ObscuredInt addHp;

	public ObscuredInt addShield;

	public ObscuredFloat addSpeed;

	public EquipmentData(EquipmentType t, EquipmentRank r, EquipmentGrade g, int index, int l, int p, float c, int h, int sh, float sp, int addp, float addc, int addh, int addsh, float addsp)
	{
		this = default(EquipmentData);
		objectIndex = DataManager.getEquipmentIndex();
		type = t;
		rank = r;
		grade = g;
		imageIndex = index;
		level = l;
		power = p;
		critical = c;
		hp = h;
		shield = sh;
		speed = sp;
		addPower = addp;
		addCritical = addc;
		addHp = addh;
		addShield = addsh;
		addSpeed = addsp;
	}
}
