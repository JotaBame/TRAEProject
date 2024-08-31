using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace TRAEProject.Changes.NPCs.SolarEclipse
{
    public class TheFlysEndurance : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {

            if(npc.buffTime[buffIndex] <= 2)
            {
                npc.defense = npc.defDefense;
            }
            else
            {
                npc.defense = npc.defDefense;
                npc.defense += 200;
                if(Main.rand.NextBool(4))
                {
                    Dust d = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<EnduranceDustEffect>())];
                    d.velocity = Vector2.UnitY * -4f;
                    d.scale *= 2;
                }
            }
            
        }
    }
    public class TheFlysLevitation : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if(npc.buffTime[buffIndex] <= 2)
            {
                npc.rotation = 0;
                npc.noTileCollide = false;
            }
            else
            {
                npc.noTileCollide = true;
                npc.velocity.Y = -6;
                npc.rotation = npc.direction  * (MathF.PI / 15f) * npc.buffTime[buffIndex];
                for(int i = 0; i < 3; i++)
                {
                    Dust d = Dust.NewDustPerfect(npc.Center + TRAEMethods.PolarVector(30, MathF.PI * 2f * ((float)i / 3f) + npc.rotation), DustID.SilverFlame, Vector2.UnitY * -6);
                }
            }
            
        }
    }
    public class TheFlysSpeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.position.X += npc.velocity.X;

            if(Main.rand.NextBool(4))
            {
                Dust d = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<SpeedDustEffect>())];
                d.velocity = Vector2.UnitY * -4f;
                d.scale *= 2;
            }
        }
    }
}