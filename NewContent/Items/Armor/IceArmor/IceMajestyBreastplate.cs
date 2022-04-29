using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TRAEProject.Common;
using Terraria.Audio;

namespace TRAEProject.NewContent.Items.Armor.IceArmor
{
	[AutoloadEquip(EquipType.Body)]
    public class IceMajestyBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glacial Breastplate");
			Tooltip.SetDefault("15% increased summon damage\n30% increased whip range");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.value = Item.sellPrice(0, 30, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.width = 32;
			Item.height = 26;
			Item.defense = 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemType<IceQueenJewel>(), 1)
				.AddIngredient(ItemID.SpectreBar, 20)
				.AddIngredient(ItemID.Silk, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void UpdateEquip(Player player)
		{
			player.whipRangeMultiplier += 0.3f;
			player.GetDamage<SummonDamageClass>() += 0.15f;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemType<IceMajestyCrown>() && legs.type == ItemType<IceMajestyLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Summons three Mad Flockos to fight for you";
			player.GetModPlayer<IceMajestySetBonus>().MadFlockoSetBonus = true;
		}
        public override void EquipFrameEffects(Player player, EquipType type)
        {
            player.back = (sbyte)TRAEProj.IceMajestyCape;
        }
    }

    public class IceMajestySetBonus : ModPlayer
	{
		public bool MadFlockoSetBonus;
		public override void ResetEffects()
		{
			MadFlockoSetBonus = false;
		}
        public override void PostUpdate()
        {
			if (MadFlockoSetBonus == true)
			{
				Player.AddBuff(BuffType<MadFlockos>(), 1);
				if (Player.ownedProjectileCounts[ProjectileType<MadFlocko>()] < 3)
				{
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0, 0, ProjectileType<MadFlocko>(), 15, 1f, Player.whoAmI, 0f, 0f);
				}
			}

        }  
    }
    public class MadFlockos : ModBuff
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flocko Flock");
			Description.SetDefault("The Mad Flockos will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			//if (player.ownedProjectileCounts[ProjectileType<MadFlocko>()] > 0)
			//{
			//	player.GetModPlayer<IceMajestySetBonus>().MadFlocko = true;
			//}
			//if (!player.GetModPlayer<IceMajestySetBonus>().MadFlocko)
			//{
			//	player.DelBuff(buffIndex);
			//	buffIndex--;
			//}
			//else
			//{
			//	player.buffTime[buffIndex] = 18000;
			//}
		}	
	}


	public class MadFlocko : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MadFlocko");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true; //This is necessary for right-click targeting
		}

		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 32;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Main.projFrames[Projectile.type] = 1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2;
			Projectile.GetGlobalProjectile<ProjectileStats>().IgnoresDefense = true;
			Projectile.tileCollide = false;
			Projectile.minionSlots = 0; // is this needed? Wouldn't the default value be 0 already?
			Projectile.timeLeft = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
            ProjectileID.Sets.MinionShot[Type] = true;
        }
        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int[] array = Projectile.localNPCImmunity;
            for (int i = 0; i < 200; i++)
            {
                NPC nPC = Main.npc[i];
                array[i] = 10;
                nPC.immune[Projectile.owner] = 0;
                if (Projectile.ai[0] > 0f)
                {
                    Projectile.ai[0] = -1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
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
        public override void AI() // this is actually Blade Staff's AI, except a little modified
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.Top + new Vector2(0f, -30f);
            if (player.dead)
            {
                player.GetModPlayer<IceMajestySetBonus>().MadFlockoSetBonus = false;
            }
            if (player.GetModPlayer<IceMajestySetBonus>().MadFlockoSetBonus)
            {
                Projectile.timeLeft = 2;
            }
        
            	if (Projectile.ai[0] == 0f)
            	{
            		AI_GetMyGroupIndexAndFillBlackList(null, out var index, out var totalIndexesInGroup);
            		float num2 = (float)Math.PI * 2f / (float)totalIndexesInGroup;
            		float num3 = totalIndexesInGroup * 0.66f;
            		Vector2 value = new Vector2(30f, 6f) / 5f * (totalIndexesInGroup - 1);
            		Vector2 value2 = Vector2.UnitY.RotatedBy(num2 * (float)index + Main.GlobalTimeWrappedHourly % num3 / num3 * ((float)Math.PI * 2f));
            		vector += value2 * value;
            		vector.Y += player.gfxOffY;
            		vector = vector.Floor();
            	}

            	if (Projectile.ai[0] == 0f)
            	{
            		Vector2 velocity = vector - Projectile.Center;
            		float num4 = 10f;
            		float lerpValue = Utils.GetLerpValue(200f, 600f, velocity.Length(), clamped: true);
            		num4 += lerpValue * 30f;
            		if (velocity.Length() >= 3000f)
            	{
            			Projectile.Center = vector;
            		}
            		Projectile.velocity = velocity;
            		if (Projectile.velocity.Length() > num4)
            		{
            			Projectile.velocity *= num4 / Projectile.velocity.Length();
            		}
            	int startAttackRange = 800;
            	int attackTarget = -1;
            		Projectile.Minion_FindTargetInRange(startAttackRange, ref attackTarget, skipIfCannotHitWithOwnBody: false);
            		if (attackTarget != -1)
            		{
            			Projectile.ai[0] = 60f;
            			Projectile.ai[1] = attackTarget;
            			Projectile.netUpdate = true;
            		}
            		float targetAngle = Projectile.velocity.SafeNormalize(Vector2.UnitY).ToRotation() + (float)Math.PI / 2f;
            	if (velocity.Length() < 40f)
            		{
            			targetAngle = Vector2.UnitY.ToRotation() + (float)Math.PI / 2f;
            		}
            		Projectile.rotation = Projectile.rotation.AngleLerp(targetAngle, 0.2f);
            	return;
            	}
            	if (Projectile.ai[0] == -1f)
            	{
            		if (Projectile.ai[1] == 0f)
            	{
            			SoundEngine.PlaySound(0, (int)Projectile.position.X, (int)Projectile.position.Y);
            			for (int i = 0; i < 2; i++)
            			{
            				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 15, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f, 0, default(Color), 1.4f);
            				if (Main.rand.Next(3) != 0)
                        {
                            dust.scale *= 1.3f;
                            dust.velocity *= 1.1f;
                        }
                        dust.noGravity = true;
                        dust.fadeIn = 0f;
                    }
                    Projectile.velocity += Main.rand.NextVector2CircularEdge(4f, 4f);
                }
                Projectile.ai[1] += 0.625f;
                Projectile.rotation += Projectile.velocity.X * 0.1f + Projectile.velocity.Y * 0.05f;
                Projectile.velocity *= 0.92f;
                if (Projectile.ai[1] >= 9f)
                {
                    Projectile.ai[0] = 0f;
                    Projectile.ai[1] = 0f;
                }
                return;
            }
            NPC nPC = null;
            int num5 = (int)Projectile.ai[1];
            if (Main.npc.IndexInRange(num5) && Main.npc[num5].CanBeChasedBy(this))
            {
                nPC = Main.npc[num5];
            }
            if (nPC == null)
            {
                Projectile.ai[0] = -1f;
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
            else if (player.Distance(nPC.Center) >= 900f)
            {
                Projectile.ai[0] = 0f;
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
            else
            {
                Vector2 velocity2 = nPC.Center - Projectile.Center;
                float num6 = 16f;
                Projectile.velocity = velocity2;
                if (Projectile.velocity.Length() > num6)
                {
                    Projectile.velocity *= num6 / Projectile.velocity.Length();
                }
                float targetAngle2 = Projectile.velocity.SafeNormalize(Vector2.UnitY).ToRotation() + (float)Math.PI / 2f;
                Projectile.rotation = Projectile.rotation.AngleLerp(targetAngle2, 0.4f);
            }
            float num7 = 0.1f;
            float num8 = Projectile.width * 5;
            for (int j = 0; j < 1000; j++)
            {
                if (j != Projectile.whoAmI && Main.projectile[j].active && Main.projectile[j].owner == Projectile.owner && Main.projectile[j].type == Projectile.type && Math.Abs(Projectile.position.X - Main.projectile[j].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[j].position.Y) < num8)
                {
                    if (Projectile.position.X < Main.projectile[j].position.X)
                    {
                        Projectile.velocity.X -= num7;
                    }
                    else
                    {
                        Projectile.velocity.X += num7;
                    }
                    if (Projectile.position.Y < Main.projectile[j].position.Y)
                    {
                        Projectile.velocity.Y -= num7;
                    }
                    else
                    {
                        Projectile.velocity.Y += num7;
                    }
                }
            }
        }
    }
}




