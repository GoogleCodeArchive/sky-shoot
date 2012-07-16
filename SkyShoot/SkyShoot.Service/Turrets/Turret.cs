﻿using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Session;
using SkyShoot.XNA.Framework;
using SkyShoot.ServProgram.Mobs;
using SkyShoot.Contracts.Weapon;
using SkyShoot.Contracts.Service;

namespace SkyShoot.Service.Weapon.Bullets
{
	class Turret : ShootingMob
	{
		public AGameObject Owner;

		public Turret(float health, AWeapon weapon, int shootingDelay, AGameObject owner, Vector2 coordinates)
			: base(health, weapon, shootingDelay)
		{
			
			Weapon = weapon;
			Weapon.Owner = owner;
			this.Owner = owner;
			Target = null;
			Coordinates = coordinates;
			ObjectType = EnumObjectType.Turret;
			TeamIdentity = (owner.TeamIdentity);
			Radius = 10;
			Speed = 0f;
			ThinkCounter = 0;
			Id = Guid.NewGuid();
			MaxHealthAmount = HealthAmount = health;
			Damage = 20;
		}

		public override void FindTarget(List<AGameObject> targetObjects)
		{
			float distance = Constants.TURRET_TARGET_SEARCHING_RANGE;

			foreach (var obj in targetObjects)
			{
				if (obj.Id == Weapon.Owner.Id ||
					!obj.Is(EnumObjectType.LivingObject) ||
					obj.Is(EnumObjectType.Turret) ||
					obj.Id == Id) //todo: teams
				{
					continue;
				}
				float temp = Vector2.Distance(Coordinates, obj.Coordinates);

				if (distance > temp)
				{
					distance = temp;
					Target = obj;
				}
			}
		}

		public override IEnumerable<AGameEvent> Do(AGameObject obj, List<AGameObject> newObjects, long time)
		{
			return new AGameEvent[] { };
		}

		public override IEnumerable<AGameEvent> Think(List<AGameObject> gameObjects, List<AGameObject> newGameObjects, long time)
		{
			if (Target != null && (!Target.IsActive ||
									Vector2.Distance(Coordinates, Target.Coordinates) > Constants.TURRET_TARGET_SEARCHING_RANGE))
			{
				Target = null;
			}
			return base.Think(gameObjects, newGameObjects, time);
		}
	}
}
