using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entities;
using OrderApi.Application.Interface;
using E_CommerceSharedLibrary.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OrderApi.Infrastructure.Data;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository : IOrderInterface
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order entity)
        {
            try
            {
                var added = await _context.Orders.AddAsync(entity);
                await _context.SaveChangesAsync();
                return added.Entity;
            }
            catch (Exception ex)
            {
                LogException.logException(ex);
                throw new Exception("Error occurred while placing order");
            }
        }

        public async Task<bool> DeleteAsync(Order entity)
        {
            try
            {
                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                LogException.logException(ex);
                return false;
            }
        }

        public async Task<Order> UpdateAsync(Order entity)
        {
            try
            {
                _context.Orders.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                LogException.logException(ex);
                throw new Exception("Error occurred while updating order");
            }
        }

        public async Task<Order?> FindByIdAsync(int id)
        {
            try
            {
                return await _context.Orders.FindAsync(id);
            }
            catch (Exception ex)
            {
                LogException.logException(ex);
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                return await _context.Orders.ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.logException(ex);
                return new List<Order>();
            }
        }

        public async Task<Order?> GetByAsync(Expression<Func<Order, bool>> expression)
        {
            try
            {
                return await _context.Orders.FirstOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                LogException.logException(ex);
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByAsync(Func<Order, bool> predicate)
        {
            try
            {
                return await Task.Run(() => _context.Orders.Where(predicate).ToList());
            }
            catch (Exception ex)
            {
                LogException.logException(ex);
                return new List<Order>();
            }
        }
    }
}
