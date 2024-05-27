using CodeStage.AntiCheat.ObscuredTypes;

public struct PetData
{
	public ObscuredInt objectIndex;

	public EquipmentRank rank;

	public ObscuredInt level;

	public ObscuredInt imageIndex;

	public PetPositionType type;

	public ObscuredFloat powerPer;

	public ObscuredFloat criticalPer;

	public ObscuredFloat shieldPer;

	public ObscuredFloat speedPer;

	public ObscuredFloat hpPer;

	public PetData(EquipmentRank r, ObscuredInt l, ObscuredInt image)
	{
		objectIndex = DataManager.getEquipmentIndex();
		rank = r;
		level = l;
		imageIndex = image;
		powerPer = 0f;
		criticalPer = 0f;
		shieldPer = 0f;
		speedPer = 0f;
		hpPer = 0f;
		type = EquipmentObjects.getPetPositionType(imageIndex);
	}
}
