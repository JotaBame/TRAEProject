using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRAEProject.Changes.Items;
using Terraria;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TRAEProject.Common.ModPlayers;
using Mono.Cecil;
using System.Collections;

namespace TRAEProject.Changes.Weapon.Melee
{
    class HardmodeSwords : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool FetidHeal = false;
        int aura = 0; // for use for those swords that also shoot a projectile on top of the aura
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.BeeKeeper:
                    item.damage = 26; // down from 30
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<BeekeeperAura>();
                    break;
                case ItemID.PearlwoodSword:
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<PearlwoodAura>();
                    item.rare = ItemRarityID.Pink;

                    break;
                case ItemID.PearlwoodHammer:
                    item.damage = 36; // up from 10
                    item.useTime = 14; // down from 19
                    item.useAnimation = 26; // down from 29
                    item.hammer = 70; // up from 55
                    item.rare = ItemRarityID.Pink;

                    break;
                case ItemID.BreakerBlade:
                    item.height = 92;
                    item.width = 92;
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<BreakerAura>();
                    break;
                case ItemID.CobaltSword:
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<CobaltAura>();
                    item.crit = 24; 
                    item.useTurn = false;

                    item.SetNameOverride("Cobalt Katana");
                    break;
                case ItemID.PalladiumSword:
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<PallaAura>();
                    item.useTurn = false;
                    item.SetNameOverride("Palladium Falcata");
                    break;
                case ItemID.MythrilSword:
                    item.damage = 75;
                    item.useTime = 30;
                    item.useAnimation = 30; 
                    item.noMelee = true; 
                    item.useTurn = false;

                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<MythrilAura>();
                    item.SetNameOverride("Mythril Zweihänder");
                    break;
                case ItemID.OrichalcumSword:
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<OrichalcumAura>();
                    item.SetNameOverride("Orichalcum Flamberge");
                    break;
                case ItemID.AdamantiteSword:
                    item.noMelee = true; item.useTurn = false;

                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<AdamantiteAura>();
                    item.SetNameOverride("Adamantite Broadsword");
                    break;
                case ItemID.TitaniumSword:
                    item.damage = 58; // down from 61
                    item.useTime = item.useAnimation = 25; // up from 20
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<TitaniumAura>();
                    item.SetNameOverride("Titanium Falchion");
                    break;

                //phasesabers
                case ItemID.PurplePhasesaber:
                case ItemID.YellowPhasesaber:
                case ItemID.BluePhasesaber:
                case ItemID.GreenPhasesaber:
                case ItemID.RedPhasesaber:
                case ItemID.OrangePhasesaber:
                case ItemID.WhitePhasesaber:
                    item.damage = 48;
                    item.crit = 24;
                    item.autoReuse = true;
                    item.useTurn = false;
                    break;
                case ItemID.ChlorophyteSaber:
                    //item.damage = 50;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.shootsEveryUse = true;
                    item.noMelee = true;
                    item.useTurn = false;
                    item.shoot = ProjectileType<SaberAura>();
                    break;
                case ItemID.ChlorophyteClaymore:
                    aura = ProjectileType<ClaymoreAura>();
                    item.noMelee = true;
                    item.useTurn = false;
                    item.useTime = 30;
                    break;
                case ItemID.Seedler:
                    item.useTime = 27;
                    item.useAnimation = 27;
                    break;
                case ItemID.Keybrand:
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<KeybrandAura>();
                    break;
                case ItemID.PiercingStarlight:
                    item.damage = 70; // down from 80
                    break;
                case ItemID.FetidBaghnakhs:
                    FetidHeal = true;
                    item.noMelee = true; item.useTurn = false;

                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<ShortAura>(); item.damage = 45;
                    item.useTime = 8;
                    item.useAnimation = 8;
                    break;
                case ItemID.PsychoKnife:
                    item.noMelee = true; item.useTurn = false;

                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<ShortAura>();
                    FetidHeal = true;
                    item.useTime = 8;
                    item.useAnimation = 8;
                    break;
                case ItemID.ChristmasTreeSword: 
                    item.useTime = 29;
                    item.useAnimation = 29;
                    item.damage = 70;
                    item.shootSpeed = 9f; 
                    item.knockBack = 4f;
                    item.autoReuse = true;
                    break;
                case ItemID.DD2SquireDemonSword:
                    item.noMelee = true;
                    item.shootsEveryUse = true;
                    item.shoot = ProjectileType<BrandAura>();
                    item.useTurn = false;

                    break;
                case ItemID.InfluxWaver:
                    item.noMelee = true;
                    aura = ProjectileType<InfluxAura>();
                    item.useTurn = false;
                    break;
                case ItemID.DD2SquireBetsySword:
                    item.noMelee = true;
                    aura = ProjectileType<DrgngnAura>();
                    item.useTurn = false;
                    break;
                case 3065: // star wrath
                    item.noMelee = true;
                    aura = ProjectileType<StarWrathAura>();
                    item.damage = 110; // down from 170
                    return;
                case 3063: // meowmere
                    return;


