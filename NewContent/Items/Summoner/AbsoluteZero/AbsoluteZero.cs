using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Summoner.Whip;
using TRAEProject.Changes.Weapon;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.Changes.Weapon.Summon.Minions;

namespace TRAEProject.NewContent.Items.Summoner.AbsoluteZero
{
    public class AbsoluteZero : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Absolute Zero");
            Tooltip.SetDefault("Your summons will focus struck enemies\n12 summon tag damage\n25% summon tag critical strike chance\nMinion critical hits will very briefly freeze enemies");
             CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.useStyle = 1;
            Item.GetGlobalItem<SpearItems>().canGetMeleeModifiers = true;
            Item.width = 54;
            Item.height = 52;
            Item.shoot = ProjectileType<AbsoluteZeroP>();
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.noUseGraphic = true;
            Item.damage = 150;
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 2f;
            Item.shootSpeed = 5.35f;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }
    }
 
    public class AbsoluteZeroP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AbsoluteZero");
        }
        public override void WhipDefaults()
        {
            originalColor = new Color(29, 41, 81);
            whipRangeMultiplier = 1.4f;
            fallOff = 0.4f;
            tag = BuffType<AbsoluteZeroTag>();
            whipSegments = 40;
            tipScale = 1.25f;
        }
    }
    public class AbsoluteZeroTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AbsoluteZeroTag");
            Description.SetDefault("You will be frozen when hit by a critical hit. Except not, because you are a player and not an enemy, but whatever.");
            Main.debuff[Type] = true;
  
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<Tag>().Damage += 12;
            npc.GetGlobalNPC<Tag>().Crit += 25;
        }
    }
    public class AbsoluteZeroVisualOnHitEffect : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (target.HasBuff(BuffType<AbsoluteZeroTag>()) && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
            {
                for (int i = 0; i < 3; i++)
                {
                    int num = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 16, 0f, 0f, 0, default(Color), 1.2f);
                    int num2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Ice, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].noLight = true;
                }
            }
        }
    }
}
