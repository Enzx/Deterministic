using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ConsoleCommon;
using Deterministic.Logs;
using MessagePack;

namespace Deterministic.Network
{
    public class TcpPeer
    {
        #region Events
        public event Action OnConnected;
        public event Action<NetError> OnConnectError;
        public event Action OnSendFinished;
        public event Action OnDisconnect;
        public event Action<Packet> OnPacketReceived;
        #endregion

        #region Private
        private TcpClient _client;
        private NetworkStream _stream;
        private bool _isConnected;
        #endregion

        #region Constructor
        public TcpPeer() { }
        public TcpPeer(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            _isConnected = _client.Connected;
            Task.Factory.StartNew(() => Receive()).ConfigureAwait(false);

        }
        #endregion

        #region Connect
        public void Connect(IPAddress ip, int port)
        {
            _isConnected = false;

            Task.Factory.StartNew(() => ConnectAsync(ip, port));
        }

        public void Disconnect()
        {
            Close();
            OnDisconnect?.Invoke();
        }
        private async Task ConnectAsync(IPAddress ip, int port, CancellationToken cancellationToken = default)
        {

            await CloseAsync();
            _client = new TcpClient();
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine($"Connecting to {ip}:{port}");
            try
            {
                await _client.ConnectAsync(ip, port);
            }
            catch (Exception e)
            {
                Log.LogException(e);
                OnConnectError?.Invoke(NetError.CantStablishConnection);
                throw;
            }
            await CloseIfCanceled(cancellationToken);

            _stream = _client.GetStream();
            _isConnected = true;
            OnConnected?.Invoke();

            _ = Task.Factory.StartNew(() =>
                    Receive(cancellationToken),
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

        }

        private async Task CloseAsync()
        {
            await Task.Yield();
            this.Close();
        }

        private void Close()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }
        private async Task CloseIfCanceled(CancellationToken token, Action onClosed = null)
        {
            if (token.IsCancellationRequested)
            {
                await CloseAsync();
                onClosed?.Invoke();
                token.ThrowIfCancellationRequested();
            }
        }
        #endregion

        #region Send
        public void Send(Packet packet)
        {
            byte[] bytes = MessagePackSerializer.Serialize(packet);
            int packetSize = bytes.Length;
            byte[] sizeBytes = BitConverter.GetBytes(packetSize);

            byte[] buffer = new byte[packetSize + 4];
            Array.Copy(sizeBytes, 0, buffer, 0, sizeBytes.Length);
            Array.Copy(bytes, 0, buffer, sizeBytes.Length, bytes.Length);
            Console.WriteLine($"Sending {buffer.Length} bytes");
            WriteBytes(buffer);
        }

        private async void WriteBytes(byte[] bytes, CancellationToken cancelToken = default)
        {
            await _stream.WriteAsync(bytes, 0, bytes.Length, cancelToken)
                .ContinueWith(task =>
                 {
                     if (task.IsCompleted)
                     {
                         Console.WriteLine($"{bytes.Length} bytes sent!");

                         OnSendFinished?.Invoke();
                     }
                 }, cancelToken);
        }
        #endregion

        #region Receive
        private async Task Receive(CancellationToken cancelToken = default)
        {
            Console.WriteLine($"Start Receiving");

            while (_isConnected)
            {
                await CloseIfCanceled(cancelToken);
                byte[] sizeBytes = await ReadBytes(4, cancelToken);
                int size;
                try
                {
                    size = BitConverter.ToInt32(sizeBytes, 0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await CloseAsync();
                    throw;
                }
                Console.WriteLine($"New Message incoming: {size} bytes");
                byte[] packetBytes = await ReadBytes(size, cancelToken);

                Packet packet = MessagePackSerializer.Deserialize<Packet>(packetBytes);

                OnPacketReceived?.Invoke(packet);
            }
        }

        private async Task<byte[]> ReadBytes(int amount, CancellationToken cancelToken = default)
        {
            Console.WriteLine($"Reading: {amount} bytes");

            if (amount == 0) return new byte[0];
            byte[] receiveBuffer = new byte[amount];
            int receivedBytes = 0;
            while (receivedBytes < amount)
            {
                cancelToken.ThrowIfCancellationRequested();
                int remaining = amount - receivedBytes;
                receivedBytes += await _stream
                    .ReadAsync(receiveBuffer, receivedBytes, remaining, cancelToken)
                    .ConfigureAwait(false);
            }
            Console.WriteLine($"Received: {receivedBytes} bytes");

            return receiveBuffer;
        }
        #endregion
    }

    public enum NetError
    {
        CantStablishConnection = 1,
    }
}