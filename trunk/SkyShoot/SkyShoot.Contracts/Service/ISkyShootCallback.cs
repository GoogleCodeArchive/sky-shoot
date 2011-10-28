﻿using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Weapon.Projectiles;

namespace SkyShoot.Contracts.Service
{
    public interface ISkyShootCallback
    {
        void GameStart(AMob mob, GameLevel arena);

        void Shoot(AProjectile projectile);

        void SpawnMob(AMob mob);

        void Hit(AMob mob, AProjectile projectile);

        void MobDead(AMob mob);

        void BonusDropped(AObtainableDamageModifier bonus);

        void BonusExpired(AObtainableDamageModifier bonus);

        void BonusDisappeared(AObtainableDamageModifier bonus);

        void GameOver();

        void PlayerLeft(AMob mob);

        void SynchroFrame(AMob[] mob);
    }
}