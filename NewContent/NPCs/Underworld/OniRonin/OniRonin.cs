using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.NPCs.Banners;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace TRAEProject.NewContent.NPCs.Underworld.OniRonin
{
    public partial class OniRoninNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            // DisplayName.SetDefault("Oni Ronin");
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.NoMultiplayerSmoothingByType[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 52;
            //NPC.aiStyle = 3;
            //AIType = NPCID.DesertBeast;
            //AnimationType = NPCID.WalkingAntlion;
            NPC.value = 5000;
            NPC.damage = 70;
            NPC.defense = 30;
            NPC.lifeMax = 5000;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit21;
            NPC.DeathSound = SoundID.NPCDeath24;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            //DrawOffsetY = -2;
            NPC.scale = 1f;
            Banner = NPC.type;
            NPC.GetGlobalNPC<UnderworldEnemies>().HellMinibossThatSpawnsInPairs = true;
            BannerItem = ItemType<OniRoninBanner>();
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Mysterious Demon that brings out the magical powers of world's flowers to attack its enemies.")
            });
        }
        
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Sake, 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ItemType<DriedRose>(), 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.ObsidianRose, 15));
            npcLoot.Add(ItemDropRule.Common(ItemID.HelFire, 10));
        }
        SmearTeleportEffect teleportEffect;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.FaceTarget();
            NPC.velocity.X *= 0.93f;
            if (Math.Abs(NPC.velocity.X) < 0.1f)
            {
                NPC.velocity.X = 0f;
            }
            if (NPC.ai[0] == 0f)
            {
                NPC.ai[0] = 200f;
            }
            int teleportCooldown = 200; 
            NPC.ai[0] += 1f;
            if (NPC.ai[0] > 1000 || (NPC.DistanceSQ(player.Center) >= 800f * 800f))
                teleportCooldown = 20;
            
            if (NPC.ai[2] != 0f && NPC.ai[3] != 0f)//rapid teleports phase
            {
                TRAEMethods.ServerClientCheck("tele");
                Vector2 tpDestination = new Vector2(NPC.ai[2] * 16 + 8f, NPC.ai[3] * 16 - NPC.height / 2);
                teleportEffect = new(NPC.Center, tpDestination, 20);
                
                NPC.position += NPC.netOffset;
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/OniTeleport") with { MaxInstances = 0 }, NPC.Center);                   
                float tpDirection = (tpDestination - NPC.Center).ToRotation();
                OniRoninExtraVisualMethods.OniDustEffects(NPC.Center, 2, tpDirection, 5);
                NPC.position -= NPC.netOffset;
                NPC.position.X = NPC.ai[2] * 16f - (NPC.width / 2) + 8f;
                NPC.position.Y = NPC.ai[3] * 16f - NPC.height;
                NPC.netOffset = Vector2.Zero;
                NPC.velocity = Vector2.Zero;
                NPC.ai[2] = 0f;
                NPC.ai[3] = 0f;
                SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, NPC.position);        
                OniRoninExtraVisualMethods.OniDustEffects(NPC.Center, 2, tpDirection , 5);

            }//rapid teleports phase

            if (NPC.ai[0] % teleportCooldown == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int targetTileX = (int)Main.player[NPC.target].Center.X / 16;
                int targetTileY = (int)Main.player[NPC.target].Center.Y / 16;
                Vector2 chosenTile = Vector2.Zero;
                for (int i = 0; i < 8; i++)
                {
                    if (AI_AttemptToFindTeleportSpot(ref chosenTile, targetTileX, targetTileY))
                    {
                        if (NPC.Center.DistanceSQ(chosenTile * 16) < 65536 && i < 7)//will try 8 times to get a teleport spot at least 16 tilesaway
                            continue;
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = chosenTile.X;
                        NPC.ai[3] = chosenTile.Y;
                        NPC.netUpdate = true;
                        break;
                    }
                }
            }
            
            
            
            if (NPC.ai[0] >= 200 && NPC.ai[0] <= 800)
            {
                NPC.ai[1] += 1f; 
                if (NPC.ai[1] > 35f && NPC.ai[1] % 25f == 0 && NPC.ai[1] <= 160)//CIRCLING PETALS//35 + 25 * 5
                {
                    SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, NPC.Center);
                    int petal = (Main.rand.NextBool(5) && Main.expertMode) ? ProjectileType<OniPetalCirclingFire>() : ProjectileType<OniPetalCircling>();
                    Vector2 projVel = player.Center.DirectionFrom(NPC.Center) * 0.1f;
                    Vector2 offsetToHand = new(NPC.spriteDirection * -14, -20);
                    OniRoninExtraVisualMethods.OniDustEffects(offsetToHand + NPC.Center, 0, NPC.spriteDirection, (NPC.ai[1] - 35) / 125);
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + offsetToHand, projVel, petal, 30, 0f);
                        NPC.netUpdate = true;
                    }
                }
            }//spawn circling petals
            else if (NPC.ai[0] >= 800 ) //petal rapid fire start
            {
                NPC.ai[1] += 1f;
                Vector2 offsetToHand = new(NPC.spriteDirection * 14,0);
                if(NPC.ai[1] > 35f)
                    OniRoninExtraVisualMethods.OniDustEffects(NPC.Center + offsetToHand, 1, (player.Center - NPC.Center).ToRotation(), (NPC.ai[1] - 35) / (1200 - 35));
                if (NPC.ai[1] > 35f && NPC.ai[1] % 8f == 0)//PETAL RAPID FIRE
                {
                    SoundEngine.PlaySound(SoundID.Item13 with { MaxInstances = 0 }, NPC.position);
                    float speed = Main.expertMode ? 10 : 7.5f;      
                    int petal = (Main.rand.NextBool(5) && Main.expertMode) ? ProjectileType<OniPetalSpiralingFire>() : ProjectileType<OniPetalSpiraling>();
                    Vector2 projVel = player.Center.DirectionFrom(NPC.Center) * speed;
                    //ufromse source  AI silly bame
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center , projVel, petal, 30, 0f, ai0: NPC.Center.X + offsetToHand.X, ai1: NPC.Center.Y + offsetToHand.Y - 4, ai2: Main.rand.NextFloat());
                    }
                }
                NPC.netUpdate = true;
            }//petal rapid fire end
            if (NPC.ai[0] > 1200 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.ai[0] = 0; 
                NPC.netUpdate = true;
            }
            if(teleportEffect.TimeLeft < -1)
            OniRoninExtraVisualMethods.OniDustEffects(NPC.Bottom, 3, NPC.spriteDirection, 0);
        }
        public bool AI_AttemptToFindTeleportSpot(ref Vector2 chosenTile, int targetTileX, int targetTileY, int rangeFromTargetTile = 20, int telefragPreventionDistanceInTiles = 5, int solidTileCheckFluff = 1, bool solidTileCheckCentered = false, bool teleportInAir = true)
        {
            int num = (int)NPC.Center.X / 16;
            int num2 = (int)NPC.Center.Y / 16;
            int num3 = 0;
            bool flag = false;
            float num4 = 20f;
            if (Math.Abs(num * 16 - targetTileX * 16) + Math.Abs(num2 * 16 - targetTileY * 16) > 2000)
            {
                num3 = 100;
                flag = false;
            }
            while (!flag && num3 < 100)
            {
                num3++;
                int num5 = Main.rand.Next(targetTileX - rangeFromTargetTile, targetTileX + rangeFromTargetTile + 1);
                for (int i = Main.rand.Next(targetTileY - rangeFromTargetTile, targetTileY + rangeFromTargetTile + 1); i < targetTileY + rangeFromTargetTile; i++)
                {
                    if ((i >= num2 - 1 && i <= num2 + 1 && num5 >= num - 1 && num5 <= num + 1) || (!teleportInAir && !Main.tile[num5, i].IsActuated))
                    {
                        continue;
                    }
                    bool flag2 = true;
                    if (Main.tile[num5, i - 1].LiquidType == 1)
                    {
                        flag2 = false;
                    }

                    if (!((!solidTileCheckCentered) ? (!Collision.SolidTiles(num5 - solidTileCheckFluff, num5 + solidTileCheckFluff, i - 3 - solidTileCheckFluff, i - 1)) : (!Collision.SolidTiles(num5 - solidTileCheckFluff, num5 + solidTileCheckFluff, i - solidTileCheckFluff, i + solidTileCheckFluff))))
                    {
                        continue;
                    }
                    Rectangle rectangle = new Rectangle(num5 * 16, i * 16, 16, 16);
                    rectangle.Inflate(telefragPreventionDistanceInTiles * 16, telefragPreventionDistanceInTiles * 16);
                    for (int j = 0; j < Main.player.Length; j++)
                    {
                        Player player = Main.player[j];
                        if (player != null && player.active && !player.DeadOrGhost)
                        {
                            Rectangle value = player.Hitbox;
                            Rectangle value2 = value.Modified((int)(player.velocity.X * num4), (int)(player.velocity.Y * num4), 0, 0);
                            Rectangle.Union(ref value2, ref value, out value2);
                            if (value2.Intersects(rectangle))
                            {
                                flag2 = false;
                                flag = false;
                                break;
                            }
                        }
                    }

                    if (flag2)
                    {

                        chosenTile = new Vector2(num5, i);

                        flag = true;

                    }
                    break;
                }

            }
            return flag;
        }
        public override void OnKill()
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("OniRoninGore1").Type, 1f);       
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("OniRoninGore2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("OniRoninGore3").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("OniRoninGore4").Type, 1f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return NPC.GetGlobalNPC<UnderworldEnemies>().MinibossSpawn(spawnInfo);

        }
    
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
            if (NPC.ai[1] > 35f)
            {
                if (NPC.ai[0] > 200 && NPC.ai[0] < 800)
                {
                    NPC.frame.Y = frameHeight * 1;
                }
                else if (NPC.ai[0] < 1000)
                {
                    NPC.frame.Y = frameHeight * 2;
                }
            }
        }
    }
    public class OniPetalCircling : ModProjectile
    {
        public override string Texture => "TRAEProject/NewContent/NPCs/Underworld/OniRonin/OniPetal";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Main.projFrames[Projectile.type] = 4;
            // DisplayName.SetDefault("Oni Petal");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false; 

        }
        int frame = Main.rand.Next(0, 4);
        float maxSpin = Main.rand.NextFloat(2.25f, 2.75f);

        public override bool PreDraw(ref Color lightColor)
        {
            OniRoninExtraVisualMethods.PetalDrawing(Projectile);
            return TRAEMethods.DrawSelfFullbright(Projectile);
        }
        public override void AI()
        {
            OniPetalCirclingFire.CirclingPetalAI(Projectile, frame);
        }
    }
    public class OniPetalSpiraling : ModProjectile
    {
        public override string Texture => "TRAEProject/NewContent/NPCs/Underworld/OniRonin/OniPetal";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            // DisplayName.SetDefault("Oni Petal");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false; 

        }
        int frame = Main.rand.Next(0, 4);
        public override void AI()
        {
            SpiralingPetalAI(Projectile, frame);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            OniRoninExtraVisualMethods.PetalDrawing(Projectile);
            return TRAEMethods.DrawSelfFullbright(Projectile);
        }
        public static void SpiralingPetalAI(Projectile projectile, int frame)
        {
            
            projectile.frame = (int)(projectile.localAI[0] % Main.projFrames[projectile.type]);
            Vector2 spawnLocation = new Vector2(projectile.ai[0], projectile.ai[1]);
            float timer = 300 - projectile.timeLeft;//timeleft is 300 is set defaults if it's changed this will break and the proj will go bckwrds
            float easingThing = MathHelper.Clamp(timer / 60, 0, 1);
            projectile.Center = spawnLocation + projectile.velocity * timer * easingThing + new Vector2(10 * easingThing).RotatedBy(timer * 0.3f + projectile.ai[2] * MathF.Tau);
            projectile.rotation = timer > 1 ? (projectile.position - projectile.oldPosition).ToRotation() + MathF.PI * 0.5f : projectile.rotation;
            projectile.tileCollide = false;
        }


    }
    public class OniPetalCirclingFire : ModProjectile
    {
        public override string Texture => "TRAEProject/NewContent/NPCs/Underworld/OniRonin/OniPetalFire";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Main.projFrames[Projectile.type] = 4;
            // DisplayName.SetDefault("Fire Petal");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false; 

        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, Main.rand.Next(300, 450));
        }
        int frame = Main.rand.Next(0, 4);
        public override void AI()
        {
            if (Main.rand.NextBool(8))
                Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f)].noGravity = true;
            CirclingPetalAI(Projectile, frame);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            OniRoninExtraVisualMethods.PetalDrawing(Projectile);
            return TRAEMethods.DrawSelfFullbright(Projectile);
        }
        public static void CirclingPetalAI(Projectile projectile, int frame)
        {
            projectile.frame = frame;
            if (projectile.ai[0] == 0 && projectile.ai[1] == 0)
            {
                projectile.ai[0] = projectile.Center.X;
                projectile.ai[1] = projectile.Center.Y;
            }
            if (projectile.ai[0] > 0 && projectile.ai[1] > 0)
            {
                int radius = 75;
                Vector2 originPos = new Vector2(projectile.ai[0], projectile.ai[1]);
                float deccelerationThing = projectile.localAI[0] > 0.8f ? 1 - MathHelper.Clamp((projectile.localAI[0] - 0.8f) * 5, 0, 1) : 1;
                //decceleration is now linear. let's give it an easing
                deccelerationThing = -(MathF.Cos(MathF.PI * deccelerationThing) - 1) * 0.5f;
                Vector2 targetPos = originPos + Vector2.One.RotatedBy((-projectile.localAI[1] - 0.3f) * MathF.Tau) * radius;
                projectile.localAI[0] += 1f / 125;//projectile firerate * amount of petals per circle
                projectile.localAI[1] += (1 / 125f) * deccelerationThing;
                float lerpValue = MathHelper.Clamp(projectile.localAI[0] * 4, 0, 1);
                projectile.Center = Vector2.SmoothStep(originPos, targetPos, lerpValue);
                float rotation = (projectile.position - projectile.oldPosition).ToRotation() + MathF.PI * 0.5f;//in a separate variable so that the loop doesn't mess with the lerp
                projectile.rotation = rotation;
                //maybe use this to give them a spawn delay
          
                if (projectile.localAI[0] >= 0.8f && projectile.ai[2] >= 0)
                {
                    for (int i = 0; i < Main.CurrentFrameFlags.ActivePlayersCount; ++i)
                    {
                        Player player = Main.player[i];
                        if (i >= 0 && player.active && !player.dead && player.DistanceSQ(projectile.Center) < 3200 * 3200)
                        {
                            projectile.rotation = Utils.AngleLerp((player.Center - projectile.Center).ToRotation() + MathF.PI * 0.5f, rotation, deccelerationThing);
                            if (projectile.localAI[0] >= 1)
                            {
                                float speed = 7.5f;
                                Vector2 projVel = player.Center.DirectionFrom(projectile.Center) * speed;
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, projVel, projectile.type == ModContent.ProjectileType<OniPetalCircling>() ? ModContent.ProjectileType<OniPetalSpiraling>() : ProjectileType<OniPetalSpiralingFire>(), 40, 0f, ai0: projectile.Center.X, ai1: projectile.Center.Y, ai2: Main.rand.NextFloat());
                                    proj.rotation = projectile.rotation;
                                    proj.localAI[0] = frame;
                                }
                                projectile.Kill();
                            }
                        }
                    }
                }

            }
        }
    }
    public class OniPetalSpiralingFire : ModProjectile
    {
        public override string Texture => "TRAEProject/NewContent/NPCs/Underworld/OniRonin/OniPetalFire";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            Main.projFrames[Projectile.type] = 4;
            // DisplayName.SetDefault("Fire Petal");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }
        int frame = Main.rand.Next(0, 4);
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, Main.rand.Next(300, 450));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            OniRoninExtraVisualMethods.PetalDrawing(Projectile);
            return TRAEMethods.DrawSelfFullbright(Projectile);
        }
        public override void AI()
        {
            if (Main.rand.NextBool(8))
                Main.dust[Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f)].noGravity = true;
            OniPetalSpiraling.SpiralingPetalAI(Projectile, frame);
        }
    }
}