using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TRAEProject.NewContent.Items.Armor.Joter;
using TRAEProject.Changes.Weapon.Melee.SpearProjectiles;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Weapon
{
	
	class SpearItems : GlobalItem
	{
		public override bool InstancePerEntity => true;
		public override GlobalItem Clone(Item item, Item itemClone)
		{
			return base.Clone(item, itemClone);
        }
		public static int[] meleePrefixes = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 59, 60, 61, 81 };

        public int altShoot = -1;
		public bool canGetMeleeModifiers = false;
		public override void SetDefaults(Item item)
		{ 
			switch (item.type)
			{

				case ItemID.Spear:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<BasicSpear>();
					altShoot = ProjectileType<BasicSpearThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.useTime = item.useAnimation = 27;
					item.shootSpeed = 6; //only the throw uses this
					break;

				case ItemID.TheRottedFork:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<RottedFork>();
					altShoot = ProjectileType<RottedForkThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 21;
					item.shootSpeed = 6.5f; //only the throw uses this
					break;

				case ItemID.BoneJavelin:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<BoneSpear>();
					altShoot = ProjectileType<BoneSpearThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.useTime = item.useAnimation = 20;
					item.damage = 16;
					item.DamageType = DamageClass.Melee;
					item.autoReuse = false;
					item.consumable = false;
					item.maxStack = 1;
					item.value = Item.sellPrice(silver: 30);
					break;
				case ItemID.Javelin:
					item.useStyle = 1; item.DamageType = DamageClass.MeleeNoSpeed;

                    canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Javelin>();
					altShoot = ProjectileType<JavelinThrow>();
					item.useTime = item.useAnimation = 18;
					item.DamageType = DamageClass.Melee;
					item.autoReuse = false;
					item.consumable = false;
					item.maxStack = 1;
					item.value = Item.sellPrice(silver: 30);
					break;
				case ItemID.ThunderSpear:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<StormSpear>();
					altShoot = ProjectileType<StormSpearThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 18;
					item.shootSpeed = 7; //only the throw uses this
					break;
				case ItemID.Trident:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Trident>();
					altShoot = ProjectileType<TridentThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.shootSpeed = 9; //only the throw uses this
					break;
				case ItemID.DarkLance:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<DarkLance>();
					altShoot = ProjectileType<DarkLanceThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.shootSpeed = 11.5f; //only the throw uses this
					item.damage = 21;
					item.useTime = item.useAnimation = 26;
					item.value = Item.sellPrice(silver: 50);
					break;
				case ItemID.Swordfish:
					item.useStyle = 1; item.DamageType = DamageClass.MeleeNoSpeed;

                    canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Swordfish>();
					altShoot = ProjectileType<SwordfishThrow>();
					break;
				case ItemID.ObsidianSwordfish:
					item.useStyle = 1;
					canGetMeleeModifiers = true; item.DamageType = DamageClass.MeleeNoSpeed;

                    item.shoot = ProjectileType<ObsidianSwordfish>();
					altShoot = ProjectileType<ObsidianSwordfishThrow>();
					item.crit = 0;
					item.damage = 22;
					item.shootSpeed = 10; //only the throw uses this
					break;
				case ItemID.CobaltNaginata:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<CobaltNaginata>();
					altShoot = ProjectileType<CobaltNaginataThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 34;
					item.useTime = item.useAnimation = 24;
					item.shootSpeed = 13; //only the throw uses this
					item.crit = 24;
					break;
				case ItemID.PalladiumPike:
					item.useStyle = 1; item.DamageType = DamageClass.MeleeNoSpeed;

                    canGetMeleeModifiers = true;
					item.shoot = ProjectileType<PalladiumPike>();
					altShoot = ProjectileType<PalladiumPikeThrow>();
					item.useTime = item.useAnimation = 34;
					item.channel = true;
					item.damage = 45;
					break;
				case ItemID.MythrilHalberd:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<MythrilHalberd>();
					altShoot = ProjectileType<MythrilHalberdThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 49;
					item.channel = true;
					break;
				case ItemID.OrichalcumHalberd:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<OrichalcumHookbill>();
					altShoot = ProjectileType<OrichalcumHookbillThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;


                    item.damage = 40;
					item.channel = true;
					item.SetNameOverride("Orichalcum Billhook");
					break;
				case ItemID.AdamantiteGlaive:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<AdamantiteGlaive>();
					altShoot = ProjectileType<AdamantiteGlaiveThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 50;
					item.shootSpeed = 10; //only the throw uses this
					break;
				case ItemID.TitaniumTrident:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<TitaniumTrident>();
					altShoot = ProjectileType<TitaniumTridentThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 47;
					item.shootSpeed = 14; //only the throw uses this
					item.useTime = item.useAnimation = 25;
					break;
				case ItemID.Gungnir:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Gungnir>();
					altShoot = ProjectileType<GungnirThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 56;
					item.shootSpeed = 12; //only the throw uses this
					item.useAnimation = 22;
					item.useTime = 33;
					break;
				case ItemID.ChlorophytePartisan:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<ChloroPartisan>();
					altShoot = ProjectileType<ChloroPartisanThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.shootSpeed = 12f; //only the throw uses this
					item.useTime = item.useAnimation = 30;
					break;
				case ItemID.MonkStaffT2:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<GhastlyGlaive>();
					altShoot = ProjectileType<GhastlyGlaiveThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 70;
					item.shootSpeed = 12; //only the throw uses this
					item.channel = false;
					break;
				case ItemID.MushroomSpear:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<MushroomSpear>();
					altShoot = ProjectileType<MushroomSpearThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.shootSpeed = 8; //only the throw uses this
					item.damage = 100;
					item.useTime = item.useAnimation = 50;
					item.value = Item.buyPrice(platinum: 2);
					break;
				case ItemID.NorthPole:
					item.useStyle = 1;
					canGetMeleeModifiers = true; item.DamageType = DamageClass.MeleeNoSpeed;

                    item.shoot = ProjectileType<NorthPole>();
					altShoot = ProjectileType<NorthPoleThrow>();
                    item.shootSpeed = 9; //only the throw uses this
					item.damage = 100;
					item.useTime = item.useAnimation = 24;
					break;
				case ItemID.ScourgeoftheCorruptor:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<SoTC>();
					altShoot = ProjectileType<SoTCThrow>();
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 62;
					item.useTime = item.useAnimation = 24;
					item.autoReuse = false;
					item.shootSpeed = 9;
					break;
				case ItemID.DayBreak:
					item.useStyle = 1;
					canGetMeleeModifiers = true;
					item.shoot = ProjectileType<Daybreak>();
					altShoot = ProjectileType<DaybreakThrow>();
					item.shootSpeed = 12.5f; //only the throw uses this
                    item.DamageType = DamageClass.MeleeNoSpeed;

                    item.damage = 94;
					item.useTime = item.useAnimation = 20;
					item.autoReuse = false;
					break;
			}
			if (item.type == ItemType<JoterTrident>())
			{
				canGetMeleeModifiers = true;

				item.damage = 100;
				item.shootSpeed = 12; //only the throw uses this
				item.useTime = item.useAnimation = 24;
				item.shoot = ProjectileType<JoterTridentSpear>();
				altShoot = ProjectileType<JoterTridentThrow>();
				item.useStyle = 1;
				item.DamageType = DamageClass.MeleeNoSpeed;
				item.autoReuse = false;
				item.rare = ItemRarityID.Cyan;
				item.maxStack = 1;
				item.noMelee = true;
				item.noUseGraphic = true;
				item.value = Item.sellPrice(silver: 30);
			}
		}
	
		public override float UseAnimationMultiplier(Item item, Player player)
		{
			if (player.altFunctionUse != 2 && altShoot != -1) // is a spear and it's not right clicking
				return 1 / player.GetAttackSpeed(DamageClass.Melee);
			return base.UseSpeedMultiplier(item, player);
		}
		public override bool AltFunctionUse(Item item, Player player)
		{
			if (altShoot != -1)
			{
				return true;
			}
			return base.AltFunctionUse(item, player);
		}
		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (altShoot != -1)
			{
				if (player.altFunctionUse == 2)
				{
					type = altShoot;
					player.itemAnimation = player.itemAnimationMax = item.useTime / 3;
				}
				else if (item.type == ItemID.Trident)
				{
					type = ProjectileType<Trident>();
				}
			}
        }
		
        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if(canGetMeleeModifiers)
            {
				
				#region pick
				int num9 = rand.Next(40);
                if (num9 == 0)
				{
					return 1;
				}
				if (num9 == 1)
				{
					return 2;
				}
				if (num9 == 2)
				{
					return 3;
				}
				if (num9 == 3)
				{
					return 4;
				}
				if (num9 == 4)
				{
					return 5;
				}
				if (num9 == 5)
				{
					return 6;
				}
				if (num9 == 6)
				{
					return 7;
				}
				if (num9 == 7)
				{
					return 8;
				}
				if (num9 == 8)
				{
					return 9;
                }       
				if (num9 == 9)
				{
					return 10;
				}
				if (num9 == 10)
				{
					return 11;
				}
				if (num9 == 11)
				{
					return 12;
				}
				if (num9 == 12)
				{
					return 13;
				}
				if (num9 == 13)
				{
					return 14;
				}
				if (num9 == 14)
				{
					return 15;
				}
				if (num9 == 15)
				{
					return 36;
				}
				if (num9 == 16)
				{
					return 37;
				}


				if (num9 == 17)
				{
					return 38;
				}
				if (num9 == 18)
				{
					return 53;
				}
				if (num9 == 19)
				{
					return 54;
				}
				if (num9 == 20)
				{
					return 55;
				}
				if (num9 == 21)
				{
					return 39;
				}
				if (num9 == 22)
				{
					return 40;
				}
				if (num9 == 23)
				{
					return 56;
				}
				if (num9 == 24)
				{
					return 41;
				}
				if (num9 == 25)
				{
					return 57;
                }

				if (num9 == 26)
				{
					return 42;
				}
				if (num9 == 27)
				{
					return 43;
				}
				if (num9 == 28)
				{
					return 44;
				}
				if (num9 == 29)
				{
					return 45;
				}
				if (num9 == 30)
				{
					return 46;
				}
				if (num9 == 31)
				{
					return 47;
				}



				if (num9 == 32)
				{
					return 48;
				}
				if (num9 == 33)
				{
					return 49;
                }                                
				if (num9 == 34)
				{
					return 50;
				}
				if (num9 == 35)
				{
					return 51;
				}
				if (num9 == 36)
				{
					return 59;
				}
				if (num9 == 37)
				{
					return 60;
				}
				if (num9 == 38)
				{
					return 61;
				}

				if (num9 == 39)
				{
					return PrefixID.Legendary;
				}
				#endregion
            }
            return -1;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.PalladiumPike:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nGreatly increases life regeneration after striking an enemy\nMore effective with Palladium armor equipped";
                        }
                    }
                    break; 

                case ItemID.MythrilHalberd:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nDeals 25% more damage on critical hits";
                        }
                    }
                    break;
            }
        }
    }
}
