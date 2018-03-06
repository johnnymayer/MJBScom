using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace MJBScom.Models
{
    public class Player
    {
        private int _id;
        private string _name;
        private int _hpTotal;
        private int _hpRemaining;
        private int _agility;
        private int _intelligence;
        private int _strength;
        private int _luck;

        public Player(string name, int hpTotal, int hpRemaining, int agility, int intel, int strength, int luck, int id = 0)
        {
          _id = id;
          _name = name;
          _hpTotal = hpTotal;
          _hpRemaining = hpRemaining;
          _agility = agility;
          _intelligence = intel;
          _strength = strength;
          _luck = luck;
        }
        public Player(string name, int hpTotal, int hpRemaining, int id = 0)
        {
          _id = id;
          _name = name;
          _hpTotal = hpTotal;
          _hpRemaining = hpRemaining;
          Random r = new Random();
          for (int i = 0; i < 20; i++) {
            int stat = r.Next(4);
            switch(stat)
            {
              case 0:
                _agility++;
                break;
              case 1:
                _intelligence++;
                break;
              case 2:
                _strength++;
                break;
              case 3:
                _luck++;
                break;
            }
          }
        }

        public int GetId() {return _id;}
        public string GetName() {return _name;}
        public int GetHPTotal() {return _hpTotal;}
        public int GetHPRemaining() {return _hpRemaining;}
        public int GetAgility() {return _agility;}
        public int GetIntelligence() {return _intelligence;}
        public int GetStrength() {return _strength;}
        public int GetLuck() {return _luck;}

        public void SetName(string name) {_name = name;}
        public void SetHPTotal(int hpTotal) {_hpTotal = hpTotal;}
        public void SetHPRemaining(int hpRemaining) {_hpRemaining = hpRemaining;}
        public void SetAgility(int agility) {_agility = agility;}
        public void SetIntelligence(int intelligence) {_intelligence = intelligence;}
        public void SetStrength(int strength) {_strength = strength;}
        public void SetLuck(int luck) {_luck = luck;}

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM players; ALTER TABLE players AUTO_INCREMENT = 1;";

            cmd.ExecuteNonQuery();

            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO `players` (`name`, `hp_total`, `hp_remaining`, `agility`, `intelligence`, `strength`, `luck`) VALUES (@Name, @HPTotal, @HPRemaining, @Agility, @Intelligence, @Strength, @Luck);";

            cmd.Parameters.AddWithValue("@Name", this._name);
            cmd.Parameters.AddWithValue("@HPTotal", this._hpTotal);
            cmd.Parameters.AddWithValue("@HPRemaining", this._hpRemaining);
            cmd.Parameters.AddWithValue("@Agility", this._agility);
            cmd.Parameters.AddWithValue("@Intelligence", this._intelligence);
            cmd.Parameters.AddWithValue("@Strength", this._strength);
            cmd.Parameters.AddWithValue("@Luck", this._luck);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Player> GetAll()
        {
            List<Player> allPlayers = new List<Player>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM players;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int playerId = rdr.GetInt32(0);
                string playerName = rdr.GetString(1);
                int playerHPTotal = rdr.GetInt32(2);
                int playerHPRemaining = rdr.GetInt32(3);
                int playerAgility = rdr.GetInt32(5);
                int playerIntelligence = rdr.GetInt32(6);
                int playerStrength = rdr.GetInt32(7);
                int playerLuck = rdr.GetInt32(8);
                Player newPlayer = new Player(playerName, playerHPTotal, playerHPRemaining, playerAgility, playerIntelligence, playerStrength, playerLuck, playerId);

                allPlayers.Add(newPlayer);
            }
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return allPlayers;
        }

        public override bool Equals(System.Object otherPlayer)
        {
            if (!(otherPlayer is Player))
            {
                return false;
            }
            else
            {
                Player newPlayer = (Player) otherPlayer;
                bool idEquality = (this.GetId() == newPlayer.GetId());
                bool nameEquality = (this.GetName() == newPlayer.GetName());
                bool hpTotalEquality = (this.GetHPTotal() == newPlayer.GetHPTotal());
                bool hpRemainingEquality = (this.GetHPRemaining() == newPlayer.GetHPRemaining());
                bool agilityEquality = (this.GetAgility() == newPlayer.GetAgility());
                bool intelligenceEquality = (this.GetIntelligence() == newPlayer.GetIntelligence());
                bool strengthEquality = (this.GetStrength() == newPlayer.GetStrength());
                bool luckEquality = (this.GetLuck() == newPlayer.GetLuck());

                return (idEquality && nameEquality && hpTotalEquality && hpRemainingEquality && agilityEquality && intelligenceEquality && strengthEquality && luckEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM `players` WHERE id = @ThisId;";

            cmd.Parameters.AddWithValue("@ThisId", this._id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Update()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE `players` SET `name` = @Name, `hp_total` = @HPTotal, `hp_remaining` = @HPRemaining, `agility` = @Agility, `intelligence` = @Intelligence, `strength` = @Strength, `luck` = @Luck WHERE id = @ThisId;";

            cmd.Parameters.AddWithValue("@ThisId", this._id);
            cmd.Parameters.AddWithValue("@Name", this._name);
            cmd.Parameters.AddWithValue("@HPTotal", this._hpTotal);
            cmd.Parameters.AddWithValue("@HPRemaining", this._hpRemaining);
            cmd.Parameters.AddWithValue("@Agility", this._agility);
            cmd.Parameters.AddWithValue("@Intelligence", this._intelligence);
            cmd.Parameters.AddWithValue("@Strength", this._strength);
            cmd.Parameters.AddWithValue("@Luck", this._luck);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Player Find(int findId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * from `players` WHERE id = @ThisId;";
            cmd.Parameters.AddWithValue("@ThisId", findId);

            int playerId = 0;
            string playerName = "";
            int playerHPTotal = 0;
            int playerHPRemaining = 0;
            int playerAgility = 0;
            int playerIntelligence = 0;
            int playerStrength = 0;
            int playerLuck = 0;

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            if (rdr.Read())
            {
                playerId = rdr.GetInt32(0);
                playerName = rdr.GetString(1);
                playerHPTotal = rdr.GetInt32(2);
                playerHPRemaining = rdr.GetInt32(3);
                playerAgility = rdr.GetInt32(5);
                playerIntelligence = rdr.GetInt32(6);
                playerStrength = rdr.GetInt32(7);
                playerLuck = rdr.GetInt32(8);
            }
            Player foundPlayer = new Player(playerName, playerHPTotal, playerHPRemaining, playerAgility, playerIntelligence, playerStrength, playerLuck, playerId);

            conn.Close();
            if (conn != null)
              conn.Dispose();
            return foundPlayer;
        }
    }
}
