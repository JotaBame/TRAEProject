using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Weapons.Scorpio
{
    class Scorpio : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scorpio");
            Tooltip.SetDefault("Shoots darts");
        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 34;
            Item.damage = 80;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Dart;
            Item.knockBack = 2f;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.PoisonDart;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5; 
        }
    }
}