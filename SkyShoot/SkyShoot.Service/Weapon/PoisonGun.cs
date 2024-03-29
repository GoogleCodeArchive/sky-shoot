﻿using System;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Service.Weapon.Bullets;
using SkyShoot.XNA.Framework;

namespace SkyShoot.Service.Weapon
{
	class PoisonGun : AWeapon
	{
		public PoisonGun(Guid id, AGameObject owner = null)
			: base(id, owner)
		{
			WeaponType = WeaponType.PoisonGun;
			ReloadSpeed = Constants.POISON_GUN_ATTACK_RATE;
		}

		public override AGameObject[] CreateBullets(Vector2 direction)
		{
			var bullets = new AGameObject[] { new PoisonBullet(Owner, Guid.NewGuid(), direction) };
			return bullets;
		}
	}
}