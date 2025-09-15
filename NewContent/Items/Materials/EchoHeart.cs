using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TRAEProject.NewContent.Items.Materials
{
    public class EchoHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
  
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 0, 25, 0);
        }
        int timer = 0;
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
 

            if (timer < 80)
                timer++;
            if (timer >= 80)
            {
                maxFallSpeed = 0;
                if (Main.rand.NextBool(4))
                {
                    int num117 = Dust.NewDust(new Vector2(Item.position.X, Item.position.Y + 2f), Item.width, Item.height, DustID.PinkTorch, Item.velocity.X * 0.2f, Item.velocity.Y * 0.2f, 100, default, 2f);
                    Main.dust[num117].noGravity = true;
                    Main.dust[num117].velocity.X *= 1f;
                    Main.dust[num117].velocity.Y *= 1f;
                }


            }


        }
    }
     
}
