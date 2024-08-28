using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using TRAEProject.Changes.Prefixes;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Misc.Potions
{
    public class TeleportationVial : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

            // DisplayName.SetDefault("Teleportation Vial");
            // Tooltip.SetDefault("Teleports you to the position of the mouse");
        }
        public override void SetDefaults()
        {
            Item.DefaultToHealingPotion(12, 34, 50);
            Item.consumable = true;
Item.maxStack = 30;
            //Item.useTime = Item.useAnimation = 30;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(silver: 25);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            //Item.UseSound = SoundID.Item1;
        }
        public override void OnConsumeItem(Player player)
        {
            Vector2 pointPoisition = default(Vector2);
            pointPoisition.X = (float)Main.mouseX + Main.screenPosition.X;
            if (player.gravDir == 1f)
            {
                pointPoisition.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
            }
            else
            {
                pointPoisition.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
            }
            pointPoisition.X -= player.width / 2;
            player.LimitPointToPlayerReachableArea(ref pointPoisition);
            if (!(pointPoisition.X > 50f) || !(pointPoisition.X < (float)(Main.maxTilesX * 16 - 50)) || !(pointPoisition.Y > 50f) || !(pointPoisition.Y < (float)(Main.maxTilesY * 16 - 50)))
            {
                return;
            }
            int num = (int)(pointPoisition.X / 16f);
            int num2 = (int)(pointPoisition.Y / 16f);
            if ((Main.tile[num, num2].WallType == 87 && !NPC.downedPlantBoss && (double)num2 > Main.worldSurface) || Collision.SolidCollision(pointPoisition, player.width, player.height))
            {
                return;
            }
            player.Teleport(pointPoisition, 1);
            NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, pointPoisition.X, pointPoisition.Y, 1);
            int potionSickness = 30;
            if (player.pStone == true)
            {
                potionSickness = 23;
            }
            ;
            int findbuffIndex = player.FindBuffIndex(BuffID.PotionSickness);
            if (findbuffIndex != -1)
            {
  
                    player.DelBuff(findbuffIndex);
                
            }
            player.AddBuff(BuffID.PotionSickness, potionSickness * 60);
        }
        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemID.TeleportationPotion, 2)
                .AddIngredient(ItemID.ChaosFish, 1)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
  

}