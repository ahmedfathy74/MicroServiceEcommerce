using BasketApii.Entities;
using System.Threading.Tasks;

namespace BasketApii.Repositories.interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
       // Task<bool> DeleteBaskett(string userName);

    }
}
