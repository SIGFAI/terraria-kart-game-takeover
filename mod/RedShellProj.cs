using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Homing shell that bounces off blocks and explodes on enemies.</summary>
public class RedShellProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 20; Projectile.height = 18; Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Ranged; Projectile.penetrate = 1; Projectile.timeLeft = 300;
        Projectile.tileCollide = true;
    }

    public override void AI()
    {
        NPC target = Mix.Nearest(Projectile.Center, 28, Mix.IsHostile);
        float speed = 10f;
        if (target != null) {
            Vector2 want = Mix.Aim(Projectile.Center, target.Center, speed);
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, want, 0.14f);
        } else Projectile.velocity.Y += 0.1f;
        if (Projectile.velocity.LengthSquared() > 0.1f) Projectile.velocity = Vector2.Normalize(Projectile.velocity) * speed;
        Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
        Projectile.rotation = Projectile.velocity.X * 0.03f;
        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, -Projectile.velocity * 0.2f, 0, default, 1.3f);
        d.noGravity = true;
        Mix.Light(Projectile.Center, new Color(255, 90, 40));
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.ai[1]++;
        if (Projectile.ai[1] > 4) return true;
        if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
        if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
        Mix.Burst(Projectile.Center, DustID.Smoke, 4, 6f, 2f);
        Mix.Sound(SoundID.Item56, Projectile.Center);
        return false;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2f, 1f,
            Projectile.spriteDirection > 0 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally : 0, 0);
        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SpinOut.Apply(target, 55);
        foreach (NPC n in Mix.Hostiles(Projectile.Center, 4)) {
            if (n.whoAmI == target.whoAmI) continue;
            n.SimpleStrikeNPC(Projectile.damage / 2, n.Center.X > Projectile.Center.X ? 1 : -1, false, 4f);
            SpinOut.Apply(n, 40);
        }
    }

    public override void OnKill(int timeLeft)
    {
        Mix.Burst(Projectile.Center, DustID.Torch, 30, 20f, 6f, null, 1.8f);
        Mix.Burst(Projectile.Center, DustID.Smoke, 14, 20f, 4f);
        Mix.Sound(SoundID.Item14, Projectile.Center);
        Mix.Shake(5, 0.25);
    }
}
