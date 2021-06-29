using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlienInvaders.Models
{
    public class AlienModel
    {
        public AlienModel(int top, int left, List<ShotModel> bullets)
        {
            DistanceFromTop = top;
            DistanceFromLeft = left;
            SetHitCoordinates(top, left);
            VisibilityCss = "visible";
            Bullets = bullets;
            AlienOperations();
        }

        public int[] HitCoordinates { get; private set; } = new[] {0, 0, 0};
        public int DistanceFromTop { get; set; }
        public int DistanceFromLeft { get; set; }
        public List<ShotModel> Bullets { get; set; }
        public bool WasHit { get; set; }
        public string VisibilityCss { get; set; }

        public event EventHandler AlienShipHit;

        private void SetHitCoordinates(int top, int left)
        {
            int topValue = top * 2 + 60;
            int leftValue = left + left - 43; //(left - 30) - 14 + 1
            int rightValue = leftValue + 27; // +30 -3
            HitCoordinates = new[] { topValue, leftValue, rightValue };
        }

        private async Task AlienOperations()
        {
            while (!WasHit)
            {
                if (CheckForHits(this, Bullets))
                {
                    AlienShipHit?.Invoke(this, EventArgs.Empty);
                }
                await Task.Delay(20);
            }
        }

        private bool CheckForHits(AlienModel alien, List<ShotModel> bullets)
        {
            if (bullets.Count > 0)
            {
                foreach (var bullet in bullets)
                {
                    if (
                        bullet.DistanceFromTop <= alien.HitCoordinates[0] // the hit surface
                        &&
                        ((bullet.Coordinates[0] >= alien.HitCoordinates[1] && bullet.Coordinates[0] <= alien.HitCoordinates[2])
                        ||
                        (bullet.Coordinates[1] >= alien.HitCoordinates[1] && bullet.Coordinates[1] <= alien.HitCoordinates[2]))
                    )
                    {
                        DestroyAlien(bullet);
                        return true;
                    }
                }
            }
            return false;
        }

        private void DestroyAlien(ShotModel bullet)
        {
            WasHit = true;
            VisibilityCss = "hidden";
            Bullets.Remove(bullet);
        }
    }
}
