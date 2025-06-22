using System;
using TRAEProject.Common; 
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using System.Collections.Generic;
using TRAEProject.Changes;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;
namespace TRAEProject
{
    public static class TRAEMethods
    {
        /// <summary>
        /// overrde the texture of something you're gonna copy a vanilla texture for, then use DrawSelfFullbright and pass the projectile ID of the vnilla projecile.
        /// This avoids bloating folders with images.
        /// public override string TRAEMethods.blankTexture;
        /// </summary>
        public const string blankTexture = "TRAEProject/NewContent/NPCs/Underworld/CheapWayToSpawnARedDevil";
        /// <summary>
        /// Eliminates the need for a glowmask texture. CALL THIS IN PREDRAW AND NOT AI
        /// Alwys returns false, so you can just do "return TRAEMethods.DrawSelfFullbright(Projectile);"
        /// This methods assumes that the projectile spritesheet is vertical
        /// </summary>
        /// <param name="projectile">the projectile to be drawn</param>
        /// <param name="texture"> if null, will use the projectile texture</param>
        /// <returns></returns>
        public static bool DrawSelfFullbright(Projectile projectile, Texture2D texture = null)
        {
            if(texture == null)
            {
                Main.instance.LoadProjectile(projectile.type);
                texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            }
            Rectangle frame = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
            Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), frame, Color.White, projectile.rotation, frame.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }
        /// <summary>
        /// Eliminates the need for a glowmask texture. CALL THIS IN PREDRAW AND NOT AI
        /// Alwys returns false, so you can just do "return TRAEMethods.DrawSelfFullbright(Projectile, projectileToCopy);"
        /// This methods assumes that the projectile spritesheet is vertical
        /// </summary>
        /// <param name="projectile">The projectile you are calling this one</param>
        /// <param name="projectileTypeToCopy">a vanilla projectile you want to copy the texture of</param>
        /// <returns></returns>
        public static bool DrawSelfFullbright(Projectile projectile, int projectileTypeToCopy)
        {
            Main.instance.LoadProjectile(projectileTypeToCopy);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[projectileTypeToCopy].Value;
            Rectangle frame = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
            Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), frame, Color.White, projectile.rotation, frame.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }
        public static bool DrawSelfFullbright(NPC npc, Texture2D texture = null)
        {
            if (texture == null)
            {
                Main.instance.LoadProjectile(npc.type);
                texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
            }
            Main.EntitySpriteDraw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
            return false;
        }
        public static void UseManaOverloadable(this Player player, int amount)
        {
            int overloadedManaLoss = Math.Min(player.GetModPlayer<Mana>().overloadedMana, amount);
            player.GetModPlayer<Mana>().overloadedMana -= overloadedManaLoss;
            int useMana = amount - overloadedManaLoss;
            player.statMana -= useMana;
        }
        public static void SpawnProjectilesFromAbove(Player Player, Vector2 Base, int projectileCount, int spreadX, int spreadY, int[] offsetCenter, float velocity, int type, int damage, float knockback, int player)
        {
            for (int i = 0; i < projectileCount; ++i)
            {
                // where the projectile spawns
                float x2 = Base.X  + Main.rand.Next(-spreadX, spreadX);
                float y = Base.Y - Main.rand.Next((int)(spreadY * 0.8), (int)(spreadY * 1.2));
                ///
                //Calculate Velocity
                Vector2 vector17 = new Vector2(x2, y);
                float X = Base.X - vector17.X;
                float Y = Base.Y + (Main.rand.Next(offsetCenter) * 100) - vector17.Y;
                float squareRoot =  MathF.Sqrt(X * X + Y * Y);
                squareRoot = velocity / squareRoot;
                X *= squareRoot;
                Y *= squareRoot;
                ///
                // Spawn the projectile
                int Projectile = Terraria.Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), x2, y, X, Y, type, damage, knockback, player);
                // once the projectile reaches the base's position, it will no longer go through tiles.
                Main.projectile[Projectile].localAI[1] += Base.Y;
           
            }
            return;
        }
        public static void Explode(Projectile projectile, int ExplosionRadius) // Doesn't set any special effects
        {
            projectile.GetGlobalProjectile<ProjectileStats>().explodes = false; // without this, the projectile will keep exploding infinitely
            projectile.damage = (int)(projectile.damage * projectile.GetGlobalProjectile<ProjectileStats>().ExplosionDamage);
            projectile.timeLeft = 3; // Explosion will stay active for 3 frames          
            projectile.alpha = 255; // Projectile becomes invisible
            projectile.tileCollide = false; // if set to true, the explosion won't go through blocks (and probably be messed up)
            // Adjust the explosion's hitbox so that it spawns at the center of the projectile.
            projectile.position.X += projectile.width / 2;
            projectile.position.Y += projectile.height / 2;
            projectile.width = projectile.height = ExplosionRadius;
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;
            projectile.position.X += projectile.width / 2;
            projectile.position.Y += projectile.height / 2;
            projectile.width = projectile.height = ExplosionRadius;
            projectile.position.X -= (projectile.width / 2);
            projectile.position.Y -= (projectile.height / 2);
        }
        public static void DefaultExplosion(Projectile projectile)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);
            for (int num731 = 0; num731 < 30; ++num731)
            {
                int num732 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default, 2f);
                Dust dust = Main.dust[num732];
                dust.velocity *= 2f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num732].scale = 0.5f;
                    Main.dust[num732].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num733 = 0; num733 < 30; ++num733)
            {
                int num734 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default, 3f);
                Main.dust[num734].noGravity = true;
                Dust dust = Main.dust[num734];
                dust.velocity *= 4f;
                num734 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default, 2f);
                dust = Main.dust[num734];
                dust.velocity *= 2f;
            }
        }
        public static Vector2 PolarVector(float radius, float theta)
        {
            return new Vector2(MathF.Cos(theta), MathF.Sin(theta)) * radius;
        }
        /// <summary>
        /// give an angle to shoot at to attempt to hit a moving target, returns NaN when this is impossible
        /// </summary>
        public static float PredictiveAim(Vector2 shootFrom, float shootSpeed, Vector2 targetPos, Vector2 targetVelocity, out float travelTime)
        {
            float angleToTarget = (targetPos - shootFrom).ToRotation();
            float targetTraj = targetVelocity.ToRotation();
            float targetSpeed = targetVelocity.Length();
            float dist = (targetPos - shootFrom).Length();

            //imagine a tirangle between the shooter, its target and where it think the target will be in the future
            // we need to find an angle in the triangle z this is the angle located at the target's corner
            float z = MathF.PI + (targetTraj - angleToTarget);

            //with this angle z we can now use the law of cosines to find time
            //the side opposite of z is equal to shootSpeed * time
            //the other sides are dist and targetSpeed * time
            // putting these values into law of cosines gets (shootSpeed * time)^2 = (targetSpeed * time)^2 + dist^2 -2*targetSpeed*time*cos(z)
            //we can rearange it to (shootSpeed^2 - targetSpeed^2)time^2 + 2*targetSpeed*dist*cos(z)*time - dist^2 = 0, this is a quadratic!

            //here we use the quadratic formula to find time
            float a = shootSpeed * shootSpeed - targetSpeed * targetSpeed;
            float b = 2 * targetSpeed * dist *  MathF.Cos(z);
            float c = -(dist * dist);
            float time = (-b +  MathF.Sqrt(b * b - 4 * a * c)) / (2 * a);

            //we now know the time allowing use to find all sides of the tirangle, now we use law of Sines to calculate the angle to shoot at.
            float calculatedShootAngle = angleToTarget -  MathF.Asin((targetSpeed * time *  MathF.Sin(z)) / (shootSpeed * time));
            travelTime = time;
            return calculatedShootAngle;
        }
        public delegate bool SpecialCondition(NPC possibleTarget);

        //used for homing projectile
        public static bool ClosestNPC(ref NPC target, float maxDistance, Vector2 position, bool ignoreTiles = false, int overrideTarget = -1, SpecialCondition specialCondition = null, int IgnoreThisOne = -1)
        {
            //very advance users can use a delegate to insert special condition into the function like only targetting enemies not currently having local iFrames, but if a special condition isn't added then just return it true
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }

            bool foundTarget = false;
            //If you want to prioritse a certain target this is where it's processed, mostly used by minions that haave a target priority
            if (overrideTarget != -1)
            {
                if ((Main.npc[overrideTarget].Center - position).Length() < maxDistance && !Main.npc[overrideTarget].immortal && (Collision.CanHit(position, 0, 0, Main.npc[overrideTarget].Center, 0, 0) || ignoreTiles) && specialCondition(Main.npc[overrideTarget]))
                {
                    target = Main.npc[overrideTarget];
                    return true;
                }
            }
            //this is the meat of the targetting logic, it loops through every NPC to check if it is valid the miniomum distance and target selected are updated so that the closest valid NPC is selected
            for (int k = 0; k < Main.npc.Length; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();

                if (distance < maxDistance  && possibleTarget.whoAmI != IgnoreThisOne &&  possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0) || ignoreTiles) && specialCondition(possibleTarget))
                {
                    target = Main.npc[k];
                    foundTarget = true;

                    maxDistance = (target.Center - position).Length();
                }
            }
            return foundTarget;
        }
        public static bool NPCsInRange(out List<NPC> targets, float maxDistance, Vector2 position, bool ignoreTiles = false, SpecialCondition specialCondition = null)
        {
            //very advance users can use a delegate to insert special condition into the function like only targetting enemies not currently having local iFrames, but if a special condition isn't added then just return it true
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }
            bool foundTarget = false;
            targets = new List<NPC>();
            for (int k = 0; k < Main.npc.Length; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();
                if (distance < maxDistance && possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0) || ignoreTiles) && specialCondition(possibleTarget))
                {
                    targets.Add(possibleTarget);
                    foundTarget = true;
                }
            }
            return foundTarget;
        }
        public static float AngularDifference(float angle1, float angle2)
        {
            angle1 = PolarVector(1f, angle1).ToRotation();
            angle2 = PolarVector(1f, angle2).ToRotation();
            if (Math.Abs(angle1 - angle2) > Math.PI)
            {
                return  MathF.PI * 2 - Math.Abs(angle1 - angle2);
            }
            return Math.Abs(angle1 - angle2);
        }
        public static void SlowRotation(this ref float currentRotation, float targetAngle, float speed)
        {
            int f = 1; //this is used to switch rotation direction
            float actDirection = new Vector2( MathF.Cos(currentRotation),  MathF.Sin(currentRotation)).ToRotation();
            targetAngle = new Vector2( MathF.Cos(targetAngle),  MathF.Sin(targetAngle)).ToRotation();

            //this makes f 1 or -1 to rotate the shorter distance
            if (Math.Abs(actDirection - targetAngle) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }

            if (actDirection <= targetAngle + speed * 2 && actDirection >= targetAngle - speed * 2)
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += speed * f;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= speed * f;
            }
            actDirection = new Vector2( MathF.Cos(actDirection),  MathF.Sin(actDirection)).ToRotation();
            currentRotation = actDirection;
        }
        public static float PredictiveAimWithOffset(Vector2 shootFrom, float shootSpeed, Vector2 targetPos, Vector2 targetVelocity, float shootOffset)
        {
            float angleToTarget = (targetPos - shootFrom).ToRotation();
            float targetTraj = targetVelocity.ToRotation();
            float targetSpeed = targetVelocity.Length();
            float dist = (targetPos - shootFrom).Length();
            if (dist < shootOffset)
            {
                shootOffset = 0;
            }

            //imagine a tirangle between the shooter, its target and where it think the target will be in the future
            // we need to find an angle in the triangle z this is the angle located at the target's corner
            float z =  MathF.PI + (targetTraj - angleToTarget);

            //with this angle z we can now use the law of cosines to find time
            //the side opposite of z is equal to shootSpeed * time
            //the other sides are dist and targetSpeed * time
            //putting these values into law of cosines gets (shootSpeed * time + shootOffset)^2 = (targetSpeed * time)^2 + dist^2 -2*targetSpeed*time*cos(z)
            //we can rearange it to (shootSpeed^2 - targetSpeed^2)time^2 + (2*targetSpeed*dist*cos(z) + 2*shootOffest*shootSpeed)*time + shootOffset^2 - dist^2 = 0, this is a quadratic!

            //here we use the quadratic formula to find time
            float a = shootSpeed * shootSpeed - targetSpeed * targetSpeed;
            float b = 2 * targetSpeed * dist *  MathF.Cos(z) + 2 * shootOffset * shootSpeed;
            float c = (shootOffset * shootOffset) - (dist * dist);
            float time = (-b +  MathF.Sqrt(b * b - 4 * a * c)) / (2 * a);

            //we now know the time allowing use to find all sides of the tirangle, now we use law of Sines to calculate the angle to shoot at.
            float calculatedShootAngle = angleToTarget -  MathF.Asin((targetSpeed * time *  MathF.Sin(z)) / (shootSpeed * time));
            return calculatedShootAngle;
        }
        public static void DrawPrettyStarSparkle(float opacity, Vector2 drawpos, Color innerColor, Color outerColor, Vector2 scale = default, Vector2 fatness = default, float flareCounter = 1, float fadeInStart = 0, float fadeInEnd = 1, float fadeOutStart =2, float fadeOutEnd = 4, float rotation = 0)
        {
            if(scale == default)
            {
                scale = Vector2.One;
            }
            if(fatness == default)
            {
                fatness = Vector2.One;
            }
            Texture2D texture = TextureAssets.Extra[98].Value;
            Color bigSparkleColor = outerColor * opacity;
            Vector2 origin = texture.Size() / 2f;
            Color smallSparkleColor = innerColor * opacity;
            float sizeMult = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 verticalLineScale = new Vector2(fatness.X * 0.5f, scale.X) * sizeMult;
            Vector2 horizontalLineScale = new Vector2(fatness.Y * 0.5f, scale.Y) * sizeMult;
            bigSparkleColor *= sizeMult;
            smallSparkleColor *= sizeMult;
            Main.EntitySpriteDraw(texture, drawpos, null, bigSparkleColor, (float)Math.PI / 2f + rotation, origin, verticalLineScale, SpriteEffects.None);
            Main.EntitySpriteDraw(texture, drawpos, null, bigSparkleColor, rotation, origin, horizontalLineScale, SpriteEffects.None);
            Main.EntitySpriteDraw(texture, drawpos, null, smallSparkleColor, (float)Math.PI / 2f + rotation, origin, verticalLineScale * 0.6f, SpriteEffects.None);
            Main.EntitySpriteDraw(texture, drawpos, null, smallSparkleColor, rotation, origin, horizontalLineScale * 0.6f, SpriteEffects.None);
        }
        public static float EaseInOutSine(float x)
        {
            return -(MathF.Cos(MathF.PI * x) - 1) / 2;
        }

        public static float EaseOutSine(float x)
        {
            return MathF.Sin((x * MathF.PI) / 2);
        }

        public static float EaseInSine(float x)
        {
            return 1 - MathF.Cos((x * MathF.PI) / 2);
        }

        public static float EaseInQuad(float x)
        {
            return x * x;
        }

        public static float EaseOutQuad(float x)
        {
            return 1 - (1 - x) * (1 - x);
        }

        public static float EaseInOutQuad(float x)
        {
            if (x < 0.5) return 2 * x * x;
            return 1 - 2 * (1 - x) * (1 - x);
        }

        public static float EaseInCubic(float x)
        {
            return x * x * x;
        }

        public static float EaseOutCubic(float x)
        {
            float t = 1 - x;
            return 1 - t * t * t;
        }

        public static float EaseInOutCubic(float x)
        {
            if (x < 0.5) return 4 * x * x * x;
            float t = 2 * x - 2;
            return 1 + t * t * t / 2;
        }
        public static float RemapEased(float fromValue, float fromMin, float fromMax, float toMin, float toMax, Func<float, float> easing)
        {
            float t = (fromValue - fromMin) / (fromMax - fromMin);
            if(t <= 0)
            {
                return toMin;
            }
            if(t >= 1)
            {
                return toMax;
            }
            t = easing(t);
            return (1 - t) * toMin + t * toMax;
        }
        public static void Shuffle(ref int[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = Main.rand.Next(i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
        public static void Shuffle(ref int[] array, UnifiedRandom rand)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
        public static void Shuffle(ref List<int> list, UnifiedRandom rand)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
        public static void ServerClientCheck(string q)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Main.NewText("Client says  " + q, Color.Pink);
            }

            if (Main.netMode == NetmodeID.Server) // Server
            {
                ChatHelper.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral("Server says " + q), Color.Green);
            }
        }
    }
}
