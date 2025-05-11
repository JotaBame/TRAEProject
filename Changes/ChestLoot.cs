using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.PalladiumShield;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;
using System.Linq;
using TRAEProject.NewContent.Items.Weapons.Magic.MagicGrenade;
using TRAEProject.NewContent.Items.Accesories.AdvFlight;
using Terraria.UI;

public class ChestLoot : ModSystem
{
    public static int[] GoldChestItems;
    public static int[] PyramidItems;
    public static int[] ShadowItems;
    public static int[] DungeonItems;

    public override void PostSetupContent()
    {
        GoldChestItems = new int[] { ItemID.Mace, ItemID.MagicMirror, ItemID.HermesBoots, ItemID.BandofRegeneration, ItemID.ShoeSpikes, ItemID.LuckyHorseshoe, ItemID.Extractinator, ItemID.FlintlockPistol, ItemType<MagicGrenade>(), ItemID.LavaCharm };
        PyramidItems = new int[] { ItemID.SandstorminaBottle, ItemID.FlyingCarpet, ItemID.AnkhCharm, ItemID.AncientChisel, ItemID.SandBoots, ItemID.ThunderSpear, ItemID.ThunderStaff, ItemID.CatBast, ItemID.MagicConch };
        ShadowItems = new int[] { ItemID.HellwingBow, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.Sunfury, ItemType<PalladiumShield>(), ItemID.GravityGlobe };
        DungeonItems = new int[] { ItemID.Muramasa, ItemID.CobaltShield, ItemID.AquaScepter, ItemID.Handgun, ItemID.BlueMoon, ItemID.Valor, ItemID.MagicMissile };
    }
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];
            if (chest != null)
            {
                if (chest.item[0].type == ItemID.DarkLance)
                {
                    chest.item[0].SetDefaults(ItemType<PalladiumShield>(), false);
                }
                if (WorldGen.genRand.NextBool(2) && Main.tile[chest.x, chest.y].TileType == TileID.Containers && (Main.tile[chest.x, chest.y].TileFrameX == 15 * 36 || Main.tile[chest.x, chest.y].TileFrameX == 10 * 36))
                {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(ItemID.FlaskofPoison);
                            break;
                        }
                    }
                }
                if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        if (chest.item[i].type == ItemID.SandstorminaBottle || chest.item[i].type == ItemID.FlyingCarpet)
                        {
                            chest.item[i].SetDefaults(ItemID.PharaohsMask, false);
                            ++i;
                            chest.item[i].SetDefaults(ItemID.PharaohsRobe, false);
                            break;
                        }
                    }
                }
                if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36 
                    && chest.item[0].type != ItemID.FlareGun 
                    && chest.item[0].type != ItemID.SandstorminaBottle
                    && chest.item[0].type != ItemID.FlyingCarpet
                    && chest.item[0].type != ItemID.PharaohsMask
                    && chest.item[0].type != ItemID.PharaohsRobe)
                {
                    chest.item[0].SetDefaults(Main.rand.Next(GoldChestItems), false);
                    if (chest.item[0].type == ItemID.FlintlockPistol)
                    {
                        chest.item[1].SetDefaults(ItemID.MusketBall, false);
                        chest.item[1].stack = 100;
                    }
                    for (int i = 0; i < 40; i++)
                    {
                        if (chest.item[i].type == ItemID.None)
                        {

                            chest.item[i].SetDefaults(ItemID.FlaskofFire, false);
                            break;


                        }
                    }
                }

                if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 11 * 36)
                {
                    if (WorldGen.genRand.NextBool(8))
                    {
                        chest.item[0].SetDefaults(ItemID.FrostStaff, false);
                    }
                }
                if (Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == 10 * 36)
                {
                    int pyramiditem = Main.rand.Next(PyramidItems);
                    chest.item[0].SetDefaults(pyramiditem, false);
                    if (WorldGen.genRand.NextBool(3))
                    {
                        for (int i = 0; i < 40; i++)
                        {
                            if (chest.item[i].IsAir)
                            {
                                chest.item[i].SetDefaults(ItemID.MysticCoilSnake);
                                break;
                            }
                        }
                    }

                }

      

                if (chest.item[0].type == ItemID.Muramasa || chest.item[0].type == ItemID.CobaltShield || chest.item[0].type == ItemID.AquaScepter || chest.item[0].type == ItemID.Handgun || chest.item[0].type == ItemID.BlueMoon || chest.item[0].type == ItemID.Valor)
                {
                    if(Main.rand.NextBool(4))
                    {
                        for (int i = 0; i < 40; i++)
                        {
                            if (chest.item[i].IsAir)
                            {
                                chest.item[i].SetDefaults(ItemType<AdvFlightSystem>());
                                break;
                            }
                        }
                    }
                }
                if (chest.item[0].type == ItemID.LuckyHorseshoe || chest.item[0].type == ItemID.CelestialMagnet || chest.item[0].type == ItemID.Starfury || chest.item[0].type == ItemID.ShinyRedBalloon)
                {

                    for (int i = 0; i < 40; i++)
                    {
                        if (chest.item[i].type == ItemID.CreativeWings)
                        {
                            chest.item[0].SetDefaults(ItemID.None, false);

                        }
                    }

                }
            
                if (chest.item[0].type == ItemID.LuckyHorseshoe && Main.tile[chest.x, chest.y].TileFrameX != 1 * 36 )
                {
                    chest.item[0].SetDefaults(ItemID.CreativeWings);

                }
            }
        }
    }
}




