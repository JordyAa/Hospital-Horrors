public class Stats
{
    public readonly Vitals vitals = new Vitals();
    public readonly Abilities abilities = new Abilities();
    public readonly ElementalChance elementalChance = new ElementalChance();
    public readonly DropChance dropChance = new DropChance();
    public readonly Powers powers = new Powers();
    public readonly Weapon weapon = new Weapon();
    
    public class Vitals
    {
        public int maxHealth = 100;
        public int currentHealth = 100;

        public int maxMana = 50;
        public float currentMana = 50f;
        public float manaRechargeRate = 5f;

        public int souls = 0;
    }
    
    public class Abilities
    {
        public bool dash = false;
        public bool melee = false;
        public bool shield = false;
    }

    public class ElementalChance
    {
        public float burn = 0.0f;
        public float explosive = 0.0f;
        public float freeze = 0.0f;
    }

    public class DropChance
    {
        public float health = 0.0f;
        public float mana = 0.0f;
    }

    public class Powers
    {
        public int lightning = 0;
        public int fireball = 0;
        public int freeze = 0;
        public int revive = 0;
    }

    public class Weapon
    {
        public int bullets = 1;
        public float bulletSpread = 180f;
    
        public float minDamageModifier = 1f;
        public float maxDamageModifier = 1f;

        public float manaCostModifier = 1f;
        public float cooldownModifier = 1f;

        public float projectileForceModifier = 1f;
    }
}
