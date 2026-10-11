using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class SigfMod : ModSystem
{
    static int Count(int type, Vector2 around, double tiles) => Mix.NpcsNear(around, tiles, n => n.type == type).Count;

    static void RaceTrack()
    {
        // Checkered start/finish line laid into the ground around the spawn.
        Vector2 c = Mix.Center;
        int x0 = (int)(c.X / 16f), y0 = (int)(c.Y / 16f);
        for (int dx = -8; dx <= 8; dx++)
            for (int row = 0; row < 2; row++) {
                int x = x0 + dx, y = y0 + row;
                if (!WorldGen.SolidTile(x, y)) continue;
                Tile t = Main.tile[x, y];
                t.TileType = ((dx + row) & 1) == 0 ? TileID.Marble : TileID.Obsidian;
                WorldGen.SquareTileFrame(x, y);
            }
    }

    static void Racers(int max)
    {
        Vector2 host = Mix.Host.Center;
        if (Count(ModContent.NPCType<KartRacer>(), host, 70) >= max) return;
        Vector2 spot = Mix.Ground(Mix.Host.Bottom + new Vector2(Main.rand.NextBool() ? -34 * 16 : 34 * 16, 0), 4);
        Mix.Spawn<KartRacer>(spot);
    }

    static void Box(Vector2 around, double radius)
    {
        if (Count(ModContent.NPCType<ItemBox>(), around, 40) >= 3) return;
        Vector2 spot = Mix.Ground(around, radius);
        NPC n = Mix.Spawn<ItemBox>(spot + new Vector2(0, -70));
        if (n != null) Mix.Burst(n.Center, DustID.RainbowMk2, 15, 16f, 3f, Color.Cyan);
    }

    public override void OnWorldLoad()
    {
        Mix.After(0.5, RaceTrack);
        Mix.Every(5, () => Racers(4));
        Mix.Every(7, () => Box(Mix.Host.Bottom, 16));

        // Demo: the kart world in 75 s.
        Mix.Demo(0.5, () => {
            Mix.Title("KART GAME TAKEOVER", "Ready... set...", 2.5);
            Mix.Arm<RedShell>();
            Box(Mix.Host.Bottom + new Vector2(Mix.Host.direction * 160, 0), 3);
            Box(Mix.Host.Bottom + new Vector2(Mix.Host.direction * 320, 0), 3);
        });
        Mix.Demo(3, () => {
            Mix.Title("GO!", "", 1.5);
            Mix.Sound("honk", Mix.Host.Center);
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(22 * 16, 0), 2));
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(30 * 16, 0), 2));
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(-24 * 16, 0), 2));
        });
        Mix.Demo(9, () => {
            Mix.Spawn(NPCID.Zombie, Mix.Ground(Mix.Host.Bottom + new Vector2(18 * 16, 0), 2));
            Mix.Spawn(NPCID.BlueSlime, Mix.Ground(Mix.Host.Bottom + new Vector2(-16 * 16, 0), 2));
        });
        Mix.Demo(15, () => {
            Mix.Title("TURBO!", "Run them over!", 2);
            Mix.Host.AddBuff(ModContent.BuffType<TurboBuff>(), 480);
            Mix.Sound("boost", Mix.Host.Center);
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(20 * 16, 0), 2));
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(-22 * 16, 0), 2));
        });
        Mix.Demo(26, () => {
            Mix.Title("BANANA TIME", "Watch your step", 2);
            Mix.Arm<BananaPeel>();
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(18 * 16, 0), 2));
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(-18 * 16, 0), 2));
            Mix.Spawn(NPCID.Zombie, Mix.Ground(Mix.Host.Bottom + new Vector2(24 * 16, 0), 2));
        });
        Mix.Demo(40, () => {
            Mix.Title("RED SHELLS!", "Homing and bouncy", 2);
            Mix.Arm<RedShell>();
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(20 * 16, 0), 2));
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(-20 * 16, 0), 2));
            Box(Mix.Host.Bottom, 8);
        });
        Mix.Demo(55, () => {
            Mix.Title("1ST PLACE?", "Not today, goblins", 2);
            Mix.Host.AddBuff(ModContent.BuffType<TurboBuff>(), 480);
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(20 * 16, 0), 2));
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(-20 * 16, 0), 2));
            Mix.Spawn<KartRacer>(Mix.Ground(Mix.Host.Bottom + new Vector2(26 * 16, 0), 2));
        });
    }
}
