using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.ServiceModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Perks;
using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;

using SkyShoot.Contracts.Weapon.Projectiles;
using SkyShoot.Game.Controls;
using SkyShoot.Game.Screens;

using SkyShoot.Game.Client.View;

using SkyShoot.Contracts.Mobs;

using System.Security.Cryptography;

namespace SkyShoot.Game.Client.Game
{
	public class HashHelper
	{
		public static string GetMd5Hash(string input)
			// ����� ������������������ �� 32 ����������������� ���� (md5 ��� �� ���������)
		{
			MD5 md5Hasher = MD5.Create();

			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

			StringBuilder sBuilder = new StringBuilder();

			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}

	public sealed class GameController :// ISkyShootCallback, 
		ISkyShootService
	{
		AudioEngine engine;
		SoundBank soundBank;
		WaveBank waveBank;

		public static Guid MyId { get; private set; }

		public bool IsGameStarted { get; private set; }

		private static GameController _localInstance;

		private ISkyShootService _service;

		public static GameController Instance
		{
			get
			{
				if (_localInstance == null)
					_localInstance = new GameController();
				return _localInstance;
			}
		}
		public GameModel GameModel { get; private set; }

		private GameController()
		{
			InitConnection();
		}

		#region ������ callbacks

		public void GameStart(AGameObject[] mobs, Contracts.Session.GameLevel arena)
		{
			ScreenManager.Instance.SetActiveScreen(typeof (GameplayScreen)); // = ScreenManager.ScreenEnum.GameplayScreen;

			GameModel = new GameModel(GameFactory.CreateClientGameLevel(arena));

			foreach (AGameObject mob in mobs)
			{
				var clientMob = GameFactory.CreateClientMob(mob);
				GameModel.AddMob(clientMob);
			}

			// GameModel initialized, set boolean flag  
			IsGameStarted = true;
		}

		public void SpawnMob(AGameObject mob)
		{
			var clientMob = GameFactory.CreateClientMob(mob);
			GameModel.AddMob(clientMob);
		}

		public void Hit(AGameObject mob, AProjectile projectile)
		{
			if (projectile != null)
				GameModel.RemoveProjectile(projectile.Id);
			GameModel.GetMob(mob.Id).HealthAmount = mob.HealthAmount;
		}

		public void MobDead(AGameObject mob)
		{
			var random = new Random();

			engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			soundBank = new SoundBank(engine, "Content\\Sounds\\Sound Bank.xsb");
			waveBank = new WaveBank(engine, "Content\\Sounds\\Wave Bank.xwb");

			//Cue cue = soundBank.GetCue("angry01");
			Cue cueDeath = soundBank.GetCue("guts04a");
			GameModel.RemoveMob(mob.Id);
			GameModel.GameLevel.AddTexture(mob.IsPlayer ? Textures.DeadPlayerTexture : Textures.DeadSpiderTexture,
			                               TypeConverter.XnaLite2Xna(mob.Coordinates));
			cueDeath.Play();
		}

		public void MobMoved(AGameObject mob, XNA.Framework.Vector2 direction)
		{
			GameModel.GetMob(mob.Id).RunVector = direction; // TypeConverter.Vector2_m2s(direction);
		}

		public void BonusDropped(AObtainableDamageModifier bonus)
		{
			throw new NotImplementedException();
		}

		public void BonusExpired(AObtainableDamageModifier bonus)
		{
			var player = GameModel.GetMob(MyId);
			player.State &= ~bonus.Type;
		}

		public void BonusDisappeared(AObtainableDamageModifier bonus)
		{
			throw new NotImplementedException();
		}

		public void GameOver()
		{
			GameModel = null;
			ScreenManager.Instance.SetActiveScreen(typeof (MainMenuScreen));
			IsGameStarted = false;
		}

		public void PlayerLeft(AGameObject mob)
		{
			//TODO! issue 26? 

			/*for (int i = 0; i < ScreenManager.ScreenManager.Instance.GetScreens().Length; i++)
			{
				if (ScreenManager.ScreenManager.Instance.GetScreens()[i] is WaitScreen)
				{
					if (ScreenManager.ScreenManager.Instance.GetScreens()[i].IsActive)
					{
						var waitScreen = (WaitScreen)ScreenManager.ScreenManager.Instance.GetScreens()[i];
						waitScreen.RemovePlayer(mob.IsPlayer);
					}
				}	
			}*/

			if (IsGameStarted)
				GameModel.RemoveMob(mob.Id);
		}

		public void MobShot(AGameObject mob, AProjectile[] projectiles)
		{
			// update ShootVector
			var clientMob = GameModel.GetMob(mob.Id);
			clientMob.ShootVector = projectiles[projectiles.Length - 1].RunVector;

			// add projectiles
			foreach (var aProjectile in projectiles)
			{
				GameModel.AddProjectile(GameFactory.CreateClientProjectile(aProjectile));
			}
		}

		public void SynchroFrame(AGameObject[] mobs)
		{
			if (!IsGameStarted)
				return;

			foreach (var mob in mobs)
			{
				AGameObject clientMob;
				try
				{
					clientMob = GameModel.GetMob(mob.Id);
				}
				catch
				{
					continue;
				}
				if (clientMob == null)
					continue;

				clientMob.Coordinates = mob.Coordinates;
				clientMob.HealthAmount = mob.HealthAmount;
				clientMob.RunVector = mob.RunVector;
				clientMob.ShootVector = mob.ShootVector;
				clientMob.Speed = mob.Speed;
				clientMob.State = mob.State;
			}
		}

		public void PlayerListChanged(String[] names)
		{
			//TODO! issue 26?
			// for (int i = 0; i < ScreenManager.Instance.GetScreens().Length; i++)
			{
				// if (ScreenManager.Instance.GetScreens()[i] is WaitScreen)
				{
					//todo!
					//if (ScreenManager.ScreenManager.Instance.GetScreens()[i].IsActive)
					//{
					// var screen = (WaitScreen) ScreenManager.Instance.GetScreens()[i];
					// screen.ChangePlayerList(names);
					//}
				}
			}
		}

		#endregion

		#region ClientInput

		private DateTime _dateTime;
		private const int Rate = 1000 / 10;

		public void HandleInput(Controller controller)
		{
			var player = GameModel.GetMob(MyId);

			if (player == null)
				return;

			// current RunVector
			Vector2? currentRunVector = controller.RunVector;

			if (currentRunVector.HasValue)
			{
				Move(currentRunVector.Value);
				player.RunVectorM = currentRunVector.Value;
			}

			Vector2 mouseCoordinates = controller.SightPosition;

			player.ShootVectorM = mouseCoordinates - GameModel.Camera2D.ConvertToLocal(player.CoordinatesM);
			if (player.ShootVector.Length() > 0)
				player.ShootVector.Normalize();

			if (controller.ShootButton == ButtonState.Pressed)
			{
				if ((DateTime.Now - _dateTime).Milliseconds > Rate)
				{
					_dateTime = DateTime.Now;
					Shoot(player.ShootVectorM);
				}
			}
		}

		private void InitConnection()
		{
			try
			{
				var channelFactory = new ChannelFactory<ISkyShootService>("SkyShootEndpoint");
				_service = channelFactory.CreateChannel();
			}
			catch (Exception exc)
			{
				Trace.WriteLine("Can't connect to SkyShootService");
				Trace.WriteLine(exc);
				// !! @todo catch this!
				throw;
			}
		}

		private void FatalError(Exception e)
		{
			Trace.WriteLine(e);

			// back to multiplayer screen
			ScreenManager.Instance.SetActiveScreen(typeof (LoginScreen));

			MessageBox.Message = "Connection error!";
			ScreenManager.Instance.SetActiveScreen(typeof (MessageBox));
		}

		public bool Register(string username, string password)
		{
			try
			{
				return _service.Register(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public Guid? Login(string username, string password)
		{
			Guid? login = null;
			try
			{
				login = _service.Login(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				FatalError(e);
			}
			if (login.HasValue)
			{
				MyId = login.Value;
			}
			else
			{
				MessageBox.Message = "Connection error!";
				ScreenManager.Instance.SetActiveScreen(typeof (MessageBox));
			}

			return login;
		}

		public GameDescription[] GetGameList()
		{
			try
			{
				return _service.GetGameList();
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tile)
		{
			try
			{
				return _service.CreateGame(mode, maxPlayers, tile);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public bool JoinGame(GameDescription game)
		{
			try
			{
				return _service.JoinGame(game);
			}
			catch (Exception e)
			{
				FatalError(e);
				return false;
			}
		}

		public void Move(Vector2 direction)
		{
			Move(TypeConverter.Xna2XnaLite(direction));
		}

		public AGameEvent[] Move(XNA.Framework.Vector2 direction)
		{
			try
			{
				// var sw = new Stopwatch();
				// sw.Start();
				return _service.Move(direction);
				// sw.Stop();
				// Trace.WriteLine("SW:serv:Move " + sw.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public Queue<AGameEvent> Shoot(XNA.Framework.Vector2 direction)
		{
			try
			{
				// var sw = new Stopwatch();
				// sw.Start();
				return _service.Shoot(direction);
				// sw.Stop();
				// Trace.WriteLine("SW:serv:Shoot " + sw.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				FatalError(e);
				return null;
			}
		}

		public AGameEvent[] GetEvents()
		{
			throw new NotImplementedException();
		}

		public void Shoot(Vector2 direction)
		{
			Shoot(TypeConverter.Xna2XnaLite(direction));
		}

		public void TakeBonus(AObtainableDamageModifier bonus)
		{
			try
			{
				//_service.TakeBonus(bonus);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public void TakePerk(Perk perk)
		{
			try
			{
				//_service.TakePerk(perk);
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public void LeaveGame()
		{
			try
			{
				_service.LeaveGame();
			}
			catch (Exception e)
			{
				FatalError(e);
			}
		}

		public Contracts.Session.GameLevel GameStart(int gameId)
		{
			try
			{
				return _service.GameStart(gameId);
			}
			catch (Exception e)
			{
				FatalError(e);				
				throw;
			}
		}

		public AGameObject[] SynchroFrame()
		{
			try
			{
				return _service.SynchroFrame();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc);
				return new AGameObject[]{};
			}
		}

		public string[] PlayerListUpdate()
		{
			try
			{
				return _service.PlayerListUpdate();
			}
			catch (Exception exc)
			{
				Trace.WriteLine(exc);
				return new string[]{};
			}
		}

		#endregion
	}
}