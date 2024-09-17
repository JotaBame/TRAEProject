using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Banners;
using TRAEProject.NewContent.Items.Weapons.Magic.WillOfTheWisp;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.IO;
using TRAEProject.Changes.NPCs.Miniboss.Santa;

namespace TRAEProject.NewContent.NPCs.Underworld.Boomxie

{
    public class Boomxie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            // DisplayName.SetDefault("Boom Pixie"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 4; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 40;
            NPC.aiStyle = 22;
            AIType = NPCID.Pixie;
            AnimationType = NPCID.Pixie;
            NPC.damage = 50;
            NPC.defense = 10;
            NPC.lifeMax = 80;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.knockBackResist = 0.5f;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.OnFire3] = true;
            Banner = NPC.type;
            BannerItem = ItemType<BoomxieBanner>();
        }
        int spamTimer = 0;
        public override void AI()
        {
            bool flag15 = false;
 
            if (NPC.justHit)
            {
                NPC.ai[2] = 0f;
            }
   
            else if (NPC.ai[2] >= 0f)
            {
                int num286 = 16;
                bool flag17 = false;
                bool flag18 = false;
                if (NPC.position.X > NPC.ai[0] - (float)num286 && NPC.position.X < NPC.ai[0] + (float)num286)
                {
                    flag17 = true;
                }
                else if ((NPC.velocity.X < 0f && NPC.direction > 0) || (NPC.velocity.X > 0f && NPC.direction < 0))
                {
                    flag17 = true;
                }
                num286 += 24;
                if (NPC.position.Y > NPC.ai[1] - (float)num286 && NPC.position.Y < NPC.ai[1] + (float)num286)
                {
                    flag18 = true;
                }
                if (flag17 && flag18)
                {
                    NPC.ai[2] += 1f;
                    if (NPC.ai[2] >= 30f && num286 == 16)
                    {
                        flag15 = true;
                    }
                    if (NPC.ai[2] >= 60f)
                    {
                        NPC.ai[2] = -200f;
                        NPC.direction *= -1;
                        NPC.velocity.X *= -1f;
                        NPC.collideX = false;
                    }
                }
                else
                {
                    NPC.ai[0] = NPC.position.X;
                    NPC.ai[1] = NPC.position.Y;
                    NPC.ai[2] = 0f;
                }
                NPC.TargetClosest();
            }
            else
            {

                NPC.ai[2] += 1f;
                
                if (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) > NPC.position.X + (float)(NPC.width / 2))
                {
                    NPC.direction = -1;
                }
                else
                {
                    NPC.direction = 1;
                }
            }
            int num287 = (int)((NPC.position.X + (float)(NPC.width / 2)) / 16f) + NPC.direction * 2;
            int num288 = (int)((NPC.position.Y + (float)NPC.height) / 16f);
            bool flag19 = true;
            bool flag20 = false;
            int num289 = 3;
     
       
                num289 = 4;
                NPC.position += NPC.netOffset;
                if (Main.rand.Next(6) == 0)
                {
                    int num299 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 200, NPC.color);
                    Dust dust = Main.dust[num299];
                    dust.velocity *= 0.3f;
                }
                if (Main.rand.Next(40) == 0)
                {
                    SoundEngine.PlaySound(SoundID.Pixie, NPC.Center);
                }
                NPC.position -= NPC.netOffset;
            
             
            if (NPC.position.Y + (float)NPC.height > Main.player[NPC.target].position.Y)
            {
             
               
                    for (int num319 = num288; num319 < num288 + num289; num319++)
                    {
                        //if (Main.tile[num287, num319] == null)
                        //{
                        //    Main.tile[num287, num319] = new Tile();
                        //}
                        if ((Main.tile[num287, num319].HasUnactuatedTile && Main.tileSolid[Main.tile[num287, num319].TileType]) || Main.tile[num287, num319].LiquidType > 0)
                        {
                            if (num319 <= num288 + 1)
                            {
                                flag20 = true;
                            }
                            flag19 = false;
                            break;
                        }
                    }
                
            }
            if (Main.player[NPC.target].npcTypeNoAggro[NPC.type])
            {
                bool flag21 = false;
                for (int num320 = num288; num320 < num288 + num289 - 2; num320++)
                {
              
                    if ((Main.tile[num287, num320].HasUnactuatedTile && Main.tileSolid[Main.tile[num287, num320].TileType]) || Main.tile[num287, num320].LiquidType > 0)
                    {
                        flag21 = true;
                        break;
                    }
                }
                NPC.directionY = (!flag21).ToDirectionInt();
            }
   
            if (flag15)
            {
                flag20 = false;
                flag19 = true;
             
            }
            if (flag19)
            {
          
                
                    NPC.velocity.Y += 0.1f;
            
                    if (NPC.velocity.Y > 3f)
                    {
                        NPC.velocity.Y = 3f;
                    }
                
            }
            else
            {
               
                    if ((NPC.directionY < 0 && NPC.velocity.Y > 0f) || flag20)
                    {
                        NPC.velocity.Y -= 0.2f;
                    }
                
 
              
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
            }
            if (NPC.wet)
            {
                NPC.velocity.Y -= 0.2f;
                if (NPC.velocity.Y < -2f)
                {
                    NPC.velocity.Y = -2f;
                }
            }
            if (NPC.collideX)
            {
                NPC.velocity.X = NPC.oldVelocity.X * -0.4f;
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 1f)
                {
                    NPC.velocity.X = 1f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -1f)
                {
                    NPC.velocity.X = -1f;
                }
            }
            if (NPC.collideY)
            {
                NPC.velocity.Y = NPC.oldVelocity.Y * -0.25f;
                if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                {
                    NPC.velocity.Y = 1f;
                }
                if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                {
                    NPC.velocity.Y = -1f;
                }
            }
            float  
                maxSpeed = 3f;
            if (NPC.Distance(NPC.GetTargetData().Center) < 750)
            {
                maxSpeed *= -1;
            }


                if (NPC.direction == -1 && NPC.velocity.X > 0f - maxSpeed)
            {
                NPC.velocity.X -= 0.1f;
                if (NPC.velocity.X > maxSpeed)
                {
                    NPC.velocity.X -= 0.1f;
                }
                else if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X += 0.05f;
                }
                if (NPC.velocity.X < 0f - maxSpeed)
                {
                    NPC.velocity.X = 0f - maxSpeed;
                }
            }
            else if (NPC.direction == 1 && NPC.velocity.X < maxSpeed)
            {
                NPC.velocity.X += 0.1f;
                if (NPC.velocity.X < 0f - maxSpeed)
                {
                    NPC.velocity.X += 0.1f;
                }
                else if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X -= 0.05f;
                }
                if (NPC.velocity.X > maxSpeed)
                {
                    NPC.velocity.X = maxSpeed;
                }
            }
            maxSpeed = 1f; // this is for vertical speed
            if (NPC.directionY == -1 && NPC.velocity.Y > 0f - maxSpeed)
            {
                NPC.velocity.Y -= 0.04f;
                if (NPC.velocity.Y > maxSpeed)
                {
                    NPC.velocity.Y -= 0.05f;
                }
                else if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y += 0.03f;
                }
                if (NPC.velocity.Y < 0f - maxSpeed)
                {
                    NPC.velocity.Y = 0f - maxSpeed;
                }
            }
            else if (NPC.directionY == 1 && NPC.velocity.Y < maxSpeed)
            {
                NPC.velocity.Y += 0.04f;
                if (NPC.velocity.Y < 0f - maxSpeed)
                {
                    NPC.velocity.Y += 0.05f;
                }
                else if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y -= 0.03f;
                }
                if (NPC.velocity.Y > maxSpeed)
                {
                    NPC.velocity.Y = maxSpeed;
                }
            }
          
        
        NPC.TargetClosest(false);
            spamTimer++;
            if (NPC.Distance(NPC.GetTargetData().Center) < 450)
            {

                spamTimer += 3;
            }

            if (spamTimer > 360)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCType<LittleBoomxie>());
                spamTimer = 0;
            }
         
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("The adult stage of Lavaflies. They defend themselves by leaving volatile dust behind. ")
            }); 
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (spawnInfo.SpawnTileType != TileID.ObsidianBrick && spawnInfo.SpawnTileType != TileID.HellstoneBrick)
            {
                if (!NPC.downedPlantBoss)
                    return SpawnCondition.Underworld.Chance * 0.12f;
                else
                    return SpawnCondition.Underworld.Chance * 0.03f;
            }
            return 0f;

        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ExplosivePowder, 2, 1, 3));
            npcLoot.Add(ItemDropRule.Common(ItemType<WillOfTheWisp>(), 20));
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(-2, -2), Main.rand.NextFloat(2, 2));
                var dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 10, NPC.Center.Y - 10), 20, 20, DustID.Torch);
                dust.scale = 0.5f;
            }
        }
        public override void OnKill()
        {
            Vector2 zero = new Vector2(0, 0);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, zero, ProjectileType<Boom>(), 30, 0);
        }
    }
    public class LittleBoomxie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            }; NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // DisplayName.SetDefault("Boom Dust"); // Automatic from .lang files
            Main.npcFrameCount[NPC.type] = 1; // make sure to set this for your modnpcs.
        }
        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 14;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.lifeMax = 20;
            NPC.knockBackResist = 1f;
        }
        public override void UpdateLifeRegen(ref int damage)
        {
            NPC.lifeRegen -= 20;
            if (damage < 5)
            {
                damage = 5;
            }
            return;
        }
        public override void AI()
        {
   

            if (NPC.wet)
            {
                if (NPC.collideY)
                {
                    NPC.velocity.Y = -2f;
                }
                if (NPC.velocity.Y < 0f && NPC.ai[3] == NPC.position.X)
                {
                    NPC.direction *= -1;
                    NPC.ai[2] = 200f;
                }
                if (NPC.velocity.Y > 0f)
                {
                    NPC.ai[3] = NPC.position.X;
                }

                if (NPC.velocity.Y > 2f)
                {
                    NPC.velocity.Y *= 0.9f;
                }
                else if (NPC.directionY < 0)
                {
                    NPC.velocity.Y -= 0.8f;
                }
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -1f)
                {
                    NPC.velocity.Y = -1f;
                }

            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ExplosivePowder, 10));
        }
        public override void OnKill()
        {
            Vector2 zero = new Vector2(0, 0);
            if (!Main.expertMode && !Main.masterMode)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, zero, ProjectileType<Boom>(), 30, 0);
            }
            else
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.BurningSphere);
            }
        }
    }
    public class Boom : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 5;
        }
        public bool runonce = false; 
        public override void AI()
        {
            if (!runonce)
            {
                if (Projectile.ai[0] == 0)
                {
                    Projectile.ai[0] = 125;
                }
                runonce = true;
                TRAEMethods.Explode(Projectile, (int)(Projectile.ai[0]));
                TRAEMethods.DefaultExplosion(Projectile);
            }
        }  
    }
}