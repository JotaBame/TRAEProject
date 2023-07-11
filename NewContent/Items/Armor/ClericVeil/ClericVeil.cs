using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TRAEProject.Common;
using Terraria.Audio;

namespace TRAEProject.NewContent.Items.Armor.ClericVeil
{
	[AutoloadEquip(EquipType.Head)]
    public class ClericVeil : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.width = 22;
            Item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 2;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.AmethystRobe || body.type == ItemID.TopazRobe || body.type == ItemID.SapphireRobe || body.type == ItemID.EmeraldRobe || body.type == ItemID.RubyRobe || body.type == ItemID.DiamondRobe || body.type == ItemID.AmberRobe || body.type == ItemID.GypsyRobe;
        }
        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            base.SetMatch(male, ref equipSlot, ref robes);
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Your life regen will be transfered to the nearby teamate that needs it the most";
            player.GetModPlayer<VeilSetBonus>().hasBonus = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.LifeCrystal, 1)
                .AddIngredient(ItemID.BandofRegeneration, 1)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
    public class VeilSetBonus : ModPlayer
    {
        public bool hasBonus;
        public int bonusRegen;
        public override void ResetEffects()
        {
            hasBonus = false;
        }
        public override void UpdateLifeRegen()
        {
            if(bonusRegen > 0)
            {
                Player.lifeRegen += bonusRegen;
                bonusRegen = 0;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if(hasBonus)
            {
                int minLife = Player.statLife;
                int playerIndex = -1;
                for(int i =0; i < Main.player.Length; i++)
                {
                    if(i == Player.whoAmI)
                    {
                        continue;
                    }
                    if(Main.player[i].statLife < minLife && (Main.player[i].Center - Player.Center).Length() < 500 && Main.player[i].team == Player.team)
                    {
                        playerIndex = i;
                        minLife = Main.player[i].statLife;
                    }
                }
                if(playerIndex != -1)
                {
                    Main.player[playerIndex].GetModPlayer<VeilSetBonus>().bonusRegen += Player.lifeRegen;
                    Player.lifeRegen = 0;
                    
                    if (Main.rand.Next(3) > 0)
                    {
                        Vector2 center = Player.Center;
                        center.X += (float)Main.rand.Next(-100, 100) * 0.05f;
                        center.Y += (float)Main.rand.Next(-100, 100) * 0.05f;
                        center += Player.velocity;
                        int num = Dust.NewDust(center, 1, 1, DustID.LifeDrain);
                        Main.dust[num].velocity *= 0f;
                        Main.dust[num].scale = (float)Main.rand.Next(70, 85) * 0.01f;
                        Main.dust[num].fadeIn = playerIndex + 1;
                    }
                }
            }
        }
    }
}