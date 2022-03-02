
using TRAEProject.Buffs;
using TRAEProject.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace TRAEProject.Changes
{
    public class TRAEGlobalItem : GlobalItem
    {   
	public override bool InstancePerEntity => true;       
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {

			if (item.ammo > 0 && item.type != ItemID.SilverCoin && item.type != ItemID.GoldCoin && item.type != ItemID.CopperCoin && item.type != ItemID.PlatinumCoin && item.type != ItemID.PlatinumCoin && item.type != ItemID.Ale && item.type != ItemID.SandBlock)
            {
                item.maxStack = 3000;
            }
            switch (item.type)
            {
                case ItemID.AdamantitePickaxe:
			       item.useTime = 7; // down from 8
                    item.useAnimation = 7;
				   return;				
                case ItemID.ObsidianHorseshoe:
                    item.SetNameOverride("Heavy Horseshoe");
                    return;
                case ItemID.ObsidianWaterWalkingBoots:
                    item.SetNameOverride("Heavy Rocket Boots");
                    return;
                case ItemID.MoonShell:
                    item.SetNameOverride("Monster Shell");
                    return;
                case ItemID.ManaRegenerationBand:
                    item.SetNameOverride("Band of Dual Regeneration");
                    return;
                case ItemID.MagicDagger: // REVISIT
                    item.useStyle = 0;
                    item.mana = 0;
                    item.damage = 0;
                    item.crit = 0;
                    item.knockBack = 0f;
                    item.useTime = 0;
                    item.useAnimation = 0;
                    item.accessory = true;
                    return;
                case ItemID.VineRope:
                    item.useTime = 5;
                    item.useAnimation = 5;
                    item.tileBoost = 6;
                    return;
                case ItemID.StrangeBrew:
                    item.healMana = 0;
                    return;
                case ItemID.BottledHoney:
                    item.healLife = 70;
                    return;
                case ItemID.FastClock:
                    item.value = 100000;
                    return;
                case ItemID.Amethyst:
                    item.value = Item.sellPrice(silver: 25);
                    return;
                case ItemID.Topaz:
                    item.value = Item.sellPrice(silver: 40);
                    return;
                case ItemID.Sapphire:
                    item.value = Item.sellPrice(silver: 55);
                    return;
                case ItemID.Emerald:
                    item.value = Item.sellPrice(silver: 70);
                    return;
                case ItemID.Ruby:
                case ItemID.Amber:
                    item.value = Item.sellPrice(silver: 85);
                    return;
                case ItemID.Diamond:
                    item.value = Item.sellPrice(gold: 1);
                    return;
            }
            return;
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.type == ItemID.DirtRod)
            {
                return true; // if you can do something with right click
            }
            return false;
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.DirtRod)
                if (player.altFunctionUse == 2)
                {
                    item.tileWand = 2;
                    item.useTurn = true;
                    player.tileSpeed /= 1.5f;
                    item.scale = 1.7f;
                    item.shoot = 0;
                    item.useAnimation = 10;
                    item.useTime = 10;
                    item.tileBoost = 11;
                    item.autoReuse = true;
                    item.createTile = TileID.Dirt;
                }
                else
                {
                    item.tileWand = 0;
                    item.createTile = 0;
                    item.channel = true;
                    item.shoot = 17;
                    item.scale = 1.7f;
                    item.UseSound = SoundID.Item8;
                    item.useAnimation = 10;
                    item.useTime = 10;
                    item.autoReuse = true;
                    item.noMelee = true;
                }
            return base.CanUseItem(item, player);
        }
        
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (player.GetModPlayer<TRAEPlayer>().MagicDagger)
            {
                if (Main.rand.Next(2) == 0)
                {
                    player.GetModPlayer<TRAEPlayer>().MagicDaggerSpawn(player, damage, knockBack);
                }
            }
            if (player.inferno)
            {
                Lighting.AddLight((int)(target.Center.X / 16f), (int)(target.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
                Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
                Vector2 spinningpoint2 = spinningpoint1;
                float RandomEvenNumberBetweenSixAndTen = Main.rand.Next(3, 5) * 2;
                int Twenty = 20;
                float OneOrMinusOne = Main.rand.Next(2) == 0 ? 1f : -1f; // one in three chance of it being 1
                bool flag = true;
                for (int i = 0; i < Twenty * RandomEvenNumberBetweenSixAndTen; ++i) // makes 120 or 240 dusts total
                {
                    if (i % Twenty == 0)
                    {
                        spinningpoint2 = spinningpoint2.RotatedBy(OneOrMinusOne * (6.28318548202515 / RandomEvenNumberBetweenSixAndTen), default);
                        spinningpoint1 = spinningpoint2;
                        flag = !flag;
                    }
                    else
                    {
                        float num4 = 6.283185f / (Twenty * RandomEvenNumberBetweenSixAndTen);
                        spinningpoint1 = spinningpoint1.RotatedBy(num4 * OneOrMinusOne * 3.0, default);
                    }
                    float num5 = MathHelper.Lerp(7.5f, 60f, i % Twenty / Twenty);
                    int index2 = Dust.NewDust(new Vector2(target.Center.X, target.Center.Y), 6, 6, 127, 0.0f, 0.0f, 100, default, 3f);
                    Dust dust1 = Main.dust[index2];
                    dust1.velocity = Vector2.Multiply(dust1.velocity, 0.1f);
                    Dust dust2 = Main.dust[index2];
                    dust2.velocity = Vector2.Add(dust2.velocity, Vector2.Multiply(spinningpoint1, num5));
                    if (flag)
                        Main.dust[index2].scale = 0.9f;
                    Main.dust[index2].noGravity = true;

                }
                float range = 100f;
                int RingDamage = damage / 10;
                for (int k = 0; k < 200; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= range)
                    {
                        int finalDefense = nPC.defense - player.armorPenetration;
                        nPC.ichor = false;
                        nPC.betsysCurse = false;
                        if (finalDefense < 0)
                        {
                            finalDefense = 0;
                        }
                        RingDamage += finalDefense / 2;
                        player.ApplyDamageToNPC(nPC, RingDamage, 0f, 0, crit: false);
                        if (nPC.FindBuffIndex(BuffID.OnFire) == -1)
                        {
                            nPC.AddBuff(BuffID.OnFire, 120);
                        }
                    }
                }
            }       
        }
        /// SHOOT STUFF

        public override void OnConsumeItem(Item item, Player player)
        {
            switch (item.type)
            {
                case ItemID.StrangeBrew:
                    int wrathchance = Main.rand.Next(1, 10);
                    int[] buffs = new int[] { BuffID.Ironskin, BuffID.Regeneration, BuffID.Swiftness };
                    int buff = Main.rand.Next(buffs);
                    if (wrathchance != 1)
                    {
                        player.AddBuff(buff, 7200, false);
                        return;
                    }
                    else
                    {
                        player.AddBuff(BuffID.Wrath, 7200, false);
                        return;
                    }
                case ItemID.BottledHoney:
                    player.AddBuff(BuffID.Honey, 2400, false);
                    return;
            }
            return;
        }
        private static int shootDelay = 1;
        public int useCount = 0;
       
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.VineRope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "+6 range";
                        }
                    }
                    return;
                case ItemID.MagicDagger:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Summons magic daggers when striking an enemy";
                        }
                    }
                    return;              
                case ItemID.ArcheryPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Increases arrow damage by 10% and arrow speed by 20%";
                        }
                    }
                    return; 
                case ItemID.TitanPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "50% increased knockback and 10% increased melee weapon size";
                        }
                    }
                    return;
                case ItemID.InfernoPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Attacks create fiery explosions, dealing 10% damage in a small area and igniting foes";
                        }
                    }
                    return;
                case ItemID.AmmoReservationPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Prevents most ammo consumption while active";
                        }
                    }
                    return;
                case ItemID.SwiftnessPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "15% increased movement speed";
                        }
                    }
                    return;
                case ItemID.FlaskofNanites:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Melee and whip attacks confuse enemies and increase health regeneration";
                        }
                    }
                    return;              
            }
        }
    }
}
