using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlienInvaders.Models
{
    public class ShotModel
    {
        public int Id { get; set; }

        public int Speed { get; set; } = 10;

        public int DistanceFromTop { get; set; }

        public int DistanceFromLeft { get; set; }

        public int[] Coordinates { get; set; }

        public ShotModel()
        {
            DistanceFromTop = 440;
        }

        public void Fly()
        {
           DistanceFromTop -= Speed;
        }

        public void Stop()
        {
            Speed = 0;
            DistanceFromTop = -20;
        }

    }
}
