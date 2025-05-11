using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using TRAEProject.NewContent.Items.Armor.Joter;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;

namespace TRAEProject.Changes.Items
{
    public class Minecarts : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if(item.type == ItemID.DiggingMoleMinecart)
            {
                item.value = Item.buyPrice(gold: 6);
            }
        }
    }
}