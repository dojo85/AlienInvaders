using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlienInvaders.Models
{
    public class ShipModel
    {
        public int ShotFrequency { get; set; } = 800;
        public int Speed { get; set; } = 10;

        public int DistanceFromLeft { get; set; } = 185;
        public bool IsReadyToFire { get; set; } = true;

        public void MoveLeft()
        {
            if(DistanceFromLeft >= 10)
                DistanceFromLeft -= Speed;
        }

        public void MoveRight()
        {
            if (DistanceFromLeft <= 360)
                DistanceFromLeft += Speed;
        }

        public async Task<ShotModel> Shoot()
        {
            int id = 0;
            ShotModel bullet = new ShotModel
            {
                Id = ++id,
                DistanceFromLeft = this.DistanceFromLeft + 14,
                Coordinates = new []{ DistanceFromLeft, DistanceFromLeft+1 }
            };
            IsReadyToFire = false;
            PrepareToFire();
            return bullet;
        }

        private async Task PrepareToFire()
        {
            await Task.Delay(ShotFrequency);
            IsReadyToFire = true;
        }

    }
}
