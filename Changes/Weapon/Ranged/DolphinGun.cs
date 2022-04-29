using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace TRAEProject.Changes.Weapon.Ranged
{
    public class QwertyFlexOnBame : ModPlayer
    {
        static List<Vector2> Dolhphin = new List<Vector2>();
        //call this in the Mod's Load() method
        public static void CreateDolphin()
        {
            if (!Main.dedServ)
            {
                var immediate = AssetRequestMode.ImmediateLoad;
                Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/Changes/Weapon/Ranged/DolphinOutline", immediate).Value;
                Color[] dataColors = new Color[texture.Width * texture.Height]; //Color array
                texture.GetData(dataColors);
                for (int c = 0; c < dataColors.Length; c++)
                {
                    if (dataColors[c].A != 0)
                    {
                        Vector2 v = new Vector2(c % texture.Width, c / texture.Width);
                        v *= 4;
                        v.X -= texture.Width * 2;
                        v.Y -= texture.Height * 2;
                        Dolhphin.Add(v);
                    }
                }
            }
        }
        int dolphincount = 0;
        public override void PostItemCheck()
        {
            if (Player.HeldItem.type == ItemID.SDMG && Player.itemTime == Player.itemAnimationMax - 1)
            {
                ++dolphincount;
                if (dolphincount == 20)
                {
                    dolphincount = 0;
                    for (int b = 0; b < Dolhphin.Count; b++)
                    {
                        Player form = Player;
                        bool canShoot = form.HasAmmo(form.inventory[form.selectedItem], canUse: true) && !form.noItems && !form.CCed;
                        int projToShoot = 14;
                        float speed = 14f;
                        int usedAmmoItemID = 0;
                        int Damage = form.GetWeaponDamage(form.inventory[form.selectedItem]) / 2;
                        float KnockBack = form.inventory[form.selectedItem].knockBack;
                        if (canShoot)
                        {
                            form.PickAmmo(form.inventory[form.selectedItem], ref projToShoot, ref speed, ref canShoot, ref Damage, ref KnockBack, out usedAmmoItemID, true);
                            KnockBack = form.GetWeaponKnockback(form.inventory[form.selectedItem], KnockBack);
                            Vector2 unit = (Main.MouseWorld - Player.Center);
                            unit.Normalize();
                            float dir = unit.ToRotation();
                            Vector2 offset = Dolhphin[b];
                            if (Main.MouseWorld.X < Player.Center.X)
                            {
                                offset.Y *= -1;
                            }
                            offset = offset.RotatedBy(dir);
                            Projectile p = Main.projectile[Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center + offset, unit * speed, projToShoot, Damage, KnockBack, Player.whoAmI)];
                            p.timeLeft = Math.Min(p.timeLeft, 45);
                        }
                    }
                }
            }
        }
    }
    public class SDMGChange : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.SDMG)
            {           
            }
        }
    }
}