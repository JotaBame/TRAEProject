using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TRAEProject
{
    public class Equipable : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if(item.type == ItemID.Umbrella || item.type == ItemID.TragicUmbrella)
            {
                item.accessory = true;
                item.hasVanityEffects = true;
                item.damage = 0;
                item.DamageType = DamageClass.Default;
                item.holdStyle = 0;
                item.useStyle = 0;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if ((item.type == ItemID.Umbrella && player.velocity.Y > 0) || item.type == ItemID.TragicUmbrella)
            {
                player.slowFall = true;
            }
        }
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if(item.type == ItemID.Umbrella || item.type == ItemID.TragicUmbrella)
            {
                return rand.Next(62, 81);
            }
            return base.ChoosePrefix(item, rand);
        }
    }
}