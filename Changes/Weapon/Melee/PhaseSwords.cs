//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.Utilities;
//using TRAEProject.NewContent.Items.Armor.Joter;
//using TRAEProject.Changes.Weapon.Melee.SpearProjectiles;
//using static Terraria.ModLoader.ModContent;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria.GameContent;

//namespace TRAEProject.Changes.Weapon
//{
	
//	public class PhaseSwords : GlobalItem
//	{
//		public override bool InstancePerEntity => true;
//		public override GlobalItem Clone(Item item, Item itemClone)
//		{
//			return base.Clone(item, itemClone);
//        }
//        public bool phaseSword = false;
//		public override void SetDefaults(Item item)
//        {
//            switch(item.type)
//            {
//                case ItemID.PurplePhaseblade:
//                case ItemID.YellowPhaseblade:
//                case ItemID.BluePhaseblade:
//                case ItemID.GreenPhaseblade:
//                case ItemID.RedPhaseblade:
//                case ItemID.OrangePhaseblade:
//                case ItemID.WhitePhaseblade:
//                    item.autoReuse = true;
//                    item.useTurn = false;
//                    phaseSword = true;
//                    item.noMelee = true;
//                    item.useAnimation = 24;
//                    item.useTime = 24;
//                    item.damage = 42;
//                    item.crit = 20;
//                    item.noUseGraphic = true;
//                    item.shoot = ModContent.ProjectileType<PhaseBladeProjectile>();
//                    item.shootsEveryUse = true;
//                    item.knockBack = 4;
//                    item.scale = 1.4f;
//                break;
//                //phasesabers
//                case ItemID.PurplePhasesaber:
//                case ItemID.YellowPhasesaber:
//                case ItemID.BluePhasesaber:
//                case ItemID.GreenPhasesaber:
//                case ItemID.RedPhasesaber:
//                case ItemID.OrangePhasesaber:
//                case ItemID.WhitePhasesaber:
//                    item.useAnimation = 24;
//                    item.useTime = 24;
//                    item.damage = 120;
//                    item.crit = 20;
//                    item.autoReuse = true;
//                    item.useTurn = false;
//                    phaseSword = true;
//                    item.noMelee = true;
//                    item.shoot = ModContent.ProjectileType<PhaseSaberProjectile>();
//                    item.shootsEveryUse = true;
//                    item.knockBack = 4;
//                    item.scale = 1.4f;
//                    break;
//            }
//            if(item.type == ItemID.BluePhaseblade || item.type == ItemID.BluePhasesaber)
//            {
//                item.crit = 30;
//            }
//            if(item.type == ItemID.RedPhaseblade || item.type == ItemID.RedPhasesaber)
//            {
//                item.knockBack = 10;
//            }
//            if(item.type == ItemID.WhitePhaseblade || item.type == ItemID.WhitePhasesaber)
//            {
//                item.scale = 1.6f;
//            }
//        }
//        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
//        {
//            if(phaseSword)
//            {
//                player.bodyFrame.Y = player.bodyFrame.Height * 0;
//                player.direction = MathF.Sign(Main.MouseWorld.X - player.MountedCenter.X);
//                if(player.itemAnimation == player.itemAnimationMax - 1)
//                {
//                    player.GetModPlayer<PhaseSwordPlayer>().swingDirection *= -1;
//                }
//                player.itemTime = player.itemAnimation;
//                float aimToward = (Main.MouseWorld - player.MountedCenter).ToRotation() * player.gravDir;

//                float trig = MathF.Cos(MathF.PI * ((float)player.itemAnimation / (float)player.itemAnimationMax));
//                float rotation = aimToward + MathF.PI * -0.5f + (player.direction * player.GetModPlayer<PhaseSwordPlayer>().swingDirection * (0.9f * MathF.PI) * trig) + player.direction * MathF.PI * 0.5f;


