using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRStreamDemo
{
    public class StreamHub : Hub
    {
        public void SendStreamInit()
        {
            Clients.Caller.SendAsync("streamStarted");
        }

        public ChannelReader<string> StartStreaming()
        {
            var channel = Channel.CreateUnbounded<string>();
            _ = WriteToChannel(channel);
            return channel.Reader;

            async Task WriteToChannel(ChannelWriter<string> writer)
            {
                while(true)
                {
                    await writer.WriteAsync("Hello from thread " + Thread.CurrentThread.ManagedThreadId);
                    await Task.Delay(1000);
                }
                writer.Complete();
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}