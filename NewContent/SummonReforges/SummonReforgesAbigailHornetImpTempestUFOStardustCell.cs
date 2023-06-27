using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

namespace TRAEProject.NewContent.SummonReforges
{
    public partial class TestForSummonReforgesMinionChanges : GlobalProjectile
    {
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            switch (projectile.type)
            {
                case ProjectileID.RainbowCrystalExplosion:
                    DrawRainbowCrystalExplosion(projectile);
                    return false;
                case ProjectileID.RainbowCrystal:
                    return true;
            }
            return true;
        }
        static Vector2 GetPredictiveAimVector(Vector2 shotOrigin, float shotVelLength, Vector2 aimedTargetPos, Vector2 aimedTargetVel)
        {
            float angleToTarget = (aimedTargetPos - shotOrigin).ToRotation();
            float targetTraj = aimedTargetVel.ToRotation();
            float aimedTargetVelLength = aimedTargetVel.Length();
            float z = MathF.PI + targetTraj - angleToTarget;
            return (angleToTarget - MathF.Asin(aimedTargetVelLength * MathF.Sin(z) / (shotVelLength))).ToRotationVector2() * shotVelLength;
        }
        static float GetPredictiveAimRotation(Vector2 shotOrigin, float shotVelLength, Vector2 aimedTargetPos, Vector2 aimedTargetVel)
        {
            float angleToTarget = (aimedTargetPos - shotOrigin).ToRotation();
            float targetTraj = aimedTargetVel.ToRotation();
            float aimedTargetVelLength = aimedTargetVel.Length();
            float z = MathF.PI + targetTraj - angleToTarget;
            return angleToTarget - MathF.Asin(aimedTargetVelLength * MathF.Sin(z) / (shotVelLength));
        }
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            if(projectile.type == ProjectileID.MiniSharkron)
            {
                Texture2D texture = TextureAssets.Projectile[ProjectileID.Cthulunado].Value;
                float heightOffset = 0;
                float scale = 0.7f;
                float rotation = projectile.rotation +( projectile.spriteDirection == -1 ? MathF.PI : 0);
                for (int i = 0; i < 4; i++)
                {
                    Rectangle frame = Utils.Frame(texture, 1, 6, 0, (int)((i * 3) + Main.timeForVisualEffects) / 3 % 6);
                    Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition + new Vector2(heightOffset,0).RotatedBy(rotation), frame, lightColor * 0.45f * projectile.Opacity, rotation + MathF.PI / 2, frame.Size() / 2, scale, SpriteEffects.None, 0);
                        scale -= 0.1f;
                    heightOffset -= ((float)frame.Height) * scale;
                }
            }
        }
        static void VisualizeCircle(Vector2 circleOrigin, float radius)
        {
            Dust.NewDustPerfect(circleOrigin, DustID.MagnetSphere, Vector2.Zero);
            float extraROtation = Main.rand.NextFloat();
            for (float i = 0; i < 1; i+= 0.02f)
            {
                Dust.NewDustPerfect(circleOrigin + (i * MathF.Tau + extraROtation).ToRotationVector2() * radius, DustID.MagnetSphere, Vector2.Zero,0,default,0.5f);
            }
        }
        static bool CircleCollision(Vector2 circleOrigin, float radius, Rectangle targetHitbox) 
        {
            return circleOrigin.DistanceSQ(targetHitbox.ClosestPointInRect(circleOrigin)) <= radius * radius;
        }
        public override bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
        {
            if(projectile.type == ProjectileID.MiniSharkron)
            {
                bool collided = false;
                collided = collided || CircleCollision(projectile.Center - Vector2.Normalize(projectile.velocity) * 10, 50, targetHitbox);
                collided = collided || CircleCollision(projectile.Center - Vector2.Normalize(projectile.velocity) * 30, 40, targetHitbox);
                collided = collided || CircleCollision(projectile.Center - Vector2.Normalize(projectile.velocity) * 50, 30, targetHitbox);
                return collided;
            }else if(projectile.type >= ProjectileID.DD2LightningAuraT1 && projectile.type <= ProjectileID.DD2LightningAuraT3)
            {
                return false;// LightningAuraCustomCollision(projectile, projHitbox, targetHitbox);
            }else if(projectile.type == ProjectileID.RainbowCrystalExplosion)
            {

                return CircleCollision(projectile.Center, projectile.width, targetHitbox);//TEST LINE
            }
            return null;
        }


        public override bool PreAI(Projectile projectile)
        {
            //prefixAttackRate = Main.MouseWorld.X - Main.player[projectile.owner].Center.X > 0 ? 1 : 1.9f;//DEBUG STUFF, REMOVE LATER
            //prefixAttackVelocity = Main.MouseWorld.X - Main.player[projectile.owner].Center.X > 0 ? 1 : 3f;
            //prefixMoveSpeed = Main.MouseWorld.X - Main.player[projectile.owner].Center.X > 0 ? 1 : 3;
            //prefixMoveAcceleration = Main.MouseWorld.X - Main.player[projectile.owner].Center.X > 0 ? 1 : 3;
            //prefixAggroRangeBoost = Main.MouseWorld.X - Main.player[projectile.owner].Center.X > 0 ? 1 : 3;
            switch (projectile.type)
            {
                
                case ProjectileID.UFOMinion or ProjectileID.Hornet or ProjectileID.Tempest or ProjectileID.AbigailMinion or ProjectileID.StardustCellMinion or ProjectileID.FlyingImp:

                    AI_062(projectile);
                    return false;
                case ProjectileID.MiniSharkron:
                    float targetAngle = GetPredictiveAimRotation(projectile.Center, projectile.velocity.Length(), Main.npc[(int)projectile.ai[2]].Center, Main.npc[(int)projectile.ai[2]].velocity);
                    projectile.velocity = projectile.velocity.ToRotation().AngleTowards(targetAngle, 0.01f).ToRotationVector2() * projectile.velocity.Length();
                    return true;
                case >= ProjectileID.DD2LightningAuraT1 and <= ProjectileID.DD2LightningAuraT3:
                    AI_137_LightningAura(projectile);
                    return false;
                case >= ProjectileID.DD2BallistraTowerT1 and <= ProjectileID.DD2BallistraTowerT3:
                    AI_134_Ballista(projectile);
                    return false;
                case ProjectileID.DD2FlameBurstTowerT1 or ProjectileID.DD2FlameBurstTowerT2 or ProjectileID.DD2FlameBurstTowerT3:
                    AI_130_FlameBurstTower(projectile);
                    return false;
                case ProjectileID.DD2ExplosiveTrapT1 or ProjectileID.DD2ExplosiveTrapT2 or ProjectileID.DD2ExplosiveTrapT3:
                    AI_138_ExplosiveTrap(projectile);
                    return false;
                case ProjectileID.RainbowCrystal://123 aistyle
                    RainbowCrystalStaffAI(projectile);
                    return false;
                case ProjectileID.RainbowCrystalExplosion://ai 112
                    RainbowCrystalStaffAIExplosion(projectile);
                    return false;
            }
            return true;
        }
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.DamageType == DamageClass.Summon;
        public override bool InstancePerEntity => true;
        float prefixAttackRate = 1;
		float prefixMoveSpeed = 1;
		float prefixMoveAcceleration = 1;
		float prefixAttackVelocity = 1;
		float prefixAggroRangeBoost = 1;
        //TODO: MAKE THESE ACCOUNT FOR PLAYER EQUIPMENT AND BUFFS(which currently exist none of so this is why they are like this
        public float GetInertia(int owner) 
        {
            return 1 / MathHelper.Clamp(prefixMoveAcceleration + GetPlayerMinionMoveAccelerationEquipStats(Main.player[owner]), float.Epsilon, float.MaxValue);
        }
        public float GetMoveSpeed(int owner)
        {
            return prefixMoveSpeed + GetPlayerMinionMoveSpeedEquipStats(Main.player[owner]);
        } 
        public float GetMoveAcceleration(int owner)
        {
            return prefixMoveAcceleration + GetPlayerMinionMoveAccelerationEquipStats(Main.player[owner]);
        }
        public float GetAttackVelocity(int owner)
        {
            return prefixAttackVelocity + GetPlayerMinionAttackVelocityEquipStats(Main.player[owner]);
        }
        public float GetAttackRateAsTimerIncrease(int owner)
        {
            return prefixAttackRate + GetPlayerMinionAttackRateEquipStats(Main.player[owner]);
        }
        public float GetAttackRateAsTimerThresholdMultiplier(int owner)
        {
            return 1 / MathHelper.Clamp(prefixAttackRate + GetPlayerMinionAttackRateEquipStats(Main.player[owner]), float.Epsilon, float.MaxValue);
        }
        public float GetAggroRangeBoost(int owner)
        {
            return prefixAggroRangeBoost + GetPlayerMinionAggroRangeEquipStats(Main.player[owner]);
        }
		private void AI_062(Projectile projectile)
        {
            //Main.NewText($"ai0:{projectile.ai[0]}, ai1:{projectile.ai[1]}, ai2:{projectile.ai[2]}", Main.hslToRgb((float)Main.timeForVisualEffects / 10 % 1, 1, 0.65f));
            //Main.NewText($"localai0:{projectile.localAI[0]}, localai1:{projectile.localAI[1]}, localai2:{projectile.localAI[2]}", Main.hslToRgb((float)Main.timeForVisualEffects / 10 % 1, 1, 0.65f));
            //#region AnnoyingRefVars
            //can't be bothered to do it for the rest
            ref int spriteDirection = ref projectile.spriteDirection;
            ref float scale = ref projectile.scale;
            ref float rotation = ref projectile.rotation;
            ref int projoriginalDamage = ref projectile.originalDamage;
            ref int numUpdates = ref projectile.numUpdates;
            ref bool netUpdate = ref projectile.netUpdate;
            ref float[] ai = ref projectile.ai;
            ref float[] projlocalAI = ref projectile.localAI;
            ref float projknockBack = ref projectile.knockBack;
            ref int projframeCounter = ref projectile.frameCounter;
            ref int projframe = ref projectile.frame;
            ref int extraUpdates = ref projectile.extraUpdates;
            ref int projdirection = ref projectile.direction;
            ref int projalpha = ref projectile.alpha;

            //#endregion
            float anotherAbigailSpeedThing = 0f;
            float abigailSpeedThing = 0f;
            float abigailInertia = 20f;
            float resetAttackThreshHold = 40f;
            float abigailVelMultWHileAttacking = 0.69f;
            if (projectile.type == ProjectileID.UFOMinion)
            {
                resetAttackThreshHold = 5f;
            }
            SustainTimeLeftWhilePlayerIsAliveAndAlsoSomeOtherAbigailThingsIdk(ref projectile, ref projoriginalDamage, ref projlocalAI, ref anotherAbigailSpeedThing, ref abigailSpeedThing, ref abigailInertia, ref resetAttackThreshHold, ref abigailVelMultWHileAttacking);
            bool shouldReturn = UFOTeleportDustEffectAndUpdateTeleportTimer(ref projectile) || StardustCellTeleport(ref projectile);
            if (shouldReturn)//workaround for having to put the teleports into methods
                return;
            SocialDistancing(ref projectile);
            Vector2 targetNPCPosition = projectile.position;
            float aggroRangeAndAlsoDistToTarget = GetAggroRange(projectile);
            bool hasTarget = false;
            int targetIndex = -1;
            projectile.tileCollide = true;

            TempestAbigailPhaseThroughBlocks(ref projectile);
            if (projectile.type == ProjectileID.Tempest || projectile.type == ProjectileID.StardustCellMinion || projectile.type == ProjectileID.UFOMinion)
            {
                TempestStardustCellUFOVerifyCurrentTargetValidity(projectile, ref targetNPCPosition, ref aggroRangeAndAlsoDistToTarget, ref hasTarget, ref targetIndex, out Vector2 center, out Vector2 point5Vec);
                TempestStardustCellUFOFindTarget(projectile, ref targetNPCPosition, ref aggroRangeAndAlsoDistToTarget, ref hasTarget, ref targetIndex, center, point5Vec);
            }
            else
            {
                AbigailHornetImpVerifyCurrentTargetValidity(projectile, ref targetNPCPosition, ref aggroRangeAndAlsoDistToTarget, ref hasTarget, ref targetIndex);
                AbigailHornetImpFindTarget(projectile, ref targetNPCPosition, ref aggroRangeAndAlsoDistToTarget, ref hasTarget, ref targetIndex);
            }
            int distanceToPlayerUntilPhaseThroughBlocks = GetDistanceToPlayerUntilPhaseThroughBlocks(projectile, hasTarget);
            Player player = Main.player[projectile.owner];
            if (Vector2.Distance(player.Center, projectile.Center) > distanceToPlayerUntilPhaseThroughBlocks)
            {
                ai[0] = 1f;
                netUpdate = true;
            }
            if (ai[0] == 1f)
            {
                projectile.tileCollide = false;
            }
            bool isAbigailMinion = projectile.type == ProjectileID.AbigailMinion;
            if (isAbigailMinion)
            {
                if (ai[0] <= 1f && projlocalAI[1] <= 0f)
                {
                    projlocalAI[1] = -1f;
                }
                else
                {
                    projlocalAI[1] = Utils.Clamp(projlocalAI[1] + 0.05f, 0f, 1f);
                    if (projlocalAI[1] == 1f)
                    {
                        projlocalAI[1] = -1f;
                    }
                }
            }
            if (projectile.type == ProjectileID.AbigailMinion && projectile.velocity.Length() > 0.1f && Main.rand.NextBool(1500))
            {
                SoundEngine.PlaySound(SoundID.AbigailCry, projectile.Center);
            }
            bool isUFOOrTempestMinion = projectile.type == ProjectileID.UFOMinion || projectile.type == ProjectileID.Tempest;
            if (ai[0] >= 2f)
            {
                if (ai[0] == 2f && projectile.type == ProjectileID.AbigailMinion)
                {
                    SoundEngine.PlaySound(SoundID.AbigailAttack, projectile.Center);
                }
                ai[0] += 1f;
                if (isAbigailMinion)
                {
                    projlocalAI[1] = ai[0] / resetAttackThreshHold;
                }
                if (!hasTarget)
                {
                    ai[0] += 1f;
                }
                if (ai[0] > resetAttackThreshHold * (1 / GetAttackRateAsTimerIncrease(projectile.owner)))
                {
                    ai[0] = 0f;
                    netUpdate = true;
                    if (hasTarget && projectile.type == ProjectileID.AbigailMinion && (targetNPCPosition - projectile.Center).Length() < 50f)
                    {
                        ai[0] = 2f;
                    }
                }
                //projectile.velocity *= abigailVelMultWHileAttacking;
            }
            if ((hasTarget && (isUFOOrTempestMinion || ai[0] == 0f) && ai[0] < 2f) || (hasTarget && isAbigailMinion))
            {
                
                Vector2 toTargetNormalized = targetNPCPosition - projectile.Center;
                float distToTarget = toTargetNormalized.Length();
                toTargetNormalized = toTargetNormalized.SafeNormalize(Vector2.Zero);
                if (projectile.type == ProjectileID.UFOMinion)
                    if (UFOTeleportToTarget(ref projectile, ref toTargetNormalized, ref targetNPCPosition, ref distToTarget))
                        return;            
                if (projectile.type == ProjectileID.StardustCellMinion)
                    if (StardustCellTeleportToTarget(ref projectile, ref toTargetNormalized, ref targetNPCPosition, ref distToTarget))
                        return;
                
                if (projectile.type == ProjectileID.Tempest)
                {
                    if (distToTarget > 400f)
                    {
                        float maxSpeed = 3 * GetMoveSpeed(projectile.owner);//base move speed is 3, idk if this is move speed 
                        toTargetNormalized *= maxSpeed;
                        float inertia = GetInertia(projectile.owner) * 20;//base is 20 idk if this is acceleration
                        projectile.velocity = (projectile.velocity * inertia + toTargetNormalized) / (inertia + 1);
                    }
                    else
                    {
                        projectile.velocity *= 0.96f;
                    }
                }
                if (distToTarget > 200f)
                {
                    float baseInertia = 6f + abigailSpeedThing * anotherAbigailSpeedThing;//these are 1 except for abigail
                    toTargetNormalized *= baseInertia * GetMoveSpeed(projectile.owner);
                    float inertia = abigailInertia * 2f * GetInertia(projectile.owner);
                    projectile.velocity = (projectile.velocity * inertia + toTargetNormalized) / (inertia + 1f);
                }
                else if (projectile.type == ProjectileID.AbigailMinion )
                {
                    if (distToTarget < 10f)
                    {

                        //projectile.velocity *= 0.5f;//THIS MADE ABIGAIL FREEZE WHEN ATTACKING
                        if (ai[0] < 2f)
                        {
                            ai[0] = 2f;
                            netUpdate = true;
                        }
                    }
                    else
                    {
                        float finalAbigailMaxSpeed = 8f + abigailSpeedThing * anotherAbigailSpeedThing;
                        toTargetNormalized *= finalAbigailMaxSpeed * GetMoveSpeed(projectile.owner);
                        projectile.velocity = (projectile.velocity * abigailInertia + toTargetNormalized) / (abigailInertia + 1f);
                    }
                    Rectangle hitboxToCheck = Main.npc[targetIndex].Hitbox;
                    hitboxToCheck.Inflate(Main.npc[targetIndex].width/2, Main.npc[targetIndex].Hitbox.Height/2);//deflate actually
                    //if (projectile.Hitbox.Intersects(hitboxToCheck))
                    {
                        
                        projectile.velocity.MoveTowards(Main.npc[targetIndex].velocity * projectile.Center.Distance(Main.npc[targetIndex].Hitbox.ClosestPointInRect(projectile.Center)), 1);
                    }
                }
                else if (projectile.type == ProjectileID.UFOMinion || projectile.type == ProjectileID.StardustCellMinion)
                {
                    if (distToTarget > 70f && distToTarget < 130f)
                    {
                        float baseMoveSpeed = 7f;
                        if (distToTarget < 100f)
                        {
                            baseMoveSpeed = -3f;
                        }
                        toTargetNormalized *= baseMoveSpeed * GetMoveSpeed(projectile.owner);
                        float inertia = 20 * GetInertia(projectile.owner);//base move speed naming is inconsistent as I am really confused by this
                        projectile.velocity = (projectile.velocity * inertia + toTargetNormalized) / (inertia + 1);
                        if (Math.Abs(toTargetNormalized.X) > Math.Abs(toTargetNormalized.Y))//so they prefer to stay vertically alligned I think
                        {
                            float xInertia = 10;
                            xInertia *= GetMoveSpeed(projectile.owner);
                            projectile.velocity.X = (projectile.velocity.X * xInertia + toTargetNormalized.X) / (xInertia + 1);
                        }
                    }
                    else
                    {
                        projectile.velocity *= 0.97f;
                    }
                }
                else if (projectile.type == ProjectileID.FlyingImp)
                {
                    if (distToTarget < 150f)
                    {
                        float impMoveSpeed = 4f * GetMoveSpeed(projectile.owner);
                        toTargetNormalized *= - impMoveSpeed;
                        float inertia = 40;
                        inertia *= GetInertia(projectile.owner);
                        projectile.velocity = (projectile.velocity * inertia + toTargetNormalized) / (inertia + 1);
                    }
                    else
                    {
                        projectile.velocity *= 0.97f;
                    }
                }
                else if (projectile.velocity.Y > -1f)
                {
                    projectile.velocity.Y -= 0.1f;
                }
            }
            else
            {
                if (projectile.type != ProjectileID.AbigailMinion && !Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
                {
                    ai[0] = 1f;
                }
                float maxSpeed = 6f;
                if (ai[0] == 1f)
                {
                    maxSpeed = 15f;
                }
                if (projectile.type == ProjectileID.Tempest)
                {
                    maxSpeed = 9f;
                }
                if (projectile.type == ProjectileID.AbigailMinion)
                {
                    maxSpeed *= 0.8f;
                }
                maxSpeed *= GetMoveSpeed(projectile.owner);
                Vector2 projCenter = projectile.Center;
                Vector2 dirToIdlePosVecNormalized = player.Center - projCenter + new Vector2(0f, -60f);
                if (projectile.type == ProjectileID.Tempest)
                {
                    dirToIdlePosVecNormalized += new Vector2(0f, 40f);
                }
                if (projectile.type == ProjectileID.AbigailMinion)
                {
                    dirToIdlePosVecNormalized += new Vector2(-40 * Main.player[projectile.owner].direction, 40f);
                }
                if (projectile.type == ProjectileID.FlyingImp)
                {
                    ai[1] = 80f;
                    netUpdate = true;
                    dirToIdlePosVecNormalized = player.Center - projCenter;
                    int numberOfThisMinionBeforeMe = 1;
                    for (int i = 0; i < projectile.whoAmI; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type)
                        {
                            numberOfThisMinionBeforeMe++;
                        }
                    }
                    dirToIdlePosVecNormalized.X -= 10 * Main.player[projectile.owner].direction;
                    dirToIdlePosVecNormalized.X -= numberOfThisMinionBeforeMe * 40 * Main.player[projectile.owner].direction;
                    dirToIdlePosVecNormalized.Y -= 10f;
                }
                float distToIdlePosLength = dirToIdlePosVecNormalized.Length();
                if (distToIdlePosLength > 200f && maxSpeed < 9f * GetMoveSpeed(projectile.owner))
                {
                    maxSpeed = 9f * GetMoveSpeed(projectile.owner);
                }
                if ((projectile.type == ProjectileID.UFOMinion || projectile.type == ProjectileID.Tempest) && distToIdlePosLength > 300f && maxSpeed < 12f * GetMoveSpeed(projectile.owner))
                {
                    maxSpeed = 12f * GetMoveSpeed(projectile.owner);
                }
                //if (projectile.type == ProjectileID.FlyingImp)
                //{
                //    maxSpeed = (int)(maxSpeed * 0.75);
                //}
                if (distToIdlePosLength < 100f && ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    ai[0] = 0f;
                    netUpdate = true;
                }
                if (distToIdlePosLength > 2000f)//teleport to player
                {
                    projectile.Center = Main.player[projectile.owner].Center;
                }
                if (projectile.type == ProjectileID.FlyingImp || projectile.type == ProjectileID.AbigailMinion)
                {
                    if (distToIdlePosLength > 10f)
                    {
                        dirToIdlePosVecNormalized = dirToIdlePosVecNormalized.SafeNormalize(Vector2.Zero);
                        if (distToIdlePosLength < 50f)
                        {
                            maxSpeed /= 2f;
                        }
                        dirToIdlePosVecNormalized *= maxSpeed;
                        float inertia = 20 * GetInertia(projectile.owner);
                        projectile.velocity = (projectile.velocity * inertia + dirToIdlePosVecNormalized) / (inertia + 1);
                    }
                    else
                    {
                        projdirection = Main.player[projectile.owner].direction;
                        projectile.velocity *= 0.9f;
                    }
                }
                else if (projectile.type == ProjectileID.Tempest)
                {
                    if (Math.Abs(dirToIdlePosVecNormalized.X) > 40f || Math.Abs(dirToIdlePosVecNormalized.Y) > 10f)
                    {
                        dirToIdlePosVecNormalized = dirToIdlePosVecNormalized.SafeNormalize(Vector2.Zero);
                        dirToIdlePosVecNormalized *= maxSpeed;
                        dirToIdlePosVecNormalized *= new Vector2(1.25f, 0.65f);
                        float inertia = 20 * GetInertia(projectile.owner);
                        projectile.velocity = (projectile.velocity * inertia + dirToIdlePosVecNormalized) / (inertia + 1);
                    }
                    else
                    {
                        if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                        {
                            projectile.velocity.X = -0.15f;
                            projectile.velocity.Y = -0.05f;
                        }
                        projectile.velocity *= 1.01f;
                    }
                }
                else if (distToIdlePosLength > 70f)
                {
                    dirToIdlePosVecNormalized = dirToIdlePosVecNormalized.SafeNormalize(Vector2.Zero);
                    dirToIdlePosVecNormalized *= maxSpeed;
                    projectile.velocity = (projectile.velocity * 20f + dirToIdlePosVecNormalized) / 21f;
                }
                else
                {
                    if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                    {
                        projectile.velocity.X = -0.15f;
                        projectile.velocity.Y = -0.05f;
                    }
                    projectile.velocity *= 1.01f;
                }
                if (distToIdlePosLength > 250f && (projectile.type == ProjectileID.UFOMinion || projectile.type == ProjectileID.Tempest))
                {
                    float playerXVel = Main.player[projectile.owner].velocity.X;
                    float playerYVel = Main.player[projectile.owner].velocity.Y;
                    if ((projectile.velocity.X < 0f && playerXVel >= 0f) || (projectile.velocity.X >= 0f && playerXVel < 0f))
                        projectile.velocity.X *= 0.95f;
                    else
                        projectile.velocity.X += playerXVel * 0.125f;

                    if ((projectile.velocity.Y < 0f && playerYVel >= 0f) || (projectile.velocity.Y >= 0f && playerYVel < 0f))
                        projectile.velocity.Y *= 0.95f;
                    else
                        projectile.velocity.Y += playerYVel * 0.125f;

                    if (projectile.velocity.Length() > maxSpeed)
                    {
                        projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * maxSpeed;
                    }
                }
            }
            HandleMinionVisualRotationDirectionFrameAndDust(ref projectile, ref spriteDirection, ref rotation, ref netUpdate, ref ai, ref projframeCounter, ref projframe, ref projdirection, ref hasTarget);
            HandleShootCooldown(ref projectile);
            if (!isUFOOrTempestMinion && ai[0] != 0f)
                return;
            GetProjectileToShootAndShotVelLengthDependingOnMinionType(projectile, out float shotVelLength, out int projTypeToShoot);
            if (!hasTarget)
                return;
            Imp_FaceTowardsTarget(ref projectile, ref spriteDirection, ref projdirection, ref targetNPCPosition);
            if (projectile.type == ProjectileID.Tempest && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                return;//TEMPEST DONT SHOOT IF NO LINE OF SIGHT 
            if (projectile.type == ProjectileID.UFOMinion)
                UFOMinionShootProjectile(targetNPCPosition, ref projectile, projTypeToShoot, shotVelLength, targetIndex);
            else if (ai[1] == 0f && projectile.type == ProjectileID.StardustCellMinion)
                StardustCellShootProjectile(targetNPCPosition, ref projectile, projTypeToShoot, shotVelLength, targetIndex);
            else if (ai[1] == 0f)
                HornetTempestImpShootProjectile(ref projectile, targetNPCPosition, shotVelLength, projTypeToShoot, targetIndex);
        }

        private void AbigailHornetImpFindTarget(Projectile projectile, ref Vector2 targetNPCPosition, ref float aggroRangeAndAlsoDistToTarget, ref bool hasTarget, ref int targetIndex)
        {
            if (!hasTarget)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC possibleTargetNPC = Main.npc[i];
                    if (!possibleTargetNPC.CanBeChasedBy(this))
                    {
                        continue;
                    }
                    float distToPossibleTarget = Vector2.Distance(possibleTargetNPC.Center, projectile.Center);
                    if (!(distToPossibleTarget >= aggroRangeAndAlsoDistToTarget))
                    {
                        if ((projectile.type != ProjectileID.AbigailMinion) ? Collision.CanHitLine(projectile.position, projectile.width, projectile.height, possibleTargetNPC.position, possibleTargetNPC.width, possibleTargetNPC.height) : Collision.CanHit(projectile.Center, 1, 1, possibleTargetNPC.Center, 1, 1))
                        {
                            aggroRangeAndAlsoDistToTarget = distToPossibleTarget;
                            targetNPCPosition = possibleTargetNPC.Center;
                            hasTarget = true;
                            targetIndex = i;
                        }
                    }
                }
            }
        }

        private void AbigailHornetImpVerifyCurrentTargetValidity(Projectile projectile, ref Vector2 targetNPCPosition, ref float aggroRangeAndAlsoDistToTarget, ref bool hasTarget, ref int targetIndex)
        {
            NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(this))
            {
                float distanceToPossibleNewTarget = Vector2.Distance(ownerMinionAttackTargetNPC2.Center, projectile.Center);
                float tripleDistToClosestTarget = aggroRangeAndAlsoDistToTarget * 3f;
                if (distanceToPossibleNewTarget < tripleDistToClosestTarget && !hasTarget)
                {
                    if ((projectile.type != ProjectileID.AbigailMinion) ? Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height) : Collision.CanHit(projectile.Center, 1, 1, ownerMinionAttackTargetNPC2.Center, 1, 1))
                    {
                        aggroRangeAndAlsoDistToTarget = distanceToPossibleNewTarget;
                        targetNPCPosition = ownerMinionAttackTargetNPC2.Center;
                        hasTarget = true;
                        targetIndex = ownerMinionAttackTargetNPC2.whoAmI;
                    }
                }
            }
        }

        private void TempestStardustCellUFOFindTarget(Projectile projectile, ref Vector2 targetNPCPosition, ref float aggroRangeAndAlsoDistToTarget, ref bool hasTarget, ref int targetIndex, Vector2 center, Vector2 point5Vec)
        {
            if (!hasTarget)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(this))
                    {
                        Vector2 targetNpcPosition = npc.position + npc.Size * point5Vec;
                        float distanceToPossibleNewTarget = Vector2.Distance(targetNpcPosition, center);
                        if (!(distanceToPossibleNewTarget >= aggroRangeAndAlsoDistToTarget) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                        {
                            aggroRangeAndAlsoDistToTarget = distanceToPossibleNewTarget;
                            targetNPCPosition = targetNpcPosition;
                            hasTarget = true;
                            targetIndex = i;
                        }
                    }
                }
            }
        }

        private void TempestStardustCellUFOVerifyCurrentTargetValidity(Projectile projectile, ref Vector2 targetNPCPosition, ref float aggroRangeAndAlsoDistToTarget, ref bool hasTarget, ref int targetIndex, out Vector2 center, out Vector2 point5Vec)
        {
            center = Main.player[projectile.owner].Center;
            point5Vec = new(0.5f);
            if (projectile.type == ProjectileID.UFOMinion)
            {
                point5Vec.Y = 0f;
            }
            NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this))
            {
                Vector2 targetCenter = ownerMinionAttackTargetNPC.position + ownerMinionAttackTargetNPC.Size * point5Vec;
                float tripleDistToClosest = aggroRangeAndAlsoDistToTarget * 3f;
                float distanceToPossibleNewTarget = Vector2.Distance(targetCenter, center);
                if (distanceToPossibleNewTarget < tripleDistToClosest && !hasTarget && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC.position, ownerMinionAttackTargetNPC.width, ownerMinionAttackTargetNPC.height))
                {
                    aggroRangeAndAlsoDistToTarget = distanceToPossibleNewTarget;
                    targetNPCPosition = targetCenter;
                    hasTarget = true;
                    targetIndex = ownerMinionAttackTargetNPC.whoAmI;
                }
            }
        }

        bool StardustCellTeleportToTarget(ref Projectile projectile, ref Vector2 directionToTargetVec, ref Vector2 targetNPCPosition, ref float distToTarget)
        {
            directionToTargetVec = targetNPCPosition;
            Vector2 fromTargetToProjCenter = projectile.Center - directionToTargetVec;
            if (fromTargetToProjCenter == Vector2.Zero)
            {
                fromTargetToProjCenter = -Vector2.UnitY;
            }
            fromTargetToProjCenter = fromTargetToProjCenter.SafeNormalize(Vector2.Zero);
            directionToTargetVec += fromTargetToProjCenter * 60f;
            int num24 = (int)directionToTargetVec.Y / 16;
            if (num24 < 0)
            {
                num24 = 0;
            }
            Tile tile = Main.tile[(int)directionToTargetVec.X / 16, num24];
            if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
            {
                directionToTargetVec += Vector2.UnitY * 16f;
                tile = Main.tile[(int)directionToTargetVec.X / 16, (int)directionToTargetVec.Y / 16];
                if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                {
                    directionToTargetVec += Vector2.UnitY * 16f;
                }
            }
            directionToTargetVec -= projectile.Center;
            distToTarget = directionToTargetVec.Length();
            directionToTargetVec = directionToTargetVec.SafeNormalize(Vector2.Zero);
            if (distToTarget > 400f && distToTarget <= 800f && projectile.localAI[0] == 0f)
            {
                projectile.ai[0] = 2f;
                projectile.ai[1] = (int)(distToTarget / 10f);
                projectile.extraUpdates = (int)projectile.ai[1];
                projectile.velocity = directionToTargetVec * 10f;
                projectile.localAI[0] = 60f;
                return true;//this is for teleporting
            }
            return false;
        }
        private void HornetTempestImpShootProjectile(ref Projectile projectile, Vector2 targetNPCPosition, float shotVelLength, int projTypeToShoot, int targetIndex)
        {
            Vector2 shotVel = targetNPCPosition - projectile.Center;
            projectile.ai[1] += 1f * prefixAttackRate;
            if (Main.myPlayer == projectile.owner)
            {
                shotVel = projectile.type == ProjectileID.Tempest ? GetPredictiveAimVector(projectile.Center, shotVelLength, targetNPCPosition, Main.npc[targetIndex].velocity) 
                    : shotVel.SafeNormalize(Vector2.Zero) * shotVelLength;
                
                int shotIndex = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, shotVel.X, shotVel.Y, projTypeToShoot, projectile.damage, projectile.knockBack, Main.myPlayer,targetIndex);
                Main.projectile[shotIndex].timeLeft = 300;
                Main.projectile[shotIndex].netUpdate = true;
                projectile.netUpdate = true;
            }
        }
        private static void TempestAbigailPhaseThroughBlocks(ref Projectile projectile)
        {
            projectile.tileCollide = projectile.type != ProjectileID.AbigailMinion;
            if (projectile.type == ProjectileID.Tempest)
            {
                projectile.tileCollide = false;
                if (Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.alpha += 20;
                    if (projectile.alpha > 150)
                    {
                        projectile.alpha = 150;
                    }
                }
                else
                {
                    projectile.alpha -= 50;
                    if (projectile.alpha < 60)
                    {
                        projectile.alpha = 60;
                    }
                }
            }
        }
        private static int GetDistanceToPlayerUntilPhaseThroughBlocks(Projectile projectile, bool hasTarget)
        {
            int distanceToPlayerUntilPhaseThroughBlocks = 500;
            if (projectile.type == ProjectileID.AbigailMinion)
            {
                distanceToPlayerUntilPhaseThroughBlocks = 800;
            }
            if (hasTarget)
            {
                distanceToPlayerUntilPhaseThroughBlocks = 1000;
            }
            if (hasTarget && projectile.type == ProjectileID.UFOMinion)
            {
                distanceToPlayerUntilPhaseThroughBlocks = 1200;
            }
            if (hasTarget && projectile.type == ProjectileID.StardustCellMinion)
            {
                distanceToPlayerUntilPhaseThroughBlocks = 1350;
            }

            return distanceToPlayerUntilPhaseThroughBlocks;
        }
        bool UFOTeleportDustEffectAndUpdateTeleportTimer(ref Projectile projectile)
        {
            if (projectile.type == ProjectileID.UFOMinion)
            {
                if (projectile.ai[0] == 2f)
                {
                    projectile.ai[1] -= 1f;
                    projectile.tileCollide = false;
                    if (projectile.ai[1] > 3f)
                    {
                        int dustIndex = Dust.NewDust(projectile.Center, 0, 0, 220 + Main.rand.Next(2), projectile.velocity.X, projectile.velocity.Y, 100);
                        Main.dust[dustIndex].scale = 0.5f + (float)Main.rand.NextDouble() * 0.3f;
                        Main.dust[dustIndex].velocity /= 2.5f;
                        Main.dust[dustIndex].noGravity = true;
                        Main.dust[dustIndex].noLight = true;
                        Main.dust[dustIndex].frame.Y = 80;
                    }
                    if (projectile.ai[1] != 0f)//if the teleport hasn't ended
                    {
                        return true;
                    }
                    projectile.ai[1] = 30f;
                    projectile.ai[0] = 0f;
                    projectile.velocity /= 5f;
                    projectile.velocity.Y = 0f;
                    projectile.extraUpdates = 0;
                    projectile.numUpdates = 0;
                    projectile.netUpdate = true;
                    projectile.extraUpdates = 0;
                    projectile.numUpdates = 0;
                }
                if (projectile.extraUpdates > 1)
                {
                    projectile.extraUpdates = 0;
                }
                if (projectile.numUpdates > 1)
                {
                    projectile.numUpdates = 0;
                }
            }
            if (projectile.type == ProjectileID.UFOMinion && projectile.localAI[0] > 0f)
            {
                projectile.localAI[0] -= 1f;
            }
            return false;
        }
        private static void SocialDistancing(ref Projectile projectile)
        {
            float pushBackAcceleration = 0.5f;
            float horizontalTolerance = projectile.width;
            if (projectile.type == ProjectileID.Tempest)
            {
                pushBackAcceleration = 0.1f;
                horizontalTolerance *= 2f;
            }
            for (int i = 0; i < 1000; i++)
            {
                if (i != projectile.whoAmI && Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[i].position.X) + Math.Abs(projectile.position.Y - Main.projectile[i].position.Y) < horizontalTolerance)
                {
                    if (projectile.position.X < Main.projectile[i].position.X)
                    {
                        projectile.velocity.X -= pushBackAcceleration;
                    }
                    else
                    {
                        projectile.velocity.X += pushBackAcceleration;
                    }
                    if (projectile.position.Y < Main.projectile[i].position.Y)
                    {
                        projectile.velocity.Y -= pushBackAcceleration;
                    }
                    else
                    {
                        projectile.velocity.Y += pushBackAcceleration;
                    }
                }
            }
        }
        bool UFOTeleportToTarget(ref Projectile projectile, ref Vector2 directionToTargetVec, ref Vector2 targetNPCPosition, ref float distToTarget)
        {
            directionToTargetVec = targetNPCPosition - Vector2.UnitY * 80f;
            int num23 = (int)directionToTargetVec.Y / 16;
            if (num23 < 0)
            {
                num23 = 0;
            }
            Tile tile = Main.tile[(int)directionToTargetVec.X / 16, num23];
            if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
            {
                directionToTargetVec += Vector2.UnitY * 16f;
                tile = Main.tile[(int)directionToTargetVec.X / 16, (int)directionToTargetVec.Y / 16];
                if (tile != null && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                {
                    directionToTargetVec += Vector2.UnitY * 16f;
                }
            }
            directionToTargetVec -= projectile.Center;
            distToTarget = directionToTargetVec.Length();
            directionToTargetVec = directionToTargetVec.SafeNormalize(Vector2.Zero);
            if (distToTarget > 300f && distToTarget <= 800f && projectile.localAI[0] == 0f)
            {
                projectile.ai[0] = 2f;
                projectile.ai[1] = (int)(distToTarget / 10f);
                projectile.extraUpdates = (int)projectile.ai[1];
                projectile.velocity = directionToTargetVec * 10f;
                projectile.localAI[0] = 60f;
                return true;
            }
            return false;
        }
        /// <summary>
        /// THIS IS FOR CELL UFO AND STUFF: YOU ARE PROBABLY LOOKING FOR GetAggroRangeBoost 
        /// </summary>
        private float GetAggroRange(Projectile projectile)
        {
            float aggroRangeAndAlsoDistToTarget;
            if (projectile.type == ProjectileID.UFOMinion)//idk why these are here
            {
                aggroRangeAndAlsoDistToTarget = 300f;
            }
            if (projectile.type == ProjectileID.StardustCellMinion)
            {
                aggroRangeAndAlsoDistToTarget = 300f;
            }
            aggroRangeAndAlsoDistToTarget = 2000f;
            if (projectile.type == ProjectileID.AbigailMinion)
            {
                aggroRangeAndAlsoDistToTarget = 700f;
            }

            return aggroRangeAndAlsoDistToTarget * prefixAggroRangeBoost;
        }
        bool StardustCellTeleport(ref Projectile projectile)
        {
            if (projectile.type == ProjectileID.StardustCellMinion)
            {
                if (projectile.ai[0] == 2f)//if on teleport mode
                {
                    projectile.ai[1] -= 1f;
                    projectile.tileCollide = false;
                    if (projectile.ai[1] > 3f)
                    {
                        if (projectile.numUpdates < 20)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Vortex)];
                                dust.noGravity = true;
                                dust.position = projectile.Center;
                                dust.velocity *= 3f;
                                dust.velocity += projectile.velocity * 3f;
                                dust.fadeIn = 1f;
                            }
                        }
                        float adjustDustFadeInBasedOnNumUpdates = 2f - projectile.numUpdates / 30f;
                        if (projectile.scale > 0f)
                        {
                            float dustPerLoop = 2f;
                            for (int j = 0; j < dustPerLoop; j++)
                            {
                                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Vortex)];
                                dust.noGravity = true;
                                dust.position = projectile.Center + Vector2.UnitY.RotatedBy((float)projectile.numUpdates * (MathF.PI / 30f) + (float)projectile.whoAmI * (MathF.PI / 4f) + MathF.PI / 2f) * (projectile.height / 2) - projectile.velocity * (j / dustPerLoop);
                                dust.velocity = projectile.velocity / 3f;
                                dust.fadeIn = adjustDustFadeInBasedOnNumUpdates / 2f;
                                dust.scale = adjustDustFadeInBasedOnNumUpdates;
                            }
                        }
                    }
                    if (projectile.ai[1] != 0f)
                    {
                        return true;
                    }
                    projectile.ai[1] = 30f;
                    projectile.ai[0] = 0f;
                    projectile.velocity /= 5f;
                    projectile.velocity.Y = 0f;
                    projectile.extraUpdates = 0;
                    projectile.numUpdates = 0;
                    projectile.netUpdate = true;
                    float dustAmount = 15f;
                    for (int i = 0; i < dustAmount; i++)
                    {
                        Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 229)];
                        dust.noGravity = true;
                        dust.position = projectile.Center - projectile.velocity * 5f;
                        dust.velocity *= 3f;
                        dust.velocity += projectile.velocity * 3f;
                        dust.fadeIn = 1f;
                        if (!Main.rand.NextBool(3))
                        {
                            dust.fadeIn = 2f;
                            dust.scale = 2f;
                            dust.velocity /= 8f;
                        }
                    }
                    for (int i = 0; (float)i < dustAmount; i++)
                    {
                        Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 229)];
                        dust.noGravity = true;
                        dust.position = projectile.Center;
                        dust.velocity *= 3f;
                        dust.velocity += projectile.velocity * 3f;
                        dust.fadeIn = 1f;
                        if (!Main.rand.NextBool(3))
                        {
                            dust.fadeIn = 2f;
                            dust.scale = 2f;
                            dust.velocity /= 8f;
                        }
                    }
                    projectile.extraUpdates = 0;
                    projectile.numUpdates = 0;
                }
                if (projectile.extraUpdates > 1)
                {
                    projectile.extraUpdates = 0;
                }
                if (projectile.numUpdates > 1)
                {
                    projectile.numUpdates = 0;
                }
            }
            return false;
        }
        void UFOMinionShootProjectile(Vector2 targetNPCPosition, ref Projectile projectile, int projTypeToShoot, float shotVelLength, int targetIndex)
        {
            if (Math.Abs((targetNPCPosition - projectile.Center).ToRotation() - MathF.PI / 2f) > MathF.PI / 4f)//this ensures they go above the target when shooting
            {
                projectile.velocity += (targetNPCPosition - projectile.Center - Vector2.UnitY * 80f).SafeNormalize(Vector2.Zero);
            }
            if (((targetNPCPosition - projectile.Center).Length() <= 400f) && projectile.ai[1] == 0f)
            {
                projectile.ai[1] += 1f * prefixAttackRate;
                if (Main.myPlayer == projectile.owner)
                {
                    Vector2 shotVelocity = targetNPCPosition - projectile.Center;
                    shotVelocity = shotVelocity.SafeNormalize(Vector2.Zero);
                    shotVelocity *= shotVelLength;
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, shotVelocity.X, shotVelocity.Y, projTypeToShoot, projectile.damage, projectile.knockBack, Main.myPlayer);
                    projectile.netUpdate = true;
                }
            }
        }
        void StardustCellShootProjectile(Vector2 targetNPCPosition, ref Projectile projectile, int projTypeToShoot, float shotVelLength, int targetIndex)
        {
            if ((targetNPCPosition - projectile.Center).Length() > 500f || projectile.ai[1] != 0f)
            {
                return;
            }
            projectile.ai[1] += 1f * prefixAttackRate;
            if (Main.myPlayer == projectile.owner)
            {
                Vector2 shotVel = targetNPCPosition - projectile.Center;
                shotVel = shotVel.SafeNormalize(Vector2.Zero);
                shotVel *= shotVelLength;
                int shotIndex = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, shotVel.X, shotVel.Y, projTypeToShoot, projectile.damage, projectile.knockBack, Main.myPlayer, 0f, targetIndex);
                Main.projectile[shotIndex].timeLeft = 300;
                Main.projectile[shotIndex].netUpdate = true;
                projectile.velocity -= ((shotVel / 3f) / prefixAttackVelocity) / prefixAttackRate;
                projectile.netUpdate = true;
            }
            StardustCellShotDustEffect(projectile, projectile.scale);
        }
        private static void StardustCellShotDustEffect(Projectile projectile, float scale)
        {
            for (int i = 0; i < 5; i++)
            {
                int num49 = projectile.width / 4;
                _ = ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * Main.rand.Next(24, 41) / 8f;
                int dustIndex = Dust.NewDust(projectile.Center - Vector2.One * num49, num49 * 2, num49 * 2, DustID.GemSapphire);
                Dust dust = Main.dust[dustIndex];
                Vector2 vector7 = (dust.position - projectile.Center).SafeNormalize(Vector2.Zero);
                dust.position = projectile.Center + vector7 * num49 * scale - new Vector2(4f);
                if (i < 30)
                {
                    dust.velocity = vector7 * dust.velocity.Length() * 2f;
                }
                else
                {
                    dust.velocity = 2f * vector7 * Main.rand.Next(45, 91) / 10f;
                }
                dust.noGravity = true;
                dust.scale = 0.7f + Main.rand.NextFloat();
            }
        }
        private void GetProjectileToShootAndShotVelLengthDependingOnMinionType(Projectile projectile, out float shotVelLength, out int projTypeToShoot)
        {
            switch (projectile.type)
            {
                case ProjectileID.Hornet:
                    shotVelLength = 10f * prefixAttackVelocity;
                    projTypeToShoot = ProjectileID.HornetStinger;
                    break;
                case ProjectileID.FlyingImp:
                    shotVelLength = 11f * prefixAttackVelocity;
                    projTypeToShoot = ProjectileID.ImpFireball;
                    break;
                case ProjectileID.Tempest:
                    shotVelLength = 20f * prefixAttackVelocity;
                    projTypeToShoot = ProjectileID.MiniSharkron;
                    break;
                case ProjectileID.UFOMinion:
                    shotVelLength = 4f;
                    projTypeToShoot = ProjectileID.UFOLaser;
                    break;
                case ProjectileID.StardustCellMinion:
                    shotVelLength = 14f * prefixAttackVelocity;
                    projTypeToShoot = ProjectileID.StardustCellMinionShot;
                    break;
                default:
                    shotVelLength = 0;
                    projTypeToShoot = 0;
                    break;
            }
        }
        private void HandleShootCooldown(ref Projectile projectile)
        {
            if (projectile.type == ProjectileID.Hornet)
            {
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] +=(float)Main.rand.Next(1, 4) * prefixAttackRate;
                }
                int attackDelay = 90;
                if (Main.player[projectile.owner].strongBees)//hive pack equiped
                {
                    attackDelay = 70;
                }
                if (projectile.ai[1] > attackDelay)
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
            }
            else if (projectile.type == ProjectileID.FlyingImp)
            {
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += 1f * prefixAttackRate;
                    if (Main.rand.NextBool(3))
                    {
                        projectile.ai[1] += 1f * prefixAttackRate;
                    }
                }
                if (projectile.ai[1] > 90f)
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
            }
            else if (projectile.type == ProjectileID.Tempest)
            {
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += 1f * prefixAttackRate;
                    if (!Main.rand.NextBool(3))
                    {
                        projectile.ai[1] += 1f * prefixAttackRate;
                    }
                }
                if (projectile.ai[1] > 50f)//attack cooldown
                {
                    projectile.ai[1] = 0f * prefixAttackRate;
                    projectile.netUpdate = true;
                }
            }
            else if (projectile.type == ProjectileID.UFOMinion)
            {
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += 1f * prefixAttackRate;
                    if (!Main.rand.NextBool(3))
                    {
                        projectile.ai[1] += 1f * prefixAttackRate;
                    }
                }
                if (projectile.ai[1] > 40f)//attack cooldown
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
            }
            else if (projectile.type == ProjectileID.StardustCellMinion)
            {
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] += 1f * prefixAttackRate;
                    if (!Main.rand.NextBool(3))
                    {
                        projectile.ai[1] += 1f * prefixAttackRate;
                    }
                }
                if (projectile.ai[1] > 60f)//cell attack threshhold
                {
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }
            }
        }
        private static void Imp_FaceTowardsTarget(ref Projectile projectile, ref int spriteDirection, ref int projdirection, ref Vector2 targetNPCPosition)
        {
            if (projectile.type == ProjectileID.FlyingImp)
            {
                if ((targetNPCPosition - projectile.Center).X > 0f)
                {
                    spriteDirection = (projdirection = -1);
                }
                else if ((targetNPCPosition - projectile.Center).X < 0f)
                {
                    spriteDirection = (projdirection = 1);
                }
            }
        }
        private void SustainTimeLeftWhilePlayerIsAliveAndAlsoSomeOtherAbigailThingsIdk(ref Projectile projectile, ref int projoriginalDamage, ref float[] localAI, ref float num, ref float num2, ref float num3, ref float num4, ref float num5)
        {
            if (projectile.type == ProjectileID.AbigailMinion)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].abigailMinion = false;
                }
                if (Main.player[projectile.owner].abigailMinion)
                {
                    projectile.timeLeft = 2;
                }
                projoriginalDamage = Main.player[projectile.owner].highestAbigailCounterOriginalDamage;
                num2 = Main.player[projectile.owner].ownedProjectileCounts[970] - 1;
                num3 = Math.Max(4f, 18f - num2 * 1.75f);
                num = 1.4f;
                num4 = Math.Max(5f, num4 - num2 * 4f);
                num5 = Math.Min(1f, num5 + num2 * 0.03f);
            }
            if (projectile.type == ProjectileID.Hornet)
            {

                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].hornetMinion = false;
                }
                if (Main.player[projectile.owner].hornetMinion)
                {
                    projectile.timeLeft = 2;
                }
            }
            if (projectile.type == ProjectileID.FlyingImp)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].impMinion = false;
                }
                if (Main.player[projectile.owner].impMinion)
                {
                    projectile.timeLeft = 2;
                }
            }
            if (projectile.type == ProjectileID.Tempest)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].sharknadoMinion = false;
                }
                if (Main.player[projectile.owner].sharknadoMinion)
                {
                    projectile.timeLeft = 2;
                }
            }
            if (projectile.type == ProjectileID.UFOMinion)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].UFOMinion = false;
                }
                if (Main.player[projectile.owner].UFOMinion)
                {
                    projectile.timeLeft = 2;
                }
            }
            if (projectile.type == ProjectileID.StardustCellMinion)
            {
                if (Main.player[projectile.owner].dead)
                {
                    Main.player[projectile.owner].stardustMinion = false;
                }
                if (Main.player[projectile.owner].stardustMinion)
                {
                    projectile.timeLeft = 2;
                }
                Lighting.AddLight(projectile.Center, 0.2f, 0.6f, 0.7f);
                if (localAI[1] > 0f)
                {
                    localAI[1] -= 1f;
                }
            }
        }
        private static void HandleMinionVisualRotationDirectionFrameAndDust(ref Projectile projectile, ref int projspriteDirection, ref float projrotation, ref bool netUpdate, ref float[] ai, ref int projframeCounter, ref int projframe, ref int projdirection, ref bool hasTarget)
        {
            projrotation = projectile.velocity.X * 0.05f;
            projframeCounter++;
            if (projectile.type == ProjectileID.Hornet)
            {
                if (projframeCounter > 1)
                {
                    projframe++;
                    projframeCounter = 0;
                }
                if (projframe > 2)
                {
                    projframe = 0;
                }
            }
            if (projectile.type == ProjectileID.AbigailMinion)
            {
                if (ai[0] >= 2f)
                {
                    int animSpeed = 12;
                    if (projframe < 8)
                    {
                        projframeCounter = 0;
                    }
                    projframeCounter++;
                    projframe = projframeCounter / animSpeed;
                    if (projframe > 6)
                    {
                        projframeCounter = 0;
                        projframe = 5;
                    }
                    switch (projframe)
                    {
                        case 0:
                        case 1:
                        case 2:
                            projframe = 8 + projframe;
                            break;
                        case 3:
                        case 5:
                        case 7:
                            projframe = 11;
                            break;
                        default:
                            projframe = 12;
                            break;
                    }
                    if (Main.rand.NextBool(2))
                    {
                        float dustVelX = 1.1f + Main.rand.NextFloat() * 0.3f;
                        float dustVelY = 1.4f + Main.rand.NextFloat() * 0.4f;
                        Vector2 dustVel = Main.rand.NextVector2CircularEdge((float)projectile.width * dustVelX, (0f - (float)projectile.height) * 0.25f * dustVelX);
                        float num38 = dustVel.ToRotation() + (float)Math.PI / 2f;
                        int dustIndex = Dust.NewDust(projectile.Bottom + dustVel, 1, 1, 303, 0f, 0f, 50, Color.GhostWhite, dustVelY);
                        Main.dust[dustIndex].velocity = dustVel * 0.0125f + new Vector2(1f, 0f).RotatedBy(num38, Vector2.Zero);
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
                else
                {
                    if (projframe > 7)
                    {
                        projframe = 0;
                        projframeCounter = 0;
                    }
                    if (projframeCounter > 6)
                    {
                        projframeCounter = 0;
                        projframe++;
                        if (projframe > 7)
                        {
                            projframe = 0;
                        }
                    }
                }
            }
            if (projectile.type == ProjectileID.FlyingImp)
            {
                if (projframeCounter >= 16)
                {
                    projframeCounter = 0;
                }
                projframe = projframeCounter / 4;
                if (ai[1] > 0f && ai[1] < 16f)
                {
                    projframe += 4;
                }
                if (Main.rand.NextBool(6))
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 0.3f;
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].noLight = true;
                }
            }
            if (projectile.type == ProjectileID.Tempest)
            {
                int frameSpeed = 2;
                if (projframeCounter >= 6 * frameSpeed)
                {
                    projframeCounter = 0;
                }
                projframe = projframeCounter / frameSpeed;
                if (Main.rand.NextBool(5))
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 217, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[dustIndex].velocity *= 0.3f;
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].noLight = true;
                }
            }
            if (projectile.type == ProjectileID.UFOMinion || projectile.type == ProjectileID.StardustCellMinion)
            {
                int num43 = 3;
                if (projframeCounter >= 4 * num43)
                {
                    projframeCounter = 0;
                }
                projframe = projframeCounter / num43;
            }
            if (projectile.velocity.X > 0f)
            {
                projspriteDirection = (projdirection = -1);
            }
            else if (projectile.velocity.X < 0f)
            {
                projspriteDirection = (projdirection = 1);
            }
            if (projectile.type == ProjectileID.AbigailMinion)
            {
                projspriteDirection = (projdirection *= -1);
                if (!hasTarget && Math.Abs(projectile.velocity.X) < 0.1f)
                {
                    projspriteDirection = ((Main.player[projectile.owner].Center.X > projectile.Center.X) ? 1 : (-1));
                }
            }
        }
    }
}