//                Player.CompositeArmStretchAmount stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
//                /*
//                if(MathF.Abs(trig) < 0.3f)
//                {
//                    stretch = Player.CompositeArmStretchAmount.Full;
//                }
//                else if(MathF.Abs(trig) < 0.6f)
//                {
//                    stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
//                }
//                */
//                player.SetCompositeArmFront(enabled: true, stretch, rotation);
//            }
//            base.UseStyle(item, player, heldItemFrame);
//        }

//        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
//        {
//            switch(item.type)
//            {
//                case ItemID.PurplePhaseblade:
//                case ItemID.PurplePhasesaber:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nConfuses enemies";
//                        }
//                    }
//                break;
//                case ItemID.YellowPhaseblade:
//                case ItemID.YellowPhasesaber:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nInflicts Midas";
//                        }
//                    }
//                break;
//                case ItemID.BluePhaseblade:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nHigher crit rate than other phaseblades";
//                        }
//                    }
//                break;
//                case ItemID.BluePhasesaber:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nHigher crit rate than other phasebsabers";
//                        }
//                    }
//                break;
//                case ItemID.GreenPhaseblade:
//                case ItemID.GreenPhasesaber:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nChance to restore health on hit";
//                        }
//                    }
//                break;
//                case ItemID.RedPhaseblade:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nHigher knockback than other phaseblades";
//                        }
//                    }
//                break;
//                case ItemID.RedPhasesaber:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nHigher knockback than other phasesabers";
//                        }
//                    }
//                break;
//                case ItemID.WhitePhaseblade:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nLonger reach than other phaseblades";
//                        }
//                    }
//                break;
//                case ItemID.WhitePhasesaber:
//                    foreach (TooltipLine line in tooltips)
//                    {
//                        if (line.Mod == "Terraria" && line.Name == "Knockback")
//                        {
//                            line.Text += "\nLonger reach than other phasesabers";
//                        }
//                    }
//                break;
//            }
            
            
//        }
//    }
//    public class PhaseSwordPlayer : ModPlayer
//    {
//        public int swingDirection = -1;
//    }
//   public class PhaseSwordProjectile : ModProjectile
//    {
//        public override string Texture => "TRAEProject/Changes/Weapon/Melee/Aura";
//        protected int withdrawnSideLength = 14;
//        protected int holdAtSideLength = 7;
//        protected float holdAt = 10;
//        protected float bladeStart = 20;
//        protected float bladeLength = 48;
//        bool debug = false;
//        float bladeOutAmount = 0f;
//        float[] oldBladeOutAmounts = new float[2];

//        public override void SetStaticDefaults()
//        {
//            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
//            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            
//        }

//        public virtual void SwordDefaults()
//        {

//        }
//        public override void SetDefaults()
//        {
//            Projectile.Size = new Vector2(16);
//            Projectile.friendly = true;
//            Projectile.DamageType = DamageClass.Melee;
//            Projectile.penetrate = -1;
//            Projectile.usesLocalNPCImmunity = true;
//            Projectile.tileCollide = false;
//            Projectile.ignoreWater = true;
//            Projectile.localNPCHitCooldown = -1;
//            Projectile.ownerHitCheck = true;
//            Projectile.usesOwnerMeleeHitCD = true;
//            Projectile.ownerHitCheckDistance = 300f;
//            Projectile.timeLeft = 2;
//            SwordDefaults();
//        }
//        public override void AI()
//        {
//            Player player = Main.player[Projectile.owner];
//            player.heldProj = Projectile.whoAmI;
//            if(player.itemAnimation > 1)
//            {
//                Projectile.timeLeft = 2;
//            }
//            Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation);
//            player.itemTime = player.itemAnimation;
//            Projectile.rotation = player.compositeFrontArm.rotation + (player.direction == -1 ? MathF.PI : 0) ;

//            float trig = 1f - MathF.Abs(MathF.Cos(MathF.PI * ((float)player.itemAnimation / (float)player.itemAnimationMax)));
//            bladeOutAmount = trig * trig * trig;
//            for (int i = oldBladeOutAmounts.Length - 1; i > 0; i--) 
//            {
//                oldBladeOutAmounts[i] = oldBladeOutAmounts[i - 1];
//            }

