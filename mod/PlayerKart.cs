using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>The blue go-kart every player drives, glued under their feet.</summary>
public class PlayerKart : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 60; Projectile.height = 24; Projectile.tileCollide = false;
        Projectile.penetrate = -1; Projectile.timeLeft = 5; Projectile.netImportant = true;
        Projectile.ignoreWater = true;
    }

    public override void AI()
    {
        Player o = Main.player[Projectile.owner];
        if (!o.active || o.dead) { Projectile.Kill(); return; }
        Projectile.timeLeft = 5;
        Projectile.direction = o.direction;
        Projectile.Center = o.Bottom + new Vector2(0f, -14f + o.gfxOffY);
        Projectile.velocity = Vector2.Zero;

        float speed = System.Math.Abs(o.velocity.X);
        if (speed > 3f && o.velocity.Y == 0f && Main.rand.NextBool(2)) {
            Vector2 tail = Projectile.Center + new Vector2(-o.direction * 38f, 8f);
            Dust d = Dust.NewDustPerfect(tail, DustID.Smoke, new Vector2(-o.direction * 1.5f, -0.4f), 100, Color.Gray, 1.2f);
            d.noGravity = true;
            if (speed > 5f) {
                Dust f = Dust.NewDustPerfect(tail, DustID.Torch, new Vector2(-o.direction * 2.5f, 0f), 0, default, 1.1f);
                f.noGravity = true;
            }
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        return false; // drawn by KartLayer, in front of the driver's legs
    }
}
