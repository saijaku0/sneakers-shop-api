using MediatR;
using Sneakers.Shop.Backend.Domain.Common;
using Sneakers.Shop.Backend.Domain.Repositories;
using Sneakers.Shop.Backend.Domain.Entities;

namespace Sneakers.Shop.Backend.Application.Orders.Command.CreateOrder
{
    public class CreateOrderHandler(
        IReservationRepository reservationRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrderCommand, Result<Guid>>
    {
        private readonly IReservationRepository _reservationRepository = reservationRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            var reservations = await _reservationRepository.GetActiveByUserIdAsync(request.UserId, ct);
            if (!reservations.Any())
                return Result<Guid>.Failure(Error.Conflict("Cart is empty."));

            var order = new Order(request.UserId, request.ShippingAddress);

            foreach (var reservation in reservations)
            {
                var warehouseItem = reservation.WarehouseItem;
                var product = warehouseItem.Product;

                order.AddOrderItem(
                    warehouseItemId: warehouseItem.Id,
                    quantity: reservation.Quantity,
                    unitPrice: product.BasePrice,
                    discountAmount: 0
                );

                reservation.Confirm();
                warehouseItem.DecreaseQuantity(reservation.Quantity);
            }
            await _orderRepository.AddAsync(order, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<Guid>.Success(order.Id);
        }
    }
}
