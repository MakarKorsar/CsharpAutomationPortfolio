using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace CsharpTests.Api
{
    [TestFixture]
    public class ApiTests
    {
        private RestClient _client;

        [SetUp]
        public void Setup()
        {
            // Базовый URL для JSONPlaceholder
            _client = new RestClient("https://jsonplaceholder.typicode.com/");
        }

        [TearDown]
        public void TearDown()
        {
            // Освобождение ресурсов RestClient
            _client?.Dispose();
        }

        [Test]
        public async Task Test_Get_Single_User_Success()
        {
            // GET запрос на /users/1
            var request = new RestRequest("users/1"); 

            // Асинхронное выполнение запроса
            var response = await _client.ExecuteGetAsync(request);

            // 1. Проверяем HTTP статус: 200 OK
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // 2. Проверяем, что ответ содержит ожидаемые поля
            StringAssert.Contains("Leanne Graham", response.Content);

            // 3. Проверяем, что ответ содержит поле id
            StringAssert.Contains("\"id\":", response.Content);
        }
    }
}