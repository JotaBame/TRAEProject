using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.SugarSkull
{
    //[AutoloadEquip(EquipType.Shield)]
    public class SugarSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
 
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 7, silver: 7, copper: 7);
            Item.width = 32;          
            Item.height = 40;
         }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SugarSkullEffec>().SugarSkull += 1;
            player.GetModPlayer<CritDamage>().critDamage += 0.1f;
            player.luck += 0.03f; // i have no idea why i'm doing this i just have a feeling it fits
        }
        public override void AddRecipes()
        {
         }

    }

}
 
public class SugarSkullEffec : ModPlayer
{
    public int SugarSkull = 0;
 
    public override void ResetEffects()
    {
        SugarSkull = 0;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (SugarSkull > 0)
        {
            if (!target.buffImmune[BuffID.OnFire])
            {
                TRAEDebuff.Apply<CrystalFire>(target, 300, 1);
 
            }
            if (hit.Crit && SugarSkull > 0) 
            {
                int duration = damageDone / Main.rand.Next(3, 6) * SugarSkull;
                TRAEDebuff.Apply<ObsidianSkulled>(target, duration, 3);
            }
        }
 
 
 
    }
    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (SugarSkull > 0)
        {
            if (!target.buffImmune[BuffID.OnFire])
            {
                TRAEDebuff.Apply<CrystalFire>(target, 300, 1);
            }
            if (hit.Crit && SugarSkull > 0)
            {
                int duration = damageDone / Main.rand.Next(3, 6) * SugarSkull;
                TRAEDebuff.Apply<ObsidianSkulled>(target, duration, 3);
            }
        }

    }


}