using System;
using System.Threading.Tasks;

namespace Luny.Scripts
{
	public sealed class DelayingObserver : IEngineObserver
	{
		private const Int32 StartupDelayInMilliseconds = 100;
		private const Int32 UpdateDelayInMilliseconds = 10;
		private const Int32 ShutdownDelayInMilliseconds = 50;
		public Boolean Enabled => false;

		public void OnStartup() => Task.Delay(StartupDelayInMilliseconds).Wait();
		public void OnFixedStep(Double fixedDeltaTime) => Task.Delay(UpdateDelayInMilliseconds).Wait();
		public void OnUpdate(Double deltaTime) => Task.Delay(UpdateDelayInMilliseconds).Wait();
		public void OnLateUpdate(Double deltaTime) => Task.Delay(UpdateDelayInMilliseconds).Wait();
		public void OnShutdown() => Task.Delay(ShutdownDelayInMilliseconds).Wait();
	}
}
