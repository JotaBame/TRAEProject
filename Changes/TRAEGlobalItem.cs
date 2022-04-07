
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
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

			if (item.ammo > 0 && item.type != ItemID.SilverCoin && item.type != ItemID.GoldCoin && item.type != ItemID.CopperCoin && item.type != ItemID.PlatinumCoin && item.type != ItemID.Ale && item.type != ItemID.SandBlock)
            {
                item.maxStack = 3000;
            }
            switch (item.type)
            {
                case ItemID.GingerBeard:
                    item.value = Item.sellPrice(gold: 8);
                    return;
                case ItemID.SiltBlock:
                case ItemID.SlushBlock:
                case ItemID.DesertFossil:
                    item.useTime = 3; // down from 10
                    item.useAnimation = 3;
                    return;
                case ItemID.AdamantitePickaxe:
			       item.useTime = 7; // down from 8
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
                    item.value = Item.sellPrice(silver: 75);
                    return;
                case ItemID.Topaz:
                    item.value = Item.sellPrice(silver: 80);
                    return;
                case ItemID.Sapphire:
                    item.value = Item.sellPrice(silver: 85);
                    return;
                case ItemID.Emerald:
                    item.value = Item.sellPrice(silver: 90);
                    return;
                case ItemID.Ruby:
                case ItemID.Amber:
                    item.value = Item.sellPrice(silver: 95);
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
            {
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
            }
            return base.CanUseItem(item, player);
        }
        
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (player.inferno)
            {
                Lighting.AddLight((int)(target.Center.X / 16f), (int)(target.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
                int OnFireID = 24;
                float range = 100f;
                int RingDamage = damage / 10;
                if (RingDamage < 1)
                {
                    RingDamage = 1;
                }
                Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
                Vector2 spinningpoint2 = spinningpoint1;
                float RandomNumberBetweenSixAndTen = Main.rand.Next(3, 5) * 2;
                int Twenty = 20;
                float OneOrMinusOne = Main.rand.Next(2) == 0 ? 1f : -1f; // one in three chance of it being 1
                bool flag = true;
                for (int i = 0; i < Twenty * RandomNumberBetweenSixAndTen; ++i) // makes 120 or 240 dusts total
                {
                    if (i % Twenty == 0)
                    {
                        spinningpoint2 = spinningpoint2.RotatedBy(OneOrMinusOne * (6.28318548202515 / RandomNumberBetweenSixAndTen), default);
                        spinningpoint1 = spinningpoint2;
                        flag = !flag;
                    }
                    else
                    {
                        float num4 = 6.283185f / (Twenty * RandomNumberBetweenSixAndTen);
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
                int NPCLimit = 0;
                for (int k = 0; k < 200; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= range)
                    {
                        ++NPCLimit;
                        if (NPCLimit < 5)
                        {
                            int finalDefense = nPC.defense - player.armorPenetration;
                            nPC.ichor = false;
                            nPC.betsysCurse = false;
                            if (finalDefense < 0)
                            {
                                finalDefense = 0;
                            }
                            if (finalDefense > 100)
                            {
                                finalDefense = 100;
                            }
                            RingDamage += finalDefense / 2;
                            player.ApplyDamageToNPC(nPC, RingDamage, 0f, 0, crit: false);
                            if (nPC.FindBuffIndex(OnFireID) == -1)
                            {
                                nPC.AddBuff(OnFireID, 120);
                            }
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
        public int useCount = 0;
       
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.VineRope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "+6 range";
                        }
                    }
                    break;
                case ItemID.ArcheryPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases arrow damage by 10% and arrow speed by 20%";
                        }
                    }
                    break; 
                case ItemID.TitanPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "50% increased knockback and 10% increased melee weapon size";
                        }
                    }
                    break;
                case ItemID.InfernoPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Attacks create fiery explosions, dealing 10% damage in a small area and igniting foes";
                        }
                    }
                    break;
                case ItemID.AmmoReservationPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Prevents most ammo consumption while active";
                        }
                    }
                    break;
                case ItemID.SwiftnessPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "20% increased movement speed";
                        }
                    }
                    break;
                case ItemID.FlaskofNanites:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Melee attacks confuse enemies and increase health regeneration";
                        }
                    }
                    break;              
            }
        }
    }
}
