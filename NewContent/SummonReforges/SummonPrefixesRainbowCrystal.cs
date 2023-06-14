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

		void RainbowCrystalStaffAIExplosion(Projectile projectile)
        {
            if (projectile.damage <= 0)
                projectile.damage = 1;
            Color myColor = Main.hslToRgb(projectile.ai[0], 1f, 0.5f);
            int indexOfCrystalThatSpawnedMe = (int)projectile.ai[1];
            if (indexOfCrystalThatSpawnedMe < 0 || indexOfCrystalThatSpawnedMe >= 1000 || (!Main.projectile[indexOfCrystalThatSpawnedMe].active && Main.projectile[indexOfCrystalThatSpawnedMe].type != 643))
            {
                projectile.ai[1] = -1f;
            }
            else
            {
                DelegateMethods.v3_1 = myColor.ToVector3() * 0.5f;
                Utils.PlotTileLine(projectile.Center, Main.projectile[indexOfCrystalThatSpawnedMe].Center, 8f, DelegateMethods.CastLight);
            }
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = Main.rand.NextFloat() * 0.8f + 0.8f;
                projectile.direction = ((Main.rand.Next(2) > 0) ? 1 : (-1));
            }
            projectile.rotation = projectile.localAI[1] / 40f * MathF.Tau * (float)projectile.direction;
            projectile.alpha = projectile.alpha > 0 ? projectile.alpha - (int)MathF.Round(8f * GetAttackRateAsTimerIncrease()) : 0;
            if (projectile.alpha == 0)
                Lighting.AddLight(projectile.Center, myColor.ToVector3() * 0.5f);
            RainbowCrystalExplosionPassiveDustEffects(projectile, myColor);
            projectile.scale = projectile.Opacity / 2f * projectile.localAI[0];
            projectile.velocity = Vector2.Zero;
            projectile.localAI[1] += GetAttackRateAsTimerIncrease();

            if (projectile.localAI[1] >= 30 && projectile.localAI[2] == 0 || projectile.localAI[1] >= 60)
            {
                DoRainbowCrystalStaffExplosionDust(projectile);
                if (Main.myPlayer == projectile.owner)//THIS SETS ITS SIZE TO BE BIGGER, ACTIVATES DAMAGE, THEN SHRINKS BACK TO NORMAL SIZE
                {
                    projectile.friendly = true;
                    int projWidthBuffer = projectile.width;
                    int projHeightBuffer = projectile.height;
                    int projPierceBuffer = projectile.penetrate;
                    projectile.position = projectile.Center;
                    projectile.width = (projectile.height = (int)(62 * GetAttackVelocity()));//EXPLOSION SIZE, TODO: LIST IN TOOLTIP THAT VELOCITY INCREASES EXPLOSION SIZE
                    projectile.Center = projectile.position;
                    projectile.penetrate = -1;
                    projectile.maxPenetrate = -1;
                    //Debug.CircleDustVIsualizer(projectile.Center,projectile.width);//DEBUG, DELETE THIS LATER, THIS SHOWS EXPLOSION HITBOX
                    projectile.Damage();
                    projectile.penetrate = projPierceBuffer;
                    projectile.position = projectile.Center;
                    projectile.width = projWidthBuffer;
                    projectile.height = projHeightBuffer;
                    projectile.Center = projectile.position;
                    projectile.friendly = false;
                    projectile.localAI[2] = 1;
                }
            }
            if (projectile.localAI[1] >= 60f)
            {
                projectile.Kill();
            }
        }

        private static void RainbowCrystalExplosionPassiveDustEffects(Projectile projectile, Color myColor)
        {
            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.NextBool(10))
                {
                    Vector2 vector134 = Vector2.UnitY.RotatedBy(i * MathF.PI).RotatedBy(projectile.rotation);
                    Dust dust43 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.RainbowMk2, 0f, 0f, 225, myColor, 1.5f)];
                    dust43.noGravity = true;
                    dust43.noLight = true;
                    dust43.scale = projectile.Opacity * projectile.localAI[0];
                    dust43.position = projectile.Center;
                    dust43.velocity = vector134 * 2.5f;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.NextBool(10))
                {
                    Vector2 dustVel = Vector2.UnitY.RotatedBy(i * MathF.PI);
                    Dust dust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.RainbowMk2, 0f, 0f, 225, myColor, 1.5f)];
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.scale = projectile.Opacity * projectile.localAI[0];
                    dust.position = projectile.Center;
                    dust.velocity = dustVel * 2.5f;
                }
            }
            if (Main.rand.NextBool(10))
            {
                float dustVelLength = 1f + Main.rand.NextFloat() * 2f;
                float fadeIn = 1f + Main.rand.NextFloat();
                float dustScale = 1f + Main.rand.NextFloat();
                Vector2 vector136 = Utils.RandomVector2(Main.rand, -1f, 1f);
                if (vector136 != Vector2.Zero)
                {
                    vector136.Normalize();
                }
                vector136 *= 20f + Main.rand.NextFloat() * 100f;
                Vector2 vec = projectile.Center + vector136;
                Point point3 = vec.ToTileCoordinates();
                bool flag51 = true;
                if (!WorldGen.InWorld(point3.X, point3.Y))
                {
                    flag51 = false;
                }
                if (flag51 && WorldGen.SolidTile(point3.X, point3.Y))
                {
                    flag51 = false;
                }
                if (flag51)
                {
                    Dust dust45 = Main.dust[Dust.NewDust(vec, 0, 0, DustID.RainbowMk2, 0f, 0f, 127, myColor)];
                    dust45.noGravity = true;
                    dust45.position = vec;
                    dust45.velocity = -Vector2.UnitY * dustVelLength * (Main.rand.NextFloat() * 0.9f + 1.6f);
                    dust45.fadeIn = fadeIn;
                    dust45.scale = dustScale;
                    dust45.noLight = true;
                    if (dust45.dustIndex != 6000)
                    {
                        Dust dust46 = Dust.CloneDust(dust45);
                        Dust dust2 = dust46;
                        dust2.scale *= 0.75f;
                        dust2 = dust46;
                        dust2.fadeIn *= 0.65f;
                        dust46.color = new Color(255, 255, 255, 255);
                    }
                }
            }
        }

        void RainbowCrystalStaffAI(Projectile projectile)
		{

			float aggroRange = 1000f * GetAggroRangeBoost();
			projectile.velocity = Vector2.Zero;
			projectile.alpha = projectile.alpha <= 0 ? 0 : projectile.alpha - (int)MathF.Ceiling(5f * GetAttackRateAsTimerIncrease());
			if (projectile.direction == 0)
			{
				projectile.direction = Main.player[projectile.owner].direction;
			}
			projectile.frameCounter++;
			projectile.frame = projectile.frameCounter / 3 % Main.projFrames[projectile.type];
			if (projectile.alpha == 0 && Main.rand.NextBool(15))
			{
				Dust dust = Main.dust[Dust.NewDust(projectile.Top, 0, 0, DustID.AncientLight, 0f, 0f, 100)];
				dust.velocity.X = 0f;
				dust.noGravity = true;
				dust.fadeIn = 1f;
				dust.position = projectile.Center + Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (4f * Main.rand.NextFloat() + 26f);
				dust.scale = 0.5f;
			}
			projectile.localAI[0] += 0;//???
			if (projectile.localAI[0] >= 60f)
			{
				projectile.localAI[0] = 0f;
			}

			if (projectile.ai[0] < 0f)
				projectile.ai[0] += GetAttackRateAsTimerIncrease();

			if (projectile.ai[0] == 0)         
                if(RainbowCrystalFindTarget(projectile, aggroRange))//todo: make it only detect if there is any possible taget,
                    return;                                         //then actually search for them when it's going to shoot
            if (projectile.ai[0] <= 0f)			                    //use an out int[] for the targets
				return;		                                        //use an afterimage register-like system with a reverse for loop ending at 1 and not 0
			int targetIndex = (int)projectile.ai[1];                //initialize array as -1
			if (!Main.npc[targetIndex].CanBeChasedBy(this))
			{
				projectile.ai[0] = 0f;
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
				return;
			}
			projectile.ai[0]++;
			float num959 = 5f;
			if (projectile.ai[0] < num959)	
				return;
			
			Vector2 directionToTargetVec = projectile.DirectionTo(Main.npc[targetIndex].Center);
			if (directionToTargetVec.HasNaNs())
			{
				directionToTargetVec = Vector2.UnitY;
			}
			int projDir = ((directionToTargetVec.X > 0f) ? 1 : (-1));
			projectile.direction = projDir;
			projectile.ai[0] = -20f;
			projectile.netUpdate = true;
			if (projectile.owner != Main.myPlayer)
			{
				return;
			}
            NPC npc = Main.npc[targetIndex];
            Vector2 toTarget = (npc.Center - projectile.Center);
            for (int i = 0; i < 3; i++)
			{

                Vector2 explosionSpawnPos = projectile.Center + toTarget;
				Vector2 predictiveOffset = npc.velocity * 30f * GetAttackRateAsTimerThresholdMultiplier();
				explosionSpawnPos += predictiveOffset;
                if (i > 0)
                {
                    Vector2 randomVec = Main.rand.NextVector2Circular(210, 210);//THIS IS THE ACCURACY
                    explosionSpawnPos = projectile.Center + randomVec + toTarget + predictiveOffset;
                }
                //if (Main.tile[explosionSpawnPos.ToTileCoordinates()].HasUnactuatedTile) 
                //{
                //    Vector2 retractionDir = Vector2.Normalize(toTarget);
                //    for (int j = 0; j < 1000000; j += 8)//UNTESTED
                //    {
                //        explosionSpawnPos -= retractionDir;
                //        if (!Main.tile[explosionSpawnPos.ToTileCoordinates()].HasUnactuatedTile)
                //            break;
                //    }
                //}
                float explosionColor = Main.rgbToHsl(Main.DiscoColor).X;
				Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), explosionSpawnPos, Vector2.Zero, ProjectileID.RainbowCrystalExplosion, projectile.damage, projectile.knockBack, projectile.owner, explosionColor, projectile.whoAmI);
			}

		}
        //THIS IWN'T WORKING I THINK
        void RainbowCrystalFindTargets(Projectile projectile, float aggroRange, out int[] targetsFound)//attempt at multiple target search, test later
        {
            targetsFound = new int[]{ -1, -1, -1 };
            float distToClosestSQ = aggroRange * aggroRange;
            NPC ownerMinionAttackTarget = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTarget != null && ownerMinionAttackTarget.CanBeChasedBy(this))
            {
                float distToTargetSQ = projectile.DistanceSQ(ownerMinionAttackTarget.Center);
                if (distToTargetSQ < distToClosestSQ && Collision.CanHitLine(projectile.Center, 0, 0, ownerMinionAttackTarget.Center, 0, 0))
                {
                    distToClosestSQ = distToTargetSQ;
                    targetsFound = new int[] { ownerMinionAttackTarget.whoAmI, ownerMinionAttackTarget.whoAmI, ownerMinionAttackTarget.whoAmI };
                }
            }
            if (targetsFound[0] < 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(this))
                    {
                        float distToTarget = projectile.DistanceSQ(npc.Center);
                        if (distToTarget < distToClosestSQ && Collision.CanHitLine(projectile.Center, 0, 0, npc.Center, 0, 0))
                        {
                            distToClosestSQ = distToTarget;
                            PushIntsDownAndUpdateFirst(ref targetsFound, i);
                        }
                    }
                }
            }
            for (int i = 0; i < targetsFound.Length; i++)
            {
                if (targetsFound[i] == -1)
                    targetsFound[i] = targetsFound[0];
            }
        }
        static void PushIntsDownAndUpdateFirst(ref int[]  array, int newFirst)
        {
            for (int i = array.Length - 1; i >= 1; i--)
            {
                array[i] = array[i - 1];
            }
            array[0] = newFirst;
        }
        private bool RainbowCrystalFindTarget(Projectile projectile, float aggroRange)
        {
            int potentialTargetIndex = -1;
            float distToClosest = aggroRange;
            NPC ownerMinionAttackTarget = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTarget != null && ownerMinionAttackTarget.CanBeChasedBy(this))
            {
                float distToTarget = projectile.Distance(ownerMinionAttackTarget.Center);
                if (distToTarget < distToClosest && Collision.CanHitLine(projectile.Center, 0, 0, ownerMinionAttackTarget.Center, 0, 0))
                {
                    distToClosest = distToTarget;
                    potentialTargetIndex = ownerMinionAttackTarget.whoAmI;
                }
            }
            if (potentialTargetIndex < 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(this))
                    {
                        float distToTarget = projectile.Distance(npc.Center);
                        if (distToTarget < distToClosest && Collision.CanHitLine(projectile.Center, 0, 0, npc.Center, 0, 0))
                        {
                            distToClosest = distToTarget;
                            potentialTargetIndex = i;
                        }
                    }
                }
            }
            if (potentialTargetIndex != -1)
            {
                projectile.ai[0] = 1f;
                projectile.ai[1] = potentialTargetIndex;
                projectile.netUpdate = true;
                return true;
            }
            return false;
        }

        private void DoRainbowCrystalStaffExplosionDust(Projectile projectile)//TODO: EDIT DUST TO FIT EXPLOSION SIZE ALSO MAYBE JUST CHANGE THE DUST PATTERN
		{
			Vector2 spinningpoint = new Vector2(0f, -3f).RotatedByRandom(MathF.PI);
			float dustAmount = Main.rand.Next(7, 13) * GetAttackVelocity() * GetAttackVelocity();
			Vector2 dustVel = new Vector2(2.1f, 2f) * GetAttackVelocity();
			Color newColor = Main.hslToRgb(projectile.ai[0], 1f, 0.5f);
			newColor.A = byte.MaxValue;
			for (float i = 0f; i < dustAmount; i += 1f)
			{
				int num3 = Dust.NewDust(projectile.Center, 0, 0, DustID.RainbowMk2, 0f, 0f, 0, newColor);
				Main.dust[num3].position = projectile.Center;
				Main.dust[num3].velocity = spinningpoint.RotatedBy(MathF.Tau * i / dustAmount) * dustVel * (0.8f + Main.rand.NextFloat() * 0.4f);
				Main.dust[num3].noGravity = true;
				Main.dust[num3].scale = 2f;
				Main.dust[num3].fadeIn = Main.rand.NextFloat() * 2f;
				if (num3 != 6000)
				{
					Dust dust = Dust.CloneDust(num3);
					dust.scale /= 2f;
					dust.fadeIn /= 2f;
					dust.color = new Color(255, 255, 255, 255);
				}
			}
			for (float i = 0f; i < dustAmount; i += 1f)
			{
				int dustIndex = Dust.NewDust(projectile.Center, 0, 0, DustID.RainbowMk2, 0f, 0f, 0, newColor);
				Main.dust[dustIndex].position = projectile.Center;
				Main.dust[dustIndex].velocity = spinningpoint.RotatedBy((float)Math.PI * 2f * i / dustAmount) * dustVel * (0.8f + Main.rand.NextFloat() * 0.4f);
				Main.dust[dustIndex].velocity *= Main.rand.NextFloat() * 0.8f;
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].scale = Main.rand.NextFloat() * 1f;
				Main.dust[dustIndex].fadeIn = Main.rand.NextFloat() * 2f;
				if (dustIndex != 6000)
				{
					Dust dust2 = Dust.CloneDust(dustIndex);
					dust2.scale /= 2f;
					dust2.fadeIn /= 2f;
					dust2.color = new Color(255, 255, 255, 255);
				}
			}
		}
		void DrawRainbowCrystalExplosion(Projectile projectile)
        {
			Vector2 drawPos = projectile.position + projectile.Size / 2 + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
			Rectangle frame = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
			Color whiteColor = projectile.GetAlpha(Lighting.GetColor(projectile.Center.ToTileCoordinates()));
			Color drawColor = Main.hslToRgb(projectile.ai[0], 1f, 0.5f).MultiplyRGBA(new Color(255, 255, 255, 0));
            float drawScale = projectile.scale * GetAttackVelocity();
			if (projectile.localAI[0] != 0)//solve a weird 1 frame fulbright draw call issue
			{
				Main.EntitySpriteDraw(texture, drawPos, frame, drawColor, projectile.rotation, texture.Size() / 2, drawScale * 2f, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, drawPos, frame, drawColor, 0f, texture.Size() / 2, drawScale * 2f, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, drawPos, frame, whiteColor, projectile.rotation, texture.Size() / 2, drawScale, SpriteEffects.None);
				Main.EntitySpriteDraw(texture, drawPos, frame, whiteColor, 0f, texture.Size() / 2, drawScale, SpriteEffects.None);
			}
		
			if (projectile.ai[1] != -1f && projectile.Opacity > 0.3f)
			{
				Vector2 toParentCrystlNormalized = Main.projectile[(int)projectile.ai[1]].Center.DirectionTo(projectile.Center);
				Vector2 toParentCrystal = Main.projectile[(int)projectile.ai[1]].Center - projectile.Center;
				Vector2 scale = new Vector2(1.6f, (toParentCrystal.Length() - 64) / (float)(texture.Height * 0.6f));
				float projRotation = toParentCrystal.ToRotation() + (float)Math.PI / 2f;
				float brightness = MathHelper.Distance(30f, projectile.localAI[1]) / 20f;
				brightness = MathHelper.Clamp(brightness, 0f, 1f);
				if (brightness > 0f)
				{
					Main.EntitySpriteDraw(texture, drawPos + toParentCrystal / 2f + toParentCrystlNormalized * 8, frame, drawColor * brightness, projRotation, texture.Size() / 2, scale, SpriteEffects.None);
					Main.EntitySpriteDraw(texture, drawPos + toParentCrystal / 2f + toParentCrystlNormalized * 8, frame, whiteColor * brightness, projRotation, texture.Size() / 2, scale / 2f, SpriteEffects.None);
				}
			}

		}

		

	}
}
