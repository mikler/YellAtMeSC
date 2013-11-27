using System;

namespace PhoneXamlDirect3DApp1
{

    public class EnergyGameTimer : GameTimer
    {
        protected static double energyPerGameSecond = 0.5625;

        public EnergyGameTimer(double energy, string name)
        {
            this.Recharge = TimeSpan.FromSeconds((energy / energyPerGameSecond) / speedFactor);
            this.Name = name;
            LastNotify = DateTime.Now;
        }
    }
    public class TimeGameTimer : GameTimer
    {
        protected static double energyPerGameSecond = 0.5625;

        public TimeGameTimer(double time, string name)
        {
            this.Recharge = TimeSpan.FromSeconds(time / speedFactor);
            this.Name = name;
            LastNotify = DateTime.Now;
        }

    }

    public abstract class GameTimer
    {
        protected static double speedFactor = 1.333333;

        public TimeSpan Recharge { get; protected set; }
        public DateTime LastNotify { get; set; }
        public string Name { get; protected set; }
    }
}