
using Terraria;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Misc.Potions
{
    public class ManaSurgePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item3;
            Item.healMana = 100;
            Item.useStyle = 9;
            Item.useTurn = true;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.width = 14;
            Item.height = 24;
            Item.rare = 4;
            Item.value = 1500;
        }
        public override void OnConsumeItem(Player player)
        {
            int maxTime = 3600 / (1 + player.GetModPlayer<AngelicStoneEffects>().stones);

            if (GetInstance<TRAEConfig>().ManaRework)
            {
 
                player.AddBuff(BuffID.ManaSickness, maxTime - 300, false); // subtract 300 because mana potions always add 5 seconds of mana sickness

            }
            else if (player.GetModPlayer<AngelicStoneEffects>().stones > 0)
            {
                maxTime = 300 / (1 + player.GetModPlayer<AngelicStoneEffects>().stones);

                player.buffTime[player.FindBuffIndex(BuffID.ManaSickness)] -= maxTime;
            }
            player.AddBuff(BuffType<Surge>(), 4 * 60);
        }
        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ItemID.GreaterManaPotion, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.CrystalShard, 3)
                .AddIngredient(ItemID.WormTooth, 3)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
    public class Surge : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.manaCost *= 0;
        }
    }

}