using System;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Weapons.Magic.DreamEater.Dusts
{
    public class DreamEaterDustPurple : ModDust
    {
        public override string Texture => "TRAEProject/NewContent/Items/Weapons/Magic/DreamEater/Dusts/DreamEaterDust";
        public override void OnSpawn(Dust dust)
        {
            int index = Main.rand.Next(3);
            int dimensions = 20;
            dust.frame = new Microsoft.Xna.Framework.Rectangle(0, index * dimensions, dimensions, dimensions);

        }
        public override bool Update(Dust dust)
        {
            if(dust.customData is not int)
            {
                dust.customData = 0;
            }
            if(dust.customData is int)
            {
                int timer = (int)dust.customData;
                timer += 1;
                dust.scale = (float)timer / 40f;
                dust.scale *= dust.scale;
                dust.scale = 1f - dust.scale;
                dust.scale *= .25f;
                dust.customData = timer;

            }
            dust.position += dust.velocity * .5f;
            dust.velocity *= 0.9f;
            dust.position += dust.velocity * .5f;
            if (dust.scale < 0.05f)
            {
                dust.active = false;
                return false;
            }
          //  dust.scale -= MathF.Max(0.001f, (1f - dust.scale) * 0.05f);//initially scale down slowly, then speed up
            return false;
        }
    }
  //  public class DreamEaterDustPink : DreamEaterDustPurple { }
}
