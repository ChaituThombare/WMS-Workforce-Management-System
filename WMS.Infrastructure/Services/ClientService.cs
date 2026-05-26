using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Client;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditService _auditService;

        public ClientService(ApplicationDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<List<ClientResponseDto>> GetAllAsync()
        {
            return await _context.Clients
                .Select(c => new ClientResponseDto
                {
                    ClientId = c.ClientId,
                    ClientName = c.ClientName,
                    ClientAddress = c.ClientAddress,
                    ClientPhoneNumber = c.ClientPhoneNumber,
                    ClientLocation = c.ClientLocation,
                    Status = c.Status
                }).ToListAsync();
        }

        public async Task<ClientResponseDto?> GetByIdAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return null;

            return new ClientResponseDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ClientAddress = client.ClientAddress,
                ClientPhoneNumber = client.ClientPhoneNumber,
                ClientLocation = client.ClientLocation,
                Status = client.Status
            };
        }

        public async Task<ClientResponseDto> CreateAsync(ClientCreateDto dto)
        {
            var client = new Client
            {
                ClientName = dto.ClientName,
                ClientAddress = dto.ClientAddress,
                ClientPhoneNumber = dto.ClientPhoneNumber,
                ClientLocation = dto.ClientLocation,
                Status = dto.Status
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Clients",
                client.ClientId,
                "Insert",
                1
            );

            var created = await GetByIdAsync(client.ClientId);
            return created!;
        }

        public async Task<bool> UpdateAsync(int id, ClientCreateDto dto)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return false;

            client.ClientName = dto.ClientName;
            client.ClientAddress = dto.ClientAddress;
            client.ClientPhoneNumber = dto.ClientPhoneNumber;
            client.ClientLocation = dto.ClientLocation;
            client.Status = dto.Status;

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Clients",
                client.ClientId,
                "Update",
                1
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return false;

            _context.Clients.Remove(client);

            await _context.SaveChangesAsync();
            await _auditService.LogAsync(
                "Clients",
                client.ClientId,
                "Delete",
                1
            );
            return true;
        }
    }
}
