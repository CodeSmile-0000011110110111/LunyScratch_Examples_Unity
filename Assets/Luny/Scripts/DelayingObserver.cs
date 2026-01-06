using Luny.Engine;
using System;
using System.Threading.Tasks;

namespace Luny.Scripts
{
	public sealed class DelayingObserver : ILunyEngineObserver
	{
		private const Int32 StartupDelayInMilliseconds = 100;
		private const Int32 UpdateDelayInMilliseconds = 10;
		private const Int32 ShutdownDelayInMilliseconds = 50;
		public Boolean Enabled => false;

		public void OnEngineStartup() => Task.Delay(StartupDelayInMilliseconds).Wait();
		public void OnEngineFixedStep(Double fixedDeltaTime) => Task.Delay(UpdateDelayInMilliseconds).Wait();
		public void OnEngineUpdate(Double deltaTime) => Task.Delay(UpdateDelayInMilliseconds).Wait();
		public void OnEngineLateUpdate(Double deltaTime) => Task.Delay(UpdateDelayInMilliseconds).Wait();
		public void OnEngineShutdown() => Task.Delay(ShutdownDelayInMilliseconds).Wait();
	}
}
