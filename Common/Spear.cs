using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using TRAEProject.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using TRAEProject.Common.ModPlayers;
using Terraria.Audio;
using Terraria.ID;
using TRAEProject.Changes.Weapon;
using TRAEProject.NewContent.Buffs;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Weapon.Melee.SpearProjectiles;
using TRAEProject.NewContent.Projectiles;
namespace TRAEProject.Common
{
    public abstract class Spear : ModProjectile
    {
        /// <summary> How long the spear is from corner to corner, check the sprite the spear uses to find the correct value. </summary>
        public float spearLength = 76f;
        /// <summary> Where along the spear's sprite the animation starts at, bottom right corner of the sprite is 0 and top left should be spearLength, also used for collision. This should e where the handle meets the tip of the spear </summary>
        public float stabStart = 54f;
        /// <summary> Where along the spear's sprite the animation ends at, bottom right corner of the sprite is 0 and top left should be spearLength </summary>
        public float stabEnd = 0;
        /// <summary> How much the spear swings when it stabs, this value is in radians </summary>
        public float swingAmount = 0;
        /// <summary> Draws colored dots on the spear, green dots should be at the ends of the spear, red dot is spear startm blue dot is spear end, and the whit dot is the player's center</summary>
        public bool debug = false;
        /// <summary> Can be used by specific spears to interupt the standard animation </summary>
        public int interupting = 0;

        /// <summary> Use this instead of SetDefaults(). </summary>
        public virtual void SpearDefaults()
        {

        }
        /// <summary> Called every frame the spear is being used, useful for stuff like dusts </summary>
        public virtual void SpearActive()
        {

        }
        /// <summary> Use this instead of OnHitNPC() </summary>
        public virtual void SpearHitNPCMelee(NPC target, NPC.HitInfo hit)
        {

        }
        /// <summary> Use this instead of ModifyHitNPC <summary> 
        public virtual void SpearModfiyHitNPCMelee(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        /// <summary> Called the moment the spear reaches its max range </summary>
        public virtual void OnMaxReach(float direction)
        {

        }
        /// <summary> If the item has channel the spear will be held out when at max reach and this method will be called </summary>
        public virtual void Channeling()
        {

        }
        /// <summary> Called when interupting > 0 </summary>
        public virtual void InteruptedAnimation()
        {

        }
        /// <summary> Called when the spear is created </summary>
        public virtual void OnStart()
        {

        }
        public static void SpearPrefixScaleing(Item item, Player player, Projectile projectile)
        {
            if (!item.IsAir)
            {
                float bonusSize = 1f;
                switch (item.prefix)
                {
                    case PrefixID.Large:
                        bonusSize = (1.18f / 1.15f);
                        break;
                    case PrefixID.Massive:
                        bonusSize = (1.25f / 1.18f);
                        break;
                    case PrefixID.Dangerous:
                        bonusSize = (1.12f / 1.5f);
                        break;
                    case PrefixID.Bulky:
                        bonusSize = (1.2f / 1.1f);
                        break;
                }
                projectile.scale = bonusSize * item.scale * (player.meleeScaleGlove ? 1.1f : 1f) * player.GetModPlayer<MeleeStats>().weaponSize;
            }
        }

        bool runOnce = true;
        float stabDirection = 0;
        public float aimDirection = 0;
        SpriteEffects effects = SpriteEffects.None;
        bool calledMaxReach = false;
        int preInteruptedAnimaion;
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = Projectile.height = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            //Projectile.aiStyle = 19; //vanilla spear AI is turned off by PreAI, this just helps other mods know this is a spear
            SpearDefaults();
        }
        protected float outAmount = 0f;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            if (runOnce)
            {
                aimDirection = Projectile.velocity.ToRotation();
                swingAmount *= player.direction * -1;
                stabDirection = aimDirection * swingAmount;
                runOnce = false;
                Projectile.velocity = Vector2.Zero;
                OnStart();
            }
            //Projectile.scale = player.HeldItem.scale * (player.meleeScaleGlove ? 1.1f : 1f) * player.GetModPlayer<MeleeStats>().weaponSize;
            SpearPrefixScaleing(player.HeldItem, player, Projectile);
            player.itemTime = player.itemAnimation;
            int switchStabTime = (int)(2f * (float)player.itemAnimationMax / 3f);
            int stabTime = player.itemAnimationMax - switchStabTime;
            int swivelDir = 1;
            if (player.itemAnimation > switchStabTime)
            {
                outAmount = 1f - ((float)(player.itemAnimation - switchStabTime) / (float)stabTime);
            }
            else
            {

                swivelDir = -1;
                outAmount = 1f - ((float)(switchStabTime - player.itemAnimation) / (float)switchStabTime);

                if (player.channel)
                {
                    player.itemAnimation = switchStabTime;
                    Channeling();
                }
                else if (interupting > 0)
                {
                    player.itemAnimation = switchStabTime;
                    InteruptedAnimation();
                }
                else
                {
                }

            }

