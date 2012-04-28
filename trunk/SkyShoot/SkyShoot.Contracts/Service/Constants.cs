﻿
namespace SkyShoot.Contracts
{
	public static class Constants
	{
		#region weapons

		#region pistol

		public const float PISTOL_BULLET_SPEED = 0.1f;
		public const float PISTOL_BULLET_DAMAGE = 10;
		public const float PISTOL_BULLET_LIFE_DISTANCE = 3000;
		public const int PISTOL_ATTACK_RATE = 400;

		#endregion

		#region rocketpistol

		public const float ROCKET_BULLET_SPEED = 0.1f;
		public const float ROCKET_BULLET_DAMAGE = 3f;
		public const float ROCKET_BULLET_LIFE_DISTANCE = 3000f;
		public const int ROCKET_PISTOL_ATTACK_RATE = 1500;
		public const float ROCKET_BULLET_RADIUS = 4f;

		#endregion

		#region shotgun

		public const float SHOTGUN_BULLET_SPEED = 0.2f;
		public const float SHOTGUN_BULLET_DAMAGE = 2f;
		public const float SHOTGUN_BULLET_LIFE_DISTANCE = 70f;
		public const int SHOTGUN_ATTACK_RATE = 500;

		#endregion

		#region flame

		public const int FLAME_BULLETS_COUNT = 16;
		public const float FLAME_SPEED = 0.05f;
		public const float FLAME_DAMAGE = 0.5f;
		public const float FLAME_LIFE_DISTANCE = 40f;
		public const int FLAME_ATTACK_RATE = 5;

		#endregion

		#region explosion

		public const float EXPLOSION_SPEED = 0f;
		public const float EXPLOSION_DAMAGE = 40f;
		public const float EXPLOSION_LIFE_DISTANCE = 200f;
		public const int EXPLOSION_ATTACK_RATE = 1;
		public const float EXPLOSION_RADIUS = 50f;

		#endregion

		#region heater

		public const float HEATER_BULLET_SPEED = 0.5f;
		public const float HEATER_BULLET_DAMAGE = 100;
		public const float HEATER_BULLET_LIFE_DISTANCE = 3000;
		public const int HEATER_ATTACK_RATE = 2000;

		#endregion

		public const int CLAW_ATTACK_RATE = 1000; // В миллисекундах.		public const float PLAYER_DEFAULT_HEALTH = 100;

		#endregion

		#region mobs

		public const float PLAYER_DEFAULT_SPEED = 0.05f;
		public const float PLAYER_RADIUS = 15f;
		public const float SPIDER_SPEED = 0.06f;

		#endregion

		#region common
		public const float Epsilon = 0.01f;

		public const int FPS = 1000 / 60;
		public const float LEVELBORDER = 50;

		#endregion

		#region bonuses
		public const int BONUS_TYPE_COUNT = 5;

		public const int SHIELD_MILLISECONDS = 30000;
		public const int DOUBLE_DAMAGE_MILLISECONDS = 30000;
		public const int REMEDY_MILLISECONDS = 30000; // redundant
		public const int MIRROR_MILLISECONDS = 30000; // redundant
		public const int SPEEDUP_MILLISECONDS = 30000;
		#endregion
	}
}