//            oldBladeOutAmounts[0] = bladeOutAmount;
            
//        }
//        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
//        {
//            float adjustedScale = Main.player[Projectile.owner].HeldItem.scale; 
//            for(int i = ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1; i >= 0; i--)
//            {
//                Vector2 currentCenter = Projectile.oldPos[i] + Projectile.Size * 0.5f;
//                float curentOutAmount = oldBladeOutAmounts[i];
//                float currentRotation = Projectile.oldRot[i];
//                if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), currentCenter + PolarVector(adjustedScale * (bladeStart - holdAt), currentRotation), currentCenter + PolarVector(adjustedScale * ((bladeLength * curentOutAmount) + bladeStart - holdAt), currentRotation)))
//                {
//                    return true;
//                }
//                if(i > 0)
//                {
//                    float intermediats = 20;
//                    Vector2 posStep = (Projectile.oldPos[i - 1] - Projectile.oldPos[i]) / intermediats;
//                    float rotStep = (Projectile.oldRot[i - 1] - Projectile.oldRot[i]) / intermediats;
//                    float outAmountStep = (oldBladeOutAmounts[i - 1] - oldBladeOutAmounts[i]) / intermediats;
//                    for(int j = 1; j < intermediats; j++)
//                    {
//                        currentCenter = Projectile.oldPos[i] + posStep * j + Projectile.Size * 0.5f;
//                        curentOutAmount = oldBladeOutAmounts[i] + j * outAmountStep;
//                        currentRotation = Projectile.oldRot[i] + rotStep * j;
//                        if(Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), currentCenter + PolarVector(adjustedScale * (bladeStart - holdAt), currentRotation), currentCenter + PolarVector(adjustedScale * ((bladeLength * curentOutAmount) + bladeStart - holdAt), currentRotation)))
//                        {
//                            return true;
//                        }
                        
//                    }
//                }
                
//            }
//            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + PolarVector(adjustedScale * (bladeStart - holdAt), Projectile.rotation), Projectile.Center + PolarVector(adjustedScale * ((bladeLength * bladeOutAmount) + bladeStart - holdAt), Projectile.rotation));
//        }
//        public override bool PreDraw(ref Color lightColor)
//        {
//            Player player = Main.player[Projectile.owner];
//            float adjustedScale = player.HeldItem.scale; 
//            //Main.NewText(adjustedScale);
//            player.ApplyMeleeScale(ref adjustedScale);
//            //Main.NewText(adjustedScale);
//            Texture2D texture = TextureAssets.Item[player.HeldItem.type].Value; Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height - withdrawnSideLength, withdrawnSideLength, withdrawnSideLength), lightColor, Projectile.rotation + MathF.PI * 0.25f, new Vector2(holdAtSideLength, withdrawnSideLength - holdAtSideLength), adjustedScale, SpriteEffects.None, 0);
//            for(int i = ProjectileID.Sets.TrailCacheLength[Projectile.type] - 1; i >= 0; i--)
//            {
//                int bladeFrameSize = (int)MathF.Round((float)(texture.Width - withdrawnSideLength) * oldBladeOutAmounts[i]) + 2;
//                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, new Rectangle(withdrawnSideLength - 2, texture.Height - withdrawnSideLength + 2 - bladeFrameSize, bladeFrameSize, bladeFrameSize), Color.White, Projectile.oldRot[i] + MathF.PI * 0.25f, new Vector2(0 + 2 - withdrawnSideLength + holdAtSideLength, bladeFrameSize - 2 + withdrawnSideLength - holdAtSideLength), adjustedScale, SpriteEffects.None, 0);
//                if(i > 0)
//                {
//                    float intermediats = 20;
//                    Vector2 posStep = (Projectile.oldPos[i - 1] - Projectile.oldPos[i]) / intermediats;
//                    float rotStep = (Projectile.oldRot[i - 1] - Projectile.oldRot[i]) / intermediats;
//                    float outAmountStep = (oldBladeOutAmounts[i - 1] - oldBladeOutAmounts[i]) / intermediats;
//                    for(int j = 1; j < intermediats; j++)
//                    {
//                        bladeFrameSize = (int)MathF.Round((float)(texture.Width - withdrawnSideLength) * (oldBladeOutAmounts[i] + j * outAmountStep)) + 2;
//                        Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + posStep * j + Projectile.Size * 0.5f - Main.screenPosition, new Rectangle(withdrawnSideLength - 2, texture.Height - withdrawnSideLength + 2 - bladeFrameSize, bladeFrameSize, bladeFrameSize), Color.White, (Projectile.oldRot[i] + rotStep * j) + MathF.PI * 0.25f, new Vector2(0 + 2 - withdrawnSideLength + holdAtSideLength, bladeFrameSize - 2 + withdrawnSideLength - holdAtSideLength), adjustedScale, SpriteEffects.None, 0);
//                    }
//                }
//            }
            


