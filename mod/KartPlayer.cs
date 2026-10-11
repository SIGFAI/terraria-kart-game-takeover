using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Every player drives a kart: faster running, kart crashes hurt enemies, driving through item boxes opens them.</summary>
public class KartPlayer : ModPlayer
{
    public override void PostUpdateRunSpeeds()
    {
        Player.maxRunSpeed += 3.5f;
        Player.runAcceleration *= 2f;
        Player.runSlowdown *= 0.6f;
    }

    public override void PostUpdate()
    {
        if (Player.whoAmI != Main.myPlayer || Player.dead) return;
        int kart = ModContent.ProjectileType<PlayerKart>();
        bool has = false;
        for (int i = 0; i < Main.maxProjectiles; i++) {
            Projectile p = Main.projectile[i];
            if (p.active && p.owner == Player.whoAmI && p.type == kart) { has = true; break; }
        }
        if (!has)
            Projectile.NewProjectile(Player.GetSource_Misc("kart"), Player.Center, Vector2.Zero, kart, 0, 0f, Player.whoAmI);

        Rectangle me = Player.getRect();
        me.Inflate(14, 6);
        bool turbo = Player.HasBuff<TurboBuff>();
        for (int i = 0; i < Main.maxNPCs; i++) {
            NPC n = Main.npc[i];
            if (!n.active || !n.getRect().Intersects(me)) continue;
            if (n.type == ModContent.NPCType<ItemBox>()) {
                n.SimpleStrikeNPC(99, Player.direction);
                continue;
            }
            if (!Mix.IsHostile(n) || n.immune[Player.whoAmI] > 0 || System.Math.Abs(Player.velocity.X) < 5f) continue;
            n.SimpleStrikeNPC(turbo ? 34 : 14, Player.direction, false, 8f);
            n.immune[Player.whoAmI] = 25;
            SpinOut.Apply(n, 40);
            Mix.Burst(n.Center, DustID.Torch, 14, 20f, 5f);
            Mix.Shake(6, 0.25);
        }
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        drawInfo.Position.Y += 3f;
        drawInfo.drawPlayer.legFrame.Y = 0;
    }
}