            stabDirection = aimDirection + swingAmount * (Math.Abs(MathF.Sin(MathF.PI * outAmount))) * swivelDir;
            Projectile.Center = ownerMountedCenter + PolarVector((outAmount * (stabStart - stabEnd) + (spearLength - stabStart)) * Projectile.scale, stabDirection);


            AnimatePlayer();
            if (player.direction == 1)
            {
                effects = SpriteEffects.FlipVertically;
                Projectile.rotation = stabDirection + 5f * MathF.PI / 4f;
            }
            else
            {
                effects = SpriteEffects.None;
                Projectile.rotation = stabDirection + 3f * MathF.PI / 4f;
            }
            if (player.itemAnimation == 0 || player.itemAnimation == 1)
            {
                Projectile.Kill();
            }
            if (!calledMaxReach && player.itemAnimation <= switchStabTime)
            {
                OnMaxReach(stabDirection);
                calledMaxReach = true;
            }
            SpearActive();
        }
        /*
        void AnimatePlayer()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            float num2 = Projectile.Center.X - pointPoisition.X;
            float num3 = Projectile.Center.Y  - pointPoisition.Y;
            float num4 = MathF.Sqrt(num2 * num2 + num3 * num3);
            num2 *= num4;
            num3 *= num4;
            float itemRotation = MathF.Atan2(num3 * (float)player.direction, num2 * (float)player.direction) - player.fullRotation;

            float num19 = itemRotation * player.direction;
            player.bodyFrame.Y = player.bodyFrame.Height * 3;
            if ((double)num19 < -0.75)
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 2;
                if (player.gravDir == -1f)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 4;
                }
            }
            if ((double)num19 > 0.6)
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 4;
                if (player.gravDir == -1f)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 2;
                }
            }
            if(Math.Sign(Projectile.Center.X - player.Center.X) != player.direction)
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 1;
            }

        }
        */
        void AnimatePlayer()
        {
            Player player = Main.player[Projectile.owner];
            player.bodyFrame.Y = player.bodyFrame.Height * 1;

            Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
            float num2 = (float)Projectile.Center.X - pointPoisition.X;
            float num3 = (float)Projectile.Center.Y - pointPoisition.Y;
            float itemRotation = MathF.Atan2(num3 * (float)player.direction, num2 * (float)player.direction) - player.fullRotation;

            Vector2 SpearStabPos = Projectile.Center + PolarVector((spearLength - stabStart) * Projectile.scale, stabDirection + MathF.PI);
            float distance = (SpearStabPos - pointPoisition).Length();

            Player.CompositeArmStretchAmount stretch = Player.CompositeArmStretchAmount.Quarter;
            if (distance > 24f)
            {
                stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
            }
            if (distance > 48f)
            {
                stretch = Player.CompositeArmStretchAmount.Full;
            }
            float rotation = itemRotation - MathF.PI / 2f * (float)player.direction;
            player.SetCompositeArmFront(enabled: true, stretch, rotation);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + PolarVector((spearLength - stabStart) * Projectile.scale, stabDirection + MathF.PI), Projectile.Center, spearLength - stabStart, ref point);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            int direction = Math.Sign(target.Center.X - Main.player[Projectile.owner].Center.X);
            modifiers.HitDirectionOverride = direction;
            SpearModfiyHitNPCMelee(target, ref modifiers);
        }
        public int[] hitCount = new int[Main.npc.Length];
        public int pierceLimit = 0;
        public int MaxPierce = 6;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount[target.whoAmI]++;
            if(hitCount[target.whoAmI] >= 2)
            {
                Projectile.localNPCImmunity[target.whoAmI] = -1;
            }
            else
            {
                Projectile.localNPCImmunity[target.whoAmI] = Main.player[Projectile.owner].itemAnimationMax / 3;
            }
            target.immune[Projectile.owner] = 0;
            pierceLimit++;
            if (pierceLimit == MaxPierce)
            {
                Projectile.damage = 0;
            }
            SpearHitNPCMelee(target, hit);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, effects == SpriteEffects.None ? Vector2.Zero : Vector2.UnitY * texture.Width, Projectile.scale, effects, 0);
            if (Projectile.type == ProjectileType<GhastlyGlaiveThrow>())
            {
                texture = Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/SpearProjectiles/GhastlyGlaiveGlow").Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, effects == SpriteEffects.None ? Vector2.Zero : Vector2.UnitY * texture.Width, Projectile.scale, effects, 0);
            }
            if (Projectile.type == ProjectileType<Daybreak>())
            {
                texture = Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/SpearProjectiles/DaybreakGlow").Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, effects == SpriteEffects.None ? Vector2.Zero : Vector2.UnitY * texture.Width, Projectile.scale, effects, 0);
            }
            if (debug)
            {
                //spearLength
                Main.EntitySpriteDraw(TRAEProj.debugCross, Projectile.Center + PolarVector(spearLength * Projectile.scale, stabDirection + MathF.PI) - Main.screenPosition, null, Color.Green, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(TRAEProj.debugCross, Projectile.Center - Main.screenPosition, null, Color.Green, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);

                //stabStart
                Main.EntitySpriteDraw(TRAEProj.debugCross, Projectile.Center + PolarVector((spearLength - stabStart) * Projectile.scale, stabDirection + MathF.PI) - Main.screenPosition, null, Color.Red, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);

                //stabEnd
                Main.EntitySpriteDraw(TRAEProj.debugCross, Projectile.Center + PolarVector((spearLength - stabEnd) * Projectile.scale, stabDirection + MathF.PI) - Main.screenPosition, null, Color.Blue, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);


                Player player = Main.player[Projectile.owner];
                Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
                //mounted center
                Main.EntitySpriteDraw(TRAEProj.debugCross, ownerMountedCenter - Main.screenPosition, null, Color.White, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            return false;
        }

        public static Vector2 PolarVector(float radius, float theta)
        {
            return new Vector2(MathF.Cos(theta), MathF.Sin(theta)) * radius;
        }
    }
    public abstract class SpearThrow : ModProjectile
    {
        /// <summary> How long the spear is from corner to corner, check the sprite the spear uses to find the correct value. </summary>
        public float spearLength = 76f;
        /// <summary> Where along the spear's sprite the player holds it, bottom right corner of the sprite is 0 and top left should be spearLength </summary>
        public float holdAt = 45f;
        /// <summary> How long a fully charge spear flies before it starts to fall, set to -1 to make it infinite time </summary>
        public int floatTime = 45;
        /// <summary> Max number of spears that can be stuck on enemies, not used if Projectile.penetrate >1 </summary>
        public int maxSticks = 0;
        /// <summary> Dps caused by each spear stuck in the enemy, not used if Projectile.penetrate >1 </summary>
        public int stickingDps = 0;
        /// <summary> Use this instead of SetDefaults(). </summary>
        public virtual void SpearDefaults()
        {

        }
        /// <summary> Called every frame the spear is being used, useful for stuff like dusts </summary>
        public virtual void SpearActive()
        {

        }
        /// <summary> Called the moment the spear reaches maximum charge </summary>
        public virtual void OnCharge()
        {

        }
        /// <summary> Use this instead of OnHitNPC() </summary>
        public virtual void SpearHitNPC(bool atMaxCharge, NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        /// <summary> Use this instead of ModifyHitNPC <summary> 
        public virtual void SpearModfiyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {

        }
        /// <summary> Called every frame the spear is stuck inside an enemy </summary>
        public virtual void StuckEffects(NPC victim)
        {
        }
        /// <summary> Called every frame the spear is flying </summary>
        public virtual void ThrownUpdate()
        {

        }
        /// <summary> Called every frame the spear is being held </summary>
        public virtual void HeldUpdate()
        {

        }
        /// <summary> Called right when the spear is thrown.</summary>
        public virtual void OnThrow(float chargeAmt)
        {
        }
        public float chargeAmt = 0;
        int timer = 0;
        bool thrown = false;
        bool justCharged = false;
        int chargeTime = 0;
        int chargeTimeMax = 0;
        SpriteEffects effects = SpriteEffects.None;
        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = Projectile.height = 10;
            
            SpearDefaults();
            if(Projectile.penetrate != 1)
            {
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
                maxSticks = 0;
                stickingDps = 0;
            }
        }
        float aimDirection;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            if (isStickingToTarget)
            {
                Sticking();
            }
            else
            {
                Projectile.friendly = thrown;
                Projectile.tileCollide = thrown;
                if (thrown)
                {
                    timer++;
                    if (timer >= floatTime && floatTime != -1)
                    {
                        Projectile.velocity.X = Projectile.velocity.X * 0.98f;
                        Projectile.velocity.Y = Projectile.velocity.Y + 0.35f;
                    }
                    Projectile.ai[0] = Projectile.velocity.ToRotation();
                    if (effects == SpriteEffects.FlipVertically)
                    {
                        Projectile.rotation = Projectile.velocity.ToRotation() + 5f * MathF.PI / 4f;
                    }
                    else
                    {
                        Projectile.rotation = Projectile.velocity.ToRotation() + 3f * MathF.PI / 4f;
                    }
                    ThrownUpdate();
                }
                else if(Projectile.ai[1] == 1)
                {
                    ThrowSpear(Projectile.ai[0]);
                }
                else
                {
                    Spear.SpearPrefixScaleing(player.HeldItem, player, Projectile);                    
					if (player.itemTime > player.itemTimeMax - 1)
                    {
                        chargeTime = player.itemTime;
                        chargeTimeMax = player.itemTimeMax;
                        player.itemAnimation = player.itemAnimationMax = player.itemTimeMax / 3;
                    }
                    player.itemTime = player.itemAnimation + 1;
                    player.heldProj = Projectile.whoAmI;
                    Projectile.velocity = Vector2.Zero;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        player.direction = Math.Sign(Main.MouseWorld.X - player.MountedCenter.X);
                    }
                    Vector2 vector24 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
                    if (player.direction != 1)
                    {
                        vector24.X = player.bodyFrame.Width - vector24.X;
                    }
                    if (player.gravDir != 1f)
                    {
                        vector24.Y = player.bodyFrame.Height - vector24.Y;
                    }
                    vector24 -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
                    Vector2 holdPos = player.RotatedRelativePoint(player.MountedCenter - new Vector2(20f, 42f) / 2f + vector24, reverseRotation: false, addGfxOffY: false);

                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.ai[0] = holdPos.DirectionTo(Main.MouseWorld).ToRotation();
                        Projectile.netUpdate = true;
                    }
                    Projectile.Center = holdPos + PolarVector((spearLength - holdAt) * Projectile.scale, Projectile.ai[0]);
                    Projectile.rotation = Projectile.ai[0] + 3f * MathF.PI / 4f;
                    if (chargeAmt >= 1)
                    {
                        chargeAmt = 1f;
                        if (!justCharged)
                        {
                            OnCharge();
                            if (player.autoReuseGlove)
                            {
                                Projectile.ai[1] = 1;
                                Projectile.netUpdate = true;
                            }
                            else
                            {
                                SoundEngine.PlaySound(SoundID.MaxMana with { MaxInstances = 0 }, player.Center);
                            }

                            justCharged = true;
                        }
                    }
                    else
                    {
                        chargeAmt = 1f - ((float)chargeTime / (float)chargeTimeMax);
                    }
                    player.itemAnimation = player.itemAnimationMax - 1;
                    if (player.direction == 1)
                    {
                        effects = SpriteEffects.FlipVertically;
                        Projectile.rotation = Projectile.ai[0] + 5f * MathF.PI / 4f;
                    }
                    else
                    {
                        effects = SpriteEffects.None;
                        Projectile.rotation = Projectile.ai[0] + 3f * MathF.PI / 4f;
                    }
                    if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                    {
                        Projectile.ai[1] = 1;
                        Projectile.netUpdate = true;
                    }
                    HeldUpdate();
                }
               
                if (chargeTime > 0)
                {
                    chargeTime--;
                }
            }
            SpearActive();
        }
        void ThrowSpear(float dir)
        {
            thrown = true;
            Player player = Main.player[Projectile.owner];
            if(chargeAmt != 1 && floatTime != -1)
            {
                floatTime = (int)(floatTime * chargeAmt);
            }
            Projectile.velocity = PolarVector(player.HeldItem.shootSpeed * player.GetModPlayer<MeleeStats>().meleeVelocity * (player.GetAttackSpeed(DamageClass.Melee)) * (chargeAmt == 1 ? 1 : 0.6f), dir);
            SoundEngine.PlaySound(SoundID.Item1 with { MaxInstances = 0 }, Projectile.Center);
            player.itemAnimationMax += chargeTime;
            player.itemAnimation = player.itemAnimationMax - 1;
            player.itemTime = player.itemAnimation + 1;
            Projectile.timeLeft = 180;
            OnThrow(chargeAmt);
        }

        public bool isStickingToTarget
        {
            get { return Projectile.ai[0] == 1f; }
            set { Projectile.ai[0] = value ? 1f : 0f; }
        }

        // WhoAmI of the current target
        public float targetWhoAmI
        {
            get { return Projectile.ai[1]; }
            set { Projectile.ai[1] = value; }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (chargeAmt == 1)
            {
                modifiers.FinalDamage *= 2f;
            }
            else
            {
                modifiers.FinalDamage *= chargeAmt;
            }
            if(maxSticks > 0)
            {
                Projectile.timeLeft = 30 * 60;
                isStickingToTarget = true;
                targetWhoAmI = target.whoAmI; // Set the target whoAmI
                Projectile.velocity =
                    (target.Center - Projectile.Center) *
                    0.75f; // Change velocity based on delta center of targets (difference between entity centers)
                Projectile.netUpdate = true; // netUpdate projectile javelin
                target.AddBuff(BuffType<Impaled>(), 900); // Adds the Impaled debuff
                Projectile.penetrate = -1;
                Projectile.friendly = false; // Makes sure the sticking javelins do not deal damage anymore 

                // The following code handles the javelin sticking to the enemy hit.
                Player player = Main.player[Projectile.owner];
                Point[] stickingJavelins = new Point[maxSticks]; // The point array holding for sticking javelins
                int javelinIndex = 0; // The javelin index
                for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
                {
                    Projectile currentProjectile = Main.projectile[i];
                    if (i != Projectile.whoAmI // Make sure the looped projectile is not the current javelin
                        && currentProjectile.active // Make sure the projectile is active
                        && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                        && currentProjectile.type == Projectile.type // Make sure the projectile is of the same type as projectile javelin
                        && currentProjectile.ai[0] == 1f // Make sure ai0 state is set to 1f (set earlier in ModifyHitNPC)
                        && currentProjectile.ai[1] == (float)target.whoAmI
                    ) // Make sure ai1 is set to the target whoAmI (set earlier in ModifyHitNPC)
                    {
                        stickingJavelins[javelinIndex++] =
                            new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                        if (javelinIndex >= stickingJavelins.Length
                        ) // If the javelin's index is bigger than or equal to the point array's length, break
                        {
                            break;
                        }
                    }
                }
                // Here we loop the other javelins if new javelin needs to take an older javelin's place.
                if (javelinIndex >= stickingJavelins.Length)
                {
                    int oldJavelinIndex = 0;
                    // Loop our point array
                    for (int i = 1; i < stickingJavelins.Length; i++)
                    {
                        // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                        if (stickingJavelins[i].Y < stickingJavelins[oldJavelinIndex].Y)
                        {
                            oldJavelinIndex = i; // Remember the index of the removed javelin
                        }
                    }
                    // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                    Main.projectile[stickingJavelins[oldJavelinIndex].X].Kill();
                }
                Projectile.ignoreWater = true; // Make sure the projectile ignores water
                Projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
            }
            SpearModfiyHitNPC(target, ref modifiers);
        }
        void Sticking()
        {
            int aiFactor = 15; // Change projectile factor to change the 'lifetime' of projectile sticking javelin
            bool killProj = false; // if true, kill projectile at the end
            bool hitEffect = false; // if true, perform a hit effect
            Projectile.localAI[0] += 1f;
            // Every 30 ticks, the javelin will perform a hit effect
            hitEffect = Projectile.localAI[0] % 30f == 0f;
            int projTargetIndex = (int)targetWhoAmI;
            if (Projectile.localAI[0] >= (float)(60 * aiFactor)// If it's time for projectile javelin to die, kill it
                || (projTargetIndex < 0 || projTargetIndex >= 200)) // If the index is past its limits, kill it
            {
                killProj = true;
            }
            else if (Main.npc[projTargetIndex].active && !Main.npc[projTargetIndex].dontTakeDamage) // If the target is active and can take damage
            {
                // Set the projectile's position relative to the target's center
                Projectile.Center = Main.npc[projTargetIndex].Center - Projectile.velocity * 2f;
                Projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;
                if (hitEffect) // Perform a hit effect here
                {
                    Main.npc[projTargetIndex].HitEffect(0, 1.0);
                }
                StuckEffects(Main.npc[projTargetIndex]);
                Main.npc[projTargetIndex].lifeRegen -= stickingDps * 2;
            }
            else // Otherwise, kill the projectile
            {
                killProj = true;
            }

            if (killProj) // Kill the projectile
            {
                Projectile.Kill();
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return thrown;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            bool shake = chargeAmt != 1f && !thrown;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + (shake ? new Vector2(-2 + Main.rand.Next(5), -2 + Main.rand.Next(5)) : Vector2.Zero), null, lightColor, Projectile.rotation, effects == SpriteEffects.None ? Vector2.Zero : Vector2.UnitY * texture.Width, Projectile.scale, effects, 0);
            if(Projectile.type == ProjectileType<GhastlyGlaiveThrow>())
            {
                texture = Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/SpearProjectiles/GhastlyGlaiveGlow").Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + (shake ? new Vector2(-2 + Main.rand.Next(5), -2 + Main.rand.Next(5)) : Vector2.Zero), null, Color.White, Projectile.rotation, effects == SpriteEffects.None ? Vector2.Zero : Vector2.UnitY * texture.Width, Projectile.scale, effects, 0);
            }
            if (Projectile.type == ProjectileType<DaybreakThrow>())
            {
                texture = Request<Texture2D>("TRAEProject/Changes/Weapon/Melee/SpearProjectiles/DaybreakGlow").Value;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + (shake ? new Vector2(-2 + Main.rand.Next(5), -2 + Main.rand.Next(5)) : Vector2.Zero), null, Color.White, Projectile.rotation, effects == SpriteEffects.None ? Vector2.Zero : Vector2.UnitY * texture.Width, Projectile.scale, effects, 0);
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SpearHitNPC(chargeAmt ==1, target, hit, damageDone);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.velocity.Length() > 4f)
            {
                float point = 0;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + PolarVector((6) * Projectile.scale + Projectile.velocity.Length() + 2f, Projectile.ai[0] + MathF.PI), Projectile.Center + PolarVector((6) * Projectile.scale, Projectile.ai[0] + MathF.PI), 18 * Projectile.scale, ref point);
            }
            return null;
        }
        public static Vector2 PolarVector(float radius, float theta)
        {
            return new Vector2(MathF.Cos(theta), MathF.Sin(theta)) * radius;
        }
        public int DustOnDeath = 81;
        public int DustOnDeathCount = 20;
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = 0 }, Projectile.Center); // Play a death sound
            Vector2 usePos = Projectile.position; // Position to use for dusts

            // Please note the usage of MathHelper, please use this!
            // We subtract 90 degrees as radians to the rotation vector to offset the sprite as its default rotation in the sprite isn't aligned properly.
            Vector2 rotVector = (Projectile.rotation + MathHelper.ToRadians(135f * Projectile.direction)).ToRotationVector2(); // rotation vector to use for dust velocity
            usePos += rotVector * 16f;

            // Declaring a constant in-line is fine as it will be optimized by the compiler
            // It is however recommended to define it outside method scope if used elswhere as well
            // They are useful to make numbers that don't change more descriptive
           

            // Spawn some dusts upon javelin death
            for (int i = 0; i < DustOnDeathCount; i++)
            {
                // Create a new dust
                Dust dust = Dust.NewDustDirect(usePos, Projectile.width, Projectile.height, DustOnDeath);
                dust.position = (dust.position + Projectile.Center) / 2f;
                dust.velocity += rotVector * 2f;
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                usePos -= rotVector * 8f;
            }
        }
    }
    public class AutoThrowAssist : ModPlayer
    {
        public override bool PreItemCheck()
        {
            if(!Player.HeldItem.IsAir && Player.HeldItem.GetGlobalItem<SpearItems>().altShoot != -1 && Main.mouseRight && Player.autoReuseGlove && Player.itemAnimation ==1)
            {
                Player.altFunctionUse = 2;
                Player.itemAnimationMax = Player.itemAnimation = Player.HeldItem.useAnimation;
            }
            return base.PreItemCheck();
        }
    }
}
