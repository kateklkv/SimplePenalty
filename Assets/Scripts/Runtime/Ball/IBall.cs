using System;

namespace Runtime.Ball
{
    public interface IBall
    {
        public event Action Destroyed;
        public void SetInputService(InputService inputService);
    }
}