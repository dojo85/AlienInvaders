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
                        WasHit = true;
                        VisibilityCss = "hidden";
                        Bullets.Remove(bullet);
                        return true;
                    }
                }
            }

            return false;
        }

        private void SetHitCoordinates(int top, int left)
        {
            int topValue = top + 90;
            int leftValue = left + (left - 30) -14;
            int rightValue = leftValue + 30;
            HitCoordinates = new[] {topValue, leftValue, rightValue};
        }

    }
}
