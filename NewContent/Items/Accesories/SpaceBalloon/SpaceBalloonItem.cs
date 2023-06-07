using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Accesories.WeirdBundle;
using TRAEProject.NewContent.Items.Accesories.ExtraJumps;

namespace TRAEProject.NewContent.Items.Accesories.SpaceBalloon
{
    [AutoloadEquip(EquipType.Balloon)]
    public class SpaceBalloonItem: ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; 
            // DisplayName.SetDefault("Space Balloon");
            // Tooltip.SetDefault("Increases jump height\nAllows reducing gravity by hodling up");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 50000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true; player.noFallDmg = true;

            player.GetModPlayer<SpaceBalloonPlayer>().SpaceBalloon += 1;
        }
    }
   
    
    public class SpaceBalloonPlayer : ModPlayer
    {
        public int SpaceBalloon = 0;
        public int SpaceBalloonTimer = 0;

        public override void ResetEffects()
        {
            SpaceBalloon = 0;

        }
        public override void UpdateDead()
        {
            SpaceBalloon = 0;
            SpaceBalloonTimer = 0;
        }

        public override void PostUpdate()
        {
            if (Player.velocity.Y == 0)
            {
                SpaceBalloonTimer = 0;
            }
            if (Player.velocity.Y != 0 && SpaceBalloon > 0)
            {
                SpaceBalloonTimer += Player.empressBrooch ? 2 : 0;
                if (SpaceBalloonTimer >= 720)
                {
                    Player.RefreshMovementAbilities(true);
                    Player.GetModPlayer<TRAEJumps>().RestoreJumps();
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item112 with { MaxInstances = 0 });
                    for (int i = 0; i < 30; ++i)
                    {
                        Vector2 position10 = new Vector2(Player.position.X, Player.position.Y);
                        Dust dust = Dust.NewDustDirect(position10, Player.width, Player.height, DustID.BubbleBurst_Purple, 0f, 0f, 100, default, 2f);
                        dust.velocity *= 1.2f;
                    }
                    SpaceBalloonTimer = 0;
                }
            }
        }       
    }
}
