using System;

namespace Plasma.HostedGraphite.Interfaces
{
	public interface IHostedGraphiteService
	{
		/// <summary>
		/// Send the specified metric to Hosted Graphite
		/// </summary>
		/// <param name="key">Dot delimited key identifying this metric</param>
		/// <param name="value">Float value to send</param>
		/// <param name="utcTimestamp">Optional Utc Timestamp to attach to this metric</param>
		void Send(string key, float value, DateTime? utcTimestamp = null);

		/// <summary>
		/// Allows the service to transmit statistics to Hosted Graphite
		/// Optionally, an environment prefix can be used to prepend to the key name provided during Send calls
		/// </summary>
		/// <param name="apiKey">HostedGraphite API Key</param>
		/// <param name="environmentPrefix">Optional prefix to use for keys when Send is called</param>
		void ActivateForUse(string apiKey, string environmentPrefix = null);

		/// <summary>
		/// Time how long the specified code block runs against the provided metric key
		/// </summary>
		IDisposable Time(string key);
	}
}