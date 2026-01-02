using SignalRDemo.Controllers;
using StackExchange.Redis;

namespace SignalRDemo
{
    public class RedisService
    {
        private readonly ILogger<RedisService> _logger;
        string channelName = "demo.channel";
        private readonly IDatabase _db;
        private readonly ISubscriber _subscriber;
        public RedisService(IConnectionMultiplexer redis,ILogger<RedisService> logger )
        {
            _logger = logger;
            _db = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
            _subscriber.Subscribe(channelName, (channel, message) =>
            {
                _logger.LogInformation($"Received message: {message}");
            });

        }
        public async Task<string> GetCachedDataAsync(string key)
        {
            // 设置字符串值
            await _db.StringSetAsync("mykey", "Hello, Redis!", TimeSpan.FromSeconds(60));
            // 获取字符串值
            return await _db.StringGetAsync("mykey");
        }


        public async Task PublishMessageAsync(string message)
        {
            // 发布消息
            await _db.PublishAsync(channelName, message);
        }
    }
}
