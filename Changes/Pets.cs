using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Misc.Potions;
using System.IO;
using Terraria.ModLoader.IO;
using TRAEProject.Common;
using TRAEProject.NewContent.NPCs.Underworld.ObsidianBasilisk;
using TRAEProject.NewContent.NPCs.Underworld.Phoenix;
using TRAEProject.NewContent.NPCs.Underworld.Salalava;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using Mono.CompilerServices.SymbolWriter;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;

namespace TRAEProject.Changes
{

    class PetItems : GlobalItem
    {
    }
    class PetProjectiles : GlobalProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.LightPet[ProjectileID.ChesterPet] = true;
            Main.lightPet[BuffID.ChesterPet] = true;
            Main.projPet[ProjectileID.ChesterPet] = false;
            Main.vanityPet[BuffID.ChesterPet] = false;
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if(projectile.type == ProjectileID.ChesterPet)
            {
                return false;
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if(projectile.type == ProjectileID.ChesterPet)
            {
                fallThrough = false;
                if(Main.player[projectile.owner].Center.Y > projectile.Center.Y)
                {
                    fallThrough = true;
                }
            }
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}