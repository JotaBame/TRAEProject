using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.BeholderItems;
using TRAEProject.NewContent.Projectiles;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Weapon.Melee
{
    public class PreHardmodeSwords : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ItemID.GoldBroadsword] = ItemID.EnchantedSword;
            ItemID.Sets.ShimmerTransformToItem[ItemID.PlatinumBroadsword] = ItemID.EnchantedSword;

        }
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                // WOODEN 
                case ItemID.WoodenSword:
                    item.scale = 1.1f; // up from 1

                    break;
                case ItemID.BorealWoodSword:
                case ItemID.PalmWoodSword:
                    item.scale = 1.1f; // up from 1

                    break;
                case ItemID.RichMahoganySword:
                case ItemID.EbonwoodSword:
                case ItemID.ShadewoodSword:
                    item.scale = 1.1f; // up from 1
                    break;
                case ItemID.CactusSword:
                    item.damage = 9; // up from 8
                    item.useTime = 25; // down from 32
                    item.useAnimation = 25;
                    break;
                // ORES
                case ItemID.CopperBroadsword:
                    item.damage = 13;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.TinBroadsword:
                    item.damage = 14;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.IronBroadsword:
                    item.damage = 15;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.LeadBroadsword:
                    item.damage = 16;
                    item.useAnimation = 20;
                    item.useTime = 20;
                    break;
                case ItemID.SilverBroadsword:
                    item.damage = 17;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.TungstenBroadsword:
                    item.damage = 18;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    break;
                case ItemID.GoldBroadsword:
                    item.damage = 19;
                    item.useTime = 18;
                    item.useAnimation = 18;
                    break;
                case ItemID.PlatinumBroadsword:
                    item.damage = 20;
                    item.useTime = 18;
                    item.useAnimation = 18;
                    break;
                
                    
                    // SHORTSWORDS
                
                
                case ItemID.CopperShortsword:
                case ItemID.TinShortsword:
                case ItemID.IronShortsword:
                case ItemID.LeadShortsword:
                case ItemID.SilverShortsword:
                case ItemID.TungstenShortsword:
                case ItemID.GoldShortsword:
                case ItemID.PlatinumShortsword:
                    item.autoReuse = true;
                    break;
                    
                    // OTHER
               
                               
                case ItemID.AntlionClaw:
                    item.damage = 14;// down from 16
                    item.useTime = 11;
                    item.useAnimation = 11;
                    break;
                case ItemID.ZombieArm:
                    item.damage = 15; // up from 12
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.knockBack = 8.25f;
                    item.autoReuse = true;
                    break;
                case ItemID.BoneSword:
                    item.damage = 24;
                    item.scale = 1.1f;
                    item.value = Item.buyPrice(gold: 5);
                    break;
                case ItemID.BatBat:
                    item.damage = 24;
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.value = Item.buyPrice(gold: 5);
                    break;
                case ItemID.PurpleClubberfish:
                    item.scale = 1.4f;
                    item.autoReuse = true;
                    break;
                case ItemID.LucyTheAxe:
                    item.damage = 42; // up from 26
                    item.useTime = 24; // up from 17, slows down axe speed tho
                    item.useAnimation = 24;
                    item.shoot = 0;
                    item.useTurn = false;
                    item.shootSpeed = 11f;
                    break;
                   

            }
        }
        

        /// SHOOT STUFF
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            switch (item.type)
            {
                case ItemID.NightsEdge:
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 });
                        return true;
  
            }
            return true;       
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.LucyTheAxe:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nRight click to throw her as a boomerang";
                        }
                    }
                    break;
                case ItemID.BeeKeeper:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
            }
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.type == ItemID.LucyTheAxe)
            {
                return true;
            }
            return base.AltFunctionUse(item, player);
        }
        bool swung = false;
        public override void UseAnimation(Item item, Player player)
        {
            if (item.type == ItemID.LucyTheAxe && swung == false && player.altFunctionUse != 2)
            {
                swung = true;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.LucyTheAxe)
            {
                if (player.ownedProjectileCounts[ProjectileType<ThrownLucy>()] > 0)
                {
                    return false;
                }
                if (player.altFunctionUse == 2)
                {
                    item.shoot = ProjectileType<ThrownLucy>();
                    item.noMelee = true;
                    item.DamageType = DamageClass.MeleeNoSpeed;
                    item.noUseGraphic = true;
                    swung = false;
                }
                if (player.altFunctionUse != 2)
                {
                    item.shoot = ProjectileType<Blank>(); 
                    item.DamageType = DamageClass.Melee;
                    item.noMelee = false;
                    item.noUseGraphic = false; 
                    if (swung)
                    {
                        item.shoot = ProjectileID.None;
                    }
                }

            }
            return base.CanUseItem(item, player);
        }
      
    }
    
}
