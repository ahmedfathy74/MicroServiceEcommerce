using AutoMapper;
using BasketApii.Entities;
using BasketApii.Repositories.interfaces;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BasketApii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IBasketRepository _repository;
        private readonly EventBusRabbitMQProducer _eventBus;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, EventBusRabbitMQProducer eventBus, IMapper mapper)
        {
            _repository = repository;
            _eventBus = eventBus;
            _mapper = mapper;
        }

        [HttpGet("{userName}",Name ="Get Basket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket= await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));

        }


        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<ShoppingCart>> updateBasket([FromBody] ShoppingCart shoppingCart)
        {
            var basket =await _repository.UpdateBasket(shoppingCart);
            return Ok(basket);

        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if(basket ==null)
            {
                return BadRequest();
            }
            await _repository.DeleteBasket(basketCheckout.UserName);
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;
            try
            {
                _eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch(Exception)
            {
                throw;
            }
            return Accepted();
        }

    }
}
