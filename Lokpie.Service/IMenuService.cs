using Lokpie.Common.Commands;
using Lokpie.Common.Dtos;
using Lokpie.Common.Queries;
using QSI.Common.Web;
using System.Collections.Generic;

namespace Lokpie.Service
{
    public interface IMenuService
    {
        #region FoodNBev
        long AddNewFoodNBev(AddFoodNBevCommand command);
        long UpdateFoodNBev(UpdateFoodNBevCommand command);
        DataTable<FoodNBevDto> GetAllFoodNBev(FoodNBevQuery query, int pageStartRow, int pageSize);
        BasePhotoDto GetFoodNBevPhoto(long menuId);
        FoodNBevDetailDto GetFoodNBevDetail(long menuId);
        void DeleteFoodNBev(long id);
        #endregion
    }
}
