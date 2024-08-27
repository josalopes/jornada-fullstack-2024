using Fina.Api.Data;
using Fina.Core.Common;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Models.Enums;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers
{
    public class TransactionHandler(AppDbContext context) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            {
                request.Amount *= -1;
            }

            var transaction = new Transaction
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            try
            {
                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso");
            }
            catch (Exception)
            {
                return new Response<Transaction?>(null, 500, "Não foi possível criar uma transação");
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await context
                    .Transactions
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                {
                    return new Response<Transaction?>(null, 404, "Transação não encontrada");
                }

                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 200, "Transação excluída com sucesso");
            }
            catch (Exception ex)
            {
                return new Response<Transaction?>(null, 500, "Não foi possível excluir a transação");
            }
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await context
                    .Transactions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return transaction is null
                    ? new Response<Transaction?>(null, 404, "Transação não encontrada")
                    : new Response<Transaction?>(transaction);
            }
            catch (Exception)
            {
                return new Response<Transaction?>(null, 500, "Não foi possível localizar a transação");
            }
        }

        public async Task<PagedResponse<List<Transaction>>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDay();
                request.EndDate ??= DateTime.Now.GetLastDay();

                var query = context
                .Transactions
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId
                && x.PaidOrReceivedAt >= request.StartDate
                && x.PaidOrReceivedAt <= request.EndDate)
                .OrderBy(x => x.PaidOrReceivedAt);

                var transactions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query.CountAsync();

                return new PagedResponse<List<Transaction>?>(
                    transactions,
                    count,
                    request.PageNumber,
                    request.PageSize);

            }
            catch (Exception)
            {
                return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível determinar a data de início da transação");
                
            }
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            {
                request.Amount *= -1;
            }

            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
            {
                return new Response<Transaction?>(null, 404, "Transação não encontrada");
            }

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.PaidOrReceivedAt = request.PaidOrReceived;
            transaction.Title = request.Title;
            transaction.Type = request.Type;

            try
            {
                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 201, "Transação atualizada com sucesso.");
            }
            catch (Exception)
            {

                return new Response<Transaction?>(null, 500, "Não foi possível atualizar a transação.");
            }
        }
    }
}