//            if (debug)
//            {
//                //holdAt
//                Main.EntitySpriteDraw(TRAEProj.debugCross, Projectile.Center - Main.screenPosition, null, Color.Green, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);
//                //bladeStart
//                Main.EntitySpriteDraw(TRAEProj.debugCross, Projectile.Center + PolarVector(adjustedScale * (bladeStart - holdAt), Projectile.rotation) - Main.screenPosition, null, Color.Blue, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);
//                //bladeStart + bladeLength
//                Main.EntitySpriteDraw(TRAEProj.debugCross, Projectile.Center + PolarVector(adjustedScale * ((bladeLength * bladeOutAmount) + bladeStart - holdAt), Projectile.rotation) - Main.screenPosition, null, Color.Red, 0, TRAEProj.debugCross.Size() * 0.5f, 1f, SpriteEffects.None, 0);
//            }

//            return false;
//        }

//        public static Vector2 PolarVector(float radius, float theta)
//        {
//            return new Vector2(MathF.Cos(theta), MathF.Sin(theta)) * radius;
//        }

//        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
//        {
//            modifiers.HitDirectionOverride = Main.player[Projectile.owner].direction;
//        }
//    }
//   public class PhaseBladeProjectile : PhaseSwordProjectile
//    {
//        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
//        {
//            Player player = Main.player[Projectile.owner];
//            switch(player.HeldItem.type)
//            {
//                case ItemID.PurplePhaseblade:
//                    target.AddBuff(BuffID.Confused, 600);
//                break;
//                case ItemID.YellowPhaseblade:
//                    target.AddBuff(BuffID.Midas, 600);
//                break;
//                case ItemID.GreenPhaseblade:
//                    if(Main.rand.NextBool(3))
//                    {
//                        if(player.statLife < player.statLifeMax2)
//                        {
//                            player.statLife++;
//                            player.HealEffect(1, true);
//                        }
//                    }
//                break;
//            }
//        }
//    }
//  public  class PhaseSaberProjectile : PhaseSwordProjectile
//    {
//        public override void SwordDefaults()
//        {
//            withdrawnSideLength = 14;
//            holdAtSideLength = 7;
//            holdAt = 10;
//            bladeStart = 23;
//            bladeLength = 79 - 23;
//        }
//        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
//        {
//            Player player = Main.player[Projectile.owner];
//            switch(player.HeldItem.type)
//            {
//                case ItemID.PurplePhasesaber:
//                    target.AddBuff(BuffID.Confused, 600);
//                break;
//                case ItemID.YellowPhasesaber:
//                    target.AddBuff(BuffID.Midas, 600);
//                break;
//                case ItemID.GreenPhasesaber:
//                    if(Main.rand.NextBool(3))
//                    {
//                        if(player.statLife < player.statLifeMax2)
//                        {
//                            player.statLife++;
//                            player.HealEffect(1, true);
//                        }
//                    }
//                break;
//            }
//        }
//    }
//}