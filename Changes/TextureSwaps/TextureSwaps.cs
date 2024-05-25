using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.ComponentModel;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace TRAEProject.Changes.TextureSwaps
{
    public class TextureSwaps : ModSystem
    {
        static Asset<Texture2D> GetTexture(string name)
        {
            return ModContent.Request<Texture2D>("TRAEProject/Changes/TextureSwaps/" + name);
        }
        static Asset<Texture2D> GetVanillaTexture(string name)
        {
            return ModContent.Request<Texture2D>("Terraria/Images/" + name);
        }
        public override void Load()
        {
            TRAERetextureConfig config = ModContent.GetInstance<TRAERetextureConfig>();
            if (config.StardustDragon)
            {
                TextureAssets.Buff[188] = GetTexture("Buff_188");
                SwapItem(ItemID.StardustDragonStaff);
                SwapProjectiles(ProjectileID.StardustDragon1, ProjectileID.StardustDragon2, ProjectileID.StardustDragon3, ProjectileID.StardustDragon4);
            }
            if (config.InfernoRing)
            {
                TextureAssets.FlameRing = GetTexture("FlameRing");
            }
            if (config.StarWrath)
            {
                SwapItem(ItemID.StarWrath);
                TextureAssets.Extra[36] = GetTexture("Extra_36");
                SwapProjectile(ProjectileID.StarWrath);
            }
            if (config.LunarPortal)
            {
                SwapProjectile(ProjectileID.MoonlordTurret);
                SwapProjectile(ProjectileID.MoonlordTurretLaser);
            }
            if (config.DarkLance)
            {
                SwapItem(ItemID.DarkLance);
            }
            if(config.PearlwoodBow)
            {
                SwapItem(ItemID.PearlwoodBow);
            }
            if (config.PearlwoodHammer)
            {
                SwapItem(ItemID.PearlwoodHammer);
            }
            if (config.PearlwoodSword)
            {
                SwapItem(ItemID.PearlwoodSword);
            }
        }
        public override void Unload()
        {
            TextureAssets.Buff[188] = GetVanillaTexture("Buff_188");
            UnswapItems(1343, 274, 3065, 3531, 659, 660, 661);
            UnswapProjectiles(503, 625, 626, 627, 628, 641, 642);
            TextureAssets.FlameRing = GetVanillaTexture("FlameRing");
            TextureAssets.Extra[36] = GetVanillaTexture("Extra_36");
        }
        static void SwapItem(int id)
        {
            TextureAssets.Item[id] = GetTexture("Item_" + id);
        }
        static void UnswapItems(params int[] ids)
        {
            foreach (int id in ids)
            {
                TextureAssets.Item[id] = GetVanillaTexture("Item_" + id);
            }
        }
        static void UnswapProjectiles(params int[] ids)
        {
            foreach (int id in ids)
            {
                TextureAssets.Projectile[id] = GetVanillaTexture("Projectile_" + id);
            }
        }
        static void SwapProjectiles(params int[] ids)
        {
            foreach (int id in ids)
            {
                TextureAssets.Projectile[id] = GetTexture("Projectile_" + id);
            }
        }
        static void SwapProjectile(int id)
        {
            TextureAssets.Projectile[id] = GetTexture("Projectile_" + id);
        }
    }


    public class TRAERetextureConfig : ModConfig //configuration settings
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        
        [Header("TextureSwaps")]

        [ReloadRequired]
        [DefaultValue(true)]
        public bool StardustDragon { get; set; }
        [ReloadRequired]
        [DefaultValue(true)]
        public bool InfernoRing { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool DarkLance { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool StarWrath { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool PearlwoodSword { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool PearlwoodHammer { get; set; }

        [ReloadRequired]
        [DefaultValue(false)]
        public bool PearlwoodBow { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool LunarPortal { get; set; }
    }
}
