﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Service.Session
{
    public class SessionManager
    {
        //Содержит текущие игры
        private List<GameSession> _gameSessions;

        //Уникальный идентификатор, который присваивается каждой игре при её создании
        private int _gameID;

        public SessionManager()
        {
            _gameSessions = new List<GameSession>();
            _gameID = 1;
        }

        //Добавляем игрока в текущую игру.
        public bool JoinGame(GameDescription game, string playerName)
        {
            game = _gameSessions.Find(curGame => curGame.LocalGameDescription.GameID == game.GameID).LocalGameDescription;

            try
            {
                if(game.Players.Contains(playerName)){
                    game.Players.Add(playerName);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        //Создаем новую игру
        public GameDescription CreateGame(GameMode mode, int maxPlayers, string playerName,TileSet tileSet)
        {
            List<string> playerNames;
            playerNames = new List<string>();
            playerNames.Add(playerName);

            var gameSession = new GameSession(tileSet, playerNames, maxPlayers, mode, _gameID);
            _gameSessions.Add(gameSession);

            return gameSession.LocalGameDescription;
        }

        //Возвращает список игр.
        public GameDescription[] GetGameList()
        {
            var gameSessions = _gameSessions.ToArray();
            var gameDescriptions = new List<GameDescription>();

            for (int i = 0; i < gameSessions.Length; i++)
            {
                gameDescriptions.Add(gameSessions[i].LocalGameDescription);
            }

            return gameDescriptions.ToArray();
        }

        //Ищем игру, в которой находится игрок и удаляем его оттуда.
        public bool LeaveGame(string playerName)
        {
            try
            {
                var game = _gameSessions.Find(gameSession => gameSession.LocalGameDescription.Players.Contains(playerName));
                game.LocalGameDescription.Players.Remove(playerName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}