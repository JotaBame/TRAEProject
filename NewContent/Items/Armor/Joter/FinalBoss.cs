using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using System.Security.Cryptography.X509Certificates;
using TRAEProject.Common.ModPlayers;
using TRAEProject.Changes.Accesory;
using TRAEProject.Changes;
using Terraria.GameContent.Prefixes;
namespace TRAEProject.NewContent.Items.Armor.Joter
{
    [AutoloadEquip(EquipType.Head)]
    public class FinalBoss : ModItem
    {
        public override void SetStaticDefaults()
        {

            // DisplayName.SetDefault("Final Boss");
            // Tooltip.SetDefault("!srepoleved gnitanosrepmi snoitanimoba gnitanosrepmi rof taerG");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.width = 32; 
            Item.vanity = true;
            Item.height = 32;
        }
        public override void UpdateEquip(Player player)
        {
            if (NPC.downedMoonlord)
            {
                player.GetAttackSpeed<GenericDamageClass>() += 0.25f;
                player.lifeRegen += 2;
                player.statDefense += 20;
                player.GetModPlayer<MeleeStats>().weaponSize += 0.2f;
                player.GetModPlayer<MeleeStats>().meleeVelocity += 0.3f;
                player.GetModPlayer<RangedStats>().rangedVelocity += 0.5f;
                player.GetModPlayer<Mana>().manaRegenBoost += 0.25f;
            }
        }
    }
}
