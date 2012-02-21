﻿using System;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.View;
using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class OptionsMenuScreen : GameScreen
	{
		private GuiManager _gui;

		private ListControl _keyboardList;

		private LabelControl _titleLabel;
		private LabelControl _cursorLabel;
		private LabelControl _keyboardLabel;
		private LabelControl _fullscreenLabel;

		private OptionControl _fullscreenButton;

		private ChoiceControl _arrowButton;
		private ChoiceControl _plusButton;
		private ChoiceControl _crossButton;
		private ChoiceControl _targetButton;

		private ButtonControl _backButton;

		private ContentManager _content;

		private static Texture2D _texture;

		private SpriteBatch _spriteBatch;

		public static short Curs = 1;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public override void LoadContent()
		{
			base.LoadContent();
			_gui = ScreenManager.Instance.Gui;
			_gui.Screen = this;

			Height = ScreenManager.Instance.Height;
			Width = ScreenManager.Instance.Width;
			
			if (_content == null)
				_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

			Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
				);

			_titleLabel = new LabelControl("Options")
			              	{
			              		Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.1f, -70), 100, 30)
			              	};
			Desktop.Children.Add(_titleLabel);

			_fullscreenLabel = new LabelControl("FullScreen: ")
			                   	{
			                   		Bounds =
			                   			new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.3f, -70), 80, 30)
			                   	};
			Desktop.Children.Add(_fullscreenLabel);

			_fullscreenButton = new OptionControl
			                    	{
			                    		Bounds =
			                    			new UniRectangle(new UniScalar(0.5f, 30), new UniScalar(0.3f, -70), 100, 30)
			                    	};
			Desktop.Children.Add(_fullscreenButton);
			_fullscreenButton.Selected = Settings.Default.FullScreenSelected;
			_fullscreenButton.Changed += FullScreenSelected;

			_keyboardLabel = new LabelControl("Keyboard:")
			                 	{
			                 		Bounds =
			                 			new UniRectangle(new UniScalar(0.5f, -150), new UniScalar(0.5f, -70), 50, 30)
			                 	};
			Desktop.Children.Add(_keyboardLabel);

			_keyboardList = new ListControl
			                	{
			                		Bounds = new UniRectangle(260f, 175f, 150f, 50f)
			                	};
			_keyboardList.Items.Add("A, S, D, W");
			_keyboardList.Items.Add("Arrows");
			_keyboardList.Slider.Bounds.Location.X.Offset -= 1.0f;
			_keyboardList.Slider.Bounds.Location.Y.Offset += 1.0f;
			_keyboardList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			_keyboardList.SelectionMode = ListSelectionMode.Single;
			_keyboardList.SelectedItems.Add(Settings.Default.KeyboardLayout == 0 ? 0 : 1);
			_keyboardList.SelectionChanged += KeyboardChoice;
			Desktop.Children.Add(_keyboardList);

			_cursorLabel = new LabelControl("Cursor:")
			               	{
			               		Bounds =
			               			new UniRectangle(new UniScalar(0.5f, -220), new UniScalar(0.8f, -70), 70, 30)
			               	};
			Desktop.Children.Add(_cursorLabel);

			_arrowButton = new ChoiceControl
			               	{
			               		Bounds =
			               			new UniRectangle(new UniScalar(0.5f, -140), new UniScalar(0.9f, -70), 70, 30)
			               	};
			_arrowButton.Changed += ArrowButtonPressed;
			if (Settings.Default.Cursor == 1) _arrowButton.Selected = true;
			Desktop.Children.Add(_arrowButton);

			_plusButton = new ChoiceControl
			              	{
			              		Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.9f, -70), 70, 30)
			              	};
			_plusButton.Changed += PlusButtonPressed;
			if (Settings.Default.Cursor == 2) _plusButton.Selected = true;
			Desktop.Children.Add(_plusButton);

			_crossButton = new ChoiceControl
			               	{
			               		Bounds = new UniRectangle(new UniScalar(0.5f, 40), new UniScalar(0.9f, -70), 70, 30)
			               	};
			_crossButton.Changed += CrossButtonPressed;
			if (Settings.Default.Cursor == 3) _plusButton.Selected = true;
			Desktop.Children.Add(_crossButton);

			_targetButton = new ChoiceControl
			                	{
			                		Bounds =
			                			new UniRectangle(new UniScalar(0.5f, 130), new UniScalar(0.9f, -70), 70, 30)
			                	};
			_targetButton.Changed += TargetButtonPressed;
			if (Settings.Default.Cursor == 4) _targetButton.Selected = true;
			Desktop.Children.Add(_targetButton);

			_backButton = new ButtonControl
			              	{
			              		Text = "Back",
			              		Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(1.1f, -70), 100, 30)
			              	};
			_backButton.Pressed += BackButtonPressed;
			Desktop.Children.Add(_backButton);

			if (_content == null)
				_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");

			Textures.Arrow = _content.Load<Texture2D>("Textures/Cursors/Arrow");
			Textures.Plus = _content.Load<Texture2D>("Textures/Cursors/Plus");
			Textures.Cross = _content.Load<Texture2D>("Textures/Cursors/Cross");
			Textures.Target = _content.Load<Texture2D>("Textures/Cursors/Target");
		}

		private void ArrowButtonPressed(object sender, EventArgs e)
		{
			Curs = 1;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();
		}

		private void PlusButtonPressed(object sender, EventArgs e)
		{
			Curs = 2;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();
		}

		private void CrossButtonPressed(object sender, EventArgs e)
		{
			Curs = 3;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();
		}

		private void TargetButtonPressed(object sender, EventArgs e)
		{
			Curs = 4;
			Settings.Default.Cursor = Curs;
			Settings.Default.Save();
		}

		private void KeyboardChoice(object sender, EventArgs e)
		{
			Settings.Default.KeyboardLayout = (short) (_keyboardList.SelectedItems[0] == 0 ? 0 : 1);
			Settings.Default.Save();
		}

		private void BackButtonPressed(object sender, EventArgs e)
		{
			ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenEnum.MainMenuScreen;
		}

		private void FullScreenSelected(object sender, EventArgs e)
		{
			Settings.Default.FullScreenSelected = _fullscreenButton.Selected;
			Settings.Default.Save();
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;
			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
			base.Draw(gameTime);
			_gui.Draw(gameTime);
			_spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
			var pos1 = new Vector2(250f, 370f);
			var pos2 = new Vector2(340f, 370f);
			var pos3 = new Vector2(430f, 370f);
			var pos4 = new Vector2(520f, 370f);
			_spriteBatch.Draw(Textures.Arrow, pos1, Color.White);
			_spriteBatch.Draw(Textures.Plus, pos2, Color.White);
			_spriteBatch.Draw(Textures.Cross, pos3, Color.White);
			_spriteBatch.Draw(Textures.Target, pos4, Color.White);
			_spriteBatch.End();
		}
	}
}