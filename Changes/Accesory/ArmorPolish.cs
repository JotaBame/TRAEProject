using Microsoft.Xna.Framework;
using System.Collections.Generic; 
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Accesory
{
    public class ArmorPolish : GlobalItem
    {

        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.ArmorPolish)
            {
                item.vanity = true;
                item.hasVanityEffects = true;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (item.type == ItemID.ArmorPolish)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Your armor sparkles";
                    }

                }
            }
        }
    }
    public class ArmorSparkles : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            Item armorPolish = null;
            int useShader = 0;
            for (int i = 3; i < 10; i++)
            {
                if (!Player.hideVisibleAccessory[i] && (Player.armor[i].type == ItemID.ArmorPolish || Player.armor[i].type == ItemID.ArmorBracing))
                {
                    armorPolish = Player.armor[i];
                    useShader = i;
                }
            }
            for (int i = 13; i < 20; i++)
            {
                if (Player.armor[i].type == ItemID.ArmorPolish || Player.armor[i].type == ItemID.ArmorBracing)
                {
                    armorPolish = Player.armor[i];
                    useShader = i - 10;
                }
            }
            if (armorPolish != null && Main.rand.NextBool(15))
            {
                Rectangle r3 = Utils.CenteredRectangle(Player.Center, Vector2.One * Player.width);
                int num3 = Dust.NewDust(r3.TopLeft(), r3.Width, r3.Height, DustID.TreasureSparkle, 0f, 0f, 150, default, 0.25f);
                Main.dust[num3].fadeIn = 1f;
                Main.dust[num3].velocity *= 0.1f;
                Main.dust[num3].noLight = true;
                Main.dust[num3].shader = GameShaders.Armor.GetSecondaryShader(Player.dye[useShader].dye, Player);
            }
        }
    }
}
