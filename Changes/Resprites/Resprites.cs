using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using ReLogic.Content;

namespace TRAEProject.Changes.Resprites
{
    public class Resprites : ModSystem
    {
        //I've attempted to fix the lost texture on unloadbug and TRAE overwriting texture packs, but was not succesful, qwerty
        static string path = "TRAEProject/Changes/Resprites/";
        static string vsrPath = "TRAEProject/VanillaSpriteReference/";
        public override void PostSetupContent()
        {           
            if(!ModLoader.TryGetMod("DragonLens",out _))
                LoadSprites();//bandaid fix to dragonlens startup issue
        }
        public static void LoadSprites()
        {
            /*
            SpriteReplace(ref TextureAssets.Item[ItemID.StarWrath], VSRI(ItemID.StarWrath), Request<Texture2D>(path + "StarWrath", AssetRequestMode.ImmediateLoad));
            SpriteReplace(ref TextureAssets.Item[ProjectileID.StarWrath], VSRP(ProjectileID.StarWrath), Request<Texture2D>(path + "StarWrathProjectile", AssetRequestMode.ImmediateLoad));

            SpriteReplace(ref TextureAssets.Item[ProjectileID.StardustDragon1], VSRP(ProjectileID.StardustDragon1), Request<Texture2D>(path + "LunarDragonHead", AssetRequestMode.ImmediateLoad));
            */
            TextureAssets.Item[ItemID.PearlwoodSword] = Request<Texture2D>(path + "PearlwoodSword", AssetRequestMode.ImmediateLoad);
            TextureAssets.Item[ItemID.PearlwoodHammer] = Request<Texture2D>(path + "PearlwoodHammer", AssetRequestMode.ImmediateLoad);
            TextureAssets.Item[ItemID.PearlwoodBow] = Request<Texture2D>(path + "PearlwoodBow", AssetRequestMode.ImmediateLoad);

            TextureAssets.Item[ItemID.StarWrath] = Request<Texture2D>(path + "StarWrath", AssetRequestMode.ImmediateLoad);
            TextureAssets.Projectile[ProjectileID.StarWrath] = Request<Texture2D>(path + "StarWrathProjectile", AssetRequestMode.ImmediateLoad);
            TextureAssets.FlameRing = Request<Texture2D>(path + "FlameRing");
            TextureAssets.Item[ItemID.StardustDragonStaff] = Request<Texture2D>(path + "LunarDragonStaff", AssetRequestMode.ImmediateLoad);

            TextureAssets.Projectile[ProjectileID.StardustDragon1] = Request<Texture2D>(path + "LunarDragonHead", AssetRequestMode.ImmediateLoad);
            TextureAssets.Projectile[ProjectileID.StardustDragon2] = Request<Texture2D>(path + "LunarDragonSpike", AssetRequestMode.ImmediateLoad);
            TextureAssets.Projectile[ProjectileID.StardustDragon3] = Request<Texture2D>(path + "LunarDragonBody", AssetRequestMode.ImmediateLoad);
            TextureAssets.Projectile[ProjectileID.StardustDragon4] = Request<Texture2D>(path + "LunarDragonTail", AssetRequestMode.ImmediateLoad);
            TextureAssets.Buff[BuffID.StardustDragonMinion] = Request<Texture2D>(path + "LunarDragonBuff", AssetRequestMode.ImmediateLoad);
            TextureAssets.Item[ItemID.MoonlordTurretStaff] = Request<Texture2D>(path + "StardustPortalStaff", AssetRequestMode.ImmediateLoad);
            TextureAssets.Projectile[ProjectileID.MoonlordTurret] = Request<Texture2D>(path + "StardustPortal", AssetRequestMode.ImmediateLoad);
            TextureAssets.Projectile[ProjectileID.MoonlordTurretLaser] = Request<Texture2D>(path + "StardustPortalBeam", AssetRequestMode.ImmediateLoad);
            //TextureAssets.Extra[50] = Request<Texture2D>(path + "Extra_50");

            TextureAssets.Item[ItemID.DarkLance] = Request<Texture2D>(path + "DarkLance", AssetRequestMode.ImmediateLoad);
        }
        public static void UnloadSprites()
        {
            /*
            TextureAssets.Item[ItemID.StarWrath] = VSRI(ItemID.StarWrath);
            TextureAssets.Projectile[ProjectileID.StarWrath] = VSRP(ProjectileID.StarWrath);

            TextureAssets.Item[ItemID.StardustDragonStaff] = VSRI(ItemID.StardustDragonStaff);

            TextureAssets.Projectile[ProjectileID.StardustDragon1] = VSRP(ProjectileID.StardustDragon1);
            TextureAssets.Projectile[ProjectileID.StardustDragon2] = VSRP(ProjectileID.StardustDragon2);
            TextureAssets.Projectile[ProjectileID.StardustDragon3] = VSRP(ProjectileID.StardustDragon3);
            TextureAssets.Projectile[ProjectileID.StardustDragon4] = VSRP(ProjectileID.StardustDragon4);
            TextureAssets.Buff[BuffID.StardustDragonMinion] =  Request<Texture2D>(vsrPath + "Buff" + BuffID.StardustDragonMinion, AssetRequestMode.ImmediateLoad);
            TextureAssets.Item[ItemID.MoonlordTurretStaff] = VSRI(ItemID.MoonlordTurretStaff);
            TextureAssets.Projectile[ProjectileID.MoonlordTurret] = VSRP(ProjectileID.MoonlordTurret); 
            TextureAssets.Projectile[ProjectileID.MoonlordTurretLaser] = VSRP(ProjectileID.StarWrath);

            TextureAssets.Item[ItemID.DarkLance] = VSRI(ItemID.DarkLance);
            */
            /*
            SpriteReplace(ref TextureAssets.Item[ItemID.StarWrath], Request<Texture2D>(path + "StarWrath", AssetRequestMode.ImmediateLoad), VSRI(ItemID.StarWrath));
            SpriteReplace(ref TextureAssets.Item[ProjectileID.StarWrath], Request<Texture2D>(path + "StarWrathProjectile", AssetRequestMode.ImmediateLoad), VSRP(ProjectileID.StarWrath));

            SpriteReplace(ref TextureAssets.Item[ProjectileID.StardustDragon1], Request<Texture2D>(path + "LunarDragonHead", AssetRequestMode.ImmediateLoad), VSRP(ProjectileID.StardustDragon1));
            */
        }
        /*
        public override void Load()
        {
            /*

            
            TextureAssets.Item[ItemID.StarWrath] = Request<Texture2D>(path + "StarWrath");
            TextureAssets.Projectile[ProjectileID.StarWrath] = Request<Texture2D>(path + "StarWrathProjectile");

            TextureAssets.Item[ItemID.StardustDragonStaff] = Request<Texture2D>(path + "LunarDragonStaff");

            TextureAssets.Projectile[ProjectileID.StardustDragon1] = Request<Texture2D>(path + "LunarDragonHead");
            TextureAssets.Projectile[ProjectileID.StardustDragon2] = Request<Texture2D>(path + "LunarDragonSpike");
            TextureAssets.Projectile[ProjectileID.StardustDragon3] = Request<Texture2D>(path + "LunarDragonBody");
            TextureAssets.Projectile[ProjectileID.StardustDragon4] = Request<Texture2D>(path + "LunarDragonTail");
            TextureAssets.Buff[BuffID.StardustDragonMinion] = Request<Texture2D>(path + "LunarDragonBuff");
            TextureAssets.Item[ItemID.MoonlordTurretStaff] = Request<Texture2D>(path + "StardustPortalStaff");
            TextureAssets.Projectile[ProjectileID.MoonlordTurret] = Request<Texture2D>(path + "StardustPortal");
            TextureAssets.Projectile[ProjectileID.MoonlordTurretLaser] = Request<Texture2D>(path + "StardustPortalBeam");

            TextureAssets.Item[ItemID.DarkLance] = Request<Texture2D>(path + "DarkLance");
            
        }
        public override void OnModUnload()
        {
            base.OnModUnload();
        }
        */
        static Asset<Texture2D> VSRI(int id)
        {
            return Request<Texture2D>(vsrPath + "Item_" + id, AssetRequestMode.ImmediateLoad);
        }
        static Asset<Texture2D> VSRP(int id)
        {
            return Request<Texture2D>(vsrPath + "Projectile_" + id, AssetRequestMode.ImmediateLoad);
        }
        static void SpriteReplace(ref Asset<Texture2D> thisSprite, Asset<Texture2D> isThis, Asset<Texture2D> changeToThis)
        {
            if(SameSprite(thisSprite, isThis))
            {
                thisSprite = changeToThis;
            }
        }
        static bool SameSprite(Asset<Texture2D> a1, Asset<Texture2D> a2)
        {
            Texture2D t1 = a1.Value;
            Texture2D t2 = a2.Value;
            Color[] dataColors1 = new Color[t1.Width * t1.Height]; //Color array
            t1.GetData(dataColors1);
            Color[] dataColors2 = new Color[t2.Width * t2.Height]; //Color array
            t2.GetData(dataColors2);
            for (int c = 0; c < dataColors1.Length; c++)
            {
                if(dataColors1[c] != dataColors2[c])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