                case ItemID.EnchantedSword:
                    aura = ModContent.ProjectileType<EnchantedAura>();
                    item.damage = 24;
                    item.useAnimation = 18;
                    item.noMelee = true;
                    break;
     

            }
        }

        public override void UseAnimation(Item item, Player player)
        {

            if (aura != 0)
            {
                Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 velocity = new Vector2(Math.Sign(mousePosition.X - player.Center.X), 0); // determines direction

                    Projectile spawnedProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), player.MountedCenter - velocity * 2, velocity * 5 , aura, item.damage, item.knockBack, Main.myPlayer,
                            Math.Sign(mousePosition.X - player.Center.X) * player.gravDir, player.itemAnimationMax, player.GetAdjustedItemScale(item));
                    NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

                }
                return;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            switch (item.type)
            {
                case ItemID.BeeKeeper:
                case ItemID.Keybrand:
                case ItemID.BreakerBlade:
                case ItemID.CobaltSword:
                case ItemID.MythrilSword:
                case ItemID.AdamantiteSword:
                case ItemID.DD2SquireDemonSword:
                case ItemID.PalladiumSword:
                case ItemID.OrichalcumSword:
                case ItemID.TitaniumSword:
                case ItemID.FetidBaghnakhs:
                case ItemID.PsychoKnife:
                case ItemID.PearlwoodSword:
                case ItemID.ChlorophyteSaber:

                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile spawnedProj = Projectile.NewProjectileDirect(source, player.MountedCenter - velocity * 2, velocity * 5, type, damage, knockback, Main.myPlayer,
                            player.direction * player.gravDir, player.itemAnimationMax, player.GetAdjustedItemScale(item));
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);


                    }
                    return false;

                case ItemID.ChristmasTreeSword:
                    {
                        int number = 1 + Main.rand.Next(1, 2); // 1 to 3 shots
                        for (int i = 0; i < number; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(24));
                            Projectile.NewProjectile(player.GetSource_ItemUse(item), position, perturbedSpeed * 0.95f, ProjectileType<LightsLong>(), damage, knockback, player.whoAmI);
                        }                        
						number = 2 + Main.rand.Next(1, 3); // 3 or 5 shots
                        for (int i = 0; i < number; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(30));
                            perturbedSpeed.X *= Main.rand.NextFloat(0.5f, 1.5f);
                            Projectile.NewProjectile(player.GetSource_ItemUse(item), position, perturbedSpeed * 1.2f, ProjectileID.OrnamentFriendly, damage, knockback, player.whoAmI);
                        }
                        for (int i = 0; i < 10; i++)
                        {
                            Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 57, 0f, 0f, 255, default, Main.rand.Next(20, 26) * 0.1f);
                            dust.noLight = true;
                            dust.noGravity = true;
                            dust.velocity *= 0.5f;
                        }
                        Projectile.NewProjectile(player.GetSource_ItemUse(item), position, velocity * 1.8f, ProjectileType<Star1>(), damage, knockback, player.whoAmI);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item25 with { MaxInstances = 0 });
                        return false;
                    }
            }
            return true;
        }
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (item.CountsAsClass(DamageClass.Melee)/* && !item.CountsAsClass(DamageClass.MeleeNoSpeed)*/)
            {
                float bonusSize = 1f;
                switch (item.prefix)
                {
                    case PrefixID.Large:
                        bonusSize = (1.18f / 1.15f);
                        break;
                    case PrefixID.Massive:
                        bonusSize = (1.25f / 1.18f);
                        break;
                    case PrefixID.Dangerous:
                        bonusSize = (1.12f / 1.05f);
                        break;
                    case PrefixID.Bulky:
                        bonusSize = (1.2f / 1.1f);
                        break;
                }
                scale *= bonusSize;
                scale *= player.GetModPlayer<MeleeStats>().weaponSize;
            }
            if (item.type == ItemID.PiercingStarlight)
            {
                scale *= 1 + (player.GetAttackSpeed<MeleeDamageClass>() - 1) / 2;
            }    
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            
        }
        public override void HoldItem(Item item, Player player)
        {
            switch (item.type)
            {
                case ItemID.TerraBlade:
                    player.GetAttackSpeed<MeleeDamageClass>() /= 0.75f;
                    break;
                case ItemID.TitaniumSword:
                    player.onHitTitaniumStorm = true;
                    return;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.MythrilSword:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nDeals 25% more damage on critical hits";
                        }
                    }
                    break;
                case ItemID.TerraBlade:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Text == "75% benefit from attack speed boosts")
                        {
                            line.Text = "";
                        }
                    }
                    break;
            }
        }
    }
}
