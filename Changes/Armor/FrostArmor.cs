using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;

namespace TRAEProject.Changes.Armor
{
    public class FrostArmor : ModPlayer
    {
        public bool frostArmor = false;
        public int frostArmorCooldown = 0;
        int frostArmorMaxCooldown = 500;
        public override void ResetEffects()
        {
            frostArmor = false;
        }
        public override void UpdateDead()
        {
            frostArmor = false;
        }
        public override void PostUpdateEquips()
        {
            if (frostArmor && frostArmorCooldown < frostArmorMaxCooldown)
            {
                frostArmorCooldown++;
            }
            if (frostArmorCooldown == frostArmorMaxCooldown - 1)
            {
                SoundEngine.PlaySound(SoundID.NPCHit5 with { MaxInstances = 0 }, Player.Center);
            }
            if (frostArmor && frostArmorCooldown >= frostArmorMaxCooldown)
            {
                if (Main.rand.Next(3) == 0)
                {
                    Dust dust3 = Dust.NewDustDirect(new Vector2(Player.position.X - 2f, Player.position.Y - 2f), Player.width + 4, Player.height + 4, DustID.IceTorch, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 0, default, 1.25f);
                    dust3.noGravity = true;
                    dust3.velocity *= 0.75f;
                    dust3.velocity.X *= 0.75f;
                    dust3.velocity.Y -= 1f;
                    if (Main.rand.Next(4) == 0)
                    {
                        dust3.noGravity = false;
                        dust3.scale *= 0.5f;
                    }
                }
                if (Player.whoAmI == Main.myPlayer && Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] > 0 && Player.doubleTapCardinalTimer[0] != 15)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(6.5f, 6.5f);
                        Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Top, DustID.IceTorch, speed * 5, Scale: 3f);
                        d.noGravity = true;
                    }
                    
                    SoundEngine.PlaySound(SoundID.Item28 with { MaxInstances = 0 }, Player.Center);
                    for (int k = 0; k < 200; k++)
                    {
                        NPC nPC = Main.npc[k];
                        float distance = 350f;
                      
                        if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(Player.Center, nPC.Center) <= distance)
                        {
                            int duration = Main.rand.Next(300, 400);
                            if (nPC.lifeMax >= 10000)
                                duration = 480;
                            nPC.GetGlobalNPC<Freeze>().FreezeMe(nPC, duration);
                        }
                        
                    }
                    frostArmorCooldown = 0;
                }
            }
        }
        
    }
}
