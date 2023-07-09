
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes;

namespace TRAEProject.NewContent.Items.Accesories.AngelicStone
{
    public class AngelicStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Angelic Stone");
            // Tooltip.SetDefault("10% increased mana regeneration\nAutomatically uses mana potions when needed\nReduces the cooldown of mana sickness by 50%");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 4);

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AngelicStoneEffects>().stones += 1;
            player.GetModPlayer<Mana>().manaRegenBoost += 0.1f;
            player.manaFlower = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PhilosophersStone, 1)
                .AddIngredient(ItemID.BandofStarpower, 1)
                .AddIngredient(ItemID.NaturesGift, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
	}
        public class AngelicStoneEffects : ModPlayer
        {
            public int stones = 0;
            public override void ResetEffects()
            {
                stones = 0;
            }
        }
    
}
