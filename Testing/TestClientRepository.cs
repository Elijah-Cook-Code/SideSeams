using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SideSeams.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideSeams.Testing
{
    public static class TestClientRepository
    {
        public static async Task RunTests(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            Console.WriteLine("🔄 Running ClientRepository Tests...");

            // 1️⃣ Add a Client
            var newClient = new ClientInfo
            {
                Name = "Test Client",
                Notes = "Testing repository pattern",
                Date = DateTime.UtcNow,
                Measurements = new List<ClientMeasurements>
                {
                    new ClientMeasurements { A_ChestMeasurement = 40, C_WaistMeasurement = 32 }
                }
            };
            await repository.AddClientAsync(newClient);
            Console.WriteLine("✅ Client Added");

            // 2️⃣ Get All Clients
            var clients = await repository.GetClientsAsync();
            Console.WriteLine($"📦 Found {clients.Count} clients.");

            // 3️⃣ Update a Client
            if (clients.Any())
            {
                var firstClient = clients.First();
                firstClient.Name = "Updated Client Name";
                await repository.UpdateClientAsync(firstClient);
                Console.WriteLine("✅ Client Updated");
            }

            // 4️⃣ Delete a Client
            if (clients.Any())
            {
                await repository.DeleteClientAsync(clients.First().Id);
                Console.WriteLine("✅ Client Deleted");
            }

            Console.WriteLine("✅ All Tests Completed!");
        }
    }
}