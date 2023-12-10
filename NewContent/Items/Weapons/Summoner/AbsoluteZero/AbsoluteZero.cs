using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Summoner.Whip;
using TRAEProject.Changes.Weapon;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.Changes.Weapon.Summon.Minions;

namespace TRAEProject.NewContent.Items.Weapons.Summoner.AbsoluteZero
{
    public class AbsoluteZero : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Absolute Zero");
            // Tooltip.SetDefault("Your summons will focus struck enemies\n12 summon tag damage\n25% summon tag critical strike chance");
             CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.useStyle = 1; 
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.width = 54;
            Item.height = 52;
            Item.shoot = ProjectileType<AbsoluteZeroP>();
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.damage = 180;
            Item.useTime = Item.useAnimation = 35;
            Item.knockBack = 2f;
            Item.shootSpeed = 7.35f;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
 
    public class AbsoluteZeroP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("AbsoluteZero"); ProjectileID.Sets.IsAWhip[Type] = true;

        }
        public override void WhipDefaults()
        {
           
            originalColor = new Color(29, 41, 81);
            whipRangeMultiplier = 1f;
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
            // DisplayName.SetDefault("AbsoluteZeroTag");
            // Description.SetDefault("You will be frozen when hit by a critical strike. Except not, because you are a player and not an enemy, but whatever.");
            Main.debuff[Type] = true;
  
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<Tag>().Damage += 12;
        }
    }
    public class AbsoluteZeroVisualOnHitEffect : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.HasBuff(BuffType<AbsoluteZeroTag>()) && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
            {
                for (int i = 0; i < 3; i++)
                {
                    int num = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 16, 0f, 0f, 0, default, 1.2f);
                    int num2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Ice, 0f, 0f, 100, default, 1f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].noLight = true;
                }
            }
        }
    }
}
