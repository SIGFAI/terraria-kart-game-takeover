using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>A peel lobbed onto the ground: whoever slips on it spins out.</summary>
public class BananaPeelProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 22; Projectile.height = 16; Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Ranged; Projectile.penetrate = 3; Projectile.timeLeft = 1200;
        Projectile.tileCollide = true;
        Projectile.usesLocalNPCImmunity = true; Projectile.localNPCHitCooldown = 40;
    }

    public override void AI()
    {
        Projectile.velocity.Y = System.Math.Min(Projectile.velocity.Y + 0.3f, 10f);
        if (Projectile.velocity.Y == 0f) Projectile.velocity.X *= 0.8f;
        else Projectile.rotation += Projectile.velocity.X * 0.08f;
        if (Projectile.velocity.Y == 0f) Projectile.rotation = 0f;
    }

    public override bool OnTileCollide(Vector2 oldVelocity) => false;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        SpinOut.Apply(target, 60);
        Mix.Popup(Projectile.Top, "SLIP!", Color.Yellow);
    }

    public override void OnKill(int timeLeft) => Mix.Burst(Projectile.Center, DustID.GoldCoin, 12, 14f, 3f);
}
