using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem
{
    public class Position
    {
        public event EventHandler Deactivated;
        public event EventHandler Activated;
        public event EventHandler Destroyed;
        public void Deactivate()
        {
            OnDeactivated(EventArgs.Empty);
        }
        public void Activate()
        {
            OnActivate(EventArgs.Empty);
        }

        public void Destroy()
        {
            OnDestroyed(EventArgs.Empty);
        }

        private void OnDestroyed(EventArgs eventArgs)
        {
            var handler = Destroyed;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnDeactivated(EventArgs eventArgs)
        {
            var handler = Deactivated;
            handler?.Invoke(this, eventArgs);
        }
        protected virtual void OnActivate(EventArgs eventArgs)
        {
            var handler = Activated;
            handler?.Invoke(this, eventArgs);
        }
    }
}
