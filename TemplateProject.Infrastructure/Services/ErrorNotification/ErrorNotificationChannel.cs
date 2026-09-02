using System.Threading.Channels;
using Microsoft.Extensions.Options;
using QrAssignment.Application.Common;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Infrastructure.Services.ErrorNotification;

public sealed class ErrorNotificationChannel : IErrorNotifier
{
    private readonly Channel<Application.Interfaces.ErrorNotification> _channel;
    private readonly ErrorNotificationSettings _settings;

    public ErrorNotificationChannel(IOptions<ErrorNotificationSettings> options)
    {
        _settings = options.Value;
        _channel = Channel.CreateBounded<Application.Interfaces.ErrorNotification>(
            new BoundedChannelOptions(Math.Max(1, _settings.QueueCapacity))
            {
                FullMode = BoundedChannelFullMode.DropWrite,
                SingleReader = true,
                SingleWriter = false
            });
    }

    public ChannelReader<Application.Interfaces.ErrorNotification> Reader => _channel.Reader;

    public bool TryEnqueue(Application.Interfaces.ErrorNotification notification)
    {
        if (!_settings.Enabled || _settings.Recipients.Length == 0)
            return false;

        return _channel.Writer.TryWrite(notification);
    }
}