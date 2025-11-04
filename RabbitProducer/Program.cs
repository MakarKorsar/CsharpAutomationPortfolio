using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Настройка соединения
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// 1. Объявление очереди
// (Если очередь не существует, RabbitMQ ее создаст)
channel.QueueDeclare(
    queue: "hello_world_queue", // Имя очереди
    durable: false,             // Не сохранять очередь после перезапуска брокера
    exclusive: false,           // Разрешить доступ нескольким потребителям
    autoDelete: false,          // Не удалять после отключения последнего потребителя
    arguments: null
);

Console.WriteLine("Производитель запущен. Нажмите Enter, чтобы отправить сообщение.");

int messageCount = 0;
while (true)
{
    Console.ReadLine(); // Ждем Enter для отправки

    messageCount++;
    string message = $"[Сообщение {messageCount}] Hello from C# Producer!";
    var body = Encoding.UTF8.GetBytes(message);

    // 2. Публикация сообщения
    channel.BasicPublish(
        exchange: string.Empty,     // Используем пустой exchange (прямая маршрутизация в очередь)
        routingKey: "hello_world_queue", // Имя очереди в качестве ключа маршрутизации
        basicProperties: null,
        body: body
    );

    Console.WriteLine($" [x] Отправлено: '{message}'");
}