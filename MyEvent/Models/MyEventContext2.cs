using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MyEvent.Models
{
    public partial class MyEventContext : DbContext
    {
        public async Task<int> ExecSPAddNewOrderAsync(int qtyTotal, string eventID, string memberID, decimal totalPrice, string orderDetails)
        {

            // 執行 Stored Procedure
            var result = this.Database.ExecuteSqlRawAsync(
                "EXEC AddNewOrder @p0, @p1, @p2, @p3, @p4",
                qtyTotal, eventID, memberID, totalPrice, orderDetails
            );

            return await result;
        }

    }
}
