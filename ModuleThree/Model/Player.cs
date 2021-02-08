using System.Collections.Generic;

namespace ModuleThree.Model
{
    internal class Player
    {
        public string NickName { get; set; }
        public List<int> Moves { get; set; }

        public Player(string nick)
        {
            NickName = nick;
            Moves = new List<int>();
        }
    }
}
