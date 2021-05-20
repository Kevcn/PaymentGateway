using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Contracts.V1.Responses;
using Xunit;

namespace PaymentGateway.IntegrationTests
{
    public class ProcessPaymentRequestValidationTests
    {
        private readonly HttpClient _client;

        public ProcessPaymentRequestValidationTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenRequestContainsInvalidCardNumber()
        {
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var expectedInvalidField = "CardNumber";
            
            var processPaymentRequest = new ProcessPaymentRequest
            {
                CardNumber = "12341234141266aaa",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };
            
            var requestContent = new StringContent(JsonConvert.SerializeObject(processPaymentRequest), Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync("api/v1/payment", requestContent);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedInvalidField, errorResponse.Errors.FirstOrDefault().FieldName);
        }

        [Fact]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenRequestContainsInvalidCurrency()
        {
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var expectedInvalidField = "Currency";
            
            var processPaymentRequest = new ProcessPaymentRequest
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBp",
                CVV = "111"
            };
            
            var requestContent = new StringContent(JsonConvert.SerializeObject(processPaymentRequest), Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync("api/v1/payment", requestContent);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedInvalidField, errorResponse.Errors.FirstOrDefault().FieldName);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnBadRequest_WhenRequestContainsInvalidCVV()
        {
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var expectedInvalidField = "CVV";
            
            var processPaymentRequest = new ProcessPaymentRequest
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "1115"
            };
            
            var requestContent = new StringContent(JsonConvert.SerializeObject(processPaymentRequest), Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync("api/v1/payment", requestContent);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedInvalidField, errorResponse.Errors.FirstOrDefault().FieldName);
        }
    }
}