using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Summoner.Whip;
using TRAEProject.Changes.Weapon;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.Changes.Weapon.Summon.Minions;
using System.Collections.Generic;
using System;

namespace TRAEProject.NewContent.Items.Weapons.Summoner.Starflow
{
    public class Starflow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stardust Trail");
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
        public override void SetDefaults()
        {
            Item.autoReuse = false;
            Item.useStyle = 1;
            Item.width = 54;
            Item.height = 30;
            Item.shoot = ProjectileType<StarflowP>();
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;

            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.noUseGraphic = true;
            Item.damage = 162;
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 0.5f;
            Item.shootSpeed = 8.25f;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(0, 10, 0, 0);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentStardust, 18)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
 
    public class StarflowP : WhipProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starflow");
        }
        public override void WhipDefaults()
        {
            originalColor = new Color(35, 77, 73);
            whipRangeMultiplier = 1.5f;
            fallOff = 0.25f;
            tag = BuffType<StarflowTag>();
            whipSegments = 10;
            tipScale = 1.25f;
        }
    }
    public class StarflowTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starflow Tag");
            // Description.SetDefault("22 tag damage pogchamp");
            Main.debuff[Type] = true;
  
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<Tag>().Damage += 22;
        }
    }
    public class StarflowInvaders: ModPlayer
    {
        public int flowies = 0;
        public int MaxFlowies = 5;

        public override void ResetEffects()
        {
            flowies = 0;
        }
        public override void UpdateDead()
        {
            flowies = 0;
        }
        

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (proj.type == ProjectileType<StarflowP>() && flowies < MaxFlowies)
			{
				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(proj.oldPosition, proj.width, proj.height, DustID.YellowStarDust, 1f);
					dust.noGravity = true;
				}
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.position, new Vector2(0), ProjectileType<StarflowInvader>(), damageDone / 2, 1f, Player.whoAmI);
            }

        }
    }
    public class StarflowInvader : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starflow Invader");
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 32;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Main.projFrames[Projectile.type] = 4;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.ArmorPenetration= 50;
            Projectile.tileCollide = false;
            Projectile.minionSlots = 0; // is this needed? Wouldn't the default value be 0 already?
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            ProjectileID.Sets.MinionShot[Type] = true;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCHitCooldown = (int)Projectile.ai[0];

        }
        public override void AI() // Sanguine AI
		{
			if (Main.rand.Next(2) == 0)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowStarDust, 1, Projectile.velocity.Y * -0.33f, 0, default, 0.7f);

			}
			Player player = Main.player[Projectile.owner];
            player.GetModPlayer<StarflowInvaders>().flowies++;
            if (player.GetModPlayer<StarflowInvaders>().flowies > player.GetModPlayer<StarflowInvaders>().MaxFlowies)
                Projectile.Kill();

			InvaderAI();
		}
		private static List<int> _ai156_blacklistedTargets = new List<int>();
		private void InvaderAI()
		{
			List<int> ai156_blacklistedTargets = _ai156_blacklistedTargets;
			Player player = Main.player[Projectile.owner];
				//DelegateMethods.v3_1 = AI_156_GetColor().ToVector3();
				Point point = Projectile.Center.ToTileCoordinates();
				DelegateMethods.CastLightOpen(point.X, point.Y);
				if (++Projectile.frameCounter >= 6)
				{
					Projectile.frameCounter = 0;
					if (++Projectile.frame >= Main.projFrames[Projectile.type] - 1)
					{
						Projectile.frame = 0;
					}
				}
				int num2 = player.direction;
				if (Projectile.velocity.X != 0f)
				{
					num2 = Math.Sign(Projectile.velocity.X);
				}
				Projectile.spriteDirection = num2;
			

			ai156_blacklistedTargets.Clear();
			InvaderBlacklist(ai156_blacklistedTargets);
		}

		private void InvaderBlacklist(List<int> blacklist)
		{
			int attackCooldown = 45;
			int attackCooldownMinusOne = attackCooldown - 1;
			int attackCoodlownPlusSixty = attackCooldown + 45;
			int attackCooldownPlusSixtyMinusOne = attackCoodlownPlusSixty - 1;
			int attackCooldownPlusOne = attackCooldown + 1;
			Player player = Main.player[Projectile.owner];
			if (player.active && Vector2.Distance(player.Center, Projectile.Center) > 2000f)
			{
				Projectile.ai[0] = 0f;
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] == -1f)
			{
				AI_GetMyGroupIndexAndFillBlackList(blacklist, out var index, out var totalIndexesInGroup);
				InvaderIdle(index, totalIndexesInGroup, out var idleSpot, out var idleRotation);
				Projectile.velocity = Vector2.Zero;
				Projectile.Center = Projectile.Center.MoveTowards(idleSpot, 32f);
				if (Projectile.Distance(idleSpot) < 2f)
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				return;
			}
			if (Projectile.ai[0] == 0f)
			{
					AI_GetMyGroupIndexAndFillBlackList(blacklist, out var index2, out var totalIndexesInGroup2);
					InvaderIdle(index2, totalIndexesInGroup2, out var idleSpot2, out var _);
					Projectile.velocity = Vector2.Zero;
					Projectile.Center = Vector2.SmoothStep(Projectile.Center, idleSpot2, 0.45f);
					if (Main.rand.Next(20) == 0)
					{
						int num6 = InvaderAttack(blacklist);
						if (num6 != -1)
						{
							Projectile.ai[0] = attackCooldown;
							Projectile.ai[1] = num6;
							Projectile.netUpdate = true;
							return;
						}
					}
	
			}
			int enemyWhoAmI = (int)Projectile.ai[1];
			if (!Main.npc.IndexInRange(enemyWhoAmI))
			{
				Projectile.ai[0] = 0f;
				Projectile.netUpdate = true;
				return;
			}
			NPC nPC = Main.npc[enemyWhoAmI];
			if (!nPC.CanBeChasedBy(this))
			{
				Projectile.ai[0] = 0f;
				Projectile.netUpdate = true;
				return;
			}
			Projectile.ai[0] -= 1f;
			if (Projectile.ai[0] >= (float)attackCooldownMinusOne)
			{
				Projectile.velocity *= 0.8f;
				if (Projectile.ai[0] == (float)attackCooldownMinusOne)
				{
					Projectile.localAI[0] = Projectile.Center.X;
					Projectile.localAI[1] = Projectile.Center.Y;
				}
				return;
			}
			float lerpValue = Utils.GetLerpValue(attackCooldownMinusOne, 0f, Projectile.ai[0], clamped: true);
			Vector2 ProjectileCenter = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
			if (lerpValue >= 0.5f)
			{
				ProjectileCenter = Main.player[Projectile.owner].Center;
			}
			Vector2 npcCenter = nPC.Center;
			float DistanceToRotation = (npcCenter - ProjectileCenter).ToRotation();
			float num10 = ((npcCenter.X > ProjectileCenter.X) ? (-MathF.PI) : (MathF.PI));
			float num11 = num10 + (0f - num10) * lerpValue * 2f;
			Vector2 spinningpoint = num11.ToRotationVector2();
			spinningpoint.Y *= MathF.Sin((float)Projectile.identity * 2.3f) * 0.5f;
			spinningpoint = spinningpoint.RotatedBy(DistanceToRotation);
			float num12 = (npcCenter - ProjectileCenter).Length() / 2f;
			Vector2 vector3 = (Projectile.Center = Vector2.Lerp(ProjectileCenter, npcCenter, 0.5f) + spinningpoint * num12);
			Vector2 vector4 = (Projectile.velocity = MathHelper.WrapAngle(DistanceToRotation + num11 + 0f).ToRotationVector2() * 10f);
			Projectile.position -= Projectile.velocity;
			Projectile.rotation = Projectile.velocity.ToRotation() * -player.direction;
			if (Projectile.ai[0] == 0f)
			{
				int num13 = InvaderAttack(blacklist);
				if (num13 != -1)
				{
					Projectile.ai[0] = attackCooldown;
					Projectile.ai[1] = num13;
					Projectile.netUpdate = true;
					return;
				}
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}

			return;
		}

		private int InvaderAttack(List<int> blackListedTargets, bool skipBodyCheck = false)
		{
			Vector2 center = Main.player[Projectile.owner].Center;
			int result = -1;
			float num = -1f;
			NPC ownerMinionAttackTargetNPC = Projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this))
			{
				bool flag = true;
				if (!ownerMinionAttackTargetNPC.boss && blackListedTargets.Contains(ownerMinionAttackTargetNPC.whoAmI))
				{
					flag = false;
				}
				if (ownerMinionAttackTargetNPC.Distance(center) > 1000f)
				{
					flag = false;
				}
				if (!skipBodyCheck && !Projectile.CanHitWithOwnBody(ownerMinionAttackTargetNPC))
				{
					flag = false;
				}
				if (flag)
				{
					return ownerMinionAttackTargetNPC.whoAmI;
				}
			}
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.CanBeChasedBy(this) && (nPC.boss || !blackListedTargets.Contains(i)))
				{
					float num2 = nPC.Distance(center);
					if (!(num2 > 1000f) && (!(num2 > num) || num == -1f) && (skipBodyCheck || Projectile.CanHitWithOwnBody(nPC)))
					{
						num = num2;
						result = i;
					}
				}
			}
			return result;
		}

		private void AI_GetMyGroupIndexAndFillBlackList(List<int> blackListedTargets, out int index, out int totalIndexesInGroup)
		{
			index = 0;
			totalIndexesInGroup = 0;
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.owner == Projectile.owner && projectile.type == Projectile.type && (projectile.type != 759 || projectile.frame == Main.projFrames[projectile.type] - 1))
				{
					if (Projectile.whoAmI > i)
					{
						index++;
					}
					totalIndexesInGroup++;
				}
			}
		}

		private void InvaderIdle(int stackedIndex, int totalIndexes, out Vector2 idleSpot, out float idleRotation)
		{
			Player player = Main.player[Projectile.owner];
			idleRotation = 0f;
			idleSpot = Vector2.Zero;
		
				float num2 = ((float)totalIndexes - 1f) / 2f;
				idleSpot = player.Center + -Vector2.UnitY.RotatedBy(4.3982296f / (float)totalIndexes * ((float)stackedIndex - num2)) * 40f;
			
		
		}

		public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.YellowStarDust, 1f);
                dust.noGravity = true;
            }
        }
    }
}
