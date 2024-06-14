using Lokpie.Repository.Models;
using QSI.Persistence.Query;
using QSI.Persistence.Repository;
using System.Collections.Generic;

namespace Lokpie.Repository
{
    public interface IFoodNBevDao : BaseDao<FoodNBev, long>
    {
        IList<FoodNBev> GetFoodNBevPaging(ICondition condition, int pageStartRow, int pageSize);
        IList<FoodNBev> GetFoodNBev(ICondition condition);
    }
}