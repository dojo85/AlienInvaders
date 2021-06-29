using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlienInvaders.Pages.Game;

namespace AlienInvaders.Models
{
    public class GameManager
    {
        public ShipModel Ship { get; set; }
        public List<ShotModel> Bullets { get; set; }
        public int ShotsFired { get; set; }
        public int AliensHit { get; set; }
        public List<List<AlienModel>> AlienInvaders { get; set; }
        public List<AlienModel> ListOfInvaders { get; set; } = new List<AlienModel>();
        public bool IsRunning { get; set; }
        public double TimeElapsed { get; set; }

        public event EventHandler GameLoopCompleted;
        public event EventHandler GameStarted;

        public GameManager()
        {
            Ship = new ShipModel();            
            Bullets = new List<ShotModel>();
            AlienInvaders = Invade();
            WaitForHits();
            IsRunning = false;
        }

        public void StartGame()
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
            IsRunning = true;
            GameLoop();
        }

        public async Task GameLoop()
        {
            while (IsRunning)
            {
                MoveObjects();
                CheckForBulletOutOfRange();
                TimeElapsed += 20;
                CheckWinCondition();
                GameLoopCompleted?.Invoke(this, EventArgs.Empty);
                await Task.Delay(20);
            }
        }

        public void MoveObjects()
        {
            foreach (var bullet in Bullets)
            {
                bullet.Fly();
            }
        }
        
        public async Task GenerateNewShot()
        {
            if (Ship.IsReadyToFire)
            {
                var bullet = await Ship.Shoot();
                Bullets.Add(bullet);
                ShotsFired++;
            }
        }

        private void CheckForBulletOutOfRange()
        {
            foreach (var bullet in Bullets)
            {
                if (bullet.DistanceFromTop <= -5)
                {
                    Bullets.Remove(bullet);
                    break;
                }
            }
        }

        private List<List<AlienModel>> Invade()
        {
            int top = 30;
            int left = 30;
            int rows = 3;
            List<List<AlienModel>> allAliens = new List<List<AlienModel>>(rows);

            for (int i = 1; i <= rows; i++)
            {
                List<AlienModel> row = new List<AlienModel>();
                top = i * 30;

                for (int j = 1; j <= 6; j++)
                {
                    left = j*30;
                    AlienModel alien = new AlienModel(top, left, Bullets);
                    row.Add(alien);
                    ListOfInvaders.Add(alien);
                }

                allAliens.Add(row);
            }

            return allAliens;
        }

        private void WaitForHits()
        {
            foreach (var alien in ListOfInvaders)
            {
                alien.AlienShipHit += (o, e) => AliensHit++;
            }
        }

        private void CheckWinCondition()
        {
            if (ListOfInvaders.TrueForAll(a=>a.WasHit))
            {
                IsRunning = false;
            }
        }

        public double CalculateScore()
        {
            double p1 = ShotsFired / 18;
            double p2 = TimeElapsed / 18;
            double score = 1 / (p1 + p2) * 1_000_000_000;
            return Math.Round(score, 0, MidpointRounding.AwayFromZero);
        }

    }
}
