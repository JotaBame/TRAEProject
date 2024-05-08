using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Misc.PermaBuffs
{
    class SpeedWish : ModItem
    {
        public override void SetStaticDefaults() 
        {
            // DisplayName.SetDefault("Wish for Speed");
            // Tooltip.SetDefault("Permanently increases movement speed by 15%\nYou have 1 wish... as long as its 15% movement speed.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// Registers a vertical animation with 4 frames and each one will last 5 ticks (1/12 second)
			ItemID.Sets.ItemIconPulse[Item.type] = true; // The item pulses while in the player's inventory
			ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
		}
        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.maxStack = 1;
            //Item.useTime = Item.useAnimation = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(silver: 20);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 20;
            Item.UseSound = SoundID.NPCDeath6;
        }
       

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 0;
            Item.velocity.X = Item.velocity.X * 0.95f;
            if ((double)Item.velocity.X < 0.1 && (double)Item.velocity.X > -0.1)
            {
                Item.velocity.X = 0f;
            }
            Item.velocity.Y = Item.velocity.Y * 0.95f;
            if ((double)Item.velocity.Y < 0.1 && (double)Item.velocity.Y > -0.1)
            {
                Item.velocity.Y = 0f;
            }
            Lighting.AddLight(Item.Center, 1f, 1f, 1f);
        }
    }

}