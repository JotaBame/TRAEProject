using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Utilities;
using TRAEProject.NewContent.Items.Armor.UnderworldWarrior;
using TRAEProject.NewContent.Items.BeholderItems;
using TRAEProject.NewContent.NPCs.Banners;
using TRAEProject.NewContent.NPCs.Underworld.OniRonin;
using TRAEProject.NewContent.Projectiles;
using static System.Formats.Asn1.AsnWriter;
using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace TRAEProject.NewContent.NPCs.Underworld.Beholder
{    [AutoloadBossHead]
    public class BeholderNPC : ModNPC
    {
        // Needs boss icon and music
        // Sound effects
        // Phase 3 scythe spam
        // Playtest
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            // DisplayName.SetDefault("Beholder");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 132;
            NPC.height = 102;
            NPC.value = 50000;
            NPC.damage = 100;
            NPC.defense = 66;
            NPC.lifeMax = 60000;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.scale = 1.2f; 
            NPC.HitSound = SoundID.NPCHit8;
            NPC.DeathSound = SoundID.NPCDeath10;
            Music = MusicID.Boss3;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note:bossAdjustment -> balance (bossAdjustment is different, see the docs for details) */
        {
            if (Main.expertMode)
            {
                NPC.lifeMax = (int)((NPC.lifeMax * 3 / 4) * bossAdjustment);
            }
            if (Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 3 / 4 * bossAdjustment); 
            }
            

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("Ancient Rulers of the Underworld sealed away by a powerful warrior. Released by the influx of souls into the world, they are ready to take the Underworld back for themselves.")
            });
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemType<GreaterRestorationPotion>();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            NPCLoader.blockLoot.Add(ItemID.LesserHealingPotion);

            //Add the treasure bag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ItemType<BeholderBag>())); //this requires you to set BossBag in SetDefaults accordingly
            //All our drops here are based on "not expert", meaning we dase .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());
            //Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<UnderworldWarriorHelmet>(), ItemType<UnderworldWarriorChestplate>(), ItemType<UnderworldWarriorGreaves>()));
            //Finally add the leading rule
            npcLoot.Add(notExpertRule);

            //Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ItemType<BeholderTrophyItem>(), 10));
            //Boss masks are spawned with 1/7 chance
            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<BeholderMask>(), 7));
            npcLoot.Add(notExpertRule);

            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<RingOfFuror>(), ItemType<RingOfTenacity>(), ItemType<RingOfMight>()));
            npcLoot.Add(notExpertRule);
           
            notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(4, ItemType<ScrollOfWipeout>(), ItemType<WandOfDisintegration>()));
            npcLoot.Add(notExpertRule);

            LeadingConditionRule MasterRule = new(new Conditions.IsMasterMode());
            MasterRule.OnSuccess(ItemDropRule.Common(ItemType<BeholderRelicItem>()));
            MasterRule.OnSuccess(ItemDropRule.Common(ItemType<EvilLookingEye>(), 4));
            npcLoot.Add(MasterRule);



        }
        void DoDeathray(Player target, Vector2 shootFrom)
        {
            facethisway = NPC.spriteDirection;
            beamDirection = (target.Center - shootFrom).ToRotation();
            SoundEngine.PlaySound(SoundID.Zombie104 with { MaxInstances = 0 }, NPC.position);
            float rotateDirection = (target.Center.Y > shootFrom.Y ? 1 : -1) * (target.Center.X > shootFrom.X ? 1 : -1); 
            float rotation = beamDirection + rotateDirection * beamSweepAngle / 2f;
            Projectile p = Main.projectile[Projectile.NewProjectile(new EntitySource_Misc(""), shootFrom, TRAEMethods.PolarVector(1f, rotation), ProjectileType<DeathgazeBeam>(), 80, 0, 0)];
            p.ai[0] = rotateDirection;
        }
        float beamDirection = 0;
        public const float beamSweepAngle = MathF.PI / 6;
        int facethisway = 0;
        float angletimer = 0;
        bool hasHeRoared = false;
        bool onSpawn = false;
        public override void AI()
        {
            NPC.takenDamageMultiplier = 1;       
            if (!onSpawn)
            {
                NPC.life += 6666;
                NPC.lifeMax += 6666;             
                onSpawn = true;
            }

            NPC.TargetClosest();
            NPC.FaceTarget();
            NPC.HitSound = SoundID.NPCHit8;

            Player target = Main.player[NPC.target];
            target.AddBuff(BuffID.Horrified, 1);
            target.blackout = true;
            float speed = 5f;
            float speedY = 2f;
            float accelerationX = 0.18f;
            float accelerationY = 0.11f;
            Vector2 ActualCenter = new Vector2(NPC.Center.X + 24 * NPC.direction, NPC.Center.Y + 14);
            float distance = NPC.Distance(target.Center);
            if (distance > 3000f)
                distance = 3000f;
            bool angryExpert = NPC.life <= (int)(NPC.lifeMax * 0.66) && Main.expertMode;

            bool belowQuarter = NPC.life <= NPC.lifeMax / 4;
            speed *= 1.5f * (distance / 300f);
            accelerationX *= 1.5f * distance / 300f;
            if (speed >= 15f)
                speed = 15f;
            if (distance > 3000f)
            {
                NPC.EncourageDespawn(1);
            }

            // movement
            NPC.velocity.X += accelerationX * NPC.direction;
            if (Math.Abs(NPC.velocity.X) >= speed)
            {
                NPC.velocity.X = speed * NPC.direction;
            }
            float TargetAboveOrBelow = target.Center.Y - ActualCenter.Y;
            if (TargetAboveOrBelow >= 200f) // speed up if you are too far vertically
            {
                accelerationY *= 1.6f * distance / 200f;

                speedY *= 1.6f * (distance / 200f);
            } 
            if (Math.Sign(TargetAboveOrBelow) == 1)
            {
                accelerationY /= 2;
            }
            NPC.velocity.Y += accelerationY * Math.Sign(TargetAboveOrBelow);
            if (Math.Abs(NPC.velocity.Y) >= speedY)
            {
                NPC.velocity.Y = speedY * Math.Sign(TargetAboveOrBelow);
            }

            if (Main.rand.Next(4) == 0)
            {

                int num117 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.PurpleTorch, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num117].noGravity = true;
                Main.dust[num117].velocity.X *= 1f;
                Main.dust[num117].velocity.Y *= 1f;
            }



            NPC.rotation += 0.005f * Math.Sign(TargetAboveOrBelow) * NPC.direction;
            if (Math.Abs(NPC.rotation) > 0.25f)
            {
                NPC.rotation = 0.25f * NPC.direction * Math.Sign(TargetAboveOrBelow);
            }

            int attackDelay = 160;

            if (angryExpert || belowQuarter)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Vector2 crybloodfrom = ActualCenter - Vector2.UnitY * (10 - 5 * NPC.rotation) + NPC.direction * Vector2.UnitX * 36;
                    int num117 = Dust.NewDust(crybloodfrom, 1, 1, DustID.Blood, NPC.velocity.X * 0.2f, 5, 0, default, 1.5f);
                }
             
            }
            if (angryExpert)
            {
                attackDelay = 90;
                if (!hasHeRoared)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath10 with { MaxInstances = 0 }, NPC.Center);
                    hasHeRoared = true;
                }
            }
            if (belowQuarter)
            {
                if (NPC.ai[1] != 10) // ROAR
                {
                    for (int i = 0; i < 20; i++)
                    {
                        float radius = 300 / 41.67f;
                        Vector2 vel = Main.rand.NextVector2CircularEdge(radius, radius);
                        Dust d = Dust.NewDustPerfect(NPC.Center, DustID.GreenTorch, vel * 2.5f, Scale: 3f);
                        if (Main.rand.NextBool(3))
                        {
                            d.scale *= Main.rand.NextFloat(1.25f, 1.5f);
                            d.velocity *= Main.rand.NextFloat(1.25f, 1.5f);
                        }
                        d.noGravity = true;
                    }
                    NPC.ai[0] = 0;
                    NPC.ai[2] = 0;
                    SoundEngine.PlaySound(SoundID.NPCDeath10 with { MaxInstances = 0 }, NPC.Center);
                }
                NPC.ai[1] = 10;

                if (Main.rand.NextBool(3))
                {
                    int num117 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 2f), NPC.width, NPC.height, DustID.GreenTorch, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, default, 2f);
                    Main.dust[num117].noGravity = true;
                    Main.dust[num117].velocity.X *= 1f;
                    Main.dust[num117].velocity.Y *= 1f;
                }
                attackDelay = 180;
                // shoot scythes when it's not charging up
                int ScytheToShoot = 0;
                int delay = 44;
                if (NPC.ai[0] % delay == 0)
                {
                    float VEL = 5f;
                    switch (NPC.ai[0] / delay)
                    {
                        case 1:
                            ScytheToShoot = ProjectileType<GreenScythe>();
                            
                            break;
                        case 2:
                            ScytheToShoot = ProjectileID.DemonSickle;
                            VEL = 1f;
                            break;
                        case 3:
                            ScytheToShoot = ProjectileType<YellowScythe>();
                            VEL = 3f;
                            break;
                        case 4:
                            SoundEngine.PlaySound(SoundID.NPCDeath10 with { MaxInstances = 0 }, NPC.Center); // make him roar that's actually cool
                            ScytheToShoot = ProjectileType<OrangeScythe>();
                            break;

                    }

                    SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, NPC.position);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    { 
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 direction = NPC.velocity.RotatedBy(360 - 45 * (i - 1)) * VEL;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction, ScytheToShoot, 40, 1f);
                    }
                    }
                }
      

            }
            if (NPC.ai[0] <= attackDelay && NPC.ai[2] == 0) // ai2 gets reset to 0 after every attack
                NPC.ai[0] += 1f;

            if (NPC.ai[0] == attackDelay)
            {
                if (!belowQuarter) // If it's on deathray spam phase, it must stay at 10
                   NPC.ai[1] += 1;
                NPC.ai[0] = attackDelay + 1;
            }


            if (NPC.ai[0] >= attackDelay + 1)
            {
                // Order:
                // 1- Green Scythes
                // 2- Purple scythes
                // 3- Orange scythes
                // 4- Deathgaze
                // 5- Green Scythes
                // 6- Color Shotgun
                // 7- Deathgaze
                // 8- Color Shotgun
                // 9- Green Scythes (Cycle is reset manually here, if you change the cycle remember to edit that)
                // 10- Deathgaze Spam
                ////////// green scythes
                if (NPC.ai[1] == 1 || NPC.ai[1] == 5 || NPC.ai[1] == 9)
                {
                    int delay = 30;
                    NPC.ai[2]++;
                    if (NPC.ai[2] % delay == 0)
                    {
                        
                        SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, NPC.position);
                        float velocityY = -30 + 15 * NPC.ai[2] / delay;
                        float velocityX = NPC.ai[2] == delay * 2 ? -15 * NPC.direction : 0;
                        float numberProjectiles = 3;
                        float rotation = MathHelper.ToRadians(30);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocityX, velocityY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 2f; // Watch out for dividing by 0 if there is only 1 projectile.
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), ActualCenter.X, ActualCenter.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<GreenScythe>(), 35, 0f);
                        }

                    }

                    if ((NPC.ai[2] == delay * 4 - delay / 3 || NPC.ai[2] == delay * 7 - delay / 6) && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float posX = target.Center.X;
                        float posY = target.Center.Y;

                        float VEL = 16f;
                        float velX = posX - ActualCenter.X;
                        float velY = posY - ActualCenter.Y;
                        float sqrRoot = MathF.Sqrt(velX * velX + velY * velY);
                        sqrRoot = VEL / sqrRoot;
                        velX *= sqrRoot;
                        velY *= sqrRoot;
                        int numberProjectiles = Main.rand.Next(9, 12);
                        for (int i = 0; i < numberProjectiles; i++)
                        {

                            Vector2 perturbedSpeed = new Vector2(velX, velY).RotatedByRandom(MathHelper.ToRadians(30));

                            perturbedSpeed *= 1f - Main.rand.NextFloat(0.2f);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), ActualCenter.X, ActualCenter.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileType<YellowScythe>(), 35, 0);


                        }
                        if (NPC.ai[1] == 9)
                        {
                            NPC.ai[1] = 0;
                        }
                        NPC.ai[0] = 0;
                        NPC.ai[2] = 0;
                    }
     
                }
                ////////// purple spiral
                if (NPC.ai[1] == 2)
                {
                    //if (NPC.ai[2] == 0)
                    //{
                    //    SoundEngine.PlaySound(SoundID. with { Pitch = -1f }, NPC.Center);
                    //}

                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    NPC.ai[2]++;
                    NPC.HitSound = SoundID.NPCHit1;
                    NPC.takenDamageMultiplier /= 4;
                    angletimer += 0.08f;
                    if (angletimer > 360)
                    {
                        angletimer = 0;
                    }
                    int delay = 6;
                    int scythes = 32;
                    if (NPC.ai[2] % delay == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, NPC.position);
                        Vector2 direction = Vector2.One.RotatedBy(angletimer);
                        Vector2 position = ActualCenter + 60 * direction;
                        float velocity = 0.25f;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), position, direction * velocity, ProjectileID.DemonSickle, 40, 0f);


                    }
                    if (NPC.ai[2] >= scythes * delay)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient && Main.expertMode)
                        {
                            for (int i = 0; i < Main.rand.Next(7, 9); i++)
                            {
                                Vector2 vector22 = new(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                vector22.Normalize();
                                vector22 *= Main.rand.Next(10, 101) * 0.2f;
                                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, vector22.X, vector22.Y, ProjectileType<GreenScythe>(), 40, 1f);
                            }
                        }
                        NPC.ai[0] = 0;
                        NPC.ai[2] = 0;

                    }
                }
                ////////// deathgaze
                if ((NPC.ai[1] == 4 || NPC.ai[1] == 7))
                {
                    NPC.rotation = 0;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;

                    if (NPC.ai[2] == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie99 with { MaxInstances = 0 }, NPC.position);
                       
                    }
                    NPC.ai[2] += 1f;
                    Vector2 shootFrom = ActualCenter - Vector2.UnitY * 10 + NPC.direction * Vector2.UnitX * 40;
                    if (NPC.ai[2] == 90)
                    {
                       DoDeathray(target, shootFrom);
                    }
                    if (NPC.ai[2] >= 90)
                    {
                        NPC.spriteDirection = facethisway ; // multiply it by -1 else it faces the wrong way, idk why
                    }
                    if (NPC.ai[2] < 90)
                    {
                        NPC.HitSound = SoundID.NPCHit1;
                        NPC.takenDamageMultiplier /= 4;
                        for (int i = 0; i < 2; i++)
                        {
                            float rot = MathF.PI * i + MathF.PI * (float)NPC.ai[2] / 10f;
                            Dust.NewDustPerfect(shootFrom + TRAEMethods.PolarVector(30, rot), DustID.PurpleTorch, TRAEMethods.PolarVector(-3f, rot));
                        }
                    }
                    if (NPC.ai[2] >= 180)
                    {

                        NPC.ai[0] = 0;
                        NPC.ai[2] = 0;
                    }

                }
                ////////// orange scythes
                if (NPC.ai[1] == 3)
                {
                    NPC.ai[2]++;
                    if (NPC.ai[2] % 35 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, NPC.position);

                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 direction = NPC.velocity.RotatedBy(360 - 45 * (i - 1)) * 10f;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction, ProjectileType<OrangeScythe>(), 40, 1f);
                        }
                    }
                    if (NPC.ai[2] >= 115 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ProjectileType<WhiteScythe>(), 0, 1f);

                        NPC.ai[0] = 0;
                        NPC.ai[2] = 0;

                    }
                }
                ////////// Color Shotgun
                if ((NPC.ai[1] == 6 || NPC.ai[1] == 8))
                {
                    NPC.ai[2]++;
                    int delay = 25;
                    int ScytheToShoot = 0;
                    if (delay <= delay * 4)
                    {
                        if (NPC.ai[2] % delay == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            float VEL = 25f;
                            int numProjectiles = 6;
                            switch (NPC.ai[2] / delay)
                            {
                                case 1:
                                    VEL = 40f;
                                    ScytheToShoot = ProjectileType<OrangeScythe>();                            
                                    break;
                                case 2:
                                    ScytheToShoot = ProjectileID.DemonSickle;
                                    VEL = 1f;
                                    break;
                                case 3:
                                    numProjectiles = 1;

                                    ScytheToShoot = ProjectileType<WhiteScythe>();

                                    break;
                                case 4:
                                    ScytheToShoot = ProjectileType<GreenScythe>();
                                    break;
                                case 5:
                                    VEL = 16f;
                                    ScytheToShoot = ProjectileType<YellowScythe>();
                                    break;

                            }
                            SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, NPC.position);
                            float posX = target.Center.X;
                            float posY = target.Center.Y;
                            float velX = posX - ActualCenter.X;
                            float velY = posY - ActualCenter.Y;
                            float sqrRoot = MathF.Sqrt(velX * velX + velY * velY);
                            sqrRoot = VEL / sqrRoot;

                            velX *= sqrRoot;
                            velY *= sqrRoot;

                          
                            for (int i = 0; i < numProjectiles; i++)
                            {

                                Vector2 perturbedSpeed = new Vector2(velX, velY).RotatedByRandom(MathHelper.ToRadians(40));

                                perturbedSpeed *= 1f - Main.rand.NextFloat(0.2f);

                                Projectile.NewProjectile(NPC.GetSource_FromThis(), ActualCenter.X, ActualCenter.Y, perturbedSpeed.X, perturbedSpeed.Y, ScytheToShoot, 35, 0);

                            }
   
                        }
                        if (NPC.ai[2] >= delay * 5)
                        {

                            NPC.ai[0] = 0;
                            NPC.ai[2] = 0;
                            NPC.netUpdate = true ;
                        }
                    }
                }
                ////////// angry
                if (NPC.ai[1] == 10)
                {


                    NPC.rotation = 0;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    NPC.ai[2] += 1f;

                    if (NPC.ai[2] == 0)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie99 with { MaxInstances = 0 }, NPC.position);

                    }

                    Vector2 shootFrom = ActualCenter - Vector2.UnitY * 10 + NPC.direction * Vector2.UnitX * 40;
                    if (NPC.ai[2] == 75)
                    {

                        DoDeathray(target, shootFrom);
                    }
                    if (NPC.ai[2] >= 75)
                    {
                        NPC.spriteDirection = facethisway; 
                    }
                    if (NPC.ai[2] < 75)
                    {
                        NPC.HitSound = SoundID.NPCHit1;
                        NPC.takenDamageMultiplier /= 4;
                        for (int i = 0; i < 2; i++)
                        {
                            float rot = MathF.PI * i + MathF.PI * (float)NPC.ai[2] / 10f;
                            Dust.NewDustPerfect(shootFrom + TRAEMethods.PolarVector(30, rot), DustID.PurpleTorch, TRAEMethods.PolarVector(-3f, rot));
                        }
                    }
                    if (NPC.ai[2] >= 165 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, ProjectileType<WhiteScythe>(), 0, 1f);

                        NPC.ai[0] = 0;
                        NPC.ai[2] = 0;
                    }

                }
            }


        }
        public override void OnKill()
        {
        //    NPCLoader.blockLoot.Add(ItemID.LesserHealingPotion);

        //    if (Main.netMode == 0)
        //    {
        //        Main.NewText("The Beholder has been defeated!", 175, 75);

        //    }
        //    else if (Main.netMode == 2)
        //    {
        //        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("The Beholder has been defeated!"), new Color(175, 75, 255));
        //    }
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BeholderGore1").Type, 1f);
            
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BeholderGore2").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BeholderGore3").Type, 1f);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("BeholderGore4").Type, 1f);
            return;

        }


        public override void FindFrame(int frameHeight)
        {

            if (NPC.direction < 0 && NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
            }
            if (NPC.direction > 0 && NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = 1;
            }
            NPC.frameCounter += 1f;
            if (NPC.frameCounter > 6.0)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y / frameHeight > 3)
            {
                NPC.frame.Y = 0;
            }
            if ((NPC.ai[1] == 2 || (NPC.ai[1] == 4 && NPC.ai[2] <= 90) || (NPC.ai[1] == 7 && NPC.ai[2] <= 90) || (NPC.ai[1] == 10 && NPC.ai[2] <= 75)) && NPC.ai[2] > 0)
            {
                NPC.frame.Y = 4 * frameHeight;
            }
            if (((NPC.ai[1] == 4 || NPC.ai[1] == 7) && NPC.ai[2] >= 90) || (NPC.ai[1] == 10 && NPC.ai[2] >= 75))
            {
                NPC.frame.Y = 5 * frameHeight;
            }

        }
    }
    public class GreenScythe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 100;
            Projectile.light = 0.2f;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 0.9f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D blackBall = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GlowBallTransparent").Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            Main.EntitySpriteDraw(blackBall, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Color.Black * 0.6f, Projectile.rotation, blackBall.Size() / 2, Projectile.scale * 0.07f, Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {

            Projectile.rotation += (float)Projectile.direction * 0.8f;

            if (Projectile.ai[0] < 75f && Projectile.ai[0] >= 0)
            {
                Projectile.ai[0] += 1f;
                Projectile.velocity *= 0.9f;
            }
            if (Projectile.ai[0] >= 75f)
            {
                Projectile.ai[0] = -1;
                for (int o = 0; o < 255; ++o)
                {
                    Player player = Main.player[o];
                    if (o >= 0 && player.active && !player.dead)
                    {
                        float posX = player.Center.X;
                        float posY = player.Center.Y;
                        float Distance = Math.Abs(Projectile.Center.X - posX) + Math.Abs(Projectile.Center.Y - posY);

                        if (Distance > 6000f)
                        {
                            Projectile.Kill();
                        }
                        float speed = 22f;
                        if (Distance > 2400f)
                        {
                            speed *= 3f;
                        }
                     
                        float velX = posX - Projectile.Center.X;
                        float velY = posY - Projectile.Center.Y;
                        float sqrRoot = MathF.Sqrt(velX * velX + velY * velY);
                        sqrRoot = speed / sqrRoot;
                        velX *= sqrRoot;
                        velY *= sqrRoot;
                        Projectile.velocity.X = velX;
                        Projectile.velocity.Y = velY;
                        return;
                    }

                }
            }
            for (int num176 = 0; num176 < 3; num176++)
            {
                int num177 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GreenTorch, 0f, 0f, 100);
                Main.dust[num177].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
            for (int num729 = 0; num729 < 30; num729++)
            {
                int num730 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GreenTorch, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.7f);
                Main.dust[num730].noGravity = true;
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GreenTorch, Projectile.velocity.X, Projectile.velocity.Y, 100);
            }
        }
    }
    public class YellowScythe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 100;
            Projectile.timeLeft = 90;
            Projectile.light = 0.2f;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 0.9f;
        }
        public override void AI()
        {

            Projectile.rotation += (float)Projectile.direction * 0.8f;

            if (Projectile.ai[0] < 40f)
            {
                Projectile.ai[0] += 1f;
                if (Main.rand.NextBool(4)) // some slight variance in their trajectories
                    Projectile.ai[0] += 1f;
            }
            if (Projectile.ai[0] >= 40f)
            {
                Projectile.velocity = Vector2.Zero;
            }
            for (int num176 = 0; num176 < 2; num176++)
            {
                int num177 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.YellowTorch, 0f, 0f, 100);
                Main.dust[num177].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D blackBall = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GlowBallTransparent").Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            Main.EntitySpriteDraw(blackBall, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Color.Black * 0.6f, Projectile.rotation, blackBall.Size() / 2, Projectile.scale * 0.07f, Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            return base.PreDraw(ref lightColor);
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
            for (int num729 = 0; num729 < 20; num729++)
            {
                int num730 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.YellowTorch, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.7f);
                Main.dust[num730].noGravity = true;
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.YellowTorch, Projectile.velocity.X, Projectile.velocity.Y, 100);
            }
        }
    }
    public class OrangeScythe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 100;
            Projectile.light = 0.2f;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 0.9f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D blackBall = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GlowBallTransparent").Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            Main.EntitySpriteDraw(blackBall, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Color.Black * 0.6f, Projectile.rotation, blackBall.Size() / 2, Projectile.scale * 0.07f, Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {

            Projectile.rotation += (float)Projectile.direction * 0.8f;

            if (Projectile.ai[0] <= 45f)
            {
                Projectile.velocity *= 0.9f;
 
                Projectile.ai[0]++;
            }
            if (Projectile.ai[0] > 45f)
            {
                Projectile.velocity *= 1.1f;
    
            }
            for (int num176 = 0; num176 <3; num176++)
            {
                int num177 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 100);
                Main.dust[num177].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
            for (int num729 = 0; num729 < 30; num729++)
            {
                int num730 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.OrangeTorch, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.7f);
                Main.dust[num730].noGravity = true;
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.OrangeTorch, Projectile.velocity.X, Projectile.velocity.Y, 100);
            }
        }
    }
    public class WhiteScythe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 100;
            Projectile.light = 0.2f;
            Projectile.timeLeft = 300;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 0.9f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D blackBall = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GlowBallTransparent").Value;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            Main.EntitySpriteDraw(blackBall, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Color.Black * 0.6f, Projectile.rotation, blackBall.Size() / 2, Projectile.scale * 0.07f, Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            return base.PreDraw(ref lightColor);
        }
        float speed = 10f;
        public override void AI()
        {
            speed *= 1 - 0.15f / 60;
            Projectile.rotation += (float)Projectile.direction * 0.8f;
            Projectile.damage = 0;
            Rectangle rectangle = Projectile.Hitbox;
            for (int index1 = 0; index1 < 255; index1++)
            {
                Player player = Main.player[index1];
                if (index1 >= 0 && player.active && !player.dead)
                {
                    Vector2 unitY = Projectile.DirectionTo(player.Center);
                    if (unitY.HasNaNs())
                        unitY = Vector2.UnitY;
                    Projectile.velocity = Vector2.Multiply(unitY, speed);

                    if (rectangle.Intersects(player.Hitbox))
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath6 with { MaxInstances = 0 }, Projectile.position);
                        if (Main.expertMode)
                        {
                            player.AddBuff(BuffID.WitheredWeapon, Main.rand.Next(7, 9) * 60); 
                            player.AddBuff(BuffID.WitheredArmor, Main.rand.Next(9, 11) * 60);
                        }
                        else
                        {
                            int debuff = Main.rand.NextBool(2) ? BuffID.WitheredArmor : BuffID.WitheredWeapon;
                            player.AddBuff(debuff, Main.rand.Next(17, 19) * 60);
                        }
                     
                        Projectile.Kill();
                    }
                 
                    return;
                }
            }

            
            for (int num176 = 0; num176 < 3; num176++)
            {
                int num177 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 100);
                Main.dust[num177].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);

            for (int num729 = 0; num729 < 30; num729++)
            {
                int num730 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WhiteTorch, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 1.7f);
                Main.dust[num730].noGravity = true;
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.WhiteTorch, Projectile.velocity.X, Projectile.velocity.Y, 100);
            }
        }
    }
    public class DeathgazeBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Deathgaze");
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.timeLeft = 90;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        bool runOnce = true;
        float length = 0;
        float beamWidth = 0;
        public override bool CanHitPlayer(Player target)
        {
            if (Projectile.timeLeft >= 85)
            {
                return false;
            }
            return true;
        }
        public override void AI()
        {
            if (runOnce)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity = Vector2.Zero;
                runOnce = false;
            }
            for (; length < 1500; length++)
            {
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + TRAEMethods.PolarVector(length, Projectile.rotation), 1, 1))
                {
                    break;
                }
            }
            if (Projectile.timeLeft > 80)
            {
                beamWidth = 90 - Projectile.timeLeft;
            }
            if (Projectile.timeLeft < 20)
            {
                beamWidth = Projectile.timeLeft;
            }
            if(Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
            }
            Projectile.rotation -= Projectile.ai[0] * BeholderNPC.beamSweepAngle / 60f;
        } 
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + TRAEMethods.PolarVector(length, Projectile.rotation), beamWidth, ref point);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 26, 22), Color.White, Projectile.rotation - MathF.PI / 2f, new Vector2(13, 11), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            float subLength = length - (11 + 22);
            int midBeamHieght = 30;
            Main.EntitySpriteDraw(texture, Projectile.Center + TRAEMethods.PolarVector(11, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 24, 26, midBeamHieght), Color.White, Projectile.rotation - MathF.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, subLength / (float)midBeamHieght), SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center + TRAEMethods.PolarVector(length - 22, Projectile.rotation) - Main.screenPosition, new Rectangle(0, 56, 26, 22), Color.White, Projectile.rotation - MathF.PI / 2f, new Vector2(13, 0), new Vector2(beamWidth / 10f, 1f), SpriteEffects.None, 0);
            return false;
        }
    }
    
}
