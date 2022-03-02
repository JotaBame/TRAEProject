using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Items.Accesories.EvilEye
{
    public class EvilEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Evil Eye");
            Tooltip.SetDefault("Unleashes curses to the wielder and nearby enemies when damaged\nProjectiles deal 15% less damage and curse all enemies");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 35000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NazarDebuffs>().Nazar += 1; 
            player.GetModPlayer<NazarDebuffs>().NazarMirror = true;
            player.GetModPlayer<Defense>().pocketMirror = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PocketMirror, 1)
                .AddIngredient(ItemID.Nazar, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    public class NazarDebuffs : ModPlayer
    {
        public int Nazar = 0; public bool NazarMirror = false;

        public override void ResetEffects()
        {
            Nazar = 0; NazarMirror = false;
        }
        public override void UpdateDead()
        {
            Nazar = 0; NazarMirror = false;
        }
        public static readonly int[] NazarDebuffList = new int[] { BuffID.ShadowFlame, BuffID.WitheredWeapon, BuffID.WitheredArmor };
        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (Nazar > 0 && damage > 1)
            {
                int DustType = 0;
                int debuffToApply = Main.rand.Next(NazarDebuffList);
                int duration = 60 + damage * 3; // 5 seconds for every 100 damage taken
                int enemyDuration = 60 + damage * 15 * Nazar; // 26 seconds for every 100 damage taken
                if (debuffToApply == BuffID.ShadowFlame)
                {
                    DustType = DustID.Shadowflame;
                    enemyDuration = 60 + damage; // 60 damage + what was taken
                }
                if (debuffToApply == BuffID.WitheredArmor)
                {
                    DustType = 21;
                }
                if (debuffToApply == BuffID.WitheredWeapon)
                {
                    DustType = 179;
                }
                Player.AddBuff(debuffToApply, duration);
                float distance = 300f;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(7.2f, 7.2f);
                    Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Top, DustType, speed * 5, Scale: 1.5f);
                    d.noGravity = true;
                }
                foreach (NPC enemy in Main.npc)
                {
                    Vector2 newMove = enemy.Center - Player.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);// could simplify this using Vector2.Length?
                    if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && distanceTo < distance)
                    {
                        enemy.AddBuff(debuffToApply, enemyDuration);
                    }
                }
            }
        }       
        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (Nazar > 0 && damage > 1)
            {
                int DustType = 0;
                int debuffToApply = Main.rand.Next(NazarDebuffList);
                int duration = 60 + damage * 3; // 5 seconds for every 100 damage taken
                int enemyDuration = 60 + damage * 15 * Nazar; // 26 seconds for every 100 damage taken
                if (debuffToApply == BuffID.ShadowFlame)
                {
                    DustType = DustID.Shadowflame; 
                    enemyDuration = 60 + damage; // 60 damage + what was taken
                }
                if (debuffToApply == BuffID.WitheredArmor)
                {
                    DustType = 21;
                }
                if (debuffToApply == BuffID.WitheredWeapon)
                {
                    DustType = 179;
                }
                Player.AddBuff(debuffToApply, duration); 
                float distance = 300f;
				if (NazarMirror)
				{
					distance = 3000f;
				}
                for (int i = 0; i < 50; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(7.2f, 7.2f);
                    Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Top, DustType, speed * 5, Scale: 1.5f);
                    d.noGravity = true;
                }
                foreach (NPC enemy in Main.npc)
                {
                    Vector2 newMove = enemy.Center - Player.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);// could simplify this using Vector2.Length?
                    if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && distanceTo < distance)
                    {
                        enemy.AddBuff(debuffToApply, enemyDuration);
                    }
                }
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(BuffID.ShadowFlame))
            {
                Player.lifeRegen -= 8; // important to note, this doesn't stop life regen.
            }
        }
    }
    public class NazarBuffs : GlobalBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[BuffID.ShadowFlame] = true;
        }
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            switch (type)
            {
                case BuffID.WitheredWeapon:
                    npc.damage = (int)(npc.defDamage * 0.8f);
                    return;
            }
        }
        public override void Update(int type, Player player, ref int buffIndex)
        {
            switch (type)
            {
                case BuffID.WitheredArmor:
                    player.witheredArmor = false;
                    player.statDefense -= 25;
                    return;
                case BuffID.WitheredWeapon:
                    player.witheredWeapon = false;
                    player.GetDamage<GenericDamageClass>() -= 0.2f;
                    return;
            }
        }
        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            switch (type)
            {
                case BuffID.WitheredArmor:
                    tip = "Defense reduced by 25";
                    return;
                case BuffID.WitheredWeapon:
                    tip = "20% reduced damage";
                    return;
            }
        }
    }
}
