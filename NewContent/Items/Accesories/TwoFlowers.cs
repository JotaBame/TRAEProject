using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.NewContent.Items.Accesories
{    [AutoloadEquip(EquipType.Waist)]
   public class TwoFlowers : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Two Flowers");
            // Tooltip.SetDefault("Magic critical strikes deal 11% increased damage and have a chance to spawn a mana star\nAutomatically uses mana potions when needed\n");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.sellPrice(gold:3);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lavaRose = true;
            player.GetModPlayer<CritDamage>().magicCritDamage += 0.1f;
            player.manaFlower = true;
            player.GetModPlayer<Mana>().newManaFlower = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.ObsidianRose, 1)
                .AddIngredient(ItemID.ManaFlower, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
