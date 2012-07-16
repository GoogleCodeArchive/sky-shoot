﻿using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class MainMenuScreen : GameScreen
	{
		private static Texture2D _texture;

		private readonly ContentManager _content;

		private SpriteBatch _spriteBatch;

		private ButtonControl _playGameButton;
		private ButtonControl _optionsButton;
		private ButtonControl _logoffButton;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public MainMenuScreen()
		{
			CreateControls();
			InitializeControls();

			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
		}

		private void CreateControls()
		{
			_playGameButton = new ButtonControl
			                  	{
			                  		Text = "Multiplayer",
			                  		Bounds =
			                  			new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.2f, 0),
			                  			                 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			                  	};

			_optionsButton = new ButtonControl
			                 	{
			                 		Text = "Options",
			                 		Bounds =
			                 			new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.35f, 0),
			                 			                 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			                 	};

			_logoffButton = new ButtonControl
			                	{
			                		Text = "Logoff",
			                		Bounds =
			                			new UniRectangle(new UniScalar(0.30f, 0), new UniScalar(0.5f, 0),
			                			                 new UniScalar(0.4f, 0), new UniScalar(0.1f, 0)),
			                	};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_playGameButton);
			Desktop.Children.Add(_optionsButton);
			Desktop.Children.Add(_logoffButton);

			ScreenManager.Instance.Controller.AddListener(_playGameButton, PlayGameButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_optionsButton, OptionsButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_logoffButton, LogoffMenuButtonPressed);
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		private void PlayGameButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(typeof (MultiplayerScreen));
		}

		private void OptionsButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(typeof (OptionsMenuScreen));
		}

		private void LogoffMenuButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.SetActiveScreen(typeof (LoginScreen));
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
		}
	}
}