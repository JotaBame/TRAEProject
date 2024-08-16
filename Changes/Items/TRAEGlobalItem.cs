
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using TRAEProject.NewContent.NPCs.Underworld.Beholder;
using Terraria.Localization;
using Terraria.Chat;
using Terraria.Audio;
using System;
using Steamworks;
using TRAEProject.Changes.Weapon.Melee;
using TRAEProject.Changes.Items;

namespace TRAEProject.Changes
{
    public class TRAEGlobalItem : GlobalItem
    {   
	public override bool InstancePerEntity => true;       
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ItemID.FastClock] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.TrifoldMap] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.Bezoar] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.MedicatedBandage] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.Vitamins] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.ArmorPolish] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.Blindfold] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.PocketMirror] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.Nazar] = 0;
            ItemID.Sets.ShimmerTransformToItem[ItemID.Megaphone] = 0;
        }
        public override void SetDefaults(Item item)
        {
            if(item.createTile >= 0 && item.damage <=0)
            {
                item.useTime = 7; // down from 15
                //item.useAnimation = 7;
            }
            switch (item.type)
            {
                case ItemID.GravityGlobe:
                    item.value = Item.sellPrice(gold: 4);
                    break;
                case ItemID.GingerBeard:
                    item.value = Item.sellPrice(gold: 8);
                    break;
                case ItemID.SiltBlock:
                case ItemID.SlushBlock:
                case ItemID.DesertFossil:
                    item.useTime = 3; // down from 10
                    item.useAnimation = 3;
                    break;
          
 		
                case ItemID.ObsidianHorseshoe:
                    item.SetNameOverride("Gravity Horseshoe");
                    break;
                case ItemID.ObsidianWaterWalkingBoots:
                    item.SetNameOverride("Heavy Rocket Boots");
                    break;
                case ItemID.WormTooth:
                    item.SetNameOverride("Rotten Tooth");
                    break;
                case ItemID.MoonShell:
                    item.SetNameOverride("Monster Shell");
                    break;
                case ItemID.ManaRegenerationBand:
                    item.SetNameOverride("Band of Dual Regeneration");
                    break;
                case ItemID.VineRope:
                    item.useTime = 5;
                    item.useAnimation = 5;
                    item.tileBoost = 6;
                    break;
                case ItemID.BottledHoney:
                    item.healLife = 70;
                    break;
                case ItemID.FastClock:
                    item.value = 100000;
                    break;
                case ItemID.Amethyst:
                    item.value = Item.sellPrice(silver: 20);
                    break;
                case ItemID.Topaz:
                    item.value = Item.sellPrice(silver: 30);
                    break;
                case ItemID.Sapphire:
                    item.value = Item.sellPrice(silver: 40);
                    break;
                case ItemID.Emerald:
                    item.value = Item.sellPrice(silver: 50);
                    break;
                case ItemID.Ruby:
                    item.value = Item.sellPrice(silver: 60);
                    break;
                case ItemID.Amber:
                    item.value = Item.sellPrice(silver: 70);
                    break;
                case ItemID.Diamond:
                    item.value = Item.sellPrice(gold: 1);
                    break;
                case ItemID.RifleScope:
                    item.rare = ItemRarityID.Pink;

                    item.value = Item.buyPrice(gold: 25);
                     break;
                case ItemID.WitchBroom:
                    item.rare = ItemRarityID.Yellow;
                    break;
                //case ItemID.Furnace:
                //    item.value = Item.buyPrice(silver: 10);
                //    break;
                //case ItemID.Bomb:
                //    item.value = Item.buyPrice(silver: 10);
                //    break;
                //case ItemID.Dynamite:
                //    item.value = Item.buyPrice(silver: 65);
                    //break;
 
                //case ItemID.TinkerersWorkshop:
                //    item.value = Item.buyPrice(gold: 30);
                //    break;
                //case ItemID.SharpeningStation:
                //case ItemID.BewitchingTable:
                //case ItemID.CrystalBall:
                //    item.value = Item.buyPrice(gold: 50);
                //    break;
                //case ItemID.TeleportationPylonDesert:
                //case ItemID.TeleportationPylonHallow:
                //case ItemID.TeleportationPylonJungle:
                //case ItemID.TeleportationPylonMushroom:
                //case ItemID.TeleportationPylonOcean:
                //case ItemID.TeleportationPylonPurity:
                //case ItemID.TeleportationPylonSnow:
                //case ItemID.TeleportationPylonUnderground:
                //case ItemID.TeleportationPylonVictory:
                //    item.value = Item.buyPrice(gold: 20);
                //    break;
                //case ItemID.ImbuingStation:
                //    item.value = Item.buyPrice(gold: 50);
                //    break;
                //case ItemID.Bell:
                //case ItemID.Harp:
                //case ItemID.SpellTome:
                //    item.value = Item.buyPrice(gold: 70);
                //    break;
                case ItemID.PirateHat:
                case ItemID.PirateShirt:
                case ItemID.PiratePants:
                    item.value = Item.buyPrice(gold: 50);
                    break;
            }
        }
        int timer = 0;
        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            if (item.type == ItemID.Uzi)
            {
                reforgePrice = Item.buyPrice(gold: 15);
            }
            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }
        public override bool OnPickup(Item item, Player player)
        {
            timer = 0;
            return true;
        }
        public override void UseAnimation(Item item, Player player)
        {
            if (!item.useTurn && item.CountsAsClass(DamageClass.Melee) && player.itemAnimation == player.itemAnimationMax && item.shoot == 0 || item.type == ItemID.BeamSword)
            {
                Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                if (player.position.X - mousePosition.X > 0)
                    player.direction = -1;
                else
                    player.direction = 1;
            }
        }
        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (item.type == ItemID.GuideVoodooDoll && NPC.downedPlantBoss)
            {
                Player player = Main.player[Player.FindClosest(item.Center, item.width, item.height)];
                player.killGuide = true;
                int num117 = Dust.NewDust(new Vector2(item.position.X, item.position.Y + 2f), item.width, item.height, DustID.PurpleTorch, item.velocity.X * 0.2f, item.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num117].noGravity = true;
                Main.dust[num117].velocity.X *= 1f;
                Main.dust[num117].velocity.Y *= 1f;
                if (timer < 80)
                    timer++;
                if (timer >= 80 && player.ZoneUnderworldHeight)
                {
      
                    maxFallSpeed = 0;
                    if (!NPC.AnyNPCs(NPCType<BeholderNPC>()))
                    {

                        timer++;
                        if (timer == 439) // almost exactly 6.66 seconds after the drop starts floating
                        {
                            SoundEngine.PlaySound(SoundID.ScaryScream with { MaxInstances = 0 }, item.Center);
                        }
                        if (timer >= 439)
                        {
                            int num116 = Dust.NewDust(new Vector2(item.position.X, item.position.Y + 2f), item.width, item.height, DustID.PurpleTorch, 1, 1, 100, default, 2f);
                            Main.dust[num116].noGravity = true;
                            Main.dust[num116].scale *= Main.rand.NextFloat(0.5f, 1.5f);

                            Main.dust[num116].velocity.X *= Main.rand.NextFloat(-0.5f, 0.5f) * 50;
                            Main.dust[num116].velocity.Y *= Main.rand.NextFloat(-0.5f, 0.5f) * 50;
                            player.AddBuff(BuffID.Horrified, 1);
                        }
                        if (timer == 746) // 666 ticks after it starts floating
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                float radius = 300 / 41.67f;
                                Vector2 vel = Main.rand.NextVector2CircularEdge(radius, radius);
                                Dust d = Dust.NewDustPerfect(item.Center, DustID.PurpleTorch, vel * 2.5f, Scale: 3f);
                                if (Main.rand.NextBool(3))
                                {
                                    d.scale *= Main.rand.NextFloat(1.25f, 1.5f);
                                    d.velocity *= Main.rand.NextFloat(1.25f, 1.5f);
                                }
                                d.noGravity = true;
                            }
                            timer = 889;
                            SoundEngine.PlaySound(SoundID.NPCDeath10 with { MaxInstances = 0 }, item.Center);

                            int spawnPositionX = (int)(player.Center.X) + Main.rand.Next(-100, 100) * 16;
                            int spawnPositionY = (int)(player.Center.Y) + 80 * 16;
                            NPC.NewNPC(Item.GetSource_None(), spawnPositionX, spawnPositionY, NPCType<BeholderNPC>(), ai0: -369);


                            if (Main.netMode == 0)
                            {
                                Main.NewText("A Beholder has awoken!", 175, 75);

                            }
                            else if (Main.netMode == 2)
                            {
                                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("A Beholder has awoken!"), new Color(175, 75, 255));
                            }
                            return;
                        }
                    }
                }

            }
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

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (player.inferno)
            {
                if (player.inferno)
                {
                    Lighting.AddLight((int)(target.Center.X / 16f), (int)(target.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
                    int OnFireID = 24;
                    float range = 100f;
                    int RingDamage = damageDone / 10;
                    if (RingDamage < 1)
                    {
                        RingDamage = 1;
                    }
                    int dustsToMake = 5 + damageDone / 10;
                    for (int i = 0; i < dustsToMake; i++)
                    {
                        float radius = range / 62.5f;
                        // Why 62.5f and not 41.67?
                        // This is 150% of 41.67, because below the extra dusts get increased distance, with a max of 50% more.
                        // Therefore, the circle of flames more or less accurately represents the radius of the fire ring.
                        Vector2 speed = Main.rand.NextVector2CircularEdge(radius, radius);
                        Dust d = Dust.NewDustPerfect(target.Center, DustID.Torch, speed * 5, Scale: 3f);
                        if (Main.rand.NextBool(3))
                        {
                            d.scale *= Main.rand.NextFloat(1.25f, 1.5f);
                            d.velocity *= Main.rand.NextFloat(1.25f, 1.5f);
                        }
                        d.noGravity = true;
                    }
                    int NPCLimit = 0;
                    for (int k = 0; k < 200; k++)
                    {
                        NPC nPC = Main.npc[k];
                        if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= range)
                        {
                            ++NPCLimit;
                            if (NPCLimit < 3)
                            {
                                float finalDefense = nPC.defense - player.GetArmorPenetration(DamageClass.Generic);
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
                                RingDamage += (int)finalDefense / 2;
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
                    player.AddBuff(BuffID.Honey, 1800, false);
                    return;
            }
            return;
        }
        public int useCount = 0;
       
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Speed")
                {
                    Player player = Main.player[item.playerIndexTheItemIsReservedFor];
                    float roundedUseAnim = MathF.Round(item.useAnimation / (1 + (player.GetAttackSpeed(item.DamageType) - 1 + player.GetAttackSpeed(DamageClass.Generic) - 1) * ItemID.Sets.BonusAttackSpeedMultiplier[item.type]));

                    if (item.CountsAsClass(DamageClass.SummonMeleeSpeed))
                    {
                        roundedUseAnim = MathF.Round(item.useAnimation / (1 + ((player.GetAttackSpeed(item.DamageType) - 1 + player.GetAttackSpeed(DamageClass.Melee) - 1 + player.GetAttackSpeed(DamageClass.Generic) - 1) * ItemID.Sets.BonusAttackSpeedMultiplier[item.type])));
                    }
                    float attacksPerSecond = 60 / roundedUseAnim;
                    line.Text = MathF.Round(attacksPerSecond, 1) + " attacks per second";

                    if (item.CountsAsClass(DamageClass.Melee) && !item.CountsAsClass(DamageClass.MeleeNoSpeed) && !item.shootsEveryUse && (item.shoot != 0 || item.GetGlobalItem<HardmodeSwords>().aura != 0)) // write down the projectile's attack speed
                    {
                        float projUseTime = MathF.Round(item.useTime / (1 + (player.GetAttackSpeed(DamageClass.MeleeNoSpeed) - 1 + player.GetAttackSpeed(DamageClass.Generic) - 1) * ItemID.Sets.BonusAttackSpeedMultiplier[item.type]));
                        attacksPerSecond = 60 / projUseTime;
                        line.Text += "\n" + MathF.Round(attacksPerSecond, 1) + " projectiles per second";
                    }
                    if (item.axe > 0 || item.hammer > 0)

                    {
                        attacksPerSecond = 60 / item.useTime;
                        line.Text = MathF.Round(attacksPerSecond, 1) + " uses per second";
                    }
                    if (item.pick > 0) 
                    {
                        float projUseTime = MathF.Round(item.useTime * (player.pickSpeed) - 1);
                        if (item.GetGlobalItem<DrillItems>().drillSpeed != -1)
                        {

                            projUseTime = item.useTime;

                        }
                        if (projUseTime < 1)
                            projUseTime = 1;
 
                        attacksPerSecond = 60 / projUseTime;
                        line.Text = MathF.Round(attacksPerSecond, 1) + " uses per second";
                    }
                    else if (item.useAnimation != item.useTime && item.reuseDelay != 0) // for weapons like CAR, eventide  
                    {
                        attacksPerSecond = 60 / (roundedUseAnim + item.reuseDelay);
                        line.Text = MathF.Round(attacksPerSecond, 1) + " attacks per second";

                    }
                    if (item.type == ItemID.DD2PhoenixBow)
                    { 
                          line.Text = "3.3 attacks per second";
                    }
                    if (item.type == ItemID.VortexBeater)
                    {
                        line.Text = "12 attacks per second\n1.7 rockets per second";
                    }
                    if (item.type == ItemID.Phantasm)
                    {
                        line.Text = "Builds up from 2.5 to 3.3 attacks per second";
                    }
                    if (item.type == ItemID.LaserMachinegun)
                    {
                        line.Text = "Builds up from 2 to 20 attacks per second";
                    }
                }
            }
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
                            line.Text = "10% increased movement speed";
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
                case ItemID.StarinaBottle:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases maximum mana by 20 when near it";
                        }
                    }
                    break;
                case ItemID.GuideVoodooDoll:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0" && NPC.downedPlantBoss)
                        {
                            line.Text += "\nFeared by powerful underworld foes";
                        }
                    }
                    break;
            }
        }
    }
}
