using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.LunaticCultist
{
	public class Cultist : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            if (GetInstance<BossConfig>().CultistChanges)
            {
                if (npc.type == NPCID.CultistBoss)
                {
                    npc.lifeMax = (int)(npc.lifeMax * ((float)80000 / 32000));

                    npc.height = 84;
                    npc.width = 84;
                }
                if (npc.type == NPCID.CultistBossClone)
                {
                    npc.height = 84;
                    npc.width = 84;
                }
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (npc.type == NPCID.CultistBoss && GetInstance<BossConfig>().CultistChanges)
            {
                npc.lifeMax = (int)(npc.lifeMax / 1.2f);
            }
        }
        static float AttackSpeed(NPC npc)
        {
            if (npc.type == NPCID.CultistBossClone)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC cultist = Main.npc[i];
                    if (cultist.type ==  NPCID.CultistBoss)
                    {
                        if (((float)cultist.life / (float)cultist.lifeMax) <= 0.1f)
                            return 2.25f;
                        if (Main.expertMode || Main.masterMode)
                            return 2.25f - 1f * (float)(cultist.life / (float)cultist.lifeMax);
                        else
                            return 2.25f - 1.25f * ((float)cultist.life / (float)cultist.lifeMax);
                    }
                }
            }
   

            if (((float)npc.life / (float)npc.lifeMax) <= 0.1f)
                return 2.25f;
            if (Main.expertMode || Main.masterMode)
                return 2.25f - 1f * ((float)npc.life / (float)npc.lifeMax);
            else
                return 2.25f - 1.25f * ((float)npc.life / (float)npc.lifeMax);
        }
        int attackNumber = 0;

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.type == NPCID.CultistBoss && GetInstance<BossConfig>().CultistChanges)
            {
                if(ProjectileID.Sets.CultistIsResistantTo[projectile.type])
                {
                    modifiers.FinalDamage /= 0.75f;

                }
            }
        }
        public override bool PreAI(NPC npc)
		{
            if (GetInstance<BossConfig>().CultistChanges)
            {
                if (npc.type == NPCID.CultistBoss)
                {
                    if (npc.ai[0] != -1f && Main.rand.NextBool(1000))
                    {
                        int Sound = Main.rand.Next(4);
                        SoundStyle sound = SoundID.Zombie88;

                        switch (Sound)
                        {
                            case 1:
                                sound = SoundID.Zombie89;
                                break;
                            case 2:
                                sound = SoundID.Zombie90;
                                break;
                            case 3:
                                sound = SoundID.Zombie91;
                                break;
                        }
                        SoundEngine.PlaySound(sound with { MaxInstances = 0 }, npc.Center);
                    }
                    bool master = Main.masterMode;
                    bool belowHalf = npc.life <= npc.lifeMax / 2;
                    npc.damage = 0;

                    bool isCultist = npc.type == NPCID.CultistBoss;
                    bool flag2 = false;
                    bool flag3 = false;
                    if (isCultist)
                    {
                        //Main.NewText(npc.ai[0] + ", " + npc.ai[1] + ", " + npc.ai[2] + ", " + npc.ai[3]);
                        //Main.NewText(attackNumber);
                    }
                    if (!isCultist)
                    {
                        if (npc.ai[3] < 0f || !Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != 439)
                        {
                            npc.life = 0;
                            npc.HitEffect();
                            npc.active = false;
                            return false;
                        }
                        npc.ai[0] = Main.npc[(int)npc.ai[3]].ai[0];
                        npc.ai[1] = Main.npc[(int)npc.ai[3]].ai[1];
                        if (npc.ai[0] == 5f)
                        {
                            if (npc.justHit)
                            {
                                npc.life = 0;
                                npc.HitEffect();
                                npc.active = false;
                                if (Main.netMode != 1)
                                {
                                    NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                                }
                                NPC obj = Main.npc[(int)npc.ai[3]];
                                obj.ai[0] = 6f;
                                obj.ai[1] = 0f;
                                obj.netUpdate = true;
                            }
                        }
                        else
                        {
                            flag2 = true;
                            flag3 = true;
                        }
                    }
                    else if (npc.ai[0] == 5f && npc.ai[1] >= 120f && npc.ai[1] < 420f && npc.justHit)
                    {
                        //Kill Clones
                        EndAttack(npc);
                        List<int> list = new List<int>();
                        for (int i = 0; i < 200; i++)
                        {
                            if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone && Main.npc[i].ai[3] == npc.whoAmI)
                            {
                                list.Add(i);
                            }
                        }
                        int num10 = 10;
                        if (Main.expertMode)
                        {
                            num10 = 3;
                        }
                        foreach (int item in list)
                        {
                            NPC nPC = Main.npc[item];
                            if (nPC.localAI[1] == npc.localAI[1] && num10 > 0)
                            {
                                num10--;
                                nPC.life = 0;
                                nPC.HitEffect();
                                nPC.active = false;
                                if (Main.netMode != 1)
                                {
                                    NetMessage.SendData(23, -1, -1, null, item);
                                }
                            }
                            else if (num10 > 0)
                            {
                                num10--;
                                nPC.life = 0;
                                nPC.HitEffect();
                                nPC.active = false;
                            }
                        }
                        Main.projectile[(int)npc.ai[2]].ai[1] = -1f;
                        Main.projectile[(int)npc.ai[2]].netUpdate = true;
                    }
                    Vector2 center = npc.Center;
                    Player player = Main.player[npc.target];
                    CheckDeadPlayer(npc, center, ref player);
                    float attackTally = npc.ai[3];
                    if (npc.localAI[0] == 0f)
                    {
                        SoundEngine.PlaySound(SoundID.Zombie89 with { MaxInstances = 0 }, npc.Center);
                        npc.localAI[0] = 1f;
                        npc.alpha = 255;
                        npc.rotation = 0f;
                        if (Main.netMode != 1)
                        {
                            npc.ai[0] = -1f;
                            npc.netUpdate = true;
                        }
                    }
                    if (npc.ai[0] == -1f)
                    {
                        Opening(npc, out flag2, out flag3);
                    }
                    if (npc.ai[0] == 0f)
                    {
                        if (npc.ai[1] == 0f)
                        {
                            npc.TargetClosest(faceTarget: false);
                        }
                        npc.localAI[2] = 10f;
                        int num13 = Math.Sign(player.Center.X - center.X);
                        if (num13 != 0)
                        {
                            npc.direction = (npc.spriteDirection = num13);
                        }
                        npc.ai[1] += AttackSpeed(npc);
                        if (npc.ai[1] >= 40f && isCultist)
                        {
                            attackNumber = 0;
                            SelectAction(npc, master, belowHalf, center, player);
                        }
                    }
                    else if (npc.ai[0] == 1f)
                    {
                        flag2 = true;
                        npc.localAI[2] = 10f;
                        if ((float)(int)npc.ai[1] % 2f != 0f && npc.ai[1] != 1f)
                        {
                            npc.position -= npc.velocity;
                        }
                        npc.ai[1] -= 1f;
                        if (npc.ai[1] <= 0f)
                        {
                            EndAttack(npc);
                        }
                    }
                    else if (npc.ai[0] == 2f)
                    {

                        int iceCount = 1;
                        int iceAttackDelay = 120;
                        int iceDamage = npc.GetAttackDamage_ForProjectiles(35f, 25f);
                        if (Main.expertMode)
                        {
                            iceAttackDelay = 90;
                        }
                        npc.localAI[2] = 11f;
                        Vector2 vec = Vector2.Normalize(player.Center - center);
                        if (vec.HasNaNs())
                        {
                            vec = new Vector2(npc.direction, 0f);
                        }
                        if (npc.ai[1] >= 4f && isCultist && (int)(npc.ai[1] - 4f) >= iceAttackDelay * (int)attackNumber)
                        {
                            attackNumber++;
                            if (Main.netMode != 1)
                            {
                                IllusionsAttack(npc, player);
                                vec = Vector2.Normalize(player.Center - center + player.velocity * 20f);
                                if (vec.HasNaNs())
                                {
                                    vec = new Vector2(npc.direction, 0f);
                                }
                                Vector2 vector2 = npc.Center + new Vector2(npc.direction * 30, 12f);
                                for (int n = 0; n < 1; n++)
                                {
                                    Vector2 vector3 = vec * 4f;
                                    Projectile.NewProjectile(npc.GetSource_FromThis(), vector2.X, vector2.Y, vector3.X, vector3.Y, 464, iceDamage, 0f, Main.myPlayer, 0f, 1f);
                                }
                            }
                        }
                        npc.ai[1] += AttackSpeed(npc);
                        if (npc.ai[1] >= (float)(4 + iceAttackDelay * iceCount))
                        {
                            EndAttack(npc);
                        }
                    }
                    else if (npc.ai[0] == 3f)
                    {
                        //fireball attack


                        int fireballAttackDelay = 10;
                        int fireballCount = 5;
                        int fireballDamage = npc.GetAttackDamage_ForProjectiles(50f, 40f);
                        if (Main.expertMode)
                        {
                            fireballCount = 7;
                        }
                        if (master)
                            fireballAttackDelay = 8;
                        npc.localAI[2] = 11f;
                        Vector2 vec2 = Vector2.Normalize(player.Center - center);
                        if (vec2.HasNaNs())
                        {
                            vec2 = new Vector2(npc.direction, 0f);
                        }
                        if (npc.ai[1] >= 90f && isCultist && (int)(npc.ai[1] - 90f) >= fireballAttackDelay * (int)attackNumber)
                        {
                            attackNumber++;
                            if ((int)(npc.ai[1] - 4f) / fireballAttackDelay == 2)
                            {

                                IllusionsAttack(npc, player);
                            }
                            int num22 = Math.Sign(player.Center.X - center.X);
                            if (num22 != 0)
                            {
                                npc.direction = (npc.spriteDirection = num22);
                            }
                            if (Main.netMode != 1)
                            {
                                vec2 = Vector2.Normalize(player.Center - center + player.velocity * 20f);
                                if (vec2.HasNaNs())
                                {
                                    vec2 = new Vector2(npc.direction, 0f);
                                }
                                Vector2 vector5 = npc.Center + new Vector2(npc.direction * 30, 12f);
                                for (int num23 = 0; num23 < 1; num23++)
                                {
                                    Vector2 spinninpoint3 = vec2 * (4.5f + (float)Main.rand.NextDouble() * 4f);
                                    spinninpoint3 = spinninpoint3.RotatedByRandom(0.52359879016876221);
                                    Projectile.NewProjectile(npc.GetSource_FromThis(), vector5.X, vector5.Y, spinninpoint3.X, spinninpoint3.Y, 467, fireballDamage, 0f, Main.myPlayer);
                                }
                            }
                        }
                        if (npc.ai[1] < 90f)
                        {
                            if (npc.ai[1] == 0f)
                            {
                                SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 0 }, npc.Center);
                            }
                            npc.ai[1] += 1f;

                        }
                        else
                        {
                            npc.ai[1] += AttackSpeed(npc);
                        }

                        if (npc.ai[1] >= (float)(96 + fireballAttackDelay * fireballCount))
                        {
                            EndAttack(npc);
                        }
                    }
                    else if (npc.ai[0] == 4f)
                    {
                        //lightning

                        int lightningAttackDelay = 80;
                        int lightningDamage = npc.GetAttackDamage_ForProjectiles(45f, 30f);
                        if (master)
                        {
                            lightningAttackDelay = 40;
                        }
                        if (isCultist)
                        {
                            npc.localAI[2] = 12f;
                        }
                        else
                        {
                            npc.localAI[2] = 11f;
                        }
                        if (isCultist && Main.netMode != 1)
                        {
                            if ((int)(npc.ai[1] - 20f) >= lightningAttackDelay * (int)attackNumber)
                            {
                                IllusionsAttack(npc, player);
                                attackNumber++;
                                Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center.X, npc.Center.Y - 100f, 0f, 0f, 465, lightningDamage, 0f, Main.myPlayer);
                            }
                        }
                        npc.ai[1] += AttackSpeed(npc);
                        if (npc.ai[1] >= (float)(20 + lightningAttackDelay))
                        {
                            EndAttack(npc);
                        }
                    }
                    else if (npc.ai[0] == 5f)
                    {
                        //ritual
                        npc.localAI[2] = 10f;
                        if (Vector2.Normalize(player.Center - center).HasNaNs())
                        {
                            new Vector2(npc.direction, 0f);
                        }
                        if (npc.ai[1] >= 0f && npc.ai[1] < 30f)
                        {
                            flag2 = true;
                            flag3 = true;
                            float num27 = (npc.ai[1] - 0f) / 30f;
                            npc.alpha = (int)(num27 * 255f);
                        }
                        else if (npc.ai[1] >= 30f && npc.ai[1] < 90f)
                        {
                            if (npc.ai[1] >= 30f && attackNumber < 1 && Main.netMode != 1 && isCultist)
                            {
                                attackNumber++;
                                npc.localAI[1] += 1f;
                                Vector2 spinningpoint = new Vector2(180f, 0f);
                                List<int> list6 = new List<int>();
                                for (int num28 = 0; num28 < 200; num28++)
                                {
                                    if (Main.npc[num28].active && Main.npc[num28].type == NPCID.CultistBossClone && Main.npc[num28].ai[3] == (float)npc.whoAmI)
                                    {
                                        list6.Add(num28);
                                    }
                                }
                                int num29 = 6 - list6.Count;
                                if (num29 > 2)
                                {
                                    num29 = 2;
                                }
                                int num30 = list6.Count + num29 + 1;
                                float[] array2 = new float[num30];
                                for (int num31 = 0; num31 < array2.Length; num31++)
                                {
                                    array2[num31] = Vector2.Distance(npc.Center + spinningpoint.RotatedBy((float)num31 * (MathF.PI * 2f) / (float)num30 - MathF.PI / 2f), player.Center);
                                }
                                int num32 = 0;
                                for (int num33 = 1; num33 < array2.Length; num33++)
                                {
                                    if (array2[num32] > array2[num33])
                                    {
                                        num32 = num33;
                                    }
                                }
                                num32 = ((num32 >= num30 / 2) ? (num32 - num30 / 2) : (num32 + num30 / 2));
                                int num34 = num29;
                                for (int num35 = 0; num35 < array2.Length; num35++)
                                {
                                    if (num32 != num35)
                                    {
                                        Vector2 center6 = npc.Center + spinningpoint.RotatedBy((float)num35 * (MathF.PI * 2f) / (float)num30 - MathF.PI / 2f);
                                        if (num34-- > 0)
                                        {
                                            int num36 = NPC.NewNPC(npc.GetSource_FromAI(), (int)center6.X, (int)center6.Y + npc.height / 2, 440, npc.whoAmI);
                                            Main.npc[num36].ai[3] = npc.whoAmI;
                                            Main.npc[num36].netUpdate = true;
                                            Main.npc[num36].localAI[1] = npc.localAI[1];
                                        }
                                        else
                                        {
                                            int num37 = list6[-num34 - 1];
                                            Main.npc[num37].Center = center6;
                                            NetMessage.SendData(23, -1, -1, null, num37);
                                        }
                                    }
                                }
                                npc.ai[2] = Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center.X, npc.Center.Y, 0f, 0f, 490, 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                                npc.Center += spinningpoint.RotatedBy((float)num32 * (MathF.PI * 2f) / (float)num30 - MathF.PI / 2f);
                                npc.netUpdate = true;
                                list6.Clear();
                            }
                            flag2 = true;
                            flag3 = true;
                            npc.alpha = 255;
                            if (isCultist)
                            {
                                Vector2 value3 = Main.projectile[(int)npc.ai[2]].Center;
                                value3 -= npc.Center;
                                if (value3 == Vector2.Zero)
                                {
                                    value3 = -Vector2.UnitY;
                                }
                                value3.Normalize();
                                if (Math.Abs(value3.Y) < 0.77f)
                                {
                                    npc.localAI[2] = 11f;
                                }
                                else if (value3.Y < 0f)
                                {
                                    npc.localAI[2] = 12f;
                                }
                                else
                                {
                                    npc.localAI[2] = 10f;
                                }
                                int num38 = Math.Sign(value3.X);
                                if (num38 != 0)
                                {
                                    npc.direction = (npc.spriteDirection = num38);
                                }
                            }
                            else
                            {
                                Vector2 value4 = Main.projectile[(int)Main.npc[(int)npc.ai[3]].ai[2]].Center;
                                value4 -= npc.Center;
                                if (value4 == Vector2.Zero)
                                {
                                    value4 = -Vector2.UnitY;
                                }
                                value4.Normalize();
                                if (Math.Abs(value4.Y) < 0.77f)
                                {
                                    npc.localAI[2] = 11f;
                                }
                                else if (value4.Y < 0f)
                                {
                                    npc.localAI[2] = 12f;
                                }
                                else
                                {
                                    npc.localAI[2] = 10f;
                                }
                                int num39 = Math.Sign(value4.X);
                                if (num39 != 0)
                                {
                                    npc.direction = (npc.spriteDirection = num39);
                                }
                            }
                        }
                        else if (npc.ai[1] >= 90f && npc.ai[1] < 120f)
                        {
                            flag2 = true;
                            flag3 = true;
                            float num40 = (npc.ai[1] - 90f) / 30f;
                            npc.alpha = 255 - (int)(num40 * 255f);
                        }
                        else if (npc.ai[1] >= 120f && npc.ai[1] < 420f)
                        {
                            flag3 = true;
                            npc.alpha = 0;
                            if (isCultist)
                            {
                                Vector2 value5 = Main.projectile[(int)npc.ai[2]].Center;
                                value5 -= npc.Center;
                                if (value5 == Vector2.Zero)
                                {
                                    value5 = -Vector2.UnitY;
                                }
                                value5.Normalize();
                                if (Math.Abs(value5.Y) < 0.77f)
                                {
                                    npc.localAI[2] = 11f;
                                }
                                else if (value5.Y < 0f)
                                {
                                    npc.localAI[2] = 12f;
                                }
                                else
                                {
                                    npc.localAI[2] = 10f;
                                }
                                int num41 = Math.Sign(value5.X);
                                if (num41 != 0)
                                {
                                    npc.direction = (npc.spriteDirection = num41);
                                }
                            }
                            else
                            {
                                Vector2 value6 = Main.projectile[(int)Main.npc[(int)npc.ai[3]].ai[2]].Center;
                                value6 -= npc.Center;
                                if (value6 == Vector2.Zero)
                                {
                                    value6 = -Vector2.UnitY;
                                }
                                value6.Normalize();
                                if (Math.Abs(value6.Y) < 0.77f)
                                {
                                    npc.localAI[2] = 11f;
                                }
                                else if (value6.Y < 0f)
                                {
                                    npc.localAI[2] = 12f;
                                }
                                else
                                {
                                    npc.localAI[2] = 10f;
                                }
                                int num42 = Math.Sign(value6.X);
                                if (num42 != 0)
                                {
                                    npc.direction = (npc.spriteDirection = num42);
                                }
                            }
                        }
                        npc.ai[1] += AttackSpeed(npc);
                        if (npc.ai[1] >= 420f)
                        {
                            flag3 = true;
                            npc.ai[0] = 0f;
                            npc.ai[1] = 0f;
                            npc.ai[3] += 1f;
                            npc.velocity = Vector2.Zero;
                            npc.netUpdate = true;
                        }
                    }
                    else if (npc.ai[0] == 6f)
                    {
                        //Laugh at your failure
                        npc.localAI[2] = 13f;
                        npc.ai[1] += AttackSpeed(npc);
                        if (npc.ai[1] >= 120f)
                        {
                            EndAttack(npc);
                        }
                    }
                    else if (npc.ai[0] == 7f)
                    {
                        //Ancient Light

                        int ancientLightAttackDelay = 20;
                        int ancientLightSalvos = 2;
                        if (master)
                        {
                            ancientLightAttackDelay = 30;
                            ancientLightSalvos = 2;
                        }

                        npc.localAI[2] = 11f;
                        Vector2 vec4 = Vector2.Normalize(player.Center - center);
                        if (vec4.HasNaNs())
                        {
                            vec4 = new Vector2(npc.direction, 0f);
                        }
                        if (npc.ai[1] >= 4f && isCultist && (int)(npc.ai[1] - 4f) >= ancientLightAttackDelay * (int)attackNumber)
                        {
                            attackNumber++;
                            if ((int)(npc.ai[1] - 4f) / ancientLightAttackDelay == 2)
                            {
                                IllusionsAttack(npc, player);
                            }
                            int num46 = Math.Sign(player.Center.X - center.X);
                            if (num46 != 0)
                            {
                                npc.direction = (npc.spriteDirection = num46);
                            }
                            if (Main.netMode != 1)
                            {
                                vec4 = Vector2.Normalize(player.Center - center);
                                if (vec4.HasNaNs())
                                {
                                    vec4 = new Vector2(npc.direction, 0f);
                                }
                                Vector2 vector8 = npc.Center + new Vector2(npc.direction * 30, 12f);
                                float scaleFactor = 8f;
                                for (int num48 = 0; (float)num48 < 5f; num48++)
                                {
                                    Vector2 spinningpoint2 = vec4 * scaleFactor;
                                    spinningpoint2 = spinningpoint2.RotatedBy((num48 / 5f) * 2f * MathF.PI + MathF.PI / 5f);
                                    float ai = (Main.rand.NextFloat() - 0.5f) * 0.3f * (MathF.PI * 2f) / 60f;
                                    int num49 = NPC.NewNPC(npc.GetSource_FromAI(), (int)vector8.X, (int)vector8.Y + 7, 522, 0, 0f, ai, spinningpoint2.X, spinningpoint2.Y);
                                    Main.npc[num49].velocity = spinningpoint2;
                                }
                            }
                        }
                        npc.ai[1] += AttackSpeed(npc);
                        if (npc.ai[1] >= (float)(4 + ancientLightAttackDelay * ancientLightSalvos))
                        {
                            EndAttack(npc);
                        }
                    }
                    else if (npc.ai[0] == 8f)
                    {
                        //Ancient Doom

                        int ancientDoomAttackDelay = 20;
                        int ancientDoomCount = 3;
                        npc.localAI[2] = 13f;
                        if (npc.ai[1] >= 4f && isCultist && (int)(npc.ai[1] - 4f) >= ancientDoomAttackDelay * (int)attackNumber)
                        {
                            attackNumber++;
                            List<int> list8 = new List<int>();
                            for (int num50 = 0; num50 < 200; num50++)
                            {
                                if (Main.npc[num50].active && Main.npc[num50].type == NPCID.CultistBossClone && Main.npc[num50].ai[3] == (float)npc.whoAmI)
                                {
                                    list8.Add(num50);
                                }
                            }
                            int num51 = list8.Count + 1;
                            if (num51 > 3)
                            {
                                num51 = 3;
                            }
                            int num52 = Math.Sign(player.Center.X - center.X);
                            if (num52 != 0)
                            {
                                npc.direction = (npc.spriteDirection = num52);
                            }
                            if (Main.netMode != 1)
                            {
                                for (int num53 = 0; num53 < num51; num53++)
                                {
                                    Point point = npc.Center.ToTileCoordinates();
                                    Point point2 = Main.player[npc.target].Center.ToTileCoordinates();
                                    Vector2 vector9 = Main.player[npc.target].Center - npc.Center;
                                    int num54 = 20;
                                    int num55 = 3;
                                    int num56 = 7;
                                    int num57 = 2;
                                    int num58 = 0;
                                    bool flag5 = false;
                                    if (vector9.Length() > 2000f)
                                    {
                                        flag5 = true;
                                    }
                                    while (!flag5 && num58 < 100)
                                    {
                                        num58++;
                                        int num59 = Main.rand.Next(point2.X - num54, point2.X + num54 + 1);
                                        int num60 = Main.rand.Next(point2.Y - num54, point2.Y + num54 + 1);
                                        //maybye wrong tile?
                                        if ((num60 < point2.Y - num56 || num60 > point2.Y + num56 || num59 < point2.X - num56 || num59 > point2.X + num56) && (num60 < point.Y - num55 || num60 > point.Y + num55 || num59 < point.X - num55 || num59 > point.X + num55) && !Main.tile[num59, num60].IsActuated)
                                        {
                                            bool flag6 = true;
                                            if (flag6 && Collision.SolidTiles(num59 - num57, num59 + num57, num60 - num57, num60 + num57))
                                            {
                                                flag6 = false;
                                            }
                                            if (flag6)
                                            {
                                                NPC.NewNPC(npc.GetSource_FromAI(), num59 * 16 + 8, num60 * 16 + 8, 523, 0, npc.whoAmI);
                                                flag5 = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        npc.ai[1] += AttackSpeed(npc);
                        if (npc.ai[1] >= (float)(4 + ancientDoomAttackDelay * ancientDoomCount))
                        {
                            npc.ai[0] = 0f;
                            npc.ai[1] = 0f;
                            npc.ai[3] += 1f;
                            npc.velocity = Vector2.Zero;
                            npc.netUpdate = true;
                        }
                    }
                    if (!isCultist)
                    {
                        npc.ai[3] = attackTally;
                    }
                    npc.dontTakeDamage = flag2;
                    npc.chaseable = !flag3;

                    return false;
                }

                return base.PreAI(npc);
            }
            return true;
		}

        private static void IllusionsAttack(NPC npc, Player player)
        {
            List<int> list5 = new List<int>();
            for (int num24 = 0; num24 < 200; num24++)
            {
                if (Main.npc[num24].active && Main.npc[num24].type == NPCID.CultistBossClone && Main.npc[num24].ai[3] == (float)npc.whoAmI)
                {
                    list5.Add(num24);
                }
            }
            foreach (int item5 in list5)
            {
                NPC nPC5 = Main.npc[item5];
                Vector2 center5 = nPC5.Center;
                int num25 = Math.Sign(player.Center.X - center5.X);
                if (num25 != 0)
                {
                    nPC5.direction = (nPC5.spriteDirection = num25);
                }
                if (Main.netMode != 1)
                {
                    Vector2 vec3 = Vector2.Normalize(player.Center - center5 + player.velocity * 20f);
                    if (vec3.HasNaNs())
                    {
                        vec3 = new Vector2(npc.direction, 0f);
                    }
                    Vector2 vector6 = center5 + new Vector2(npc.direction * 30, 12f);
                    for (int num26 = 0; num26 < 1; num26++)
                    {
                        Vector2 spinninpoint4 = vec3 * (6f + (float)Main.rand.NextDouble() * 4f);
                        spinninpoint4 = spinninpoint4.RotatedByRandom(0.52359879016876221);
                        Projectile.NewProjectile(nPC5.GetSource_FromThis(), vector6.X, vector6.Y, spinninpoint4.X, spinninpoint4.Y, 468, 18, 0f, Main.myPlayer);
                    }
                }
            }
        }

        private static void SelectAction(NPC npc, bool expert, bool belowHalf, Vector2 center, Player player)
        {
            
            int num14 = 0;
            if (belowHalf)
            {
                switch ((int)npc.ai[3])
                {
                    case 0:
                        num14 = 0;
                        break;
                    case 1:
                        num14 = 1;
                        break;
                    case 2:
                        num14 = 0;
                        break;
                    case 3:
                        num14 = 5;
                        break;
                    case 4:
                        num14 = 0;
                        break;
                    case 5:
                        num14 = 3;
                        break;
                    case 6:
                        num14 = 0;
                        break;
                    case 7:
                        num14 = 5;
                        break;
                    case 8:
                        num14 = 0;
                        break;
                    case 9:
                        num14 = 2;
                        break;
                    case 10:
                        num14 = 0;
                        break;
                    case 11:
                        num14 = 3;
                        break;
                    case 12:
                        num14 = 0;
                        break;
                    case 13:
                        num14 = 4;
                        npc.ai[3] = -1f;
                        break;
                    default:
                        npc.ai[3] = -1f;
                        break;
                }
            }
            else
            {
                switch ((int)npc.ai[3])
                {
                    case 0:
                        num14 = 0;
                        break;
                    case 1:
                        num14 = 1;
                        break;
                    case 2:
                        num14 = 0;
                        break;
                    case 3:
                        num14 = 2;
                        break;
                    case 4:
                        num14 = 0;
                        break;
                    case 5:
                        num14 = 3;
                        break;
                    case 6:
                        num14 = 0;
                        break;
                    case 7:
                        num14 = 1;
                        break;
                    case 8:
                        num14 = 0;
                        break;
                    case 9:
                        num14 = 2;
                        break;
                    case 10:
                        num14 = 0;
                        break;
                    case 11:
                        num14 = 4;
                        npc.ai[3] = -1f;
                        break;
                    default:
                        npc.ai[3] = -1f;
                        break;
                }
            }
            int maxValue = 6;
            if (npc.life < npc.lifeMax / 3)
            {
                maxValue = 4;
            }
            if (npc.life < npc.lifeMax / 4)
            {
                maxValue = 3;
            }
            if (expert && belowHalf && Main.rand.Next(maxValue) == 0 && num14 != 0 && num14 != 4 && num14 != 5 && NPC.CountNPCS(523) < 10)
            {
                num14 = 6;
            }
            if (num14 == 0)
            {
                float position = MathF.Ceiling((player.Center + new Vector2(0f, -100f) - center).Length() / (50f * AttackSpeed(npc)));
                if (position == 0f)
                {
                    position = 1f;
                }
                List<int> list2 = new List<int>();
                int num16 = 0;
                list2.Add(npc.whoAmI);
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && Main.npc[k].type == 440 && Main.npc[k].ai[3] == (float)npc.whoAmI)
                    {
                        list2.Add(k);
                    }
                }
                bool flag4 = list2.Count % 2 == 0;
                foreach (int item2 in list2)
                {
                    NPC nPC2 = Main.npc[item2];
                    Vector2 center2 = nPC2.Center;
                    float num17 = (float)((num16 + flag4.ToInt() + 1) / 2) * (MathF.PI * 2f) * 0.4f / (float)list2.Count;
                    if (num16 % 2 == 1)
                    {
                        num17 *= -1f;
                    }
                    if (list2.Count == 1)
                    {
                        num17 = 0f;
                    }
                    Vector2 value = new Vector2(0f, -1f).RotatedBy(num17) * new Vector2(300f, 200f);
                    Vector2 value2 = player.Center + value - center2;
                    nPC2.ai[0] = 1f;
                    nPC2.ai[1] = position * 2;
                    nPC2.velocity = value2 / position;
                    if (npc.whoAmI >= nPC2.whoAmI)
                    {
                        nPC2.position -= nPC2.velocity;
                    }
                    nPC2.netUpdate = true;
                    num16++;
                }
            }
            switch (num14)
            {
                case 1:
                    npc.ai[0] = 3f;
                    npc.ai[1] = 0f;
                    break;
                case 2:
                    npc.ai[0] = 2f;
                    npc.ai[1] = 0f;
                    break;
                case 3:
                    npc.ai[0] = 4f;
                    npc.ai[1] = 0f;
                    break;
                case 4:
                    npc.ai[0] = 5f;
                    npc.ai[1] = 0f;
                    break;
            }
            if (num14 == 5)
            {
                npc.ai[0] = 7f;
                npc.ai[1] = 0f;
            }
            if (num14 == 6)
            {
                npc.ai[0] = 8f;
                npc.ai[1] = 0f;
            }
            npc.netUpdate = true;
        }

        private static void Opening(NPC npc, out bool flag2, out bool flag3)
        {
            npc.alpha -= 5;
            if (npc.alpha < 0)
            {
                npc.alpha = 0;
            }
            npc.ai[1] += AttackSpeed(npc);
            if (npc.ai[1] >= 420f)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
            else if (npc.ai[1] > 360f)
            {
                npc.velocity *= 0.95f;
                if (npc.localAI[2] != 13f)
                {
                    SoundEngine.PlaySound(SoundID.Zombie105 with { MaxInstances = 0 }, npc.Center);
                }
                npc.localAI[2] = 13f;
            }
            else if (npc.ai[1] > 300f)
            {
                npc.velocity = -Vector2.UnitY;
                npc.localAI[2] = 10f;
            }
            else if (npc.ai[1] > 120f)
            {
                npc.localAI[2] = 1f;
            }
            else
            {
                npc.localAI[2] = 0f;
            }
            flag2 = true;
            flag3 = true;
        }

        private static void CheckDeadPlayer(NPC npc, Vector2 center, ref Player player)
        {
            float num11 = 5600f;
            if (npc.target < 0 || npc.target == 255 || player.dead || !player.active || Vector2.Distance(player.Center, center) > num11)
            {
                npc.TargetClosest(faceTarget: false);
                player = Main.player[npc.target];
                npc.netUpdate = true;
            }
            if (player.dead || !player.active || Vector2.Distance(player.Center, center) > num11)
            {
                npc.life = 0;
                npc.HitEffect();
                npc.active = false;
                if (Main.netMode != 1)
                {
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                }
                new List<int>().Add(npc.whoAmI);
                for (int j = 0; j < 200; j++)
                {
                    if (Main.npc[j].active && Main.npc[j].type == 440 && Main.npc[j].ai[3] == (float)npc.whoAmI)
                    {
                        Main.npc[j].life = 0;
                        Main.npc[j].HitEffect();
                        Main.npc[j].active = false;
                        if (Main.netMode != 1)
                        {
                            NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                        }
                    }
                }
            }
        }

        private static void EndAttack(NPC npc)
        {
            npc.ai[0] = 0f;
            npc.ai[1] = 0f;
            npc.ai[3] += 1f; 
            npc.velocity = Vector2.Zero;
            npc.netUpdate = true;
        }
    }
}
