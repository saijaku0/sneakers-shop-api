using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sneakers.Shop.Backend.Domain.Repositories;

namespace Sneakers.Shop.Backend.Infrastructure.BackgroundJobs
{
    public class ExpireReservationsJob(
        IServiceScopeFactory scopeFactory,
        ILogger<ExpireReservationsJob> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<ExpireReservationsJob> _logger = logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(120);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExpireReservationsJob started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var reservationService = scope.ServiceProvider.GetRequiredService<IReservationRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var expiredReservations = await reservationService.GetExpiredAsync(stoppingToken);
                    foreach (var reservation in expiredReservations)
                    {
                        reservation.Expire();
                        reservationService.Update(reservation);
                    }

                    if (expiredReservations.Any())
                    {
                        await unitOfWork.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Expired {Count} reservations.", expiredReservations.Count);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while expiring reservations.");
                }
                // Run every 2 hours
                await Task.Delay(_interval, stoppingToken);
            }
            _logger.LogInformation("ExpireReservationsJob stopped.");
        }
    }
}
