using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Настройка соединения
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// 1. Объявление очереди (важно, чтобы очередь существовала)
channel.QueueDeclare(
    queue: "hello_world_queue", // Имя должно совпадать с Producer
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

Console.WriteLine(" [*] Ожидание сообщений. Нажмите Ctrl+C для выхода.");

// 2. Создание потребителя
var consumer = new EventingBasicConsumer(channel);

// Устанавливаем обработчик события получения сообщения
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    
    // Имитация задержки обработки
    Thread.Sleep(500); 

    Console.WriteLine($" [x] Получено и обработано: '{message}'");
};

// 3. Запуск прослушивания очереди
channel.BasicConsume(
    queue: "hello_world_queue",
    autoAck: true, // Автоматическое подтверждение получения сообщения
    consumer: consumer
);

// Блокируем главный поток, чтобы потребитель продолжал работать
Console.ReadKey();