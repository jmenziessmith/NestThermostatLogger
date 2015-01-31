using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Plasma.HostedGraphite.Interfaces;

namespace Plasma.HostedGraphite.Services
{
    public class HostedGraphiteService : IHostedGraphiteService
    {
	    const string RemoteHostname = "carbon.hostedgraphite.com";
	    const int RemotePort = 2003;
	    static readonly Socket _socket;
	    static readonly IPEndPoint _endPoint;
	    static readonly DateTime _unixEpoch;
		string _apiKey;
	    string _environmentPrefix;

	    static HostedGraphiteService()
		{
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) { Blocking = false };
			_endPoint = new IPEndPoint(Dns.GetHostAddresses(RemoteHostname)[0], RemotePort);
			_unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		}

		/// <summary>
		/// Allows the service to transmit statistics to Hosted Graphite
		/// Optionally, an environment prefix can be used to prepend to the key name provided during Send calls
		/// </summary>
		/// <param name="apiKey">HostedGraphite API Key</param>
		/// <param name="environmentPrefix">Optional prefix to use for keys when Send is called</param>
		public void ActivateForUse(string apiKey, string environmentPrefix = null)
		{
			_apiKey = apiKey;
			_environmentPrefix = environmentPrefix;
		}

		/// <summary>
		/// Time how long the specified code block runs against the provided metric key
		/// </summary>
		public IDisposable Time(string key)
		{
			return new MetricTimer(key, this);
		}

		/// <summary>
		/// Send the specified metric to Hosted Graphite
		/// </summary>
		/// <param name="key">Dot delimited key identifying this metric</param>
		/// <param name="value">Float value to send</param>
		/// <param name="utcTimestamp">Optional Utc Timestamp to attach to this metric</param>
		public void Send(string key, float value, DateTime? utcTimestamp = null)
		{
			// Skip if not enabled
			if (string.IsNullOrEmpty(_apiKey))
				return;

			// Calculate Metric Key
			var revisedKey = !string.IsNullOrEmpty(_environmentPrefix) ? string.Format("{0}.{1}", _environmentPrefix, key) : key;

			// Format Packet
			var builder = new StringBuilder();
			builder.AppendFormat("{0}.{1} {2}", _apiKey, revisedKey, value);
			if (utcTimestamp != null)
				builder.AppendFormat(" {0}", GetUnixTimestamp(utcTimestamp.Value));
			builder.Append("\n");

			// Send Packet
			SendPacket(builder.ToString());
		}

		/// <summary>
		/// Send a string to the remote host
		/// </summary>
	    static void SendPacket(string packet)
		{
			var bytes = Encoding.ASCII.GetBytes(packet);

			lock(_socket)
				_socket.SendTo(bytes, _endPoint);
		}

		/// <summary>
		/// Calculate a Unix Timestsamp given a DateTime
		/// </summary>
		static int GetUnixTimestamp(DateTime dateTime)
		{
			return (int) (_unixEpoch - dateTime).TotalSeconds;
		}

		/// <summary>
		/// Helper class to record the time between construction/destruction
		/// </summary>
		private class MetricTimer : IDisposable
		{
			readonly string _key;
			readonly Stopwatch _stopwatch;
			HostedGraphiteService _service;

			public MetricTimer(string key, HostedGraphiteService service)
			{
				_key = key;
				_service = service;
				_stopwatch = Stopwatch.StartNew();
			}

			public void Dispose()
			{
				if (_service == null)
					return;

				var elapsed = _stopwatch.ElapsedMilliseconds;
				_service.Send(_key, elapsed);
				_service = null;
			}
		}
    }
}
