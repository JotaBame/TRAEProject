using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.Plantera
{
    public class Plantera : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.Plantera)
            {
                npc.lifeMax = 54000;
                npc.behindTiles = true;
            }
        }
        float speed = 6;
        int currentAtk = 0;
        float timer = 0;
        int pullBackTime = 240;
        bool runOnce = true;
        int attackCounter = 0;
        float expertSpeedBonus = 1f;
        void Start()
        {

        }
        void Teleport(NPC npc, Player player)
        {
            float r = (npc.Center - player.Center).ToRotation() + 2f;
            Vector2 teleHere = player.Center + TRAEMethods.PolarVector(500, r);
            for (int i = 500; i < 1300; i += 20)
            {
                teleHere = player.Center + TRAEMethods.PolarVector(i, r);
                if (!Collision.CanHit(teleHere, 1, 1, player.Center, 1, 1) && ClosedRegion(npc, teleHere))
                {
                    break;
                }
            }
            npc.Center = teleHere;
            PickAttack(npc, player);
        }
        bool ClosedRegion(NPC npc, Vector2 center)
        {
            for (int i = 0; i < 9; i++)
            {
                Vector2 pos = center - new Vector2(npc.width / 2, npc.height / 2);
                Vector2 checkSpot = pos;
                switch (i)
                {
                    case 0:
                        checkSpot = pos;
                        break;
                    case 1:
                        checkSpot = pos + new Vector2(npc.width / 2, 0);
                        break;
                    case 2:
                        checkSpot = pos + new Vector2(npc.width, 0);
                        break;
                    case 3:
                        checkSpot = pos + new Vector2(npc.width, npc.height / 2);
                        break;
                    case 4:
                        checkSpot = pos + new Vector2(npc.width, npc.height);
                        break;
                    case 5:
                        checkSpot = pos + new Vector2(npc.width / 2, npc.height);
                        break;
                    case 6:
                        checkSpot = pos + new Vector2(0, npc.height);
                        break;
                    case 7:
                        checkSpot = pos + new Vector2(0, npc.height / 2);
                        break;
                    case 8:
                        checkSpot = center;
                        break;
                }
                Point coords = checkSpot.ToTileCoordinates();
                if (!Main.tile[coords.X, coords.Y].HasTile)
                {
                    return false;
                }
            }
            return true;
        }
        void PickAttack(NPC npc, Player player)
        {
            playedCharge = false;
            currentAtk++;
            attackCounter = 0;

            for (int i = 0; i < hooks.Length; i++)
            {
                if (hooks[i] != null && hooks[i].active)
                {
                    hooks[i].Kill();
                }
            }
            pullBackTime = 240;
            if ((float)npc.life / (float)npc.lifeMax < 0.5f)
            {
                if (currentAtk >= 11)
                {
                    currentAtk = 3;
                }
                if (currentAtk < 3)
                {
                    currentAtk = 3;
                }
                switch (currentAtk)
                {
                    case 6:
                        break;
                    case 10:
                        pullBackTime = 510;
                        attackCounter = -1;
                        break;
                    default:
                        pullBackTime = 80;
                        break;
                }
            }
            else
            {
                if (currentAtk >= 2)
                {
                    currentAtk = 0;
                }
                if (currentAtk == 1)
                {
                    pullBackTime = 300;
                }
            }
            //currentAtk = currentAtk == 0 ? 1 : 0;
            timer = 0;

            speed = 8;
            /*
            speed = 6 * expertSpeedBonus;
            if ((float)npc.life / (float)npc.lifeMax < 0.5f)
            {
                speed += 6;
            }
            */
            //Main.NewText(currentAtk);
        }
        void Movement(NPC npc, Player player)
        {
            float rot = (player.Center - npc.Center).ToRotation();
            if ((!Collision.CanHit(npc.Center + TRAEMethods.PolarVector(-20, rot), 1, 1, player.Center, 1, 1) || !Collision.CanHit(npc.Center + TRAEMethods.PolarVector(80, rot), 1, 1, player.Center, 1, 1)))
            {
                speed = 1f + (player.Center - npc.Center).Length() * 0.02f;
                npc.velocity = (player.Center - npc.Center).SafeNormalize(Vector2.UnitY) * speed;

                Dust(npc);

            }
            else
            {
                npc.velocity = Vector2.Zero;
                DoAttack(npc, player);
            }
        }
        void ReverseMovement(NPC npc, Player player)
        {
            speed = 6;
            npc.velocity = (player.Center - npc.Center).SafeNormalize(Vector2.UnitY) * -speed;
            timer += expertSpeedBonus;
            Dust(npc);
            if (timer > pullBackTime + 120 && (!Collision.CanHit(npc.Center + (player.Center - npc.Center).SafeNormalize(Vector2.UnitY) * 100, 1, 1, player.Center, 1, 1) && ClosedRegion(npc, npc.Center)) || (player.Center - npc.Center).Length() > 1800)
            {
                Teleport(npc, player);
            }
        }
        bool playedCharge = false;
        void Charge(NPC npc, Player player)
        {
            if (!playedCharge)
            {
                SoundEngine.PlaySound(SoundID.Roar, npc.Center, 0);
            }
            npc.velocity = TRAEMethods.PolarVector(16, npc.rotation - (float)Math.PI / 2);
            timer += expertSpeedBonus;
            Dust(npc);
            if (timer > pullBackTime + 120 && (!Collision.CanHit(npc.Center + (player.Center - npc.Center).SafeNormalize(Vector2.UnitY) * 100, 1, 1, player.Center, 1, 1) && ClosedRegion(npc, npc.Center)) || (player.Center - npc.Center).Length() > 1800)
            {
                Teleport(npc, player);
            }
        }
        void Dust(NPC npc)
        {
            float rot = npc.velocity.ToRotation();
            for (int i = 0; i < 260; i += 16)
            {
                for (int j = -50; j < 50; j += 16)
                {
                    Vector2 pos = npc.Center + TRAEMethods.PolarVector(i, rot) + TRAEMethods.PolarVector(j, rot + (float)Math.PI / 2f);
                    Point tileCoord = pos.ToTileCoordinates();
                    WorldGen.KillTile(tileCoord.X, tileCoord.Y, fail: true, effectOnly: true);
                }
            }

        }
        void DoAttack(NPC npc, Player player)
        {
            if (runOnce)
            {
                Start();
                runOnce = false;
            }

            timer += expertSpeedBonus;
            switch (currentAtk)
            {
                case 0:
                    SpitSeeds(npc, player);
                    break;
                case 1:
                    ThornBall(npc, player);
                    break;
                case 2:
                    Tentacles(npc, player);
                    break;
                case 6:
                    ThornBall2(npc, player);
                    break;
                case 10:
                    Tentacles2(npc, player);
                    break;
                default:
                    Vines(npc, player);
                    break;
            }
        }
        void SpitSeeds(NPC npc, Player player)
        {
            if (timer > 90)
            {

                if (timer > 90 + attackCounter * 20)
                {
                    attackCounter++;
                    float rot = (player.Center - npc.Center).ToRotation();
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + TRAEMethods.PolarVector(50, rot), TRAEMethods.PolarVector((player.Center - npc.Center).Length() / 120f, rot), ProjectileID.SeedPlantera, npc.GetAttackDamage_ForProjectiles(26f, 22f), 0);
                }
            }
        }
        Projectile thornBall = null;
        int thornBallStart = 60;
        int thornBallHold = 120;
        int thornBallRelease = 60;
        int thornBallEnd = 30;
        float spins = 4;
        void ThornBall(NPC npc, Player player)
        {

            float rot = (player.Center - npc.Center).ToRotation();
            float rotOffset = 2f * (float)Math.PI * ((float)(timer - thornBallStart) / (float)(thornBallHold / spins));
            float outAmount = (timer - thornBallStart) * 4f;
            if (outAmount > 80)
            {
                outAmount = 80;
            }

            if (thornBall == null && timer >= thornBallStart && timer < thornBallStart + thornBallHold)
            {
                thornBall = Main.projectile[Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.ThornBall, npc.GetAttackDamage_ForProjectiles(39f, 33f), 0)];
                thornBall.tileCollide = false;
            }
            if (thornBall != null)
            {
                if (timer < thornBallStart + thornBallHold)
                {
                    thornBall.Center = npc.Center + TRAEMethods.PolarVector(outAmount, rot + rotOffset);
                    thornBall.rotation = rot + rotOffset;
                }
                else if (timer < thornBallStart + thornBallHold + thornBallRelease)
                {

                    if (thornBall.localAI[0] == 0 && attackCounter % 2 == 0)
                    {
                        attackCounter++;
                        thornBall.velocity = TRAEMethods.PolarVector(10 * expertSpeedBonus, rot);
                    }
                    thornBall.tileCollide = true;
                }
                else if (timer < thornBallStart + thornBallHold + thornBallRelease + thornBallEnd)
                {
                    if (thornBall.localAI[0] == 1)
                    {

                        thornBall.tileCollide = false;
                        thornBall.velocity = (npc.Center - thornBall.Center).SafeNormalize(-Vector2.UnitY) * 16f;
                        if (Collision.CheckAABBvAABBCollision(thornBall.position, thornBall.Size, npc.position, npc.Size))
                        {
                            thornBall.Kill();
                            thornBall = null;
                        }
                    }
                    else
                    {
                        thornBall.ai[1] = 1;
                        thornBall.timeLeft = 480 + (int)(player.Center - thornBall.Center).Length();
                        // thornBall.velocity /= 2;
                        thornBall.extraUpdates = 1;
                        thornBall = null;
                    }
                }
                else
                {
                    thornBall.Kill();
                    thornBall = null;
                }
            }
        }
        int thornBall2Start = 60;
        int thornBall2Hold = 60;
        int thornBall2Release = 60;
        int thornBall2End = 30;
        float spins2 = 2;
        void ThornBall2(NPC npc, Player player)
        {

            float rot = (player.Center - npc.Center).ToRotation();
            float rotOffset = 2f * (float)Math.PI * ((float)(timer - thornBall2Start) / (float)(thornBall2Hold / spins2));
            float outAmount = (timer - thornBall2Start) * 4f;
            if (outAmount > 80)
            {
                outAmount = 80;
            }

            if (thornBall == null && timer >= thornBall2Start && timer < thornBall2Start + thornBall2Hold)
            {
                thornBall = Main.projectile[Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.ThornBall, npc.GetAttackDamage_ForProjectiles(39f, 33f), 0)];
                thornBall.tileCollide = false;
            }
            if (thornBall != null)
            {
                if (timer < thornBall2Start + thornBall2Hold)
                {
                    thornBall.Center = npc.Center + TRAEMethods.PolarVector(outAmount, rot + rotOffset);
                    thornBall.rotation = rot + rotOffset;
                }
                else if (timer < thornBall2Start + thornBall2Hold + thornBall2Release)
                {

                    if (thornBall.localAI[0] == 0 && attackCounter % 2 == 0)
                    {
                        attackCounter++;
                        thornBall.velocity = TRAEMethods.PolarVector(10 * expertSpeedBonus, rot);
                    }
                    thornBall.tileCollide = true;
                }
                else if (timer < thornBall2Start + thornBall2Hold + thornBall2Release + thornBall2End)
                {
                    if (thornBall.localAI[0] == 1)
                    {

                        thornBall.tileCollide = false;
                        thornBall.velocity = (npc.Center - thornBall.Center).SafeNormalize(-Vector2.UnitY) * 16f;
                        if (Collision.CheckAABBvAABBCollision(thornBall.position, thornBall.Size, npc.position, npc.Size))
                        {
                            thornBall.Kill();
                            thornBall = null;
                        }
                    }
                    else
                    {
                        thornBall.ai[1] = 1;
                        thornBall.timeLeft = 480 + (int)(player.Center - thornBall.Center).Length();
                        //thornBall.velocity /= 2;
                        thornBall.extraUpdates = 1;
                        thornBall = null;
                    }
                }
                else
                {
                    thornBall.Kill();
                    thornBall = null;
                }
            }
            if (attackCounter < 4 && timer > thornBall2Start + thornBall2Hold + thornBall2Release + thornBall2End)
            {
                attackCounter++;
                timer = 30;
            }
        }
        void Tentacles(NPC npc, Player player)
        {
            if (timer > 90)
            {
                if (timer > 90 + attackCounter * 24)
                {
                    attackCounter++;
                    Vector2 pos = npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height));
                    NPC tent = Main.npc[NPC.NewNPC(npc.GetSource_FromAI(), (int)pos.X, (int)pos.Y, NPCID.PlanterasTentacle)];
                    tent.localAI[0] = npc.whoAmI;
                    tent.localAI[1] = -1;
                }
            }
        }
        Projectile[] hooks = new Projectile[6];
        void Tentacles2(NPC npc, Player player)
        {
            if (attackCounter >= 0 && timer > 180 + attackCounter * 150)
            {
                attackCounter++;
                for (int i = 0; i < hooks.Length; i++)
                {
                    if (hooks[i] != null && hooks[i].active)
                    {
                        float rot = (player.Center - hooks[i].Center).ToRotation();
                        Projectile p = Main.projectile[Projectile.NewProjectile(npc.GetSource_FromAI(), hooks[i].Center, TRAEMethods.PolarVector((player.Center - hooks[i].Center).Length() / 120f, rot), ProjectileID.PoisonSeedPlantera, npc.GetAttackDamage_ForProjectiles(26f, 22f), 0)];
                        if (i == 0)
                        {
                            p.localAI[1] = 1;
                        }
                    }
                }

            }

            if (attackCounter == -1 && timer > 30)
            {
                attackCounter++;

                
                float length = 0;
                for (int i = 0; i < 8; i++)
                {
                    float rot = ((float)i / 8f) * ((float)Math.PI * 2f);
                    Vector2 pos = player.Center + TRAEMethods.PolarVector(50, rot);
                    for (int k = 0; k < 600; k++)
                    {
                        if (!Collision.CanHit(pos, 1, 1, player.Center, 1, 1))
                        {
                            break;
                        }
                        pos += TRAEMethods.PolarVector(10, rot);
                        length += 10;
                    }
                }
                int hookCount = 3 + (int)(length / (16f * 8 * 10));
                hooks = new Projectile[hookCount];
                float rotOffset = -(float)Math.PI/2f;
                if(hookCount % 2 == 0)
                {
                    rotOffset += ((float)Math.PI) / hookCount;
                }
                for (int i = 0; i < hooks.Length; i++)
                {
                    float rot = ((float)i / hooks.Length) * ((float)Math.PI * 2f) + rotOffset;
                    Vector2 pos = player.Center + TRAEMethods.PolarVector(50, rot);
                    for (int k = 0; k < 600; k++)
                    {
                        if (!Collision.CanHit(pos, 1, 1, player.Center, 1, 1))
                        {
                            break;
                        }
                        pos += TRAEMethods.PolarVector(10, rot);
                    }
                    hooks[i] = Main.projectile[Projectile.NewProjectile(npc.GetSource_FromAI(), pos, TRAEMethods.PolarVector(1f, rot + (float)Math.PI), ProjectileType<PlanteraHook2>(), npc.GetAttackDamage_ForProjectiles(26f, 22f), 0)];
                }
            }
        }
        void Vines(NPC npc, Player player)
        {
            if (timer > 40 && attackCounter < 1)
            {
                float rot = (player.Center - npc.Center).ToRotation();
                attackCounter++;
                for (int k = 0; k < 2; k++)
                {
                    Vector2 pos = npc.Center + TRAEMethods.PolarVector(260, rot + (float)Math.PI / 2f + k * (float)Math.PI);
                    for (int i = 0; i < 100; i++)
                    {
                        if (!Collision.CanHit(pos, 1, 1, player.Center, 1, 1))
                        {
                            break;
                        }
                        pos += TRAEMethods.PolarVector(-10, rot);
                    }
                    Projectile.NewProjectile(npc.GetSource_FromAI(), pos, TRAEMethods.PolarVector(8, rot), ProjectileType<PlanteraHook>(), npc.GetAttackDamage_ForProjectiles(39f, 33f), 0);
                }
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Plantera)
            {
                if (Main.expertMode && npc.life < npc.lifeMax)
                {
                    expertSpeedBonus = 2f - (float)(npc.life % (npc.lifeMax / 2)) / (float)(npc.lifeMax / 2);
                    if ((float)npc.life / (float)npc.lifeMax < 0.5f)
                    {
                        expertSpeedBonus += 0.2f;
                    }
                }
                npc.TargetClosest();
                Player player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.TargetClosest(false);
                    player = Main.player[npc.target];
                    if (!player.active || player.dead)
                    {
                        npc.velocity = new Vector2(0f, 10f);
                        if (npc.timeLeft > 10)
                        {
                            npc.timeLeft = 10;
                        }
                        return false;
                    }
                }
                if ((float)npc.life / (float)npc.lifeMax < 0.5f && currentAtk < 3)
                {

                    npc.rotation = (player.Center - npc.Center).ToRotation() + (float)Math.PI / 2;
                    ReverseMovement(npc, player);
                    if (thornBall != null)
                    {
                        thornBall.Kill();
                        thornBall = null;
                    }
                }
                else if (timer > pullBackTime)
                {
                    if ((float)npc.life / (float)npc.lifeMax < 0.5f)
                    {
                        Charge(npc, player);
                    }
                    else
                    {
                        npc.rotation = (player.Center - npc.Center).ToRotation() + (float)Math.PI / 2;
                        ReverseMovement(npc, player);
                    }
                }
                else
                {

                    npc.rotation = (player.Center - npc.Center).ToRotation() + (float)Math.PI / 2;
                    Movement(npc, player);
                }

                npc.defense = 36;
                if ((float)npc.life / (float)npc.lifeMax < 0.5f)
                {
                    npc.defense = 18;
                }
                float rot = (player.Center - npc.Center).ToRotation();
                if (!Collision.CanHit(npc.Center + TRAEMethods.PolarVector(-20, rot), 1, 1, player.Center, 1, 1))
                {
                    npc.defense += 60;
                }
                return false;
            }
            return base.PreAI(npc);
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (npc.type == NPCID.Plantera)
            {
                if (thornBall != null)
                {
                    Texture2D vine = TextureAssets.Chain26.Value;
                    float dist = (thornBall.Center - npc.Center).Length();
                    float rot = (npc.Center - thornBall.Center).ToRotation();
                    for (int k = 0; k < dist; k += vine.Height)
                    {
                        Vector2 pos = thornBall.Center + TRAEMethods.PolarVector(k, rot);
                        spriteBatch.Draw(vine, pos - screenPos, null, Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16), rot + (float)Math.PI / 2f, new Vector2(vine.Width / 2, vine.Height), 1f, SpriteEffects.None, 0);
                    }
                }
                if (currentAtk == 0 && timer > 0 && timer < 90)
                {
                    Texture2D pinkDraw = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Plantera/PinkDraw").Value;
                    Player player = Main.player[npc.target];
                    float rot = (player.Center - npc.Center).ToRotation();
                    Color color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
                    float distance = (player.Center - npc.Center).Length() * 1f;
                    if (distance > 500)
                    {
                        spriteBatch.Draw(pinkDraw, npc.Center - Main.screenPosition + TRAEMethods.PolarVector(distance / 2f, rot), null, color, rot, pinkDraw.Size() * .5f, new Vector2(distance / 4f, 1f), SpriteEffects.None, 0f);
                    }
                }
                if (currentAtk == 10)
                {

                    if (timer > (180 + attackCounter * 150) - 30 && attackCounter < 3)
                    {
                        Texture2D pinkDraw = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Plantera/PinkDraw").Value;
                        Player player = Main.player[npc.target];
                        for (int i = 0; i < hooks.Length; i++)
                        {
                            if (hooks[i] != null && hooks[i].active)
                            {

                                float rot = (player.Center - hooks[i].Center).ToRotation();
                                Color color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
                                float distance = (player.Center - hooks[i].Center).Length();
                                spriteBatch.Draw(pinkDraw, hooks[i].Center - Main.screenPosition + TRAEMethods.PolarVector(distance / 2f, rot), null, color, rot, pinkDraw.Size() * .5f, new Vector2(distance / 4f, 1f), SpriteEffects.None, 0f);
                                hooks[i].rotation = rot + (float)Math.PI / 2;
                            }
                        }
                    }
                }
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }

}
