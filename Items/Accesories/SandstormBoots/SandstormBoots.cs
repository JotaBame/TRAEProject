using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Items.Accesories.SandstormBoots
{
	[AutoloadEquip(EquipType.Shoes)]
    class SandstormBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Sandstorm Boots");
<<<<<<< Updated upstream:Items/Accesories/SandstormBoots/SandstormBoots.cs
            Tooltip.SetDefault("25% increased movement speed\nThe wearer can perform an improved double jump\nRun even faster in sand");
=======
            Tooltip.SetDefault("25% increased running speed\nThe wearer can perform an improved double jump\nRunning and jumping speed increased by 25% on sand, and for 4 seconds after leaving it");
>>>>>>> Stashed changes:NewContent/Items/Accesories/SandstormBoots/SandstormBoots.cs
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 3);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.accRunSpeed = 4.8f;
<<<<<<< Updated upstream:Items/Accesories/SandstormBoots/SandstormBoots.cs
			player.moveSpeed += 0.25f; 
            player.desertBoots = true;
=======
            if (player.velocity.Y == 0)
            {
                player.moveSpeed += 0.25f;
            }
            player.desertBoots = false; 
            player.GetModPlayer<AccesoryEffects>().sandRunning = true;
>>>>>>> Stashed changes:NewContent/Items/Accesories/SandstormBoots/SandstormBoots.cs
            player.hasJumpOption_Sandstorm = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.SandBoots, 1)
                .AddIngredient(ItemID.SandstorminaBottle, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
