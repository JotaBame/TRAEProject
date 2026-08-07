using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRAEProject.NewContent.StarfuryTemple;

namespace TRAEProject.NewContent.Structures.StarfuryTemple

{
    public class StarfuryTempleTestItem : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BeamSword;//random texture I chose
        public override void SetDefaults()
        {
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ProjectileID.FireArrow;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            StarfuryTempleGen.MainGeneration();
            return false;
        }
    }
}
